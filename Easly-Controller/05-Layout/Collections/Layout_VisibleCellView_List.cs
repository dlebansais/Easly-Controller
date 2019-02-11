#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// List of IxxxVisibleCellView
    /// </summary>
    public interface ILayoutVisibleCellViewList : IFocusVisibleCellViewList, IList<ILayoutVisibleCellView>, IReadOnlyList<ILayoutVisibleCellView>
    {
        new ILayoutVisibleCellView this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutVisibleCellView> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxVisibleCellView
    /// </summary>
    public class LayoutVisibleCellViewList : Collection<ILayoutVisibleCellView>, ILayoutVisibleCellViewList
    {
        #region Frame
        IFrameVisibleCellView IFrameVisibleCellViewList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutVisibleCellView)value; } }
        IFrameVisibleCellView IList<IFrameVisibleCellView>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutVisibleCellView)value; } }
        int IList<IFrameVisibleCellView>.IndexOf(IFrameVisibleCellView value) { return IndexOf((ILayoutVisibleCellView)value); }
        void IList<IFrameVisibleCellView>.Insert(int index, IFrameVisibleCellView item) { Insert(index, (ILayoutVisibleCellView)item); }
        void ICollection<IFrameVisibleCellView>.Add(IFrameVisibleCellView item) { Add((ILayoutVisibleCellView)item); }
        bool ICollection<IFrameVisibleCellView>.Contains(IFrameVisibleCellView value) { return Contains((ILayoutVisibleCellView)value); }
        void ICollection<IFrameVisibleCellView>.CopyTo(IFrameVisibleCellView[] array, int index) { CopyTo((ILayoutVisibleCellView[])array, index); }
        bool ICollection<IFrameVisibleCellView>.IsReadOnly { get { return ((ICollection<ILayoutVisibleCellView>)this).IsReadOnly; } }
        bool ICollection<IFrameVisibleCellView>.Remove(IFrameVisibleCellView item) { return Remove((ILayoutVisibleCellView)item); }
        IEnumerator<IFrameVisibleCellView> IEnumerable<IFrameVisibleCellView>.GetEnumerator() { return GetEnumerator(); }
        IFrameVisibleCellView IReadOnlyList<IFrameVisibleCellView>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusVisibleCellView IFocusVisibleCellViewList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutVisibleCellView)value; } }
        IEnumerator<IFocusVisibleCellView> IFocusVisibleCellViewList.GetEnumerator() { return GetEnumerator(); }
        IFocusVisibleCellView IList<IFocusVisibleCellView>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutVisibleCellView)value; } }
        int IList<IFocusVisibleCellView>.IndexOf(IFocusVisibleCellView value) { return IndexOf((ILayoutVisibleCellView)value); }
        void IList<IFocusVisibleCellView>.Insert(int index, IFocusVisibleCellView item) { Insert(index, (ILayoutVisibleCellView)item); }
        void ICollection<IFocusVisibleCellView>.Add(IFocusVisibleCellView item) { Add((ILayoutVisibleCellView)item); }
        bool ICollection<IFocusVisibleCellView>.Contains(IFocusVisibleCellView value) { return Contains((ILayoutVisibleCellView)value); }
        void ICollection<IFocusVisibleCellView>.CopyTo(IFocusVisibleCellView[] array, int index) { CopyTo((ILayoutVisibleCellView[])array, index); }
        bool ICollection<IFocusVisibleCellView>.IsReadOnly { get { return ((ICollection<ILayoutVisibleCellView>)this).IsReadOnly; } }
        bool ICollection<IFocusVisibleCellView>.Remove(IFocusVisibleCellView item) { return Remove((ILayoutVisibleCellView)item); }
        IEnumerator<IFocusVisibleCellView> IEnumerable<IFocusVisibleCellView>.GetEnumerator() { return GetEnumerator(); }
        IFocusVisibleCellView IReadOnlyList<IFocusVisibleCellView>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
