using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Frame
{
    /// <summary>
    /// Read-only dictionary of Type, IxxxTemplate
    /// </summary>
    public interface IFrameTemplateReadOnlyDictionary : IReadOnlyDictionary<Type, IFrameTemplate>
    {
    }

    /// <summary>
    /// Read-only dictionary of Type, IxxxTemplate
    /// </summary>
    public class FrameTemplateReadOnlyDictionary : ReadOnlyDictionary<Type, IFrameTemplate>, IFrameTemplateReadOnlyDictionary
    {
        public FrameTemplateReadOnlyDictionary(IFrameTemplateDictionary dictionary)
            : base(dictionary)
        {
        }
    }
}
