using System.Collections.Generic;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyInnerDictionary<TKey> : IDictionary<TKey, IReadOnlyInner>
    {
    }

    public class ReadOnlyInnerDictionary<TKey> : Dictionary<TKey, IReadOnlyInner>, IReadOnlyInnerDictionary<TKey>
    {
    }
}
