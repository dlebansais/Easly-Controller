using System.Diagnostics;
using System.Windows.Markup;

namespace EaslyController.Frame
{
    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFrameBlockTemplate : IFrameTemplate
    {
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context);
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
        public virtual IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context)
        {
            IFrameBlockFrame BlockFrame = Root as IFrameBlockFrame;
            Debug.Assert(BlockFrame != null);

            return BlockFrame.BuildBlockCells(context);
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
