namespace EaslyController
{
    using System.Collections.Generic;
    using System.Diagnostics;

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
        /// Initializes a new instance of the <see cref="CompareEqual"/> class.
        /// </summary>
        /// <param name="canReturnFalse">True if the comparer should not break when a comparison fails.</param>
        public CompareEqual(bool canReturnFalse)
        {
            CanReturnFalse = canReturnFalse;
            ComparisonCount = 0;
            FailIndex = -1;
        }

        /// <summary>
        /// True if this comparer might return false during normal operations.
        /// </summary>
        public bool CanReturnFalse { get; }

        /// <summary>
        /// Number of comparisons performed.
        /// </summary>
        public int ComparisonCount { get; private set; }

        /// <summary>
        /// Index at which a comparison will fail regardless of the actual equality.
        /// </summary>
        public int FailIndex { get; private set; }

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

            bool Result;

            if (obj1.IsEqual(this, obj2))
                Result = true;
            else if (CanReturnFalse)
                Result = false;
            else
                Result = false; // For breakpoints.

            if (ComparisonCount == FailIndex)
                return false;

            ComparisonCount++;

            return Result;
        }

        /// <summary>
        /// Resets the comparison history (for debug purpose).
        /// </summary>
        public void Reset()
        {
            ComparedObjectList.Clear();
            ComparisonCount = 0;
            FailIndex = -1;
        }

        /// <summary>
        /// Set the index at which a comparison will fail regardless of the actual equality.
        /// </summary>
        /// <param name="failIndex">The index at which the comparison will fail.</param>
        public void SetFailIndex(int failIndex)
        {
            Reset();
            FailIndex = failIndex;
        }

        /// <summary>
        /// Proxy that breaks on failure.
        /// </summary>
        /// <returns>Always return false.</returns>
        public bool Failed()
        {
            if (!CanReturnFalse && ComparisonCount != FailIndex)
                Debug.Fail("Objects are not equal");

            return false;
        }

        /// <summary>
        /// Check that a condition is true.
        /// </summary>
        /// <param name="condition">The condition to check.</param>
        public bool IsTrue(bool condition)
        {
            if (ComparisonCount == FailIndex)
                return false;

            ComparisonCount++;

            return condition;
        }

        /// <summary>
        /// Checks if an object is of the expected type, and return the corresponding converted reference
        /// </summary>
        /// <typeparam name="TObject">Any <see cref="IEqualComparable"/>.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <param name="asTObject">The converted object if successful. Null otherwise.</param>
        public bool IsSameType<TObject>(IEqualComparable obj, out TObject asTObject)
            where TObject : IEqualComparable
        {
            bool Result;

            if (obj is TObject AsTObject)
            {
                asTObject = AsTObject;
                Result = true;
            }
            else
            {
                asTObject = default;
                Result = false;
            }

            if (ComparisonCount == FailIndex)
            {
                asTObject = default;
                return false;
            }

            ComparisonCount++;

            return Result;
        }

        /// <summary>
        /// Checks that two references refer to the same object.
        /// </summary>
        /// <param name="object1">The first reference.</param>
        /// <param name="object2">The second reference.</param>
        public bool IsSameReference(object object1, object object2)
        {
            bool Result = object1 == object2;

            if (ComparisonCount == FailIndex)
                return false;

            ComparisonCount++;

            return Result;
        }

        /// <summary>
        /// Checks that two counts are equal.
        /// </summary>
        /// <param name="count1">The first count.</param>
        /// <param name="count2">The second count.</param>
        public bool IsSameCount(int count1, int count2)
        {
            return IsSameInteger(count1, count2);
        }

        /// <summary>
        /// Checks that two integer values are equal.
        /// </summary>
        /// <param name="int1">The first value.</param>
        /// <param name="int2">The second value.</param>
        public bool IsSameInteger(int int1, int int2)
        {
            bool Result = int1 == int2;

            if (ComparisonCount == FailIndex)
                return false;

            ComparisonCount++;

            return Result;
        }

        /// <summary>
        /// Checks that two strings are equal.
        /// </summary>
        /// <param name="string1">The first string.</param>
        /// <param name="string2">The second string.</param>
        public bool IsSameString(string string1, string string2)
        {
            bool Result = string1 == string2;

            if (ComparisonCount == FailIndex)
                return false;

            ComparisonCount++;

            return Result;
        }

        /// <summary>
        /// Compares two objects, ensuring all failure path are executed.
        /// </summary>
        /// <param name="object1">The first object to compare.</param>
        /// <param name="object2">The Second object to compare.</param>
        public static bool CoverIsEqual(IEqualComparable object1, IEqualComparable object2)
        {
            CompareEqual Comparer = New();

            bool IsValid = object1.IsEqual(Comparer, object2);
            if (!IsValid)
                return false;

            int ComparisonCount = Comparer.ComparisonCount;

            for (int i = 0; i < ComparisonCount; i++)
            {
                Comparer.SetFailIndex(i);
                object1.IsEqual(Comparer, object2);
            }

            return true;
        }

        private Dictionary<IEqualComparable, bool> ComparedObjectList = new Dictionary<IEqualComparable, bool>();
    }
}
