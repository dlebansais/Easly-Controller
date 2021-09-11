namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

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

        #region IFocusOperation
        IEnumerator<IFocusOperation> IEnumerable<IFocusOperation>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusOperation)iterator.Current; } }
        IFocusOperation IReadOnlyList<IFocusOperation>.this[int index] { get { return (IFocusOperation)this[index]; } }
        #endregion
    }
}
