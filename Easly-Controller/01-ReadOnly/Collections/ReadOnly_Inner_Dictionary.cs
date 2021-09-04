namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <inheritdoc/>
    public class ReadOnlyInnerDictionary<TKey> : Dictionary<TKey, IReadOnlyInner>, IEqualComparable
    {
        /// <inheritdoc/>
        public virtual ReadOnlyInnerReadOnlyDictionary<TKey> ToReadOnly()
        {
            return new ReadOnlyInnerReadOnlyDictionary<TKey>(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyInnerDictionary<TKey> AsInnerDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsInnerDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<TKey, IReadOnlyInner> Entry in this)
            {
                if (!comparer.IsTrue(AsInnerDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Entry.Value, AsInnerDictionary[Entry.Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
