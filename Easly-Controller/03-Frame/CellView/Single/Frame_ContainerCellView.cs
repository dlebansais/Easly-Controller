namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    public interface IFrameContainerCellView : IFrameCellView, IFrameAssignableCellView
    {
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        IFrameCellViewCollection ParentCellView { get; }

        /// <summary>
        /// The state view of the state associated to this cell.
        /// </summary>
        IFrameNodeStateView ChildStateView { get; }
    }

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    public class FrameContainerCellView : FrameCellView, IFrameContainerCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameContainerCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view.</param>
        /// <param name="childStateView">The state view of the state associated to this cell.</param>
        public FrameContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
            : base(stateView)
        {
            Debug.Assert(parentCellView != null);
            Debug.Assert(childStateView != null);

            ParentCellView = parentCellView;
            ChildStateView = childStateView;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        public IFrameCellViewCollection ParentCellView { get; }

        /// <summary>
        /// The state view of the state associated to this cell.
        /// </summary>
        public IFrameNodeStateView ChildStateView { get; }

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
        /// Enumerate all visible cell views.
        /// </summary>
        /// <param name="list">The list of visible cell views upon return.</param>
        public override void EnumerateVisibleCellViews(IFrameVisibleCellViewList list)
        {
            Debug.Assert(list != null);

            ChildStateView.EnumerateVisibleCellViews(list);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameContainerCellView AsContainerCellView))
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

            if (ChildStateView.RootCellView != null)
            {
                Result += $"Container, state: {ChildStateView}\n";

                if (isVerbose)
                    Result += ChildStateView.RootCellView.PrintTree(indentation + 1, isVerbose);
            }
            else
            {
                Result += $"Container, state: {ChildStateView} (no root)\n";
            }

            return Result;
        }

        /// <summary>
        /// Checks if the tree of cell views under this state is valid.
        /// </summary>
        /// <param name="expectedCellViewTable">Cell views that are associated to a property of the node.</param>
        /// <param name="actualCellViewTable">Cell views that are found in the tree.</param>
        public override bool IsCellViewTreeValid(IFrameAssignableCellViewReadOnlyDictionary<string> expectedCellViewTable, IFrameAssignableCellViewDictionary<string> actualCellViewTable)
        {
            if (!ChildStateView.IsCellViewTreeValid())
                return false;

            if (!IsCellViewProperlyAssigned(expectedCellViewTable, actualCellViewTable))
                return false;

            return true;
        }

        /// <summary></summary>
        private protected virtual bool IsCellViewProperlyAssigned(IFrameAssignableCellViewReadOnlyDictionary<string> expectedCellViewTable, IFrameAssignableCellViewDictionary<string> actualCellViewTable)
        {
            string PropertyName = null;
            foreach (KeyValuePair<string, IFrameAssignableCellView> Entry in expectedCellViewTable)
                if (Entry.Value == this)
                {
                    PropertyName = Entry.Key;
                    break;
                }

            if (IsAssignedToTable != (PropertyName != null))
                return false;

            if (PropertyName != null)
            {
                bool IsActual = false;
                foreach (KeyValuePair<string, IFrameAssignableCellView> Entry in actualCellViewTable)
                    if (Entry.Value == this)
                    {
                        IsActual = true;
                        break;
                    }
                if (IsActual)
                    return false;

                actualCellViewTable.Add(PropertyName, this);
            }

            return true;
        }
        #endregion
    }
}
