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
    }
}
