using System;

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
        /// <param name="propertyName">The property for which frames can be selected.</param>
        bool IsValid(Type nodeType, string propertyName);
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
        /// <param name="propertyName">The property for which frames can be selected.</param>
        public virtual bool IsValid(Type nodeType, string propertyName)
        {
            if (SelectorType == null)
                return false;

            if (string.IsNullOrEmpty(SelectorName))
                return false;

            return true;
        }
        #endregion
    }
}
