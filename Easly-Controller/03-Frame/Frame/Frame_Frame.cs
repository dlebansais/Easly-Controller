namespace EaslyController.Frame
{
    using System.Diagnostics;
    using Contracts;
    using NotNullReflection;

    /// <summary>
    /// Base frame.
    /// </summary>
    public interface IFrameFrame
    {
        /// <summary>
        /// Parent template.
        /// </summary>
        IFrameTemplate ParentTemplate { get; }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        IFrameFrame ParentFrame { get; }

        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        /// <param name="commentFrameCount">Number of comment frames found so far.</param>
        bool IsValid(Type nodeType, FrameTemplateReadOnlyDictionary nodeTemplateTable, ref int commentFrameCount);

        /// <summary>
        /// Update the reference to the parent frame.
        /// </summary>
        /// <param name="parentTemplate">The parent template.</param>
        /// <param name="parentFrame">The parent frame.</param>
        void UpdateParent(IFrameTemplate parentTemplate, IFrameFrame parentFrame);
    }

    /// <summary>
    /// Base frame.
    /// </summary>
    public abstract class FrameFrame : IFrameFrame
    {
        #region Init
        private class FrameRootFrame : IFrameFrame
        {
            public FrameRootFrame()
            {
                UpdateParent(null, null);
                int CommentFrameCount = 0;
                Debug.Assert(ParentTemplate == null);
                Debug.Assert(ParentFrame == null);
                Debug.Assert(!IsValid(null, null, ref CommentFrameCount));
            }

            public IFrameTemplate ParentTemplate { get; }
            public IFrameFrame ParentFrame { get; }
            public bool IsValid(Type nodeType, FrameTemplateReadOnlyDictionary nodeTemplateTable, ref int commentFrameCount) { return false; }
            public void UpdateParent(IFrameTemplate parentTemplate, IFrameFrame parentFrame) { }
        }

        /// <summary>
        /// Singleton object representing the root of a tree of frames.
        /// </summary>
        public static IFrameFrame FrameRoot { get; } = new FrameRootFrame();
        #endregion

        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public IFrameTemplate ParentTemplate { get; private set; }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public IFrameFrame ParentFrame { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        /// <param name="commentFrameCount">Number of comment frames found so far.</param>
        public virtual bool IsValid(Type nodeType, FrameTemplateReadOnlyDictionary nodeTemplateTable, ref int commentFrameCount)
        {
            return true;
        }

        /// <summary>
        /// Update the reference to the parent frame.
        /// </summary>
        /// <param name="parentTemplate">The parent template.</param>
        /// <param name="parentFrame">The parent frame.</param>
        public virtual void UpdateParent(IFrameTemplate parentTemplate, IFrameFrame parentFrame)
        {
            Contract.RequireNotNull(parentTemplate, out IFrameTemplate ParentTemplate);
            Contract.RequireNotNull(parentFrame, out IFrameFrame ParentFrame);

            Debug.Assert(this.ParentTemplate == null);
            this.ParentTemplate = ParentTemplate;

            Debug.Assert(this.ParentFrame == null);
            this.ParentFrame = ParentFrame;
        }
        #endregion
    }
}
