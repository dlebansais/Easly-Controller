namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class ReadOnlyPlaceholderNodeStateList : List<IReadOnlyPlaceholderNodeState>
    {
        /// <inheritdoc/>
        public virtual ReadOnlyPlaceholderNodeStateReadOnlyList ToReadOnly()
        {
            return new ReadOnlyPlaceholderNodeStateReadOnlyList(this);
        }
    }
}
