using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public interface IFocusRemoveOperation : IFrameRemoveOperation, IFocusOperation
    {
    }

    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public abstract class FocusRemoveOperation : FrameRemoveOperation, IFocusRemoveOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FocusRemoveOperation"/> object.
        /// </summary>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusRemoveOperation(bool isNested)
            : base(isNested)
        {
        }
        #endregion
    }
}
