namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class ReadOnlyBrowsingBlockNodeIndexList : List<IReadOnlyBrowsingBlockNodeIndex>
    {
        /// <inheritdoc/>
        public virtual ReadOnlyBrowsingBlockNodeIndexReadOnlyList ToReadOnly()
        {
            return new ReadOnlyBrowsingBlockNodeIndexReadOnlyList(this);
        }
    }
}
