using BaseNodeHelper;
using System;
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
            if (Max - Min + 1 != Items.Count)
                return false;

            return true;
        }
        #endregion
    }
}
