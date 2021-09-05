namespace EaslyController.Focus
{
    using System.Diagnostics;
    using System.Windows;
    using BaseNodeHelper;
    using EaslyController.Controller;

    /// <summary>
    /// A selection of part of a comment.
    /// </summary>
    public interface IFocusCommentSelection : IFocusTextSelection
    {
    }

    /// <summary>
    /// A selection of part of a comment.
    /// </summary>
    public class FocusCommentSelection : FocusSelection, IFocusCommentSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCommentSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="start">Index of the first character in the selected text.</param>
        /// <param name="end">Index following the last character in the selected text.</param>
        public FocusCommentSelection(IFocusNodeStateView stateView, int start, int end)
            : base(stateView)
        {
            if (start <= end)
            {
                Start = start;
                End = end;
            }
            else
            {
                Start = end;
                End = start;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the first character in the selected text.
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// Index following the last character in the selected text.
        /// </summary>
        public int End { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates the text selection with new start and end values.
        /// </summary>
        /// <param name="start">The new start value.</param>
        /// <param name="end">The new end value.</param>
        public virtual void Update(int start, int end)
        {
            if (start <= end)
            {
                Start = start;
                End = end;
            }
            else
            {
                Start = end;
                End = start;
            }
        }

        /// <summary>
        /// Copy the selection in the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        public override void Copy(IDataObject dataObject)
        {
            string Content = CommentHelper.Get(StateView.State.Node.Documentation);
            Debug.Assert(Content != null);
            Debug.Assert(Start <= End);
            Debug.Assert(End <= Content.Length);

            dataObject.SetData(typeof(string), Content.Substring(Start, End - Start));
        }

        /// <summary>
        /// Copy the selection in the clipboard then removes it.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        /// <param name="isDeleted">True if something was deleted.</param>
        public override void Cut(IDataObject dataObject, out bool isDeleted)
        {
            Debug.Assert(dataObject != null);

            CutOrDelete(dataObject, out isDeleted);
        }

        /// <summary>
        /// Replaces the selection with the content of the clipboard.
        /// </summary>
        /// <param name="isChanged">True if something was replaced or added.</param>
        public override void Paste(out bool isChanged)
        {
            isChanged = false;

            if (ClipboardHelper.TryReadText(out string Text) && Text.Length > 0)
            {
                string Content = CommentHelper.Get(StateView.State.Node.Documentation);
                Debug.Assert(Content != null);
                Debug.Assert(Start <= End);
                Debug.Assert(End <= Content.Length);

                Content = Content.Substring(0, Start) + Text + Content.Substring(End);

                FocusController Controller = StateView.ControllerView.Controller;
                int OldCaretPosition = StateView.ControllerView.CaretPosition;
                int NewCaretPosition = Start + Text.Length;
                Controller.ChangeCommentAndCaretPosition(StateView.State.ParentIndex, Content, OldCaretPosition, NewCaretPosition, false);

                StateView.ControllerView.ClearSelection();
                isChanged = true;
            }
        }

        /// <summary>
        /// Deletes the selection.
        /// </summary>
        /// <param name="isDeleted">True if something was deleted.</param>
        public override void Delete(out bool isDeleted)
        {
            CutOrDelete(null, out isDeleted);
        }

        private void CutOrDelete(IDataObject dataObject, out bool isDeleted)
        {
            isDeleted = false;

            string Content = CommentHelper.Get(StateView.State.Node.Documentation);
            Debug.Assert(Content != null);
            Debug.Assert(Start <= End);
            Debug.Assert(End <= Content.Length);

            if (Start < End)
            {
                if (dataObject != null)
                    dataObject.SetData(typeof(string), Content.Substring(Start, End - Start));

                Content = Content.Substring(0, Start) + Content.Substring(End);

                FocusController Controller = StateView.ControllerView.Controller;
                int OldCaretPosition = StateView.ControllerView.CaretPosition;
                int NewCaretPosition = Start;
                Controller.ChangeCommentAndCaretPosition(StateView.State.ParentIndex, Content, OldCaretPosition, NewCaretPosition, true);

                StateView.ControllerView.ClearSelection();
                isDeleted = true;
            }
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"Comment from {Start} to {End}";
        }
        #endregion
    }
}
