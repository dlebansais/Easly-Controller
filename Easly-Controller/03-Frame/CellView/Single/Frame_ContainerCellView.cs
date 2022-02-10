namespace EaslyController.Frame
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    public interface IFrameContainerCellView : IFrameCellView, IFrameAssignableCellView
    {
        /// <summary>
        /// The state view of the state associated to this cell.
        /// </summary>
        IFrameNodeStateView ChildStateView { get; }

        /// <summary>
        /// The frame that was used to create this cell.
        /// </summary>
        IFrameFrame Frame { get; }
    }

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    internal class FrameContainerCellView : FrameCellView, IFrameContainerCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameContainerCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view.</param>
        /// <param name="childStateView">The state view of the state associated to this cell.</param>
        /// <param name="frame">The frame that was used to create this cell.</param>
        public FrameContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameFrame frame)
            : base(stateView, parentCellView)
        {
            ChildStateView = childStateView;
            Frame = frame;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view of the state associated to this cell.
        /// </summary>
        public IFrameNodeStateView ChildStateView { get; }

        /// <summary>
        /// The frame that was used to create this cell.
        /// </summary>
        public IFrameFrame Frame { get; }

        /// <summary>
        /// True if the cell is assigned to a property in a cell view table.
        /// </summary>
        public bool IsAssignedToTable { get; private set; }

        /// <summary>
        /// True if the block view contain at least one visible cell view.
        /// </summary>
        public override bool HasVisibleCellView
        {
            get
            {
                Debug.Assert(ChildStateView != null);
                return ChildStateView.HasVisibleCellView;
            }
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Clears all views (cells and states) within this cell view.
        /// </summary>
        public override void ClearCellTree()
        {
            ChildStateView.ClearRootCellView();
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
            RecalculateChildLineNumbers(ChildStateView, ref lineNumber, ref maxLineNumber, ref columnNumber, ref maxColumnNumber);
        }

        /// <summary>
        /// Indicates that the cell view is assigned to a property in a cell view table.
        /// </summary>
        public virtual void AssignToCellViewTable()
        {
            Debug.Assert(!IsAssignedToTable);

            IsAssignedToTable = true;
        }
        #endregion

        #region Descendant Interface
        /// <summary>
        /// Update line numbers in the cell view from the update in the child state view.
        /// </summary>
        /// <param name="nodeStateView">The child state view.</param>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="maxLineNumber">The maximum line number observed, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        /// <param name="maxColumnNumber">The maximum column number observed, updated upon return.</param>
        private protected virtual void RecalculateChildLineNumbers(IFrameNodeStateView nodeStateView, ref int lineNumber, ref int maxLineNumber, ref int columnNumber, ref int maxColumnNumber)
        {
            nodeStateView.UpdateLineNumbers(ref lineNumber, ref maxLineNumber, ref columnNumber, ref maxColumnNumber);
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
            return ChildStateView.EnumerateVisibleCellViews(handler, out cellView, reversed);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameContainerCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out FrameContainerCellView AsContainerCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsContainerCellView))
                return comparer.Failed();

            if (!comparer.VerifyEqual(ParentCellView, AsContainerCellView.ParentCellView))
                return comparer.Failed();

            if (!comparer.VerifyEqual(ChildStateView, AsContainerCellView.ChildStateView))
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
            string Result = string.Empty;
            for (int i = 0; i < indentation; i++)
                Result += " ";

            string NoRoot = " (no root)";
            if (ChildStateView.RootCellView != null)
                NoRoot = string.Empty;

            Result += $"Container, state: {ChildStateView}{NoRoot}\n";

            if (ChildStateView.RootCellView != null && isVerbose)
                Result += ChildStateView.RootCellView.PrintTree(indentation + 1, isVerbose);

            return Result;
        }

        /// <summary>
        /// Checks if the tree of cell views under this state is valid.
        /// </summary>
        /// <param name="expectedCellViewTable">Cell views that are associated to a property of the node.</param>
        /// <param name="actualCellViewTable">Cell views that are found in the tree.</param>
        public override bool IsCellViewTreeValid(FrameAssignableCellViewReadOnlyDictionary<string> expectedCellViewTable, FrameAssignableCellViewDictionary<string> actualCellViewTable)
        {
            bool IsValid = true;

            IsValid &= ChildStateView.IsCellViewTreeValid();
            IsValid &= IsCellViewProperlyAssigned(expectedCellViewTable, actualCellViewTable);

            return IsValid;
        }

        private protected virtual bool IsCellViewProperlyAssigned(FrameAssignableCellViewReadOnlyDictionary<string> expectedCellViewTable, FrameAssignableCellViewDictionary<string> actualCellViewTable)
        {
            bool IsAssigned = true;

            string PropertyName = null;
            foreach (KeyValuePair<string, IFrameAssignableCellView> Entry in expectedCellViewTable)
                if (Entry.Value == this)
                {
                    PropertyName = Entry.Key;
                    break;
                }

            IsAssigned &= IsAssignedToTable == (PropertyName != null);

            if (PropertyName != null)
            {
                foreach (KeyValuePair<string, IFrameAssignableCellView> Entry in actualCellViewTable)
                    IsAssigned &= Entry.Value != this;

                actualCellViewTable.Add(PropertyName, this);
            }

            return IsAssigned;
        }
        #endregion
    }
}
