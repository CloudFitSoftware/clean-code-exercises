namespace ComparisonCompactor
{
    /// <summary>
    /// Produces compact, readable difference messages when comparing two strings.
    ///
    /// REFACTORING SUMMARY (Clean Code by Robert C. Martin):
    ///
    /// This class has been refactored following principles from multiple chapters:
    /// - Chapter 2: Meaningful Names
    /// - Chapter 3: Functions
    /// - Chapter 15: JUnit Internals (the chapter specifically about this class)
    /// - Chapter 17: Smells and Heuristics
    ///
    /// The original code had several issues that are addressed below with inline comments
    /// explaining each refactoring decision and its origin in the book.
    /// </summary>
    public class ComparisonCompactor
    {
        // REFACTORING #1: Follow C# Naming Conventions (Chapter 2, "Meaningful Names")
        // Original: ELLIPSIS, DELTA_END, DELTA_START used Java's SCREAMING_SNAKE_CASE.
        // Changed to PascalCase to follow C# conventions for constants.
        // The names themselves (Ellipsis, DeltaStart, DeltaEnd) are already descriptive
        // and intention-revealing.
        private const string Ellipsis = "...";
        private const string DeltaEnd = "]";
        private const string DeltaStart = "[";

        // REFACTORING #2: Remove Hungarian Notation (Chapter 2, "Avoid Encodings")
        // Original: fContextLength, fExpected, fActual, fPrefix, fSuffix
        // The 'f' prefix (field notation) is a form of Hungarian notation that
        // "merely adds clutter" (p.24). Modern IDEs make such prefixes unnecessary.
        //
        // REFACTORING #3: Intention-Revealing Names (Chapter 2, "Use Intention-Revealing Names")
        // Renamed fPrefix/fSuffix to prefixLength/suffixLength - the new names
        // describe what the values represent (a length), not just that they relate
        // to prefix/suffix.
        //
        // REFACTORING #4: Immutable Fields (Chapter 15, reducing mutable state)
        // Added 'readonly' to fields that never change after construction
        // (contextLength, expected, actual) to make immutability explicit and
        // prevent accidental modification.
        private readonly int contextLength;
        private readonly string expected;
        private readonly string actual;
        private int prefixLength;
        private int suffixLength;

        public ComparisonCompactor(int contextLength, string expected, string actual)
        {
            this.contextLength = contextLength;
            this.expected = expected;
            this.actual = actual;
        }

        public string Compact(string message)
        {
            // REFACTORING #5: Guard Clause (Chapter 15)
            // Early return for cases where compaction cannot or should not occur.
            // This "flattens" the code and makes the happy path clear.
            if (!CanBeCompacted())
            {
                return FormatUncompactedMessage(message);
            }

            // REFACTORING #6: Eliminate Hidden Temporal Coupling (Chapter 17, "G31")
            // See #12 for how this is implemented.
            FindCommonPrefixAndSuffix();

            // REFACTORING #7: Explanatory Variables (Chapter 2, "Use Intention-Revealing Names")
            // Using well-named local variables makes the code self-documenting.
            // "The name of a variable...should answer why it exists, what it does,
            // and how it is used" (p.18).
            string compactedExpected = CompactString(expected);
            string compactedActual = CompactString(actual);

            return FormatCompactedMessage(message, compactedExpected, compactedActual);
        }

        // REFACTORING #8: Encapsulate Conditionals (Chapter 17, "G28")
        // Original: if (fExpected == null || fActual == null || AreStringsEqual())
        // Complex conditionals should be extracted into well-named methods that
        // explain the intent. "Boolean logic is hard enough to understand without
        // having to see it in the context of an if or while statement."
        //
        // REFACTORING #9: Positive Conditional Name (Chapter 17, "G29: Avoid Negative Conditionals")
        // Original: CannotBeCompacted() was a negative name.
        // "Negatives are just a bit harder to understand than positives. So, when
        // possible, conditionals should be expressed as positives."
        // CanBeCompacted() is positive - it returns true when compaction is possible.
        private bool CanBeCompacted()
        {
            return expected != null && actual != null && !AreStringsEqual();
        }

        // REFACTORING #10: Stepdown Rule (Chapter 3, "Reading Code from Top to Bottom")
        // "We want every function to be followed by those at the next level of
        // abstraction so that we can read the program, descending one level of
        // abstraction at a time as we read down the list of functions."
        // AreStringsEqual() is placed right after CanBeCompacted() which calls it.
        private bool AreStringsEqual()
        {
            return expected.Equals(actual);
        }

        // REFACTORING #11: Single Level of Abstraction (Chapter 3, "One Level of Abstraction per Function")
        // These methods operate at a consistent level of abstraction - they deal with
        // the concept of "formatting" not the details of string concatenation.
        // The Compact() method now reads like a sequence of high-level steps.
        private string FormatUncompactedMessage(string message)
        {
            return Assert.Format(message, expected, actual);
        }

        private string FormatCompactedMessage(string message, string compactedExpected, string compactedActual)
        {
            return Assert.Format(message, compactedExpected, compactedActual);
        }

        // REFACTORING #12: Cohesive Method to Enforce Temporal Coupling (Chapter 15 & Chapter 17, "G31")
        // Original code required FindCommonPrefix() to be called before FindCommonSuffix()
        // because suffix calculation depended on prefix. This hidden dependency was
        // "temporal coupling" - methods had to be called in a specific order, but
        // nothing in the code enforced or documented this requirement.
        // Solution: Combine them into one method that handles both calculations,
        // making the dependency explicit and impossible to violate.
        private void FindCommonPrefixAndSuffix()
        {
            FindCommonPrefix();
            // REFACTORING #13: Express Temporal Coupling Through Arguments (Chapter 15)
            // Passing prefixLength explicitly to FindCommonSuffix() makes the dependency
            // visible in the function signature. Anyone reading this code can immediately
            // see that suffix calculation depends on the prefix result.
            suffixLength = FindCommonSuffix(prefixLength);
        }

        // REFACTORING #14: Small Functions (Chapter 3, "Small!")
        // "The first rule of functions is that they should be small.
        // The second rule of functions is that they should be smaller than that."
        // This method does one thing: finds where the strings start to differ from the beginning.
        private void FindCommonPrefix()
        {
            prefixLength = 0;
            // REFACTORING #15: Encapsulate Boundary Conditions (Chapter 17, "G33")
            // "Boundary conditions are hard to keep track of. Put the processing for them in one place."
            // The shorter string length determines how far we can safely compare.
            int shorterStringLength = Math.Min(expected.Length, actual.Length);

            while (prefixLength < shorterStringLength && expected[prefixLength] == actual[prefixLength])
            {
                prefixLength++;
            }
        }

        // REFACTORING #16: Symmetry (Chapter 15) - Find Methods
        // FindCommonPrefix() and FindCommonSuffix() are now symmetric in structure.
        // Both iterate through characters comparing expected vs actual.
        // The parameter makes the dependency on prefix explicit (see #13).
        private int FindCommonSuffix(int prefixLength)
        {
            int suffixLength = 0;
            // REFACTORING #15: Encapsulate Boundary Conditions (Chapter 17, "G33")
            // Starting positions for reverse iteration - last character index of each string.
            int expectedIndex = expected.Length - 1;
            int actualIndex = actual.Length - 1;

            while (actualIndex >= prefixLength &&
                   expectedIndex >= prefixLength &&
                   expected[expectedIndex] == actual[actualIndex])
            {
                suffixLength++;
                expectedIndex--;
                actualIndex--;
            }

            return suffixLength;
        }

        // REFACTORING #17: Compose Method (Chapter 3, "Do One Thing")
        // Break a method into steps at a consistent level of abstraction.
        // This method now reads almost like English: "build prefix + delta + suffix"
        // Each step is delegated to a well-named method.
        private string CompactString(string source)
        {
            return BuildPrefix() +
                   BuildDelta(source) +
                   BuildSuffix();
        }

        // REFACTORING #18: Extract Method to Hide Complexity (Chapter 3)
        // Original had complex inline expressions with ternary operators and substring math.
        // Extracting to a method with a clear name hides the complexity and explains intent:
        // "Build the prefix portion, adding ellipsis if we're truncating"
        private string BuildPrefix()
        {
            // REFACTORING #15: Encapsulate Boundary Conditions (Chapter 17, "G33")
            // These variables explain the boundary logic:
            // - We show at most 'contextLength' characters before the difference
            // - If prefix is longer than context, we truncate and add ellipsis
            int contextStart = Math.Max(0, prefixLength - contextLength);
            int contextEnd = prefixLength;
            bool shouldTruncate = prefixLength > contextLength;

            string contextPrefix = expected.Substring(contextStart, contextEnd - contextStart);

            if (shouldTruncate)
            {
                return Ellipsis + contextPrefix;
            }

            return contextPrefix;
        }

        // REFACTORING #19: Intention-Revealing Method Name (Chapter 2)
        // "Delta" clearly communicates this is the differing portion between expected and actual.
        // The method name explains what this string segment represents - the part that changed.
        private string BuildDelta(string source)
        {
            // REFACTORING #15: Encapsulate Boundary Conditions (Chapter 17, "G33")
            // The delta is the portion between the common prefix and common suffix.
            int deltaStart = prefixLength;
            int deltaEnd = source.Length - suffixLength;
            int deltaLength = deltaEnd - deltaStart;

            return DeltaStart + source.Substring(deltaStart, deltaLength) + DeltaEnd;
        }

        // REFACTORING #20: Symmetry (Chapter 15) - Build Methods
        // BuildPrefix() and BuildSuffix() now have symmetric structure, making the code
        // easier to understand. When you read one, you can predict how the other works.
        // Both: calculate boundaries, extract context substring, conditionally add ellipsis.
        private string BuildSuffix()
        {
            // REFACTORING #15: Encapsulate Boundary Conditions (Chapter 17, "G33")
            // These variables explain the boundary logic:
            // - We show at most 'contextLength' characters after the difference
            // - If suffix is longer than context, we truncate and add ellipsis
            int contextStart = expected.Length - suffixLength;
            int contextEnd = Math.Min(expected.Length, contextStart + contextLength);
            bool shouldTruncate = suffixLength > contextLength;

            string contextSuffix = expected.Substring(contextStart, contextEnd - contextStart);

            if (shouldTruncate)
            {
                return contextSuffix + Ellipsis;
            }

            return contextSuffix;
        }
    }
}
