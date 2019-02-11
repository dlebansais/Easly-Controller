namespace EaslyController.Focus
{
    using System.Windows.Markup;
    using EaslyController.Frame;

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFocusNodeTemplate : IFrameNodeTemplate, IFocusTemplate
    {
        /// <summary>
        /// True if the associated expression should be surrounded with parenthesis.
        /// (Set in Xaml)
        /// </summary>
        bool IsComplex { get; set; }

        /// <summary>
        /// True if the parent template rather than this template should be fully displayed when visibility is enforced.
        /// (Set in Xaml)
        /// </summary>
        bool IsSimple { get; set; }

        /// <summary>
        /// Returns the frame associated to a property if can have selectors.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for.</param>
        /// <param name="frame">Frame found upon return. Null if not matching <paramref name="propertyName"/>.</param>
        bool FrameSelectorForProperty(string propertyName, out IFocusNodeFrameWithSelector frame);

        /// <summary>
        /// Gets preferred frames to receive the focus when the source code is changed.
        /// </summary>
        /// <param name="firstPreferredFrame">The first preferred frame found.</param>
        /// <param name="lastPreferredFrame">The last preferred frame found.</param>
        void GetPreferredFrame(out IFocusNodeFrame firstPreferredFrame, out IFocusNodeFrame lastPreferredFrame);
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    [ContentProperty("Root")]
    public class FocusNodeTemplate : FrameNodeTemplate, IFocusNodeTemplate
    {
        #region Properties
        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        public new IFocusFrame Root { get { return (IFocusFrame)base.Root; } set { base.Root = value; } }

        /// <summary>
        /// True if the associated expression should be surrounded with parenthesis.
        /// (Set in Xaml)
        /// </summary>
        public bool IsComplex { get; set; }

        /// <summary>
        /// True if the parent template rather than this template should be fully displayed when visibility is enforced.
        /// (Set in Xaml)
        /// </summary>
        public bool IsSimple { get; set; }

        /// <summary></summary>
        private protected override bool IsRootValid { get { return Root.ParentFrame == FocusFrame.FocusRoot; } }

        /// <summary>
        /// Checks that a template and all its frames are valid.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                bool IsValid = true;

                IsValid &= base.IsValid;

                GetPreferredFrame(out IFocusNodeFrame FirstPreferredFrame, out IFocusNodeFrame LastPreferredFrame);
                IsValid &= FirstPreferredFrame != null && LastPreferredFrame != null;

                return IsValid;
            }
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Returns the frame associated to a property if can have selectors.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for.</param>
        /// <param name="frame">Frame found upon return. Null if not matching <paramref name="propertyName"/>.</param>
        public virtual bool FrameSelectorForProperty(string propertyName, out IFocusNodeFrameWithSelector frame)
        {
            frame = null;
            bool Found = false;

            if (Root is IFocusSelectorPropertyFrame AsSelectorPropertyFrame)
                Found = AsSelectorPropertyFrame.FrameSelectorForProperty(propertyName, out frame);

            return Found;
        }

        /// <summary>
        /// Gets preferred frames to receive the focus when the source code is changed.
        /// </summary>
        /// <param name="firstPreferredFrame">The first preferred frame found.</param>
        /// <param name="lastPreferredFrame">The last preferred frame found.</param>
        public virtual void GetPreferredFrame(out IFocusNodeFrame firstPreferredFrame, out IFocusNodeFrame lastPreferredFrame)
        {
            firstPreferredFrame = null;
            lastPreferredFrame = null;
            ((IFocusNodeFrame)Root).GetPreferredFrame(ref firstPreferredFrame, ref lastPreferredFrame);
        }
        #endregion
    }
}
