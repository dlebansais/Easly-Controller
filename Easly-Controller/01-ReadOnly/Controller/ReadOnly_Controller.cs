using BaseNode;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyController
    {
    }

    public class ReadOnlyController : IReadOnlyController
    {
        #region Init
        public static IReadOnlyController Create(IReadOnlyRootNodeIndex nodeIndex)
        {
            ReadOnlyController Controller = new ReadOnlyController();
            Controller.SetRoot(nodeIndex);
            return Controller;
        }

        protected ReadOnlyController()
        {
            _StateTable = CreateStateTable();
            StateTable = CreateStateTableReadOnly(_StateTable);
        }
        #endregion

        #region Client Interface
        public virtual bool ContainsNode(INode node)
        {
            Debug.Assert(node != null);

            return StateTable.ContainsKey(node);
        }

        public virtual IReadOnlyNodeState NodeToState(INode node)
        {
            if (node == null)
                return null;
            Debug.Assert(node != null);
            if (node == null)
                return null;

            Debug.Assert(StateTable.ContainsKey(node));
            if (!StateTable.ContainsKey(node))
                return null;

            return StateTable[node];
        }
        #endregion

        #region Descendant Interface
        public virtual void SetRoot(IReadOnlyRootNodeIndex rootIndex)
        {
            Debug.Assert(rootIndex != null);

            RootIndex = rootIndex;
        }
        #endregion

        #region StateTable
        public IReadOnlyNodeState RootState { get; private set; }
        public IReadOnlyRootNodeIndex RootIndex { get; private set; }
        protected IReadOnlyNodeStateReadOnlyDictionary<INode> StateTable { get; private set; }
        protected IReadOnlyNodeStateDictionary<INode> _StateTable;
        #endregion

        #region Create Methods
        protected virtual IReadOnlyNodeStateDictionary<INode> CreateStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyNodeStateDictionary<INode>();
        }

        protected virtual IReadOnlyNodeStateReadOnlyDictionary<INode> CreateStateTableReadOnly(IReadOnlyNodeStateDictionary<INode> stateTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyNodeStateReadOnlyDictionary<INode>(stateTable);
        }

        protected virtual IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyInnerDictionary<string>();
        }

        protected virtual IReadOnlyNodeIndexNodeStateDictionary CreateChildStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyNodeIndexNodeStateDictionary();
        }

        protected virtual IReadOnlyBrowseNodeContext CreateBrowseContext(IReadOnlyBrowseNodeContext parentBrowseContext, IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyBrowseNodeContext(state);
        }

        protected virtual IReadOnlyPlaceholderInner CreatePlaceholderInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex, ReadOnlyBrowsingPlaceholderNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        protected virtual IReadOnlyOptionalInner CreateOptionalInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex, ReadOnlyBrowsingOptionalNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        protected virtual IReadOnlyListInner CreateListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyListInner<IReadOnlyBrowsingListNodeIndex, ReadOnlyBrowsingListNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        protected virtual IReadOnlyBlockListInner CreateBlockListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex, ReadOnlyBrowsingBlockNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        protected virtual IReadOnlyBrowsingPatternIndex CreateExistingPatternIndex(IReadOnlyBlockListInner inner, IBlock childBlock, IPattern patternNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyBrowsingPatternIndex(childBlock, patternNode);
        }

        protected virtual IReadOnlyBrowsingSourceIndex CreateExistingSourceIndex(IReadOnlyBlockListInner inner, IBlock childBlock, IIdentifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyBrowsingSourceIndex(childBlock, sourceNode);
        }

        protected virtual IReadOnlyNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyNodeState(nodeIndex.Node);
        }

        protected virtual IReadOnlyPatternState CreatePatternState(IReadOnlyBlockState parentBlockState, IReadOnlyNodeIndex patternIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyPatternState(parentBlockState, patternIndex.Node);
        }

        protected virtual IReadOnlySourceState CreateSourceState(IReadOnlyBlockState parentBlockState, IReadOnlyNodeIndex sourceIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlySourceState(parentBlockState, sourceIndex.Node);
        }

        protected virtual IReadOnlyInnerReadOnlyDictionary<string> CreateInnerTableReadOnly(IReadOnlyInnerDictionary<string> innerTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyInnerReadOnlyDictionary<string>(innerTable);
        }
        #endregion
    }
}
