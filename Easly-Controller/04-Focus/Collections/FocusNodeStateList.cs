namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class FocusNodeStateList : FrameNodeStateList, ICollection<IFocusNodeState>, IEnumerable<IFocusNodeState>, IList<IFocusNodeState>, IReadOnlyCollection<IFocusNodeState>, IReadOnlyList<IFocusNodeState>
    {
        /// <inheritdoc/>
        public new IFocusNodeState this[int index] { get { return (IFocusNodeState)base[index]; } set { base[index] = value; } }

        #region IFocusNodeState
        void ICollection<IFocusNodeState>.Add(IFocusNodeState item) { Add(item); }
        bool ICollection<IFocusNodeState>.Contains(IFocusNodeState item) { return Contains(item); }
        void ICollection<IFocusNodeState>.CopyTo(IFocusNodeState[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFocusNodeState>.Remove(IFocusNodeState item) { return Remove(item); }
        bool ICollection<IFocusNodeState>.IsReadOnly { get { return ((ICollection<IFrameNodeState>)this).IsReadOnly; } }
        IEnumerator<IFocusNodeState> IEnumerable<IFocusNodeState>.GetEnumerator() { var iterator = ((List<IReadOnlyNodeState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusNodeState)iterator.Current; } }
        IFocusNodeState IList<IFocusNodeState>.this[int index] { get { return (IFocusNodeState)this[index]; } set { this[index] = value; } }
        int IList<IFocusNodeState>.IndexOf(IFocusNodeState item) { return IndexOf(item); }
        void IList<IFocusNodeState>.Insert(int index, IFocusNodeState item) { Insert(index, item); }
        IFocusNodeState IReadOnlyList<IFocusNodeState>.this[int index] { get { return (IFocusNodeState)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyNodeStateReadOnlyList ToReadOnly()
        {
            return new FocusNodeStateReadOnlyList(this);
        }
    }
}
