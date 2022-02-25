namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using NotNullReflection;

    /// <summary>
    /// Base interface for cycle managers.
    /// </summary>
    public interface IFocusCycleManager
    {
        /// <summary>
        /// Type of the base interface for all nodes participating to the cycle.
        /// </summary>
        Type InterfaceType { get; }

        /// <summary>
        /// Adds a new node to the list of nodes that can replace the current one. Does nothing if all types of nodes have been added.
        /// Applies only to bodies and features.
        /// </summary>
        /// <param name="state">The state to update.</param>
        void AddNodeToCycle(IFocusCyclableNodeState state);
    }

    /// <summary>
    /// Base class for cycle managers.
    /// </summary>
    public abstract class FocusCycleManager : IFocusCycleManager
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FocusCycleManager"/> object.
        /// </summary>
        public static IFocusCycleManager Empty { get; } = new FocusCycleManagerNone();
        #endregion

        #region Properties
        /// <summary>
        /// Type of the base interface for all nodes participating to the cycle.
        /// </summary>
        public abstract Type InterfaceType { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Adds a new node to the list of nodes that can replace the current one. Does nothing if all types of nodes have been added.
        /// Applies only to bodies and features.
        /// </summary>
        /// <param name="state">The state to update.</param>
        public virtual void AddNodeToCycle(IFocusCyclableNodeState state)
        {
            state.InitializeCycleIndexList();

            AddNextNodeToCycle(state);

            state.UpdateCyclePosition();
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        protected abstract void AddNextNodeToCycle(IFocusCyclableNodeState state);
        #endregion
    }
}
