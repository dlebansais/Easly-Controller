using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyIndexCollectionList : IList<IReadOnlyIndexCollection>, IReadOnlyList<IReadOnlyIndexCollection>
    {
        new int Count { get; }
        new IReadOnlyIndexCollection this[int index] { get; set; }
    }

    public class ReadOnlyIndexCollectionList : Collection<IReadOnlyIndexCollection>, IReadOnlyIndexCollectionList
    {
    }
}
