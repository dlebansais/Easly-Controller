﻿namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;

    /// <summary>
    /// Base interface for collection of cell views.
    /// </summary>
    public interface IFocusCellViewCollection : IFrameCellViewCollection, IFocusCellView, IFocusAssignableCellView
    {
        /// <summary>
        /// The collection of child cells.
        /// </summary>
        new IFocusCellViewList CellViewList { get; }

        /// <summary>
        /// The frame that was used to create this cell. Can be null.
        /// </summary>
        new IFocusFrame Frame { get; }
    }

    /// <summary>
    /// Base interface for collection of cell views.
    /// </summary>
    internal abstract class FocusCellViewCollection : FrameCellViewCollection, IFocusCellViewCollection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCellViewCollection"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        /// <param name="frame">The frame that was used to create this cell. Can be null.</param>
        public FocusCellViewCollection(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView, IFocusCellViewList cellViewList, IFocusFrame frame)
            : base(stateView, parentCellView, cellViewList, frame)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new IFocusNodeStateView StateView { get { return (IFocusNodeStateView)base.StateView; } }

        /// <summary>
        /// The collection of cell views containing this view. Null for the root of the cell tree.
        /// </summary>
        public new IFocusCellViewCollection ParentCellView { get { return (IFocusCellViewCollection)base.ParentCellView; } }

        /// <summary>
        /// The collection of child cells.
        /// </summary>
        public new IFocusCellViewList CellViewList { get { return (IFocusCellViewList)base.CellViewList; } }

        /// <summary>
        /// The frame that was used to create this cell. Can be null.
        /// </summary>
        public new IFocusFrame Frame { get { return (IFocusFrame)base.Frame; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        /// <param name="focusedNode">The currently focused node.</param>
        /// <param name="focusedFrame">The currently focused frame in the template associated to <paramref name="focusedNode"/>.</param>
        /// <param name="matchingFocus">The focus in <paramref name="focusChain"/> that match <paramref name="focusedNode"/> and <paramref name="focusedFrame"/> upon return.</param>
        public abstract void UpdateFocusChain(IFocusFocusList focusChain, INode focusedNode, IFocusFrame focusedFrame, ref IFocusFocus matchingFocus);
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FocusCellViewCollection"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public abstract override bool IsEqual(CompareEqual comparer, IEqualComparable other);
        #endregion
    }
}
