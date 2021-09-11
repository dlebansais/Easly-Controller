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
        public FrameTemplateDictionary(IEqualityComparer<System.Type> comparer) : base(comparer) { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(int capacity) : base(capacity) { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(IDictionary<System.Type, IFrameTemplate> dictionary, IEqualityComparer<System.Type> comparer) : base(dictionary, comparer) { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(int capacity, IEqualityComparer<System.Type> comparer) : base(capacity, comparer) { }

        /// <inheritdoc/>
        public virtual FrameTemplateReadOnlyDictionary ToReadOnly()
        {
            return new FrameTemplateReadOnlyDictionary(this);
        }
    }
}
