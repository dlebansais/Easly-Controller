#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Read-only list of IxxxNodeState
    /// </summary>
    public class ReadOnlyNodeStateReadOnlyList : ReadOnlyCollection<IReadOnlyNodeState>
    {
        public ReadOnlyNodeStateReadOnlyList(ReadOnlyNodeStateList list)
            : base(list)
        {
        }
    }
}
