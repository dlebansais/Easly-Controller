namespace EaslyController.ReadOnly
{
    public interface IReadOnlyPatternStateView : IReadOnlyPlaceholderNodeStateView
    {
        new IReadOnlyPatternState State { get; }
    }

    public class ReadOnlyPatternStateView : ReadOnlyPlaceholderNodeStateView, IReadOnlyPatternStateView
    {
        #region Init
        public ReadOnlyPatternStateView(IReadOnlyPatternState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        public new IReadOnlyPatternState State { get { return (IReadOnlyPatternState)base.State; } }
        #endregion
    }
}
