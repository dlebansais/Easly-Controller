namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;
    using System.Windows.Markup;

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public interface IFrameKeywordFrame : IFrameStaticFrame, IFrameBlockFrame
    {
        /// <summary>
        /// Free text.
        /// (Set in Xaml)
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// True if the keyword should be allowed to receive the focus.
        /// (Set in Xaml)
        /// </summary>
        bool IsFocusable { get; set; }
    }

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    [ContentProperty("Text")]
    public class FrameKeywordFrame : FrameStaticFrame, IFrameKeywordFrame
    {
        #region Properties
        /// <summary>
        /// Free text.
        /// (Set in Xaml)
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// True if the keyword should be allowed to receive the focus.
        /// (Set in Xaml)
        /// </summary>
        public bool IsFocusable { get; set; }

        /// <summary></summary>
        private protected override bool IsFrameFocusable { get { return true; } }
        #endregion

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
            IsValid &= !string.IsNullOrEmpty(Text);

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public virtual IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context)
        {
            IFrameVisibleCellView CellView = CreateVisibleCellView(context.StateView);
            ValidateVisibleCellView(context, CellView);

            return CellView;
        }
        #endregion
    }
}
