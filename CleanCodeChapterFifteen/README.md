# Clean Code - Chapter 15: JUnit Internals

This solution contains exercise code from Chapter 15 ("JUnit Internals") of Robert C. Martin's *Clean Code* book, converted from Java to C#.

## Overview

Chapter 15 examines the `ComparisonCompactor` class from JUnit, which produces compact failure messages when comparing strings in assertions. The code is intentionally in its **pre-refactored state** to demonstrate refactoring techniques discussed in the book.

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

## Refactoring Opportunities

The code in its current state exhibits several code smells that Chapter 15 addresses:

1. **Cryptic variable names** - Fields like `fPrefix`, `fSuffix` use Hungarian notation
2. **Temporal coupling** - `FindCommonPrefix()` must be called before `FindCommonSuffix()`
3. **Hidden side effects** - Methods modify instance state rather than returning values
4. **Complex expressions** - Some substring calculations are hard to follow
5. **Inconsistent abstraction levels** - Mixed levels of detail in methods

## Purpose

Use this code as a starting point to practice the refactoring techniques from Chapter 15, applying clean code principles to improve readability and maintainability while keeping all tests passing.

## Reference

Martin, Robert C. *Clean Code: A Handbook of Agile Software Craftsmanship*. Prentice Hall, 2008. Chapter 15: JUnit Internals.
