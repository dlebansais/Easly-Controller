namespace EaslyController.Focus
{
    using BaseNode;
    using NotNullReflection;

    /// <summary>
    /// Cycle manager for no nodes.
    /// </summary>
    public class FocusCycleManagerNone : FocusCycleManager
    {
        #region Properties
        /// <summary>
        /// Type of the base interface for all nodes participating to the cycle.
        /// </summary>
        public override Type InterfaceType { get { return Type.FromTypeof<Node>(); } }
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
