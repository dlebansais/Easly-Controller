namespace EaslyController.ReadOnly
{
    public interface IReadOnlySourceStateView : IReadOnlyPlaceholderNodeStateView
    {
        new IReadOnlySourceState State { get; }
    }

    public class ReadOnlySourceStateView : ReadOnlyPlaceholderNodeStateView, IReadOnlySourceStateView
    {
        #region Init
        public ReadOnlySourceStateView(IReadOnlySourceState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        public new IReadOnlySourceState State { get { return (IReadOnlySourceState)base.State; } }
        #endregion
    }
}
