# Clean Code - Chapter 15: JUnit Internals

This solution contains exercise code from Chapter 15 ("JUnit Internals") of Robert C. Martin's *Clean Code* book, converted from Java to C#.

## Overview

Chapter 15 examines the `ComparisonCompactor` class from JUnit, which produces compact failure messages when comparing strings in assertions. The code has been **refactored** following the techniques discussed in the book, with detailed inline comments explaining each refactoring decision.

## What Does ComparisonCompactor Do?

The `ComparisonCompactor` class creates readable difference messages when two strings don't match. For example:

- Input: expected `"abc"`, actual `"adc"`
- Output: `"expected:<a[b]c> but was:<a[d]c>"`

It highlights the differences using brackets `[...]` and uses ellipses `...` to truncate context when strings are long.

## Project Structure

```
CleanCodeChapterFifteen/
├── ComparisonCompactor/           # Main library
│   ├── ComparisonCompactor.cs     # The class under study
│   └── Assert.cs                  # Helper for formatting messages
└── ComparisonCompactor.Tests/     # Unit tests
    └── ComparisonCompactorTest.cs # Test cases from the book
```

## Prerequisites

- .NET 10.0 SDK

## Building

```bash
dotnet build
```

## Running Tests

```bash
dotnet test
```

## Refactoring Summary

The code has been refactored following Clean Code principles. Each refactoring is documented with inline comments in `ComparisonCompactor.cs` referencing the specific chapter and concept from the book.

| # | Refactoring | Clean Code Reference |
|---|-------------|---------------------|
| 1 | C# naming conventions for constants (PascalCase) | Chapter 2: Meaningful Names |
| 2 | Remove Hungarian notation (`f` prefix) | Chapter 2: Avoid Encodings |
| 3 | Intention-revealing names (`prefixLength`, `suffixLength`) | Chapter 2: Use Intention-Revealing Names |
| 4 | Immutable fields (`readonly`) | Chapter 15: Reduce mutable state |
| 5 | Guard clause | Chapter 15: Early return, flatten code |
| 6 | Eliminate hidden temporal coupling | Chapter 17: G31 (see #12 for implementation) |
| 7 | Explanatory variables (`compactedExpected`, `compactedActual`) | Chapter 2: Use Intention-Revealing Names |
| 8 | Encapsulate conditionals (`CanBeCompacted()`) | Chapter 17: G28 Encapsulate Conditionals |
| 9 | Positive conditional name (`CanBeCompacted()` not `CannotBeCompacted()`) | Chapter 17: G29 Avoid Negative Conditionals |
| 10 | Stepdown rule - helper methods near callers | Chapter 3: Reading Code from Top to Bottom |
| 11 | Single level of abstraction (`Format*Message()` methods) | Chapter 3: One Level of Abstraction per Function |
| 12 | Cohesive method to enforce temporal coupling (`FindCommonPrefixAndSuffix()`) | Chapter 15 & Chapter 17: G31 |
| 13 | Express temporal coupling through arguments | Chapter 15: Make dependencies explicit |
| 14 | Small functions (`FindCommonPrefix()`) | Chapter 3: Small! |
| 15 | Encapsulate boundary conditions with explanatory variables | Chapter 17: G33 (applied in multiple methods) |
| 16 | Symmetry - Find methods (`FindCommonPrefix()` / `FindCommonSuffix()`) | Chapter 15: Consistency |
| 17 | Compose method (`CompactString()` reads like prose) | Chapter 3: Do One Thing |
| 18 | Extract method to hide complexity (`BuildPrefix()`) | Chapter 3: Hide Complexity |
| 19 | Intention-revealing method name (`BuildDelta()`) | Chapter 2: Intention-Revealing Names |
| 20 | Symmetry - Build methods (`BuildPrefix()` / `BuildSuffix()`) | Chapter 15: Consistency |

### Key Improvements

- **Temporal coupling eliminated**: `FindCommonPrefixAndSuffix()` ensures correct ordering; dependency made explicit via argument passing
- **Positive conditionals**: `CanBeCompacted()` instead of `CannotBeCompacted()` (G29)
- **Symmetric functions**: `FindCommonPrefix()`/`FindCommonSuffix()` and `BuildPrefix()`/`BuildSuffix()` mirror each other
- **Clearer abstraction levels**: `CompactString()` reads as prose: build prefix + delta + suffix
- **Stepdown rule**: Methods ordered so callees appear after callers
- **No Hungarian notation**: Fields named for what they represent, not their type
- **Boundary conditions encapsulated**: Variables like `shouldTruncate`, `contextStart`, `contextEnd` explain boundary logic

### Original Code Smells (Now Fixed)

1. **Cryptic variable names** - Fields like `fPrefix`, `fSuffix` used Hungarian notation
2. **Temporal coupling** - `FindCommonPrefix()` had to be called before `FindCommonSuffix()`
3. **Hidden side effects** - Methods modified instance state rather than returning values
4. **Complex expressions** - Substring calculations were hard to follow
5. **Inconsistent abstraction levels** - Mixed levels of detail in methods

## Reference

Martin, Robert C. *Clean Code: A Handbook of Agile Software Craftsmanship*. Prentice Hall, 2008. Chapter 15: JUnit Internals.
