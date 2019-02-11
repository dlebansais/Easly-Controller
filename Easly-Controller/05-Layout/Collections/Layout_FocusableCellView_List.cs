#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;

    /// <summary>
    /// List of IxxxFocusableCellView
    /// </summary>
    public interface ILayoutFocusableCellViewList : IFocusFocusableCellViewList, IList<ILayoutFocusableCellView>, IReadOnlyList<ILayoutFocusableCellView>
    {
        new ILayoutFocusableCellView this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutFocusableCellView> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxFocusableCellView
    /// </summary>
    internal class LayoutFocusableCellViewList : Collection<ILayoutFocusableCellView>, ILayoutFocusableCellViewList
    {
        #region Focus
        IFocusFocusableCellView IFocusFocusableCellViewList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutFocusableCellView)value; } }
        IFocusFocusableCellView IList<IFocusFocusableCellView>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutFocusableCellView)value; } }
        int IList<IFocusFocusableCellView>.IndexOf(IFocusFocusableCellView value) { return IndexOf((ILayoutFocusableCellView)value); }
        void IList<IFocusFocusableCellView>.Insert(int index, IFocusFocusableCellView item) { Insert(index, (ILayoutFocusableCellView)item); }
        void ICollection<IFocusFocusableCellView>.Add(IFocusFocusableCellView item) { Add((ILayoutFocusableCellView)item); }
        bool ICollection<IFocusFocusableCellView>.Contains(IFocusFocusableCellView value) { return Contains((ILayoutFocusableCellView)value); }
        void ICollection<IFocusFocusableCellView>.CopyTo(IFocusFocusableCellView[] array, int index) { CopyTo((ILayoutFocusableCellView[])array, index); }
        bool ICollection<IFocusFocusableCellView>.IsReadOnly { get { return ((ICollection<ILayoutFocusableCellView>)this).IsReadOnly; } }
        bool ICollection<IFocusFocusableCellView>.Remove(IFocusFocusableCellView item) { return Remove((ILayoutFocusableCellView)item); }
        IEnumerator<IFocusFocusableCellView> IEnumerable<IFocusFocusableCellView>.GetEnumerator() { return GetEnumerator(); }
        IFocusFocusableCellView IReadOnlyList<IFocusFocusableCellView>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
