namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameNodeStateList : WriteableNodeStateList, ICollection<IFrameNodeState>, IEnumerable<IFrameNodeState>, IList<IFrameNodeState>, IReadOnlyCollection<IFrameNodeState>, IReadOnlyList<IFrameNodeState>
    {
        #region IFrameNodeState
        void ICollection<IFrameNodeState>.Add(IFrameNodeState item) { Add(item); }
        bool ICollection<IFrameNodeState>.Contains(IFrameNodeState item) { return Contains(item); }
        void ICollection<IFrameNodeState>.CopyTo(IFrameNodeState[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFrameNodeState>.Remove(IFrameNodeState item) { return Remove(item); }
        bool ICollection<IFrameNodeState>.IsReadOnly { get { return ((ICollection<IReadOnlyNodeState>)this).IsReadOnly; } }
        IEnumerator<IFrameNodeState> IEnumerable<IFrameNodeState>.GetEnumerator() { return ((IList<IFrameNodeState>)this).GetEnumerator(); }
        IFrameNodeState IList<IFrameNodeState>.this[int index] { get { return (IFrameNodeState)this[index]; } set { this[index] = value; } }
        int IList<IFrameNodeState>.IndexOf(IFrameNodeState item) { return IndexOf(item); }
        void IList<IFrameNodeState>.Insert(int index, IFrameNodeState item) { Insert(index, item); }
        IFrameNodeState IReadOnlyList<IFrameNodeState>.this[int index] { get { return (IFrameNodeState)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyNodeStateReadOnlyList ToReadOnly()
        {
            return new FrameNodeStateReadOnlyList(this);
        }
    }
}
