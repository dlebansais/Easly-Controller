using EaslyController.Frame;
using System.Windows.Markup;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus describing an enum value that can be displayed with different frames depending on its value.
    /// </summary>
    public interface IFocusDiscreteFrame : IFrameDiscreteFrame, IFocusValueFrame
    {
        /// <summary>
        /// List of frames that can be displayed.
        /// (Set in Xaml)
        /// </summary>
        new IFocusKeywordFrameList Items { get; }
    }

    /// <summary>
    /// Focus describing an enum value that can be displayed with different frames depending on its value.
    /// </summary>
    [ContentProperty("Items")]
    public class FocusDiscreteFrame : FrameDiscreteFrame, IFocusDiscreteFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new IFocusTemplate ParentTemplate { get { return (IFocusTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new IFocusFrame ParentFrame { get { return (IFocusFrame)base.ParentFrame; } }

        /// <summary>
        /// List of frames that can be displayed.
        /// (Set in Xaml)
        /// </summary>
        public new IFocusKeywordFrameList Items { get { return (IFocusKeywordFrameList)base.Items; } }
        #endregion
    }
}
