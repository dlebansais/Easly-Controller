#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Read-only list of IxxxBlockState
    /// </summary>
    public interface IReadOnlyBlockStateReadOnlyList : IReadOnlyList<IReadOnlyBlockState>
    {
        bool Contains(IReadOnlyBlockState value);
        int IndexOf(IReadOnlyBlockState value);
    }

    /// <summary>
    /// Read-only list of IxxxBlockState
    /// </summary>
    internal class ReadOnlyBlockStateReadOnlyList : ReadOnlyCollection<IReadOnlyBlockState>, IReadOnlyBlockStateReadOnlyList
    {
        public ReadOnlyBlockStateReadOnlyList(IReadOnlyBlockStateList list)
            : base(list)
        {
        }
    }
}
