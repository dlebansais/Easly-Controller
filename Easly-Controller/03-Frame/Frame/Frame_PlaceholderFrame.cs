using BaseNodeHelper;
using System;

namespace EaslyController.Frame
{
    public interface IFramePlaceholderFrame : IFrameNamedFrame
    {
    }

    public class FramePlaceholderFrame : FrameNamedFrame, IFramePlaceholderFrame
    {
        #region Client Interface
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
