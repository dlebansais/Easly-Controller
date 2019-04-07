namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// Base frame.
    /// </summary>
    public interface IFocusFrame : IFrameFrame
    {
        /// <summary>
        /// Parent template.
        /// </summary>
        new IFocusTemplate ParentTemplate { get; }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        new IFocusFrame ParentFrame { get; }
    }

    /// <summary>
    /// Base frame.
    /// </summary>
    public abstract class FocusFrame : FrameFrame, IFocusFrame
    {
        #region Init
        private class FocusRootFrame : IFocusFrame
        {
            public FocusRootFrame()
            {
                UpdateParent(null, null);
                int CommentFrameCount = 0;
                Debug.Assert(ParentTemplate == null);
                Debug.Assert(ParentFrame == null);
                Debug.Assert(!IsValid(null, null, ref CommentFrameCount));

                IFrameFrame AsFrameFrame = this;
                Debug.Assert(AsFrameFrame.ParentTemplate == null);
                Debug.Assert(AsFrameFrame.ParentFrame == null);
            }

            public IFocusTemplate ParentTemplate { get; }
            IFrameTemplate IFrameFrame.ParentTemplate { get { return ParentTemplate; } }
            public IFocusFrame ParentFrame { get; }
            IFrameFrame IFrameFrame.ParentFrame { get { return ParentFrame; } }
            public bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable, ref int commentFrameCount) { return false; }
            public void UpdateParent(IFrameTemplate parentTemplate, IFrameFrame parentFrame) { }
        }

        /// <summary>
        /// Singleton object representing the root of a tree of frames.
        /// </summary>
        public static IFocusFrame FocusRoot { get; } = new FocusRootFrame();
        #endregion

        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new IFocusTemplate ParentTemplate { get { return (IFocusTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new IFocusFrame ParentFrame { get { return (IFocusFrame)base.ParentFrame; } }
        #endregion
    }
}
