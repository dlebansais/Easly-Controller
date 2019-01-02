using System;

namespace EaslyController.Frame
{
    public interface IFrameNamedFrame : IFrameFrame
    {
        string PropertyName { get; set; } // Set in Xaml
    }

    public abstract class FrameNamedFrame : FrameFrame, IFrameNamedFrame
    {
        #region Properties
        public string PropertyName { get; set; } // Set in Xaml
        #endregion

        #region Client Interface
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            if (string.IsNullOrEmpty(PropertyName) || nodeType.GetProperty(PropertyName) == null)
                return false;

            return true;
        }
        #endregion
    }
}
