#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxNodeFrameVisibility
    /// </summary>
    public interface IFocusNodeFrameVisibilityList : IList<IFocusNodeFrameVisibility>, IReadOnlyList<IFocusNodeFrameVisibility>
    {
        new int Count { get; }
        new IFocusNodeFrameVisibility this[int index] { get; set; }
        new IEnumerator<IFocusNodeFrameVisibility> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxNodeFrameVisibility
    /// </summary>
    internal class FocusNodeFrameVisibilityList : Collection<IFocusNodeFrameVisibility>, IFocusNodeFrameVisibilityList
    {
    }
}
