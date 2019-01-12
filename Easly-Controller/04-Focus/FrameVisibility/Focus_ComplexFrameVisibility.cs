using BaseNodeHelper;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Frame visibility that depends on the IsComplex template property.
    /// </summary>
    public interface IFocusComplexFrameVisibility : IFocusFrameVisibility, IFocusNodeFrameVisibility
    {
        /// <summary>
        /// Name of the property that can be complex or not.
        /// (Set in Xaml)
        /// </summary>
        string PropertyName { get; set; }
    }

    /// <summary>
    /// Frame visibility that depends on the IsComplex template property.
    /// </summary>
    public class FocusComplexFrameVisibility : FocusFrameVisibility, IFocusComplexFrameVisibility
    {
        #region Properties
        /// <summary>
        /// True if the visibility depends on the show/hidden state of the view with the focus.
        /// </summary>
        public override bool IsVolatile { get { return false; } }

        /// <summary>
        /// Name of the property that can be complex or not.
        /// (Set in Xaml)
        /// </summary>
        public string PropertyName { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame visibility is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame visibility can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            if (string.IsNullOrEmpty(PropertyName))
                return false;

            if (!NodeTreeHelperChild.IsChildNodeProperty(nodeType, PropertyName, out Type ChildNodeType))
                return false;

            return true;
        }

        /// <summary>
        /// Is the associated frame visible.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        /// <param name="frame">The frame with the associated visibility.</param>
        public virtual bool IsVisible(IFocusControllerView controllerView, IFocusNodeStateView stateView, IFocusNodeFrame frame)
        {
            if (!controllerView.IsTemplateComplex(stateView, PropertyName))
                return false;

            return true;
        }
        #endregion
    }
}
