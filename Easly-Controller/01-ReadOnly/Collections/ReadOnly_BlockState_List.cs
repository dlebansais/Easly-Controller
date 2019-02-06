#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    public interface IReadOnlyBlockStateList : IList<IReadOnlyBlockState>, IReadOnlyList<IReadOnlyBlockState>
    {
        new IReadOnlyBlockState this[int index] { get; set; }
        new int Count { get; }
        IReadOnlyBlockStateReadOnlyList ToReadOnly();
    }

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    internal class ReadOnlyBlockStateList : Collection<IReadOnlyBlockState>, IReadOnlyBlockStateList
    {
        public virtual IReadOnlyBlockStateReadOnlyList ToReadOnly()
        {
            return new ReadOnlyBlockStateReadOnlyList(this);
        }
    }
}
