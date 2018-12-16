using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyNodeStateView
    {
        IReadOnlyNodeState State { get; }
    }

    public class ReadOnlyNodeStateView : IReadOnlyNodeStateView
    {
        #region Init
        public ReadOnlyNodeStateView(IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);

            State = state;
        }
        #endregion

        #region Properties
        public IReadOnlyNodeState State { get; }
        #endregion
    }
}
