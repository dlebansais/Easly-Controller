using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyIndexNodeStateReadOnlyDictionary : IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>
    {
    }

    public class ReadOnlyIndexNodeStateReadOnlyDictionary : ReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>, IReadOnlyIndexNodeStateReadOnlyDictionary
    {
        public ReadOnlyIndexNodeStateReadOnlyDictionary(IReadOnlyIndexNodeStateDictionary dictionary)
            : base(dictionary)
        {
        }
    }
}
