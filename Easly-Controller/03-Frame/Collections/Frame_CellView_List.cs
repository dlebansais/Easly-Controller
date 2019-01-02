using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.Frame
{
    /// <summary>
    /// List of IxxxCellView
    /// </summary>
    public interface IFrameCellViewList : IList<IFrameCellView>, IReadOnlyList<IFrameCellView>
    {
        new int Count { get; }
        new IFrameCellView this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxCellView
    /// </summary>
    public class FrameCellViewList : Collection<IFrameCellView>, IFrameCellViewList
    {
    }
}
