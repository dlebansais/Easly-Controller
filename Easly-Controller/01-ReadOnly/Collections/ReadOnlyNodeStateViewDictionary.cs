﻿namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class ReadOnlyNodeStateViewDictionary : Dictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>, IEqualComparable
    {
        /// <inheritdoc/>
        public ReadOnlyNodeStateViewDictionary() : base() { }
        /// <inheritdoc/>
        public ReadOnlyNodeStateViewDictionary(IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView> dictionary) : base(dictionary) { }
        /// <inheritdoc/>
        public ReadOnlyNodeStateViewDictionary(IEqualityComparer<IReadOnlyNodeState> comparer) : base(comparer) { }
        /// <inheritdoc/>
        public ReadOnlyNodeStateViewDictionary(int capacity) : base(capacity) { }
        /// <inheritdoc/>
        public ReadOnlyNodeStateViewDictionary(IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView> dictionary, IEqualityComparer<IReadOnlyNodeState> comparer) : base(dictionary, comparer) { }
        /// <inheritdoc/>
        public ReadOnlyNodeStateViewDictionary(int capacity, IEqualityComparer<IReadOnlyNodeState> comparer) : base(capacity, comparer) { }

        /// <inheritdoc/>
        public virtual ReadOnlyNodeStateViewReadOnlyDictionary ToReadOnly()
        {
            return new ReadOnlyNodeStateViewReadOnlyDictionary(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyNodeStateViewDictionary AsOtherDictionary))
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
