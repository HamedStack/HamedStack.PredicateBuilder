// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable CommentTypo
// ReSharper disable NotResolvedInText
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

using System.Linq.Expressions;

namespace HamedStack.PredicateBuilder;

/// <summary>
/// Provides methods to build and compose predicate expressions.
/// </summary>
public static class PredicateBuilder
{
    /// <summary>
    /// Returns a predicate that always evaluates to true.
    /// </summary>
    /// <typeparam name="T">The type of the parameter in the predicate.</typeparam>
    /// <returns>A predicate that always returns true.</returns>
    public static Expression<Func<T, bool>> True<T>() { return param => true; }

    /// <summary>
    /// Returns a predicate that always evaluates to false.
    /// </summary>
    /// <typeparam name="T">The type of the parameter in the predicate.</typeparam>
    /// <returns>A predicate that always returns false.</returns>
    public static Expression<Func<T, bool>> False<T>() { return param => false; }

    /// <summary>
    /// Creates a predicate from the provided expression.
    /// </summary>
    /// <typeparam name="T">The type of the parameter in the predicate.</typeparam>
    /// <param name="predicate">The expression to use as the predicate.</param>
    /// <returns>The provided predicate expression.</returns>
    public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) { return predicate; }

    /// <summary>
    /// Combines two predicates using a logical AND.
    /// </summary>
    /// <typeparam name="T">The type of the parameter in the predicates.</typeparam>
    /// <param name="first">The first predicate.</param>
    /// <param name="second">The second predicate.</param>
    /// <returns>A new predicate that represents the logical AND of the two predicates.</returns>
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        return first.Compose(second, Expression.AndAlso);
    }

    /// <summary>
    /// Combines two predicates using a logical OR.
    /// </summary>
    /// <typeparam name="T">The type of the parameter in the predicates.</typeparam>
    /// <param name="first">The first predicate.</param>
    /// <param name="second">The second predicate.</param>
    /// <returns>A new predicate that represents the logical OR of the two predicates.</returns>
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        return first.Compose(second, Expression.OrElse);
    }

    /// <summary>
    /// Negates the given predicate.
    /// </summary>
    /// <typeparam name="T">The type of the parameter in the predicate.</typeparam>
    /// <param name="expression">The predicate to negate.</param>
    /// <returns>A new predicate that represents the logical NOT of the given predicate.</returns>
    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
    {
        var negated = Expression.Not(expression.Body);
        return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
    }

    /// <summary>
    /// Combines two predicates using a logical XOR.
    /// </summary>
    /// <typeparam name="T">The type of the parameter in the predicates.</typeparam>
    /// <param name="first">The first predicate.</param>
    /// <param name="second">The second predicate.</param>
    /// <returns>A new predicate that represents the logical XOR of the two predicates.</returns>
    public static Expression<Func<T, bool>> Xor<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        return first.Compose(second, (firstBody, secondBody) =>
            Expression.OrElse(
                Expression.AndAlso(firstBody, Expression.Not(secondBody)),
                Expression.AndAlso(Expression.Not(firstBody), secondBody)
            )
        );
    }

    /// <summary>
    /// Combines two predicates using a logical XNOR.
    /// </summary>
    /// <typeparam name="T">The type of the parameter in the predicates.</typeparam>
    /// <param name="first">The first predicate.</param>
    /// <param name="second">The second predicate.</param>
    /// <returns>A new predicate that represents the logical XNOR of the two predicates.</returns>
    public static Expression<Func<T, bool>> Xnor<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        return first.Compose(second, (firstBody, secondBody) =>
            Expression.OrElse(
                Expression.AndAlso(firstBody, secondBody),
                Expression.AndAlso(Expression.Not(firstBody), Expression.Not(secondBody))
            )
        );
    }

    /// <summary>
    /// Combines two predicates using a logical NAND.
    /// </summary>
    /// <typeparam name="T">The type of the parameter in the predicates.</typeparam>
    /// <param name="first">The first predicate.</param>
    /// <param name="second">The second predicate.</param>
    /// <returns>A new predicate that represents the logical NAND of the two predicates.</returns>
    public static Expression<Func<T, bool>> Nand<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        return first.And(second).Not();
    }

    /// <summary>
    /// Combines two predicates using a logical NOR.
    /// </summary>
    /// <typeparam name="T">The type of the parameter in the predicates.</typeparam>
    /// <param name="first">The first predicate.</param>
    /// <param name="second">The second predicate.</param>
    /// <returns>A new predicate that represents the logical NOR of the two predicates.</returns>
    public static Expression<Func<T, bool>> Nor<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        return first.Or(second).Not();
    }

    /// <summary>
    /// Composes two expressions by merging them using the specified merge function.
    /// </summary>
    /// <typeparam name="T">The type of the parameter in the expressions.</typeparam>
    /// <param name="first">The first expression.</param>
    /// <param name="second">The second expression.</param>
    /// <param name="merge">The function to merge the expressions.</param>
    /// <returns>A new expression that represents the merged expressions.</returns>
    private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
    {
        var map = first.Parameters
            .Select((f, i) => new { f, s = second.Parameters[i] })
            .ToDictionary(p => p.s, p => p.f);
        var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
        return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
    }

    /// <summary>
    /// A helper class to replace parameters in an expression.
    /// </summary>
    private class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
        /// </summary>
        /// <param name="map">A dictionary mapping original parameters to replacement parameters.</param>
        ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression>? map)
        {
            _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        /// Replaces the parameters in the given expression using the specified map.
        /// </summary>
        /// <param name="map">A dictionary mapping original parameters to replacement parameters.</param>
        /// <param name="exp">The expression in which to replace parameters.</param>
        /// <returns>A new expression with parameters replaced.</returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression>? map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        /// <summary>
        /// Visits the <see cref="ParameterExpression"/> in the expression tree.
        /// </summary>
        /// <param name="p">The parameter expression to visit.</param>
        /// <returns>The modified expression, if it was changed; otherwise, returns the original expression.</returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (_map.TryGetValue(p, out var replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }
}
