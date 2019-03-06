namespace EaslyDraw
{
    using System.Diagnostics;

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
        /// Checks if a character is a digit in this base, and return the corresponding value.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <param name="value">The digit's value.</param>
        /// <returns>True if <paramref name="c"/> is a valid digit; Otherwise, false.</returns>
        bool IsValidDigit(char c, out int value);

        /// <summary>
        /// Checks if a number is made of digits in this base.
        /// A valid number must not start with 0 (unless it is zero), and must not be empty.
        /// </summary>
        /// <param name="text">The number to check.</param>
        /// <returns>True if <paramref name="text"/> is a valid number; Otherwise, false.</returns>
        bool IsValidNumber(string text);

        /// <summary>
        /// Checks if a number is made of digits in this base.
        /// A valid significand must be a valid number and not end with a zero (unless it is zero).
        /// </summary>
        /// <param name="text">The number to check.</param>
        /// <returns>True if <paramref name="text"/> is a valid significand; Otherwise, false.</returns>
        bool IsValidSignificand(string text);

        /// <summary>
        /// Returns the digit corresponding to a value.
        /// </summary>
        /// <param name="value">The value.</param>
        char ToDigit(int value);

        /// <summary>
        /// Returns the value corresponding to a digit.
        /// </summary>
        /// <param name="c">The digit.</param>
        int ToValue(char c);

        /// <summary>
        /// Returns the input number divided by two.
        /// </summary>
        /// <param name="text">The number to divide.</param>
        /// <param name="hasCarry">True upon return if <paramref name="text"/> is odd.</param>
        string DividedByTwo(string text, out bool hasCarry);

        /// <summary>
        /// Returns the input number muliplied by two.
        /// </summary>
        /// <param name="text">The number to multiply.</param>
        /// <param name="addCarry">True if a carry should be added.</param>
        string MultipliedByTwo(string text, bool addCarry);
    }

    /// <summary>
    /// Class describing an integer digits base.
    /// </summary>
    public abstract class IntegerBase : IIntegerBase
    {
        #region Constants
        /// <summary>
        /// The zero.
        /// </summary>
        public static readonly string Zero = "0";

        /// <summary>
        /// The hexadecimal base.
        /// </summary>
        public static readonly IIntegerBase Hexadecimal = new HexadecimalIntegerBase();

        /// <summary>
        /// The decimal base.
        /// </summary>
        public static readonly IIntegerBase Decimal = new DecimalIntegerBase();

        /// <summary>
        /// The octal base.
        /// </summary>
        public static readonly IIntegerBase Octal = new OctalIntegerBase();

        /// <summary>
        /// The binary base.
        /// </summary>
        public static readonly IIntegerBase Binary = new BinaryIntegerBase();
        #endregion

        #region Properties
        /// <summary>
        /// The suffix used to specify the base, null if none.
        /// </summary>
        public abstract string Suffix { get; }

        /// <summary>
        /// The number of digits in the base.
        /// </summary>
        public abstract int Radix { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks if a character is a digit in this base, and return the corresponding value.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <param name="value">The digit's value.</param>
        /// <returns>True if <paramref name="c"/> is a valid digit; Otherwise, false.</returns>
        public abstract bool IsValidDigit(char c, out int value);

        /// <summary>
        /// Checks if a number is made of digits in this base.
        /// A valid number must not start with 0, and must not be empty.
        /// </summary>
        /// <param name="text">The number to check.</param>
        /// <returns>True if <paramref name="text"/> is a valid number; Otherwise, false.</returns>
        public virtual bool IsValidNumber(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (!IsValidDigit(c, out int Value))
                    return false;

                if (i == 0 && Value == 0 && text.Length != 1)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if a number is made of digits in this base.
        /// A valid significand must be a valid number and not end with a zero (unless it is zero).
        /// </summary>
        /// <param name="text">The number to check.</param>
        /// <returns>True if <paramref name="text"/> is a valid significand; Otherwise, false.</returns>
        public virtual bool IsValidSignificand(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (!IsValidDigit(c, out int Value))
                    return false;

                if ((i == 0 || i + 1 == text.Length) && Value == 0 && text.Length != 1)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the digit corresponding to a value.
        /// </summary>
        /// <param name="value">The value.</param>
        public abstract char ToDigit(int value);

        /// <summary>
        /// Returns the value corresponding to a digit.
        /// </summary>
        /// <param name="c">The digit.</param>
        public abstract int ToValue(char c);

        /// <summary>
        /// Returns the input number divided by two.
        /// </summary>
        /// <param name="text">The number to divide.</param>
        /// <param name="hasCarry">True upon return if <paramref name="text"/> is odd.</param>
        public virtual string DividedByTwo(string text, out bool hasCarry)
        {
            Debug.Assert(IsValidNumber(text));

            string Result = string.Empty;
            int Carry = 0;

            for (int i = 0; i < text.Length; i++)
            {
                bool IsValid = IsValidDigit(text[i], out int Value);
                Debug.Assert(IsValid);

                Value += Carry;
                char Digit = ToDigit(Value / 2);

                if (Digit != '0' || i > 0 || text.Length == 1)
                    Result += Digit;

                Carry = Value % 2 != 0 ? Radix : 0;
            }

            hasCarry = Carry != 0;

            Debug.Assert(IsValidNumber(Result));
            return Result;
        }

        /// <summary>
        /// Returns the input number muliplied by two.
        /// </summary>
        /// <param name="text">The number to multiply.</param>
        /// <param name="addCarry">True if a carry should be added.</param>
        public virtual string MultipliedByTwo(string text, bool addCarry)
        {
            Debug.Assert(IsValidNumber(text));

            string Result = string.Empty;
            int Carry = addCarry ? 1 : 0;

            for (int i = 0; i < text.Length; i++)
            {
                bool IsValid = IsValidDigit(text[text.Length - 1 - i], out int Value);
                Debug.Assert(IsValid);

                Value = (Value * 2) + Carry;
                if (Value >= Radix)
                {
                    Value -= Radix;
                    Carry = 1;
                }
                else
                    Carry = 0;

                Result = ToDigit(Value) + Result;
            }

            if (Carry > 0)
                Result = ToDigit(Carry) + Result;

            Debug.Assert(IsValidNumber(Result));
            return Result;
        }

        /// <summary>
        /// Returns the value of <paramref name="text"/> converted to a new base.
        /// </summary>
        /// <param name="text">The number to convert.</param>
        /// <param name="fromBase">The base in which <paramref name="text"/> is encoded.</param>
        /// <param name="toBase">The base in which the returned number is encoded.</param>
        public static string Convert(string text, IIntegerBase fromBase, IIntegerBase toBase)
        {
            return ConvertFromBinary(ConvertToBinary(text, fromBase), toBase);
        }
        #endregion

        #region Implementation
        private static string ConvertToBinary(string text, IIntegerBase fromBase)
        {
            Debug.Assert(!string.IsNullOrEmpty(text));

            string Number = text;
            string Result = string.Empty;

            do
            {
                Number = fromBase.DividedByTwo(Number, out bool HasCarry);
                Result = (HasCarry ? "1" : "0") + Result;
            }
            while (Number != "0");

            return Result;
        }

        private static string ConvertFromBinary(string text, IIntegerBase toBase)
        {
            Debug.Assert(!string.IsNullOrEmpty(text));

            string Result = "0";

            for (int i = 0; i < text.Length; i++)
            {
                bool AddCarry = text[i] != '0';
                Result = toBase.MultipliedByTwo(Result, AddCarry);
            }

            return Result;
        }
        #endregion
    }
}
