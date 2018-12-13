using BaseNode;
using BaseNodeHelper;
using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingOptionalNodeIndex : IReadOnlyBrowsingChildIndex
    {
        INode ParentNode { get; }
    }

    public class ReadOnlyBrowsingOptionalNodeIndex : IReadOnlyBrowsingOptionalNodeIndex
    {
        #region Init
        public ReadOnlyBrowsingOptionalNodeIndex(INode parentNode, string propertyName)
        {
            Debug.Assert(NodeTreeHelper.IsOptionalChildNodeProperty(parentNode, propertyName, out Type ChildNodeType));

            ParentNode = parentNode;
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        public INode ParentNode { get; }
        public string PropertyName { get; }
        #endregion
    }
}
