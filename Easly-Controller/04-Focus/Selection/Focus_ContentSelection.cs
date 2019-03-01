namespace EaslyController.Focus
{
    /// <summary>
    /// A selection of a property of a node, or a partial selection for text.
    /// </summary>
    public interface IFocusContentSelection : IFocusSelection
    {
        /// <summary>
        /// The property name.
        /// </summary>
        string PropertyName { get; }
    }
}
