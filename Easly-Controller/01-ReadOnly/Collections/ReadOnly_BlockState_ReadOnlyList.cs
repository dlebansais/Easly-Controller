using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBlockStateReadOnlyList : IReadOnlyList<IReadOnlyBlockState>
    {
        bool Contains(IReadOnlyBlockState value);
        int IndexOf(IReadOnlyBlockState value);
    }

    public class ReadOnlyBlockStateReadOnlyList : ReadOnlyCollection<IReadOnlyBlockState>, IReadOnlyBlockStateReadOnlyList
    {
        public ReadOnlyBlockStateReadOnlyList(IReadOnlyBlockStateList list)
            : base(list)
        {
        }
    }
}
