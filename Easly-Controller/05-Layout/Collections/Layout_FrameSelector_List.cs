namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutFrameSelectorList : FocusFrameSelectorList, ICollection<ILayoutFrameSelector>, IEnumerable<ILayoutFrameSelector>, IList<ILayoutFrameSelector>, IReadOnlyCollection<ILayoutFrameSelector>, IReadOnlyList<ILayoutFrameSelector>, IEqualComparable
    {
        /// <inheritdoc/>
        public new ILayoutFrameSelector this[int index] { get { return (ILayoutFrameSelector)base[index]; } set { base[index] = value; } }

        #region ILayoutFrameSelector
        void ICollection<ILayoutFrameSelector>.Add(ILayoutFrameSelector item) { Add(item); }
        bool ICollection<ILayoutFrameSelector>.Contains(ILayoutFrameSelector item) { return Contains(item); }
        void ICollection<ILayoutFrameSelector>.CopyTo(ILayoutFrameSelector[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutFrameSelector>.Remove(ILayoutFrameSelector item) { return Remove(item); }
        bool ICollection<ILayoutFrameSelector>.IsReadOnly { get { return ((ICollection<IFocusFrameSelector>)this).IsReadOnly; } }
        IEnumerator<ILayoutFrameSelector> IEnumerable<ILayoutFrameSelector>.GetEnumerator() { return ((IList<ILayoutFrameSelector>)this).GetEnumerator(); }
        ILayoutFrameSelector IList<ILayoutFrameSelector>.this[int index] { get { return (ILayoutFrameSelector)this[index]; } set { this[index] = value; } }
        int IList<ILayoutFrameSelector>.IndexOf(ILayoutFrameSelector item) { return IndexOf(item); }
        void IList<ILayoutFrameSelector>.Insert(int index, ILayoutFrameSelector item) { Insert(index, item); }
        ILayoutFrameSelector IReadOnlyList<ILayoutFrameSelector>.this[int index] { get { return (ILayoutFrameSelector)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutFrameSelectorList AsFrameSelectorList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsFrameSelectorList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsFrameSelectorList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion

        /// <inheritdoc/>
        public override FocusFrameSelectorReadOnlyList ToReadOnly()
        {
            return new LayoutFrameSelectorReadOnlyList(this);
        }
    }
}
