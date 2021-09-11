namespace EaslyController.Focus
{
    using System.Diagnostics;
    using EaslyController.Constants;
    using EaslyController.Frame;

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public partial class FocusControllerView : FrameControllerView, IFocusInternalControllerView
    {
        /// <summary>
        /// The controller.
        /// </summary>
        public new FocusController Controller { get { return (FocusController)base.Controller; } }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        public new FocusNodeStateViewDictionary StateViewTable { get { return (FocusNodeStateViewDictionary)base.StateViewTable; } }

        /// <summary>
        /// Table of views of each block state in the controller.
        /// </summary>
        public new FocusBlockStateViewDictionary BlockStateViewTable { get { return (FocusBlockStateViewDictionary)base.BlockStateViewTable; } }

        /// <summary>
        /// State view of the root state.
        /// </summary>
        public new IFocusNodeStateView RootStateView { get { return (IFocusNodeStateView)base.RootStateView; } }

        /// <summary>
        /// Template set describing the node tree.
        /// </summary>
        public new IFocusTemplateSet TemplateSet { get { return (IFocusTemplateSet)base.TemplateSet; } }

        /// <summary>
        /// Cell view with the focus.
        /// </summary>
        public IFocusFocus Focus { get; private set; }

        private protected FocusFocusList FocusChain { get; private set; }

        /// <summary>
        /// Lowest valid value for <see cref="MoveFocus"/>.
        /// </summary>
        public int MinFocusMove
        {
            get
            {
                int FocusIndex = FocusChain.IndexOf(Focus);
                Debug.Assert(FocusIndex >= 0 && FocusIndex < FocusChain.Count);

                return -FocusIndex;
            }
        }

        /// <summary>
        /// Highest valid value for <see cref="MoveFocus"/>.
        /// </summary>
        public int MaxFocusMove
        {
            get
            {
                int FocusIndex = FocusChain.IndexOf(Focus);
                Debug.Assert(FocusIndex >= 0 && FocusIndex < FocusChain.Count);

                return FocusChain.Count - FocusIndex - 1;
            }
        }

        /// <summary>
        /// Position of the caret in a text with the focus, -1 otherwise.
        /// </summary>
        public int CaretPosition { get; private set; }

        /// <summary>
        /// Position of the caret anchor in a text with the focus, -1 otherwise.
        /// </summary>
        public int CaretAnchorPosition { get; private set; }

        /// <summary>
        /// Max position of the caret in a text with the focus, -1 otherwise.
        /// </summary>
        public int MaxCaretPosition { get; private set; }

        /// <summary>
        /// Current caret mode when editing text.
        /// </summary>
        public CaretModes CaretMode { get; private set; }

        /// <summary>
        /// Current text if the focus is on a string property or comment. Null otherwise.
        /// </summary>
        public string FocusedText
        {
            get
            {
                string Result = null;

                if (Focus is IFocusTextFocus AsTextFocus)
                    Result = GetFocusedText(AsTextFocus);

                return Result;
            }
        }

        /// <summary>
        /// Indicates if the node with the focus has all its frames forced to visible.
        /// </summary>
        public bool IsUserVisible { get { return Focus.CellView.StateView.IsUserVisible; } }

        /// <summary>
        /// The current selection.
        /// </summary>
        public IFocusSelection Selection { get; private set; }
        private IFocusEmptySelection EmptySelection;

        /// <summary>
        /// The anchor to use to calculate the selection.
        /// </summary>
        public IFocusNodeStateView SelectionAnchor { get; private set; }

        /// <summary>
        /// Gets how extended is the selection.
        /// </summary>
        public int SelectionExtension { get; private set; }

        /// <summary>
        /// True if the selection is empty.
        /// </summary>
        public bool IsSelectionEmpty { get { return Selection == EmptySelection; } }

        /// <summary>
        /// Current auto formatting mode.
        /// </summary>
        public AutoFormatModes AutoFormatMode { get; private set; }
    }
}
