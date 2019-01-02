using System;

namespace EaslyController.Frame
{
    public interface IFrameFrame
    {
        IFrameFrame ParentFrame { get; }
        bool IsValid(Type nodeType);
        void UpdateParentFrame(IFrameFrame parentFrame);
    }

    public abstract class FrameFrame : IFrameFrame
    {
        #region Properties
        public IFrameFrame ParentFrame { get; private set; }
        #endregion

        #region Client Interface
        public virtual bool IsValid(Type nodeType)
        {
            return true;
        }

        public virtual void UpdateParentFrame(IFrameFrame parentFrame)
        {
            ParentFrame = parentFrame;
        }
        #endregion

        #region Debugging
        public override string ToString()
        {
            return base.ToString() + " (" + GetHashCode() + ")";
        }
        #endregion
    }
}
