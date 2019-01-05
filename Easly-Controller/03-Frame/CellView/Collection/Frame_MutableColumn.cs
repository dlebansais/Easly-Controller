using System.Diagnostics;

namespace EaslyController.Frame
{
    public interface IFrameMutableColumn : IFrameMutableCellViewCollection
    {
    }

    public class FrameMutableColumn : FrameMutableCellViewCollection, IFrameMutableColumn
    {
        #region Init
        public FrameMutableColumn(IFrameNodeStateView stateView, IFrameCellViewList cellViewList)
            : base(stateView, cellViewList)
        {
        }
        #endregion

        #region Client Interface
        public override void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber)
        {
            int StartMutableColumnNumber = columnNumber;
            int MaxMutableColumnNumber = columnNumber;

            foreach (IFrameCellView CellView in CellViewList)
            {
                int ChildMutableColumnNumber = StartMutableColumnNumber;
                RecalculateChildLineNumbers(controller, CellView, ref lineNumber, ref ChildMutableColumnNumber);

                if (MaxMutableColumnNumber < ChildMutableColumnNumber)
                    MaxMutableColumnNumber = ChildMutableColumnNumber;
            }

            columnNumber = MaxMutableColumnNumber;
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

            if (!(other is IFrameMutableColumn AsMutableColumn))
                return false;

            if (!base.IsEqual(comparer, AsMutableColumn))
                return false;

            return true;
        }

        public override string PrintTree(int indentation)
        {
            string Result = "";
            for (int i = 0; i < indentation; i++)
                Result += " ";

            Result += $"Mutable Column, {CellViewList.Count} cell(s)\n";

            foreach (IFrameCellView Item in CellViewList)
                Result += Item.PrintTree(indentation + 1);

            return Result;
        }
        #endregion
    }
}
