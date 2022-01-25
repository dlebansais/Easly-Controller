namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using Contracts;

    /// <inheritdoc/>
    public class ReadOnlyNodeStateViewDictionary : Dictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public ReadOnlyNodeStateViewDictionary() : base() { }
        /// <inheritdoc/>
        public ReadOnlyNodeStateViewDictionary(IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView> dictionary) : base(dictionary) { }
        /// <inheritdoc/>
        public ReadOnlyNodeStateViewDictionary(int capacity) : base(capacity) { }

        /// <inheritdoc/>
        public virtual ReadOnlyNodeStateViewReadOnlyDictionary ToReadOnly()
        {
            return new ReadOnlyNodeStateViewReadOnlyDictionary(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out ReadOnlyNodeStateViewDictionary AsOtherDictionary))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherDictionary.Count))
                return comparer.Failed();

            foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in this)
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
