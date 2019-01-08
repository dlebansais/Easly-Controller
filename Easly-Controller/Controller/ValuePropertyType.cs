namespace EaslyController
{
    /// <summary>
    /// Types that a value in a node can be.
    /// </summary>
    public enum ValuePropertyType
    {
        /// <summary>
        /// A boolean.
        /// </summary>
        Boolean,

        /// <summary>
        /// A enumeration of discrete values.
        /// </summary>
        Enum,

        /// <summary>
        /// A string.
        /// </summary>
        String,

        /// <summary>
        /// A <see cref="System.Guid"/>.
        /// </summary>
        Guid,
    }
}
