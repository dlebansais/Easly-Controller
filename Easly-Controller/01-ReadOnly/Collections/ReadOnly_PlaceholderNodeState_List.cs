#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxPlaceholderNodeState
    /// </summary>
    public class ReadOnlyPlaceholderNodeStateList : List<IReadOnlyPlaceholderNodeState>
    {
        public virtual ReadOnlyPlaceholderNodeStateReadOnlyList ToReadOnly()
        {
            return new ReadOnlyPlaceholderNodeStateReadOnlyList(this);
        }
    }
}
