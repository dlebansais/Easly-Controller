using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.Frame
{
    /// <summary>
    /// Read-only list of IxxxCellView
    /// </summary>
    public interface IFrameCellViewReadOnlyList : IReadOnlyList<IFrameCellView>
    {
        bool Contains(IFrameCellView value);
        int IndexOf(IFrameCellView value);
    }

    /// <summary>
    /// Read-only list of IxxxCellView
    /// </summary>
    public class FrameCellViewReadOnlyList : ReadOnlyCollection<IFrameCellView>, IFrameCellViewReadOnlyList
    {
        public FrameCellViewReadOnlyList(IFrameCellViewList list)
            : base(list)
        {
        }
    }
}
