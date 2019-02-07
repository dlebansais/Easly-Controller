namespace EaslyController.Focus
{
    using System;
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// Base frame for displaying more frames.
    /// </summary>
    public interface IFocusPanelFrame : IFramePanelFrame, IFocusFrame, IFocusNodeFrameWithVisibility, IFocusBlockFrame
    {
        /// <summary>
        /// List of frames within this frame.
        /// </summary>
        new IFocusFrameList Items { get; }
    }

    /// <summary>
    /// Base frame for displaying more frames.
    /// </summary>
    public abstract class FocusPanelFrame : FramePanelFrame, IFocusPanelFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new IFocusTemplate ParentTemplate { get { return (IFocusTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new IFocusFrame ParentFrame { get { return (IFocusFrame)base.ParentFrame; } }

        /// <summary>
        /// List of frames within this frame.
        /// </summary>
        public new IFocusFrameList Items { get { return (IFocusFrameList)base.Items; } }

        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusNodeFrameVisibility Visibility { get; set; }

        /// <summary>
        /// Block frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusBlockFrameVisibility BlockVisibility { get; set; }

        /// <summary>
        /// Indicates that this is the preferred frame when restoring the focus.
        /// (Set in Xaml)
        /// </summary>
        public bool IsPreferred { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        public override bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable)
        {
            bool IsValid = true;

            IsValid &= base.IsValid(nodeType, nodeTemplateTable);
            IsValid &= Visibility == null || Visibility.IsValid(nodeType);
            IsValid &= BlockVisibility == null || BlockVisibility.IsValid(nodeType);

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public override IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            ((IFocusCellViewTreeContext)context).UpdateNodeFrameVisibility(this, out bool OldFrameVisibility);

            IFocusCellViewCollection EmbeddingCellView = base.BuildNodeCells(context, parentCellView) as IFocusCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            if (!((IFocusCellViewTreeContext)context).IsVisible)
            {
                Debug.Assert(!EmbeddingCellView.HasVisibleCellView);
            }

            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldFrameVisibility);

            return EmbeddingCellView;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public override IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context)
        {
            ((IFocusCellViewTreeContext)context).UpdateBlockFrameVisibility(this, out bool OldFrameVisibility);

            IFocusCellViewCollection EmbeddingCellView = base.BuildBlockCells(context) as IFocusCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            if (!((IFocusCellViewTreeContext)context).IsVisible)
            {
                Debug.Assert(!EmbeddingCellView.HasVisibleCellView);
            }

            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldFrameVisibility);

            return EmbeddingCellView;
        }

        /// <summary></summary>
        private protected override IFrameCellView BuildBlockCellsForPlaceholderFrame(IFrameCellViewTreeContext context, IFramePlaceholderFrame frame, IFrameCellViewCollection embeddingCellView, IFrameBlockState blockState)
        {
            Type OldSelectorType = null;
            string OldSelectorName = null;
            ((IFocusCellViewTreeContext)context).AddOrReplaceSelectors(((IFocusPlaceholderFrame)frame).Selectors, out OldSelectorType, out OldSelectorName);

            IFrameCellView ItemCellView = base.BuildBlockCellsForPlaceholderFrame(context, frame, embeddingCellView, blockState);

            ((IFocusCellViewTreeContext)context).RemoveOrRestoreSelectors(((IFocusPlaceholderFrame)frame).Selectors, OldSelectorType, OldSelectorName);

            return ItemCellView;
        }

        /// <summary>
        /// Returns the frame associated to a property if can have selectors.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for.</param>
        /// <param name="frame">Frame found upon return. Null if not matching <paramref name="propertyName"/>.</param>
        public virtual bool FrameSelectorForProperty(string propertyName, out IFocusNodeFrameWithSelector frame)
        {
            foreach (IFocusFrame Item in Items)
                if (Item is IFocusNodeFrame AsNodeFrame)
                    if (AsNodeFrame.FrameSelectorForProperty(propertyName, out frame))
                        return true;

            frame = null;
            return false;
        }

        /// <summary>
        /// Gets preferred frames to receive the focus when the source code is changed.
        /// </summary>
        /// <param name="firstPreferredFrame">The first preferred frame found.</param>
        /// <param name="lastPreferredFrame">The last preferred frame found.</param>
        public virtual void GetPreferredFrame(ref IFocusNodeFrame firstPreferredFrame, ref IFocusNodeFrame lastPreferredFrame)
        {
            foreach (IFocusFrame Item in Items)
                if (Item is IFocusNodeFrame AsNodeFrame)
                    AsNodeFrame.GetPreferredFrame(ref firstPreferredFrame, ref lastPreferredFrame);
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateEmbeddingCellView(IFrameCellViewTreeContext context, IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(((IFocusCellViewCollection)embeddingCellView).StateView == ((IFocusCellViewTreeContext)context).StateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFrameList object.
        /// </summary>
        private protected override IFrameFrameList CreateItems()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPanelFrame));
            return new FocusFrameList();
        }

        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        private protected override IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPanelFrame));
            return new FocusCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPanelFrame));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView);
        }
        #endregion
    }
}
