namespace EaslyController.Frame
{
    public interface IFrameSingleDiscreteFocusableCellView : IFrameFocusableCellView
    {
    }

    public class FrameSingleDiscreteFocusableCellView : FrameFocusableCellView, IFrameSingleDiscreteFocusableCellView
    {
        #region Init
        public FrameSingleDiscreteFocusableCellView(IFrameNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion
    }
}
