namespace EaslyController.Frame
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class FrameVisibleCellViewReadOnlyList : ReadOnlyCollection<IFrameVisibleCellView>
    {
        /// <inheritdoc/>
        public FrameVisibleCellViewReadOnlyList(FrameVisibleCellViewList list)
            : base(list)
        {
        }
    }
}
