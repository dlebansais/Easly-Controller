using EaslyController.Frame;
using System;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus for describing an optional child node.
    /// </summary>
    public interface IFocusOptionalFrame : IFrameOptionalFrame, IFocusNamedFrame, IFocusNodeFrame
    {
    }

    /// <summary>
    /// Focus for describing an optional child node.
    /// </summary>
    public class FocusOptionalFrame : FrameOptionalFrame, IFocusOptionalFrame
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
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public override IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            ((IFocusCellViewTreeContext)context).UpdateNodeFrameVisibility(this, out bool OldNodeFrameVisibility);

            IFocusCellView EmbeddingCellView = base.BuildNodeCells(context, parentCellView) as IFocusCellView;
            Debug.Assert(EmbeddingCellView != null);

            IFocusCellView Result;
            if (((IFocusCellViewTreeContext)context).IsVisible)
                Result = EmbeddingCellView;
            else
            {
                Debug.Assert(((EmbeddingCellView is IFocusContainerCellView AsContainer) && AsContainer.ChildStateView.RootCellView is IFocusEmptyCellView) || (EmbeddingCellView is IFocusEmptyCellView));
                EmbeddingCellView.ClearCellTree();

                Result = CreateEmptyCellView(((IFocusCellViewTreeContext)context).StateView);
            }

            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldNodeFrameVisibility);

            return Result;
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
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusOptionalFrame));
            return new FocusEmptyCellView(stateView);
        }
        #endregion
    }
}
