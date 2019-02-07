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

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(cellView.StateView == context.StateView);
            Debug.Assert(cellView.Frame == this);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxVisibleCellView object.
        /// </summary>
        private protected override IFrameVisibleCellView CreateVisibleCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameKeywordFrame));
            return new FrameVisibleCellView(stateView, this);
        }
        #endregion
    }
}
