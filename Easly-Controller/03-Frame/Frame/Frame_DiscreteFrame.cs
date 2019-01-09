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
        #region Properties
        /// <summary>
        /// List of frames that can be displayed.
        /// (Set in Xaml)
        /// </summary>
        public IFrameKeywordFrameList Items { get; } = new FrameKeywordFrameList();
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
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

            return true;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public override IFrameCellView BuildNodeCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            IFrameNodeState State = stateView.State;
            int Value = NodeTreeHelper.GetEnumValue(State.Node, PropertyName);

            Debug.Assert(Value >= 0 && Value < Items.Count);

            IFrameKeywordFrame KeywordFrame = Items[Value];
            return KeywordFrame.BuildNodeCells(controllerView, stateView, parentCellView);
        }
        #endregion
    }
}
