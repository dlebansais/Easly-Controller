namespace EaslyController.Focus
{
    /// <summary>
    /// Semantic describing all specifics of a node that are not captured in its structure.
    /// </summary>
    public interface IFocusPropertySemantic
    {
        /// <summary>
        /// The property name.
        /// (Set in Xaml)
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// True if the associated collection is never empty.
        /// (Set in Xaml)
        /// </summary>
        bool IsNeverEmpty { get; set; }
    }

    /// <summary>
    /// Semantic describing all specifics of a node that are not captured in its structure.
    /// </summary>
    public class FocusPropertySemantic : IFocusPropertySemantic
    {
        #region Properties
        /// <summary>
        /// The property name.
        /// (Set in Xaml)
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// True if the associated collection is never empty.
        /// (Set in Xaml)
        /// </summary>
        public bool IsNeverEmpty { get; set; }
        #endregion
    }
}
