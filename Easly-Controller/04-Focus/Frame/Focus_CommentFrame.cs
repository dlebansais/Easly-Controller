namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;

    /// <summary>
    /// Frame to display comments.
    /// </summary>
    public interface IFocusCommentFrame : IFrameCommentFrame, IFocusNodeFrame, IFocusBlockFrame
    {
    }

    /// <summary>
    /// Frame to display comments.
    /// </summary>
    public class FocusCommentFrame : FrameCommentFrame, IFocusCommentFrame
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
        #endregion

        #region Client Interface
        /// <summary>
        /// Gets preferred frames to receive the focus when the source code is changed.
        /// </summary>
        /// <param name="firstPreferredFrame">The first preferred frame found.</param>
        /// <param name="lastPreferredFrame">The last preferred frame found.</param>
        public virtual void GetPreferredFrame(ref IFocusNodeFrame firstPreferredFrame, ref IFocusNodeFrame lastPreferredFrame)
        {
        }

        /// <summary>
        /// Gets selectors in the frame and nested frames.
        /// </summary>
        /// <param name="selectorTable">The table of selectors to update.</param>
        public virtual void CollectSelectors(Dictionary<string, FocusFrameSelectorList> selectorTable)
        {
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        protected override bool IsDisplayed(IFrameCellViewTreeContext context, string text)
        {
            bool IsCommentForced;
            ((IFocusCellViewTreeContext)context).CheckCommentForced(out IsCommentForced);

            if (text == null && !IsCommentForced)
                return false;

            if (context.ControllerView.CommentDisplayMode != Constants.CommentDisplayModes.OnFocus && context.ControllerView.CommentDisplayMode != Constants.CommentDisplayModes.All)
                return false;

            return true;
        }

        private protected override void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(((IFocusVisibleCellView)cellView).StateView == ((IFocusCellViewTreeContext)context).StateView);
            Debug.Assert(((IFocusVisibleCellView)cellView).Frame == this);
            IFocusCellViewCollection ParentCellView = ((IFocusVisibleCellView)cellView).ParentCellView;
        }

        private protected override void ValidateEmptyCellView(IFrameCellViewTreeContext context, IFrameEmptyCellView emptyCellView)
        {
            Debug.Assert(((IFocusEmptyCellView)emptyCellView).StateView == ((IFocusCellViewTreeContext)context).StateView);
            IFocusCellViewCollection ParentCellView = ((IFocusEmptyCellView)emptyCellView).ParentCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCommentCellView object.
        /// </summary>
        private protected override IFrameCommentCellView CreateCommentCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, Document documentation)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusCommentFrame));
            return new FocusCommentCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, this, documentation);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected override IFrameEmptyCellView CreateEmptyCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusCommentFrame));
            return new FocusEmptyCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView);
        }
        #endregion
    }
}
