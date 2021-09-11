namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public partial class LayoutControllerView : FocusControllerView, ILayoutInternalControllerView
    {
        /// <summary>
        /// The controller.
        /// </summary>
        public new LayoutController Controller { get { return (LayoutController)base.Controller; } }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        public new LayoutNodeStateViewDictionary StateViewTable { get { return (LayoutNodeStateViewDictionary)base.StateViewTable; } }

        /// <summary>
        /// Table of views of each block state in the controller.
        /// </summary>
        public new LayoutBlockStateViewDictionary BlockStateViewTable { get { return (LayoutBlockStateViewDictionary)base.BlockStateViewTable; } }

        /// <summary>
        /// State view of the root state.
        /// </summary>
        public new ILayoutNodeStateView RootStateView { get { return (ILayoutNodeStateView)base.RootStateView; } }

        /// <summary>
        /// Template set describing the node tree.
        /// </summary>
        public new ILayoutTemplateSet TemplateSet { get { return (ILayoutTemplateSet)base.TemplateSet; } }

        /// <summary>
        /// Cell view with the focus.
        /// </summary>
        public new ILayoutFocus Focus { get { return (ILayoutFocus)base.Focus; } }

        /// <summary>
        /// The current selection.
        /// </summary>
        public new ILayoutSelection Selection { get { return (ILayoutSelection)base.Selection; } }

        /// <summary>
        /// The measure context.
        /// </summary>
        public ILayoutMeasureContext MeasureContext { get; private set; }

        /// <summary>
        /// The draw context.
        /// </summary>
        public ILayoutDrawContext DrawContext { get; private set; }

        /// <summary>
        /// The print context.
        /// </summary>
        public ILayoutPrintContext PrintContext { get; private set; }

        /// <summary>
        /// Size of view.
        /// </summary>
        public Size ViewSize
        {
            get
            {
                if (IsInvalidated)
                    MeasureAndArrange();

                return InternalViewSize;
            }
        }
        private Size InternalViewSize;

        /// <summary>
        /// Current text style if the focus is on a string property. Default otherwise.
        /// </summary>
        public TextStyles FocusedTextStyle
        {
            get
            {
                TextStyles Result = TextStyles.Default;
                bool IsHandled = false;

                if (Focus is ILayoutStringContentFocus AsText)
                {
                    switch (AsText.CellView.Frame)
                    {
                        case ILayoutCharacterFrame AsCharacterFrame:
                            Result = TextStyles.Character;
                            IsHandled = true;
                            break;

                        case ILayoutNumberFrame AsNumberFrame:
                            Result = TextStyles.Number;
                            IsHandled = true;
                            break;

                        case ILayoutTextValueFrame AsTextValueFrame:
                            Result = AsTextValueFrame.TextStyle;
                            IsHandled = true;
                            break;
                    }
                }
                else
                    IsHandled = true;

                Debug.Assert(IsHandled);
                return Result;
            }
        }

        /// <summary>
        /// Displayed caret mode.
        /// </summary>
        public CaretModes ActualCaretMode { get { return CaretPosition >= 0 && CaretPosition < MaxCaretPosition ? CaretMode : CaretModes.Insertion; } }

        /// <summary>
        /// Indicates if the caret is shown or hidden.
        /// </summary>
        public bool IsCaretShown { get; private set; }

        /// <summary>
        /// Indicates if there are cells that must be measured and arranged.
        /// </summary>
        public bool IsInvalidated { get; private set; }

        /// <summary>
        /// Shows a comment sign over comments in <see cref="CommentDisplayModes.OnFocus"/> mode.
        /// </summary>
        public bool ShowUnfocusedComments { get; private set; }

        /// <summary>
        /// Shows block geometry around blocks.
        /// </summary>
        public bool ShowBlockGeometry { get; private set; }

        /// <summary>
        /// Shows line numbers.
        /// </summary>
        public bool ShowLineNumber { get; private set; }
    }
}
