﻿namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutOperationReadOnlyList : FocusOperationReadOnlyList, IReadOnlyCollection<LayoutOperation>, IReadOnlyList<LayoutOperation>
    {
        /// <inheritdoc/>
        public LayoutOperationReadOnlyList(LayoutOperationList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new LayoutOperation this[int index] { get { return (LayoutOperation)base[index]; } }

        #region LayoutOperation
        IEnumerator<LayoutOperation> IEnumerable<LayoutOperation>.GetEnumerator() { return ((IList<LayoutOperation>)this).GetEnumerator(); }
        LayoutOperation IReadOnlyList<LayoutOperation>.this[int index] { get { return (LayoutOperation)this[index]; } }
        #endregion
    }
}
