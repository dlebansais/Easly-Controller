namespace EaslyController.Frame
{
    using System.Diagnostics;
    using EaslyController.Constants;

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public interface IFrameSymbolFrame : IFrameStaticFrame
    {
        /// <summary>
        /// Free symbol.
        /// (Set in Xaml)
        /// </summary>
        Symbols Symbol { get; set; }
    }

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public class FrameSymbolFrame : FrameStaticFrame, IFrameSymbolFrame
    {
        #region Properties
        /// <summary>
        /// Free symbol.
        /// (Set in Xaml)
        /// </summary>
        public Symbols Symbol { get; set; }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(cellView.StateView == context.StateView);
            Debug.Assert(cellView.Frame == this);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxVisibleCellView object.
        /// </summary>
        private protected override IFrameVisibleCellView CreateVisibleCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSymbolFrame));
            return new FrameVisibleCellView(stateView, this);
        }
        #endregion
    }
}
