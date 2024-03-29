﻿namespace EaslyController.Frame
{
    using System.Diagnostics;
    using System.Windows.Markup;
    using BaseNodeHelper;
    using NotNullReflection;

    /// <summary>
    /// Frame describing an enum value that can be displayed with different frames depending on its value.
    /// </summary>
    public interface IFrameDiscreteFrame : IFrameValueFrame
    {
        /// <summary>
        /// List of frames that can be displayed.
        /// (Set in Xaml)
        /// </summary>
        FrameKeywordFrameList Items { get; }
    }

    /// <summary>
    /// Frame describing an enum value that can be displayed with different frames depending on its value.
    /// </summary>
    [ContentProperty("Items")]
    public class FrameDiscreteFrame : FrameValueFrame, IFrameDiscreteFrame
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameDiscreteFrame"/> class.
        /// </summary>
        public FrameDiscreteFrame()
        {
            Items = CreateKeywordFrameList();
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of frames that can be displayed.
        /// (Set in Xaml)
        /// </summary>
        public FrameKeywordFrameList Items { get; }
        #endregion

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
            IsValid &= Items != null && Items.Count > 0;
            IsValid &= NodeTreeHelper.IsEnumProperty(nodeType, PropertyName) || NodeTreeHelper.IsBooleanProperty(nodeType, PropertyName);

            NodeTreeHelper.GetEnumRange(nodeType, PropertyName, out int Min, out int Max);
            IsValid &= Min == 0;
            IsValid &= Max + 1 == Items.Count;

            foreach (IFrameKeywordFrame Item in Items)
                IsValid &= Item.IsValid(nodeType, nodeTemplateTable, ref commentFrameCount);

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary>
        /// Update the reference to the parent frame.
        /// </summary>
        /// <param name="parentTemplate">The parent template.</param>
        /// <param name="parentFrame">The parent frame.</param>
        public override void UpdateParent(IFrameTemplate parentTemplate, IFrameFrame parentFrame)
        {
            base.UpdateParent(parentTemplate, parentFrame);

            foreach (IFrameKeywordFrame Item in Items)
                Item.UpdateParent(parentTemplate, this);
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public override IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            IFrameNodeState State = context.StateView.State;
            int Value = NodeTreeHelper.GetEnumValue(State.Node, PropertyName);

            Debug.Assert(Value >= 0 && Value < Items.Count);

            IFrameKeywordFrame KeywordFrame = Items[Value];
            IFrameDiscreteContentFocusableCellView CellView = CreateDiscreteContentFocusableCellView(context.StateView, parentCellView, KeywordFrame);
            ValidateDiscreteContentFocusableCellView(context, KeywordFrame, CellView);

            return CellView;
        }

        private protected virtual void ValidateDiscreteContentFocusableCellView(IFrameCellViewTreeContext context, IFrameKeywordFrame keywordFrame, IFrameDiscreteContentFocusableCellView cellView)
        {
            Debug.Assert(cellView.StateView == context.StateView);
            Debug.Assert(cellView.Frame == this);
            Debug.Assert(cellView.KeywordFrame == keywordFrame);
            IFrameCellViewCollection ParentCellView = cellView.ParentCellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxKeywordFrameList object.
        /// </summary>
        private protected virtual FrameKeywordFrameList CreateKeywordFrameList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameDiscreteFrame>());
            return new FrameKeywordFrameList();
        }

        /// <summary>
        /// Creates a IxxxDiscreteContentFocusableCellView object.
        /// </summary>
        private protected virtual IFrameDiscreteContentFocusableCellView CreateDiscreteContentFocusableCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameKeywordFrame keywordFrame)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameDiscreteFrame>());
            return new FrameDiscreteContentFocusableCellView(stateView, parentCellView, this, PropertyName, keywordFrame);
        }
        #endregion
    }
}
