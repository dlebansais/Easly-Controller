namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Markup;
    using EaslyController.Frame;
    using NotNullReflection;

    /// <summary>
    /// Frame for displaying more frames horizontally.
    /// </summary>
    public interface IFocusHorizontalPanelFrame : IFrameHorizontalPanelFrame, IFocusPanelFrame
    {
    }

    /// <summary>
    /// Frame for displaying more frames horizontally.
    /// </summary>
    [ContentProperty("Items")]
    public class FocusHorizontalPanelFrame : FrameHorizontalPanelFrame, IFocusHorizontalPanelFrame
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
        public new FocusFrameList Items { get { return (FocusFrameList)base.Items; } }

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
        /// <param name="commentFrameCount">Number of comment frames found so far.</param>
        public override bool IsValid(Type nodeType, FrameTemplateReadOnlyDictionary nodeTemplateTable, ref int commentFrameCount)
        {
            bool IsValid = true;

            IsValid &= base.IsValid(nodeType, nodeTemplateTable, ref commentFrameCount);
            IsValid &= Visibility == null || Visibility.IsValid(nodeType);
            IsValid &= BlockVisibility == null || BlockVisibility.IsValid(nodeType);

#if DEBUG
            // For code coverage purpose.
            CollectSelectors(new Dictionary<string, FocusFrameSelectorList>());
#endif

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

            IFocusCellViewCollection EmbeddingCellView;
            EmbeddingCellView = base.BuildNodeCells(context, parentCellView) as IFocusCellViewCollection;

            bool HasVisibleCellView = EmbeddingCellView.HasVisibleCellView;
            Debug.Assert(((IFocusCellViewTreeContext)context).IsVisible || !HasVisibleCellView);

            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldFrameVisibility);

            return EmbeddingCellView;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        public override IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            ((IFocusCellViewTreeContext)context).UpdateBlockFrameVisibility(this, out bool OldFrameVisibility);

            IFocusCellViewCollection EmbeddingCellView;
            EmbeddingCellView = base.BuildBlockCells(context, parentCellView) as IFocusCellViewCollection;

            bool HasVisibleCellView = EmbeddingCellView.HasVisibleCellView;
            Debug.Assert(((IFocusCellViewTreeContext)context).IsVisible || !HasVisibleCellView);

            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldFrameVisibility);

            return EmbeddingCellView;
        }

        private protected override IFrameCellView BuildBlockCellsForPlaceholderFrame(IFrameCellViewTreeContext context, IFramePlaceholderFrame frame, IFrameCellViewCollection embeddingCellView, IFrameBlockState blockState)
        {
            ((IFocusCellViewTreeContext)context).AddOrReplaceSelectors(((IFocusPlaceholderFrame)frame).Selectors, out Type OldSelectorType, out string OldSelectorName);

            IFrameCellView ItemCellView = base.BuildBlockCellsForPlaceholderFrame(context, frame, embeddingCellView, blockState);

            bool HasVisibleCellView = ItemCellView.HasVisibleCellView;
            Debug.Assert(((IFocusCellViewTreeContext)context).IsVisible || !HasVisibleCellView);

            ((IFocusCellViewTreeContext)context).RemoveOrRestoreSelectors(((IFocusPlaceholderFrame)frame).Selectors, OldSelectorType, OldSelectorName);

            return ItemCellView;
        }

        /// <summary>
        /// Returns the frame associated to a property if it can have selectors.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for.</param>
        /// <param name="frame">Frame found upon return. Null if not matching <paramref name="propertyName"/>.</param>
        public virtual bool FrameSelectorForProperty(string propertyName, out IFocusFrameWithSelector frame)
        {
            frame = null;
            bool IsFound = false;

            foreach (IFocusFrame Item in Items)
                if (Item is IFocusSelectorPropertyFrame AsSelectorPropertyFrame)
                    if (AsSelectorPropertyFrame.FrameSelectorForProperty(propertyName, out frame))
                    {
                        IsFound = true;
                        break;
                    }

            return IsFound;
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

        /// <summary>
        /// Gets selectors in the frame and nested frames.
        /// </summary>
        /// <param name="selectorTable">The table of selectors to update.</param>
        public virtual void CollectSelectors(Dictionary<string, FocusFrameSelectorList> selectorTable)
        {
            foreach (IFocusFrame Item in Items)
                if (Item is IFocusNodeFrame AsNodeFrame)
                    AsNodeFrame.CollectSelectors(selectorTable);
        }
        #endregion

        #region Implementation
        private protected override void ValidateEmbeddingCellView(IFrameCellViewTreeContext context, IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(((IFocusCellViewCollection)embeddingCellView).StateView == ((IFocusCellViewTreeContext)context).StateView);
            IFocusCellViewCollection ParentCellView = ((IFocusCellViewCollection)embeddingCellView).ParentCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFrameList object.
        /// </summary>
        private protected override FrameFrameList CreateItems()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusHorizontalPanelFrame>());
            return new FocusFrameList();
        }

        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        private protected override FrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusHorizontalPanelFrame>());
            return new FocusCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFramePlaceholderFrame frame)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusHorizontalPanelFrame>());
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView, (IFocusPlaceholderFrame)frame);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusHorizontalPanelFrame>());
            return new FocusLine((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (FocusCellViewList)list, this);
        }
        #endregion
    }
}
