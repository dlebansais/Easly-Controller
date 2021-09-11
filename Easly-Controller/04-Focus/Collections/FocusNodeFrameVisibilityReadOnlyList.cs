namespace EaslyController.Focus
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class FocusNodeFrameVisibilityReadOnlyList : ReadOnlyCollection<IFocusNodeFrameVisibility>
    {
        /// <inheritdoc/>
        public FocusNodeFrameVisibilityReadOnlyList(FocusNodeFrameVisibilityList list)
            : base(list)
        {
        }
    }
}
