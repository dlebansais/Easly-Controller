namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class LayoutKeywordFrameList : FocusKeywordFrameList, ICollection<ILayoutKeywordFrame>, IEnumerable<ILayoutKeywordFrame>, IList<ILayoutKeywordFrame>, IReadOnlyCollection<ILayoutKeywordFrame>, IReadOnlyList<ILayoutKeywordFrame>
    {
        /// <inheritdoc/>
        public new ILayoutKeywordFrame this[int index] { get { return (ILayoutKeywordFrame)base[index]; } set { base[index] = value; } }

        #region ILayoutKeywordFrame
        void ICollection<ILayoutKeywordFrame>.Add(ILayoutKeywordFrame item) { Add(item); }
        bool ICollection<ILayoutKeywordFrame>.Contains(ILayoutKeywordFrame item) { return Contains(item); }
        void ICollection<ILayoutKeywordFrame>.CopyTo(ILayoutKeywordFrame[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutKeywordFrame>.Remove(ILayoutKeywordFrame item) { return Remove(item); }
        bool ICollection<ILayoutKeywordFrame>.IsReadOnly { get { return ((ICollection<IFocusKeywordFrame>)this).IsReadOnly; } }
        IEnumerator<ILayoutKeywordFrame> IEnumerable<ILayoutKeywordFrame>.GetEnumerator() { Enumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutKeywordFrame)iterator.Current; } }
        ILayoutKeywordFrame IList<ILayoutKeywordFrame>.this[int index] { get { return (ILayoutKeywordFrame)this[index]; } set { this[index] = value; } }
        int IList<ILayoutKeywordFrame>.IndexOf(ILayoutKeywordFrame item) { return IndexOf(item); }
        void IList<ILayoutKeywordFrame>.Insert(int index, ILayoutKeywordFrame item) { Insert(index, item); }
        ILayoutKeywordFrame IReadOnlyList<ILayoutKeywordFrame>.this[int index] { get { return (ILayoutKeywordFrame)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override FrameKeywordFrameReadOnlyList ToReadOnly()
        {
            return new LayoutKeywordFrameReadOnlyList(this);
        }
    }
}
