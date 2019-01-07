﻿using BaseNode;
using BaseNodeHelper;
using Easly;
using EaslyController.ReadOnly;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for replacing an optional node.
    /// </summary>
    public interface IWriteableInsertionOptionalNodeIndex : IWriteableInsertionChildIndex, IWriteableNodeIndex
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
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.</param>
        /// <param name="node">The assigned node.</param>
        public WriteableInsertionOptionalNodeIndex(INode parentNode, string propertyName, INode node)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(node != null);
            Debug.Assert(NodeTreeHelper.IsAssignable(parentNode, propertyName, node));

            ParentNode = parentNode;
            PropertyName = propertyName;
            Node = node;

            Optional = NodeTreeHelperOptional.GetOptionalReference(parentNode, propertyName);
            Debug.Assert(Optional != null);
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
        /// The assigned node.
        /// </summary>
        public INode Node { get; }

        /// <summary>
        /// Interface to the optional object for the node.
        /// </summary>
        public IOptionalReference Optional { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates a browsing index from an insertion index.
        /// To call after the insertion operation has been completed.
        /// </summary>
        public virtual IWriteableBrowsingChildIndex ToBrowsingIndex()
        {
            return CreateBrowsingIndex();
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteableInsertionOptionalNodeIndex AsInsertionOptionalNodeIndex))
                return false;

            if (ParentNode != AsInsertionOptionalNodeIndex.ParentNode)
                return false;

            if (PropertyName != AsInsertionOptionalNodeIndex.PropertyName)
                return false;

            if (Node != AsInsertionOptionalNodeIndex.Node)
                return false;

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        protected virtual IWriteableBrowsingOptionalNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableInsertionOptionalNodeIndex));
            return new WriteableBrowsingOptionalNodeIndex(ParentNode, PropertyName);
        }
        #endregion
    }
}
