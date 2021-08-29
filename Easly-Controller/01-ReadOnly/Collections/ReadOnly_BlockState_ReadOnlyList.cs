#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Read-only list of IxxxBlockState
    /// </summary>
    public class ReadOnlyBlockStateReadOnlyList : ReadOnlyCollection<IReadOnlyBlockState>
    {
        public ReadOnlyBlockStateReadOnlyList(ReadOnlyBlockStateList list)
            : base(list)
        {
        }
    }
}
