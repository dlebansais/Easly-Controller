namespace EaslyController.Focus
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class FocusSelectableFrameReadOnlyList : ReadOnlyCollection<IFocusSelectableFrame>
    {
        /// <inheritdoc/>
        public FocusSelectableFrameReadOnlyList(FocusSelectableFrameList list)
            : base(list)
        {
        }
    }
}
