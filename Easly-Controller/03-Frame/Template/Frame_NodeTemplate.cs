namespace EaslyController.Frame
{
    using System.Diagnostics;
    using System.Windows.Markup;

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFrameNodeTemplate : IFrameTemplate
    {
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context);
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    [ContentProperty("Root")]
    public class FrameNodeTemplate : FrameTemplate, IFrameNodeTemplate
    {
        #region Client Interface
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public virtual IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context)
        {
            IFrameNodeFrame NodeFrame = Root as IFrameNodeFrame;
            Debug.Assert(NodeFrame != null);

            return NodeFrame.BuildNodeCells(context, null);
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
