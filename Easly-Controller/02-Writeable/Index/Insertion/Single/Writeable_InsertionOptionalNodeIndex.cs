using BaseNode;
using BaseNodeHelper;
using Easly;
using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for replacing an optional node.
    /// </summary>
    public interface IWriteableInsertionOptionalNodeIndex : IWriteableInsertionChildIndex
    {
        /// <summary>
        /// Interface to the optional object for the node.
        /// </summary>
        IOptionalReference Optional { get; }
    }

    /// <summary>
    /// Index for replacing an optional node.
    /// </summary>
    public class WriteableInsertionOptionalNodeIndex : IWriteableInsertionOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionOptionalNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.
        public WriteableInsertionOptionalNodeIndex(INode parentNode, string propertyName)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(NodeTreeHelperOptional.IsOptionalChildNodeProperty(parentNode, propertyName, out Type ChildNodeType));

            Optional = NodeTreeHelperOptional.GetOptionalChildNode(parentNode, propertyName);
            Debug.Assert(Optional != null);

            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node in which the insertion operation is taking place.
        /// </summary>
        public INode ParentNode { get; }

        /// <summary>
        /// Property indexed for <see cref="Optional"/>.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Interface to the optional object for the node.
        /// </summary>
        public IOptionalReference Optional { get; }
        #endregion
    }
}
