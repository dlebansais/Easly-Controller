namespace EaslyDraw
{
    /// <summary>
    /// Interface describing an integer digits base.
    /// </summary>
    public interface IIntegerBase
    {
        /// <summary>
        /// The suffix used to specify the base, null if none.
        /// </summary>
        string Suffix { get; }

        /// <summary>
        /// The number of digits in the base.
        /// </summary>
        int Radix { get; }

        /// <summary>
        /// Checks if a character is a digit in this base, and return the corresponding digit value.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <param name="value">The digit value.</param>
        /// <returns>True if <paramref name="c"/> is a valid digit; Otherwise, false.</returns>
        bool IsValidDigit(char c, out int value);
    }

    /// <summary>
    /// Class describing an integer digits base.
    /// </summary>
    public abstract class IntegerBase : IIntegerBase
    {
        /// <summary>
        /// The suffix used to specify the base, null if none.
        /// </summary>
        public abstract string Suffix { get; }

        /// <summary>
        /// The number of digits in the base.
        /// </summary>
        public abstract int Radix { get; }

        /// <summary>
        /// Checks if a character is a digit in this base, and return the corresponding digit value.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <param name="value">The digit value.</param>
        /// <returns>True if <paramref name="c"/> is a valid digit; Otherwise, false.</returns>
        public abstract bool IsValidDigit(char c, out int value);
    }
}
