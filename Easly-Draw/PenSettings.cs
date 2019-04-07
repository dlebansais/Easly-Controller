namespace EaslyDraw
{
    /// <summary>
    /// Choice of pen when drawing.
    /// </summary>
    public enum PenSettings
    {
        /// <summary>
        /// The default pen.
        /// </summary>
        Default,

        /// <summary>
        /// The pen for drawing around a comment box.
        /// </summary>
        Comment,

        /// <summary>
        /// The pen for drawing the insertion caret.
        /// </summary>
        CaretInsertion,

        /// <summary>
        /// The pen for drawing the override caret.
        /// </summary>
        CaretOverride,

        /// <summary>
        /// The pen for drawing around a text selection.
        /// </summary>
        SelectionText,

        /// <summary>
        /// The pen for drawing around a node selection.
        /// </summary>
        SelectionNode,

        /// <summary>
        /// The pen for drawing around a node list selection.
        /// </summary>
        SelectionNodeList,

        /// <summary>
        /// The pen for drawing around a block list selection.
        /// </summary>
        SelectionBlockList,

        /// <summary>
        /// The pen for drawing a block geometry.
        /// </summary>
        BlockGeometry,

        /// <summary>
        /// The pen for drawing a vertical separator.
        /// </summary>
        VerticalSeparator,
    }
}
