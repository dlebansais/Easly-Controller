namespace EaslyController.Focus
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class FocusNodeFrameVisibilityList : List<IFocusNodeFrameVisibility>
    {
        /// <inheritdoc/>
        public virtual FocusNodeFrameVisibilityReadOnlyList ToReadOnly()
        {
            return new FocusNodeFrameVisibilityReadOnlyList(this);
        }
    }
}
