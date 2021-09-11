namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class ReadOnlyNodeStateList : List<IReadOnlyNodeState>
    {
        /// <inheritdoc/>
        public virtual ReadOnlyNodeStateReadOnlyList ToReadOnly()
        {
            return new ReadOnlyNodeStateReadOnlyList(this);
        }
    }
}
