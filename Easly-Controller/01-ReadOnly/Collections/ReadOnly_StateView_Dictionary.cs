using System.Collections.Generic;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Dictionary of IxxxNodeState, IxxxNodeStateView
    /// </summary>
    public interface IReadOnlyStateViewDictionary : IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>
    {
    }

    /// <summary>
    /// Dictionary of IxxxNodeState, IxxxNodeStateView
    /// </summary>
    public class ReadOnlyStateViewDictionary : Dictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>, IReadOnlyStateViewDictionary
    {
    }
}
