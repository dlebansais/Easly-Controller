using System.Diagnostics;

namespace EaslyController.Frame
{
    public interface IFrameMutableLine : IFrameMutableCellViewCollection
    {
    }

    public class FrameMutableLine : FrameMutableCellViewCollection, IFrameMutableLine
    {
        #region Init
        public FrameMutableLine(IFrameNodeStateView stateView, IFrameCellViewList cellViewList)
            : base(stateView, cellViewList)
        {
        }
        #endregion

        #region Client Interface
        public override void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber)
        {
            int StartMutableLineNumber = lineNumber;
            int MaxMutableLineNumber = lineNumber;

            foreach (IFrameCellView CellView in CellViewList)
            {
                int ChildMutableLineNumber = StartMutableLineNumber;
                RecalculateChildLineNumbers(controller, CellView, ref ChildMutableLineNumber, ref columnNumber);

                if (MaxMutableLineNumber < ChildMutableLineNumber)
                    MaxMutableLineNumber = ChildMutableLineNumber;
            }

            lineNumber = MaxMutableLineNumber;
        }
        #endregion

        #region Descendant Interface
        protected virtual void RecalculateChildLineNumbers(IFrameController controller, IFrameCellView cell, ref int lineNumber, ref int columnNumber)
        {
            cell.RecalculateLineNumbers(controller, ref lineNumber, ref columnNumber);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameMutableLine AsMutableLine))
                return false;

            if (!base.IsEqual(comparer, AsMutableLine))
                return false;

            return true;
        }

        public override string PrintTree(int indentation)
        {
            string Result = "";
            for (int i = 0; i < indentation; i++)
                Result += " ";

            Result += $"Mutable Line, {CellViewList.Count} cell(s)\n";

            foreach (IFrameCellView Item in CellViewList)
                Result += Item.PrintTree(indentation + 1);

            return Result;
        }
        #endregion
    }
}
