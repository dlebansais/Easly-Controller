namespace EaslyController.Frame
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class FrameKeywordFrameList : List<IFrameKeywordFrame>
    {
        /// <inheritdoc/>
        public virtual FrameKeywordFrameReadOnlyList ToReadOnly()
        {
            return new FrameKeywordFrameReadOnlyList(this);
        }
    }
}
