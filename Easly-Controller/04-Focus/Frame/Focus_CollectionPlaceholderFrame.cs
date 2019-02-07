namespace EaslyController.Focus
{
    using System;
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// Base frame for a placeholder node in a block list.
    /// </summary>
    public interface IFocusCollectionPlaceholderFrame : IFrameCollectionPlaceholderFrame, IFocusFrame, IFocusBlockFrame, IFocusNodeFrameWithSelector
    {
    }

    /// <summary>
    /// Base frame for a placeholder node in a block list.
    /// </summary>
    public abstract class FocusCollectionPlaceholderFrame : FrameCollectionPlaceholderFrame, IFocusCollectionPlaceholderFrame
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCollectionPlaceholderFrame"/> class.
        /// </summary>
        public FocusCollectionPlaceholderFrame()
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
        /// Block frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusBlockFrameVisibility BlockVisibility { get; set; }

        /// <summary>
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        public IFocusFrameSelectorList Selectors { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public override IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context)
        {
            ((IFocusCellViewTreeContext)context).UpdateBlockFrameVisibility(this, out bool OldFrameVisibility);
            Type OldSelectorType = null;
            string OldSelectorName = null;
            ((IFocusCellViewTreeContext)context).AddOrReplaceSelectors(Selectors, out OldSelectorType, out OldSelectorName);

            IFocusCellViewCollection EmbeddingCellView = base.BuildBlockCells(context) as IFocusCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            if (!((IFocusCellViewTreeContext)context).IsVisible)
            {
                Debug.Assert(!EmbeddingCellView.HasVisibleCellView);
            }

            ((IFocusCellViewTreeContext)context).RemoveOrRestoreSelectors(Selectors, OldSelectorType, OldSelectorName);
            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldFrameVisibility);

            return EmbeddingCellView;
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateEmbeddingCellView(IFrameCellViewTreeContext context, IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(((IFocusCellViewCollection)embeddingCellView).StateView == ((IFocusCellViewTreeContext)context).StateView);
        }

        /// <summary></summary>
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
        private protected override IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusCollectionPlaceholderFrame));
            return new FocusCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusCollectionPlaceholderFrame));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView);
        }

        /// <summary>
        /// Creates a IxxxFrameSelectorList object.
        /// </summary>
        private protected virtual IFocusFrameSelectorList CreateEmptySelectorList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusCollectionPlaceholderFrame));
            return new FocusFrameSelectorList();
        }
        #endregion
    }
}
