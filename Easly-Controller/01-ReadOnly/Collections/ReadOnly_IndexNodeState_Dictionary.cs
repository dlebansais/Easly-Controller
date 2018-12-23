﻿using System.Collections.Generic;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public interface IReadOnlyIndexNodeStateDictionary : IDictionary<IReadOnlyIndex, IReadOnlyNodeState>
    {
    }

    /// <summary>
    /// Dictionary of IxxxIndex, IxxxNodeState
    /// </summary>
    public class ReadOnlyIndexNodeStateDictionary : Dictionary<IReadOnlyIndex, IReadOnlyNodeState>, IReadOnlyIndexNodeStateDictionary
    {
    }
}