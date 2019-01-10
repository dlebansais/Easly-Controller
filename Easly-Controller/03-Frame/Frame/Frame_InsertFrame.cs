using BaseNode;
using BaseNodeHelper;
using System;
using System.Windows.Markup;

namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for bringing the focus to an insertion point.
    /// </summary>
    public interface IFrameInsertFrame : IFrameStaticFrame
    {
        /// <summary>
        /// The property name for the list or block list where new elements should be inserted.
        /// (Set in Xaml)
        /// </summary>
        string CollectionName { get; set; }
    }

    /// <summary>
    /// Frame for bringing the focus to an insertion point.
    /// </summary>
    [ContentProperty("CollectionName")]
    public class FrameInsertFrame : FrameStaticFrame, IFrameInsertFrame
    {
        #region Properties
        /// <summary>
        /// The property name for the list or block list where new elements should be inserted.
        /// (Set in Xaml)
        /// </summary>
        public string CollectionName { get; set; }
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

            if (string.IsNullOrEmpty(CollectionName))
                return false;

            string[] Split = CollectionName.Split('.');
            Type BaseType = nodeType;

            for (int i = 0; i < Split.Length; i++)
            {
                string PropertyName = Split[i];
                Type ChildNodeType;

                if (i + 1 < Split.Length)
                {
                    if (!NodeTreeHelperChild.IsChildNodeProperty(BaseType, PropertyName, out ChildNodeType) && !NodeTreeHelperOptional.IsOptionalChildNodeProperty(BaseType, PropertyName, out ChildNodeType))
                        return false;

                    BaseType = ChildNodeType;
                }
                else
                {
                    if (!NodeTreeHelperBlockList.IsBlockListProperty(BaseType, PropertyName, out Type ChildInterfaceType, out ChildNodeType) && !NodeTreeHelperList.IsNodeListProperty(BaseType, PropertyName, out ChildNodeType))
                        return false;
                }
            }

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFocusableCellView object.
        /// </summary>
        protected override IFrameVisibleCellView CreateFrameCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameInsertFrame));
            return new FrameFocusableCellView(stateView);
        }
        #endregion
    }
}
