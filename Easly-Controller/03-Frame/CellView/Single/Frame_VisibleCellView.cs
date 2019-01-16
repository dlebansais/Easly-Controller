using System.Diagnostics;

namespace EaslyController.Frame
{
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
    public class FrameVisibleCellView : FrameCellView, IFrameVisibleCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameVisibleCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        public FrameVisibleCellView(IFrameNodeStateView stateView, IFrameFrame frame)
            : base(stateView)
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
        /// Enumerate all visible cell views.
        /// </summary>
        /// <param name="list">The list of visible cell views upon return.</param>
        public override void EnumerateVisibleCellViews(IFrameVisibleCellViewList list)
        {
            Debug.Assert(list != null);

            list.Add(this);
        }
        #endregion

        #region Descendant Interface
        /// <summary></summary>
        protected virtual void IncrementLineNumber(ref int lineNumber)
        {
            lineNumber++;
        }

        /// <summary></summary>
        protected virtual void IncrementColumnNumber(ref int columnNumber)
        {
            columnNumber++;
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

            if (!(other is IFrameVisibleCellView AsVisibleCellView))
                return false;

            if (!base.IsEqual(comparer, AsVisibleCellView))
                return false;

            if (LineNumber != AsVisibleCellView.LineNumber)
                return false;

            if (ColumnNumber != AsVisibleCellView.ColumnNumber)
                return false;

            return true;
        }

        /// <summary>
        /// Returns a string representing this part of the cell view tree.
        /// </summary>
        /// <param name="indentation">The indentation level to use.</param>
        /// <param name="isVerbose">True to verbose information.</param>
        public override string PrintTree(int indentation, bool isVerbose)
        {
            string Result = "";
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
        public override bool IsCellViewTreeValid(IFrameAssignableCellViewReadOnlyDictionary<string> expectedCellViewTable, IFrameAssignableCellViewDictionary<string> actualCellViewTable)
        {
            return true;
        }
        #endregion
    }
}
