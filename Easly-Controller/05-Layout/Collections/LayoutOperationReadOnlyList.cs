namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

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

        #region ILayoutOperation
        IEnumerator<ILayoutOperation> IEnumerable<ILayoutOperation>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutOperation)iterator.Current; } }
        ILayoutOperation IReadOnlyList<ILayoutOperation>.this[int index] { get { return (ILayoutOperation)this[index]; } }
        #endregion
    }
}
