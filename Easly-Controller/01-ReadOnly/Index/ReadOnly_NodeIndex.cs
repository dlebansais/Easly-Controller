using BaseNode;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyNodeIndex : IReadOnlyIndex
    {
        INode Node { get; }
    }
}
