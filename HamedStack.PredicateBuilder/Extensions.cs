// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable CommentTypo
// ReSharper disable NotResolvedInText
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace HamedStack.PredicateBuilder;

/// <summary>
/// A static class containing extension methods for Expressions.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Checks if a specified property/field is null.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the property/field.</typeparam>
    /// <typeparam name="TProp">The type of the property/field to check for null.</typeparam>
    /// <param name="selector">Expression that selects the property/field to check.</param>
    /// <returns>An expression that represents the condition if the property/field is null.</returns>
    /// <remarks>This method generates an expression that checks if the selected property/field is null.</remarks>
    public static Expression<Func<T, bool>> IsNull<T, TProp>(this Expression<Func<T, TProp>> selector)
    {
        var body = Expression.Equal(selector.Body, Expression.Constant(null, typeof(TProp)));
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a specified property/field is not null.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the property/field.</typeparam>
    /// <typeparam name="TProp">The type of the property/field to check for not null.</typeparam>
    /// <param name="selector">Expression that selects the property/field to check.</param>
    /// <returns>An expression that represents the condition if the property/field is not null.</returns>
    public static Expression<Func<T, bool>> IsNotNull<T, TProp>(this Expression<Func<T, TProp>> selector)
    {
        var body = Expression.NotEqual(selector.Body, Expression.Constant(null, typeof(TProp)));
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a string property/field starts with a specific substring.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the property/field.</typeparam>
    /// <param name="selector">Expression that selects the string property/field to check.</param>
    /// <param name="substring">The substring to check for at the start of the string property/field.</param>
    /// <returns>An expression that represents the condition if the string property/field starts with the specified substring.</returns>
    public static Expression<Func<T, bool>> StartsWith<T>(this Expression<Func<T, string>> selector, string substring)
    {
        var method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) }) ?? throw new ArgumentNullException("typeof(string).GetMethod(\"StartsWith\", new[] { typeof(string) })");
        var body = Expression.Call(selector.Body, method, Expression.Constant(substring));
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a string property/field ends with a specific substring.
    /// </summary>
    public static Expression<Func<T, bool>> EndsWith<T>(this Expression<Func<T, string>> selector, string substring)
    {
        var method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) }) ?? throw new ArgumentNullException("typeof(string).GetMethod(\"EndsWith\", new[] { typeof(string) })");
        var body = Expression.Call(selector.Body, method, Expression.Constant(substring));
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a string property/field contains a specific substring.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the string property/field.</typeparam>
    /// <param name="selector">Expression that selects the string property/field to check.</param>
    /// <param name="substring">The substring to check for within the string property/field.</param>
    /// <returns>An expression that represents the condition if the string property/field contains the specified substring.</returns>
    public static Expression<Func<T, bool>> Contains<T>(this Expression<Func<T, string>> selector, string substring)
    {
        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) }) ?? throw new ArgumentNullException("typeof(string).GetMethod(\"Contains\", new[] { typeof(string) })");
        var body = Expression.Call(selector.Body, method, Expression.Constant(substring));
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a numeric property/field is between two values.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the property/field.</typeparam>
    /// <param name="selector">Expression that selects the numeric property/field to check.</param>
    /// <param name="low">The lower bound of the range.</param>
    /// <param name="high">The upper bound of the range.</param>
    /// <returns>An expression that represents the condition if the numeric property/field is between the specified values.</returns>

    public static Expression<Func<T, bool>> Between<T>(this Expression<Func<T, int>> selector, int low, int high)
    {
        var greaterThanLow = Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(low));
        var lessThanHigh = Expression.LessThanOrEqual(selector.Body, Expression.Constant(high));
        var body = Expression.AndAlso(greaterThanLow, lessThanHigh);
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a collection property/field is empty.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the collection property/field.</typeparam>
    /// <typeparam name="TCollection">The type of the collection property/field.</typeparam>
    /// <param name="selector">Expression that selects the collection property/field to check.</param>
    /// <returns>An expression that represents the condition if the collection property/field is empty.</returns>
    public static Expression<Func<T, bool>> IsEmpty<T, TCollection>(this Expression<Func<T, ICollection<TCollection>>> selector)
    {
        var property = Expression.Property(selector.Body, "Count");
        var zero = Expression.Constant(0);
        var body = Expression.Equal(property, zero);
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a collection property/field is not empty.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the collection property/field.</typeparam>
    /// <typeparam name="TCollection">The type of the collection property/field.</typeparam>
    /// <param name="selector">Expression that selects the collection property/field to check.</param>
    /// <returns>An expression that represents the condition if the collection property/field is not empty.</returns>
    public static Expression<Func<T, bool>> IsNotEmpty<T, TCollection>(this Expression<Func<T, ICollection<TCollection>>> selector)
    {
        var property = Expression.Property(selector.Body, "Count");
        var zero = Expression.Constant(0);
        var body = Expression.NotEqual(property, zero);
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a value is in a given list of values.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the value property/field.</typeparam>
    /// <typeparam name="TValue">The type of the value property/field.</typeparam>
    /// <param name="selector">Expression that selects the value property/field to check.</param>
    /// <param name="values">The list of values to check against.</param>
    /// <returns>An expression that represents the condition if the value property/field is in the list of values.</returns>
    public static Expression<Func<T, bool>> In<T, TValue>(this Expression<Func<T, TValue>> selector, params TValue[] values)
    {
        var containsMethod = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(TValue));

        var valuesExpr = Expression.Constant(values, values.GetType());
        var body = Expression.Call(null, containsMethod, valuesExpr, selector.Body);
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a value is not in a given list of values.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the value property/field.</typeparam>
    /// <typeparam name="TValue">The type of the value property/field.</typeparam>
    /// <param name="selector">Expression that selects the value property/field to check.</param>
    /// <param name="values">The list of values to check against.</param>
    /// <returns>An expression that represents the condition if the value property/field is not in the list of values.</returns>
    public static Expression<Func<T, bool>> NotIn<T, TValue>(this Expression<Func<T, TValue>> selector, params TValue[] values)
    {
        var inExpression = In(selector, values);
        var body = Expression.Not(inExpression.Body);
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a nullable property/field has a value.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the nullable property/field.</typeparam>
    /// <typeparam name="TProp">The type of the nullable property/field.</typeparam>
    /// <param name="selector">Expression that selects the nullable property/field to check.</param>
    /// <returns>An expression that represents the condition if the nullable property/field has a value.</returns>
    public static Expression<Func<T, bool>> HasValue<T, TProp>(this Expression<Func<T, TProp?>> selector) where TProp : struct
    {
        var property = Expression.Property(selector.Body, "HasValue");
        return Expression.Lambda<Func<T, bool>>(property, selector.Parameters);
    }

    /// <summary>
    /// Checks if a nullable property/field does not have a value.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the nullable property/field.</typeparam>
    /// <typeparam name="TProp">The type of the nullable property/field.</typeparam>
    /// <param name="selector">Expression that selects the nullable property/field to check.</param>
    /// <returns>An expression that represents the condition if the nullable property/field does not have a value.</returns>
    public static Expression<Func<T, bool>> DoesNotHaveValue<T, TProp>(this Expression<Func<T, TProp?>> selector) where TProp : struct
    {
        var hasValueExpression = HasValue(selector);
        var body = Expression.Not(hasValueExpression.Body);
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a string property/field has a specific length.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the string property/field.</typeparam>
    /// <param name="selector">Expression that selects the string property/field to check.</param>
    /// <param name="length">The length to compare the string property/field with.</param>
    /// <returns>An expression that represents the condition if the string property/field has the specified length.</returns>
    public static Expression<Func<T, bool>> IsLength<T>(this Expression<Func<T, string>> selector, int length)
    {
        var lengthProperty = Expression.Property(selector.Body, "Length");
        var body = Expression.Equal(lengthProperty, Expression.Constant(length));
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a numeric property/field is greater than a specific value.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the numeric property/field.</typeparam>
    /// <param name="selector">Expression that selects the numeric property/field to check.</param>
    /// <param name="value">The value to compare the numeric property/field with.</param>
    /// <returns>An expression that represents the condition if the numeric property/field is greater than the specified value.</returns>
    public static Expression<Func<T, bool>> IsGreaterThan<T>(this Expression<Func<T, int>> selector, int value)
    {
        var body = Expression.GreaterThan(selector.Body, Expression.Constant(value));
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a numeric property/field is less than a specific value.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the numeric property/field.</typeparam>
    /// <param name="selector">Expression that selects the numeric property/field to check.</param>
    /// <param name="value">The value to compare the numeric property/field with.</param>
    /// <returns>An expression that represents the condition if the numeric property/field is less than the specified value.</returns>
    public static Expression<Func<T, bool>> IsLessThan<T>(this Expression<Func<T, int>> selector, int value)
    {
        var body = Expression.LessThan(selector.Body, Expression.Constant(value));
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a DateTime property/field is before a specific date.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the DateTime property/field.</typeparam>
    /// <param name="selector">Expression that selects the DateTime property/field to check.</param>
    /// <param name="date">The date to compare the DateTime property/field with.</param>
    /// <returns>An expression that represents the condition if the DateTime property/field is before the specified date.</returns>
    public static Expression<Func<T, bool>> IsDateBefore<T>(this Expression<Func<T, DateTime>> selector, DateTime date)
    {
        var body = Expression.LessThan(selector.Body, Expression.Constant(date));
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a DateTime property/field is after a specific date.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the DateTime property/field.</typeparam>
    /// <param name="selector">Expression that selects the DateTime property/field to check.</param>
    /// <param name="date">The date to compare the DateTime property/field with.</param>
    /// <returns>An expression that represents the condition if the DateTime property/field is after the specified date.</returns>
    public static Expression<Func<T, bool>> IsDateAfter<T>(this Expression<Func<T, DateTime>> selector, DateTime date)
    {
        var body = Expression.GreaterThan(selector.Body, Expression.Constant(date));
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if a string property/field matches a regex pattern.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the property/field.</typeparam>
    /// <param name="selector">Expression that selects the string property/field to check.</param>
    /// <param name="pattern">The regex pattern to match against the string property/field.</param>
    /// <returns>An expression that represents the condition if the string property/field matches the regex pattern.</returns>
    public static Expression<Func<T, bool>> MatchesRegex<T>(this Expression<Func<T, string>> selector, string pattern)
    {
        var method = typeof(Regex).GetMethod("IsMatch", new[] { typeof(string), typeof(string) });
        var regexBody = Expression.Call(method ?? throw new InvalidOperationException(), selector.Body, Expression.Constant(pattern));
        return Expression.Lambda<Func<T, bool>>(regexBody, selector.Parameters);
    }

    /// <summary>
    /// Checks if a DateTime property/field is within a specified duration from the current date/time.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the property/field.</typeparam>
    /// <param name="selector">Expression that selects the DateTime property/field to check.</param>
    /// <param name="duration">The duration within which the DateTime property/field should fall.</param>
    /// <returns>An expression that represents the condition if the DateTime property/field is within the specified duration from the current date/time.</returns>
    public static Expression<Func<T, bool>> WithinDuration<T>(this Expression<Func<T, DateTime>> selector, TimeSpan duration)
    {
        var now = DateTime.Now;
        var fromTime = now - duration;
        var toTime = now + duration;

        var greaterThanFrom = Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(fromTime));
        var lessThanTo = Expression.LessThanOrEqual(selector.Body, Expression.Constant(toTime));

        var body = Expression.AndAlso(greaterThanFrom, lessThanTo);
        return Expression.Lambda<Func<T, bool>>(body, selector.Parameters);
    }

    /// <summary>
    /// Checks if two properties/fields of the same object are equal.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the properties/fields.</typeparam>
    /// <typeparam name="TValue">The type of the properties/fields to compare.</typeparam>
    /// <param name="selector1">Expression that selects the first property/field.</param>
    /// <param name="selector2">Expression that selects the second property/field.</param>
    /// <returns>An expression that represents the condition if the properties/fields are equal.</returns>
    /// <remarks>This method generates an expression that checks if the selected properties/fields are equal.</remarks>
    public static Expression<Func<T, bool>> PropertyEquals<T, TValue>(this Expression<Func<T, TValue>> selector1, Expression<Func<T, TValue>> selector2)
    {
        var body = Expression.Equal(selector1.Body, selector2.Body);
        return Expression.Lambda<Func<T, bool>>(body, selector1.Parameters);
    }

    /// <summary>
    /// Checks if two properties/fields of the same object are not equal.
    /// </summary>
    /// <typeparam name="T">The type of the object containing the properties/fields.</typeparam>
    /// <typeparam name="TValue">The type of the properties/fields to compare.</typeparam>
    /// <param name="selector1">Expression that selects the first property/field.</param>
    /// <param name="selector2">Expression that selects the second property/field.</param>
    /// <returns>An expression that represents the condition if the properties/fields are not equal.</returns>
    public static Expression<Func<T, bool>> PropertyNotEquals<T, TValue>(this Expression<Func<T, TValue>> selector1, Expression<Func<T, TValue>> selector2)
    {
        var body = Expression.NotEqual(selector1.Body, selector2.Body);
        return Expression.Lambda<Func<T, bool>>(body, selector1.Parameters);
    }

    /// <summary>
    /// Compiles and invokes the given expression.
    /// </summary>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<TResult>(this Expression<Func<TResult>> expression)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate();
    }

    /// <summary>
    /// Compiles and invokes the given expression with one parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <param name="param1">The first parameter to pass to the compiled delegate.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<T1, TResult>(this Expression<Func<T1, TResult>> expression, T1 param1)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate(param1);
    }

    /// <summary>
    /// Compiles and invokes the given expression with two parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <param name="param1">The first parameter to pass to the compiled delegate.</param>
    /// <param name="param2">The second parameter to pass to the compiled delegate.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> expression, T1 param1, T2 param2)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate(param1, param2);
    }

    /// <summary>
    /// Compiles and invokes the given expression with three parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <param name="param1">The first parameter to pass to the compiled delegate.</param>
    /// <param name="param2">The second parameter to pass to the compiled delegate.</param>
    /// <param name="param3">The third parameter to pass to the compiled delegate.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> expression, T1 param1, T2 param2, T3 param3)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate(param1, param2, param3);
    }

    /// <summary>
    /// Compiles and invokes the given expression with four parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <param name="param1">The first parameter to pass to the compiled delegate.</param>
    /// <param name="param2">The second parameter to pass to the compiled delegate.</param>
    /// <param name="param3">The third parameter to pass to the compiled delegate.</param>
    /// <param name="param4">The fourth parameter to pass to the compiled delegate.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<T1, T2, T3, T4, TResult>(this Expression<Func<T1, T2, T3, T4, TResult>> expression, T1 param1, T2 param2, T3 param3, T4 param4)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate(param1, param2, param3, param4);
    }

    /// <summary>
    /// Compiles and invokes the given expression with five parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <param name="param1">The first parameter to pass to the compiled delegate.</param>
    /// <param name="param2">The second parameter to pass to the compiled delegate.</param>
    /// <param name="param3">The third parameter to pass to the compiled delegate.</param>
    /// <param name="param4">The fourth parameter to pass to the compiled delegate.</param>
    /// <param name="param5">The fifth parameter to pass to the compiled delegate.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<T1, T2, T3, T4, T5, TResult>(this Expression<Func<T1, T2, T3, T4, T5, TResult>> expression, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate(param1, param2, param3, param4, param5);
    }

    /// <summary>
    /// Compiles and invokes the given expression with six parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <param name="param1">The first parameter to pass to the compiled delegate.</param>
    /// <param name="param2">The second parameter to pass to the compiled delegate.</param>
    /// <param name="param3">The third parameter to pass to the compiled delegate.</param>
    /// <param name="param4">The fourth parameter to pass to the compiled delegate.</param>
    /// <param name="param5">The fifth parameter to pass to the compiled delegate.</param>
    /// <param name="param6">The sixth parameter to pass to the compiled delegate.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<T1, T2, T3, T4, T5, T6, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> expression, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate(param1, param2, param3, param4, param5, param6);
    }

    /// <summary>
    /// Compiles and invokes the given expression with seven parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <param name="param1">The first parameter to pass to the compiled delegate.</param>
    /// <param name="param2">The second parameter to pass to the compiled delegate.</param>
    /// <param name="param3">The third parameter to pass to the compiled delegate.</param>
    /// <param name="param4">The fourth parameter to pass to the compiled delegate.</param>
    /// <param name="param5">The fifth parameter to pass to the compiled delegate.</param>
    /// <param name="param6">The sixth parameter to pass to the compiled delegate.</param>
    /// <param name="param7">The seventh parameter to pass to the compiled delegate.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<T1, T2, T3, T4, T5, T6, T7, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> expression, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate(param1, param2, param3, param4, param5, param6, param7);
    }

    /// <summary>
    /// Compiles and invokes the given expression with eight parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <param name="param1">The first parameter to pass to the compiled delegate.</param>
    /// <param name="param2">The second parameter to pass to the compiled delegate.</param>
    /// <param name="param3">The third parameter to pass to the compiled delegate.</param>
    /// <param name="param4">The fourth parameter to pass to the compiled delegate.</param>
    /// <param name="param5">The fifth parameter to pass to the compiled delegate.</param>
    /// <param name="param6">The sixth parameter to pass to the compiled delegate.</param>
    /// <param name="param7">The seventh parameter to pass to the compiled delegate.</param>
    /// <param name="param8">The eighth parameter to pass to the compiled delegate.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate(param1, param2, param3, param4, param5, param6, param7, param8);
    }

    /// <summary>
    /// Compiles and invokes the given expression with nine parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <param name="param1">The first parameter to pass to the compiled delegate.</param>
    /// <param name="param2">The second parameter to pass to the compiled delegate.</param>
    /// <param name="param3">The third parameter to pass to the compiled delegate.</param>
    /// <param name="param4">The fourth parameter to pass to the compiled delegate.</param>
    /// <param name="param5">The fifth parameter to pass to the compiled delegate.</param>
    /// <param name="param6">The sixth parameter to pass to the compiled delegate.</param>
    /// <param name="param7">The seventh parameter to pass to the compiled delegate.</param>
    /// <param name="param8">The eighth parameter to pass to the compiled delegate.</param>
    /// <param name="param9">The ninth parameter to pass to the compiled delegate.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate(param1, param2, param3, param4, param5, param6, param7, param8, param9);
    }

    /// <summary>
    /// Compiles and invokes the given expression with ten parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <param name="param1">The first parameter to pass to the compiled delegate.</param>
    /// <param name="param2">The second parameter to pass to the compiled delegate.</param>
    /// <param name="param3">The third parameter to pass to the compiled delegate.</param>
    /// <param name="param4">The fourth parameter to pass to the compiled delegate.</param>
    /// <param name="param5">The fifth parameter to pass to the compiled delegate.</param>
    /// <param name="param6">The sixth parameter to pass to the compiled delegate.</param>
    /// <param name="param7">The seventh parameter to pass to the compiled delegate.</param>
    /// <param name="param8">The eighth parameter to pass to the compiled delegate.</param>
    /// <param name="param9">The ninth parameter to pass to the compiled delegate.</param>
    /// <param name="param10">The tenth parameter to pass to the compiled delegate.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);
    }

    /// <summary>
    /// Compiles and invokes the given expression with eleven parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <param name="param1">The first parameter to pass to the compiled delegate.</param>
    /// <param name="param2">The second parameter to pass to the compiled delegate.</param>
    /// <param name="param3">The third parameter to pass to the compiled delegate.</param>
    /// <param name="param4">The fourth parameter to pass to the compiled delegate.</param>
    /// <param name="param5">The fifth parameter to pass to the compiled delegate.</param>
    /// <param name="param6">The sixth parameter to pass to the compiled delegate.</param>
    /// <param name="param7">The seventh parameter to pass to the compiled delegate.</param>
    /// <param name="param8">The eighth parameter to pass to the compiled delegate.</param>
    /// <param name="param9">The ninth parameter to pass to the compiled delegate.</param>
    /// <param name="param10">The tenth parameter to pass to the compiled delegate.</param>
    /// <param name="param11">The eleventh parameter to pass to the compiled delegate.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> expression, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11);
    }

    /// <summary>
    /// Compiles and invokes the given expression with twelve parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <param name="param1">The first parameter to pass to the compiled delegate.</param>
    /// <param name="param2">The second parameter to pass to the compiled delegate.</param>
    /// <param name="param3">The third parameter to pass to the compiled delegate.</param>
    /// <param name="param4">The fourth parameter to pass to the compiled delegate.</param>
    /// <param name="param5">The fifth parameter to pass to the compiled delegate.</param>
    /// <param name="param6">The sixth parameter to pass to the compiled delegate.</param>
    /// <param name="param7">The seventh parameter to pass to the compiled delegate.</param>
    /// <param name="param8">The eighth parameter to pass to the compiled delegate.</param>
    /// <param name="param9">The ninth parameter to pass to the compiled delegate.</param>
    /// <param name="param10">The tenth parameter to pass to the compiled delegate.</param>
    /// <param name="param11">The eleventh parameter to pass to the compiled delegate.</param>
    /// <param name="param12">The twelfth parameter to pass to the compiled delegate.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> expression, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11, T12 param12)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12);
    }

    /// <summary>
    /// Compiles and invokes the given expression with thirteen parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <param name="param1">The first parameter to pass to the compiled delegate.</param>
    /// <param name="param2">The second parameter to pass to the compiled delegate.</param>
    /// <param name="param3">The third parameter to pass to the compiled delegate.</param>
    /// <param name="param4">The fourth parameter to pass to the compiled delegate.</param>
    /// <param name="param5">The fifth parameter to pass to the compiled delegate.</param>
    /// <param name="param6">The sixth parameter to pass to the compiled delegate.</param>
    /// <param name="param7">The seventh parameter to pass to the compiled delegate.</param>
    /// <param name="param8">The eighth parameter to pass to the compiled delegate.</param>
    /// <param name="param9">The ninth parameter to pass to the compiled delegate.</param>
    /// <param name="param10">The tenth parameter to pass to the compiled delegate.</param>
    /// <param name="param11">The eleventh parameter to pass to the compiled delegate.</param>
    /// <param name="param12">The twelfth parameter to pass to the compiled delegate.</param>
    /// <param name="param13">The thirteenth parameter to pass to the compiled delegate.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> expression, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11, T12 param12, T13 param13)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13);
    }

    /// <summary>
    /// Compiles and invokes the given expression with fourteen parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <param name="param1">The first parameter to pass to the compiled delegate.</param>
    /// <param name="param2">The second parameter to pass to the compiled delegate.</param>
    /// <param name="param3">The third parameter to pass to the compiled delegate.</param>
    /// <param name="param4">The fourth parameter to pass to the compiled delegate.</param>
    /// <param name="param5">The fifth parameter to pass to the compiled delegate.</param>
    /// <param name="param6">The sixth parameter to pass to the compiled delegate.</param>
    /// <param name="param7">The seventh parameter to pass to the compiled delegate.</param>
    /// <param name="param8">The eighth parameter to pass to the compiled delegate.</param>
    /// <param name="param9">The ninth parameter to pass to the compiled delegate.</param>
    /// <param name="param10">The tenth parameter to pass to the compiled delegate.</param>
    /// <param name="param11">The eleventh parameter to pass to the compiled delegate.</param>
    /// <param name="param12">The twelfth parameter to pass to the compiled delegate.</param>
    /// <param name="param13">The thirteenth parameter to pass to the compiled delegate.</param>
    /// <param name="param14">The fourteenth parameter to pass to the compiled delegate.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> expression, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11, T12 param12, T13 param13, T14 param14)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14);
    }

    /// <summary>
    /// Compiles and invokes the given expression with fifteen parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the expression.</typeparam>
    /// <param name="expression">The expression to compile and invoke.</param>
    /// <param name="param1">The first parameter to pass to the compiled delegate.</param>
    /// <param name="param2">The second parameter to pass to the compiled delegate.</param>
    /// <param name="param3">The third parameter to pass to the compiled delegate.</param>
    /// <param name="param4">The fourth parameter to pass to the compiled delegate.</param>
    /// <param name="param5">The fifth parameter to pass to the compiled delegate.</param>
    /// <param name="param6">The sixth parameter to pass to the compiled delegate.</param>
    /// <param name="param7">The seventh parameter to pass to the compiled delegate.</param>
    /// <param name="param8">The eighth parameter to pass to the compiled delegate.</param>
    /// <param name="param9">The ninth parameter to pass to the compiled delegate.</param>
    /// <param name="param10">The tenth parameter to pass to the compiled delegate.</param>
    /// <param name="param11">The eleventh parameter to pass to the compiled delegate.</param>
    /// <param name="param12">The twelfth parameter to pass to the compiled delegate.</param>
    /// <param name="param13">The thirteenth parameter to pass to the compiled delegate.</param>
    /// <param name="param14">The fourteenth parameter to pass to the compiled delegate.</param>
    /// <param name="param15">The fifteenth parameter to pass to the compiled delegate.</param>
    /// <returns>The result of the compiled and invoked expression.</returns>
    public static TResult CompileAndInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> expression, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11, T12 param12, T13 param13, T14 param14, T15 param15)
    {
        var compiledDelegate = expression.Compile();
        return compiledDelegate(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14, param15);
    }

}
