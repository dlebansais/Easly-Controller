#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// List of IxxxFocus
    /// </summary>
    public interface ILayoutFrameList : IFocusFrameList, IList<ILayoutFrame>, IReadOnlyList<ILayoutFrame>
    {
        new ILayoutFrame this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutFrame> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxFocus
    /// </summary>
    internal class LayoutFrameList : Collection<ILayoutFrame>, ILayoutFrameList
    {
        #region Frame
        IFrameFrame IFrameFrameList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutFrame)value; } }
        IFrameFrame IList<IFrameFrame>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutFrame)value; } }
        int IList<IFrameFrame>.IndexOf(IFrameFrame value) { return IndexOf((ILayoutFrame)value); }
        void IList<IFrameFrame>.Insert(int index, IFrameFrame item) { Insert(index, (ILayoutFrame)item); }
        void ICollection<IFrameFrame>.Add(IFrameFrame item) { Add((ILayoutFrame)item); }
        bool ICollection<IFrameFrame>.Contains(IFrameFrame value) { return Contains((ILayoutFrame)value); }
        void ICollection<IFrameFrame>.CopyTo(IFrameFrame[] array, int index) { CopyTo((ILayoutFrame[])array, index); }
        bool ICollection<IFrameFrame>.IsReadOnly { get { return ((ICollection<ILayoutFrame>)this).IsReadOnly; } }
        bool ICollection<IFrameFrame>.Remove(IFrameFrame item) { return Remove((ILayoutFrame)item); }
        IEnumerator<IFrameFrame> IEnumerable<IFrameFrame>.GetEnumerator() { return GetEnumerator(); }
        IFrameFrame IReadOnlyList<IFrameFrame>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusFrame IFocusFrameList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutFrame)value; } }
        IEnumerator<IFocusFrame> IFocusFrameList.GetEnumerator() { return GetEnumerator(); }
        IFocusFrame IList<IFocusFrame>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutFrame)value; } }
        int IList<IFocusFrame>.IndexOf(IFocusFrame value) { return IndexOf((ILayoutFrame)value); }
        void IList<IFocusFrame>.Insert(int index, IFocusFrame item) { Insert(index, (ILayoutFrame)item); }
        void ICollection<IFocusFrame>.Add(IFocusFrame item) { Add((ILayoutFrame)item); }
        bool ICollection<IFocusFrame>.Contains(IFocusFrame value) { return Contains((ILayoutFrame)value); }
        void ICollection<IFocusFrame>.CopyTo(IFocusFrame[] array, int index) { CopyTo((ILayoutFrame[])array, index); }
        bool ICollection<IFocusFrame>.IsReadOnly { get { return ((ICollection<ILayoutFrame>)this).IsReadOnly; } }
        bool ICollection<IFocusFrame>.Remove(IFocusFrame item) { return Remove((ILayoutFrame)item); }
        IEnumerator<IFocusFrame> IEnumerable<IFocusFrame>.GetEnumerator() { return GetEnumerator(); }
        IFocusFrame IReadOnlyList<IFocusFrame>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
