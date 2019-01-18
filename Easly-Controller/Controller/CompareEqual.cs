using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController
{
    /// <summary>
    /// Represent objects that can be compared for equality in a tree, while avoiding infinite recursion.
    /// </summary>
    public interface IEqualComparable
    {
        /// <summary>
        /// Checks that two objects are equal.
        /// </summary>
        /// <param name="comparer">The comparison object to use.</param>
        /// <param name="other">The other instance of an object to compare with this one.</param>
        /// <returns></returns>
        bool IsEqual(CompareEqual comparer, IEqualComparable other);
    }

    /// <summary>
    /// Compares <see cref="IEqualComparable"/> objects.
    /// </summary>
    public class CompareEqual
    {
        /// <summary>
        /// Create a new comparer.
        /// </summary>
        /// <param name="canReturnFalse">True if this comparer might return false during normal operations.</param>
        public static CompareEqual New(bool canReturnFalse = false)
        {
            return new CompareEqual(canReturnFalse);
        }

        /// <summary>
        /// Initializers a new instance of a <see cref="CompareEqual"/> object.
        /// </summary>
        /// <param name="canReturnFalse"></param>
        public CompareEqual(bool canReturnFalse)
        {
            CanReturnFalse = canReturnFalse;
        }

        /// <summary>
        /// True if this comparer might return false during normal operations.
        /// </summary>
        public bool CanReturnFalse { get; }

        /// <summary>
        /// Checks that two objects are equal.
        /// </summary>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        public bool VerifyEqual(IEqualComparable obj1, IEqualComparable obj2)
        {
            Debug.Assert(obj1 != null);
            Debug.Assert(obj2 != null);

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

        /// <summary>
        /// Resets the comparison history (for debug purpose).
        /// </summary>
        public void Reset()
        {
            ComparedObjectList.Clear();
        }

        private Dictionary<IEqualComparable, bool> ComparedObjectList = new Dictionary<IEqualComparable, bool>();
    }
}
