using BaseNode;
using BaseNodeHelper;
using Easly;
using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Index for an optional node.
    /// </summary>
    public interface IReadOnlyBrowsingOptionalNodeIndex : IReadOnlyBrowsingChildIndex
    {
        /// <summary>
        /// Interface to the optional object for the node.
        /// </summary>
        IOptionalReference Optional { get; }
    }

    /// <summary>
    /// Index for an optional node.
    /// </summary>
    public class ReadOnlyBrowsingOptionalNodeIndex : IReadOnlyBrowsingOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingOptionalNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.
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
        /// <summary>
        /// Interface to the optional object for the node.
        /// </summary>
        public IOptionalReference Optional { get; }

        /// <summary>
        /// Property indexed for <see cref="Optional"/>.
        /// </summary>
        public string PropertyName { get; }
        #endregion
    }
}
