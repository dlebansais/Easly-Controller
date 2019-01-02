namespace EaslyController.Frame
{
    public interface IFrameColumn : IFrameCellViewCollection
    {
    }

    public class FrameColumn : FrameCellViewCollection, IFrameColumn
    {
        #region Init
        public FrameColumn(IFrameNodeStateView stateView, IFrameCellViewReadOnlyList cellViewList)
            : base(stateView, cellViewList)
        {
        }
        #endregion

        #region Client Interface
        public override void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber)
        {
            int StartColumnNumber = columnNumber;
            int MaxColumnNumber = columnNumber;

            foreach (IFrameCellView CellView in CellViewList)
            {
                int ChildColumnNumber = StartColumnNumber;
                RecalculateChildLineNumbers(controller, CellView, ref lineNumber, ref ChildColumnNumber);

                if (MaxColumnNumber < ChildColumnNumber)
                    MaxColumnNumber = ChildColumnNumber;
            }

            columnNumber = MaxColumnNumber;
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
