namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class ReadOnlyIndexCollectionList : List<IReadOnlyIndexCollection>
    {
        /// <inheritdoc/>
        public virtual ReadOnlyIndexCollectionReadOnlyList ToReadOnly()
        {
            return new ReadOnlyIndexCollectionReadOnlyList(this);
        }
    }
}
