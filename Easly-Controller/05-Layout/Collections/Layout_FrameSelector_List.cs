#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;

    /// <summary>
    /// List of IxxxFrameSelector
    /// </summary>
    public interface ILayoutFrameSelectorList : IFocusFrameSelectorList, IList<ILayoutFrameSelector>, IReadOnlyList<ILayoutFrameSelector>
    {
        new ILayoutFrameSelector this[int index] { get; set; }
        new int Count { get; }
    }

    /// <summary>
    /// List of IxxxFrameSelector
    /// </summary>
    internal class LayoutFrameSelectorList : Collection<ILayoutFrameSelector>, ILayoutFrameSelectorList
    {
        #region Focus
        IFocusFrameSelector IFocusFrameSelectorList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutFrameSelector)value; } }
        IFocusFrameSelector IList<IFocusFrameSelector>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutFrameSelector)value; } }
        int IList<IFocusFrameSelector>.IndexOf(IFocusFrameSelector value) { return IndexOf((ILayoutFrameSelector)value); }
        void IList<IFocusFrameSelector>.Insert(int index, IFocusFrameSelector item) { Insert(index, (ILayoutFrameSelector)item); }
        void ICollection<IFocusFrameSelector>.Add(IFocusFrameSelector item) { Add((ILayoutFrameSelector)item); }
        bool ICollection<IFocusFrameSelector>.Contains(IFocusFrameSelector value) { return Contains((ILayoutFrameSelector)value); }
        void ICollection<IFocusFrameSelector>.CopyTo(IFocusFrameSelector[] array, int index) { CopyTo((ILayoutFrameSelector[])array, index); }
        bool ICollection<IFocusFrameSelector>.IsReadOnly { get { return ((ICollection<ILayoutFrameSelector>)this).IsReadOnly; } }
        bool ICollection<IFocusFrameSelector>.Remove(IFocusFrameSelector item) { return Remove((ILayoutFrameSelector)item); }
        IEnumerator<IFocusFrameSelector> IEnumerable<IFocusFrameSelector>.GetEnumerator() { return GetEnumerator(); }
        IFocusFrameSelector IReadOnlyList<IFocusFrameSelector>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
