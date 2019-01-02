using System;
using System.Collections.Generic;

namespace EaslyController.Frame
{
    /// <summary>
    /// Dictionary of Type, IxxxTemplate
    /// </summary>
    public interface IFrameTemplateDictionary : IDictionary<Type, IFrameTemplate>
    {
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
    }
}
