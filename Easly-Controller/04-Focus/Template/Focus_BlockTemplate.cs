namespace EaslyController.Focus
{
    using System.Windows.Markup;
    using EaslyController.Frame;

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFocusBlockTemplate : IFrameBlockTemplate, IFocusTemplate
    {
        /// <summary>
        /// Returns the frame associated to a property if it can have selectors.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for.</param>
        /// <param name="frame">Frame found upon return. Null if not matching <paramref name="propertyName"/>.</param>
        bool FrameSelectorForProperty(string propertyName, out IFocusFrameWithSelector frame);
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

        private protected override bool IsRootValid { get { return Root.ParentFrame == FocusFrame.FocusRoot; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Returns the frame associated to a property if it can have selectors.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for.</param>
        /// <param name="frame">Frame found upon return. Null if not matching <paramref name="propertyName"/>.</param>
        public virtual bool FrameSelectorForProperty(string propertyName, out IFocusFrameWithSelector frame)
        {
            frame = null;
            bool Found = false;

            if (Root is IFocusSelectorPropertyFrame AsSelectorPropertyFrame)
                Found = AsSelectorPropertyFrame.FrameSelectorForProperty(propertyName, out frame);

            return Found;
        }
        #endregion
    }
}
