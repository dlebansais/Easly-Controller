using BaseNode;
using BaseNodeHelper;
using Easly;
using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingOptionalNodeIndex : IReadOnlyBrowsingChildIndex
    {
        IOptionalReference Optional { get; }
    }

    public class ReadOnlyBrowsingOptionalNodeIndex : IReadOnlyBrowsingOptionalNodeIndex
    {
        #region Init
        public ReadOnlyBrowsingOptionalNodeIndex(INode parentNode, string propertyName)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(NodeTreeHelper.IsOptionalChildNodeProperty(parentNode, propertyName, out Type ChildNodeType));

            Optional = NodeTreeHelper.GetOptionalChildNode(parentNode, propertyName);
            Debug.Assert(Optional != null);

            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        public IOptionalReference Optional { get; }
        public string PropertyName { get; }
        #endregion
    }
}
