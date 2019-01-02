using BaseNodeHelper;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Base frame for a block list.
    /// </summary>
    public interface IFrameBlockListFrame : IFrameNamedFrame
    {
    }

    /// <summary>
    /// Base frame for a block list.
    /// </summary>
    public abstract class FrameBlockListFrame : FrameNamedFrame, IFrameBlockListFrame
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

            return NodeTreeHelperBlockList.IsBlockListProperty(nodeType, PropertyName, out Type ChildInterfaceType, out Type ChildNodeType);
        }
        #endregion
    }
}
