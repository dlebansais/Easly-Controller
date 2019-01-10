using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Cell view for components that are displayed.
    /// </summary>
    public interface IFrameVisibleCellView : IFrameCellView
    {
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
        public FrameVisibleCellView(IFrameNodeStateView stateView)
            : base(stateView)
        {
            LineNumber = 0;
            ColumnNumber = 0;
        }
        #endregion

        #region Properties
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
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        /// <param name="maxColumnNumber">The maximum column number observed, updated upon return.</param>
        public override void UpdateLineNumbers(ref int lineNumber, ref int columnNumber, ref int maxColumnNumber)
        {
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;

            IncrementLineNumber(ref lineNumber);
            IncrementColumnNumber(ref columnNumber);

            if (maxColumnNumber < columnNumber)
                maxColumnNumber = columnNumber;
        }
        #endregion

        #region Descendant Interface
        protected virtual void IncrementLineNumber(ref int lineNumber)
        {
            lineNumber++;
        }

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
        #endregion
    }
}
