namespace EaslyController.Focus
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class FocusInsertionChildNodeIndexList : List<IFocusInsertionChildNodeIndex>
    {
        /// <inheritdoc/>
        public virtual FocusInsertionChildNodeIndexReadOnlyList ToReadOnly()
        {
            return new FocusInsertionChildNodeIndexReadOnlyList(this);
        }
    }
}
