namespace EaslyController.Frame
{
    public interface IFrameVisibleCellView : IFrameCellView
    {
        int LineNumber { get; }
        int ColumnNumber { get; }
    }

    public class FrameVisibleCellView : FrameCellView, IFrameVisibleCellView
    {
        #region Init
        public FrameVisibleCellView(IFrameNodeStateView stateView)
            : base(stateView)
        {
            LineNumber = 0;
            ColumnNumber = 0;
        }
        #endregion

        #region Properties
        public int LineNumber { get; private set; }
        public int ColumnNumber { get; private set; }
        #endregion

        #region Client Interface
        public override void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber)
        {
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;

            IncrementLineNumber(ref lineNumber);
            IncrementColumnNumber(ref columnNumber);
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
    }
}
