using System.Collections.Generic;

namespace EaslyController
{
    public interface IEqualComparable
    {
        bool IsEqual(CompareEqual comparer, IEqualComparable other);
    }

    public class CompareEqual
    {
        public static CompareEqual New(bool canReturnFalse = false)
        {
            return new CompareEqual(canReturnFalse);
        }

        public CompareEqual(bool canReturnFalse)
        {
            CanReturnFalse = canReturnFalse;
        }

        public bool CanReturnFalse { get; }

        public bool VerifyEqual(IEqualComparable obj1, IEqualComparable obj2)
        {
            if (!ComparedObjectList.Contains(obj1))
                ComparedObjectList.Add(obj1);
            else if (!ComparedObjectList.Contains(obj2))
                ComparedObjectList.Add(obj2);
            else
                return true;

            if (obj1.IsEqual(this, obj2))
                return true;
            else if (CanReturnFalse)
                return false;
            else
                return false; // For breakpoints.
        }

        private List<IEqualComparable> ComparedObjectList = new List<IEqualComparable>();
    }
}
