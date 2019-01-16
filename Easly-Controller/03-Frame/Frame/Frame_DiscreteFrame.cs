using BaseNodeHelper;
using System;
using System.Diagnostics;
using System.Windows.Markup;

namespace EaslyController.Frame
{
    /// <summary>
    /// Frame describing an enum value that can be displayed with different frames depending on its value.
    /// </summary>
    public interface IFrameDiscreteFrame : IFrameValueFrame
    {
        /// <summary>
        /// List of frames that can be displayed.
        /// (Set in Xaml)
        /// </summary>
        IFrameKeywordFrameList Items { get; }
    }

    /// <summary>
    /// Frame describing an enum value that can be displayed with different frames depending on its value.
    /// </summary>
    [ContentProperty("Items")]
    public class FrameDiscreteFrame : FrameValueFrame, IFrameDiscreteFrame
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameDiscreteFrame"/>.
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
        public IFrameKeywordFrameList Items { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        public override bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable)
        {
            if (!base.IsValid(nodeType, nodeTemplateTable))
                return false;

            if (Items == null || Items.Count == 0)
                return false;

            if (!NodeTreeHelper.IsEnumProperty(nodeType, PropertyName) && !NodeTreeHelper.IsBooleanProperty(nodeType, PropertyName))
                return false;

            NodeTreeHelper.GetEnumRange(nodeType, PropertyName, out int Min, out int Max);
            if (Min != 0)
                return false;

            if (Max + 1 != Items.Count)
                return false;

            foreach (IFrameKeywordFrame Item in Items)
                if (!Item.IsValid(nodeType, nodeTemplateTable))
                    return false;

            return true;
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
            IFrameCellView CellView = CreateDiscreteContentFocusableCellView(context.StateView, KeywordFrame);

            return CellView;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxKeywordFrameList object.
        /// </summary>
        protected virtual IFrameKeywordFrameList CreateKeywordFrameList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameDiscreteFrame));
            return new FrameKeywordFrameList();
        }

        /// <summary>
        /// Creates a IxxxDiscreteContentFocusableCellView object.
        /// </summary>
        protected virtual IFrameDiscreteContentFocusableCellView CreateDiscreteContentFocusableCellView(IFrameNodeStateView stateView, IFrameKeywordFrame keywordFrame)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameDiscreteFrame));
            return new FrameDiscreteContentFocusableCellView(stateView, this, PropertyName, keywordFrame);
        }
        #endregion
    }
}
