// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable CommentTypo
// ReSharper disable NotResolvedInText
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

using System.Linq.Expressions;

namespace HamedStack.PredicateBuilder;

/// <summary>
/// Provides a fluent API for building complex predicates using logical operators such as AND, OR, NOT, NAND, NOR, XOR, and XNOR.
/// </summary>
/// <typeparam name="T">The type of the object the predicate applies to.</typeparam>
public class PredicateQueryBuilder<T>
{
    private Expression<Func<T, bool>>? _predicate;

    /// <summary>
    /// Adds a new predicate group with an AND operator.
    /// </summary>
    /// <param name="groupAction">The action to define the predicate group.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> AndGroup(Action<PredicateQueryBuilder<T>> groupAction)
    {
        return GroupWithOperator(groupAction, (a, b) => a.And(b));
    }

    /// <summary>
    /// Adds a new negated predicate group with an AND operator.
    /// </summary>
    /// <param name="groupAction">The action to define the predicate group.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> AndGroupNot(Action<PredicateQueryBuilder<T>> groupAction)
    {
        return GroupWithOperator(groupAction, (a, b) => a.And(b.Not()));
    }

    /// <summary>
    /// Adds a new predicate with an AND operator.
    /// </summary>
    /// <param name="predicate">The predicate to add.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> AndWhere(Expression<Func<T, bool>> predicate)
    {
        return Where(predicate);
    }

    /// <summary>
    /// Adds a new negated predicate with an AND operator.
    /// </summary>
    /// <param name="predicate">The predicate to negate and add.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> AndWhereNot(Expression<Func<T, bool>> predicate)
    {
        return AndWhere(predicate.Not());
    }

    /// <summary>
    /// Builds and returns the current predicate expression.
    /// </summary>
    /// <returns>An expression representing the built predicate, or null if no predicates have been added.</returns>
    public Expression<Func<T, bool>>? Build()
    {
        return _predicate;
    }

    /// <summary>
    /// Adds a new predicate group with a NAND operator.
    /// </summary>
    /// <param name="groupAction">The action to define the predicate group.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> NandGroup(Action<PredicateQueryBuilder<T>> groupAction)
    {
        return GroupWithOperator(groupAction, (a, b) => a.Nand(b));
    }

    /// <summary>
    /// Adds a new negated predicate group with a NAND operator.
    /// </summary>
    /// <param name="groupAction">The action to define the predicate group.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> NandGroupNot(Action<PredicateQueryBuilder<T>> groupAction)
    {
        return GroupWithOperator(groupAction, (a, b) => a.Nand(b.Not()));
    }

    /// <summary>
    /// Adds a new predicate with a NAND operator.
    /// </summary>
    /// <param name="predicate">The predicate to add.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> NandWhere(Expression<Func<T, bool>> predicate)
    {
        _predicate = _predicate == null ? predicate.Not() : _predicate.Nand(predicate);
        return this;
    }

    /// <summary>
    /// Adds a new negated predicate with a NAND operator.
    /// </summary>
    /// <param name="predicate">The predicate to negate and add.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> NandWhereNot(Expression<Func<T, bool>> predicate)
    {
        return NandWhere(predicate.Not());
    }

    /// <summary>
    /// Adds a new predicate group with a NOR operator.
    /// </summary>
    /// <param name="groupAction">The action to define the predicate group.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> NorGroup(Action<PredicateQueryBuilder<T>> groupAction)
    {
        return GroupWithOperator(groupAction, (a, b) => a.Nor(b));
    }

    /// <summary>
    /// Adds a new negated predicate group with a NOR operator.
    /// </summary>
    /// <param name="groupAction">The action to define the predicate group.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> NorGroupNot(Action<PredicateQueryBuilder<T>> groupAction)
    {
        return GroupWithOperator(groupAction, (a, b) => a.Nor(b.Not()));
    }

    /// <summary>
    /// Adds a new predicate with a NOR operator.
    /// </summary>
    /// <param name="predicate">The predicate to add.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> NorWhere(Expression<Func<T, bool>> predicate)
    {
        _predicate = _predicate == null ? predicate.Not() : _predicate.Nor(predicate);
        return this;
    }

    /// <summary>
    /// Adds a new negated predicate with a NOR operator.
    /// </summary>
    /// <param name="predicate">The predicate to negate and add.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> NorWhereNot(Expression<Func<T, bool>> predicate)
    {
        return NorWhere(predicate.Not());
    }

    /// <summary>
    /// Negates the current predicate.
    /// </summary>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> Not()
    {
        _predicate = _predicate?.Not();
        return this;
    }

    /// <summary>
    /// Adds a new predicate group with an OR operator.
    /// </summary>
    /// <param name="groupAction">The action to define the predicate group.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> OrGroup(Action<PredicateQueryBuilder<T>> groupAction)
    {
        return GroupWithOperator(groupAction, (a, b) => a.Or(b));
    }

    /// <summary>
    /// Adds a new negated predicate group with an OR operator.
    /// </summary>
    /// <param name="groupAction">The action to define the predicate group.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> OrGroupNot(Action<PredicateQueryBuilder<T>> groupAction)
    {
        return GroupWithOperator(groupAction, (a, b) => a.Or(b.Not()));
    }

    /// <summary>
    /// Adds a new predicate with an OR operator.
    /// </summary>
    /// <param name="predicate">The predicate to add.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> OrWhere(Expression<Func<T, bool>> predicate)
    {
        _predicate = _predicate == null ? predicate : _predicate.Or(predicate);
        return this;
    }

    /// <summary>
    /// Adds a new negated predicate with an OR operator.
    /// </summary>
    /// <param name="predicate">The predicate to negate and add.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> OrWhereNot(Expression<Func<T, bool>> predicate)
    {
        return OrWhere(predicate.Not());
    }

    /// <summary>
    /// Adds a new predicate with an AND operator.
    /// </summary>
    /// <param name="predicate">The predicate to add.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> Where(Expression<Func<T, bool>> predicate)
    {
        _predicate = _predicate == null ? predicate : _predicate.And(predicate);
        return this;
    }

    /// <summary>
    /// Adds a new negated predicate with an AND operator.
    /// </summary>
    /// <param name="predicate">The predicate to negate and add.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> WhereNot(Expression<Func<T, bool>> predicate)
    {
        return Where(predicate.Not());
    }

    /// <summary>
    /// Adds a new predicate group with an XNOR operator.
    /// </summary>
    /// <param name="groupAction">The action to define the predicate group.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> XnorGroup(Action<PredicateQueryBuilder<T>> groupAction)
    {
        return GroupWithOperator(groupAction, (a, b) => a.Xnor(b));
    }

    /// <summary>
    /// Adds a new negated predicate group with an XNOR operator.
    /// </summary>
    /// <param name="groupAction">The action to define the predicate group.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> XnorGroupNot(Action<PredicateQueryBuilder<T>> groupAction)
    {
        return GroupWithOperator(groupAction, (a, b) => a.Xnor(b.Not()));
    }

    /// <summary>
    /// Adds a new predicate with an XNOR operator.
    /// </summary>
    /// <param name="predicate">The predicate to add.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> XnorWhere(Expression<Func<T, bool>> predicate)
    {
        _predicate = _predicate == null ? predicate : _predicate.Xnor(predicate);
        return this;
    }

    /// <summary>
    /// Adds a new negated predicate with an XNOR operator.
    /// </summary>
    /// <param name="predicate">The predicate to negate and add.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> XnorWhereNot(Expression<Func<T, bool>> predicate)
    {
        return XnorWhere(predicate.Not());
    }

    /// <summary>
    /// Adds a new predicate group with an XOR operator.
    /// </summary>
    /// <param name="groupAction">The action to define the predicate group.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> XorGroup(Action<PredicateQueryBuilder<T>> groupAction)
    {
        return GroupWithOperator(groupAction, (a, b) => a.Xor(b));
    }

    /// <summary>
    /// Adds a new negated predicate group with an XOR operator.
    /// </summary>
    /// <param name="groupAction">The action to define the predicate group.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> XorGroupNot(Action<PredicateQueryBuilder<T>> groupAction)
    {
        return GroupWithOperator(groupAction, (a, b) => a.Xor(b.Not()));
    }

    /// <summary>
    /// Adds a new predicate with an XOR operator.
    /// </summary>
    /// <param name="predicate">The predicate to add.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> XorWhere(Expression<Func<T, bool>> predicate)
    {
        _predicate = _predicate == null ? predicate : _predicate.Xor(predicate);
        return this;
    }

    /// <summary>
    /// Adds a new negated predicate with an XOR operator.
    /// </summary>
    /// <param name="predicate">The predicate to negate and add.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    public PredicateQueryBuilder<T> XorWhereNot(Expression<Func<T, bool>> predicate)
    {
        return XorWhere(predicate.Not());
    }

    /// <summary>
    /// Combines the current predicate with a group of predicates using a specified logical operator.
    /// </summary>
    /// <param name="groupAction">The action to define the group of predicates.</param>
    /// <param name="combine">The function to combine the current predicate with the group predicate.</param>
    /// <returns>The current <see cref="PredicateQueryBuilder{T}"/> instance.</returns>
    private PredicateQueryBuilder<T> GroupWithOperator(
        Action<PredicateQueryBuilder<T>> groupAction,
        Func<Expression<Func<T, bool>>, Expression<Func<T, bool>>, Expression<Func<T, bool>>> combine)
    {
        var groupBuilder = new PredicateQueryBuilder<T>();
        groupAction(groupBuilder);
        var groupPredicate = groupBuilder.Build();

        if (groupPredicate != null)
        {
            _predicate = _predicate == null ? groupPredicate : combine(_predicate, groupPredicate);
        }
        return this;
    }
}
