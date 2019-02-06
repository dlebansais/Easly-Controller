#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxVisibleCellView
    /// </summary>
    public interface IFrameVisibleCellViewList : IList<IFrameVisibleCellView>, IReadOnlyList<IFrameVisibleCellView>
    {
        new IFrameVisibleCellView this[int index] { get; set; }
        new int Count { get; }
    }

    /// <summary>
    /// List of IxxxVisibleCellView
    /// </summary>
    public class FrameVisibleCellViewList : Collection<IFrameVisibleCellView>, IFrameVisibleCellViewList
    {
    }
}
