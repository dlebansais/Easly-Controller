namespace EaslyDraw
{
    using System.Diagnostics;

    /// <summary>
    /// Interface to manipulate integer or real numbers of any size.
    /// </summary>
    public interface ICanonicalNumber
    {
        void MultiplyAndAdd(int Multiply, int Digit);
        bool IsFractionalNegative { get; }
        string FractionalPart { get; }
        bool IsExponentNegative { get; }
        string ExponentPart { get; }
        bool IsEqual(ICanonicalNumber Other);
        bool IsLesser(ICanonicalNumber Other);
        bool IsGreater(ICanonicalNumber Other);
        ICanonicalNumber OppositeOf();
        bool TryParseInt(out int Value);
    }

    /// <summary>
    /// Interface to manipulate integer or real numbers of any size.
    /// </summary>
    public class CanonicalNumber : ICanonicalNumber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CanonicalNumber"/> class.
        /// </summary>
        /// <param name="FractionalPart"></param>
        /// <param name="IsExponentNegative"></param>
        /// <param name="ExponentPart"></param>
        public CanonicalNumber(string FractionalPart, bool IsExponentNegative, string ExponentPart)
        {
            this.IsFractionalNegative = false;
            this.FractionalPart = FractionalPart;
            this.IsExponentNegative = IsExponentNegative;
            this.ExponentPart = ExponentPart;

            Debug.Assert(IsNotationValid);
            FormatCanonicString();
        }

        public CanonicalNumber(int n)
        {
            int InputExponent = 0;
            int Intvalue = 10;
            while (n >= Intvalue)
            {
                InputExponent++;
                Intvalue = Intvalue * 10;
            }

            string FractionalPart = n.ToString();
            while (FractionalPart.Length > 1 && FractionalPart[FractionalPart.Length - 1] == '0')
                FractionalPart = FractionalPart.Substring(0, FractionalPart.Length - 1);

            this.IsFractionalNegative = false;
            this.FractionalPart = FractionalPart;
            this.IsExponentNegative = false;
            this.ExponentPart = InputExponent.ToString();

            Debug.Assert(IsNotationValid);
            FormatCanonicString();
        }

        public override string ToString()
        {
            return CanonicRepresentation;
        }

        public bool IsFractionalNegative { get; private set; }
        public string FractionalPart { get; private set; }
        public bool IsExponentNegative { get; private set; }
        public string ExponentPart { get; private set; }
        public string CanonicRepresentation { get; private set; }

        public bool IsNotationValid
        {
            get
            {
                // If using more than one digit, the fractional part must not start or end with a zero
                if (FractionalPart.Length > 1 && (FractionalPart[0] == '0' || FractionalPart[FractionalPart.Length - 1] == '0'))
                    return false;

                // If using more than one digit, the exponent part must not start with a zero
                if (ExponentPart.Length > 1 && ExponentPart[0] == '0')
                    return false;

                // If zero, it must be 0e+0
                if (FractionalPart == "0" && (IsExponentNegative || ExponentPart != "0"))
                    return false;

                return true;
            }
        }

        private void FormatCanonicString()
        {
            if (FractionalPart == "0" && ExponentPart == "0")
                CanonicRepresentation = "0";

            else
            {
                if (FractionalPart.Length == 1)
                    CanonicRepresentation = FractionalPart[0] + ".0e" + (IsExponentNegative ? "-" : "+") + ExponentPart;

                else
                    CanonicRepresentation = FractionalPart[0] + "." + FractionalPart.Substring(1) + "e" + (IsExponentNegative ? "-" : "+") + ExponentPart;

                if (IsFractionalNegative)
                    CanonicRepresentation = '-' + CanonicRepresentation;
            }
        }

        public void MultiplyAndAdd(int Multiplicand, int Digit)
        {
            Debug.Assert(!IsFractionalNegative);
            Debug.Assert(!IsExponentNegative);

            int Carry = 0;
            string FractionalResult = "";

            for (int i = FractionalPart.Length; i > 0; i--)
            {
                int NumberDigit = FractionalPart[i - 1] - '0';
                int OperationResult = (NumberDigit * Multiplicand) + Digit + Carry;
                Carry = OperationResult / 10;

                char NewDigit = (char)('0' + (OperationResult - Carry * 10));

                FractionalResult = NewDigit + FractionalResult;
            }

            while (FractionalResult.Length > 1 && FractionalResult[FractionalResult.Length - 1] == '0')
                FractionalResult = FractionalResult.Substring(0, FractionalResult.Length - 1);

            int ExponentIncrement = 0;
            while (Carry > 0)
            {
                int OperationResult = Carry;
                Carry = OperationResult / 10;

                char NewDigit = (char)('0' + (OperationResult - Carry));
                FractionalResult = NewDigit + FractionalResult;

                ExponentIncrement++;
            }

            string ExponentResult = "";

            for (int i = ExponentPart.Length; i > 0; i--)
            {
                int NumberDigit = ExponentPart[i - 1] - '0';
                int OperationResult = NumberDigit + ExponentIncrement + Carry;
                Carry = OperationResult / 10;
                ExponentIncrement /= 10;

                char NewDigit = (char)('0' + (OperationResult - Carry * 10));
                ExponentResult = NewDigit + ExponentResult;
            }

            while (Carry > 0)
            {
                int OperationResult = Carry;
                Carry = OperationResult / 10;

                char NewDigit = (char)('0' + (OperationResult - Carry));
                ExponentResult = NewDigit + ExponentResult;
            }

            FractionalPart = FractionalResult;
            ExponentPart = ExponentResult;

            Debug.Assert(IsNotationValid);
            FormatCanonicString();
        }

        public bool IsEqual(ICanonicalNumber Other)
        {
            return IsFractionalNegative == Other.IsFractionalNegative && FractionalPart == Other.FractionalPart && IsExponentNegative == Other.IsExponentNegative && ExponentPart == Other.ExponentPart;
        }

        public bool IsLesser(ICanonicalNumber Other)
        {
            if (IsFractionalNegative != Other.IsFractionalNegative)
                return IsFractionalNegative;

            return StringCompareParts(Other, IsFractionalNegative ? 1 : -1);
        }

        public bool IsGreater(ICanonicalNumber Other)
        {
            if (IsFractionalNegative != Other.IsFractionalNegative)
                return !IsFractionalNegative;

            return StringCompareParts(Other, IsFractionalNegative ? -1 : 1);
        }

        private bool StringCompareParts(ICanonicalNumber Other, int Direction)
        {
            if (!IsExponentNegative && !Other.IsExponentNegative)
            {
                int ComparedExponent = string.Compare(ExponentPart, Other.ExponentPart) * Direction;

                if (ComparedExponent > 0)
                    return true;
                else if (ComparedExponent < 0)
                    return false;
                else
                {
                    int ComparedFractional = string.Compare(FractionalPart, Other.FractionalPart) * Direction;
                    return ComparedFractional > 0;
                }
            }

            else if (IsExponentNegative && !Other.IsExponentNegative)
                return true;

            else if (!IsExponentNegative && Other.IsExponentNegative)
                return false;

            else
            {
                int ComparedExponent = string.Compare(ExponentPart, Other.ExponentPart) * Direction;

                if (ComparedExponent < 0)
                    return true;
                else if (ComparedExponent > 0)
                    return false;
                else
                {
                    int ComparedFractional = string.Compare(FractionalPart, Other.FractionalPart) * Direction;
                    return ComparedFractional < 0;
                }
            }
        }

        public ICanonicalNumber OppositeOf()
        {
            CanonicalNumber Result = new CanonicalNumber(FractionalPart, IsExponentNegative, ExponentPart);
            Result.IsFractionalNegative = !IsFractionalNegative;
            Result.FormatCanonicString();

            return Result;
        }

        public bool TryParseInt(out int Value)
        {
            Value = 0;

            if (IsExponentNegative)
                return false;

            if (FractionalPart.Length > 10 || ExponentPart.Length > 1)
                return false;

            int Fractional;
            int Exponent;
            if (!int.TryParse(FractionalPart, out Fractional) || !int.TryParse(ExponentPart, out Exponent))
                return false;

            if (Exponent + 1 < FractionalPart.Length)
                return false;

            Value = Fractional;
            int RemainingDigits = Exponent + 1 - FractionalPart.Length;

            while (RemainingDigits-- > 0)
                Value *= 10;

            return true;
        }
    }
}
