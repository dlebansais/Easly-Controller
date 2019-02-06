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
        new IFocusNodeFrameVisibility this[int index] { get; set; }
        new int Count { get; }
    }

    /// <summary>
    /// List of IxxxNodeFrameVisibility
    /// </summary>
    internal class FocusNodeFrameVisibilityList : Collection<IFocusNodeFrameVisibility>, IFocusNodeFrameVisibilityList
    {
    }
}
