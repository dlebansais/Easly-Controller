using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public interface IFocusCollectionInner : IFrameCollectionInner, IFocusInner
    {
        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFocusPlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public interface IFocusCollectionInner<out IIndex> : IFrameCollectionInner<IIndex>, IFocusInner<IIndex>
        where IIndex : IFocusBrowsingCollectionNodeIndex
    {/// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFocusPlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public abstract class FocusCollectionInner<IIndex, TIndex> : FrameCollectionInner<IIndex, TIndex>, IFocusCollectionInner<IIndex>, IFocusCollectionInner
        where IIndex : IFocusBrowsingCollectionNodeIndex
        where TIndex : FocusBrowsingCollectionNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCollectionInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FocusCollectionInner(IFocusNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IFocusNodeState Owner { get { return (IFocusNodeState)base.Owner; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new IFocusPlaceholderNodeState FirstNodeState { get { return (IFocusPlaceholderNodeState)base.FirstNodeState; } }
        #endregion
    }
}
