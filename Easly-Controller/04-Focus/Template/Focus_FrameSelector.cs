namespace EaslyController.Focus
{
    using System;
    using System.Diagnostics;
    using BaseNodeHelper;

    /// <summary>
    /// Selects specific frames in the remaining of the cell view tree.
    /// </summary>
    public interface IFocusFrameSelector
    {
        /// <summary>
        /// Base type this selector can specify.
        /// (Set in Xaml)
        /// </summary>
        Type SelectorType { get; set; }

        /// <summary>
        /// Selector name.
        /// (Set in Xaml)
        /// </summary>
        string SelectorName { get; set; }

        /// <summary>
        /// Checks that a frame selector is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame selector can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        /// <param name="propertyName">The property for which frames can be selected.</param>
        bool IsValid(Type nodeType, IFocusTemplateReadOnlyDictionary nodeTemplateTable, string propertyName);
    }

    /// <summary>
    /// Selects specific frames in the remaining of the cell view tree.
    /// </summary>
    public class FocusFrameSelector : IFocusFrameSelector
    {
        #region Properties
        /// <summary>
        /// Base type this selector can specify.
        /// (Set in Xaml)
        /// </summary>
        public Type SelectorType { get; set; }

        /// <summary>
        /// Selector name.
        /// (Set in Xaml)
        /// </summary>
        public string SelectorName { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame selector is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame selector can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        /// <param name="propertyName">The property for which frames can be selected.</param>
        public virtual bool IsValid(Type nodeType, IFocusTemplateReadOnlyDictionary nodeTemplateTable, string propertyName)
        {
            bool IsValid = true;

            IsValid &= SelectorType != null;
            IsValid &= !string.IsNullOrEmpty(SelectorName);

            Type ChildInterfaceType, ChildNodeType;
            IsValid &= NodeTreeHelperChild.IsChildNodeProperty(nodeType, propertyName, out ChildInterfaceType) ||
                       NodeTreeHelperOptional.IsOptionalChildNodeProperty(nodeType, propertyName, out ChildInterfaceType) ||
                       NodeTreeHelperList.IsNodeListProperty(nodeType, propertyName, out ChildInterfaceType) ||
                       NodeTreeHelperBlockList.IsBlockListProperty(nodeType, propertyName, out ChildInterfaceType, out ChildNodeType);

            IsValid &= nodeTemplateTable.ContainsKey(SelectorType);

            IFocusNodeTemplate Template = nodeTemplateTable[SelectorType] as IFocusNodeTemplate;
            Debug.Assert(Template != null);

            IFocusSelectionFrame AsSelectionFrame = Template.Root as IFocusSelectionFrame;
            IsValid &= AsSelectionFrame != null;

            if (IsValid)
            {
                IFocusSelectableFrame SelectedItem = null;
                foreach (IFocusSelectableFrame Item in AsSelectionFrame.Items)
                    if (Item.Name == SelectorName)
                    {
                        SelectedItem = Item;
                        break;
                    }

                IsValid &= SelectedItem != null;
            }

            Debug.Assert(IsValid);
            return IsValid;
        }
        #endregion
    }
}
