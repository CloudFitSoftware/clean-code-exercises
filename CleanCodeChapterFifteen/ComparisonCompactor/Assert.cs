namespace ComparisonCompactor
{
    public class Assert
    {
        public static string Format(string message, string expected, string actual)
        {
            string formatted = "";
            if (message != null && message.Length > 0)
                formatted = message + " ";
            string expectedString = expected ?? "null";
            string actualString = actual ?? "null";
            return formatted + "expected:<" + expectedString + "> but was:<" + actualString + ">";
        }
    }
}
