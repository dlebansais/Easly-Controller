namespace EaslyController.Layout
{
    using System.Diagnostics;
    using System.Windows.Markup;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Focus selecting sub-frames.
    /// </summary>
    public interface ILayoutSelectionFrame : IFocusSelectionFrame, ILayoutFrame, ILayoutNodeFrame
    {
        /// <summary>
        /// List of frames among which to select.
        /// </summary>
        new ILayoutSelectableFrameList Items { get; }
    }

    /// <summary>
    /// Focus selecting sub-frames.
    /// </summary>
    [ContentProperty("Items")]
    public class LayoutSelectionFrame : FocusSelectionFrame, ILayoutSelectionFrame
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

        /// <summary>
        /// List of frames among which to select.
        /// </summary>
        public new ILayoutSelectableFrameList Items { get { return (ILayoutSelectableFrameList)base.Items; } }

        /// <summary></summary>
        private protected override bool IsParentRoot { get { return ParentFrame == LayoutFrame.LayoutRoot; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the reference to the parent frame.
        /// </summary>
        /// <param name="parentTemplate">The parent template.</param>
        /// <param name="parentFrame">The parent frame.</param>
        public override void UpdateParent(IFrameTemplate parentTemplate, IFrameFrame parentFrame)
        {
            base.UpdateParent(parentTemplate, parentFrame);

            Debug.Assert(ParentTemplate == parentTemplate);
            Debug.Assert(ParentFrame == parentFrame);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxSelectableFrameList object.
        /// </summary>
        private protected override IFocusSelectableFrameList CreateSelectableFrameList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutSelectionFrame));
            return new LayoutSelectableFrameList();
        }
        #endregion
    }
}
