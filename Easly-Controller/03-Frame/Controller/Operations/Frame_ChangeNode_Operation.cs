namespace EaslyController.Frame
{
    using System;
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for changing a node.
    /// </summary>
    public interface IFrameChangeNodeOperation : IWriteableChangeNodeOperation, IFrameOperation
    {
        /// <summary>
        /// State changed.
        /// </summary>
        new IFrameNodeState State { get; }
    }

    /// <summary>
    /// Operation details for moving a node in a list or block list.
    /// </summary>
    public class FrameChangeNodeOperation : WriteableChangeNodeOperation, IFrameChangeNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameChangeNodeOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the change is taking place.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="value">The new value.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameChangeNodeOperation(INode parentNode, string propertyName, int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, value, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State changed.
        /// </summary>
        public new IFrameNodeState State { get { return (IFrameNodeState)base.State; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxChangeNodeOperation object.
        /// </summary>
        protected override IWriteableChangeNodeOperation CreateChangeNodeOperation(int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameChangeNodeOperation));
            return new FrameChangeNodeOperation(ParentNode, PropertyName, value, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
