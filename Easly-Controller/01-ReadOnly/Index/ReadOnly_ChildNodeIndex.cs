using BaseNode;
using System.Diagnostics;

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
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        public string PropertyName { get; private set; }
        #endregion
    }
}
