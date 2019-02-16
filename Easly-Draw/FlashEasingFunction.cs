namespace EaslyDraw
{
    using System.Windows.Media.Animation;

    /// <summary>
    /// A function defining a flashing caret animation.
    /// </summary>
    public class FlashEasingFunction : IEasingFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlashEasingFunction"/> class.
        /// </summary>
        public FlashEasingFunction()
        {
            IsActive = true;
            IsInactiveVisible = false;
        }

        /// <summary>
        /// True if flashing is active.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// True if visible when inactive.
        /// </summary>
        public bool IsInactiveVisible { get; private set; }

        /// <summary>
        /// Gets the opacity of the caret depending on the current time.
        /// </summary>
        /// <param name="normalizedTime">The current time.</param>
        /// <returns>Opacity of the caret.</returns>
        public virtual double Ease(double normalizedTime)
        {
            if ((IsActive && normalizedTime >= 0.5) || (!IsActive && IsInactiveVisible))
                return 0.0;
            else
                return 1.0;
        }

        /// <summary>
        /// Shows of hides the caret.
        /// </summary>
        /// <param name="isActive">True if the caret is flashing.</param>
        /// <param name="isInactiveVisible">True if visible when inactive.</param>
        public virtual void SetIsVisible(bool isActive, bool isInactiveVisible)
        {
            IsActive = isActive;
            IsInactiveVisible = isInactiveVisible;
        }
    }
}
