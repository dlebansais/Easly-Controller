using System.Diagnostics;

namespace EaslyController.Frame
{
    public interface IFrameContainerCellView : IFrameCellView
    {
        IFrameMutableCellViewCollection ParentCellView { get; }
        IFrameNodeStateView ChildStateView { get; }
    }

    public class FrameContainerCellView : FrameCellView, IFrameContainerCellView
    {
        #region Init
        public FrameContainerCellView(IFrameNodeStateView stateView, IFrameMutableCellViewCollection parentCellView, IFrameNodeStateView childStateView)
            : base(stateView)
        {
            ParentCellView = parentCellView;
            ChildStateView = childStateView;
        }
        #endregion

        #region Properties
        public IFrameMutableCellViewCollection ParentCellView { get; }
        public IFrameNodeStateView ChildStateView { get; }
        #endregion

        #region Client Interface
        public override void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber)
        {
            RecalculateChildLineNumbers(controller, ChildStateView, ref lineNumber, ref columnNumber);
        }
        #endregion

        #region Descendant Interface
        protected virtual void RecalculateChildLineNumbers(IFrameController controller, IFrameNodeStateView nodeStateView, ref int lineNumber, ref int columnNumber)
        {
            nodeStateView.RecalculateLineNumbers(controller, ref lineNumber, ref columnNumber);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameContainerCellView AsContainerCellView))
                return false;

            if (!base.IsEqual(comparer, AsContainerCellView))
                return false;

            if (!comparer.VerifyEqual(ChildStateView, AsContainerCellView.ChildStateView))
                return false;

            return true;
        }

        public override string PrintTree(int indentation)
        {
            string Result = "";
            for (int i = 0; i < indentation; i++)
                Result += " ";

            Result += $"Container, state: {ChildStateView}\n";

            return Result;
        }
        #endregion
    }
}
