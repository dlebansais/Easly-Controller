namespace EaslyController.Frame
{
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class FrameTemplateDictionary : Dictionary<System.Type, IFrameTemplate>
    {
        /// <inheritdoc/>
        public FrameTemplateDictionary() : base() { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(IDictionary<System.Type, IFrameTemplate> dictionary) : base(dictionary) { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(int capacity) : base(capacity) { }

        /// <inheritdoc/>
        public virtual FrameTemplateReadOnlyDictionary ToReadOnly()
        {
            return new FrameTemplateReadOnlyDictionary(this);
        }
    }
}
