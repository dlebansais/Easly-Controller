namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <inheritdoc/>
    public class ReadOnlyStateViewDictionary : Dictionary<IReadOnlyNodeState, ReadOnlyNodeStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public virtual ReadOnlyStateViewReadOnlyDictionary ToReadOnly()
        {
            return new ReadOnlyStateViewReadOnlyDictionary(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyStateViewDictionary AsStateViewDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsStateViewDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<IReadOnlyNodeState, ReadOnlyNodeStateView> Entry in this)
            {
                if (!comparer.IsTrue(AsStateViewDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Entry.Value, AsStateViewDictionary[Entry.Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
