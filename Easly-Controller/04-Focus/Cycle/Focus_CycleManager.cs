namespace EaslyController.Focus
{
    using System;
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Base class for cycle managers.
    /// </summary>
    public abstract class FocusCycleManager
    {
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
