namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public class ReadOnlyInnerDictionary<TKey> : Dictionary<TKey, IReadOnlyInner>
    {
        /// <summary>
        /// Gets a read-only view of the dictionary.
        /// </summary>
        public virtual ReadOnlyInnerReadOnlyDictionary<TKey> ToReadOnly()
        {
            return new ReadOnlyInnerReadOnlyDictionary<TKey>(this);
        }
    }
}
