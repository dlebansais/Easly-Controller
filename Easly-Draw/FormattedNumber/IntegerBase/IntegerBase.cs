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
        /// A valid number must not start with 0, and must not be empty.
        /// </summary>
        /// <param name="text">The number to check.</param>
        /// <returns>True if <paramref name="text"/> is a valid number; Otherwise, false.</returns>
        bool IsValidNumber(string text);

        /// <summary>
        /// Returns the digit corresponding to a value.
        /// </summary>
        /// <param name="value">The value.</param>
        char ToChar(int value);

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
        /// <summary>
        /// The suffix used to specify the base, null if none.
        /// </summary>
        public abstract string Suffix { get; }

        /// <summary>
        /// The number of digits in the base.
        /// </summary>
        public abstract int Radix { get; }

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
        /// Returns the digit corresponding to a value.
        /// </summary>
        /// <param name="value">The value.</param>
        public abstract char ToChar(int value);

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
            string Result = string.Empty;
            int Carry = 0;

            for (int i = 0; i < text.Length; i++)
            {
                bool IsValid = IsValidDigit(text[i], out int Value);
                Debug.Assert(IsValid);

                Value += Carry;
                Result += ToChar(Value / 2);
                Carry = Value % 2 != 0 ? Radix : 0;
            }

            hasCarry = Carry != 0;
            return Result;
        }

        /// <summary>
        /// Returns the input number muliplied by two.
        /// </summary>
        /// <param name="text">The number to multiply.</param>
        /// <param name="addCarry">True if a carry should be added.</param>
        public virtual string MultipliedByTwo(string text, bool addCarry)
        {
            string Result = string.Empty;
            int Carry = 0;

            for (int i = 0; i < text.Length; i++)
            {
                bool IsValid = IsValidDigit(text[i], out int Value);
                Debug.Assert(IsValid);

                Value = (Value * 2) + Carry;
                if (Value >= Radix)
                {
                    Value -= Radix;
                    Carry = Radix;
                }
                else
                    Carry = 0;

                Result += ToChar(Value);
            }

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
            Debug.Assert(!string.IsNullOrEmpty(text));

            string Result = "0";

            for (int i = text.Length; i > 0; i--)
            {
                char c = text[i - 1];
                bool IsValid = fromBase.IsValidDigit(c, out int Value);
                Debug.Assert(IsValid);
                Debug.Assert(i > 0 || Value != 0 || text.Length > 1);

                string Added = string.Empty;
                int Carry = Value;

                for (int j = Result.Length; j > 0; j--)
                {
                    toBase.IsValidDigit(Result[j - 1], out int ResultValue);
                    int Sum = ResultValue + Carry;

                    while (Sum >= toBase.Radix)
                    {
                        c = toBase.ToChar(Sum % toBase.Radix);
                        Added = c + Added;

                        Sum -= toBase.Radix;
                        Carry++;
                    }

                    c = toBase.ToChar(Sum);
                    Added = c + Added;
                }

                while (Carry >= toBase.Radix)
                {
                    c = toBase.ToChar(Carry % toBase.Radix);
                    Added = c + Added;
                    Carry -= toBase.Radix;
                }

                Result = Added;
            }

            return Result;
        }

        private static string ConvertToBinary(string text, IIntegerBase fromBase)
        {
            Debug.Assert(!string.IsNullOrEmpty(text));

            string Number = text;
            string Result = string.Empty;

            while (Number != "0")
            {
                Number = fromBase.DividedByTwo(Number, out bool HasCarry);
                Result += HasCarry ? "1" : "0";
            }

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
    }
}
