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

        /// <summary>
        /// Gets the frame that associated to a given property.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        IFrameNamedFrame PropertyToFrame(string propertyName);

        /// <summary>
        /// Gets the frame associated to the comment.
        /// </summary>
        IFrameCommentFrame GetCommentFrame();
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    [ContentProperty("Root")]
    [DebuggerDisplay("Node Template {{{NodeType?.Name}}}")]
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

        /// <summary>
        /// Gets the frame that associated to a given property.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        public virtual IFrameNamedFrame PropertyToFrame(string propertyName)
        {
            // Call overloads of this method if they exist.
            ControllerTools.AssertNoOverride(this, typeof(FrameNodeTemplate));

            bool Found = GetFirstNamedFrame(Root, propertyName, out IFrameNamedFrame Result);
            Debug.Assert(Found);
            Debug.Assert(Result != null);

            return Result;
        }

        private protected virtual bool GetFirstNamedFrame(IFrameFrame root, string propertyName, out IFrameNamedFrame frame)
        {
            bool Found = false;
            frame = null;

            if (root is IFrameNamedFrame AsNamedFrame)
            {
                if (AsNamedFrame.PropertyName == propertyName)
                {
                    frame = AsNamedFrame;
                    Found = true;
                }
            }

            if (!Found && root is IFramePanelFrame AsPanelFrame)
            {
                foreach (IFrameFrame Item in AsPanelFrame.Items)
                    if (GetFirstNamedFrame(Item, propertyName, out frame))
                    {
                        Found = true;
                        break;
                    }
            }

            return Found;
        }

        /// <summary>
        /// Gets the frame associated to the comment.
        /// </summary>
        public virtual IFrameCommentFrame GetCommentFrame()
        {
            // Call overloads of this method if they exist.
            ControllerTools.AssertNoOverride(this, typeof(FrameNodeTemplate));

            bool Found = GetFirstCommentFrame(Root, out IFrameCommentFrame Result);
            Debug.Assert(Found);
            Debug.Assert(Result != null);

            return Result;
        }

        private protected virtual bool GetFirstCommentFrame(IFrameFrame root, out IFrameCommentFrame frame)
        {
            bool Found = false;
            frame = null;

            if (root is IFrameCommentFrame AsCommentFrame)
            {
                frame = AsCommentFrame;
                Found = true;
            }

            if (!Found && root is IFramePanelFrame AsPanelFrame)
            {
                foreach (IFrameFrame Item in AsPanelFrame.Items)
                    if (GetFirstCommentFrame(Item, out frame))
                    {
                        Found = true;
                        break;
                    }
            }

            return Found;
        }
        #endregion
    }
}
