using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyInnerReadOnlyDictionary<TKey> : IReadOnlyDictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>
    {
    }

    public class ReadOnlyInnerReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>, IReadOnlyInnerReadOnlyDictionary<TKey>
    {
        public ReadOnlyInnerReadOnlyDictionary(IReadOnlyInnerDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }
    }
}
