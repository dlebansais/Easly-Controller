namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusKeywordFrameList : FrameKeywordFrameList, ICollection<IFocusKeywordFrame>, IEnumerable<IFocusKeywordFrame>, IList<IFocusKeywordFrame>, IReadOnlyCollection<IFocusKeywordFrame>, IReadOnlyList<IFocusKeywordFrame>
    {
        /// <inheritdoc/>
        public new IFocusKeywordFrame this[int index] { get { return (IFocusKeywordFrame)base[index]; } set { base[index] = value; } }

        #region IFocusKeywordFrame
        void ICollection<IFocusKeywordFrame>.Add(IFocusKeywordFrame item) { Add(item); }
        bool ICollection<IFocusKeywordFrame>.Contains(IFocusKeywordFrame item) { return Contains(item); }
        void ICollection<IFocusKeywordFrame>.CopyTo(IFocusKeywordFrame[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFocusKeywordFrame>.Remove(IFocusKeywordFrame item) { return Remove(item); }
        bool ICollection<IFocusKeywordFrame>.IsReadOnly { get { return ((ICollection<IFrameKeywordFrame>)this).IsReadOnly; } }
        IEnumerator<IFocusKeywordFrame> IEnumerable<IFocusKeywordFrame>.GetEnumerator() { var iterator = ((List<IFrameKeywordFrame>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusKeywordFrame)iterator.Current; } }
        IFocusKeywordFrame IList<IFocusKeywordFrame>.this[int index] { get { return (IFocusKeywordFrame)this[index]; } set { this[index] = value; } }
        int IList<IFocusKeywordFrame>.IndexOf(IFocusKeywordFrame item) { return IndexOf(item); }
        void IList<IFocusKeywordFrame>.Insert(int index, IFocusKeywordFrame item) { Insert(index, item); }
        IFocusKeywordFrame IReadOnlyList<IFocusKeywordFrame>.this[int index] { get { return (IFocusKeywordFrame)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override FrameKeywordFrameReadOnlyList ToReadOnly()
        {
            return new FocusKeywordFrameReadOnlyList(this);
        }
    }
}
