#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Dictionary of Type, IxxxTemplate
    /// </summary>
    public interface IFrameTemplateDictionary : IDictionary<Type, IFrameTemplate>
    {
        /// <summary>
        /// Gets a read-only view of the dictionary.
        /// </summary>
        IFrameTemplateReadOnlyDictionary ToReadOnly();
    }

    /// <summary>
    /// Dictionary of Type, IxxxTemplate
    /// </summary>
    public class FrameTemplateDictionary : Dictionary<Type, IFrameTemplate>, IFrameTemplateDictionary
    {
        public FrameTemplateDictionary()
        {
        }

        public FrameTemplateDictionary(IDictionary<Type, IFrameTemplate> dictionary)
            : base(dictionary)
        {
        }

        /// <summary>
        /// Gets a read-only view of the dictionary.
        /// </summary>
        public virtual IFrameTemplateReadOnlyDictionary ToReadOnly()
        {
            return new FrameTemplateReadOnlyDictionary(this);
        }
    }
}
