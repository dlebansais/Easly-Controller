namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class LayoutOperationReadOnlyList : FocusOperationReadOnlyList, IReadOnlyCollection<ILayoutOperation>, IReadOnlyList<ILayoutOperation>
    {
        /// <inheritdoc/>
        public LayoutOperationReadOnlyList(LayoutOperationList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new ILayoutOperation this[int index] { get { return (ILayoutOperation)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<ILayoutOperation> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IWriteableOperation>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutOperation)iterator.Current; } }

        #region ILayoutOperation
        IEnumerator<ILayoutOperation> IEnumerable<ILayoutOperation>.GetEnumerator() { return GetEnumerator(); }
        ILayoutOperation IReadOnlyList<ILayoutOperation>.this[int index] { get { return (ILayoutOperation)this[index]; } }
        #endregion
    }
}
