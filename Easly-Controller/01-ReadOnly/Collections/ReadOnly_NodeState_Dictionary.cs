using System.Collections.Generic;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyNodeStateDictionary<TKey> : IDictionary<TKey, IReadOnlyNodeState>
    {
    }

    public class ReadOnlyNodeStateDictionary<TKey> : Dictionary<TKey, IReadOnlyNodeState>, IReadOnlyNodeStateDictionary<TKey>
    {
    }
}
