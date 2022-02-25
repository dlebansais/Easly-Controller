namespace EaslyController.Frame
{
    using System.Diagnostics;
    using BaseNodeHelper;
    using NotNullReflection;

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
        /// <param name="commentFrameCount">Number of comment frames found so far.</param>
        public override bool IsValid(Type nodeType, FrameTemplateReadOnlyDictionary nodeTemplateTable, ref int commentFrameCount)
        {
            bool IsValid = true;

            IsValid &= base.IsValid(nodeType, nodeTemplateTable, ref commentFrameCount);
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
            IFrameVisibleCellView EmbeddingCellView = CreateVisibleCellView(context.StateView, parentCellView);
            ValidateVisibleCellView(context, EmbeddingCellView);

            return EmbeddingCellView;
        }
        #endregion

        #region Implementation
        private protected virtual void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(cellView.StateView == context.StateView);
            Debug.Assert(cellView.Frame == this);
            IFrameCellViewCollection ParentCellView = cellView.ParentCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxTextFocusableCellView object.
        /// </summary>
        private protected virtual IFrameVisibleCellView CreateVisibleCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTextValueFrame>());
            return new FrameStringContentFocusableCellView(stateView, parentCellView, this, PropertyName);
        }
        #endregion
    }
}
