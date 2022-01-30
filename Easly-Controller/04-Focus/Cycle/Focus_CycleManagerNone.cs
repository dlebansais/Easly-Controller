namespace EaslyController.Focus
{
    using System;
    using BaseNode;

    /// <summary>
    /// Cycle manager for no nodes.
    /// </summary>
    public class FocusCycleManagerNone : FocusCycleManager
    {
        #region Properties
        /// <summary>
        /// Type of the base interface for all nodes participating to the cycle.
        /// </summary>
        public override Type InterfaceType { get { return typeof(Node); } }
        #endregion

        #region Implementation
        /// <summary></summary>
        protected override void AddNextNodeToCycle(IFocusCyclableNodeState state)
        {
            throw new System.NotSupportedException();
        }
        #endregion
    }
}
