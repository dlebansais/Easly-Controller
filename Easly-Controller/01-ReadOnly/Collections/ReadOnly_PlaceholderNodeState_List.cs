﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
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
    public class ReadOnlyPlaceholderNodeStateList : Collection<IReadOnlyPlaceholderNodeState>, IReadOnlyPlaceholderNodeStateList
    {
    }
}
