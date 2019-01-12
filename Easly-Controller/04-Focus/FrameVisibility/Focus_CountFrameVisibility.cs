using BaseNodeHelper;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Frame visibility that depends if a collection has at least one item.
    /// </summary>
    public interface IFocusCountFrameVisibility : IFocusFrameVisibility, IFocusNodeFrameVisibility
    {
        /// <summary>
        /// Name of the collection property.
        /// (Set in Xaml)
        /// </summary>
        string PropertyName { get; set; }
    }

    /// <summary>
    /// Frame visibility that depends if a collection has at least one item.
    /// </summary>
    public class FocusCountFrameVisibility : FocusFrameVisibility, IFocusCountFrameVisibility
    {
        #region Properties
        /// <summary>
        /// True if the visibility depends on the show/hidden state of the view with the focus.
        /// </summary>
        public override bool IsVolatile { get { return true; } }

        /// <summary>
        /// Name of the collection property.
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

            Type ChildInterfaceType, ChildNodeType;
            if (!NodeTreeHelperList.IsNodeListProperty(nodeType, PropertyName, out ChildNodeType) && !NodeTreeHelperBlockList.IsBlockListProperty(nodeType, PropertyName, out ChildInterfaceType, out ChildNodeType))
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
            if (!controllerView.CollectionHasItems(stateView, PropertyName))
                return false;

            return true;
        }
        #endregion
    }
}
