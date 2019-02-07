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

        /// <summary></summary>
        private protected override bool IsFrameFocusable { get { return false; } }
        #endregion
    }
}
