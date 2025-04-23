# 📘 HamedStack.PredicateBuilder – User Guide

`HamedStack.PredicateBuilder` is a powerful, fluent, and expressive C# library to dynamically build LINQ expressions (predicates) for filtering collections, querying databases, and more.

Perfect for scenarios like:
- Complex search filters
- Dynamic API queries
- Advanced logical expression combinations

---

## 📦 Installation

Just add a reference to the compiled library, or include the source in your project.

---

## 🧠 Core Concepts

- **Predicate**: A function `T => bool` used for filtering.
- **Builder**: Use fluent methods to dynamically build predicates using logical operations.
- **Composition**: Use `And`, `Or`, `Xor`, `Nor`, `Nand`, `Xnor`, and their negations.

---

## 🚀 Quick Start

```csharp
var builder = new PredicateQueryBuilder<Person>();

builder.AndWhere(p => p.Age > 18)
       .OrWhere(p => p.Name.StartsWith("A"));

var predicate = builder.Build();
var result = people.Where(predicate.Compile()).ToList();
```

---

## 🛠️ PredicateQueryBuilder API

### `Where` / `AndWhere`
Add predicate using logical **AND**.

```csharp
builder.Where(p => p.Age > 18);
```

### `OrWhere`
Add using logical **OR**.

```csharp
builder.OrWhere(p => p.Name.StartsWith("A"));
```

### `WhereNot`, `AndWhereNot`, `OrWhereNot`
Add **negated** predicates.

```csharp
builder.AndWhereNot(p => p.IsActive);
```

### Logical Groups (`GroupWithOperator`)
Nest logic inside a group. Useful for grouped conditions:

```csharp
builder.OrGroup(g => {
    g.AndWhere(p => p.Age > 30)
     .AndWhere(p => p.City == "Paris");
});
```

### Negated Groups
```csharp
builder.OrGroupNot(g => {
    g.AndWhere(p => p.IsActive);
});
```

---

## 🔄 Advanced Logical Operators

| Method | Description |
|--------|-------------|
| `And`  | Logical AND |
| `Or`   | Logical OR |
| `Not`  | Logical NOT |
| `Xor`  | Exclusive OR |
| `Xnor` | Exclusive NOR |
| `Nand` | NOT AND |
| `Nor`  | NOT OR |

```csharp
builder.XorWhere(p => p.IsMale);
```

Grouped Example:

```csharp
builder.NorGroup(g => {
    g.AndWhere(p => p.IsStudent)
     .OrWhere(p => p.Grade > 90);
});
```

---

## 🧱 PredicateBuilder API

These are extension methods to directly combine expressions.

```csharp
var adult = PredicateBuilder.Create<Person>(p => p.Age >= 18);
var livesInParis = PredicateBuilder.Create<Person>(p => p.City == "Paris");

var combined = adult.And(livesInParis); // returns Expression<Func<Person, bool>>
```

You can also start with:

```csharp
PredicateBuilder.True<Person>(); // Always true
PredicateBuilder.False<Person>(); // Always false
```

---

## ✅ Full Example

```csharp
var builder = new PredicateQueryBuilder<User>();

builder.AndWhere(u => u.IsActive)
       .OrGroup(g => g.AndWhere(u => u.Role == "Admin")
                      .AndWhere(u => u.LastLogin.WithinDuration(TimeSpan.FromDays(30))))
       .XorWhereNot(u => u.Email.Contains("temp"));

var predicate = builder.Build();
var users = dbContext.Users.Where(predicate.Compile()).ToList();
```

---

## 🧠 Tips

- Always use `Build()` to get the final `Expression<Func<T, bool>>`.
- Use `Compile()` if you want to run it on in-memory collections.
- For EF or LINQ-to-SQL, use the raw expression directly.

---
