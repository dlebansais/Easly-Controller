namespace EaslyDraw
{
    /// <summary>
    /// Base interface for a number format.
    /// </summary>
    public interface IFormattedNumber
    {
    }

    /// <summary>
    /// Base class for a number format.
    /// </summary>
    public class FormattedNumber : IFormattedNumber
    {
        /// <summary>
        /// Parses a string to a formatted number.
        /// </summary>
        /// <param name="text">The string to parse.</param>
        public static IFormattedNumber Parse(string text)
        {
            return new InvalidNumber(text);
        }
    }
}
