namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameOperationReadOnlyList : WriteableOperationReadOnlyList, IReadOnlyCollection<IFrameOperation>, IReadOnlyList<IFrameOperation>
    {
        /// <inheritdoc/>
        public FrameOperationReadOnlyList(FrameOperationList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IFrameOperation this[int index] { get { return (IFrameOperation)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<IFrameOperation> GetEnumerator() { var iterator = ((ReadOnlyCollection<IWriteableOperation>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFrameOperation)iterator.Current; } }

        #region IFrameOperation
        IEnumerator<IFrameOperation> IEnumerable<IFrameOperation>.GetEnumerator() { return GetEnumerator(); }
        IFrameOperation IReadOnlyList<IFrameOperation>.this[int index] { get { return (IFrameOperation)this[index]; } }
        #endregion
    }
}
