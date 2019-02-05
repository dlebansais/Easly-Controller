namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;
    using System.Windows.Markup;
    using BaseNodeHelper;

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
            bool IsValid = true;

            IsValid &= base.IsValid(nodeType, nodeTemplateTable);
            IsValid &= !string.IsNullOrEmpty(CollectionName);

            UpdateInterfaceType(nodeType);

            IsValid &= InterfaceType != null;

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary></summary>
        private protected virtual void UpdateInterfaceType(Type nodeType)
        {
            if (InterfaceType == null)
            {
                string[] Split = CollectionName.Split('.');
                Type BaseType = nodeType;

                for (int i = 0; i < Split.Length; i++)
                {
                    string PropertyName = Split[i];
                    Type ChildNodeType;

                    if (i + 1 < Split.Length)
                    {
                        bool IsValidProperty = NodeTreeHelperChild.IsChildNodeProperty(BaseType, PropertyName, out ChildNodeType) || NodeTreeHelperOptional.IsOptionalChildNodeProperty(BaseType, PropertyName, out ChildNodeType);
                        Debug.Assert(IsValidProperty);

                        BaseType = ChildNodeType;
                    }
                    else
                    {
                        bool IsValidProperty = NodeTreeHelperBlockList.IsBlockListProperty(BaseType, PropertyName, out Type ChildInterfaceType, out ChildNodeType) || NodeTreeHelperList.IsNodeListProperty(BaseType, PropertyName, out ChildInterfaceType);
                        Debug.Assert(IsValidProperty);

                        BaseType = ChildInterfaceType;
                    }
                }

                InterfaceType = BaseType;
            }
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFocusableCellView object.
        /// </summary>
        private protected override IFrameVisibleCellView CreateFrameCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameInsertFrame));
            return new FrameFocusableCellView(stateView, this);
        }
        #endregion
    }
}
