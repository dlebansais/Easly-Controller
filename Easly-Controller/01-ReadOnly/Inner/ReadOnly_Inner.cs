namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;
    using Contracts;
    using NotNullReflection;

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
        void CloneChildren(Node parentNode);

        /// <summary>
        /// Attach a view to the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        void Attach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet);

        /// <summary>
        /// Detach a view from the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        void Detach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet);
    }

    /// <inheritdoc/>
    public abstract class ReadOnlyInner<IIndex> : IReadOnlyInner<IIndex>, IReadOnlyInner, IEqualComparable
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="ReadOnlyInner{IIndex}"/> object.
        /// </summary>
        public static ReadOnlyInner<IIndex> Empty { get; } = new ReadOnlyEmptyInner<IIndex>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyInner{IIndex}"/> class.
        /// </summary>
        protected ReadOnlyInner()
            : this(ReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        protected ReadOnlyInner(IReadOnlyNodeState owner)
        {
            Owner = owner;
            PropertyName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public ReadOnlyInner(IReadOnlyNodeState owner, string propertyName)
        {
            Contract.RequireNotNull(owner, out IReadOnlyNodeState Owner);
            Contract.RequireNotNull(propertyName, out string PropertyName);
            Debug.Assert(PropertyName.Length > 0);

            this.Owner = Owner;
            this.PropertyName = PropertyName;
        }
        #endregion

        #region Properties
        /// <inheritdoc/>
        public IReadOnlyNodeState Owner { get; }

        /// <inheritdoc/>
        public string PropertyName { get; }

        /// <inheritdoc/>
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
        public abstract void CloneChildren(Node parentNode);

        /// <summary>
        /// Attach a view to the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        public abstract void Attach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet);

        /// <summary>
        /// DEtach a view from the inner.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        public abstract void Detach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet);
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out ReadOnlyInner<IIndex> AsInner))
                return comparer.Failed();

            if (!comparer.VerifyEqual(Owner, AsInner.Owner))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsInner.PropertyName))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
