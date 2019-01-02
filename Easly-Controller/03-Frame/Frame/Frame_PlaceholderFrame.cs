using BaseNodeHelper;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for describing an child node.
    /// </summary>
    public interface IFramePlaceholderFrame : IFrameNamedFrame
    {
    }

    /// <summary>
    /// Frame for describing an child node.
    /// </summary>
    public class FramePlaceholderFrame : FrameNamedFrame, IFramePlaceholderFrame
    {
        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            if (!NodeTreeHelperChild.IsChildNodeProperty(nodeType, PropertyName, out Type ChildNodeType))
                return false;

            return true;
        }
        #endregion
    }
}
