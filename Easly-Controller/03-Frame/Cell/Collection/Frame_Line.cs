namespace EaslyController.Frame
{
    public interface IFrameLine : IFrameCellViewCollection
    {
    }

    public class FrameLine : FrameCellViewCollection, IFrameLine
    {
        #region Init
        public FrameLine(IFrameNodeStateView stateView, IFrameCellViewReadOnlyList cellViewList)
            : base(stateView, cellViewList)
        {
        }
        #endregion

        #region Client Interface
        public override void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber)
        {
            int StartLineNumber = lineNumber;
            int MaxLineNumber = lineNumber;

            foreach (IFrameCellView CellView in CellViewList)
            {
                int ChildLineNumber = StartLineNumber;
                RecalculateChildLineNumbers(controller, CellView, ref ChildLineNumber, ref columnNumber);

                if (MaxLineNumber < ChildLineNumber)
                    MaxLineNumber = ChildLineNumber;
            }

            lineNumber = MaxLineNumber;
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
