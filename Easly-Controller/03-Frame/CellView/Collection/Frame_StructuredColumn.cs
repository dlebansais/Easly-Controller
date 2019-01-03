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
    }
}
