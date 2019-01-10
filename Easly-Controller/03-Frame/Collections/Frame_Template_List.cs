using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Frame
{
    /// <summary>
    /// List of IxxxTemplate
    /// </summary>
    public interface IFrameTemplateList : IList<IFrameTemplate>, IReadOnlyList<IFrameTemplate>
    {
        new int Count { get; }
        new IFrameTemplate this[int index] { get; set; }
        new IEnumerator<IFrameTemplate> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxTemplate
    /// </summary>
    public class FrameTemplateList : Collection<IFrameTemplate>, IFrameTemplateList
    {
    }
}
