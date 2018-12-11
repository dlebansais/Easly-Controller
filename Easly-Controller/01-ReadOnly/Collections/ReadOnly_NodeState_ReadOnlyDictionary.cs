using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyNodeStateReadOnlyDictionary<TKey> : IReadOnlyDictionary<TKey, IReadOnlyNodeState>
    {
    }

    public class ReadOnlyNodeStateReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IReadOnlyNodeState>, IReadOnlyNodeStateReadOnlyDictionary<TKey>
    {
        public ReadOnlyNodeStateReadOnlyDictionary(IReadOnlyNodeStateDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }
    }
}
