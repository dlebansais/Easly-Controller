namespace EaslyController.ReadOnly
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Contracts;
    using Easly;

    /// <summary>
    /// Index for an optional node.
    /// </summary>
    public interface IReadOnlyBrowsingOptionalNodeIndex : IReadOnlyBrowsingChildIndex, IEqualComparable
    {
        /// <summary>
        /// Interface to the optional object for the node.
        /// </summary>
        IOptionalReference Optional { get; }
    }

    /// <inheritdoc/>
    public class ReadOnlyBrowsingOptionalNodeIndex : IReadOnlyBrowsingChildIndex, IReadOnlyBrowsingOptionalNodeIndex, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingOptionalNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.</param>
        public ReadOnlyBrowsingOptionalNodeIndex(Node parentNode, string propertyName)
        {
            Contract.RequireNotNull(parentNode, out Node ParentNode);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(NodeTreeHelperOptional.IsOptionalChildNodeProperty(ParentNode, propertyName, out _));

            Optional = NodeTreeHelperOptional.GetOptionalReference(ParentNode, propertyName);
            Debug.Assert(Optional != null);

            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        /// <inheritdoc/>
        public IOptionalReference Optional { get; }

        /// <inheritdoc/>
        public string PropertyName { get; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out ReadOnlyBrowsingOptionalNodeIndex AsOptionalNodeIndex))
                return comparer.Failed();

            if (!comparer.IsSameReference(Optional, AsOptionalNodeIndex.Optional))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsOptionalNodeIndex.PropertyName))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
