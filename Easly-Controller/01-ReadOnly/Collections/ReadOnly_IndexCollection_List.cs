using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    public interface IReadOnlyIndexCollectionList : IList<IReadOnlyIndexCollection>, IReadOnlyList<IReadOnlyIndexCollection>
    {
        new int Count { get; }
        new IReadOnlyIndexCollection this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    public class ReadOnlyIndexCollectionList : Collection<IReadOnlyIndexCollection>, IReadOnlyIndexCollectionList
    {
    }
}
