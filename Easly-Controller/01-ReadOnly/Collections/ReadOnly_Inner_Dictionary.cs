using System.Collections.Generic;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyInnerDictionary<TKey> : IDictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>
    {
    }

    public class ReadOnlyInnerDictionary<TKey> : Dictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>, IReadOnlyInnerDictionary<TKey>
    {
    }
}
