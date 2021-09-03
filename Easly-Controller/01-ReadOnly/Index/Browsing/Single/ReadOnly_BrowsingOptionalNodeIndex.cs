namespace EaslyController.ReadOnly
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
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

    /// <summary>
    /// Index for an optional node.
    /// </summary>
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
            Debug.Assert(parentNode != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(NodeTreeHelperOptional.IsOptionalChildNodeProperty(parentNode, propertyName, out Type ChildNodeType));

            Optional = NodeTreeHelperOptional.GetOptionalReference(parentNode, propertyName);
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

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyBrowsingOptionalNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyBrowsingOptionalNodeIndex AsOptionalNodeIndex))
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
