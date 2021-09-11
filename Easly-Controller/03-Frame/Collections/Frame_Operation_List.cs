﻿namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameOperationList : WriteableOperationList, ICollection<FrameOperation>, IEnumerable<FrameOperation>, IList<FrameOperation>, IReadOnlyCollection<FrameOperation>, IReadOnlyList<FrameOperation>
    {
        /// <inheritdoc/>
        public new FrameOperation this[int index] { get { return (FrameOperation)base[index]; } set { base[index] = value; } }

        #region FrameOperation
        void ICollection<FrameOperation>.Add(FrameOperation item) { Add(item); }
        bool ICollection<FrameOperation>.Contains(FrameOperation item) { return Contains(item); }
        void ICollection<FrameOperation>.CopyTo(FrameOperation[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<FrameOperation>.Remove(FrameOperation item) { return Remove(item); }
        bool ICollection<FrameOperation>.IsReadOnly { get { return ((ICollection<IWriteableOperation>)this).IsReadOnly; } }
        IEnumerator<FrameOperation> IEnumerable<FrameOperation>.GetEnumerator() { Enumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (FrameOperation)iterator.Current; } }
        FrameOperation IList<FrameOperation>.this[int index] { get { return (FrameOperation)this[index]; } set { this[index] = value; } }
        int IList<FrameOperation>.IndexOf(FrameOperation item) { return IndexOf(item); }
        void IList<FrameOperation>.Insert(int index, FrameOperation item) { Insert(index, item); }
        FrameOperation IReadOnlyList<FrameOperation>.this[int index] { get { return (FrameOperation)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override WriteableOperationReadOnlyList ToReadOnly()
        {
            return new FrameOperationReadOnlyList(this);
        }
    }
}
