using BaseNodeHelper;
using System;

namespace EaslyController.Frame
{
    public interface IFrameOptionalFrame : IFrameNamedFrame
    {
    }

    public class FrameOptionalFrame : FrameNamedFrame, IFrameOptionalFrame
    {
        #region Client Interface
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
