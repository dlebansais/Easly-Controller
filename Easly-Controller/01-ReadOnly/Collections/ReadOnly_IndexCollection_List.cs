#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

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
