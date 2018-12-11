using BaseNode;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyChildNodeIndex : IReadOnlyNodeIndex
    {
        string PropertyName { get; }
    }

    public class ReadOnlyChildNodeIndex : ReadOnlyNodeIndex, IReadOnlyChildNodeIndex
    {
        #region Init
        public ReadOnlyChildNodeIndex(INode node, string propertyName)
            : base(node)
        {
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        public string PropertyName { get; private set; }
        #endregion
    }
}
