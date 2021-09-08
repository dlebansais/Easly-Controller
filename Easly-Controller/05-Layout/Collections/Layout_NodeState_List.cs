﻿namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutNodeStateList : FocusNodeStateList, ICollection<ILayoutNodeState>, IEnumerable<ILayoutNodeState>, IList<ILayoutNodeState>, IReadOnlyCollection<ILayoutNodeState>, IReadOnlyList<ILayoutNodeState>
    {
        /// <inheritdoc/>
        public new ILayoutNodeState this[int index] { get { return (ILayoutNodeState)base[index]; } set { base[index] = value; } }

        #region ILayoutNodeState
        void ICollection<ILayoutNodeState>.Add(ILayoutNodeState item) { Add(item); }
        bool ICollection<ILayoutNodeState>.Contains(ILayoutNodeState item) { return Contains(item); }
        void ICollection<ILayoutNodeState>.CopyTo(ILayoutNodeState[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutNodeState>.Remove(ILayoutNodeState item) { return Remove(item); }
        bool ICollection<ILayoutNodeState>.IsReadOnly { get { return ((ICollection<IReadOnlyNodeState>)this).IsReadOnly; } }
        IEnumerator<ILayoutNodeState> IEnumerable<ILayoutNodeState>.GetEnumerator() { return ((IList<ILayoutNodeState>)this).GetEnumerator(); }
        ILayoutNodeState IList<ILayoutNodeState>.this[int index] { get { return (ILayoutNodeState)this[index]; } set { this[index] = value; } }
        int IList<ILayoutNodeState>.IndexOf(ILayoutNodeState item) { return IndexOf(item); }
        void IList<ILayoutNodeState>.Insert(int index, ILayoutNodeState item) { Insert(index, item); }
        ILayoutNodeState IReadOnlyList<ILayoutNodeState>.this[int index] { get { return (ILayoutNodeState)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyNodeStateReadOnlyList ToReadOnly()
        {
            return new LayoutNodeStateReadOnlyList(this);
        }
    }
}
