using System.Collections.Generic;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    public interface IReadOnlyInnerDictionary<TKey> : IDictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>
    {
    }

    /// <summary>
    /// Dictionary of ..., IxxxInner
    /// </summary>
    public class ReadOnlyInnerDictionary<TKey> : Dictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>, IReadOnlyInnerDictionary<TKey>
    {
    }
}
