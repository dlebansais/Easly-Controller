﻿using EaslyController.Frame;
using System.Windows.Markup;

namespace EaslyController.Focus
{
    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFocusBlockTemplate : IFrameBlockTemplate, IFocusTemplate
    {
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    [ContentProperty("Root")]
    public class FocusBlockTemplate : FrameBlockTemplate, IFocusBlockTemplate
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

        protected override bool IsRootValid { get { return (Root.ParentFrame == FocusFrame.FocusRoot); } }
        #endregion
    }
}
