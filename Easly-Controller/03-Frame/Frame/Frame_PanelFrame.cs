using System;

namespace EaslyController.Frame
{
    public interface IFramePanelFrame : IFrameFrame
    {
        IFrameFrameList Items { get; }
    }

    public abstract class FramePanelFrame : FrameFrame, IFramePanelFrame
    {
        #region Init
        public FramePanelFrame()
        {
            Items = CreateItems();
        }
        #endregion

        #region Properties
        public IFrameFrameList Items { get; set; }
        #endregion

        #region Client Interface
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            if (Items == null || Items.Count == 0)
                return false;

            foreach (IFrameFrame Item in Items)
                if (!Item.IsValid(nodeType))
                    return false;

            return true;
        }

        public override void UpdateParentFrame(IFrameFrame parentFrame)
        {
            base.UpdateParentFrame(parentFrame);

            foreach (IFrameFrame Item in Items)
                Item.UpdateParentFrame(this);
        }
        #endregion

        #region Create Methods
        protected virtual IFrameFrameList CreateItems()
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePanelFrame));
            return new FrameFrameList();
        }
        #endregion
    }
}
