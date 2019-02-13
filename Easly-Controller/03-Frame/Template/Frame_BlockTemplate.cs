namespace EaslyController.Frame
{
    using System.Diagnostics;
    using System.Windows.Markup;

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFrameBlockTemplate : IFrameTemplate
    {
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView);
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    [ContentProperty("Root")]
    public class FrameBlockTemplate : FrameTemplate, IFrameBlockTemplate
    {
        #region Client Interface
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        public virtual IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            IFrameBlockFrame BlockFrame = Root as IFrameBlockFrame;
            Debug.Assert(BlockFrame != null);

            return BlockFrame.BuildBlockCells(context, parentCellView);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return "Node Template {" + NodeType?.Name + "}";
        }
        #endregion
    }
}
