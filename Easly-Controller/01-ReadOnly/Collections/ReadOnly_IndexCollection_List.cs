#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    internal class ReadOnlyIndexCollectionList : List<IReadOnlyIndexCollection>
    {
        public virtual ReadOnlyIndexCollectionReadOnlyList ToReadOnly()
        {
            return new ReadOnlyIndexCollectionReadOnlyList(this);
        }
    }
}
