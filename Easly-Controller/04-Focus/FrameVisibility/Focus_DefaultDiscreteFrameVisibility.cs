namespace EaslyController.Focus
{
    using System;
    using BaseNodeHelper;

    /// <summary>
    /// Frame visibility that depends if an enum or boolean has the default value.
    /// </summary>
    public interface IFocusDefaultDiscreteFrameVisibility : IFocusFrameVisibility, IFocusNodeFrameVisibility
    {
        /// <summary>
        /// Name of the enum or boolean property.
        /// (Set in Xaml)
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// Default value as int.
        /// (Set in Xaml)
        /// </summary>
        int DefaultValue { get; set; }
    }

    /// <summary>
    /// Frame visibility that depends if an enum or boolean has the default value.
    /// </summary>
    public class FocusDefaultDiscreteFrameVisibility : FocusFrameVisibility, IFocusDefaultDiscreteFrameVisibility
    {
        #region Properties
        /// <summary>
        /// True if the visibility depends on the show/hidden state of the view with the focus.
        /// </summary>
        public override bool IsVolatile { get { return true; } }

        /// <summary>
        /// Name of the enum or boolean property.
        /// (Set in Xaml)
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Default value as int.
        /// (Set in Xaml)
        /// </summary>
        public int DefaultValue { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame visibility is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame visibility can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            bool IsValid = true;

            IsValid &= !string.IsNullOrEmpty(PropertyName);
            IsValid &= NodeTreeHelper.IsEnumProperty(nodeType, PropertyName) || NodeTreeHelper.IsBooleanProperty(nodeType, PropertyName);

            return IsValid;
        }

        /// <summary>
        /// Is the associated frame visible.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="frame">The frame with the associated visibility.</param>
        public virtual bool IsVisible(IFocusCellViewTreeContext context, IFocusNodeFrameWithVisibility frame)
        {
            if (context.ControllerView.DiscreteHasDefaultValue(context.StateView, PropertyName, DefaultValue))
                return false;

            return true;
        }
        #endregion
    }
}
