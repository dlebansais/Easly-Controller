namespace EaslyController.Frame
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class FrameTemplateReadOnlyList : ReadOnlyCollection<IFrameTemplate>
    {
        /// <inheritdoc/>
        public FrameTemplateReadOnlyList(FrameTemplateList list)
            : base(list)
        {
        }
    }
}
