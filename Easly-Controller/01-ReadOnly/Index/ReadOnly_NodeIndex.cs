using BaseNode;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyNodeIndex : IReadOnlyIndex
    {
        INode Node { get; }
    }
    /*
    public abstract class ReadOnlyNodeIndex : IReadOnlyIndex, IReadOnlyNodeIndex
    {
        #region Init
        public ReadOnlyNodeIndex(INode node)
        {
            Debug.Assert(node != null);

            Node = node;
        }
        #endregion

        #region Properties
        public INode Node { get; private set; }
        #endregion
    }*/
}
