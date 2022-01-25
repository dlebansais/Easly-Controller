namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using Contracts;

    /// <inheritdoc/>
    public class ReadOnlyInnerDictionary<TKey> : Dictionary<TKey, IReadOnlyInner>, IEqualComparable
    {
        /// <inheritdoc/>
        public ReadOnlyInnerDictionary() : base() { }
        /// <inheritdoc/>
        public ReadOnlyInnerDictionary(IDictionary<TKey, IReadOnlyInner> dictionary) : base(dictionary) { }
        /// <inheritdoc/>
        public ReadOnlyInnerDictionary(int capacity) : base(capacity) { }

        /// <inheritdoc/>
        public virtual ReadOnlyInnerReadOnlyDictionary<TKey> ToReadOnly()
        {
            return new ReadOnlyInnerReadOnlyDictionary<TKey>(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out ReadOnlyInnerDictionary<TKey> AsOtherDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<TKey, IReadOnlyInner> Entry in this)
            {
                if (!comparer.IsTrue(AsOtherDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Entry.Value, AsOtherDictionary[Entry.Key]))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
