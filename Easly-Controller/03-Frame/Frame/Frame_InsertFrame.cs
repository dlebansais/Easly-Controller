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

        /// <summary>
        /// Interface type of items in the collection associated to this frame.
        /// </summary>
        Type InterfaceType { get; }
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

        /// <summary>
        /// Interface type of items in the collection associated to this frame.
        /// </summary>
        public Type InterfaceType { get; private set; }
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

            if (string.IsNullOrEmpty(CollectionName))
                return false;

            UpdateInterfaceType(nodeType);
            if (InterfaceType == null)
                return false;

            return true;
        }

        protected virtual void UpdateInterfaceType(Type nodeType)
        {
            if (InterfaceType != null)
                return;

            string[] Split = CollectionName.Split('.');
            Type BaseType = nodeType;

            for (int i = 0; i < Split.Length; i++)
            {
                string PropertyName = Split[i];
                Type ChildNodeType;

                if (i + 1 < Split.Length)
                {
                    if (!NodeTreeHelperChild.IsChildNodeProperty(BaseType, PropertyName, out ChildNodeType) && !NodeTreeHelperOptional.IsOptionalChildNodeProperty(BaseType, PropertyName, out ChildNodeType))
                        return;

                    BaseType = ChildNodeType;
                }
                else
                {
                    if (!NodeTreeHelperBlockList.IsBlockListProperty(BaseType, PropertyName, out Type ChildInterfaceType, out ChildNodeType) && !NodeTreeHelperList.IsNodeListProperty(BaseType, PropertyName, out ChildInterfaceType))
                        return;

                    BaseType = ChildInterfaceType;
                }
            }

            InterfaceType = BaseType;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFocusableCellView object.
        /// </summary>
        protected override IFrameVisibleCellView CreateFrameCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameInsertFrame));
            return new FrameFocusableCellView(stateView, this);
        }
        #endregion
    }
}
