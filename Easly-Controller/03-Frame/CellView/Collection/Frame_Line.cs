namespace EaslyController.Frame
{
    using System.Diagnostics;

    /// <summary>
    /// A collection of cell views organized in a line.
    /// </summary>
    public interface IFrameLine : IFrameCellViewCollection
    {
    }

    /// <summary>
    /// A collection of cell views organized in a line.
    /// </summary>
    internal class FrameLine : FrameCellViewCollection, IFrameLine
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameLine"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        public FrameLine(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameCellViewList cellViewList)
            : base(stateView, parentCellView, cellViewList)
        {
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update line numbers in the cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="maxLineNumber">The maximum line number observed, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        /// <param name="maxColumnNumber">The maximum column number observed, updated upon return.</param>
        public override void UpdateLineNumbers(ref int lineNumber, ref int maxLineNumber, ref int columnNumber, ref int maxColumnNumber)
        {
            int StartLineNumber = lineNumber;
            int LocalMaxLineNumber = lineNumber;

            foreach (IFrameCellView CellView in CellViewList)
            {
                int ChildLineNumber = StartLineNumber;
                RecalculateChildLineNumbers(CellView, ref ChildLineNumber, ref maxLineNumber, ref columnNumber, ref maxColumnNumber);

                if (LocalMaxLineNumber < ChildLineNumber)
                    LocalMaxLineNumber = ChildLineNumber;
            }

            lineNumber = LocalMaxLineNumber;
        }
        #endregion

        #region Descendant Interface
        /// <summary>
        /// Update line numbers in the cell view from the update in a child cell.
        /// </summary>
        /// <param name="cellView">The child cell view.</param>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="maxLineNumber">The maximum line number observed, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        /// <param name="maxColumnNumber">The maximum column number observed, updated upon return.</param>
        private protected virtual void RecalculateChildLineNumbers(IFrameCellView cellView, ref int lineNumber, ref int maxLineNumber, ref int columnNumber, ref int maxColumnNumber)
        {
            cellView.UpdateLineNumbers(ref lineNumber, ref maxLineNumber, ref columnNumber, ref maxColumnNumber);
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

            if (!comparer.IsSameType(other, out FrameLine AsLine))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsLine))
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

            Result += $" Line, {CellViewList.Count} cell(s)\n";

            foreach (IFrameCellView Item in CellViewList)
                Result += Item.PrintTree(indentation + 1, isVerbose);

            return Result;
        }
        #endregion
    }
}
