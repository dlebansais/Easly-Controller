#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxSelectableFrame
    /// </summary>
    public interface IFocusSelectableFrameList : IList<IFocusSelectableFrame>, IReadOnlyList<IFocusSelectableFrame>
    {
        new IFocusSelectableFrame this[int index] { get; set; }
        new int Count { get; }
    }

    /// <summary>
    /// List of IxxxSelectableFrame
    /// </summary>
    internal class FocusSelectableFrameList : Collection<IFocusSelectableFrame>, IFocusSelectableFrameList
    {
    }
}
