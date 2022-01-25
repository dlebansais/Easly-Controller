namespace EaslyController.Frame
{
    using System.Collections.ObjectModel;

    /// <inheritdoc/>
    public class FrameTemplateReadOnlyDictionary : ReadOnlyDictionary<System.Type, IFrameTemplate>
    {
        /// <inheritdoc/>
        public FrameTemplateReadOnlyDictionary(FrameTemplateDictionary dictionary)
            : base(dictionary)
        {
        }
    }
}
