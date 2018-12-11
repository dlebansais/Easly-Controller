using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyInnerReadOnlyDictionary<TKey> : IReadOnlyDictionary<TKey, IReadOnlyInner>
    {
    }

    public class ReadOnlyInnerReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IReadOnlyInner>, IReadOnlyInnerReadOnlyDictionary<TKey>
    {
        public ReadOnlyInnerReadOnlyDictionary(IReadOnlyInnerDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }
    }
}
