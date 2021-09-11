namespace EaslyController.Frame
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class FrameTemplateReadOnlyDictionary : System.Collections.ObjectModel.ReadOnlyDictionary<System.Type, IFrameTemplate>
    {
        /// <inheritdoc/>
        public FrameTemplateReadOnlyDictionary(FrameTemplateDictionary dictionary)
            : base(dictionary)
        {
        }
    }
}
