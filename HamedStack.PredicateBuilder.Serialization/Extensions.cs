// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable CommentTypo
// ReSharper disable NotResolvedInText
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

using System.Linq.Expressions;
using System.Text.Json;
using Remote.Linq;
using Remote.Linq.Text.Json;

namespace HamedStack.PredicateBuilder.Serialization;

/// <summary>
/// A static class containing extension methods for serializing and deserializing expressions.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Serializes the given expression to a JSON string.
    /// </summary>
    /// <param name="expression">The expression to serialize.</param>
    /// <returns>A JSON string representing the serialized expression.</returns>
    public static string SerializeFromExpression(this Expression expression)
    {
        var remoteExpression = expression.ToRemoteLinqExpression();
        var serializerOptions = new JsonSerializerOptions().ConfigureRemoteLinq();
        return JsonSerializer.Serialize(remoteExpression, serializerOptions);
    }

    /// <summary>
    /// Deserializes a JSON string to an expression.
    /// </summary>
    /// <param name="expressionJson">The JSON string representing the serialized expression.</param>
    /// <returns>The deserialized expression, or <c>null</c> if deserialization fails.</returns>
    public static Expression? DeserializeToExpression(this string expressionJson)
    {
        var serializerOptions = new JsonSerializerOptions().ConfigureRemoteLinq();
        var deserializedExpr = JsonSerializer.Deserialize<Remote.Linq.Expressions.LambdaExpression>(expressionJson, serializerOptions);
        return deserializedExpr?.ToLinqExpression();
    }

    /// <summary>
    /// Deserializes a JSON string to a strongly typed expression.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the expression.</typeparam>
    /// <typeparam name="TU">The return type of the expression.</typeparam>
    /// <param name="expressionJson">The JSON string representing the serialized expression.</param>
    /// <returns>The deserialized expression as <see cref="Expression{TDelegate}"/>, or <c>null</c> if deserialization fails.</returns>
    public static Expression<Func<T, TU>>? DeserializeToExpression<T, TU>(this string expressionJson)
    {
        var serializerOptions = new JsonSerializerOptions().ConfigureRemoteLinq();
        var deserializedExpr = JsonSerializer.Deserialize<Remote.Linq.Expressions.LambdaExpression>(expressionJson, serializerOptions);
        return deserializedExpr?.ToLinqExpression<T, TU>();
    }
}

