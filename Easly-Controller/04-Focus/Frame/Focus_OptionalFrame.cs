﻿using EaslyController.Frame;
using System;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus for describing an optional child node.
    /// </summary>
    public interface IFocusOptionalFrame : IFrameOptionalFrame, IFocusNamedFrame, IFocusNodeFrame
    {
        /// <summary>
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        IFocusFrameSelectorList Selectors { get; }
    }

    /// <summary>
    /// Focus for describing an optional child node.
    /// </summary>
    public class FocusOptionalFrame : FrameOptionalFrame, IFocusOptionalFrame
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FocusOptionalFrame"/> object.
        /// </summary>
        public FocusOptionalFrame()
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
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusNodeFrameVisibility Visibility { get; set; }

        /// <summary>
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        public IFocusFrameSelectorList Selectors { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            if (Visibility != null && !Visibility.IsValid(nodeType))
                return false;

            foreach (IFocusFrameSelector FrameSelector in Selectors)
                if (!FrameSelector.IsValid(nodeType, PropertyName))
                    return false;

            return true;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public override IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            ((IFocusCellViewTreeContext)context).UpdateNodeFrameVisibility(this, out bool OldFrameVisibility);

            IFocusCellView EmbeddingCellView = base.BuildNodeCells(context, parentCellView) as IFocusCellView;
            Debug.Assert(EmbeddingCellView != null);

            if (!((IFocusCellViewTreeContext)context).IsVisible)
            {
                Debug.Assert(((EmbeddingCellView is IFocusContainerCellView AsContainer) && AsContainer.ChildStateView.RootCellView is IFocusEmptyCellView) || (EmbeddingCellView is IFocusEmptyCellView));
            }

            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldFrameVisibility);

            return EmbeddingCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusOptionalFrame));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView);
        }

        /// <summary>
        /// Creates a IxxxFrameSelectorList object.
        /// </summary>
        protected virtual IFocusFrameSelectorList CreateEmptySelectorList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusOptionalFrame));
            return new FocusFrameSelectorList();
        }
        #endregion
    }
}
