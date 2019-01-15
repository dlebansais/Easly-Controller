using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus describing a value property (or string) in a node.
    /// </summary>
    public interface IFocusValueFrame : IFrameValueFrame, IFocusNamedFrame, IFocusNodeFrameWithVisibility
    {
    }

    /// <summary>
    /// Focus describing a value property (or string) in a node.
    /// </summary>
    public abstract class FocusValueFrame : FrameValueFrame, IFocusValueFrame
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
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusNodeFrameVisibility Visibility { get; set; }
        #endregion
    }
}
