﻿using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// A collection of cell views organized in a line.
    /// </summary>
    public interface IFrameLine : IFrameCellViewCollection
    {
    }

    /// <summary>
    /// A collection of cell views organized in a line.
    /// </summary>
    public class FrameLine : FrameCellViewCollection, IFrameLine
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameColumn"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        public FrameLine(IFrameNodeStateView stateView, IFrameCellViewList cellViewList)
            : base(stateView, cellViewList)
        {
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update line numbers in the cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        public override void UpdateLineNumbers(ref int lineNumber, ref int columnNumber)
        {
            int StartLineNumber = lineNumber;
            int MaxLineNumber = lineNumber;

            foreach (IFrameCellView CellView in CellViewList)
            {
                int ChildLineNumber = StartLineNumber;
                RecalculateChildLineNumbers(CellView, ref ChildLineNumber, ref columnNumber);

                if (MaxLineNumber < ChildLineNumber)
                    MaxLineNumber = ChildLineNumber;
            }

            lineNumber = MaxLineNumber;
        }
        #endregion

        #region Descendant Interface
        /// <summary>
        /// Update line numbers in the cell view from the update in a child cell.
        /// </summary>
        /// <param name="cellView">The child cell view.</param>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        protected virtual void RecalculateChildLineNumbers(IFrameCellView cellView, ref int lineNumber, ref int columnNumber)
        {
            cellView.UpdateLineNumbers(ref lineNumber, ref columnNumber);
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

            if (!(other is IFrameLine AsLine))
                return false;

            if (!base.IsEqual(comparer, AsLine))
                return false;

            return true;
        }

        public override string PrintTree(int indentation, bool printFull)
        {
            string Result = "";
            for (int i = 0; i < indentation; i++)
                Result += " ";

            Result += $" Line, {CellViewList.Count} cell(s)\n";

            foreach (IFrameCellView Item in CellViewList)
                Result += Item.PrintTree(indentation + 1, printFull);

            return Result;
        }
        #endregion
    }
}