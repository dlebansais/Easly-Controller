#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    internal interface IReadOnlyIndexCollectionList : IList<IReadOnlyIndexCollection>, IReadOnlyList<IReadOnlyIndexCollection>
    {
        new IReadOnlyIndexCollection this[int index] { get; set; }
        new int Count { get; }
        IReadOnlyIndexCollectionReadOnlyList ToReadOnly();
    }

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    internal class ReadOnlyIndexCollectionList : Collection<IReadOnlyIndexCollection>, IReadOnlyIndexCollectionList
    {
        public virtual IReadOnlyIndexCollectionReadOnlyList ToReadOnly()
        {
            return new ReadOnlyIndexCollectionReadOnlyList(this);
        }
    }
}
