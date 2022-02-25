using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Exception = System.Exception;
using ArgumentException = System.ArgumentException;
using Guid = System.Guid;
using EaslyController;
using EaslyController.Constants;
using EaslyController.Controller;
using EaslyController.Focus;
using EaslyController.Frame;
using EaslyController.Layout;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using NUnit.Framework;
using NotNullReflection;

namespace Coverage
{
    [TestFixture]
    public partial class CoverageSet
    {
        [Test]
        [Category("Coverage")]
        public static void WriteableCreation()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;
            WriteableController Controller;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

            try
            {
                RootIndex = new WriteableRootNodeIndex(RootNode);
                Controller = (WriteableController)WriteableController.Create(RootIndex);
            }
            catch (Exception e)
            {
                Assert.Fail($"#0: {e}");
            }

            RootNode = CreateRoot(ValueGuid0, Imperfections.BadGuid);
            Assert.That(!BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode, throwOnInvalid: false));

            try
            {
                RootIndex = new WriteableRootNodeIndex(RootNode);
                Assert.Fail($"#1: no exception");
            }
            catch (ArgumentException e)
            {
                Assert.That(e.Message == "node", $"#1: wrong exception message '{e.Message}'");
            }
            catch (Exception e)
            {
                Assert.Fail($"#1: {e}");
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableProperties()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex0;
            IWriteableRootNodeIndex RootIndex1;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

            RootIndex0 = new WriteableRootNodeIndex(RootNode);
            Assert.That(RootIndex0.Node == RootNode);
            Assert.That(RootIndex0.IsEqual(CompareEqual.New(), RootIndex0));

            RootIndex1 = new WriteableRootNodeIndex(RootNode);
            Assert.That(RootIndex1.Node == RootNode);
            Assert.That(CompareEqual.CoverIsEqual(RootIndex0, RootIndex1));

            WriteableController Controller0 = (WriteableController)WriteableController.Create(RootIndex0);
            Assert.That(Controller0.RootIndex == RootIndex0);

            Stats Stats = Controller0.Stats;
            Assert.That(Stats.NodeCount >= 0);
            Assert.That(Stats.PlaceholderNodeCount >= 0);
            Assert.That(Stats.OptionalNodeCount >= 0);
            Assert.That(Stats.AssignedOptionalNodeCount >= 0);
            Assert.That(Stats.ListCount >= 0);
            Assert.That(Stats.BlockListCount >= 0);
            Assert.That(Stats.BlockCount >= 0);

            IWriteablePlaceholderNodeState RootState = Controller0.RootState;
            Assert.That(RootState.ParentIndex == RootIndex0);

            Assert.That(Controller0.Contains(RootIndex0));
            Assert.That(Controller0.IndexToState(RootIndex0) == RootState);

            Assert.That(RootState.InnerTable.Count == 8);
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.PlaceholderTree)));
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.PlaceholderLeaf)));
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.UnassignedOptionalLeaf)));
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.EmptyOptionalLeaf)));
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.AssignedOptionalTree)));
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.AssignedOptionalLeaf)));
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.LeafBlocks)));
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.LeafPath)));

            IWriteablePlaceholderInner MainPlaceholderTreeInner = (IWriteablePlaceholderInner)RootState.PropertyToInner(nameof(Main.PlaceholderTree));
            Assert.That(MainPlaceholderTreeInner != null);
            Assert.That(MainPlaceholderTreeInner.InterfaceType.IsTypeof<Tree>());
            Assert.That(MainPlaceholderTreeInner.ChildState != null);
            Assert.That(MainPlaceholderTreeInner.ChildState.ParentInner == MainPlaceholderTreeInner);

            IWriteablePlaceholderInner MainPlaceholderLeafInner = (IWriteablePlaceholderInner)RootState.PropertyToInner(nameof(Main.PlaceholderLeaf));
            Assert.That(MainPlaceholderLeafInner != null);
            Assert.That(MainPlaceholderLeafInner.InterfaceType.IsTypeof<Leaf>());
            Assert.That(MainPlaceholderLeafInner.ChildState != null);
            Assert.That(MainPlaceholderLeafInner.ChildState.ParentInner == MainPlaceholderLeafInner);

            IWriteableOptionalInner MainUnassignedOptionalInner = (IWriteableOptionalInner)RootState.PropertyToInner(nameof(Main.UnassignedOptionalLeaf));
            Assert.That(MainUnassignedOptionalInner != null);
            Assert.That(MainUnassignedOptionalInner.InterfaceType.IsTypeof<Leaf>());
            Assert.That(!MainUnassignedOptionalInner.IsAssigned);
            Assert.That(MainUnassignedOptionalInner.ChildState != null);
            Assert.That(MainUnassignedOptionalInner.ChildState.ParentInner == MainUnassignedOptionalInner);

            IWriteableOptionalInner MainAssignedOptionalTreeInner = (IWriteableOptionalInner)RootState.PropertyToInner(nameof(Main.AssignedOptionalTree));
            Assert.That(MainAssignedOptionalTreeInner != null);
            Assert.That(MainAssignedOptionalTreeInner.InterfaceType.IsTypeof<Tree>());
            Assert.That(MainAssignedOptionalTreeInner.IsAssigned);

            IWriteableNodeState AssignedOptionalTreeState = MainAssignedOptionalTreeInner.ChildState;
            Assert.That(AssignedOptionalTreeState != null);
            Assert.That(AssignedOptionalTreeState.ParentInner == MainAssignedOptionalTreeInner);
            Assert.That(AssignedOptionalTreeState.ParentState == RootState);

            WriteableNodeStateReadOnlyList AssignedOptionalTreeAllChildren = (WriteableNodeStateReadOnlyList)AssignedOptionalTreeState.GetAllChildren();
            Assert.That(AssignedOptionalTreeAllChildren != null);
            Assert.That(AssignedOptionalTreeAllChildren.Count == 2, $"New count: {AssignedOptionalTreeAllChildren.Count}");

            IWriteableOptionalInner MainAssignedOptionalLeafInner = (IWriteableOptionalInner)RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf));
            Assert.That(MainAssignedOptionalLeafInner != null);
            Assert.That(MainAssignedOptionalLeafInner.InterfaceType.IsTypeof<Leaf>());
            Assert.That(MainAssignedOptionalLeafInner.IsAssigned);
            Assert.That(MainAssignedOptionalLeafInner.ChildState != null);
            Assert.That(MainAssignedOptionalLeafInner.ChildState.ParentInner == MainAssignedOptionalLeafInner);

            IWriteableBlockListInner MainLeafBlocksInner = (IWriteableBlockListInner)RootState.PropertyToInner(nameof(Main.LeafBlocks));
            Assert.That(MainLeafBlocksInner != null);
            Assert.That(!MainLeafBlocksInner.IsNeverEmpty);
            Assert.That(!MainLeafBlocksInner.IsEmpty);
            Assert.That(!MainLeafBlocksInner.IsSingle);
            Assert.That(MainLeafBlocksInner.InterfaceType.IsTypeof<Leaf>());
            Assert.That(MainLeafBlocksInner.BlockType.IsTypeof<BaseNode.IBlock<Leaf>>());
            Assert.That(MainLeafBlocksInner.ItemType.IsTypeof<Leaf>());
            Assert.That(MainLeafBlocksInner.Count == 4);
            Assert.That(MainLeafBlocksInner.BlockStateList != null);
            Assert.That(MainLeafBlocksInner.BlockStateList.Count == 3);
            Assert.That(MainLeafBlocksInner.AllIndexes().Count == MainLeafBlocksInner.Count);

            IWriteableBlockState LeafBlock = MainLeafBlocksInner.BlockStateList[0];
            Assert.That(LeafBlock != null);
            Assert.That(LeafBlock.StateList != null);
            Assert.That(LeafBlock.StateList.Count == 1);
            Assert.That(MainLeafBlocksInner.FirstNodeState == LeafBlock.StateList[0]);
            Assert.That(MainLeafBlocksInner.IndexAt(0, 0) == MainLeafBlocksInner.FirstNodeState.ParentIndex);

            IWriteablePlaceholderInner PatternInner = (IWriteablePlaceholderInner)LeafBlock.PropertyToInner(nameof(BaseNode.IBlock.ReplicationPattern));
            Assert.That(PatternInner != null);

            IWriteablePlaceholderInner SourceInner = (IWriteablePlaceholderInner)LeafBlock.PropertyToInner(nameof(BaseNode.IBlock.SourceIdentifier));
            Assert.That(SourceInner != null);

            IWriteablePatternState PatternState = LeafBlock.PatternState;
            Assert.That(PatternState != null);
            Assert.That(PatternState.ParentBlockState == LeafBlock);
            Assert.That(PatternState.ParentInner == PatternInner);
            Assert.That(PatternState.ParentIndex == LeafBlock.PatternIndex);
            Assert.That(PatternState.ParentState == RootState);
            Assert.That(PatternState.InnerTable.Count == 0);
            Assert.That(PatternState is IWriteableNodeState AsPlaceholderPatternNodeState && AsPlaceholderPatternNodeState.ParentIndex == LeafBlock.PatternIndex);
            Assert.That(PatternState.GetAllChildren().Count == 1);

            IWriteableSourceState SourceState = LeafBlock.SourceState;
            Assert.That(SourceState != null);
            Assert.That(SourceState.ParentBlockState == LeafBlock);
            Assert.That(SourceState.ParentInner == SourceInner);
            Assert.That(SourceState.ParentIndex == LeafBlock.SourceIndex);
            Assert.That(SourceState.ParentState == RootState);
            Assert.That(SourceState.InnerTable.Count == 0);
            Assert.That(SourceState is IWriteableNodeState AsPlaceholderSourceNodeState && AsPlaceholderSourceNodeState.ParentIndex == LeafBlock.SourceIndex);
            Assert.That(SourceState.GetAllChildren().Count == 1);

            Assert.That(MainLeafBlocksInner.FirstNodeState == LeafBlock.StateList[0]);

            IWriteableListInner MainLeafPathInner = (IWriteableListInner)RootState.PropertyToInner(nameof(Main.LeafPath));
            Assert.That(MainLeafPathInner != null);
            Assert.That(!MainLeafPathInner.IsNeverEmpty);
            Assert.That(MainLeafPathInner.InterfaceType.IsTypeof<Leaf>());
            Assert.That(MainLeafPathInner.Count == 2);
            Assert.That(MainLeafPathInner.StateList != null);
            Assert.That(MainLeafPathInner.StateList.Count == 2);
            Assert.That(MainLeafPathInner.FirstNodeState == MainLeafPathInner.StateList[0]);
            Assert.That(MainLeafPathInner.IndexAt(0) == MainLeafPathInner.FirstNodeState.ParentIndex);
            Assert.That(MainLeafPathInner.AllIndexes().Count == MainLeafPathInner.Count);

            WriteableNodeStateReadOnlyList AllChildren = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
            Assert.That(AllChildren.Count == 19, $"New count: {AllChildren.Count}");

            IWriteablePlaceholderInner PlaceholderInner = (IWriteablePlaceholderInner)RootState.InnerTable[nameof(Main.PlaceholderLeaf)];
            Assert.That(PlaceholderInner != null);

            IWriteableBrowsingPlaceholderNodeIndex PlaceholderNodeIndex = (IWriteableBrowsingPlaceholderNodeIndex)PlaceholderInner.ChildState.ParentIndex;
            Assert.That(PlaceholderNodeIndex != null);
            Assert.That(Controller0.Contains(PlaceholderNodeIndex));

            IWriteableOptionalInner UnassignedOptionalInner = (IWriteableOptionalInner)RootState.InnerTable[nameof(Main.UnassignedOptionalLeaf)];
            Assert.That(UnassignedOptionalInner != null);

            IWriteableBrowsingOptionalNodeIndex UnassignedOptionalNodeIndex = UnassignedOptionalInner.ChildState.ParentIndex;
            Assert.That(UnassignedOptionalNodeIndex != null);
            Assert.That(Controller0.Contains(UnassignedOptionalNodeIndex));
            Assert.That(Controller0.IsAssigned(UnassignedOptionalNodeIndex) == false);

            IWriteableOptionalInner AssignedOptionalInner = (IWriteableOptionalInner)RootState.InnerTable[nameof(Main.AssignedOptionalLeaf)];
            Assert.That(AssignedOptionalInner != null);

            IWriteableBrowsingOptionalNodeIndex AssignedOptionalNodeIndex = AssignedOptionalInner.ChildState.ParentIndex;
            Assert.That(AssignedOptionalNodeIndex != null);
            Assert.That(Controller0.Contains(AssignedOptionalNodeIndex));
            Assert.That(Controller0.IsAssigned(AssignedOptionalNodeIndex) == true);

            int Min, Max;
            object ReadValue;

            RootState.PropertyToValue(nameof(Main.ValueBoolean), out ReadValue, out Min, out Max);
            bool ReadAsBoolean = ((int)ReadValue) != 0;
            Assert.That(ReadAsBoolean == true);
            Assert.That(Controller0.GetDiscreteValue(RootIndex0, nameof(Main.ValueBoolean), out Min, out Max) == (ReadAsBoolean ? 1 : 0));
            Assert.That(Min == 0);
            Assert.That(Max == 1);

            RootState.PropertyToValue(nameof(Main.ValueEnum), out ReadValue, out Min, out Max);
            BaseNode.CopySemantic ReadAsEnum = (BaseNode.CopySemantic)(int)ReadValue;
            Assert.That(ReadAsEnum == BaseNode.CopySemantic.Value);
            Assert.That(Controller0.GetDiscreteValue(RootIndex0, nameof(Main.ValueEnum), out Min, out Max) == (int)ReadAsEnum);
            Assert.That(Min == 0);
            Assert.That(Max == 2);

            RootState.PropertyToValue(nameof(Main.ValueString), out ReadValue, out Min, out Max);
            string ReadAsString = ReadValue as string;
            Assert.That(ReadAsString == "s");
            Assert.That(Controller0.GetStringValue(RootIndex0, nameof(Main.ValueString)) == ReadAsString);

            RootState.PropertyToValue(nameof(Main.ValueGuid), out ReadValue, out Min, out Max);
            Guid ReadAsGuid = (Guid)ReadValue;
            Assert.That(ReadAsGuid == ValueGuid0);
            Assert.That(Controller0.GetGuidValue(RootIndex0, nameof(Main.ValueGuid)) == ReadAsGuid);

            WriteableController Controller1 = (WriteableController)WriteableController.Create(RootIndex0);
            Assert.That(Controller0.IsEqual(CompareEqual.New(), Controller0));

            Assert.That(CompareEqual.CoverIsEqual(Controller0, Controller1));

            Assert.That(!Controller0.CanUndo);
            Assert.That(!Controller0.CanRedo);

            //System.Diagnostics.Debug.Assert(false);
            Controller0.Unassign(AssignedOptionalNodeIndex, out bool IsChanged);
            Assert.That(IsChanged);
            Assert.That(!CompareEqual.CoverIsEqual(Controller0, Controller1, canReturnFalse: true));
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableClone()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode = CreateRoot(ValueGuid0, Imperfections.None);

            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(RootNode);
            Assert.That(RootIndex != null);

            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            Assert.That(Controller != null);

            IWriteablePlaceholderNodeState RootState = Controller.RootState;
            Assert.That(RootState != null);

            BaseNode.Node ClonedNode = RootState.CloneNode();
            Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(ClonedNode));

            IWriteableRootNodeIndex CloneRootIndex = new WriteableRootNodeIndex(ClonedNode);
            Assert.That(CloneRootIndex != null);

            WriteableController CloneController = (WriteableController)WriteableController.Create(CloneRootIndex);
            Assert.That(CloneController != null);

            IWriteablePlaceholderNodeState CloneRootState = Controller.RootState;
            Assert.That(CloneRootState != null);

            WriteableNodeStateReadOnlyList AllChildren = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
            WriteableNodeStateReadOnlyList CloneAllChildren = (WriteableNodeStateReadOnlyList)CloneRootState.GetAllChildren();
            Assert.That(AllChildren.Count == CloneAllChildren.Count);
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableViews()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);
                Assert.That(ControllerView0.RootStateView == ControllerView0.StateViewTable[Controller.RootState]);

                using (WriteableControllerView ControllerView1 = WriteableControllerView.Create(Controller))
                {
                    Assert.That(ControllerView0.IsEqual(CompareEqual.New(), ControllerView0));
                    Assert.That(CompareEqual.CoverIsEqual(ControllerView0, ControllerView1));
                }

                foreach (IWriteableBlockState BlockState in ControllerView0.BlockStateViewTable.Keys)
                {
                    Assert.That(BlockState != null);

                    WriteableBlockStateView BlockStateView = (WriteableBlockStateView)ControllerView0.BlockStateViewTable[BlockState];
                    Assert.That(BlockStateView != null);
                    Assert.That(BlockStateView.BlockState == BlockState);

                    Assert.That(BlockStateView.ControllerView == ControllerView0);
                }

                foreach (IWriteableNodeState State in ControllerView0.StateViewTable.Keys)
                {
                    Assert.That(State != null);

                    IWriteableNodeStateView StateView = (IWriteableNodeStateView)ControllerView0.StateViewTable[State];
                    Assert.That(StateView != null);
                    Assert.That(StateView.State == State);

                    IWriteableIndex ParentIndex = State.ParentIndex;
                    Assert.That(ParentIndex != null);

                    Assert.That(Controller.Contains(ParentIndex));
                    Assert.That(StateView.ControllerView == ControllerView0);

                    switch (StateView)
                    {
                        case WriteablePatternStateView AsPatternStateView:
                            Assert.That(AsPatternStateView.State == State);
                            Assert.That(AsPatternStateView is IWriteableNodeStateView AsPlaceholderPatternNodeStateView && AsPlaceholderPatternNodeStateView.State == State);
                            break;

                        case WriteableSourceStateView AsSourceStateView:
                            Assert.That(AsSourceStateView.State == State);
                            Assert.That(AsSourceStateView is IWriteableNodeStateView AsPlaceholderSourceNodeStateView && AsPlaceholderSourceNodeStateView.State == State);
                            break;

                        case WriteablePlaceholderNodeStateView AsPlaceholderNodeStateView:
                            Assert.That(AsPlaceholderNodeStateView.State == State);
                            break;

                        case WriteableOptionalNodeStateView AsOptionalNodeStateView:
                            Assert.That(AsOptionalNodeStateView.State == State);
                            break;
                    }
                }
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableInsert()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IWriteableListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IWriteableListInner;
                Assert.That(LeafPathInner != null);

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                IWriteableBrowsingListNodeIndex ExistingIndex = LeafPathInner.IndexAt(0) as IWriteableBrowsingListNodeIndex;

                Leaf NewItem0 = CreateLeaf(Guid.NewGuid());

                IWriteableInsertionListNodeIndex InsertionIndex0;
                InsertionIndex0 = ExistingIndex.ToInsertionIndex(RootNode, NewItem0) as IWriteableInsertionListNodeIndex;
                Assert.That(InsertionIndex0.ParentNode == RootNode);
                Assert.That(InsertionIndex0.Node == NewItem0);
                Assert.That(CompareEqual.CoverIsEqual(InsertionIndex0, InsertionIndex0));

                WriteableNodeStateReadOnlyList AllChildren0 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Insert(LeafPathInner, InsertionIndex0, out IWriteableBrowsingCollectionNodeIndex NewItemIndex0);
                Assert.That(Controller.Contains(NewItemIndex0));

                IWriteableBrowsingListNodeIndex DuplicateExistingIndex0 = InsertionIndex0.ToBrowsingIndex() as IWriteableBrowsingListNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex0 as IWriteableBrowsingListNodeIndex, DuplicateExistingIndex0));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex0, NewItemIndex0 as IWriteableBrowsingListNodeIndex));

                Assert.That(LeafPathInner.Count == PathCount + 1);
                Assert.That(LeafPathInner.StateList.Count == PathCount + 1);

                IWriteablePlaceholderNodeState NewItemState0 = (IWriteablePlaceholderNodeState)LeafPathInner.StateList[0];
                Assert.That(NewItemState0.Node == NewItem0);
                Assert.That(NewItemState0.ParentIndex == NewItemIndex0);

                WriteableNodeStateReadOnlyList AllChildren1 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                IWriteableBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IWriteableBlockListInner;
                Assert.That(LeafBlocksInner != null);

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[0].StateList.Count;
                Assert.That(BlockNodeCount == 4);

                IWriteableBrowsingExistingBlockNodeIndex ExistingIndex1 = LeafBlocksInner.IndexAt(0, 0) as IWriteableBrowsingExistingBlockNodeIndex;

                Leaf NewItem1 = CreateLeaf(Guid.NewGuid());
                IWriteableInsertionExistingBlockNodeIndex InsertionIndex1;
                InsertionIndex1 = ExistingIndex1.ToInsertionIndex(RootNode, NewItem1) as IWriteableInsertionExistingBlockNodeIndex;
                Assert.That(InsertionIndex1.ParentNode == RootNode);
                Assert.That(InsertionIndex1.Node == NewItem1);
                Assert.That(CompareEqual.CoverIsEqual(InsertionIndex1, InsertionIndex1));

                Controller.Insert(LeafBlocksInner, InsertionIndex1, out IWriteableBrowsingCollectionNodeIndex NewItemIndex1);
                Assert.That(Controller.Contains(NewItemIndex1));

                IWriteableBrowsingExistingBlockNodeIndex DuplicateExistingIndex1 = InsertionIndex1.ToBrowsingIndex() as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex1 as IWriteableBrowsingExistingBlockNodeIndex, DuplicateExistingIndex1));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex1, NewItemIndex1 as IWriteableBrowsingExistingBlockNodeIndex));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount + 1);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == NodeCount + 1);

                IWriteablePlaceholderNodeState NewItemState1 = (IWriteablePlaceholderNodeState)LeafBlocksInner.BlockStateList[0].StateList[0];
                Assert.That(NewItemState1.Node == NewItem1);
                Assert.That(NewItemState1.ParentIndex == NewItemIndex1);
                Assert.That(NewItemState1.ParentState == RootState);

                WriteableNodeStateReadOnlyList AllChildren2 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count + 1, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));




                Leaf NewItem2 = CreateLeaf(Guid.NewGuid());
                BaseNode.Pattern NewPattern = BaseNodeHelper.NodeHelper.CreateSimplePattern("");
                BaseNode.Identifier NewSource = BaseNodeHelper.NodeHelper.CreateSimpleIdentifier("");

                IWriteableInsertionNewBlockNodeIndex InsertionIndex2 = new WriteableInsertionNewBlockNodeIndex(RootNode, nameof(Main.LeafBlocks), NewItem2, 0, NewPattern, NewSource);
                Assert.That(CompareEqual.CoverIsEqual(InsertionIndex2, InsertionIndex2));

                int BlockCount = LeafBlocksInner.BlockStateList.Count;
                Assert.That(BlockCount == 3);

                Controller.Insert(LeafBlocksInner, InsertionIndex2, out IWriteableBrowsingCollectionNodeIndex NewItemIndex2);
                Assert.That(Controller.Contains(NewItemIndex2));

                IWriteableBrowsingExistingBlockNodeIndex DuplicateExistingIndex2 = InsertionIndex2.ToBrowsingIndex() as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex2 as IWriteableBrowsingExistingBlockNodeIndex, DuplicateExistingIndex2));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex2, NewItemIndex2 as IWriteableBrowsingExistingBlockNodeIndex));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount + 2);
                Assert.That(LeafBlocksInner.BlockStateList.Count == BlockCount + 1);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == 1, $"Count: {LeafBlocksInner.BlockStateList[0].StateList.Count}");
                Assert.That(LeafBlocksInner.BlockStateList[1].StateList.Count == 2, $"Count: {LeafBlocksInner.BlockStateList[1].StateList.Count}");
                Assert.That(LeafBlocksInner.BlockStateList[2].StateList.Count == 2, $"Count: {LeafBlocksInner.BlockStateList[2].StateList.Count}");

                IWriteablePlaceholderNodeState NewItemState2 = (IWriteablePlaceholderNodeState)LeafBlocksInner.BlockStateList[0].StateList[0];
                Assert.That(NewItemState2.Node == NewItem2);
                Assert.That(NewItemState2.ParentIndex == NewItemIndex2);

                WriteableNodeStateReadOnlyList AllChildren3 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count + 3, $"New count: {AllChildren3.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableRemove()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IWriteableListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IWriteableListInner;
                Assert.That(LeafPathInner != null);

                IWriteableBrowsingListNodeIndex RemovedLeafIndex0 = LeafPathInner.StateList[0].ParentIndex as IWriteableBrowsingListNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex0));

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                WriteableNodeStateReadOnlyList AllChildren0 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Assert.That(Controller.IsRemoveable(LeafPathInner, RemovedLeafIndex0));

                Controller.Remove(LeafPathInner, RemovedLeafIndex0);
                Assert.That(!Controller.Contains(RemovedLeafIndex0));

                Assert.That(LeafPathInner.Count == PathCount - 1);
                Assert.That(LeafPathInner.StateList.Count == PathCount - 1);

                WriteableNodeStateReadOnlyList AllChildren1 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count - 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                RemovedLeafIndex0 = LeafPathInner.StateList[0].ParentIndex as IWriteableBrowsingListNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex0));

                Assert.That(LeafPathInner.Count == 1);

                Assert.That(Controller.IsRemoveable(LeafPathInner, RemovedLeafIndex0));

                IDictionary<Type, string[]> NeverEmptyCollectionTable = BaseNodeHelper.NodeHelper.NeverEmptyCollectionTable as IDictionary<Type, string[]>;
                NeverEmptyCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafPath) });
                Assert.That(!Controller.IsRemoveable(LeafPathInner, RemovedLeafIndex0));



                IWriteableBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IWriteableBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IWriteableBrowsingExistingBlockNodeIndex RemovedLeafIndex1 = LeafBlocksInner.BlockStateList[1].StateList[0].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex1));

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[1].StateList.Count;
                Assert.That(BlockNodeCount == 4, $"New count: {BlockNodeCount}");

                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex1));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex1);
                Assert.That(!Controller.Contains(RemovedLeafIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount - 1);
                Assert.That(LeafBlocksInner.BlockStateList[1].StateList.Count == NodeCount - 1);

                WriteableNodeStateReadOnlyList AllChildren2 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count - 1, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                IWriteableBrowsingExistingBlockNodeIndex RemovedLeafIndex2 = LeafBlocksInner.BlockStateList[1].StateList[0].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex2));


                int BlockCount = LeafBlocksInner.BlockStateList.Count;
                Assert.That(BlockCount == 3);

                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex2));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex2);
                Assert.That(!Controller.Contains(RemovedLeafIndex2));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount - 2);
                Assert.That(LeafBlocksInner.BlockStateList.Count == BlockCount - 1);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == 1, $"Count: {LeafBlocksInner.BlockStateList[0].StateList.Count}");

                WriteableNodeStateReadOnlyList AllChildren3 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count - 3, $"New count: {AllChildren3.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));


                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();


                NeverEmptyCollectionTable.Remove(Type.FromTypeof<Main>());
                Assert.That(Controller.IsRemoveable(LeafPathInner, RemovedLeafIndex0));

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableRemoveBlockRange()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                WriteableNodeStateReadOnlyList AllChildren0 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                IWriteableBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IWriteableBlockListInner;
                Assert.That(LeafBlocksInner != null);
                Assert.That(LeafBlocksInner.BlockStateList.Count == 3, $"New count: {LeafBlocksInner.BlockStateList.Count}");
                Assert.That(Controller.IsBlockRangeRemoveable(LeafBlocksInner, 0, 2));

                Controller.RemoveBlockRange(LeafBlocksInner, 0, 2);
                Assert.That(LeafBlocksInner.Count == 1);

                WriteableNodeStateReadOnlyList AllChildren2 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren0.Count - 7, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                IDictionary<Type, string[]> NeverEmptyCollectionTable = BaseNodeHelper.NodeHelper.NeverEmptyCollectionTable as IDictionary<Type, string[]>;
                NeverEmptyCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafBlocks) });

                Assert.That(!Controller.IsBlockRangeRemoveable(LeafBlocksInner, 0, 3));

                NeverEmptyCollectionTable.Remove(Type.FromTypeof<Main>());
                Assert.That(Controller.IsBlockRangeRemoveable(LeafBlocksInner, 0, 3));

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableRemoveNodeRange()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                WriteableNodeStateReadOnlyList AllChildren0 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");


                //System.Diagnostics.Debug.Assert(false);
                IWriteableListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IWriteableListInner;
                Assert.That(LeafPathInner != null);
                Assert.That(LeafPathInner.StateList.Count == 2, $"New count: {LeafPathInner.StateList.Count}");
                Assert.That(Controller.IsNodeRangeRemoveable(LeafPathInner, -1, 0, 2));

                Controller.RemoveNodeRange(LeafPathInner, -1, 0, 2);

                WriteableNodeStateReadOnlyList AllChildren1 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count - 2, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();


                IWriteableBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IWriteableBlockListInner;
                Assert.That(LeafBlocksInner != null);
                Assert.That(LeafBlocksInner.BlockStateList.Count == 3, $"New count: {LeafBlocksInner.BlockStateList.Count}");
                Assert.That(Controller.IsNodeRangeRemoveable(LeafBlocksInner, 1, 0, 2));

                Controller.RemoveNodeRange(LeafBlocksInner, 1, 0, 2);

                WriteableNodeStateReadOnlyList AllChildren2 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count - 2, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                IDictionary<Type, string[]> NeverEmptyCollectionTable = BaseNodeHelper.NodeHelper.NeverEmptyCollectionTable as IDictionary<Type, string[]>;
                NeverEmptyCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafBlocks) });

                Controller.RemoveBlockRange(LeafBlocksInner, 2, 3);
                Controller.RemoveBlockRange(LeafBlocksInner, 0, 1);
                Assert.That(!Controller.IsNodeRangeRemoveable(LeafBlocksInner, 0, 0, 2));

                NeverEmptyCollectionTable.Remove(Type.FromTypeof<Main>());
                Assert.That(Controller.IsNodeRangeRemoveable(LeafBlocksInner, 0, 0, 2));

                Controller.Undo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableMove()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IWriteableListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IWriteableListInner;
                Assert.That(LeafPathInner != null);

                IWriteableBrowsingListNodeIndex MovedLeafIndex0 = LeafPathInner.IndexAt(0) as IWriteableBrowsingListNodeIndex;
                Assert.That(Controller.Contains(MovedLeafIndex0));

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                WriteableNodeStateReadOnlyList AllChildren0 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Assert.That(Controller.IsMoveable(LeafPathInner, MovedLeafIndex0, +1));

                Controller.Move(LeafPathInner, MovedLeafIndex0, +1);
                Assert.That(Controller.Contains(MovedLeafIndex0));

                Assert.That(LeafPathInner.Count == PathCount);
                Assert.That(LeafPathInner.StateList.Count == PathCount);

                //System.Diagnostics.Debug.Assert(false);
                IWriteableBrowsingListNodeIndex NewLeafIndex0 = LeafPathInner.IndexAt(1) as IWriteableBrowsingListNodeIndex;
                Assert.That(NewLeafIndex0 == MovedLeafIndex0);

                WriteableNodeStateReadOnlyList AllChildren1 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));




                IWriteableBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IWriteableBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IWriteableBrowsingExistingBlockNodeIndex MovedLeafIndex1 = LeafBlocksInner.IndexAt(1, 1) as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(MovedLeafIndex1));

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[1].StateList.Count;
                Assert.That(BlockNodeCount == 4, $"New count: {BlockNodeCount}");

                Assert.That(Controller.IsMoveable(LeafBlocksInner, MovedLeafIndex1, -1));
                Controller.Move(LeafBlocksInner, MovedLeafIndex1, -1);
                Assert.That(Controller.Contains(MovedLeafIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount);
                Assert.That(LeafBlocksInner.BlockStateList[1].StateList.Count == NodeCount);

                IWriteableBrowsingExistingBlockNodeIndex NewLeafIndex1 = LeafBlocksInner.IndexAt(1, 0) as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(NewLeafIndex1 == MovedLeafIndex1);

                WriteableNodeStateReadOnlyList AllChildren2 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableMoveBlock()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                WriteableNodeStateReadOnlyList AllChildren1 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == 19, $"New count: {AllChildren1.Count}");

                IWriteableBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IWriteableBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IWriteableBrowsingExistingBlockNodeIndex MovedLeafIndex1 = LeafBlocksInner.IndexAt(1, 0) as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(MovedLeafIndex1));

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[1].StateList.Count;
                Assert.That(BlockNodeCount == 4, $"New count: {BlockNodeCount}");

                Assert.That(Controller.IsBlockMoveable(LeafBlocksInner, 1, -1));
                Controller.MoveBlock(LeafBlocksInner, 1, -1);
                Assert.That(Controller.Contains(MovedLeafIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == NodeCount);

                IWriteableBrowsingExistingBlockNodeIndex NewLeafIndex1 = LeafBlocksInner.IndexAt(0, 0) as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(NewLeafIndex1 == MovedLeafIndex1);

                WriteableNodeStateReadOnlyList AllChildren2 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableChangeDiscreteValue()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetEnumValue(RootState.Node, nameof(Main.ValueEnum)) == (int)BaseNode.CopySemantic.Value);

                Controller.ChangeDiscreteValue(RootIndex, nameof(Main.ValueEnum), (int)BaseNode.CopySemantic.Reference);

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetEnumValue(RootNode, nameof(Main.ValueEnum)) == (int)BaseNode.CopySemantic.Reference);

                IWriteablePlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IWriteablePlaceholderInner;
                IWriteablePlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as IWriteablePlaceholderNodeState;

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetEnumValue(PlaceholderTreeState.Node, nameof(Tree.ValueEnum)) == (int)BaseNode.CopySemantic.Value);

                Controller.ChangeDiscreteValue(PlaceholderTreeState.ParentIndex, nameof(Tree.ValueEnum), (int)BaseNode.CopySemantic.Any);

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetEnumValue(PlaceholderTreeState.Node, nameof(Tree.ValueEnum)) == (int)BaseNode.CopySemantic.Any);

                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableChangeText()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetString(RootState.Node, nameof(Main.ValueString)) == "s");

                Controller.ChangeText(RootIndex, nameof(Main.ValueString), "test");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetString(RootNode, nameof(Main.ValueString)) == "test");

                IWriteablePlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IWriteablePlaceholderInner;
                IWriteablePlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as IWriteablePlaceholderNodeState;

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableChangeComment()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetCommentText(RootState.Node) == "main doc");

                Controller.ChangeComment(RootIndex, "test");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetCommentText(RootNode) == "test");

                IWriteablePlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IWriteablePlaceholderInner;
                IWriteablePlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as IWriteablePlaceholderNodeState;

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableReplace()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Leaf NewItem0 = CreateLeaf(Guid.NewGuid());
                IWriteableInsertionListNodeIndex ReplacementIndex0 = new WriteableInsertionListNodeIndex(RootNode, nameof(Main.LeafPath), NewItem0, 0);

                IWriteableListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IWriteableListInner;
                Assert.That(LeafPathInner != null);

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                WriteableNodeStateReadOnlyList AllChildren0 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Replace(LeafPathInner, ReplacementIndex0, out IWriteableBrowsingChildIndex NewItemIndex0);
                Assert.That(Controller.Contains(NewItemIndex0));

                Assert.That(LeafPathInner.Count == PathCount);
                Assert.That(LeafPathInner.StateList.Count == PathCount);

                IWriteablePlaceholderNodeState NewItemState0 = (IWriteablePlaceholderNodeState)LeafPathInner.StateList[0];
                Assert.That(NewItemState0.Node == NewItem0);
                Assert.That(NewItemState0.ParentIndex == NewItemIndex0);

                WriteableNodeStateReadOnlyList AllChildren1 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                Leaf NewItem1 = CreateLeaf(Guid.NewGuid());
                IWriteableInsertionExistingBlockNodeIndex ReplacementIndex1 = new WriteableInsertionExistingBlockNodeIndex(RootNode, nameof(Main.LeafBlocks), NewItem1, 0, 0);

                IWriteableBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IWriteableBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IWriteableBlockState BlockState = (IWriteableBlockState)LeafBlocksInner.BlockStateList[0];

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = BlockState.StateList.Count;
                Assert.That(BlockNodeCount == 4);

                Controller.Replace(LeafBlocksInner, ReplacementIndex1, out IWriteableBrowsingChildIndex NewItemIndex1);
                Assert.That(Controller.Contains(NewItemIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount);
                Assert.That(BlockState.StateList.Count == NodeCount);

                IWriteablePlaceholderNodeState NewItemState1 = (IWriteablePlaceholderNodeState)BlockState.StateList[0];
                Assert.That(NewItemState1.Node == NewItem1);
                Assert.That(NewItemState1.ParentIndex == NewItemIndex1);

                WriteableNodeStateReadOnlyList AllChildren2 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                IWriteablePlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IWriteablePlaceholderInner;
                Assert.That(PlaceholderTreeInner != null);

                IWriteableBrowsingPlaceholderNodeIndex ExistingIndex2 = PlaceholderTreeInner.ChildState.ParentIndex as IWriteableBrowsingPlaceholderNodeIndex;

                Tree NewItem2 = CreateTree();
                IWriteableInsertionPlaceholderNodeIndex ReplacementIndex2;
                ReplacementIndex2 = ExistingIndex2.ToInsertionIndex(RootNode, NewItem2) as IWriteableInsertionPlaceholderNodeIndex;

                Controller.Replace(PlaceholderTreeInner, ReplacementIndex2, out IWriteableBrowsingChildIndex NewItemIndex2);
                Assert.That(Controller.Contains(NewItemIndex2));

                IWriteablePlaceholderNodeState NewItemState2 = PlaceholderTreeInner.ChildState as IWriteablePlaceholderNodeState;
                Assert.That(NewItemState2.Node == NewItem2);
                Assert.That(NewItemState2.ParentIndex == NewItemIndex2);

                IWriteableBrowsingPlaceholderNodeIndex DuplicateExistingIndex2 = ReplacementIndex2.ToBrowsingIndex() as IWriteableBrowsingPlaceholderNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex2 as IWriteableBrowsingPlaceholderNodeIndex, DuplicateExistingIndex2));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex2, NewItemIndex2 as IWriteableBrowsingPlaceholderNodeIndex));

                WriteableNodeStateReadOnlyList AllChildren3 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count, $"New count: {AllChildren3.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                IWriteablePlaceholderInner PlaceholderLeafInner = NewItemState2.PropertyToInner(nameof(Tree.Placeholder)) as IWriteablePlaceholderInner;
                Assert.That(PlaceholderLeafInner != null);

                IWriteableBrowsingPlaceholderNodeIndex ExistingIndex3 = PlaceholderLeafInner.ChildState.ParentIndex as IWriteableBrowsingPlaceholderNodeIndex;

                Leaf NewItem3 = CreateLeaf(Guid.NewGuid());
                IWriteableInsertionPlaceholderNodeIndex ReplacementIndex3;
                ReplacementIndex3 = ExistingIndex3.ToInsertionIndex(NewItem2, NewItem3) as IWriteableInsertionPlaceholderNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(ReplacementIndex3, ReplacementIndex3));

                Controller.Replace(PlaceholderLeafInner, ReplacementIndex3, out IWriteableBrowsingChildIndex NewItemIndex3);
                Assert.That(Controller.Contains(NewItemIndex3));

                IWriteablePlaceholderNodeState NewItemState3 = PlaceholderLeafInner.ChildState as IWriteablePlaceholderNodeState;
                Assert.That(NewItemState3.Node == NewItem3);
                Assert.That(NewItemState3.ParentIndex == NewItemIndex3);

                WriteableNodeStateReadOnlyList AllChildren4 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren4.Count == AllChildren3.Count, $"New count: {AllChildren4.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));




                IWriteableOptionalInner OptionalLeafInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IWriteableOptionalInner;
                Assert.That(OptionalLeafInner != null);

                IWriteableBrowsingOptionalNodeIndex ExistingIndex4 = OptionalLeafInner.ChildState.ParentIndex as IWriteableBrowsingOptionalNodeIndex;

                Leaf NewItem4 = CreateLeaf(Guid.NewGuid());
                IWriteableInsertionOptionalNodeIndex ReplacementIndex4;
                ReplacementIndex4 = ExistingIndex4.ToInsertionIndex(RootNode, NewItem4) as IWriteableInsertionOptionalNodeIndex;
                Assert.That(ReplacementIndex4.ParentNode == RootNode);
                Assert.That(ReplacementIndex4.PropertyName == OptionalLeafInner.PropertyName);
                Assert.That(CompareEqual.CoverIsEqual(ReplacementIndex4, ReplacementIndex4));

                Controller.Replace(OptionalLeafInner, ReplacementIndex4, out IWriteableBrowsingChildIndex NewItemIndex4);
                Assert.That(Controller.Contains(NewItemIndex4));

                Assert.That(OptionalLeafInner.IsAssigned);
                IWriteableOptionalNodeState NewItemState4 = OptionalLeafInner.ChildState as IWriteableOptionalNodeState;
                Assert.That(NewItemState4.Node == NewItem4);
                Assert.That(NewItemState4.ParentIndex == NewItemIndex4);

                IWriteableBrowsingOptionalNodeIndex DuplicateExistingIndex4 = ReplacementIndex4.ToBrowsingIndex() as IWriteableBrowsingOptionalNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex4 as IWriteableBrowsingOptionalNodeIndex, DuplicateExistingIndex4));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex4, NewItemIndex4 as IWriteableBrowsingOptionalNodeIndex));

                WriteableNodeStateReadOnlyList AllChildren5 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren5.Count == AllChildren4.Count, $"New count: {AllChildren5.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                IWriteableBrowsingOptionalNodeIndex ExistingIndex5 = OptionalLeafInner.ChildState.ParentIndex as IWriteableBrowsingOptionalNodeIndex;

                Leaf NewItem5 = CreateLeaf(Guid.NewGuid());
                IWriteableInsertionOptionalClearIndex ReplacementIndex5;
                ReplacementIndex5 = ExistingIndex5.ToInsertionIndex(RootNode, null) as IWriteableInsertionOptionalClearIndex;
                Assert.That(ReplacementIndex5.ParentNode == RootNode);
                Assert.That(ReplacementIndex5.PropertyName == OptionalLeafInner.PropertyName);
                Assert.That(CompareEqual.CoverIsEqual(ReplacementIndex5, ReplacementIndex5));

                Controller.Replace(OptionalLeafInner, ReplacementIndex5, out IWriteableBrowsingChildIndex NewItemIndex5);
                Assert.That(Controller.Contains(NewItemIndex5));

                Assert.That(!OptionalLeafInner.IsAssigned);
                IWriteableOptionalNodeState NewItemState5 = OptionalLeafInner.ChildState as IWriteableOptionalNodeState;
                Assert.That(NewItemState5.ParentIndex == NewItemIndex5);

                IWriteableBrowsingOptionalNodeIndex DuplicateExistingIndex5 = ReplacementIndex5.ToBrowsingIndex() as IWriteableBrowsingOptionalNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex5 as IWriteableBrowsingOptionalNodeIndex, DuplicateExistingIndex5));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex5, NewItemIndex5 as IWriteableBrowsingOptionalNodeIndex));

                WriteableNodeStateReadOnlyList AllChildren6 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren6.Count == AllChildren5.Count - 1, $"New count: {AllChildren6.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableAssign()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)EaslyController.Writeable.WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IWriteableOptionalInner UnassignedOptionalLeafInner = RootState.PropertyToInner(nameof(Main.UnassignedOptionalLeaf)) as IWriteableOptionalInner;
                Assert.That(UnassignedOptionalLeafInner != null);
                Assert.That(!UnassignedOptionalLeafInner.IsAssigned);

                IWriteableBrowsingOptionalNodeIndex AssignmentIndex0 = UnassignedOptionalLeafInner.ChildState.ParentIndex;
                Assert.That(AssignmentIndex0 != null);

                WriteableNodeStateReadOnlyList AllChildren0 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Assign(AssignmentIndex0, out bool IsChanged);
                Assert.That(IsChanged);
                Assert.That(UnassignedOptionalLeafInner.IsAssigned);

                WriteableNodeStateReadOnlyList AllChildren1 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Assign(AssignmentIndex0, out IsChanged);
                Assert.That(!IsChanged);
                Assert.That(UnassignedOptionalLeafInner.IsAssigned);

                WriteableNodeStateReadOnlyList AllChildren2 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Controller.Unassign(AssignmentIndex0, out IsChanged);
                Assert.That(IsChanged);
                Assert.That(!UnassignedOptionalLeafInner.IsAssigned);

                WriteableNodeStateReadOnlyList AllChildren3 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count - 1, $"New count: {AllChildren3.Count}");

                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableUnassign()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IWriteableOptionalInner AssignedOptionalLeafInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IWriteableOptionalInner;
                Assert.That(AssignedOptionalLeafInner != null);
                Assert.That(AssignedOptionalLeafInner.IsAssigned);

                IWriteableBrowsingOptionalNodeIndex AssignmentIndex0 = AssignedOptionalLeafInner.ChildState.ParentIndex;
                Assert.That(AssignmentIndex0 != null);

                WriteableNodeStateReadOnlyList AllChildren0 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Unassign(AssignmentIndex0, out bool IsChanged);
                Assert.That(IsChanged);
                Assert.That(!AssignedOptionalLeafInner.IsAssigned);

                WriteableNodeStateReadOnlyList AllChildren1 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count - 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Unassign(AssignmentIndex0, out IsChanged);
                Assert.That(!IsChanged);
                Assert.That(!AssignedOptionalLeafInner.IsAssigned);

                WriteableNodeStateReadOnlyList AllChildren2 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Controller.Assign(AssignmentIndex0, out IsChanged);
                Assert.That(IsChanged);
                Assert.That(AssignedOptionalLeafInner.IsAssigned);

                WriteableNodeStateReadOnlyList AllChildren3 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count + 1, $"New count: {AllChildren3.Count}");

                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableChangeReplication()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IWriteableBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IWriteableBlockListInner;
                Assert.That(LeafBlocksInner != null);

                WriteableNodeStateReadOnlyList AllChildren0 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                IWriteableBlockState BlockState = (IWriteableBlockState)LeafBlocksInner.BlockStateList[0];
                Assert.That(BlockState != null);
                Assert.That(BlockState.ParentInner == LeafBlocksInner);
                BaseNode.IBlock ChildBlock = BlockState.ChildBlock;
                Assert.That(ChildBlock.Replication == BaseNode.ReplicationStatus.Normal);

                Controller.ChangeReplication(LeafBlocksInner, 0, BaseNode.ReplicationStatus.Replicated);

                Assert.That(ChildBlock.Replication == BaseNode.ReplicationStatus.Replicated);

                WriteableNodeStateReadOnlyList AllChildren1 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableSplit()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IWriteableBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IWriteableBlockListInner;
                Assert.That(LeafBlocksInner != null);

                WriteableNodeStateReadOnlyList AllChildren0 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                IWriteableBlockState BlockState0 = (IWriteableBlockState)LeafBlocksInner.BlockStateList[0];
                Assert.That(BlockState0 != null);
                BaseNode.IBlock ChildBlock0 = BlockState0.ChildBlock;
                Assert.That(ChildBlock0.NodeList.Count == 1);

                IWriteableBlockState BlockState1 = (IWriteableBlockState)LeafBlocksInner.BlockStateList[1];
                Assert.That(BlockState1 != null);
                BaseNode.IBlock ChildBlock1 = BlockState1.ChildBlock;
                Assert.That(ChildBlock1.NodeList.Count == 2);

                Assert.That(LeafBlocksInner.Count == 4);
                Assert.That(LeafBlocksInner.BlockStateList.Count == 3);

                IWriteableBrowsingExistingBlockNodeIndex SplitIndex0 = LeafBlocksInner.IndexAt(1, 1) as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.IsSplittable(LeafBlocksInner, SplitIndex0));

                Controller.SplitBlock(LeafBlocksInner, SplitIndex0);

                Assert.That(LeafBlocksInner.BlockStateList.Count == 4);
                Assert.That(ChildBlock0 == LeafBlocksInner.BlockStateList[0].ChildBlock);
                Assert.That(ChildBlock1 == LeafBlocksInner.BlockStateList[2].ChildBlock);
                Assert.That(ChildBlock1.NodeList.Count == 1);

                IWriteableBlockState BlockState12 = (IWriteableBlockState)LeafBlocksInner.BlockStateList[1];
                Assert.That(BlockState12.ChildBlock.NodeList.Count == 1);

                WriteableNodeStateReadOnlyList AllChildren1 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 2, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableMerge()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IWriteableBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IWriteableBlockListInner;
                Assert.That(LeafBlocksInner != null);

                WriteableNodeStateReadOnlyList AllChildren0 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                IWriteableBlockState BlockState0 = (IWriteableBlockState)LeafBlocksInner.BlockStateList[0];
                Assert.That(BlockState0 != null);
                BaseNode.IBlock ChildBlock0 = BlockState0.ChildBlock;
                Assert.That(ChildBlock0.NodeList.Count == 1);

                IWriteableBlockState BlockState1 = (IWriteableBlockState)LeafBlocksInner.BlockStateList[1];
                Assert.That(BlockState1 != null);
                BaseNode.IBlock ChildBlock1 = BlockState1.ChildBlock;
                Assert.That(ChildBlock1.NodeList.Count == 2);

                Assert.That(LeafBlocksInner.Count == 4);

                IWriteableBrowsingExistingBlockNodeIndex MergeIndex0 = LeafBlocksInner.IndexAt(1, 0) as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.IsMergeable(LeafBlocksInner, MergeIndex0));

                Assert.That(LeafBlocksInner.BlockStateList.Count == 3);

                Controller.MergeBlocks(LeafBlocksInner, MergeIndex0);

                Assert.That(LeafBlocksInner.BlockStateList.Count == 2);
                Assert.That(ChildBlock1 == LeafBlocksInner.BlockStateList[0].ChildBlock);
                Assert.That(ChildBlock1.NodeList.Count == 3);

                Assert.That(LeafBlocksInner.BlockStateList[0] == BlockState1);

                WriteableNodeStateReadOnlyList AllChildren1 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count - 2, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableExpand()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                WriteableNodeStateReadOnlyList AllChildren0 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Expand(RootIndex, out bool IsChanged);
                Assert.That(IsChanged);

                WriteableNodeStateReadOnlyList AllChildren1 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 2, $"New count: {AllChildren1.Count - AllChildren0.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(!IsChanged);

                WriteableNodeStateReadOnlyList AllChildren2 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                IWriteableOptionalInner OptionalLeafInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IWriteableOptionalInner;
                Assert.That(OptionalLeafInner != null);

                IWriteableInsertionOptionalClearIndex ReplacementIndex5 = new WriteableInsertionOptionalClearIndex(RootNode, nameof(Main.AssignedOptionalLeaf));

                Controller.Replace(OptionalLeafInner, ReplacementIndex5, out IWriteableBrowsingChildIndex NewItemIndex5);
                Assert.That(Controller.Contains(NewItemIndex5));

                WriteableNodeStateReadOnlyList AllChildren3 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count - 1, $"New count: {AllChildren3.Count - AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                WriteableNodeStateReadOnlyList AllChildren4 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren4.Count == AllChildren3.Count + 1, $"New count: {AllChildren4.Count}");



                IWriteableBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IWriteableBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IWriteableBrowsingExistingBlockNodeIndex RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                WriteableNodeStateReadOnlyList AllChildren5 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren5.Count == AllChildren4.Count - 10, $"New count: {AllChildren5.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(LeafBlocksInner.IsEmpty);

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(!IsChanged);

                WriteableNodeStateReadOnlyList AllChildren6 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren6.Count == AllChildren5.Count, $"New count: {AllChildren6.Count - AllChildren5.Count}");

                IDictionary<Type, string[]> WithExpandCollectionTable = BaseNodeHelper.NodeHelper.WithExpandCollectionTable as IDictionary<Type, string[]>;
                WithExpandCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafBlocks) });

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                WriteableNodeStateReadOnlyList AllChildren7 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren7.Count == AllChildren6.Count + 3, $"New count: {AllChildren7.Count - AllChildren6.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(!LeafBlocksInner.IsEmpty);
                Assert.That(LeafBlocksInner.IsSingle);

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                WithExpandCollectionTable.Remove(Type.FromTypeof<Main>());

                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableReduce()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;
            bool IsChanged;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IWriteableBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IWriteableBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IWriteableBrowsingExistingBlockNodeIndex RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(LeafBlocksInner.IsEmpty);

                WriteableNodeStateReadOnlyList AllChildren0 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 9, $"New count: {AllChildren0.Count}");

                IDictionary<Type, string[]> WithExpandCollectionTable = BaseNodeHelper.NodeHelper.WithExpandCollectionTable as IDictionary<Type, string[]>;
                WithExpandCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafBlocks) });

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                WriteableNodeStateReadOnlyList AllChildren1 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 5, $"New count: {AllChildren1.Count - AllChildren0.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                //System.Diagnostics.Debug.Assert(false);
                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                WriteableNodeStateReadOnlyList AllChildren2 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count - 4, $"New count: {AllChildren2.Count - AllChildren1.Count}");

                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(!IsChanged);

                WriteableNodeStateReadOnlyList AllChildren3 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count, $"New count: {AllChildren3.Count - AllChildren2.Count}");

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                WriteableNodeStateReadOnlyList AllChildren4 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren4.Count == AllChildren3.Count + 4, $"New count: {AllChildren4.Count - AllChildren3.Count}");

                BaseNode.IBlock ChildBlock = LeafBlocksInner.BlockStateList[0].ChildBlock;
                Leaf FirstNode = ChildBlock.NodeList[0] as Leaf;
                Assert.That(FirstNode != null);
                BaseNodeHelper.NodeTreeHelper.SetString(FirstNode, nameof(Coverage.Leaf.Text), "!");

                //System.Diagnostics.Debug.Assert(false);
                IWriteableOptionalInner LeafOptionalInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IWriteableOptionalInner;
                Assert.That(LeafOptionalInner != null);

                Leaf Leaf = LeafOptionalInner.ChildState.Node as Leaf;
                BaseNodeHelper.NodeTreeHelper.SetStringProperty(Leaf, "Text", "");

                //System.Diagnostics.Debug.Assert(false);
                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                WriteableNodeStateReadOnlyList AllChildren5 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren5.Count == AllChildren4.Count - 2, $"New count: {AllChildren5.Count - AllChildren4.Count}");

                BaseNodeHelper.NodeTreeHelper.SetString(FirstNode, nameof(Leaf.Text), "");

                //System.Diagnostics.Debug.Assert(false);
                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                WriteableNodeStateReadOnlyList AllChildren6 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren6.Count == AllChildren5.Count - 3, $"New count: {AllChildren6.Count - AllChildren5.Count}");

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                WithExpandCollectionTable.Remove(Type.FromTypeof<Main>());

                //System.Diagnostics.Debug.Assert(false);
                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                WriteableNodeStateReadOnlyList AllChildren7 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren7.Count == AllChildren6.Count + 3, $"New count: {AllChildren7.Count - AllChildren6.Count}");

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                WithExpandCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafBlocks) });

                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();

                WithExpandCollectionTable.Remove(Type.FromTypeof<Main>());

                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableCanonicalize()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;
            bool IsChanged;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IWriteableBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IWriteableBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IWriteableBrowsingExistingBlockNodeIndex RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                Assert.That(Controller.CanUndo);
                WriteableOperationGroup LastOperation = Controller.OperationStack[Controller.RedoIndex - 1];
                Assert.That(LastOperation.MainOperation is IWriteableRemoveOperation);
                Assert.That(LastOperation.OperationList.Count > 0);
                Assert.That(LastOperation.Refresh == null);

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(LeafBlocksInner.IsEmpty);

                IWriteableListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IWriteableListInner;
                Assert.That(LeafPathInner != null);
                Assert.That(LeafPathInner.Count == 2);

                IWriteableBrowsingListNodeIndex RemovedListLeafIndex = LeafPathInner.StateList[0].ParentIndex as IWriteableBrowsingListNodeIndex;
                Assert.That(Controller.Contains(RemovedListLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafPathInner, RemovedListLeafIndex));

                Controller.Remove(LeafPathInner, RemovedListLeafIndex);
                Assert.That(!Controller.Contains(RemovedListLeafIndex));

                IDictionary<Type, string[]> NeverEmptyCollectionTable = BaseNodeHelper.NodeHelper.NeverEmptyCollectionTable as IDictionary<Type, string[]>;
                NeverEmptyCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.PlaceholderTree) });

                RemovedListLeafIndex = LeafPathInner.StateList[0].ParentIndex as IWriteableBrowsingListNodeIndex;
                Assert.That(Controller.Contains(RemovedListLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafPathInner, RemovedListLeafIndex));

                Controller.Remove(LeafPathInner, RemovedListLeafIndex);
                Assert.That(!Controller.Contains(RemovedListLeafIndex));
                Assert.That(LeafPathInner.Count == 0);

                NeverEmptyCollectionTable.Remove(Type.FromTypeof<Main>());

                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();

                WriteableNodeStateReadOnlyList AllChildren0 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 12, $"New count: {AllChildren0.Count}");

                IDictionary<Type, string[]> WithExpandCollectionTable = BaseNodeHelper.NodeHelper.WithExpandCollectionTable as IDictionary<Type, string[]>;
                WithExpandCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafBlocks) });

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                WriteableNodeStateReadOnlyList AllChildren1 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 2, $"New count: {AllChildren1.Count - AllChildren0.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                //System.Diagnostics.Debug.Assert(false);
                IWriteableOptionalInner LeafOptionalInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IWriteableOptionalInner;
                Assert.That(LeafOptionalInner != null);

                Leaf Leaf = LeafOptionalInner.ChildState.Node as Leaf;
                BaseNodeHelper.NodeTreeHelper.SetStringProperty(Leaf, "Text", "");


                //System.Diagnostics.Debug.Assert(false);
                Controller.Canonicalize(out IsChanged);
                Assert.That(IsChanged);

                WriteableNodeStateReadOnlyList AllChildren2 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count - 2, $"New count: {AllChildren2.Count - AllChildren1.Count}");

                Controller.Undo();
                Controller.Redo();

                Controller.Canonicalize(out IsChanged);
                Assert.That(!IsChanged);

                WriteableNodeStateReadOnlyList AllChildren3 = (WriteableNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count, $"New count: {AllChildren3.Count - AllChildren2.Count}");

                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();

                NeverEmptyCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafBlocks) });
                Assert.That(LeafBlocksInner.BlockStateList.Count == 1);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == 1, LeafBlocksInner.BlockStateList[0].StateList.Count.ToString());

                Controller.Canonicalize(out IsChanged);
                Assert.That(IsChanged);

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                NeverEmptyCollectionTable.Remove(Type.FromTypeof<Main>());

                WithExpandCollectionTable.Remove(Type.FromTypeof<Main>());

                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteablePrune()
        {
            ControllerTools.ResetExpectedName();

            Main MainItemH = CreateRoot(ValueGuid0, Imperfections.None);
            Main MainItemV = CreateRoot(ValueGuid1, Imperfections.None);
            BaseNode.Document RootDocument = BaseNodeHelper.NodeHelper.CreateSimpleDocument("root doc", Guid.NewGuid());
            Root RootNode = new Root(RootDocument);
            BaseNode.IBlockList<Main> MainBlocksH = BaseNodeHelper.BlockListHelper.CreateSimpleBlockList<Main>(MainItemH);
            BaseNode.IBlockList<Main> MainBlocksV = BaseNodeHelper.BlockListHelper.CreateSimpleBlockList<Main>(MainItemV);

            Main UnassignedOptionalMain = CreateRoot(ValueGuid2, Imperfections.None);
            Easly.IOptionalReference<Main> UnassignedOptional = BaseNodeHelper.OptionalReferenceHelper.CreateReference<Main>(UnassignedOptionalMain);

            IList<Leaf> LeafPathH = new List<Leaf>();
            IList<Leaf> LeafPathV = new List<Leaf>();

            BaseNodeHelper.NodeTreeHelperBlockList.SetBlockList(RootNode, nameof(Root.MainBlocksH), (BaseNode.IBlockList)MainBlocksH);
            BaseNodeHelper.NodeTreeHelperBlockList.SetBlockList(RootNode, nameof(Root.MainBlocksV), (BaseNode.IBlockList)MainBlocksV);
            BaseNodeHelper.NodeTreeHelperOptional.SetOptionalReference(RootNode, nameof(Root.UnassignedOptionalMain), (Easly.IOptionalReference)UnassignedOptional);
            BaseNodeHelper.NodeTreeHelper.SetString(RootNode, nameof(Root.ValueString), "root string");
            BaseNodeHelper.NodeTreeHelperList.SetChildNodeList(RootNode, nameof(Root.LeafPathH), (IList)LeafPathH);
            BaseNodeHelper.NodeTreeHelperList.SetChildNodeList(RootNode, nameof(Root.LeafPathV), (IList)LeafPathV);
            BaseNodeHelper.NodeTreeHelperOptional.SetOptionalReference(RootNode, nameof(Root.UnassignedOptionalLeaf), (Easly.IOptionalReference)BaseNodeHelper.OptionalReferenceHelper.CreateReference(new Leaf()));

            //System.Diagnostics.Debug.Assert(false);
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            using (WriteableControllerView ControllerView0 = WriteableControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IWriteableBlockListInner MainInnerH = RootState.PropertyToInner(nameof(Root.MainBlocksH)) as IWriteableBlockListInner;
                Assert.That(MainInnerH != null);

                IWriteableBrowsingExistingBlockNodeIndex MainIndex = MainInnerH.IndexAt(0, 0) as IWriteableBrowsingExistingBlockNodeIndex;
                Controller.Remove(MainInnerH, MainIndex);

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                MainIndex = MainInnerH.IndexAt(0, 0) as IWriteableBrowsingExistingBlockNodeIndex;
                Controller.Remove(MainInnerH, MainIndex);

                Controller.Undo();
                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void WriteableCollections()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IWriteableRootNodeIndex RootIndex;
            bool IsReadOnly;
            IReadOnlyBlockState FirstBlockState;
            IReadOnlyBrowsingBlockNodeIndex FirstBlockNodeIndex;
            IReadOnlyBrowsingListNodeIndex FirstListNodeIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new WriteableRootNodeIndex(RootNode);

            WriteableController ControllerBase = (WriteableController)WriteableController.Create(RootIndex);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            ReadOnlyNodeStateDictionary ControllerStateTable = DebugObjects.GetReferenceByInterface(Type.FromTypeof<WriteableNodeStateDictionary>()) as ReadOnlyNodeStateDictionary;

            using (WriteableControllerView ControllerView = WriteableControllerView.Create(Controller))
            {
                // IxxxBlockStateViewDictionary 

                ReadOnlyBlockStateViewDictionary ReadOnlyBlockStateViewTable = ControllerView.BlockStateViewTable;

                foreach (KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView> Entry in ReadOnlyBlockStateViewTable)
                {
                    ReadOnlyBlockStateView StateView = ReadOnlyBlockStateViewTable[Entry.Key];
                    ReadOnlyBlockStateViewTable.TryGetValue(Entry.Key, out ReadOnlyBlockStateView Value);
                    ((ICollection<KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView>>)ReadOnlyBlockStateViewTable).Contains(Entry);
                    ReadOnlyBlockStateViewTable.Remove(Entry.Key);
                    ReadOnlyBlockStateViewTable.Add(Entry.Key, Entry.Value);
                    ICollection<IReadOnlyBlockState> Keys = ReadOnlyBlockStateViewTable.Keys;
                    ICollection<ReadOnlyBlockStateView> Values = ReadOnlyBlockStateViewTable.Values;

                    break;
                }

                IDictionary<IReadOnlyBlockState, ReadOnlyBlockStateView> ReadOnlyBlockStateViewTableAsDictionary = ReadOnlyBlockStateViewTable;
                foreach (KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView> Entry in ReadOnlyBlockStateViewTableAsDictionary)
                {
                    ReadOnlyBlockStateView StateView = ReadOnlyBlockStateViewTableAsDictionary[Entry.Key];
                    break;
                }

                ICollection<KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView>> ReadOnlyBlockStateViewTableAsCollection = ReadOnlyBlockStateViewTable;
                IsReadOnly = ReadOnlyBlockStateViewTableAsCollection.IsReadOnly;
                foreach (KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView> Entry in ReadOnlyBlockStateViewTableAsCollection)
                {
                    ReadOnlyBlockStateViewTableAsCollection.Contains(Entry);
                    ReadOnlyBlockStateViewTableAsCollection.Remove(Entry);
                    ReadOnlyBlockStateViewTableAsCollection.Add(Entry);
                    ReadOnlyBlockStateViewTableAsCollection.CopyTo(new KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView>[ReadOnlyBlockStateViewTableAsCollection.Count], 0);
                    break;
                }

                IEnumerable<KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView>> ReadOnlyBlockStateViewTableAsEnumerable = ReadOnlyBlockStateViewTable;
                foreach (KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView> Entry in ReadOnlyBlockStateViewTableAsEnumerable)
                {
                    break;
                }

                // WriteableBlockStateList

                IWriteableNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IWriteableBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IWriteableBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IWriteableListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IWriteableListInner;
                Assert.That(LeafPathInner != null);

                //System.Diagnostics.Debug.Assert(false);
                IWriteablePlaceholderNodeState FirstNodeState = LeafBlocksInner.FirstNodeState;
                WriteableBlockStateList DebugBlockStateList = DebugObjects.GetReferenceByInterface(Type.FromTypeof<WriteableBlockStateList>()) as WriteableBlockStateList;
                if (DebugBlockStateList != null)
                {
                    Assert.That(DebugBlockStateList.Count > 0);
                    IsReadOnly = ((ICollection<IReadOnlyBlockState>)DebugBlockStateList).IsReadOnly;
                    FirstBlockState = DebugBlockStateList[0];
                    Assert.That(DebugBlockStateList.Contains(FirstBlockState));
                    Assert.That(DebugBlockStateList.IndexOf(FirstBlockState) == 0);
                    DebugBlockStateList.Remove(FirstBlockState);
                    DebugBlockStateList.Add(FirstBlockState);
                    DebugBlockStateList.Remove(FirstBlockState);
                    DebugBlockStateList.Insert(0, FirstBlockState);
                    DebugBlockStateList.CopyTo((IReadOnlyBlockState[])(new IWriteableBlockState[DebugBlockStateList.Count]), 0);

                    IEnumerable<IReadOnlyBlockState> BlockStateListAsEnumerable = DebugBlockStateList;
                    foreach (IReadOnlyBlockState Item in BlockStateListAsEnumerable)
                    {
                        break;
                    }

                    IList<IReadOnlyBlockState> BlockStateListAsIlist = DebugBlockStateList;
                    Assert.That(BlockStateListAsIlist[0] == FirstBlockState);

                    IReadOnlyList<IReadOnlyBlockState> BlockStateListAsIReadOnlylist = DebugBlockStateList;
                    Assert.That(BlockStateListAsIReadOnlylist[0] == FirstBlockState);
                }

                WriteableBlockStateReadOnlyList WriteableBlockStateList = LeafBlocksInner.BlockStateList;
                Assert.That(WriteableBlockStateList.Count > 0);
                FirstBlockState = WriteableBlockStateList[0];
                Assert.That(WriteableBlockStateList.Contains(FirstBlockState));
                Assert.That(WriteableBlockStateList.IndexOf(FirstBlockState) == 0);
                Assert.That(WriteableBlockStateList.Contains((IWriteableBlockState)FirstBlockState));
                Assert.That(WriteableBlockStateList.IndexOf((IWriteableBlockState)FirstBlockState) == 0);

                IEnumerable<IWriteableBlockState> WriteableBlockStateListAsIEnumerable = WriteableBlockStateList;
                IEnumerator<IWriteableBlockState> WriteableBlockStateListAsIEnumerableEnumerator = WriteableBlockStateListAsIEnumerable.GetEnumerator();

                // WriteableBrowsingBlockNodeIndexList

                WriteableBrowsingBlockNodeIndexList BlockNodeIndexList = LeafBlocksInner.AllIndexes() as WriteableBrowsingBlockNodeIndexList;
                Assert.That(BlockNodeIndexList.Count > 0);
                IsReadOnly = ((ICollection<IReadOnlyBrowsingBlockNodeIndex>)BlockNodeIndexList).IsReadOnly;
                FirstBlockNodeIndex = BlockNodeIndexList[0];
                Assert.That(BlockNodeIndexList.Contains(FirstBlockNodeIndex));
                Assert.That(BlockNodeIndexList.IndexOf(FirstBlockNodeIndex) == 0);
                BlockNodeIndexList.Remove(FirstBlockNodeIndex);
                BlockNodeIndexList.Add(FirstBlockNodeIndex);
                BlockNodeIndexList.Remove(FirstBlockNodeIndex);
                BlockNodeIndexList.Insert(0, FirstBlockNodeIndex);
                BlockNodeIndexList.CopyTo((IReadOnlyBrowsingBlockNodeIndex[])(new IWriteableBrowsingBlockNodeIndex[BlockNodeIndexList.Count]), 0);

                IEnumerable<IReadOnlyBrowsingBlockNodeIndex> BlockNodeIndexListAsEnumerable = BlockNodeIndexList;
                foreach (IReadOnlyBrowsingBlockNodeIndex Item in BlockNodeIndexListAsEnumerable)
                {
                    break;
                }

                IList<IReadOnlyBrowsingBlockNodeIndex> BlockNodeIndexListAsIlist = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsIlist[0] == FirstBlockNodeIndex);

                IReadOnlyList<IReadOnlyBrowsingBlockNodeIndex> BlockNodeIndexListAsIReadOnlylist = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsIReadOnlylist[0] == FirstBlockNodeIndex);

                ReadOnlyBrowsingBlockNodeIndexList BlockNodeIndexListAsReadOnly = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsReadOnly[0] == FirstBlockNodeIndex);

                // WriteableBrowsingListNodeIndexList

                WriteableBrowsingListNodeIndexList ListNodeIndexList = LeafPathInner.AllIndexes() as WriteableBrowsingListNodeIndexList;
                Assert.That(ListNodeIndexList.Count > 0);
                IsReadOnly = ((ICollection<IReadOnlyBrowsingListNodeIndex>)ListNodeIndexList).IsReadOnly;
                FirstListNodeIndex = ListNodeIndexList[0];
                Assert.That(ListNodeIndexList.Contains(FirstListNodeIndex));
                Assert.That(ListNodeIndexList.IndexOf(FirstListNodeIndex) == 0);
                ListNodeIndexList.Remove(FirstListNodeIndex);
                ListNodeIndexList.Add(FirstListNodeIndex);
                ListNodeIndexList.Remove(FirstListNodeIndex);
                ListNodeIndexList.Insert(0, FirstListNodeIndex);
                ListNodeIndexList.CopyTo((IReadOnlyBrowsingListNodeIndex[])(new IWriteableBrowsingListNodeIndex[ListNodeIndexList.Count]), 0);

                IEnumerable<IReadOnlyBrowsingListNodeIndex> ListNodeIndexListAsEnumerable = ListNodeIndexList;
                foreach (IReadOnlyBrowsingListNodeIndex Item in ListNodeIndexListAsEnumerable)
                {
                    break;
                }

                IList<IReadOnlyBrowsingListNodeIndex> ListNodeIndexListAsIlist = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsIlist[0] == FirstListNodeIndex);

                IReadOnlyList<IReadOnlyBrowsingListNodeIndex> ListNodeIndexListAsIReadOnlylist = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsIReadOnlylist[0] == FirstListNodeIndex);

                ReadOnlyBrowsingListNodeIndexList ListNodeIndexListAsReadOnly = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsReadOnly[0] == FirstListNodeIndex);

                // WriteableNodeStateDictionary
                if (ControllerStateTable != null)
                {
                    foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in ControllerStateTable)
                    {
                        IReadOnlyNodeState StateView = ControllerStateTable[Entry.Key];
                        ControllerStateTable.TryGetValue(Entry.Key, out IReadOnlyNodeState Value);
                        ((ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>>)ControllerStateTable).Contains(Entry);
                        ControllerStateTable.Remove(Entry.Key);
                        ControllerStateTable.Add(Entry.Key, Entry.Value);
                        ICollection<IReadOnlyIndex> Keys = ControllerStateTable.Keys;
                        ICollection<IReadOnlyNodeState> Values = ControllerStateTable.Values;

                        break;
                    }

                    IDictionary<IReadOnlyIndex, IReadOnlyNodeState> ControllerStateTableAsDictionary = ControllerStateTable;
                    foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in ControllerStateTableAsDictionary)
                    {
                        IReadOnlyNodeState StateView = ControllerStateTableAsDictionary[Entry.Key];
                        Assert.That(ControllerStateTableAsDictionary.ContainsKey(Entry.Key));
                        break;
                    }

                    ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> ControllerStateTableAsCollection = ControllerStateTable;
                    IsReadOnly = ControllerStateTableAsCollection.IsReadOnly;
                    foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in ControllerStateTableAsCollection)
                    {
                        ControllerStateTableAsCollection.Contains(Entry);
                        ControllerStateTableAsCollection.Remove(Entry);
                        ControllerStateTableAsCollection.Add(Entry);
                        ControllerStateTableAsCollection.CopyTo(new KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>[ControllerStateTableAsCollection.Count], 0);
                        break;
                    }

                    IEnumerable<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> ControllerStateTableAsEnumerable = ControllerStateTable;
                    foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in ControllerStateTableAsEnumerable)
                    {
                        break;
                    }
                }

                // IWriteableIndexNodeStateReadOnlyDictionary

                ReadOnlyNodeStateReadOnlyDictionary StateTable = Controller.StateTable;
                IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState> StateTableAsDictionary = StateTable;
                Assert.That(StateTable.TryGetValue(RootIndex, out IReadOnlyNodeState RootStateValue) == StateTableAsDictionary.TryGetValue(RootIndex, out IReadOnlyNodeState RootStateValueFromDictionary) && RootStateValue == RootStateValueFromDictionary);
                Assert.That(StateTableAsDictionary.Keys != null);
                Assert.That(StateTableAsDictionary.Values != null);

                // WriteableInnerDictionary

                //System.Diagnostics.Debug.Assert(false);
                WriteableInnerDictionary<string> InnerTableModify = DebugObjects.GetReferenceByInterface(Type.FromTypeof<WriteableInnerDictionary<string>>()) as WriteableInnerDictionary<string>;
                Assert.That(InnerTableModify != null);
                Assert.That(InnerTableModify.Count > 0);

                IDictionary<string, IReadOnlyInner> InnerTableModifyAsDictionary = InnerTableModify;
                Assert.That(InnerTableModifyAsDictionary.Keys != null);
                Assert.That(InnerTableModifyAsDictionary.Values != null);

                foreach (string Key in InnerTableModify.Keys)
                {
                    IWriteableInner Value = (IWriteableInner)InnerTableModify[Key];
                    Assert.That(InnerTableModifyAsDictionary.ContainsKey(Key));
                    Assert.That(InnerTableModifyAsDictionary[Key] == Value);
                }

                ICollection<KeyValuePair<string, IReadOnlyInner>> InnerTableModifyAsCollection = InnerTableModify;
                Assert.That(!InnerTableModifyAsCollection.IsReadOnly);

                IEnumerable<KeyValuePair<string, IReadOnlyInner>> InnerTableModifyAsEnumerable = InnerTableModify;
                IEnumerator<KeyValuePair<string, IReadOnlyInner>> InnerTableModifyAsEnumerableEnumerator = InnerTableModifyAsEnumerable.GetEnumerator();

                foreach (KeyValuePair<string, IReadOnlyInner> Entry in InnerTableModifyAsEnumerable)
                {
                    Assert.That(InnerTableModifyAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(InnerTableModifyAsDictionary[Entry.Key] == Entry.Value);
                    Assert.That(InnerTableModify.TryGetValue(Entry.Key, out IReadOnlyInner ReadOnlyInnerValue) == InnerTableModify.TryGetValue(Entry.Key, out IReadOnlyInner WriteableInnerValue));

                    Assert.That(InnerTableModifyAsCollection.Contains(Entry));
                    InnerTableModifyAsCollection.Remove(Entry);
                    InnerTableModifyAsCollection.Add(Entry);
                    InnerTableModifyAsCollection.CopyTo(new KeyValuePair<string, IReadOnlyInner>[InnerTableModify.Count], 0);
                    break;
                }

                // WriteableInnerReadOnlyDictionary

                WriteableInnerReadOnlyDictionary<string> InnerTable = RootState.InnerTable;

                IReadOnlyDictionary<string, IReadOnlyInner> InnerTableAsDictionary = InnerTable;
                Assert.That(InnerTableAsDictionary.Keys != null);
                Assert.That(InnerTableAsDictionary.Values != null);

                foreach (string Key in InnerTable.Keys)
                {
                    IWriteableInner Value = (IWriteableInner)InnerTable[Key];
                    Assert.That(InnerTable.TryGetValue(Key, out IReadOnlyInner ReadOnlyInnerValue) == InnerTable.TryGetValue(Key, out IReadOnlyInner WriteableInnerValue));
                    break;
                }

                // WriteableNodeStateList

                FirstNodeState = LeafPathInner.FirstNodeState;
                Assert.That(FirstNodeState != null);

                WriteableNodeStateList NodeStateListModify = DebugObjects.GetReferenceByInterface(Type.FromTypeof<WriteableNodeStateList>()) as WriteableNodeStateList;
                Assert.That(NodeStateListModify != null);
                Assert.That(NodeStateListModify.Count > 0);
                FirstNodeState = NodeStateListModify[0] as IWriteablePlaceholderNodeState;
                Assert.That(NodeStateListModify.Contains((IReadOnlyNodeState)FirstNodeState));
                Assert.That(NodeStateListModify.IndexOf((IReadOnlyNodeState)FirstNodeState) == 0);

                NodeStateListModify.Remove((IReadOnlyNodeState)FirstNodeState);
                NodeStateListModify.Insert(0, (IReadOnlyNodeState)FirstNodeState);
                NodeStateListModify.CopyTo((IReadOnlyNodeState[])(new IWriteableNodeState[NodeStateListModify.Count]), 0);

                ReadOnlyNodeStateList NodeStateListModifyAsReadOnly = NodeStateListModify as ReadOnlyNodeStateList;
                Assert.That(NodeStateListModifyAsReadOnly != null);
                Assert.That(NodeStateListModifyAsReadOnly[0] == NodeStateListModify[0]);

                IList<IReadOnlyNodeState> NodeStateListModifyAsIList = NodeStateListModify as IList<IReadOnlyNodeState>;
                Assert.That(NodeStateListModifyAsIList != null);
                Assert.That(NodeStateListModifyAsIList[0] == NodeStateListModify[0]);

                IReadOnlyList<IReadOnlyNodeState> NodeStateListModifyAsIReadOnlyList = NodeStateListModify as IReadOnlyList<IReadOnlyNodeState>;
                Assert.That(NodeStateListModifyAsIReadOnlyList != null);
                Assert.That(NodeStateListModifyAsIReadOnlyList[0] == NodeStateListModify[0]);

                ICollection<IReadOnlyNodeState> NodeStateListModifyAsCollection = NodeStateListModify as ICollection<IReadOnlyNodeState>;
                Assert.That(NodeStateListModifyAsCollection != null);
                Assert.That(!NodeStateListModifyAsCollection.IsReadOnly);

                IEnumerable<IReadOnlyNodeState> NodeStateListModifyAsEnumerable = NodeStateListModify as IEnumerable<IReadOnlyNodeState>;
                Assert.That(NodeStateListModifyAsEnumerable != null);
                Assert.That(NodeStateListModifyAsEnumerable.GetEnumerator() != null);

                // WriteableNodeStateReadOnlyList

                WriteableNodeStateReadOnlyList NodeStateList = NodeStateListModify.ToReadOnly() as WriteableNodeStateReadOnlyList;
                Assert.That(NodeStateList != null);
                Assert.That(NodeStateList.Count > 0);
                FirstNodeState = NodeStateList[0] as IWriteablePlaceholderNodeState;
                Assert.That(NodeStateList.Contains((IReadOnlyNodeState)FirstNodeState));
                Assert.That(NodeStateList.IndexOf((IReadOnlyNodeState)FirstNodeState) == 0);

                IReadOnlyList<IReadOnlyNodeState> NodeStateListAsIReadOnlyList = NodeStateList as IReadOnlyList<IReadOnlyNodeState>;
                Assert.That(NodeStateListAsIReadOnlyList[0] == FirstNodeState);

                IEnumerable<IReadOnlyNodeState> NodeStateListAsEnumerable = NodeStateList as IEnumerable<IReadOnlyNodeState>;
                Assert.That(NodeStateListAsEnumerable != null);
                Assert.That(NodeStateListAsEnumerable.GetEnumerator() != null);

                // WriteablePlaceholderNodeStateList

                //System.Diagnostics.Debug.Assert(false);
                FirstNodeState = LeafPathInner.FirstNodeState;
                Assert.That(FirstNodeState != null);

                WriteablePlaceholderNodeStateList PlaceholderNodeStateListModify = DebugObjects.GetReferenceByInterface(Type.FromTypeof<WriteablePlaceholderNodeStateList>()) as WriteablePlaceholderNodeStateList;
                if (PlaceholderNodeStateListModify != null)
                {
                    Assert.That(PlaceholderNodeStateListModify.Count > 0);
                    FirstNodeState = PlaceholderNodeStateListModify[0] as IWriteablePlaceholderNodeState;
                    Assert.That(PlaceholderNodeStateListModify.Contains((IReadOnlyPlaceholderNodeState)FirstNodeState));
                    Assert.That(PlaceholderNodeStateListModify.IndexOf((IReadOnlyPlaceholderNodeState)FirstNodeState) == 0);

                    PlaceholderNodeStateListModify.Remove((IReadOnlyPlaceholderNodeState)FirstNodeState);
                    PlaceholderNodeStateListModify.Add((IReadOnlyPlaceholderNodeState)FirstNodeState);
                    PlaceholderNodeStateListModify.Remove((IReadOnlyPlaceholderNodeState)FirstNodeState);
                    PlaceholderNodeStateListModify.Insert(0, (IReadOnlyPlaceholderNodeState)FirstNodeState);
                    PlaceholderNodeStateListModify.CopyTo((IReadOnlyPlaceholderNodeState[])(new IWriteablePlaceholderNodeState[PlaceholderNodeStateListModify.Count]), 0);

                    ReadOnlyPlaceholderNodeStateList PlaceholderNodeStateListModifyAsReadOnly = PlaceholderNodeStateListModify as ReadOnlyPlaceholderNodeStateList;
                    Assert.That(PlaceholderNodeStateListModifyAsReadOnly != null);
                    Assert.That(PlaceholderNodeStateListModifyAsReadOnly[0] == PlaceholderNodeStateListModify[0]);

                    IList<IReadOnlyPlaceholderNodeState> PlaceholderNodeStateListModifyAsIList = PlaceholderNodeStateListModify as IList<IReadOnlyPlaceholderNodeState>;
                    Assert.That(PlaceholderNodeStateListModifyAsIList != null);
                    Assert.That(PlaceholderNodeStateListModifyAsIList[0] == PlaceholderNodeStateListModify[0]);

                    IReadOnlyList<IReadOnlyPlaceholderNodeState> PlaceholderNodeStateListModifyAsIReadOnlyList = PlaceholderNodeStateListModify as IReadOnlyList<IReadOnlyPlaceholderNodeState>;
                    Assert.That(PlaceholderNodeStateListModifyAsIReadOnlyList != null);
                    Assert.That(PlaceholderNodeStateListModifyAsIReadOnlyList[0] == PlaceholderNodeStateListModify[0]);

                    ICollection<IReadOnlyPlaceholderNodeState> PlaceholderNodeStateListModifyAsCollection = PlaceholderNodeStateListModify as ICollection<IReadOnlyPlaceholderNodeState>;
                    Assert.That(PlaceholderNodeStateListModifyAsCollection != null);
                    Assert.That(!PlaceholderNodeStateListModifyAsCollection.IsReadOnly);

                    IEnumerable<IReadOnlyPlaceholderNodeState> PlaceholderNodeStateListModifyAsEnumerable = PlaceholderNodeStateListModify as IEnumerable<IReadOnlyPlaceholderNodeState>;
                    Assert.That(PlaceholderNodeStateListModifyAsEnumerable != null);
                    Assert.That(PlaceholderNodeStateListModifyAsEnumerable.GetEnumerator() != null);
                }

                // WriteablePlaceholderNodeStateReadOnlyList

                WriteablePlaceholderNodeStateReadOnlyList PlaceholderNodeStateList = LeafPathInner.StateList;
                Assert.That(PlaceholderNodeStateList != null);
                Assert.That(PlaceholderNodeStateList.Count > 0);
                FirstNodeState = PlaceholderNodeStateList[0] as IWriteablePlaceholderNodeState;
                Assert.That(PlaceholderNodeStateList.Contains((IReadOnlyPlaceholderNodeState)FirstNodeState));
                Assert.That(PlaceholderNodeStateList.IndexOf((IReadOnlyPlaceholderNodeState)FirstNodeState) == 0);

                IReadOnlyList<IReadOnlyPlaceholderNodeState> PlaceholderNodeStateListAsIReadOnlyList = PlaceholderNodeStateList as IReadOnlyList<IReadOnlyPlaceholderNodeState>;
                Assert.That(PlaceholderNodeStateListAsIReadOnlyList[0] == FirstNodeState);

                IEnumerable<IReadOnlyPlaceholderNodeState> PlaceholderNodeStateListAsEnumerable = PlaceholderNodeStateList as IEnumerable<IReadOnlyPlaceholderNodeState>;
                Assert.That(PlaceholderNodeStateListAsEnumerable != null);
                Assert.That(PlaceholderNodeStateListAsEnumerable.GetEnumerator() != null);

                // WriteableNodeStateViewDictionary
                WriteableNodeStateViewDictionary StateViewTable = ControllerView.StateViewTable;

                IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView> StateViewTableAsDictionary = StateViewTable;
                Assert.That(StateViewTableAsDictionary != null);
                Assert.That(StateViewTableAsDictionary.TryGetValue(RootState, out IReadOnlyNodeStateView StateViewTableAsDictionaryValue) == StateViewTable.TryGetValue(RootState, out IReadOnlyNodeStateView StateViewTableValue));
                Assert.That(StateViewTableAsDictionary.Keys != null);
                Assert.That(StateViewTableAsDictionary.Values != null);

                ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> StateViewTableAsCollection = StateViewTable;
                Assert.That(!StateViewTableAsCollection.IsReadOnly);

                foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in StateViewTableAsCollection)
                {
                    Assert.That(StateViewTableAsCollection.Contains(Entry));
                    StateViewTableAsCollection.Remove(Entry);
                    StateViewTableAsCollection.Add(Entry);
                    StateViewTableAsCollection.CopyTo(new KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>[StateViewTable.Count], 0);
                    break;
                }
            }
        }
    }
}
