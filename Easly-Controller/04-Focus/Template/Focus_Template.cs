﻿namespace EaslyController.Focus
{
    using EaslyController.Frame;

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFocusTemplate : IFrameTemplate
    {
        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        new IFocusFrame Root { get; set; }

        /// <summary>
        /// True if the parent template rather than this template should be fully displayed when visibility is enforced.
        /// (Set in Xaml)
        /// </summary>
        bool IsSimple { get; set; }
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public abstract class FocusTemplate : FrameTemplate, IFocusTemplate
    {
        #region Properties
        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        public new IFocusFrame Root { get { return (IFocusFrame)base.Root; } set { base.Root = value; } }

        /// <summary>
        /// True if the parent template rather than this template should be fully displayed when visibility is enforced.
        /// (Set in Xaml)
        /// </summary>
        public bool IsSimple { get; set; }

        /// <summary></summary>
        private protected override bool IsRootValid { get { return Root.ParentFrame == FocusFrame.FocusRoot; } }
        #endregion
    }
}
