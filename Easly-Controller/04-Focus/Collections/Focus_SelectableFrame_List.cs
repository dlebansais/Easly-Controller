using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Focus
{
    /// <summary>
    /// List of IxxxSelectableFrame
    /// </summary>
    public interface IFocusSelectableFrameList : IList<IFocusSelectableFrame>, IReadOnlyList<IFocusSelectableFrame>
    {
        new int Count { get; }
        new IFocusSelectableFrame this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxSelectableFrame
    /// </summary>
    public class FocusSelectableFrameList : Collection<IFocusSelectableFrame>, IFocusSelectableFrameList
    {
    }
}
