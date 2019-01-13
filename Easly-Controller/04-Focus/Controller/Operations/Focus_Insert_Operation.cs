using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    public interface IFocusInsertOperation : IFrameInsertOperation, IFocusOperation
    {
    }

    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    public abstract class FocusInsertOperation : FrameInsertOperation, IFocusInsertOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FocusInsertOperation"/> object.
        /// </summary>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusInsertOperation(bool isNested)
            : base(isNested)
        {
        }
        #endregion
    }
}
