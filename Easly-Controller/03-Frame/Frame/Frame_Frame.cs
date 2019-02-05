namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;

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
        bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable);

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
                Debug.Assert(ParentTemplate == null);
                Debug.Assert(ParentFrame == null);
                Debug.Assert(!IsValid(null, null));
            }

            public IFrameTemplate ParentTemplate { get; }
            public IFrameFrame ParentFrame { get; }
            public bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable) { return false; }
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
        public virtual bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable)
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
            Debug.Assert(parentTemplate != null);
            Debug.Assert(parentFrame != null);

            Debug.Assert(ParentTemplate == null);
            ParentTemplate = parentTemplate;

            Debug.Assert(ParentFrame == null);
            ParentFrame = parentFrame;
        }
        #endregion
    }
}
