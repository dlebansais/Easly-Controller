﻿namespace EaslyController.Layout
{
    using System;
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Layout for a block list displayed vertically.
    /// </summary>
    public interface ILayoutVerticalBlockListFrame : IFocusVerticalBlockListFrame, ILayoutBlockListFrame
    {
    }

    /// <summary>
    /// Layout for a block list displayed vertically.
    /// </summary>
    public class LayoutVerticalBlockListFrame : FocusVerticalBlockListFrame, ILayoutVerticalBlockListFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new ILayoutTemplate ParentTemplate { get { return (ILayoutTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new ILayoutFrame ParentFrame { get { return (ILayoutFrame)base.ParentFrame; } }

        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutNodeFrameVisibility Visibility { get { return (ILayoutNodeFrameVisibility)base.Visibility; } set { base.Visibility = value; } }

        /// <summary>
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutFrameSelectorList Selectors { get { return (ILayoutFrameSelectorList)base.Selectors; } }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateEmbeddingCellView(IFrameCellViewTreeContext context, IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(((ILayoutCellViewCollection)embeddingCellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
        }

        /// <summary></summary>
        private protected override void ValidateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockCellView blockCellView)
        {
            Debug.Assert(((ILayoutBlockCellView)blockCellView).StateView == (ILayoutNodeStateView)stateView);
            Debug.Assert(((ILayoutBlockCellView)blockCellView).ParentCellView == (ILayoutCellViewCollection)parentCellView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewList object.
        /// </summary>
        private protected override IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutVerticalBlockListFrame));
            return new LayoutCellViewList();
        }

        /// <summary>
        /// Creates a IxxxBlockCellView object.
        /// </summary>
        private protected override IFrameBlockCellView CreateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockStateView blockStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutVerticalBlockListFrame));
            return new LayoutBlockCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, (ILayoutBlockStateView)blockStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutVerticalBlockListFrame));
            return new LayoutColumn((ILayoutNodeStateView)stateView, (ILayoutCellViewList)list);
        }

        /// <summary>
        /// Creates a IxxxFrameSelectorList object.
        /// </summary>
        private protected override IFocusFrameSelectorList CreateEmptySelectorList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutVerticalBlockListFrame));
            return new LayoutFrameSelectorList();
        }
        #endregion
    }
}
