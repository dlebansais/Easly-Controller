﻿namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// A cell view for a block state.
    /// </summary>
    public interface IFrameBlockCellView : IFrameCellView
    {
        /// <summary>
        /// The block state view of the state associated to this cell.
        /// </summary>
        FrameBlockStateView BlockStateView { get; }
    }

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    internal class FrameBlockCellView : FrameCellView, IFrameBlockCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBlockCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="blockStateView">The block state view of the state associated to this cell.</param>
        public FrameBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameBlockStateView blockStateView)
            : base(stateView, parentCellView)
        {
            Debug.Assert(blockStateView.RootCellView != null);

            BlockStateView = blockStateView;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The block state view of the state associated to this cell.
        /// </summary>
        public FrameBlockStateView BlockStateView { get; }

        /// <summary>
        /// True if the block cell view contain at least one visible cell view.
        /// </summary>
        public override bool HasVisibleCellView { get { return BlockStateView.HasVisibleCellView; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Clears all views (cells and states) within this cell view.
        /// </summary>
        public override void ClearCellTree()
        {
            BlockStateView.ClearRootCellView(StateView);
        }

        /// <summary>
        /// Update line numbers in the cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="maxLineNumber">The maximum line number observed, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        /// <param name="maxColumnNumber">The maximum column number observed, updated upon return.</param>
        public override void UpdateLineNumbers(ref int lineNumber, ref int maxLineNumber, ref int columnNumber, ref int maxColumnNumber)
        {
            BlockStateView.UpdateLineNumbers(ref lineNumber, ref maxLineNumber, ref columnNumber, ref maxColumnNumber);
        }

        /// <summary>
        /// Enumerate all visible cell views. Enumeration is interrupted if <paramref name="handler"/> returns true.
        /// </summary>
        /// <param name="handler">A handler to execute for each cell view.</param>
        /// <param name="cellView">The cell view for which <paramref name="handler"/> returned true. Null if none.</param>
        /// <param name="reversed">If true, search in reverse order.</param>
        /// <returns>The last value returned by <paramref name="handler"/>.</returns>
        public override bool EnumerateVisibleCellViews(Func<IFrameVisibleCellView, bool> handler, out IFrameVisibleCellView cellView, bool reversed)
        {
            return BlockStateView.EnumerateVisibleCellViews(handler, out cellView, reversed);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameBlockCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out FrameBlockCellView AsBlockCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBlockCellView))
                return comparer.Failed();

            if (!comparer.VerifyEqual(ParentCellView, AsBlockCellView.ParentCellView))
                return comparer.Failed();

            if (!comparer.VerifyEqual(BlockStateView, AsBlockCellView.BlockStateView))
                return comparer.Failed();

            return true;
        }

        /// <summary>
        /// Returns a string representing this part of the cell view tree.
        /// </summary>
        /// <param name="indentation">The indentation level to use.</param>
        /// <param name="isVerbose">True to verbose information.</param>
        public override string PrintTree(int indentation, bool isVerbose)
        {
            return BlockStateView.RootCellView.PrintTree(indentation, isVerbose);
        }

        /// <summary>
        /// Checks if the tree of cell views under this state is valid.
        /// </summary>
        /// <param name="expectedCellViewTable">Cell views that are associated to a property of the node.</param>
        /// <param name="actualCellViewTable">Cell views that are found in the tree.</param>
        public override bool IsCellViewTreeValid(FrameAssignableCellViewReadOnlyDictionary<string> expectedCellViewTable, FrameAssignableCellViewDictionary<string> actualCellViewTable)
        {
            bool IsValid = true;

            IsValid &= BlockStateView.IsCellViewTreeValid();

            return IsValid;
        }
        #endregion
    }
}
