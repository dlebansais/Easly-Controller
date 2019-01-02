using BaseNodeHelper;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Base frame for a list of nodes.
    /// </summary>
    public interface IFrameListFrame : IFrameNamedFrame
    {
    }

    /// <summary>
    /// Base frame for a list of nodes.
    /// </summary>
    public abstract class FrameListFrame : FrameNamedFrame, IFrameListFrame
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

            return NodeTreeHelperList.IsNodeListProperty(nodeType, PropertyName, out Type ChildNodeType);
        }
        #endregion
    }
}
