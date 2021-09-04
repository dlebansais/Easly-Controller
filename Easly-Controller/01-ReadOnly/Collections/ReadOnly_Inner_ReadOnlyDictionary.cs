namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    /// <inheritdoc/>
    public class ReadOnlyInnerReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IReadOnlyInner>, IEqualComparable
    {
        /// <inheritdoc/>
        public ReadOnlyInnerReadOnlyDictionary(ReadOnlyInnerDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyInnerReadOnlyDictionary<TKey> AsInnerReadOnlyDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsInnerReadOnlyDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<TKey, IReadOnlyInner> Entry in this)
            {
                if (!comparer.IsTrue(AsInnerReadOnlyDictionary.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(Entry.Value, AsInnerReadOnlyDictionary[Entry.Key] as IReadOnlyInner))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
