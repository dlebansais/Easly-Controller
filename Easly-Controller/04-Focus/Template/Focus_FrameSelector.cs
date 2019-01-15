using BaseNodeHelper;
using System;
using System.Diagnostics;

namespace EaslyController.Focus
{
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

        #region Properties
        /// <summary>
        /// Checks that a frame selector is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame selector can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        /// <param name="propertyName">The property for which frames can be selected.</param>
        public virtual bool IsValid(Type nodeType, IFocusTemplateReadOnlyDictionary nodeTemplateTable, string propertyName)
        {
            if (SelectorType == null)
                return false;

            if (string.IsNullOrEmpty(SelectorName))
                return false;

            Type ChildInterfaceType, ChildNodeType;
            if (!NodeTreeHelperChild.IsChildNodeProperty(nodeType, propertyName, out ChildInterfaceType) &&
                !NodeTreeHelperOptional.IsOptionalChildNodeProperty(nodeType, propertyName, out ChildInterfaceType) &&
                !NodeTreeHelperList.IsNodeListProperty(nodeType, propertyName, out ChildInterfaceType) &&
                !NodeTreeHelperBlockList.IsBlockListProperty(nodeType, propertyName, out ChildInterfaceType, out ChildNodeType))
                return false;

            if (!ChildInterfaceType.IsAssignableFrom(SelectorType))
                return false;

            if (!nodeTemplateTable.ContainsKey(SelectorType))
                return false;

            IFocusNodeTemplate Template = nodeTemplateTable[SelectorType] as IFocusNodeTemplate;
            Debug.Assert(Template != null);

            if (!(Template.Root is IFocusSelectionFrame AsSelectionFrame))
                return false;

            IFocusSelectableFrame SelectedItem = null;
            foreach (IFocusSelectableFrame Item in AsSelectionFrame.Items)
                if (Item.Name == SelectorName)
                {
                    SelectedItem = Item;
                    break;
                }

            if (SelectedItem == null)
                return false;

            return true;
        }
        #endregion
    }
}
