using BaseNodeHelper;
using System;

namespace EaslyController.Frame
{
    public interface IFrameListFrame : IFrameNamedFrame
    {
    }

    public abstract class FrameListFrame : FrameNamedFrame, IFrameListFrame
    {
        #region Client Interface
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            return NodeTreeHelperList.IsNodeListProperty(nodeType, PropertyName, out Type ChildNodeType);
        }
        #endregion
    }
}
