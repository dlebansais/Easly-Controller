namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// A selection of part of a comment.
    /// </summary>
    public interface ILayoutCommentSelection : IFocusCommentSelection, ILayoutTextSelection
    {
    }

    /// <summary>
    /// A selection of part of a comment.
    /// </summary>
    public class LayoutCommentSelection : FocusCommentSelection, ILayoutCommentSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutCommentSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="start">Index of the first character in the selected text.</param>
        /// <param name="end">Index following the last character in the selected text.</param>
        public LayoutCommentSelection(ILayoutNodeStateView stateView, int start, int end)
            : base(stateView, start, end)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view that encompasses the selection.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Prints the selection.
        /// </summary>
        public virtual void Print()
        {
            ILayoutControllerView ControllerView = StateView.ControllerView;
            Debug.Assert(ControllerView.PrintContext != null);
            ControllerView.UpdateLayout();

            Debug.Assert(RegionHelper.IsValid(StateView.ActualCellSize));

            ILayoutTemplateSet TemplateSet = ControllerView.TemplateSet;
            IList<IFocusFrameSelectorList> SelectorStack = StateView.GetSelectorStack();
            ILayoutCommentFrame Frame = (ILayoutCommentFrame)TemplateSet.GetCommentFrame(StateView.State, SelectorStack);
            Debug.Assert(Frame != null);

            string Text = CommentHelper.Get(StateView.State.Node.Documentation);
            Debug.Assert(Text != null);

            Debug.Assert(Start <= End);
            Debug.Assert(End <= Text.Length);

            Frame.Print(ControllerView.PrintContext, Text.Substring(Start, End - Start), Point.Origin);
        }
        #endregion
    }
}
