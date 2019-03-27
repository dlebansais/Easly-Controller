namespace EaslyController.Constants
{
    /// <summary>
    /// Modes for automatic formatting of identifiers and names.
    /// </summary>
    public enum AutoFormatModes
    {
        /// <summary>
        /// No change performed.
        /// </summary>
        None,

        /// <summary>
        /// The first character of each word is uppercase and others lowercase.
        /// </summary>
        FirstOnly,

        /// <summary>
        /// First characters of each word are uppercase and only followed by lowercase characters until the next word.
        /// </summary>
        FirstOrAll,

        /// <summary>
        /// All characters are lower case.
        /// </summary>
        AllLowercase,
    }
}
