using EaslyController.ReadOnly;
using EaslyController.Writeable;

namespace EaslyController.Frame
{
    public interface IFrameBrowseContext : IWriteableBrowseContext
    {
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        new IFrameNodeState State { get; }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        new IFrameIndexCollectionReadOnlyList IndexCollectionList { get; }
    }

    public class FrameBrowseContext : WriteableBrowseContext, IFrameBrowseContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameBrowseContext"/>.
        /// </summary>
        /// <param name="state">The state that will be browsed.</param>
        public FrameBrowseContext(IFrameNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        public new IFrameNodeState State { get { return (IFrameNodeState)base.State; } }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        public new IFrameIndexCollectionReadOnlyList IndexCollectionList { get { return (IFrameIndexCollectionReadOnlyList)base.IndexCollectionList; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCollectionList object.
        /// </summary>
        protected override IReadOnlyIndexCollectionList CreateIndexCollectionList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBrowseContext));
            return new FrameIndexCollectionList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollectionReadOnlyList object.
        /// </summary>
        protected override IReadOnlyIndexCollectionReadOnlyList CreateIndexCollectionListReadOnly(IReadOnlyIndexCollectionList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBrowseContext));
            return new FrameIndexCollectionReadOnlyList((IFrameIndexCollectionList)list);
        }
        #endregion
    }
}
