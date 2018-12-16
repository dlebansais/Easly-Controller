namespace EaslyController.ReadOnly
{
    public interface IReadOnlyOptionalNodeStateView : IReadOnlyNodeStateView
    {
        new IReadOnlyOptionalNodeState State { get; }
    }

    public class ReadOnlyOptionalNodeStateView : ReadOnlyNodeStateView, IReadOnlyOptionalNodeStateView
    {
        #region Init
        public ReadOnlyOptionalNodeStateView(IReadOnlyOptionalNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        public new IReadOnlyOptionalNodeState State { get { return (IReadOnlyOptionalNodeState)base.State; } }
        #endregion
    }
}
