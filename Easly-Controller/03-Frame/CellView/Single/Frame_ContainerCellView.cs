namespace EaslyController.Frame
{
    public interface IFrameContainerCellView : IFrameCellView
    {
        IFrameNodeStateView ChildStateView { get; }
    }

    public class FrameContainerCellView : FrameCellView, IFrameContainerCellView
    {
        #region Init
        public FrameContainerCellView(IFrameNodeStateView stateView, IFrameNodeStateView childStateView)
            : base(stateView)
        {
            ChildStateView = childStateView;
        }
        #endregion

        #region Properties
        public IFrameNodeStateView ChildStateView { get; }
        #endregion

        #region Client Interface
        public override void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber)
        {
            RecalculateChildLineNumbers(controller, ChildStateView, ref lineNumber, ref columnNumber);
        }
        #endregion

        #region Descendant Interface
        protected virtual void RecalculateChildLineNumbers(IFrameController controller, IFrameNodeStateView nodeStateView, ref int lineNumber, ref int columnNumber)
        {
            nodeStateView.RecalculateLineNumbers(controller, ref lineNumber, ref columnNumber);
        }
        #endregion
    }
}
