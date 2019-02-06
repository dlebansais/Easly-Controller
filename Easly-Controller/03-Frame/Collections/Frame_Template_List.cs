#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxTemplate
    /// </summary>
    public interface IFrameTemplateList : IList<IFrameTemplate>, IReadOnlyList<IFrameTemplate>
    {
        new IFrameTemplate this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IFrameTemplate> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxTemplate
    /// </summary>
    public class FrameTemplateList : Collection<IFrameTemplate>, IFrameTemplateList
    {
    }
}
