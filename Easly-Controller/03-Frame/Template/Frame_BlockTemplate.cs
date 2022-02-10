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

        /// <summary>
        /// Gets the collection placeholder frame that creates cells for nodes using this template.
        /// </summary>
        IFrameCollectionPlaceholderFrame GetPlaceholderFrame();
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    [ContentProperty("Root")]
    [DebuggerDisplay("Node Template {{{NodeType?.Name}}}")]
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

        /// <summary>
        /// Gets the collection placeholder frame that creates cells for nodes using this template.
        /// </summary>
        public virtual IFrameCollectionPlaceholderFrame GetPlaceholderFrame()
        {
            bool Found = GetFirstCollectionPlaceholderFrame(Root, out IFrameCollectionPlaceholderFrame Result);
            Debug.Assert(Found);
            Debug.Assert(Result != null);

            return Result;
        }

        private protected bool GetFirstCollectionPlaceholderFrame(IFrameFrame root, out IFrameCollectionPlaceholderFrame frame)
        {
            if (root is IFrameCollectionPlaceholderFrame AsCollectionPlaceholderFrame)
            {
                frame = AsCollectionPlaceholderFrame;
                return true;
            }

            if (root is IFramePanelFrame AsPanelFrame)
            {
                foreach (IFrameFrame Item in AsPanelFrame.Items)
                    if (GetFirstCollectionPlaceholderFrame(Item, out frame))
                        return true;
            }

            frame = null;
            return false;
        }
        #endregion
    }
}
