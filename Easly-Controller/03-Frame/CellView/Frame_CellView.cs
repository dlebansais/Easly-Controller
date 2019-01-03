namespace EaslyController.Frame
{
    public interface IFrameCellView
    {
        IFrameNodeStateView StateView { get; }
        void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber);
    }

    public abstract class FrameCellView
    {
        #region Init
        public FrameCellView(IFrameNodeStateView stateView)
        {
            StateView = stateView;
        }
        #endregion

        #region Properties
        public IFrameNodeStateView StateView { get; private set; }
        #endregion

        #region Client Interface
        public abstract void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber);
        #endregion
    }
}
