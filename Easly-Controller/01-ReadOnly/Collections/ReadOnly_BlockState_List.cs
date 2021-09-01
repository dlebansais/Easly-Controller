#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    public class ReadOnlyBlockStateList : List<IReadOnlyBlockState>
    {
        public virtual ReadOnlyBlockStateReadOnlyList ToReadOnly()
        {
            return new ReadOnlyBlockStateReadOnlyList(this);
        }
    }
}
