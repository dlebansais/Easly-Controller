namespace EaslyController.Focus
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class FocusInsertionChildNodeIndexReadOnlyList : ReadOnlyCollection<IFocusInsertionChildNodeIndex>
    {
        /// <inheritdoc/>
        public FocusInsertionChildNodeIndexReadOnlyList(FocusInsertionChildNodeIndexList list)
            : base(list)
        {
        }
    }
}
