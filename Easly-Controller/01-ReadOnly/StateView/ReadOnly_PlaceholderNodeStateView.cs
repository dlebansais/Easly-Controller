namespace EaslyController.ReadOnly
{
    public interface IReadOnlyPlaceholderNodeStateView : IReadOnlyNodeStateView
    {
        new IReadOnlyPlaceholderNodeState State { get; }
    }

    public class ReadOnlyPlaceholderNodeStateView : ReadOnlyNodeStateView, IReadOnlyPlaceholderNodeStateView
    {
        #region Init
        public ReadOnlyPlaceholderNodeStateView(IReadOnlyPlaceholderNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        public new IReadOnlyPlaceholderNodeState State { get { return (IReadOnlyPlaceholderNodeState)base.State; } }
        #endregion
    }
}
