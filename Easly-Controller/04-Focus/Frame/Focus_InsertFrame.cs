﻿using EaslyController.Frame;
using System;
using System.Windows.Markup;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus for bringing the focus to an insertion point.
    /// </summary>
    public interface IFocusInsertFrame : IFrameInsertFrame, IFocusStaticFrame
    {
    }

    /// <summary>
    /// Focus for bringing the focus to an insertion point.
    /// </summary>
    [ContentProperty("CollectionName")]
    public class FocusInsertFrame : FrameInsertFrame, IFocusInsertFrame
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
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusNodeFrameVisibility Visibility { get; set; }
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

            return true;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in which cells are created.</param>
        /// <param name="stateView">The state view containing <paramref name="blockStateView"/> for which to create cells.</param>
        /// <param name="blockStateView">The block state view for which to create cells.</param>
        public override IFrameCellView BuildNodeCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            if (Visibility != null && !Visibility.IsVisible((IFocusControllerView)controllerView, (IFocusNodeStateView)stateView, this))
                return CreateEmptyCellView((IFocusNodeStateView)stateView);

            return base.BuildNodeCells(controllerView, stateView, parentCellView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFocusableCellView object.
        /// </summary>
        protected override IFrameVisibleCellView CreateFrameCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusInsertFrame));
            return new FocusFocusableCellView((IFocusNodeStateView)stateView, this);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusHorizontalCollectionPlaceholderFrame));
            return new FocusEmptyCellView(stateView);
        }
        #endregion
    }
}