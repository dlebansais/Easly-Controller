namespace EaslyController.Frame
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class FrameFrameList : List<IFrameFrame>
    {
        /// <inheritdoc/>
        public virtual FrameFrameReadOnlyList ToReadOnly()
        {
            return new FrameFrameReadOnlyList(this);
        }
    }
}
