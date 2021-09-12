namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FocusOperationReadOnlyList : FrameOperationReadOnlyList, IReadOnlyCollection<IFocusOperation>, IReadOnlyList<IFocusOperation>
    {
        /// <inheritdoc/>
        public FocusOperationReadOnlyList(FocusOperationList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IFocusOperation this[int index] { get { return (IFocusOperation)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<IFocusOperation> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IWriteableOperation>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusOperation)iterator.Current; } }

        #region IFocusOperation
        IEnumerator<IFocusOperation> IEnumerable<IFocusOperation>.GetEnumerator() { return GetEnumerator(); }
        IFocusOperation IReadOnlyList<IFocusOperation>.this[int index] { get { return (IFocusOperation)this[index]; } }
        #endregion
    }
}
