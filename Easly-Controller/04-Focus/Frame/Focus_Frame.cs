using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Base frame.
    /// </summary>
    public interface IFocusFrame : IFrameFrame
    {
        /// <summary>
        /// Parent template.
        /// </summary>
        new IFocusTemplate ParentTemplate { get; }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        new IFocusFrame ParentFrame { get; }
    }

    /// <summary>
    /// Base frame.
    /// </summary>
    public abstract class FocusFrame : FrameFrame, IFocusFrame
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
        #endregion
    }
}
