using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    public interface IReadOnlyInnerReadOnlyDictionary<TKey> : IReadOnlyDictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>
    {
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    public class ReadOnlyInnerReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IReadOnlyInner<IReadOnlyBrowsingChildIndex>>, IReadOnlyInnerReadOnlyDictionary<TKey>
    {
        public ReadOnlyInnerReadOnlyDictionary(IReadOnlyInnerDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }
    }
}
