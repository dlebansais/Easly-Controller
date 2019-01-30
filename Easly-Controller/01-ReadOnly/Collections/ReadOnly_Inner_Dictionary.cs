namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IReadOnlyInnerDictionary<TKey> : IDictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>
    {
    }

    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    internal class ReadOnlyInnerDictionary<TKey> : Dictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>, IReadOnlyInnerDictionary<TKey>
    {
    }
}
