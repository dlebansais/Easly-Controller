namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;
    using BaseNodeHelper;

    /// <summary>
    /// Frame describing a string value property in a node.
    /// </summary>
    public interface IFrameTextValueFrame : IFrameValueFrame
    {
    }

    /// <summary>
    /// Frame describing a string value property in a node.
    /// </summary>
    public class FrameTextValueFrame : FrameValueFrame, IFrameTextValueFrame
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
            IsValid &= NodeTreeHelper.IsStringProperty(nodeType, PropertyName);

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public override IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            IFrameVisibleCellView EmbeddingCellView = CreateVisibleCellView(context.StateView);
            ValidateVisibleCellView(context, EmbeddingCellView);

            return EmbeddingCellView;
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected virtual void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(cellView.StateView == context.StateView);
            Debug.Assert(cellView.Frame == this);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxTextFocusableCellView object.
        /// </summary>
        private protected virtual IFrameVisibleCellView CreateVisibleCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTextValueFrame));
            return new FrameTextFocusableCellView(stateView, this, PropertyName);
        }
        #endregion
    }
}
