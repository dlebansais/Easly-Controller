using System;
using System.Windows.Markup;

namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public interface IFrameKeywordFrame : IFrameStaticFrame
    {
        /// <summary>
        /// Free text.
        /// (Set in Xaml)
        /// </summary>
        string Text { get; set; }
    }

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    [ContentProperty("Text")]
    public class FrameKeywordFrame : FrameStaticFrame, IFrameKeywordFrame
    {
        #region Properties
        /// <summary>
        /// Free text.
        /// (Set in Xaml)
        /// </summary>
        public string Text { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            if (string.IsNullOrEmpty(Text))
                return false;

            return true;
        }
        #endregion
    }
}
