namespace EaslyController.Focus
{
    /// <summary>
    /// Internal interface for a view of a IxxxController.
    /// </summary>
    internal interface IFocusInternalControllerView
    {
        /// <summary>
        /// Checks if the template associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state is complex.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        bool IsTemplateComplex(IFocusNodeStateView stateView, string propertyName);

        /// <summary>
        /// Checks if the collection associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state has more than <paramref name="count"/> item.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the collection to check.</param>
        /// <param name="count">The number of items.</param>
        bool CollectionHasItems(IFocusNodeStateView stateView, string propertyName, int count);

        /// <summary>
        /// Checks if the optional node associated to <paramref name="propertyName"/> is assigned.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the node to check.</param>
        bool IsOptionalNodeAssigned(IFocusNodeStateView stateView, string propertyName);

        /// <summary>
        /// Checks if the enum or boolean associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state has value <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        /// <param name="defaultValue">Expected default value.</param>
        bool DiscreteHasDefaultValue(IFocusNodeStateView stateView, string propertyName, int defaultValue);

        /// <summary>
        /// Checks if the <paramref name="stateView"/> state is the first in a collection in the parent.
        /// </summary>
        /// <param name="stateView">The state view.</param>
        bool IsFirstItem(IFocusNodeStateView stateView);

        /// <summary>
        /// Checks if the <paramref name="blockStateView"/> block state belongs to a replicated block.
        /// </summary>
        /// <param name="blockStateView">The block state view.</param>
        bool IsInReplicatedBlock(IFocusBlockStateView blockStateView);

        /// <summary>
        /// Checks if the string associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state matches the pattern in <paramref name="textPattern"/>.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        /// <param name="textPattern">Expected text.</param>
        bool StringMatchTextPattern(IFocusNodeStateView stateView, string propertyName, string textPattern);

        /// <summary>
        /// Replaces a node with the content of the clipboard.
        /// </summary>
        /// <param name="state">The node state.</param>
        void ReplaceWithClipboardContent(IFocusNodeState state);
    }
}
