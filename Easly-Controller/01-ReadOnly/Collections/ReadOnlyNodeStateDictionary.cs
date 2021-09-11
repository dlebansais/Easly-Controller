namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class ReadOnlyNodeStateDictionary : Dictionary<IReadOnlyIndex, IReadOnlyNodeState>
    {
        /// <inheritdoc/>
        public ReadOnlyNodeStateDictionary() : base() { }
        /// <inheritdoc/>
        public ReadOnlyNodeStateDictionary(IDictionary<IReadOnlyIndex, IReadOnlyNodeState> dictionary) : base(dictionary) { }
        /// <inheritdoc/>
        public ReadOnlyNodeStateDictionary(IEqualityComparer<IReadOnlyIndex> comparer) : base(comparer) { }
        /// <inheritdoc/>
        public ReadOnlyNodeStateDictionary(int capacity) : base(capacity) { }
        /// <inheritdoc/>
        public ReadOnlyNodeStateDictionary(IDictionary<IReadOnlyIndex, IReadOnlyNodeState> dictionary, IEqualityComparer<IReadOnlyIndex> comparer) : base(dictionary, comparer) { }
        /// <inheritdoc/>
        public ReadOnlyNodeStateDictionary(int capacity, IEqualityComparer<IReadOnlyIndex> comparer) : base(capacity, comparer) { }

        /// <inheritdoc/>
        public virtual ReadOnlyNodeStateReadOnlyDictionary ToReadOnly()
        {
            return new ReadOnlyNodeStateReadOnlyDictionary(this);
        }
    }
}
