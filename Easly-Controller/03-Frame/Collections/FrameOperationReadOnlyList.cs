namespace EaslyController.Frame
{
    using System.Collections.Generic;
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

        #region IFrameOperation
        IEnumerator<IFrameOperation> IEnumerable<IFrameOperation>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFrameOperation)iterator.Current; } }
        IFrameOperation IReadOnlyList<IFrameOperation>.this[int index] { get { return (IFrameOperation)this[index]; } }
        #endregion
    }
}
