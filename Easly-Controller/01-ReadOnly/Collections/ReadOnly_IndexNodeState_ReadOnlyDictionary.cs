using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Read-only dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface IReadOnlyIndexNodeStateReadOnlyDictionary : IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>
    {
    }

    /// <summary>
    /// Read-only dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public class ReadOnlyIndexNodeStateReadOnlyDictionary : ReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState>, IReadOnlyIndexNodeStateReadOnlyDictionary
    {
        public ReadOnlyIndexNodeStateReadOnlyDictionary(IReadOnlyIndexNodeStateDictionary dictionary)
            : base(dictionary)
        {
        }
    }
}
