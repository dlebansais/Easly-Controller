﻿namespace EaslyController.Frame
{
    using System.Diagnostics;
    using System.Windows.Markup;
    using BaseNodeHelper;
    using NotNullReflection;

    /// <summary>
    /// Frame for bringing the focus to an insertion point.
    /// </summary>
    public interface IFrameInsertFrame : IFrameStaticFrame
    {
        /// <summary>
        /// The property name for the list or block list where new elements should be inserted.
        /// (Set in Xaml)
        /// </summary>
        string CollectionName { get; }

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
        public Type InterfaceType { get; private set; } = Type.Missing;

        private protected override bool IsFrameFocusable { get { return true; } }
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
            IsValid &= !string.IsNullOrEmpty(CollectionName);
            IsValid &= InterfaceType != Type.Missing;

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

            UpdateInterfaceType(parentTemplate.NodeType);
        }

        private protected virtual void UpdateInterfaceType(Type nodeType)
        {
            if (InterfaceType == Type.Missing)
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
                        Type ChildInterfaceType;
                        bool IsValidProperty = NodeTreeHelperBlockList.IsBlockListProperty(BaseType, PropertyName, /*out ChildInterfaceType,*/ out /*ChildNodeType*/ChildInterfaceType) || NodeTreeHelperList.IsNodeListProperty(BaseType, PropertyName, out ChildInterfaceType);
                        Debug.Assert(IsValidProperty);

                        BaseType = ChildInterfaceType;
                    }
                }

                InterfaceType = BaseType;
            }
        }
        #endregion
    }
}
