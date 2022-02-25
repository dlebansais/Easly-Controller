namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using NotNullReflection;

    /// <inheritdoc/>
    public class FrameTemplateDictionary : Dictionary<Type, IFrameTemplate>
    {
        /// <inheritdoc/>
        public FrameTemplateDictionary() : base() { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(IDictionary<Type, IFrameTemplate> dictionary) : base(dictionary) { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(int capacity) : base(capacity) { }

        /// <inheritdoc/>
        public virtual FrameTemplateReadOnlyDictionary ToReadOnly()
        {
            return new FrameTemplateReadOnlyDictionary(this);
        }
    }
}
