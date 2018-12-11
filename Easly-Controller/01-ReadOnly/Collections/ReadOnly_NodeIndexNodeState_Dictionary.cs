using System.Collections.Generic;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyNodeIndexNodeStateDictionary : IDictionary<IReadOnlyNodeIndex, IReadOnlyNodeState>
    {
    }

    public class ReadOnlyNodeIndexNodeStateDictionary : Dictionary<IReadOnlyNodeIndex, IReadOnlyNodeState>, IReadOnlyNodeIndexNodeStateDictionary
    {
    }
}
