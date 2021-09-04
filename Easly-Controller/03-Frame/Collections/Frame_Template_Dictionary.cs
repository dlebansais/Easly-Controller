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
        public FrameTemplateDictionary(IEnumerable<KeyValuePair<Type, IFrameTemplate>> collection) : base(collection) { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(IEqualityComparer<Type> comparer) : base(comparer) { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(int capacity) : base(capacity) { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(IDictionary<Type, IFrameTemplate> dictionary, IEqualityComparer<Type> comparer) : base(dictionary, comparer) { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(IEnumerable<KeyValuePair<Type, IFrameTemplate>> collection, IEqualityComparer<Type> comparer) : base(collection, comparer) { }
        /// <inheritdoc/>
        public FrameTemplateDictionary(int capacity, IEqualityComparer<Type> comparer) : base(capacity, comparer) { }

        /// <inheritdoc/>
        public virtual FrameTemplateReadOnlyDictionary ToReadOnly()
        {
            return new FrameTemplateReadOnlyDictionary(this);
        }
    }
}
