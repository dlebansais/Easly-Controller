namespace EaslyController.Frame
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class FrameFrameReadOnlyList : ReadOnlyCollection<IFrameFrame>
    {
        /// <inheritdoc/>
        public FrameFrameReadOnlyList(FrameFrameList list)
            : base(list)
        {
        }
    }
}
