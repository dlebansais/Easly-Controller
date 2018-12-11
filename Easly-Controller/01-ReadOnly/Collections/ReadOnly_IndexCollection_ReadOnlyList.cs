using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyIndexCollectionReadOnlyList : IReadOnlyList<IReadOnlyIndexCollection>
    {
        bool Contains(IReadOnlyIndexCollection value);
        int IndexOf(IReadOnlyIndexCollection value);
    }

    public class ReadOnlyIndexCollectionReadOnlyList : ReadOnlyCollection<IReadOnlyIndexCollection>, IReadOnlyIndexCollectionReadOnlyList
    {
        public ReadOnlyIndexCollectionReadOnlyList(IReadOnlyIndexCollectionList list)
            : base(list)
        {
        }
    }
}
