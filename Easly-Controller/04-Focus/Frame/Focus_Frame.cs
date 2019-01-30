namespace EaslyController.Focus
{
    using System;
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
            public IFocusTemplate ParentTemplate { get { throw new InvalidOperationException(); } }
            IFrameTemplate IFrameFrame.ParentTemplate { get { throw new InvalidOperationException(); } }
            public IFocusFrame ParentFrame { get { throw new InvalidOperationException(); } }
            IFrameFrame IFrameFrame.ParentFrame { get { throw new InvalidOperationException(); } }
            public bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable) { return false; }
            public void UpdateParent(IFrameTemplate parentTemplate, IFrameFrame parentFrame) { throw new InvalidOperationException(); }
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
