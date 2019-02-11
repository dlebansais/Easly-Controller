namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Base frame for frames that describe property in a node.
    /// </summary>
    public interface ILayoutNamedFrame : IFocusNamedFrame, ILayoutFrame
    {
    }

    /// <summary>
    /// Base frame for frames that describe property in a node.
    /// </summary>
    internal abstract class LayoutNamedFrame : FocusNamedFrame, ILayoutNamedFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new ILayoutTemplate ParentTemplate { get { return (ILayoutTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new ILayoutFrame ParentFrame { get { return (ILayoutFrame)base.ParentFrame; } }
        #endregion
    }
}
