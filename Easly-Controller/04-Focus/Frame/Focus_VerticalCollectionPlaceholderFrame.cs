using EaslyController.Frame;
using System;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus for a placeholder node in a block list displayed vertically.
    /// </summary>
    public interface IFocusVerticalCollectionPlaceholderFrame : IFrameVerticalCollectionPlaceholderFrame, IFocusCollectionPlaceholderFrame, IFocusNodeFrameWithSelector
    {
    }

    /// <summary>
    /// Focus for a placeholder node in a block list displayed vertically.
    /// </summary>
    public class FocusVerticalCollectionPlaceholderFrame : FrameVerticalCollectionPlaceholderFrame, IFocusVerticalCollectionPlaceholderFrame
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FocusVerticalCollectionPlaceholderFrame"/> object.
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

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        protected override IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusVerticalCollectionPlaceholderFrame));
            return new FocusCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusVerticalCollectionPlaceholderFrame));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusVerticalCollectionPlaceholderFrame));
            return new FocusColumn((IFocusNodeStateView)stateView, (IFocusCellViewList)list);
        }

        /// <summary>
        /// Creates a IxxxFrameSelectorList object.
        /// </summary>
        protected virtual IFocusFrameSelectorList CreateEmptySelectorList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusVerticalCollectionPlaceholderFrame));
            return new FocusFrameSelectorList();
        }
        #endregion
    }
}
