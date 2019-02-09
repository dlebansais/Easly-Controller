namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// Context used when building the cell view tree.
    /// </summary>
    public interface IFocusCellViewTreeContext : IFrameCellViewTreeContext
    {
        /// <summary>
        /// The view in which cells are created.
        /// </summary>
        new IFocusControllerView ControllerView { get; }

        /// <summary>
        /// The state view for which to create cells.
        /// </summary>
        new IFocusNodeStateView StateView { get; }

        /// <summary>
        /// The block state view for which to create cells. Can be null.
        /// </summary>
        new IFocusBlockStateView BlockStateView { get; }

        /// <summary>
        /// True if cells are shown ccording to the frame visibility.
        /// </summary>
        bool IsFrameVisible { get; }

        /// <summary>
        /// True if the user requested to see elements that are otherwise not shown.
        /// </summary>
        bool IsUserVisible { get; }

        /// <summary>
        /// True if visibility allow the cell tree to be visible.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Table of selectors for child frames.
        /// </summary>
        IDictionary<Type, string> SelectorTable { get; }

        /// <summary>
        /// Update the current visibility of frames.
        /// </summary>
        /// <param name="frame">The frame with the visibility to check.</param>
        /// <param name="oldFrameVisibility">The previous visibility upon return.</param>
        void UpdateNodeFrameVisibility(IFocusNodeFrameWithVisibility frame, out bool oldFrameVisibility);

        /// <summary>
        /// Update the current visibility of frames.
        /// </summary>
        /// <param name="frame">The frame with the visibility to check.</param>
        /// <param name="oldFrameVisibility">The previous visibility upon return.</param>
        void UpdateBlockFrameVisibility(IFocusBlockFrame frame, out bool oldFrameVisibility);

        /// <summary>
        /// Restores the frame visibility that was changed with <see cref="UpdateNodeFrameVisibility"/> or <see cref="UpdateBlockFrameVisibility"/>.
        /// </summary>
        /// <param name="oldFrameVisibility">The previous visibility</param>
        void RestoreFrameVisibility(bool oldFrameVisibility);

        /// <summary>
        /// Adds new selectors to the table.
        /// </summary>
        /// <param name="selectors">Selectors to add.</param>
        void AddSelectors(IFocusFrameSelectorList selectors);

        /// <summary>
        /// Removes selectors from the table.
        /// </summary>
        /// <param name="selectors">Selectors to add.</param>
        void RemoveSelectors(IFocusFrameSelectorList selectors);

        /// <summary>
        /// Adds new selectors to the table. This method is allowed to substitute one selector.
        /// </summary>
        /// <param name="selectors">Selectors to add.</param>
        /// <param name="oldSelectorType">Previous value for a substituted selector type upon return. Null if none.</param>
        /// <param name="oldSelectorName">Previous value for a substituted selector name upon return. Null if none.</param>
        void AddOrReplaceSelectors(IFocusFrameSelectorList selectors, out Type oldSelectorType, out string oldSelectorName);

        /// <summary>
        /// Removes selectors from the table.
        /// This method is allowed to substitute one selector substituted with <see cref="AddOrReplaceSelectors"/>.
        /// </summary>
        /// <param name="selectors">Selectors to add.</param>
        /// <param name="oldSelectorType">Previous value for a substituted selector type.</param>
        /// <param name="oldSelectorName">Previous value for a substituted selector name.</param>
        void RemoveOrRestoreSelectors(IFocusFrameSelectorList selectors, Type oldSelectorType, string oldSelectorName);

        /// <summary>
        /// Sets the <see cref="IsUserVisible"/> flag.
        /// </summary>
        /// <param name="isUserVisible">The new value.</param>
        /// <param name="oldValue">The previous value.</param>
        void ChangeIsUserVisible(bool isUserVisible, out bool oldValue);

        /// <summary>
        /// Restores the <see cref="IsUserVisible"/> flag previously set with <see cref="ChangeIsUserVisible"/>.
        /// </summary>
        /// <param name="isUserVisible">The new value.</param>
        void RestoreIsUserVisible(bool isUserVisible);
    }

    /// <summary>
    /// Context used when building the cell view tree.
    /// </summary>
    internal class FocusCellViewTreeContext : FrameCellViewTreeContext, IFocusCellViewTreeContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCellViewTreeContext"/> class.
        /// </summary>
        /// <param name="controllerView">The view in which cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        public FocusCellViewTreeContext(IFrameControllerView controllerView, IFrameNodeStateView stateView)
            : base(controllerView, stateView)
        {
            IsFrameVisible = true;
            IsUserVisible = false;
            SelectorTable = new Dictionary<Type, string>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// The view in which cells are created.
        /// </summary>
        public new IFocusControllerView ControllerView { get { return (IFocusControllerView)base.ControllerView; } }

        /// <summary>
        /// The state view for which to create cells.
        /// </summary>
        public new IFocusNodeStateView StateView { get { return (IFocusNodeStateView)base.StateView; } }

        /// <summary>
        /// The block state view for which to create cells. Can be null.
        /// </summary>
        public new IFocusBlockStateView BlockStateView { get { return (IFocusBlockStateView)base.BlockStateView; } }

        /// <summary>
        /// True if cells are shown ccording to the frame visibility.
        /// </summary>
        public bool IsFrameVisible { get; private set; }

        /// <summary>
        /// True if the user requested to see elements that are otherwise not shown.
        /// </summary>
        public bool IsUserVisible { get; private set; }

        /// <summary>
        /// True if visibility allow the cell tree to be visible.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                if (IsFrameVisible)
                    return true;

                if (IsUserVisible)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Table of selectors for child frames.
        /// </summary>
        public IDictionary<Type, string> SelectorTable { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the current visibility of frames.
        /// </summary>
        /// <param name="frame">The frame with the visibility to check.</param>
        /// <param name="oldFrameVisibility">The previous visibility upon return.</param>
        public virtual void UpdateNodeFrameVisibility(IFocusNodeFrameWithVisibility frame, out bool oldFrameVisibility)
        {
            oldFrameVisibility = IsFrameVisible;
            bool IsPreferred = frame.IsPreferred;

            IFocusNodeFrameVisibility Visibility = frame.Visibility;
            if (Visibility != null)
            {
                bool IsVolatile = Visibility.IsVolatile;

                bool IsVisible = Visibility.IsVisible(this, frame);
                IsFrameVisible &= IsVisible;
            }
        }

        /// <summary>
        /// Update the current visibility of frames.
        /// </summary>
        /// <param name="frame">The frame with the visibility to check.</param>
        /// <param name="oldFrameVisibility">The previous visibility upon return.</param>
        public virtual void UpdateBlockFrameVisibility(IFocusBlockFrame frame, out bool oldFrameVisibility)
        {
            oldFrameVisibility = IsFrameVisible;

            if (frame.BlockVisibility != null)
            {
                bool IsVisible = frame.BlockVisibility.IsBlockVisible(this, frame);
                IsFrameVisible &= IsVisible;
            }
        }

        /// <summary>
        /// Restores the frame visibility that was changed with <see cref="UpdateNodeFrameVisibility"/> or <see cref="UpdateBlockFrameVisibility"/>.
        /// </summary>
        /// <param name="oldFrameVisibility">The previous visibility</param>
        public virtual void RestoreFrameVisibility(bool oldFrameVisibility)
        {
            IsFrameVisible = oldFrameVisibility;
        }

        /// <summary>
        /// Adds new selectors to the table.
        /// </summary>
        /// <param name="selectors">Selectors to add.</param>
        public virtual void AddSelectors(IFocusFrameSelectorList selectors)
        {
            Debug.Assert(selectors != null);

            foreach (IFocusFrameSelector Item in selectors)
            {
                Debug.Assert(!SelectorTable.ContainsKey(Item.SelectorType));
                SelectorTable.Add(Item.SelectorType, Item.SelectorName);
            }
        }

        /// <summary>
        /// Removes selectors from the table.
        /// </summary>
        /// <param name="selectors">Selectors to add.</param>
        public virtual void RemoveSelectors(IFocusFrameSelectorList selectors)
        {
            Debug.Assert(selectors != null);

            foreach (IFocusFrameSelector Item in selectors)
            {
                Debug.Assert(SelectorTable.ContainsKey(Item.SelectorType));
                SelectorTable.Remove(Item.SelectorType);
            }
        }

        /// <summary>
        /// Adds new selectors to the table. This method is allowed to substitute one selector.
        /// </summary>
        /// <param name="selectors">Selectors to add.</param>
        /// <param name="oldSelectorType">Previous value for a substituted selector type upon return. Null if none.</param>
        /// <param name="oldSelectorName">Previous value for a substituted selector name upon return. Null if none.</param>
        public virtual void AddOrReplaceSelectors(IFocusFrameSelectorList selectors, out Type oldSelectorType, out string oldSelectorName)
        {
            Debug.Assert(selectors != null);

            oldSelectorType = null;
            oldSelectorName = null;

            foreach (IFocusFrameSelector Item in selectors)
                if (SelectorTable.ContainsKey(Item.SelectorType))
                {
                    Debug.Assert(oldSelectorType == null);
                    Debug.Assert(oldSelectorName == null);

                    oldSelectorType = Item.SelectorType;
                    oldSelectorName = SelectorTable[Item.SelectorType];

                    SelectorTable[Item.SelectorType] = Item.SelectorName;
                }
                else
                    SelectorTable.Add(Item.SelectorType, Item.SelectorName);
        }

        /// <summary>
        /// Removes selectors from the table.
        /// This method is allowed to substitute one selector substituted with <see cref="AddOrReplaceSelectors"/>.
        /// </summary>
        /// <param name="selectors">Selectors to add.</param>
        /// <param name="oldSelectorType">Previous value for a substituted selector type.</param>
        /// <param name="oldSelectorName">Previous value for a substituted selector name.</param>
        public virtual void RemoveOrRestoreSelectors(IFocusFrameSelectorList selectors, Type oldSelectorType, string oldSelectorName)
        {
            Debug.Assert(selectors != null);

            foreach (IFocusFrameSelector Item in selectors)
            {
                Debug.Assert(SelectorTable.ContainsKey(Item.SelectorType));

                if (Item.SelectorType == oldSelectorType)
                    SelectorTable[oldSelectorType] = oldSelectorName;
                else
                    SelectorTable.Remove(Item.SelectorType);
            }
        }

        /// <summary>
        /// Sets the <see cref="IsUserVisible"/> flag.
        /// </summary>
        /// <param name="isUserVisible">The new value.</param>
        /// <param name="oldValue">The previous value.</param>
        public virtual void ChangeIsUserVisible(bool isUserVisible, out bool oldValue)
        {
            oldValue = IsUserVisible;
            IsUserVisible = isUserVisible;
        }

        /// <summary>
        /// Restores the <see cref="IsUserVisible"/> flag previously set with <see cref="ChangeIsUserVisible"/>.
        /// </summary>
        /// <param name="isUserVisible">The new value.</param>
        public virtual void RestoreIsUserVisible(bool isUserVisible)
        {
            IsUserVisible = isUserVisible;
        }
        #endregion
    }
}
