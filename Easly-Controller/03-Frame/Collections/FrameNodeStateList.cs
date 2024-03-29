﻿namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameNodeStateList : WriteableNodeStateList, ICollection<IFrameNodeState>, IEnumerable<IFrameNodeState>, IList<IFrameNodeState>, IReadOnlyCollection<IFrameNodeState>, IReadOnlyList<IFrameNodeState>
    {
        /// <inheritdoc/>
        public new IFrameNodeState this[int index] { get { return (IFrameNodeState)base[index]; } set { base[index] = value; } }

        #region IFrameNodeState
        void ICollection<IFrameNodeState>.Add(IFrameNodeState item) { Add(item); }
        bool ICollection<IFrameNodeState>.Contains(IFrameNodeState item) { return Contains(item); }
        void ICollection<IFrameNodeState>.CopyTo(IFrameNodeState[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFrameNodeState>.Remove(IFrameNodeState item) { return Remove(item); }
        bool ICollection<IFrameNodeState>.IsReadOnly { get { return ((ICollection<IWriteableNodeState>)this).IsReadOnly; } }
        IEnumerator<IFrameNodeState> IEnumerable<IFrameNodeState>.GetEnumerator() { var iterator = ((List<IReadOnlyNodeState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFrameNodeState)iterator.Current; } }
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
