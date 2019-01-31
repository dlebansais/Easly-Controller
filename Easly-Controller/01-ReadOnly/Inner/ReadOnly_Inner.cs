namespace EaslyController.ReadOnly
{
    using System;
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public interface IReadOnlyInner : IEqualComparable
    {
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        IReadOnlyNodeState Owner { get; }

        /// <summary>
        /// Property name of the inner in the parent.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Interface type for all nodes in the inner.
        /// </summary>
        Type InterfaceType { get; }

        /// <summary>
        /// Creates a clone of all children of the inner, using <paramref name="parentNode"/> as their parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains references to cloned children upon return.</param>
        void CloneChildren(INode parentNode);
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    public interface IReadOnlyInner<out IIndex> : IEqualComparable
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        IReadOnlyNodeState Owner { get; }

        /// <summary>
        /// Property name of the inner in the parent.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Interface type for all nodes in the inner.
        /// </summary>
        Type InterfaceType { get; }

        /// <summary>
        /// Initializes a newly created state for a node in the inner.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex);

        /// <summary>
        /// Creates a clone of all children of the inner, using <paramref name="parentNode"/> as their parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains references to cloned children upon return.</param>
        void CloneChildren(INode parentNode);

        /// <summary>
        /// Attach a view to the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        void Attach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet);

        /// <summary>
        /// Detach a view from the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        void Detach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet);
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal abstract class ReadOnlyInner<IIndex> : IReadOnlyInner<IIndex>, IReadOnlyInner
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public ReadOnlyInner(IReadOnlyNodeState owner, string propertyName)
        {
            Debug.Assert(owner != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            Owner = owner;
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public IReadOnlyNodeState Owner { get; }

        /// <summary>
        /// Property name of the inner in the parent.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Interface type for all nodes in the inner.
        /// </summary>
        public abstract Type InterfaceType { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Initializes a newly created state for a node in the inner.
        /// </summary>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>The created node state.</returns>
        public abstract IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex);

        /// <summary>
        /// Creates a clone of all children of the inner, using <paramref name="parentNode"/> as their parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains references to cloned children upon return.</param>
        public abstract void CloneChildren(INode parentNode);

        /// <summary>
        /// Attach a view to the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        public abstract void Attach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet);

        /// <summary>
        /// DEtach a view from the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        public abstract void Detach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet);
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyInner"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyInner<IIndex> AsInner))
                return comparer.Failed();

            if (!comparer.VerifyEqual(Owner, AsInner.Owner))
                return comparer.Failed();

            if (PropertyName != AsInner.PropertyName)
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
