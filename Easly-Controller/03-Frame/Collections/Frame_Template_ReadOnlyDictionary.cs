namespace EaslyController.Frame
{
    using System;
    using System.Collections.ObjectModel;

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
