namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface IReadOnlyIndexNodeStateDictionary : IDictionary<IReadOnlyIndex, IReadOnlyNodeState>
    {
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    internal class ReadOnlyIndexNodeStateDictionary : Dictionary<IReadOnlyIndex, IReadOnlyNodeState>, IReadOnlyIndexNodeStateDictionary
    {
    }
}
