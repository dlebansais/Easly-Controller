namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IReadOnlyInnerDictionary<TKey> : IDictionary<TKey, IReadOnlyInner>
    {
        /// <summary>
        /// Gets a read-only view of the dictionary.
        /// </summary>
        IReadOnlyInnerReadOnlyDictionary<TKey> ToReadOnly();
    }

    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    internal class ReadOnlyInnerDictionary<TKey> : Dictionary<TKey, IReadOnlyInner>, IReadOnlyInnerDictionary<TKey>
    {
        /// <summary>
        /// Gets a read-only view of the dictionary.
        /// </summary>
        public virtual IReadOnlyInnerReadOnlyDictionary<TKey> ToReadOnly()
        {
            return new ReadOnlyInnerReadOnlyDictionary<TKey>(this);
        }
    }
}
