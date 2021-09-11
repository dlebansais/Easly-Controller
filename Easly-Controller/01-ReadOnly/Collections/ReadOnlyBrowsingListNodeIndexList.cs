namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class ReadOnlyBrowsingListNodeIndexList : List<IReadOnlyBrowsingListNodeIndex>
    {
        /// <inheritdoc/>
        public virtual ReadOnlyBrowsingListNodeIndexReadOnlyList ToReadOnly()
        {
            return new ReadOnlyBrowsingListNodeIndexReadOnlyList(this);
        }
    }
}
