#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    public class ReadOnlyNodeStateList : Collection<IReadOnlyNodeState>
    {
        public virtual ReadOnlyNodeStateReadOnlyList ToReadOnly()
        {
            return new ReadOnlyNodeStateReadOnlyList(this);
        }
    }
}
