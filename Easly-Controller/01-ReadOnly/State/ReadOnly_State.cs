namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Base interface for a state.
    /// </summary>
    public interface IReadOnlyState
    {
        /// <summary>
        /// Gets the inner corresponding to a property.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        IReadOnlyInner<IReadOnlyBrowsingChildIndex> PropertyToInner(string propertyName);
    }
}
