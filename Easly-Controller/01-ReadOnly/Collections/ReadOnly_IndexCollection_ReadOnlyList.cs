#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    internal class ReadOnlyIndexCollectionReadOnlyList : ReadOnlyCollection<IReadOnlyIndexCollection>
    {
        public ReadOnlyIndexCollectionReadOnlyList(ReadOnlyIndexCollectionList list)
            : base(list)
        {
        }
    }
}
