using BaseNode;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlySourceState : IReadOnlyPlaceholderNodeState
    {
        new IIdentifier Node { get; }
        IReadOnlyBlockState ParentBlockState { get; }
        new IIdentifier CloneNode();
    }

    public class ReadOnlySourceState : ReadOnlyPlaceholderNodeState, IReadOnlySourceState
    {
        #region Init
        public ReadOnlySourceState(IReadOnlyBlockState parentBlockState, IReadOnlyNodeIndex index)
            : base(index)
        {
            Debug.Assert(parentBlockState != null);

            ParentBlockState = parentBlockState;
        }
        #endregion

        #region Properties
        public IReadOnlyBlockState ParentBlockState { get; private set; }
        public new IIdentifier Node { get { return (IIdentifier)base.Node; } }
        #endregion

        #region Client Interface
        public new IIdentifier CloneNode() { return (IIdentifier)base.CloneNode(); }
        #endregion
    }
}
