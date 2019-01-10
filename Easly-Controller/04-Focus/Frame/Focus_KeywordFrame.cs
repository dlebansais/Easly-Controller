using EaslyController.Frame;
using System.Windows.Markup;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus for decoration purpose only.
    /// </summary>
    public interface IFocusKeywordFrame : IFrameKeywordFrame
    {
    }

    /// <summary>
    /// Focus for decoration purpose only.
    /// </summary>
    [ContentProperty("Text")]
    public class FocusKeywordFrame : FrameKeywordFrame, IFocusKeywordFrame
    {
    }
}
