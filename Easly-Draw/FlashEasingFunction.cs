namespace EaslyDraw
{
    using System.Windows.Media.Animation;

    /// <summary>
    /// A function defining a flashing caret animation.
    /// </summary>
    public class FlashEasingFunction : IEasingFunction
    {
        /// <summary>
        /// True if the caret is visible.
        /// </summary>
        public bool IsVisible { get; private set; }

        /// <summary>
        /// Gets the opacity of the caret depending on the current time.
        /// </summary>
        /// <param name="normalizedTime">The current time.</param>
        /// <returns>Opacity of the caret.</returns>
        public virtual double Ease(double normalizedTime)
        {
            if (IsVisible && normalizedTime >= 0.5)
                return 1.0;
            else
                return 0.0;
        }

        /// <summary>
        /// Shows of hides the caret.
        /// </summary>
        /// <param name="isVisible">True if the caret is visible</param>
        public virtual void SetIsVisible(bool isVisible)
        {
            IsVisible = isVisible;
        }
    }
}
