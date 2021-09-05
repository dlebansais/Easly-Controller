namespace EaslyController.Frame
{
    using System;
    using System.Collections.Generic;

    /// <inheritdoc/>
    public class FrameTemplateDictionary : Dictionary<Type, IFrameTemplate>
    {
        /// <inheritdoc/>
        public FrameTemplateDictionary() : base() { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(IDictionary<Type, IFrameTemplate> dictionary) : base(dictionary) { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(IEqualityComparer<Type> comparer) : base(comparer) { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(int capacity) : base(capacity) { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(IDictionary<Type, IFrameTemplate> dictionary, IEqualityComparer<Type> comparer) : base(dictionary, comparer) { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(int capacity, IEqualityComparer<Type> comparer) : base(capacity, comparer) { }

        /// <inheritdoc/>
        public virtual FrameTemplateReadOnlyDictionary ToReadOnly()
        {
            return new FrameTemplateReadOnlyDictionary(this);
        }
    }
}
