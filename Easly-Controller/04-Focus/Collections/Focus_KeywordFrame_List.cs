#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;

    /// <summary>
    /// List of IxxxKeywordFrame
    /// </summary>
    public interface IFocusKeywordFrameList : IFrameKeywordFrameList, IList<IFocusKeywordFrame>, IReadOnlyList<IFocusKeywordFrame>
    {
        new IFocusKeywordFrame this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IFocusKeywordFrame> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxKeywordFrame
    /// </summary>
    internal class FocusKeywordFrameList : Collection<IFocusKeywordFrame>, IFocusKeywordFrameList
    {
        #region Frame
        IFrameKeywordFrame IFrameKeywordFrameList.this[int index] { get { return this[index]; } set { this[index] = (IFocusKeywordFrame)value; } }
        IEnumerator<IFrameKeywordFrame> IFrameKeywordFrameList.GetEnumerator() { return GetEnumerator(); }
        IFrameKeywordFrame IList<IFrameKeywordFrame>.this[int index] { get { return this[index]; } set { this[index] = (IFocusKeywordFrame)value; } }
        int IList<IFrameKeywordFrame>.IndexOf(IFrameKeywordFrame value) { return IndexOf((IFocusKeywordFrame)value); }
        void IList<IFrameKeywordFrame>.Insert(int index, IFrameKeywordFrame item) { Insert(index, (IFocusKeywordFrame)item); }
        void ICollection<IFrameKeywordFrame>.Add(IFrameKeywordFrame item) { Add((IFocusKeywordFrame)item); }
        bool ICollection<IFrameKeywordFrame>.Contains(IFrameKeywordFrame value) { return Contains((IFocusKeywordFrame)value); }
        void ICollection<IFrameKeywordFrame>.CopyTo(IFrameKeywordFrame[] array, int index) { CopyTo((IFocusKeywordFrame[])array, index); }
        bool ICollection<IFrameKeywordFrame>.IsReadOnly { get { return ((ICollection<IFocusKeywordFrame>)this).IsReadOnly; } }
        bool ICollection<IFrameKeywordFrame>.Remove(IFrameKeywordFrame item) { return Remove((IFocusKeywordFrame)item); }
        IEnumerator<IFrameKeywordFrame> IEnumerable<IFrameKeywordFrame>.GetEnumerator() { return GetEnumerator(); }
        IFrameKeywordFrame IReadOnlyList<IFrameKeywordFrame>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
