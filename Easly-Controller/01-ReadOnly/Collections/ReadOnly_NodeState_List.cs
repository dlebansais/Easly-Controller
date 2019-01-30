#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    public interface IReadOnlyNodeStateList : IList<IReadOnlyNodeState>, IReadOnlyList<IReadOnlyNodeState>
    {
        new int Count { get; }
        new IReadOnlyNodeState this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    public class ReadOnlyNodeStateList : Collection<IReadOnlyNodeState>, IReadOnlyNodeStateList
    {
    }
}
