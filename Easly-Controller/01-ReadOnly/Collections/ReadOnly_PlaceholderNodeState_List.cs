#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    public interface IReadOnlyPlaceholderNodeStateList : IList<IReadOnlyPlaceholderNodeState>, IReadOnlyList<IReadOnlyPlaceholderNodeState>
    {
        new int Count { get; }
        new IReadOnlyPlaceholderNodeState this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    internal class ReadOnlyPlaceholderNodeStateList : Collection<IReadOnlyPlaceholderNodeState>, IReadOnlyPlaceholderNodeStateList
    {
    }
}
