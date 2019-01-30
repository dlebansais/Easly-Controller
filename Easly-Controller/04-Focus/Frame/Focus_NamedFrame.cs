namespace EaslyController.Focus
{
    using EaslyController.Frame;

    /// <summary>
    /// Base frame for frames that describe property in a node.
    /// </summary>
    public interface IFocusNamedFrame : IFrameNamedFrame, IFocusFrame
    {
    }

    /// <summary>
    /// Base frame for frames that describe property in a node.
    /// </summary>
    public abstract class FocusNamedFrame : FrameNamedFrame, IFocusNamedFrame
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
