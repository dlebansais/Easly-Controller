namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <inheritdoc/>
    public class FrameAssignableCellViewDictionary<TKey> : Dictionary<TKey, IFrameAssignableCellView>, IEqualComparable
    {
        /// <inheritdoc/>
        public virtual FrameAssignableCellViewReadOnlyDictionary<TKey> ToReadOnly()
        {
            return new FrameAssignableCellViewReadOnlyDictionary<TKey>(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameAssignableCellViewDictionary<TKey> AsAssignableCellViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsAssignableCellViewDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<TKey, IFrameAssignableCellView> Entry in this)
            {
                if (!comparer.IsTrue(AsAssignableCellViewDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Entry.Value, AsAssignableCellViewDictionary[Entry.Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
