using System.Collections.Generic;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyIndexNodeStateDictionary : IDictionary<IReadOnlyIndex, IReadOnlyNodeState>
    {
    }

    public class ReadOnlyIndexNodeStateDictionary : Dictionary<IReadOnlyIndex, IReadOnlyNodeState>, IReadOnlyIndexNodeStateDictionary
    {
    }
}
