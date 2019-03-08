namespace EaslyController.Constants
{
    /// <summary>
    /// Selection styles supported for drawing the selection.
    /// </summary>
    public enum SelectionStyles
    {
        /// <summary>
        /// Selection around some text.
        /// </summary>
        Text,

        /// <summary>
        /// Selection around a single node.
        /// </summary>
        Node,

        /// <summary>
        /// Selection around a list of nodes.
        /// </summary>
        NodeList,

        /// <summary>
        /// Selection around a list of blocks.
        /// </summary>
        BlockList,
    }
}
