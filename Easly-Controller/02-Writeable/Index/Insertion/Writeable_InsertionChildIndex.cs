using BaseNode;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for an inserted child node.
    /// </summary>
    public interface IWriteableInsertionChildIndex : IWriteableChildIndex
    {
        /// <summary>
        /// Node in which the insertion operation is taking place.
        /// </summary>
        INode ParentNode { get; }
    }
}
