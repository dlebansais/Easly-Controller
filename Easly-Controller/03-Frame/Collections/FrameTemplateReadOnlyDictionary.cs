namespace EaslyController.Frame
{
    using System.Collections.ObjectModel;
    using NotNullReflection;

    /// <inheritdoc/>
    public class FrameTemplateReadOnlyDictionary : ReadOnlyDictionary<Type, IFrameTemplate>
    {
        /// <inheritdoc/>
        public FrameTemplateReadOnlyDictionary(FrameTemplateDictionary dictionary)
            : base(dictionary)
        {
        }
    }
}
