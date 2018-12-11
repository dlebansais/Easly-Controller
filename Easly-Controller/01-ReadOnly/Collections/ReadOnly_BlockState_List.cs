using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBlockStateList : IList<IReadOnlyBlockState>, IReadOnlyList<IReadOnlyBlockState>
    {
        new int Count { get; }
        new IReadOnlyBlockState this[int index] { get; set; }
    }

    public class ReadOnlyBlockStateList : Collection<IReadOnlyBlockState>, IReadOnlyBlockStateList
    {
    }
}
