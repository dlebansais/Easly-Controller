using BaseNodeHelper;
using System;

namespace EaslyController.Frame
{
    public interface IFrameBlockListFrame : IFrameNamedFrame
    {
    }

    public abstract class FrameBlockListFrame : FrameNamedFrame, IFrameBlockListFrame
    {
        #region Client Interface
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            return NodeTreeHelperBlockList.IsBlockListProperty(nodeType, PropertyName, out Type ChildInterfaceType, out Type ChildNodeType);
        }
        #endregion
    }
}
