using BaseNodeHelper;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for describing an optional child node.
    /// </summary>
    public interface IFrameOptionalFrame : IFrameNamedFrame
    {
    }

    /// <summary>
    /// Frame for describing an optional child node.
    /// </summary>
    public class FrameOptionalFrame : FrameNamedFrame, IFrameOptionalFrame
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

            if (!NodeTreeHelperOptional.IsOptionalChildNodeProperty(nodeType, PropertyName, out Type ChildNodeType))
                return false;

            return true;
        }
        #endregion
    }
}
