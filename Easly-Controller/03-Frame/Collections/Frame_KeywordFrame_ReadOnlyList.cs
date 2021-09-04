namespace EaslyController.Frame
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class FrameKeywordFrameReadOnlyList : ReadOnlyCollection<IFrameKeywordFrame>
    {
        /// <inheritdoc/>
        public FrameKeywordFrameReadOnlyList(FrameKeywordFrameList list)
            : base(list)
        {
        }
    }
}
