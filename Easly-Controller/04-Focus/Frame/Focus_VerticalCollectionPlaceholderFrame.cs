namespace EaslyController.Focus
{
    using System;
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// Frame for a placeholder node in a block list displayed vertically.
    /// </summary>
    public interface IFocusVerticalCollectionPlaceholderFrame : IFrameVerticalCollectionPlaceholderFrame, IFocusCollectionPlaceholderFrame, IFocusFrameWithSelector
    {
    }

    /// <summary>
    /// Frame for a placeholder node in a block list displayed vertically.
    /// </summary>
    public class FocusVerticalCollectionPlaceholderFrame : FrameVerticalCollectionPlaceholderFrame, IFocusVerticalCollectionPlaceholderFrame
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusVerticalCollectionPlaceholderFrame"/> class.
        /// </summary>
        public FocusVerticalCollectionPlaceholderFrame()
        {
            Selectors = CreateEmptySelectorList();
        }
        #endregion

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
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        public FocusFrameSelectorList Selectors { get; }
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

            foreach (IFocusFrameSelector Selector in Selectors)
                IsValid &= Selector.IsValid(nodeType, (FocusTemplateReadOnlyDictionary)nodeTemplateTable, nameof(BaseNode.IBlock.NodeList));

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        public override IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            Type OldSelectorType = null;
            string OldSelectorName = null;
            ((IFocusCellViewTreeContext)context).AddOrReplaceSelectors(Selectors, out OldSelectorType, out OldSelectorName);

            IFocusCellViewCollection EmbeddingCellView = base.BuildBlockCells(context, parentCellView) as IFocusCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            ((IFocusCellViewTreeContext)context).RemoveOrRestoreSelectors(Selectors, OldSelectorType, OldSelectorName);

            return EmbeddingCellView;
        }
        #endregion

        #region Implementation
        private protected override void ValidateEmbeddingCellView(IFrameCellViewTreeContext context, IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(((IFocusCellViewCollection)embeddingCellView).StateView == ((IFocusCellViewTreeContext)context).StateView);
            IFocusCellViewCollection ParentCellView = ((IFocusCellViewCollection)embeddingCellView).ParentCellView;
        }

        private protected override void ValidateContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameContainerCellView containerCellView)
        {
            Debug.Assert(((IFocusContainerCellView)containerCellView).StateView == (IFocusNodeStateView)stateView);
            Debug.Assert(((IFocusContainerCellView)containerCellView).ParentCellView == (IFocusCellViewCollection)parentCellView);
            Debug.Assert(((IFocusContainerCellView)containerCellView).ChildStateView == (IFocusNodeStateView)childStateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        private protected override FrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusVerticalCollectionPlaceholderFrame));
            return new FocusCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusVerticalCollectionPlaceholderFrame));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView, this);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, FrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusVerticalCollectionPlaceholderFrame));
            return new FocusColumn((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (FocusCellViewList)list, this);
        }

        /// <summary>
        /// Creates a IxxxFrameSelectorList object.
        /// </summary>
        private protected virtual FocusFrameSelectorList CreateEmptySelectorList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusVerticalCollectionPlaceholderFrame));
            return new FocusFrameSelectorList();
        }
        #endregion
    }
}
