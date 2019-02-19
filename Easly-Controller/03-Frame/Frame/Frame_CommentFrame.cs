namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Controller;

    /// <summary>
    /// Frame to display comments.
    /// </summary>
    public interface IFrameCommentFrame : IFrameFrame, IFrameNodeFrame, IFrameBlockFrame
    {
    }

    /// <summary>
    /// Frame to display comments.
    /// </summary>
    public class FrameCommentFrame : FrameFrame, IFrameCommentFrame
    {
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

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public virtual IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            IDocument Documentation = context.StateView.State.Node.Documentation;
            string Text = CommentHelper.Get(Documentation);

            if (Text != null)
            {
                IFrameVisibleCellView CellView = CreateCommentCellView(context.StateView, parentCellView, context.StateView.State.Node.Documentation);
                ValidateVisibleCellView(context, CellView);

                return CellView;
            }
            else
            {
                IFrameEmptyCellView CellView = CreateEmptyCellView(context.StateView, parentCellView);
                ValidateEmptyCellView(context, CellView);

                return CellView;
            }
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        public virtual IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            IDocument Documentation = context.BlockStateView.BlockState.ChildBlock.Documentation;
            string Text = CommentHelper.Get(Documentation);

            if (Text != null)
            {
                IFrameVisibleCellView CellView = CreateCommentCellView(context.StateView, parentCellView, Documentation);
                ValidateVisibleCellView(context, CellView);

                return CellView;
            }
            else
            {
                IFrameEmptyCellView CellView = CreateEmptyCellView(context.StateView, parentCellView);
                ValidateEmptyCellView(context, CellView);

                return CellView;
            }
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected virtual void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(cellView.StateView == context.StateView);
            Debug.Assert(cellView.Frame == this);
            IFrameCellViewCollection ParentCellView = cellView.ParentCellView;
        }

        /// <summary></summary>
        private protected virtual void ValidateEmptyCellView(IFrameCellViewTreeContext context, IFrameEmptyCellView emptyCellView)
        {
            Debug.Assert(emptyCellView.StateView == context.StateView);
            IFrameCellViewCollection ParentCellView = emptyCellView.ParentCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCommentCellView object.
        /// </summary>
        private protected virtual IFrameCommentCellView CreateCommentCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IDocument documentation)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameCommentFrame));
            return new FrameCommentCellView(stateView, parentCellView, this, documentation);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected virtual IFrameEmptyCellView CreateEmptyCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameCommentFrame));
            return new FrameEmptyCellView(stateView, parentCellView);
        }
        #endregion
    }
}
