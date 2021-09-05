namespace EaslyController.Focus
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class FocusSelectableFrameList : List<IFocusSelectableFrame>
    {
        /// <inheritdoc/>
        public virtual FocusSelectableFrameReadOnlyList ToReadOnly()
        {
            return new FocusSelectableFrameReadOnlyList(this);
        }
    }
}
