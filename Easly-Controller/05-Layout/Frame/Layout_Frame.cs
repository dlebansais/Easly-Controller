namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using NotNullReflection;

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
        private class LayoutRootFrame : ILayoutFrame, ILayoutMeasurableFrame, ILayoutDrawableFrame
        {
            public LayoutRootFrame()
            {
                UpdateParent(null, null);
                int CommentFrameCount = 0;
                Debug.Assert(ParentTemplate == null);
                Debug.Assert(ParentFrame == null);
                Debug.Assert(!IsValid(null, null, ref CommentFrameCount));

                IFrameFrame AsFrameFrame = this;
                Debug.Assert(AsFrameFrame.ParentTemplate == null);
                Debug.Assert(AsFrameFrame.ParentFrame == null);

                IFocusFrame AsFocusFrame = this;
                Debug.Assert(AsFocusFrame.ParentTemplate == null);
                Debug.Assert(AsFocusFrame.ParentFrame == null);
            }

            public ILayoutTemplate ParentTemplate { get; }
            IFocusTemplate IFocusFrame.ParentTemplate { get { return ParentTemplate; } }
            IFrameTemplate IFrameFrame.ParentTemplate { get { return ParentTemplate; } }
            public ILayoutFrame ParentFrame { get; }
            IFocusFrame IFocusFrame.ParentFrame { get { return ParentFrame; } }
            IFrameFrame IFrameFrame.ParentFrame { get { return ParentFrame; } }
            public bool IsValid(Type nodeType, FrameTemplateReadOnlyDictionary nodeTemplateTable, ref int commentFrameCount) { return false; }
            public void UpdateParent(IFrameTemplate parentTemplate, IFrameFrame parentFrame) { }
            public void Measure(ILayoutMeasureContext measureContext, ILayoutCellView cellView, ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, Measure separatorLength, out Size size, out Padding padding) { size = new(); padding = new(); }
            public void Draw(ILayoutDrawContext drawContext, ILayoutCellView cellView, Point origin, Size size, Padding padding) { }
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
