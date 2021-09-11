namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusOperationReadOnlyList : FrameOperationReadOnlyList, IReadOnlyCollection<FocusOperation>, IReadOnlyList<FocusOperation>
    {
        /// <inheritdoc/>
        public FocusOperationReadOnlyList(FocusOperationList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new FocusOperation this[int index] { get { return (FocusOperation)base[index]; } }

        #region FocusOperation
        IEnumerator<FocusOperation> IEnumerable<FocusOperation>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (FocusOperation)iterator.Current; } }
        FocusOperation IReadOnlyList<FocusOperation>.this[int index] { get { return (FocusOperation)this[index]; } }
        #endregion
    }
}
