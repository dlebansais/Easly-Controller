namespace EaslyController.Focus
{
    using System.Windows.Markup;
    using NotNullReflection;

    /// <summary>
    /// Frame visibility that shows if one frame visibility among a list does.
    /// </summary>
    public interface IFocusMixedFrameVisibility : IFocusFrameVisibility, IFocusNodeFrameVisibility
    {
        /// <summary>
        /// List of frame visibilities that must be satisfied at least for one.
        /// (Set in Xaml)
        /// </summary>
        FocusNodeFrameVisibilityList Items { get; }
    }

    /// <summary>
    /// Frame visibility that shows if one frame visibility among a list does.
    /// </summary>
    [ContentProperty("Items")]
    public class FocusMixedFrameVisibility : FocusFrameVisibility, IFocusMixedFrameVisibility
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusMixedFrameVisibility"/> class.
        /// </summary>
        public FocusMixedFrameVisibility()
        {
            Items = CreateNodeFrameVisibilityList();
        }
        #endregion

        #region Properties
        /// <summary>
        /// True if the visibility depends on the show/hidden state of the view with the focus.
        /// </summary>
        public virtual bool IsVolatile { get { return false; } }

        /// <summary>
        /// List of frame visibilities that must be satisfied at least for one.
        /// (Set in Xaml)
        /// </summary>
        public FocusNodeFrameVisibilityList Items { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame visibility is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame visibility can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            bool IsValid = true;

            IsValid &= Items != null && Items.Count > 0;

            foreach (IFocusNodeFrameVisibility Item in Items)
                IsValid &= Item.IsValid(nodeType);

            return IsValid;
        }

        /// <summary>
        /// Is the associated frame visible.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="frame">The frame with the associated visibility.</param>
        public virtual bool IsVisible(IFocusCellViewTreeContext context, IFocusNodeFrameWithVisibility frame)
        {
            foreach (IFocusNodeFrameVisibility Item in Items)
                if (Item.IsVisible(context, frame))
                    return true;

            return false;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeFrameVisibilityList object.
        /// </summary>
        private protected virtual FocusNodeFrameVisibilityList CreateNodeFrameVisibilityList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusMixedFrameVisibility>());
            return new FocusNodeFrameVisibilityList();
        }
        #endregion
    }
}
