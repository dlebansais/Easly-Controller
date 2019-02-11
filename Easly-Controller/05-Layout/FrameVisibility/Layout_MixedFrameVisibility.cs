namespace EaslyController.Layout
{
    using System.Windows.Markup;
    using EaslyController.Focus;

    /// <summary>
    /// Focus visibility that shows if one frame visibility among a list does.
    /// </summary>
    public interface ILayoutMixedFrameVisibility : IFocusMixedFrameVisibility, ILayoutFrameVisibility, ILayoutNodeFrameVisibility
    {
        /// <summary>
        /// List of frame visibilities that must be satisfied at least for one.
        /// (Set in Xaml)
        /// </summary>
        new ILayoutNodeFrameVisibilityList Items { get; }
    }

    /// <summary>
    /// Focus visibility that shows if one frame visibility among a list does.
    /// </summary>
    [ContentProperty("Items")]
    public class LayoutMixedFrameVisibility : FocusMixedFrameVisibility, ILayoutMixedFrameVisibility
    {
        #region Properties
        /// <summary>
        /// List of frame visibilities that must be satisfied at least for one.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutNodeFrameVisibilityList Items { get { return (ILayoutNodeFrameVisibilityList)base.Items; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeFrameVisibilityList object.
        /// </summary>
        private protected override IFocusNodeFrameVisibilityList CreateNodeFrameVisibilityList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutMixedFrameVisibility));
            return new LayoutNodeFrameVisibilityList();
        }
        #endregion
    }
}
