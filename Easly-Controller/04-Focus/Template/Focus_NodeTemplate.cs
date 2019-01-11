using EaslyController.Frame;
using System.Windows.Markup;

namespace EaslyController.Focus
{
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
        #endregion
    }
}
