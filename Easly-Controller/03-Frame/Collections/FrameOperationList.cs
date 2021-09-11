namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameOperationList : WriteableOperationList, ICollection<IFrameOperation>, IEnumerable<IFrameOperation>, IList<IFrameOperation>, IReadOnlyCollection<IFrameOperation>, IReadOnlyList<IFrameOperation>
    {
        /// <inheritdoc/>
        public new IFrameOperation this[int index] { get { return (IFrameOperation)base[index]; } set { base[index] = value; } }

        #region IFrameOperation
        void ICollection<IFrameOperation>.Add(IFrameOperation item) { Add(item); }
        bool ICollection<IFrameOperation>.Contains(IFrameOperation item) { return Contains(item); }
        void ICollection<IFrameOperation>.CopyTo(IFrameOperation[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFrameOperation>.Remove(IFrameOperation item) { return Remove(item); }
        bool ICollection<IFrameOperation>.IsReadOnly { get { return ((ICollection<IWriteableOperation>)this).IsReadOnly; } }
        IEnumerator<IFrameOperation> IEnumerable<IFrameOperation>.GetEnumerator() { Enumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFrameOperation)iterator.Current; } }
        IFrameOperation IList<IFrameOperation>.this[int index] { get { return (IFrameOperation)this[index]; } set { this[index] = value; } }
        int IList<IFrameOperation>.IndexOf(IFrameOperation item) { return IndexOf(item); }
        void IList<IFrameOperation>.Insert(int index, IFrameOperation item) { Insert(index, item); }
        IFrameOperation IReadOnlyList<IFrameOperation>.this[int index] { get { return (IFrameOperation)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override WriteableOperationReadOnlyList ToReadOnly()
        {
            return new FrameOperationReadOnlyList(this);
        }
    }
}
