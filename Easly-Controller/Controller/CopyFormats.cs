namespace EaslyController.Constants
{
    /// <summary>
    /// Modes affecting how text is copied to the clipboard.
    /// </summary>
    public enum CopyFormats
    {
        /// <summary>
        /// Use the RTF format. This is the default.
        /// </summary>
        Rtf,

        /// <summary>
        /// Use the HTML format.
        /// </summary>
        Html,

        /// <summary>
        /// Use the raw HTML format.
        /// </summary>
        RawHtml,
    }
}
