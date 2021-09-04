namespace EaslyController.Frame
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class FrameVisibleCellViewList : List<IFrameVisibleCellView>
    {
        /// <inheritdoc/>
        public virtual FrameVisibleCellViewReadOnlyList ToReadOnly()
        {
            return new FrameVisibleCellViewReadOnlyList(this);
        }
    }
}
