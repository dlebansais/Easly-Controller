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
        new int Count { get; }
        new IFocusKeywordFrame this[int index] { get; set; }
        new IEnumerator<IFocusKeywordFrame> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxKeywordFrame
    /// </summary>
    internal class FocusKeywordFrameList : Collection<IFocusKeywordFrame>, IFocusKeywordFrameList
    {
        #region Frame
        IFrameKeywordFrame IFrameKeywordFrameList.this[int index] { get { return this[index]; } set { this[index] = (IFocusKeywordFrame)value; } }
        IFrameKeywordFrame IList<IFrameKeywordFrame>.this[int index] { get { return this[index]; } set { this[index] = (IFocusKeywordFrame)value; } }
        IFrameKeywordFrame IReadOnlyList<IFrameKeywordFrame>.this[int index] { get { return this[index]; } }
        public void Add(IFrameKeywordFrame item) { base.Add((IFocusKeywordFrame)item); }
        public void Insert(int index, IFrameKeywordFrame item) { base.Insert(index, (IFocusKeywordFrame)item); }
        public bool Remove(IFrameKeywordFrame item) { return base.Remove((IFocusKeywordFrame)item); }
        public void CopyTo(IFrameKeywordFrame[] array, int index) { base.CopyTo((IFocusKeywordFrame[])array, index); }
        bool ICollection<IFrameKeywordFrame>.IsReadOnly { get { return ((ICollection<IFocusKeywordFrame>)this).IsReadOnly; } }
        public bool Contains(IFrameKeywordFrame value) { return base.Contains((IFocusKeywordFrame)value); }
        public int IndexOf(IFrameKeywordFrame value) { return base.IndexOf((IFocusKeywordFrame)value); }
        IEnumerator<IFrameKeywordFrame> IFrameKeywordFrameList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameKeywordFrame> IEnumerable<IFrameKeywordFrame>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
