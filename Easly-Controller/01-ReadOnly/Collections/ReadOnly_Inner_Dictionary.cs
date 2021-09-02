namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class ReadOnlyInnerDictionary<TKey> : Dictionary<TKey, IReadOnlyInner>
    {
        /// <inheritdoc/>
        public virtual ReadOnlyInnerReadOnlyDictionary<TKey> ToReadOnly()
        {
            return new ReadOnlyInnerReadOnlyDictionary<TKey>(this);
        }
    }
}
