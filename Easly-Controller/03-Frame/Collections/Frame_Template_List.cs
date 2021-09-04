namespace EaslyController.Frame
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class FrameTemplateList : List<IFrameTemplate>
    {
        /// <inheritdoc/>
        public virtual FrameTemplateReadOnlyList ToReadOnly()
        {
            return new FrameTemplateReadOnlyList(this);
        }
    }
}
