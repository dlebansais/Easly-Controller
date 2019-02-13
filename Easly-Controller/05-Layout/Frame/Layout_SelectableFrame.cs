namespace EaslyController.Layout
{
    using System.Windows.Markup;
    using EaslyController.Focus;

    /// <summary>
    /// Frame selectable by name.
    /// </summary>
    public interface ILayoutSelectableFrame : IFocusSelectableFrame, ILayoutFrame, ILayoutNodeFrame
    {
        /// <summary>
        /// The selectable frame.
        /// (Set in Xaml)
        /// </summary>
        new ILayoutNodeFrame Content { get; }
    }

    /// <summary>
    /// Frame selectable by name.
    /// </summary>
    [ContentProperty("Content")]
    public class LayoutSelectableFrame : FocusSelectableFrame, ILayoutSelectableFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new ILayoutTemplate ParentTemplate { get { return (ILayoutTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new ILayoutFrame ParentFrame { get { return (ILayoutFrame)base.ParentFrame; } }

        /// <summary>
        /// The selectable frame.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutNodeFrame Content { get { return (ILayoutNodeFrame)base.Content; } set { base.Content = value; } }
        #endregion
    }
}
