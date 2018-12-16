using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    public interface IReadOnlyIndexCollectionReadOnlyList : IReadOnlyList<IReadOnlyIndexCollection>
    {
        bool Contains(IReadOnlyIndexCollection value);
        int IndexOf(IReadOnlyIndexCollection value);
    }

    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    public class ReadOnlyIndexCollectionReadOnlyList : ReadOnlyCollection<IReadOnlyIndexCollection>, IReadOnlyIndexCollectionReadOnlyList
    {
        public ReadOnlyIndexCollectionReadOnlyList(IReadOnlyIndexCollectionList list)
            : base(list)
        {
        }
    }
}
