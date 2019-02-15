using System.Windows.Media.Animation;

namespace EditorDebug
{
    public class FlashEasingFunction : IEasingFunction
    {
        public double Ease(double normalizedTime)
        {
            if (normalizedTime < 0.5)
                return 0.0;
            else
                return 1.0;
        }
    }
}
