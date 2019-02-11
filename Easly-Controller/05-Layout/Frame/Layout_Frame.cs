﻿namespace EaslyController.Layout
{
    using System;
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Base frame.
    /// </summary>
    public interface ILayoutFrame : IFocusFrame
    {
        /// <summary>
        /// Parent template.
        /// </summary>
        new ILayoutTemplate ParentTemplate { get; }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        new ILayoutFrame ParentFrame { get; }
    }

    /// <summary>
    /// Base frame.
    /// </summary>
    public abstract class LayoutFrame : FocusFrame, ILayoutFrame
    {
        #region Init
        private class LayoutRootFrame : ILayoutFrame
        {
            public LayoutRootFrame()
            {
                UpdateParent(null, null);
                Debug.Assert(ParentTemplate == null);
                Debug.Assert(ParentFrame == null);
                Debug.Assert(!IsValid(null, null));

                IFocusFrame AsFrame = this;
                Debug.Assert(AsFrame.ParentTemplate == null);
                Debug.Assert(AsFrame.ParentFrame == null);
            }

            public ILayoutTemplate ParentTemplate { get; }
            IFocusTemplate IFocusFrame.ParentTemplate { get { return ParentTemplate; } }
            IFrameTemplate IFrameFrame.ParentTemplate { get { return ParentTemplate; } }
            public ILayoutFrame ParentFrame { get; }
            IFocusFrame IFocusFrame.ParentFrame { get { return ParentFrame; } }
            IFrameFrame IFrameFrame.ParentFrame { get { return ParentFrame; } }
            public bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable) { return false; }
            public void UpdateParent(IFrameTemplate parentTemplate, IFrameFrame parentFrame) { }
        }

        /// <summary>
        /// Singleton object representing the root of a tree of frames.
        /// </summary>
        public static ILayoutFrame LayoutRoot { get; } = new LayoutRootFrame();
        #endregion

        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new ILayoutTemplate ParentTemplate { get { return (ILayoutTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new ILayoutFrame ParentFrame { get { return (ILayoutFrame)base.ParentFrame; } }
        #endregion
    }
}
