#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// List of IxxxKeywordFrame
    /// </summary>
    public interface ILayoutKeywordFrameList : IFocusKeywordFrameList, IList<ILayoutKeywordFrame>, IReadOnlyList<ILayoutKeywordFrame>
    {
        new ILayoutKeywordFrame this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutKeywordFrame> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxKeywordFrame
    /// </summary>
    internal class LayoutKeywordFrameList : Collection<ILayoutKeywordFrame>, ILayoutKeywordFrameList
    {
        #region Frame
        IFrameKeywordFrame IFrameKeywordFrameList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutKeywordFrame)value; } }
        IFrameKeywordFrame IList<IFrameKeywordFrame>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutKeywordFrame)value; } }
        int IList<IFrameKeywordFrame>.IndexOf(IFrameKeywordFrame value) { return IndexOf((ILayoutKeywordFrame)value); }
        void IList<IFrameKeywordFrame>.Insert(int index, IFrameKeywordFrame item) { Insert(index, (ILayoutKeywordFrame)item); }
        void ICollection<IFrameKeywordFrame>.Add(IFrameKeywordFrame item) { Add((ILayoutKeywordFrame)item); }
        bool ICollection<IFrameKeywordFrame>.Contains(IFrameKeywordFrame value) { return Contains((ILayoutKeywordFrame)value); }
        void ICollection<IFrameKeywordFrame>.CopyTo(IFrameKeywordFrame[] array, int index) { CopyTo((ILayoutKeywordFrame[])array, index); }
        bool ICollection<IFrameKeywordFrame>.IsReadOnly { get { return ((ICollection<ILayoutKeywordFrame>)this).IsReadOnly; } }
        bool ICollection<IFrameKeywordFrame>.Remove(IFrameKeywordFrame item) { return Remove((ILayoutKeywordFrame)item); }
        IEnumerator<IFrameKeywordFrame> IEnumerable<IFrameKeywordFrame>.GetEnumerator() { return GetEnumerator(); }
        IFrameKeywordFrame IReadOnlyList<IFrameKeywordFrame>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusKeywordFrame IFocusKeywordFrameList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutKeywordFrame)value; } }
        IEnumerator<IFocusKeywordFrame> IFocusKeywordFrameList.GetEnumerator() { return GetEnumerator(); }
        IFocusKeywordFrame IList<IFocusKeywordFrame>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutKeywordFrame)value; } }
        int IList<IFocusKeywordFrame>.IndexOf(IFocusKeywordFrame value) { return IndexOf((ILayoutKeywordFrame)value); }
        void IList<IFocusKeywordFrame>.Insert(int index, IFocusKeywordFrame item) { Insert(index, (ILayoutKeywordFrame)item); }
        void ICollection<IFocusKeywordFrame>.Add(IFocusKeywordFrame item) { Add((ILayoutKeywordFrame)item); }
        bool ICollection<IFocusKeywordFrame>.Contains(IFocusKeywordFrame value) { return Contains((ILayoutKeywordFrame)value); }
        void ICollection<IFocusKeywordFrame>.CopyTo(IFocusKeywordFrame[] array, int index) { CopyTo((ILayoutKeywordFrame[])array, index); }
        bool ICollection<IFocusKeywordFrame>.IsReadOnly { get { return ((ICollection<ILayoutKeywordFrame>)this).IsReadOnly; } }
        bool ICollection<IFocusKeywordFrame>.Remove(IFocusKeywordFrame item) { return Remove((ILayoutKeywordFrame)item); }
        IEnumerator<IFocusKeywordFrame> IEnumerable<IFocusKeywordFrame>.GetEnumerator() { return GetEnumerator(); }
        IFocusKeywordFrame IReadOnlyList<IFocusKeywordFrame>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
