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
            if (!ComparedObjectList.ContainsKey(obj1))
                ComparedObjectList.Add(obj1, false);
            else if (!ComparedObjectList.ContainsKey(obj2))
                ComparedObjectList.Add(obj2, false);
            else
                return true;

            if (obj1.IsEqual(this, obj2))
                return true;
            else if (CanReturnFalse)
                return false;
            else
                return false; // For breakpoints.
        }

        public void Reset()
        {
            ComparedObjectList.Clear();
        }

        private Dictionary<IEqualComparable, bool> ComparedObjectList = new Dictionary<IEqualComparable, bool>();
    }
}
