namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Cell view for components that are displayed.
    /// </summary>
    public interface IFrameVisibleCellView : IFrameCellView
    {
        /// <summary>
        /// The frame that created this cell view.
        /// </summary>
        IFrameFrame Frame { get; }

        /// <summary>
        /// Line number where the cell view appears.
        /// </summary>
        int LineNumber { get; }

        /// <summary>
        /// Column number where the cell view appears.
        /// </summary>
        int ColumnNumber { get; }
    }

    /// <summary>
    /// Cell view for components that are displayed.
    /// </summary>
    internal class FrameVisibleCellView : FrameCellView, IFrameVisibleCellView
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FrameVisibleCellView"/> object.
        /// </summary>
        public static FrameVisibleCellView Empty { get; } = new FrameVisibleCellView(FrameNodeStateView.Empty, FrameCellViewCollection.Empty, FrameFrame.FrameRoot);

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameVisibleCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        public FrameVisibleCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameFrame frame)
            : base(stateView, parentCellView)
        {
            Frame = frame;
            LineNumber = 0;
            ColumnNumber = 0;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The frame that created this cell view.
        /// </summary>
        public IFrameFrame Frame { get; }

        /// <summary>
        /// Line number where the cell view appears.
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// Column number where the cell view appears.
        /// </summary>
        public int ColumnNumber { get; private set; }

        /// <summary>
        /// True if the cell view is visible or contains at least one visible cell view.
        /// </summary>
        public override bool HasVisibleCellView { get { return true; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Clears all views (cells and states) within this cell view.
        /// </summary>
        public override void ClearCellTree()
        {
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
            Debug.Assert(lineNumber >= 1);
            Debug.Assert(columnNumber >= 1);

            LineNumber = lineNumber;
            ColumnNumber = columnNumber;

            if (maxLineNumber < lineNumber)
                maxLineNumber = lineNumber;

            if (maxColumnNumber < columnNumber)
                maxColumnNumber = columnNumber;

            IncrementLineNumber(ref lineNumber);
            IncrementColumnNumber(ref columnNumber);
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
            Debug.Assert(handler != null);

            if (handler(this))
            {
                cellView = this;
                return true;
            }
            else
            {
                cellView = null;
                return false;
            }
        }
        #endregion

        #region Descendant Interface
        private protected virtual void IncrementLineNumber(ref int lineNumber)
        {
            lineNumber++;
        }

        private protected virtual void IncrementColumnNumber(ref int columnNumber)
        {
            columnNumber++;
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameVisibleCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameVisibleCellView AsVisibleCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsVisibleCellView))
                return comparer.Failed();

            if (!comparer.IsSameInteger(LineNumber, AsVisibleCellView.LineNumber))
                return comparer.Failed();

            if (!comparer.IsSameInteger(ColumnNumber, AsVisibleCellView.ColumnNumber))
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

            Result += "Visible\n";

            return Result;
        }

        /// <summary>
        /// Checks if the tree of cell views under this state is valid.
        /// </summary>
        /// <param name="expectedCellViewTable">Cell views that are associated to a property of the node.</param>
        /// <param name="actualCellViewTable">Cell views that are found in the tree.</param>
        public override bool IsCellViewTreeValid(FrameAssignableCellViewReadOnlyDictionary<string> expectedCellViewTable, FrameAssignableCellViewDictionary<string> actualCellViewTable)
        {
            return true;
        }
        #endregion
    }
}
