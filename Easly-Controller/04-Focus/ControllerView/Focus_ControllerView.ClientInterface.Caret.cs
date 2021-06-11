namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public partial class FocusControllerView : FrameControllerView, IFocusControllerView, IFocusInternalControllerView
    {
        /// <summary>
        /// Changes the caret position. Does nothing if the focus isn't on a string property.
        /// </summary>
        /// <param name="position">The new position.</param>
        /// <param name="resetAnchor">If true, resets the selected text anchor.</param>
        /// <param name="isMoved">True if the position was changed. False otherwise.</param>
        public virtual void SetCaretPosition(int position, bool resetAnchor, out bool isMoved)
        {
            isMoved = false;
            ulong OldFocusHash = FocusHash;

            if (Focus is IFocusTextFocus AsTextFocus)
            {
                string Text = GetFocusedText(AsTextFocus);
                SetTextCaretPosition(Text, position, resetAnchor, out isMoved);
            }

            Debug.Assert(isMoved || resetAnchor || OldFocusHash == FocusHash);
        }

        private protected virtual void SetTextCaretPosition(string text, int position, bool resetAnchor, out bool isMoved)
        {
            int OldPosition = CaretPosition;

            if (position <= 0)
                CaretPosition = 0;
            else if (position >= text.Length)
                CaretPosition = text.Length;
            else
                CaretPosition = position;

            if (resetAnchor)
            {
                ResetSelection();
                CaretAnchorPosition = CaretPosition;
            }
            else if (Selection == EmptySelection)
            {
                IFocusTextFocus AsTextFocus = Focus as IFocusTextFocus;
                Debug.Assert(AsTextFocus != null);

                SetTextSelection(AsTextFocus);
            }
            else if (Selection is IFocusTextSelection AsTextSelection)
                AsTextSelection.Update(CaretAnchorPosition, CaretPosition);

            CheckCaretInvariant(text);

            isMoved = CaretPosition != OldPosition;
        }

        /// <summary>
        /// Changes the caret mode.
        /// </summary>
        /// <param name="mode">The new mode.</param>
        /// <param name="isChanged">True if the mode was changed.</param>
        public virtual void SetCaretMode(CaretModes mode, out bool isChanged)
        {
            isChanged = false;
            ulong OldFocusHash = FocusHash;

            if (CaretMode != mode)
            {
                CaretMode = mode;
                isChanged = true;

                CheckCaretInvariant();
            }

            Debug.Assert(isChanged || OldFocusHash == FocusHash);
        }
    }
}
