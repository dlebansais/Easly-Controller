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
        public static void LayoutCreation()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;
            LayoutController Controller;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

            try
            {
                RootIndex = new LayoutRootNodeIndex(RootNode);
                Controller = LayoutController.Create(RootIndex);
            }
            catch (Exception e)
            {
                Assert.Fail($"#0: {e}");
            }

            RootNode = CreateRoot(ValueGuid0, Imperfections.BadGuid);
            Assert.That(!BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode, throwOnInvalid: false));

            try
            {
                RootIndex = new LayoutRootNodeIndex(RootNode);
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
        public static void LayoutProperties()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex0;
            ILayoutRootNodeIndex RootIndex1;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

            RootIndex0 = new LayoutRootNodeIndex(RootNode);
            Assert.That(RootIndex0.Node == RootNode);
            Assert.That(RootIndex0.IsEqual(CompareEqual.New(), RootIndex0));

            RootIndex1 = new LayoutRootNodeIndex(RootNode);
            Assert.That(RootIndex1.Node == RootNode);
            Assert.That(CompareEqual.CoverIsEqual(RootIndex0, RootIndex1));

            LayoutController Controller0 = LayoutController.Create(RootIndex0);
            Assert.That(Controller0.RootIndex == RootIndex0);

            Stats Stats = Controller0.Stats;
            Assert.That(Stats.NodeCount >= 0);
            Assert.That(Stats.PlaceholderNodeCount >= 0);
            Assert.That(Stats.OptionalNodeCount >= 0);
            Assert.That(Stats.AssignedOptionalNodeCount >= 0);
            Assert.That(Stats.ListCount >= 0);
            Assert.That(Stats.BlockListCount >= 0);
            Assert.That(Stats.BlockCount >= 0);

            ILayoutPlaceholderNodeState RootState = Controller0.RootState;
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

            ILayoutPlaceholderInner MainPlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as ILayoutPlaceholderInner;
            Assert.That(MainPlaceholderTreeInner != null);
            Assert.That(MainPlaceholderTreeInner.InterfaceType.IsTypeof<Tree>());
            Assert.That(MainPlaceholderTreeInner.ChildState != null);
            Assert.That(MainPlaceholderTreeInner.ChildState.ParentInner == MainPlaceholderTreeInner);

            ILayoutPlaceholderInner MainPlaceholderLeafInner = RootState.PropertyToInner(nameof(Main.PlaceholderLeaf)) as ILayoutPlaceholderInner;
            Assert.That(MainPlaceholderLeafInner != null);
            Assert.That(MainPlaceholderLeafInner.InterfaceType.IsTypeof<Leaf>());
            Assert.That(MainPlaceholderLeafInner.ChildState != null);
            Assert.That(MainPlaceholderLeafInner.ChildState.ParentInner == MainPlaceholderLeafInner);

            ILayoutOptionalInner MainUnassignedOptionalInner = RootState.PropertyToInner(nameof(Main.UnassignedOptionalLeaf)) as ILayoutOptionalInner;
            Assert.That(MainUnassignedOptionalInner != null);
            Assert.That(MainUnassignedOptionalInner.InterfaceType.IsTypeof<Leaf>());
            Assert.That(!MainUnassignedOptionalInner.IsAssigned);
            Assert.That(MainUnassignedOptionalInner.ChildState != null);
            Assert.That(MainUnassignedOptionalInner.ChildState.ParentInner == MainUnassignedOptionalInner);

            ILayoutOptionalInner MainAssignedOptionalTreeInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalTree)) as ILayoutOptionalInner;
            Assert.That(MainAssignedOptionalTreeInner != null);
            Assert.That(MainAssignedOptionalTreeInner.InterfaceType.IsTypeof<Tree>());
            Assert.That(MainAssignedOptionalTreeInner.IsAssigned);

            ILayoutNodeState AssignedOptionalTreeState = MainAssignedOptionalTreeInner.ChildState;
            Assert.That(AssignedOptionalTreeState != null);
            Assert.That(AssignedOptionalTreeState.ParentInner == MainAssignedOptionalTreeInner);
            Assert.That(AssignedOptionalTreeState.ParentState == RootState);

            LayoutNodeStateReadOnlyList AssignedOptionalTreeAllChildren = AssignedOptionalTreeState.GetAllChildren() as LayoutNodeStateReadOnlyList;
            Assert.That(AssignedOptionalTreeAllChildren != null);
            Assert.That(AssignedOptionalTreeAllChildren.Count == 2, $"New count: {AssignedOptionalTreeAllChildren.Count}");

            ILayoutOptionalInner MainAssignedOptionalLeafInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as ILayoutOptionalInner;
            Assert.That(MainAssignedOptionalLeafInner != null);
            Assert.That(MainAssignedOptionalLeafInner.InterfaceType.IsTypeof<Leaf>());
            Assert.That(MainAssignedOptionalLeafInner.IsAssigned);
            Assert.That(MainAssignedOptionalLeafInner.ChildState != null);
            Assert.That(MainAssignedOptionalLeafInner.ChildState.ParentInner == MainAssignedOptionalLeafInner);

            ILayoutBlockListInner MainLeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
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

            ILayoutBlockState LeafBlock = (ILayoutBlockState)MainLeafBlocksInner.BlockStateList[0];
            Assert.That(LeafBlock != null);
            Assert.That(LeafBlock.StateList != null);
            Assert.That(LeafBlock.StateList.Count == 1);
            Assert.That(MainLeafBlocksInner.FirstNodeState == LeafBlock.StateList[0]);
            Assert.That(MainLeafBlocksInner.IndexAt(0, 0) == MainLeafBlocksInner.FirstNodeState.ParentIndex);

            ILayoutPlaceholderInner PatternInner = LeafBlock.PropertyToInner(nameof(BaseNode.IBlock.ReplicationPattern)) as ILayoutPlaceholderInner;
            Assert.That(PatternInner != null);

            ILayoutPlaceholderInner SourceInner = LeafBlock.PropertyToInner(nameof(BaseNode.IBlock.SourceIdentifier)) as ILayoutPlaceholderInner;
            Assert.That(SourceInner != null);

            ILayoutPatternState PatternState = LeafBlock.PatternState;
            Assert.That(PatternState != null);
            Assert.That(PatternState.ParentBlockState == LeafBlock);
            Assert.That(PatternState.ParentInner == PatternInner);
            Assert.That(PatternState.ParentIndex == LeafBlock.PatternIndex);
            Assert.That(PatternState.ParentState == RootState);
            Assert.That(PatternState.InnerTable.Count == 0);
            Assert.That(PatternState is ILayoutNodeState AsPlaceholderPatternNodeState && AsPlaceholderPatternNodeState.ParentIndex == LeafBlock.PatternIndex);
            Assert.That(PatternState.GetAllChildren().Count == 1);

            ILayoutSourceState SourceState = LeafBlock.SourceState;
            Assert.That(SourceState != null);
            Assert.That(SourceState.ParentBlockState == LeafBlock);
            Assert.That(SourceState.ParentInner == SourceInner);
            Assert.That(SourceState.ParentIndex == LeafBlock.SourceIndex);
            Assert.That(SourceState.ParentState == RootState);
            Assert.That(SourceState.InnerTable.Count == 0);
            Assert.That(SourceState is ILayoutNodeState AsPlaceholderSourceNodeState && AsPlaceholderSourceNodeState.ParentIndex == LeafBlock.SourceIndex);
            Assert.That(SourceState.GetAllChildren().Count == 1);

            Assert.That(MainLeafBlocksInner.FirstNodeState == LeafBlock.StateList[0]);

            ILayoutListInner MainLeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as ILayoutListInner;
            Assert.That(MainLeafPathInner != null);
            Assert.That(!MainLeafPathInner.IsNeverEmpty);
            Assert.That(MainLeafPathInner.InterfaceType.IsTypeof<Leaf>());
            Assert.That(MainLeafPathInner.Count == 2);
            Assert.That(MainLeafPathInner.StateList != null);
            Assert.That(MainLeafPathInner.StateList.Count == 2);
            Assert.That(MainLeafPathInner.FirstNodeState == MainLeafPathInner.StateList[0]);
            Assert.That(MainLeafPathInner.IndexAt(0) == MainLeafPathInner.FirstNodeState.ParentIndex);
            Assert.That(MainLeafPathInner.AllIndexes().Count == MainLeafPathInner.Count);

            LayoutNodeStateReadOnlyList AllChildren = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
            Assert.That(AllChildren.Count == 19, $"New count: {AllChildren.Count}");

            ILayoutPlaceholderInner PlaceholderInner = RootState.InnerTable[nameof(Main.PlaceholderLeaf)] as ILayoutPlaceholderInner;
            Assert.That(PlaceholderInner != null);

            ILayoutBrowsingPlaceholderNodeIndex PlaceholderNodeIndex = PlaceholderInner.ChildState.ParentIndex as ILayoutBrowsingPlaceholderNodeIndex;
            Assert.That(PlaceholderNodeIndex != null);
            Assert.That(Controller0.Contains(PlaceholderNodeIndex));

            ILayoutOptionalInner UnassignedOptionalInner = RootState.InnerTable[nameof(Main.UnassignedOptionalLeaf)] as ILayoutOptionalInner;
            Assert.That(UnassignedOptionalInner != null);

            ILayoutBrowsingOptionalNodeIndex UnassignedOptionalNodeIndex = UnassignedOptionalInner.ChildState.ParentIndex;
            Assert.That(UnassignedOptionalNodeIndex != null);
            Assert.That(Controller0.Contains(UnassignedOptionalNodeIndex));
            Assert.That(Controller0.IsAssigned(UnassignedOptionalNodeIndex) == false);

            ILayoutOptionalInner AssignedOptionalInner = RootState.InnerTable[nameof(Main.AssignedOptionalLeaf)] as ILayoutOptionalInner;
            Assert.That(AssignedOptionalInner != null);

            ILayoutBrowsingOptionalNodeIndex AssignedOptionalNodeIndex = AssignedOptionalInner.ChildState.ParentIndex;
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

            LayoutController Controller1 = LayoutController.Create(RootIndex0);
            Assert.That(Controller0.IsEqual(CompareEqual.New(), Controller0));

            //System.Diagnostics.Debug.Assert(false);
            Assert.That(CompareEqual.CoverIsEqual(Controller0, Controller1));

            Assert.That(!Controller0.CanUndo);
            Assert.That(!Controller0.CanRedo);
        }

        [Test]
        [Category("Coverage")]
        public static void LayoutClone()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode = CreateRoot(ValueGuid0, Imperfections.None);

            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(RootNode);
            Assert.That(RootIndex != null);

            LayoutController Controller = LayoutController.Create(RootIndex);
            Assert.That(Controller != null);

            ILayoutPlaceholderNodeState RootState = Controller.RootState;
            Assert.That(RootState != null);

            BaseNode.Node ClonedNode = RootState.CloneNode();
            Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(ClonedNode));

            ILayoutRootNodeIndex CloneRootIndex = new LayoutRootNodeIndex(ClonedNode);
            Assert.That(CloneRootIndex != null);

            LayoutController CloneController = LayoutController.Create(CloneRootIndex);
            Assert.That(CloneController != null);

            ILayoutPlaceholderNodeState CloneRootState = Controller.RootState;
            Assert.That(CloneRootState != null);

            LayoutNodeStateReadOnlyList AllChildren = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
            LayoutNodeStateReadOnlyList CloneAllChildren = (LayoutNodeStateReadOnlyList)CloneRootState.GetAllChildren();
            Assert.That(AllChildren.Count == CloneAllChildren.Count);
        }

        [Test]
        [Category("Coverage")]
        public static void LayoutInsert()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                ILayoutListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as ILayoutListInner;
                Assert.That(LeafPathInner != null);

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                ILayoutBrowsingListNodeIndex ExistingIndex = LeafPathInner.IndexAt(0) as ILayoutBrowsingListNodeIndex;

                Leaf NewItem0 = CreateLeaf(Guid.NewGuid());

                ILayoutInsertionListNodeIndex InsertionIndex0;
                InsertionIndex0 = ExistingIndex.ToInsertionIndex(RootNode, NewItem0) as ILayoutInsertionListNodeIndex;
                Assert.That(InsertionIndex0.ParentNode == RootNode);
                Assert.That(InsertionIndex0.Node == NewItem0);
                Assert.That(CompareEqual.CoverIsEqual(InsertionIndex0, InsertionIndex0));

                LayoutNodeStateReadOnlyList AllChildren0 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Insert(LeafPathInner, InsertionIndex0, out IWriteableBrowsingCollectionNodeIndex NewItemIndex0);
                Assert.That(Controller.Contains(NewItemIndex0));

                ILayoutBrowsingListNodeIndex DuplicateExistingIndex0 = InsertionIndex0.ToBrowsingIndex() as ILayoutBrowsingListNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex0 as ILayoutBrowsingListNodeIndex, DuplicateExistingIndex0));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex0, NewItemIndex0 as ILayoutBrowsingListNodeIndex));

                Assert.That(LeafPathInner.Count == PathCount + 1);
                Assert.That(LeafPathInner.StateList.Count == PathCount + 1);

                ILayoutPlaceholderNodeState NewItemState0 = (ILayoutPlaceholderNodeState)LeafPathInner.StateList[0];
                Assert.That(NewItemState0.Node == NewItem0);
                Assert.That(NewItemState0.ParentIndex == NewItemIndex0);

                LayoutNodeStateReadOnlyList AllChildren1 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                ILayoutBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
                Assert.That(LeafBlocksInner != null);

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[0].StateList.Count;
                Assert.That(BlockNodeCount == 4);

                ILayoutBrowsingExistingBlockNodeIndex ExistingIndex1 = LeafBlocksInner.IndexAt(0, 0) as ILayoutBrowsingExistingBlockNodeIndex;

                Leaf NewItem1 = CreateLeaf(Guid.NewGuid());
                ILayoutInsertionExistingBlockNodeIndex InsertionIndex1;
                InsertionIndex1 = ExistingIndex1.ToInsertionIndex(RootNode, NewItem1) as ILayoutInsertionExistingBlockNodeIndex;
                Assert.That(InsertionIndex1.ParentNode == RootNode);
                Assert.That(InsertionIndex1.Node == NewItem1);
                Assert.That(CompareEqual.CoverIsEqual(InsertionIndex1, InsertionIndex1));

                Controller.Insert(LeafBlocksInner, InsertionIndex1, out IWriteableBrowsingCollectionNodeIndex NewItemIndex1);
                Assert.That(Controller.Contains(NewItemIndex1));

                ILayoutBrowsingExistingBlockNodeIndex DuplicateExistingIndex1 = InsertionIndex1.ToBrowsingIndex() as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex1 as ILayoutBrowsingExistingBlockNodeIndex, DuplicateExistingIndex1));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex1, NewItemIndex1 as ILayoutBrowsingExistingBlockNodeIndex));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount + 1);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == NodeCount + 1);

                ILayoutPlaceholderNodeState NewItemState1 = (ILayoutPlaceholderNodeState)LeafBlocksInner.BlockStateList[0].StateList[0];
                Assert.That(NewItemState1.Node == NewItem1);
                Assert.That(NewItemState1.ParentIndex == NewItemIndex1);
                Assert.That(NewItemState1.ParentState == RootState);

                LayoutNodeStateReadOnlyList AllChildren2 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count + 1, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));




                Leaf NewItem2 = CreateLeaf(Guid.NewGuid());
                BaseNode.Pattern NewPattern = BaseNodeHelper.NodeHelper.CreateSimplePattern("");
                BaseNode.Identifier NewSource = BaseNodeHelper.NodeHelper.CreateSimpleIdentifier("");

                ILayoutInsertionNewBlockNodeIndex InsertionIndex2 = new LayoutInsertionNewBlockNodeIndex(RootNode, nameof(Main.LeafBlocks), NewItem2, 0, NewPattern, NewSource);
                Assert.That(CompareEqual.CoverIsEqual(InsertionIndex2, InsertionIndex2));

                int BlockCount = LeafBlocksInner.BlockStateList.Count;
                Assert.That(BlockCount == 3);

                Controller.Insert(LeafBlocksInner, InsertionIndex2, out IWriteableBrowsingCollectionNodeIndex NewItemIndex2);
                Assert.That(Controller.Contains(NewItemIndex2));

                ILayoutBrowsingExistingBlockNodeIndex DuplicateExistingIndex2 = InsertionIndex2.ToBrowsingIndex() as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex2 as ILayoutBrowsingExistingBlockNodeIndex, DuplicateExistingIndex2));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex2, NewItemIndex2 as ILayoutBrowsingExistingBlockNodeIndex));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount + 2);
                Assert.That(LeafBlocksInner.BlockStateList.Count == BlockCount + 1);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == 1, $"Count: {LeafBlocksInner.BlockStateList[0].StateList.Count}");
                Assert.That(LeafBlocksInner.BlockStateList[1].StateList.Count == 2, $"Count: {LeafBlocksInner.BlockStateList[1].StateList.Count}");
                Assert.That(LeafBlocksInner.BlockStateList[2].StateList.Count == 2, $"Count: {LeafBlocksInner.BlockStateList[2].StateList.Count}");

                ILayoutPlaceholderNodeState NewItemState2 = (ILayoutPlaceholderNodeState)LeafBlocksInner.BlockStateList[0].StateList[0];
                Assert.That(NewItemState2.Node == NewItem2);
                Assert.That(NewItemState2.ParentIndex == NewItemIndex2);

                LayoutNodeStateReadOnlyList AllChildren3 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void LayoutRemove()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                ILayoutListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as ILayoutListInner;
                Assert.That(LeafPathInner != null);

                ILayoutBrowsingListNodeIndex RemovedLeafIndex0 = LeafPathInner.StateList[0].ParentIndex as ILayoutBrowsingListNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex0));

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                LayoutNodeStateReadOnlyList AllChildren0 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Assert.That(Controller.IsRemoveable(LeafPathInner, RemovedLeafIndex0));

                Controller.Remove(LeafPathInner, RemovedLeafIndex0);
                Assert.That(!Controller.Contains(RemovedLeafIndex0));

                Assert.That(LeafPathInner.Count == PathCount - 1);
                Assert.That(LeafPathInner.StateList.Count == PathCount - 1);

                LayoutNodeStateReadOnlyList AllChildren1 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count - 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                RemovedLeafIndex0 = LeafPathInner.StateList[0].ParentIndex as ILayoutBrowsingListNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex0));

                Assert.That(LeafPathInner.Count == 1);

                Assert.That(Controller.IsRemoveable(LeafPathInner, RemovedLeafIndex0));

                IDictionary<Type, string[]> NeverEmptyCollectionTable = BaseNodeHelper.NodeHelper.NeverEmptyCollectionTable as IDictionary<Type, string[]>;
                NeverEmptyCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafPath) });
                Assert.That(!Controller.IsRemoveable(LeafPathInner, RemovedLeafIndex0));



                ILayoutBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
                Assert.That(LeafBlocksInner != null);

                ILayoutBrowsingExistingBlockNodeIndex RemovedLeafIndex1 = LeafBlocksInner.BlockStateList[1].StateList[0].ParentIndex as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex1));

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[1].StateList.Count;
                Assert.That(BlockNodeCount == 4, $"New count: {BlockNodeCount}");

                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex1));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex1);
                Assert.That(!Controller.Contains(RemovedLeafIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount - 1);
                Assert.That(LeafBlocksInner.BlockStateList[1].StateList.Count == NodeCount - 1);

                LayoutNodeStateReadOnlyList AllChildren2 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count - 1, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                ILayoutBrowsingExistingBlockNodeIndex RemovedLeafIndex2 = LeafBlocksInner.BlockStateList[1].StateList[0].ParentIndex as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex2));


                int BlockCount = LeafBlocksInner.BlockStateList.Count;
                Assert.That(BlockCount == 3);

                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex2));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex2);
                Assert.That(!Controller.Contains(RemovedLeafIndex2));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount - 2);
                Assert.That(LeafBlocksInner.BlockStateList.Count == BlockCount - 1);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == 1, $"Count: {LeafBlocksInner.BlockStateList[0].StateList.Count}");

                LayoutNodeStateReadOnlyList AllChildren3 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
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

        // TestFixture requiring use of the STA.
        [TestFixture, Apartment(ApartmentState.STA)]
        public class LayoutFixtureRequiringSTA
        {
            [Test]
            [Category("Coverage")]
            public static void LayoutViews()
            {
                ControllerTools.ResetExpectedName();

                Main RootNode;
                ILayoutRootNodeIndex RootIndex;
                bool IsMoved;
                bool IsCaretMoved;

                RootNode = CreateRoot(ValueGuid0, Imperfections.None);
                RootIndex = new LayoutRootNodeIndex(RootNode);

                LayoutController Controller = LayoutController.Create(RootIndex);
                ILayoutTemplateSet DefaultTemplateSet = LayoutTemplateSet.Default;
                DefaultTemplateSet = LayoutTemplateSet.Default;

                //System.Diagnostics.Debug.Assert(false);
                LayoutFrameSelectorList FrameSelectorList = null;
                foreach (KeyValuePair<Type, ILayoutTemplate> TemplateEntry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)TestDebug.CoverageLayoutTemplateSet.NodeTemplateDictionary)
                    if (TemplateEntry.Key.IsTypeof<Root>())
                    {
                        ILayoutNodeTemplate Template = TemplateEntry.Value as ILayoutNodeTemplate;
                        Assert.That(Template != null);

                        ILayoutHorizontalPanelFrame RootFrame = Template.Root as ILayoutHorizontalPanelFrame;
                        foreach (ILayoutFrame Frame in RootFrame.Items)
                            if (Frame is ILayoutFrameWithSelector AsFrameWithSelector && AsFrameWithSelector.Selectors.Count > 0)
                            {
                                FrameSelectorList = AsFrameWithSelector.Selectors;
                                break;
                            }
                    }
                Assert.That(CompareEqual.CoverIsEqual(FrameSelectorList, FrameSelectorList));

                System.Windows.IDataObject DataObject = new System.Windows.DataObject();

                using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
                {
                    Assert.That(ControllerView0.Controller == Controller);
                    Assert.That(ControllerView0.RootStateView == ControllerView0.StateViewTable[Controller.RootState]);
                    Assert.That(ControllerView0.TemplateSet == TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet);
                    Assert.That(ControllerView0.CaretMode == CaretModes.Insertion);
                    Assert.That(ControllerView0.IsInvalidated);

                    bool IsChanged;
                    ControllerView0.SetCaretMode(CaretModes.Override, out IsChanged);
                    Assert.That(ControllerView0.CaretMode == CaretModes.Override);
                    Assert.That(IsChanged);

                    ControllerView0.SetCaretMode(CaretModes.Insertion, out IsChanged);
                    Assert.That(ControllerView0.CaretMode == CaretModes.Insertion);
                    Assert.That(IsChanged);

                    ControllerView0.SetCaretMode(CaretModes.Override, out IsChanged);
                    Assert.That(ControllerView0.CaretMode == CaretModes.Override);
                    Assert.That(ControllerView0.ActualCaretMode == CaretModes.Override);
                    Assert.That(IsChanged);

                    ControllerView0.SetCaretPosition(1000, true, out IsMoved);
                    Assert.That(IsMoved);
                    Assert.That(ControllerView0.ActualCaretMode == CaretModes.Insertion);
                    ControllerView0.SetCaretPosition(0, true, out IsMoved);
                    Assert.That(IsMoved);
                    Assert.That(ControllerView0.ActualCaretMode == CaretModes.Override);
                    Assert.That(ControllerView0.CaretPosition == ControllerView0.CaretAnchorPosition);

                    using (LayoutControllerView ControllerView1 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
                    {
                        Assert.That(ControllerView0.IsEqual(CompareEqual.New(), ControllerView0));
                        Assert.That(CompareEqual.CoverIsEqual(ControllerView0, ControllerView1));
                    }

                    ControllerView0.MeasureAndArrange();
                    Assert.That(!ControllerView0.IsInvalidated);
                    Assert.That(RegionHelper.IsFixed(ControllerView0.ViewSize));
                    ControllerView0.Invalidate();
                    ControllerView0.Invalidate(Rect.Empty);
                    Assert.That(RegionHelper.IsFixed(ControllerView0.ViewSize));
                    Assert.That(!ControllerView0.IsInvalidated);

                    ILayoutFocusableCellView FocusedCellView = ControllerView0.Focus.CellView;
                    Point CellOrigin = FocusedCellView.CellOrigin;
                    Assert.That(RegionHelper.IsFixed(CellOrigin));
                    Assert.That(!RegionHelper.IsFloatingHorizontally(CellOrigin));
                    Assert.That(!RegionHelper.IsFloatingVertically(CellOrigin));
                    Assert.That(Point.IsEqual(CellOrigin, CellOrigin));
                    Assert.That(CellOrigin.ToString() != null);
                    Assert.That(CellOrigin.ToString(CultureInfo.InvariantCulture) != null);
                    Assert.That(CellOrigin.ToString(null, CultureInfo.InvariantCulture) != null);
                    bool IsOrigin = CellOrigin.IsOrigin;
                    double Distance = Point.Distance(CellOrigin, CellOrigin);
                    Assert.That(Distance == 0);
                    Size CellSize = FocusedCellView.CellSize;
                    Assert.That(!CellSize.IsEmpty);
                    Assert.That(CellSize.IsVisible);
                    Assert.That(Size.IsEqual(CellSize, CellSize));
                    Assert.That(CellSize.ToString() != null);
                    Assert.That(CellSize.ToString(CultureInfo.InvariantCulture) != null);
                    Assert.That(CellSize.ToString(null, CultureInfo.InvariantCulture) != null);
                    Padding CellPadding = FocusedCellView.CellPadding;
                    Assert.That(Padding.IsEqual(CellPadding, CellPadding));
                    Assert.That(CellPadding.ToString() != null);
                    Assert.That(CellPadding.ToString(CultureInfo.InvariantCulture) != null);
                    Assert.That(CellPadding.ToString(null, CultureInfo.InvariantCulture) != null);

                    foreach (KeyValuePair<ILayoutBlockState, LayoutBlockStateView> Entry in (ICollection<KeyValuePair<ILayoutBlockState, LayoutBlockStateView>>)ControllerView0.BlockStateViewTable)
                    {
                        ILayoutBlockState BlockState = Entry.Key;
                        Assert.That(BlockState != null);

                        LayoutBlockStateView BlockStateView = Entry.Value;
                        Assert.That(BlockStateView != null);
                        Assert.That(BlockStateView.BlockState == BlockState);
                        Assert.That(BlockStateView.Template != null);
                        Assert.That(BlockStateView.RootCellView != null);
                        Assert.That(BlockStateView.EmbeddingCellView != null);

                        Assert.That(BlockStateView.ControllerView == ControllerView0);
                    }

                    foreach (KeyValuePair<ILayoutNodeState, ILayoutNodeStateView> Entry in (ICollection<KeyValuePair<ILayoutNodeState, ILayoutNodeStateView>>)ControllerView0.StateViewTable)
                    {
                        ILayoutNodeState State = Entry.Key;
                        Assert.That(State != null);

                        ILayoutNodeStateView StateView = Entry.Value;
                        Assert.That(StateView != null);
                        Assert.That(StateView.State == State);

                        ILayoutIndex ParentIndex = State.ParentIndex;
                        Assert.That(ParentIndex != null);

                        Assert.That(Controller.Contains(ParentIndex));
                        Assert.That(StateView.ControllerView == ControllerView0);

                        ILayoutContainerCellView ParentContainer;
                        LayoutAssignableCellViewReadOnlyDictionary<string> CellViewTable;

                        switch (StateView)
                        {
                            case ILayoutPatternStateView AsPatternStateView:
                                Assert.That(AsPatternStateView.State == State);
                                Assert.That(AsPatternStateView is ILayoutNodeStateView AsPlaceholderPatternNodeStateView && AsPlaceholderPatternNodeStateView.State == State);
                                Assert.That(AsPatternStateView.Template != null);
                                //Assert.That(AsPatternStateView.RootCellView != null);
                                CellViewTable = AsPatternStateView.CellViewTable;
                                ParentContainer = AsPatternStateView.ParentContainer;
                                break;

                            case ILayoutSourceStateView AsSourceStateView:
                                Assert.That(AsSourceStateView.State == State);
                                Assert.That(AsSourceStateView is ILayoutNodeStateView AsPlaceholderSourceNodeStateView && AsPlaceholderSourceNodeStateView.State == State);
                                Assert.That(AsSourceStateView.Template != null);
                                //Assert.That(AsSourceStateView.RootCellView != null);
                                CellViewTable = AsSourceStateView.CellViewTable;
                                ParentContainer = AsSourceStateView.ParentContainer;
                                break;

                            case ILayoutPlaceholderNodeStateView AsPlaceholderNodeStateView:
                                Assert.That(AsPlaceholderNodeStateView.State == State);
                                Assert.That(AsPlaceholderNodeStateView.Template != null);
                                Assert.That(AsPlaceholderNodeStateView.RootCellView != null);
                                Assert.That(AsPlaceholderNodeStateView.CellViewTable != null);
                                Assert.That(AsPlaceholderNodeStateView.ParentContainer != null || State == Controller.RootState);
                                break;

                            case ILayoutOptionalNodeStateView AsOptionalNodeStateView:
                                Assert.That(AsOptionalNodeStateView.State == State);
                                Assert.That(AsOptionalNodeStateView.Template != null);
                                Assert.That(AsOptionalNodeStateView.RootCellView != null);
                                Assert.That(AsOptionalNodeStateView.CellViewTable != null);
                                Assert.That(AsOptionalNodeStateView.ParentContainer != null);
                                break;
                        }
                    }

                    LayoutVisibleCellViewList VisibleCellViewList = new LayoutVisibleCellViewList();
                    ControllerView0.EnumerateVisibleCellViews((IFrameVisibleCellView item) => ListCellViews(item, VisibleCellViewList), out IFrameVisibleCellView FoundCellView, false);
                    ControllerView0.PrintCellViewTree(true);

                    ControllerView0.RootStateView.UpdateActualCellsSize();
                    Rect CellRect;

                    foreach (ILayoutVisibleCellView CellView in VisibleCellViewList)
                    {
                        CellView.Draw();
                        CellView.Print(Point.Origin);
                        CellRect = CellView.CellRect;
                    }

                    CellRect = FocusedCellView.CellRect;
                    Assert.That(!CellRect.IsEmpty);
                    Assert.That(CellRect.IsVisible);
                    Assert.That(RegionHelper.IsFixed(CellRect));
                    Assert.That(Rect.IsEqual(CellRect, CellRect));
                    Assert.That(CellRect.ToString() != null);
                    Assert.That(CellRect.ToString(CultureInfo.InvariantCulture) != null);
                    Assert.That(CellRect.ToString(null, CultureInfo.InvariantCulture) != null);
                    Point RectOrigin = CellRect.Origin;
                    //System.Diagnostics.Debug.Assert(false);
                    CellRect = Rect.VisibleUnion(CellRect, CellRect);
                    Rect EmptyLine = new Rect(CellRect.Origin, new Size(CellRect.Size.Width, Measure.Zero));
                    Rect EmptyColumn = new Rect(CellRect.Origin, new Size(Measure.Zero, CellRect.Size.Height));
                    Rect UnionLine0 = Rect.VisibleUnion(CellRect, EmptyLine);
                    Rect UnionColumn0 = Rect.VisibleUnion(CellRect, EmptyColumn);
                    Rect UnionLine1 = Rect.VisibleUnion(EmptyLine, CellRect);
                    Rect UnionColumn1 = Rect.VisibleUnion(EmptyColumn, CellRect);
                    Rect UnionLine2 = Rect.VisibleUnion(EmptyLine, EmptyLine);
                    Rect UnionColumn2 = Rect.VisibleUnion(EmptyColumn, EmptyColumn);

                    ControllerView0.SetCommentDisplayMode(CommentDisplayModes.All);
                    ControllerView0.SetShowUnfocusedComments(false);
                    ControllerView0.SetShowUnfocusedComments(true);
                    VisibleCellViewList.Clear();
                    ControllerView0.EnumerateVisibleCellViews((IFrameVisibleCellView item) => ListCellViews(item, VisibleCellViewList), out FoundCellView, false);
                    ControllerView0.PrintCellViewTree(true);
                    ControllerView0.MeasureAndArrange();

                    ControllerView0.RootStateView.UpdateActualCellsSize();

                    foreach (ILayoutVisibleCellView CellView in VisibleCellViewList)
                    {
                        Assert.That(CompareEqual.CoverIsEqual(CellView, CellView));
                        CellView.Draw();
                        CellView.Print(Point.Origin);
                    }

                    //Assert.That(ControllerView0.MinFocusMove == -2);
                    Assert.That(ControllerView0.MinFocusMove == -1);
                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    Assert.That(IsMoved);
                    Assert.That(ControllerView0.MinFocusMove == 0);

                    ControllerView0.MoveFocus(-1, true, out IsMoved);
                    Assert.That(!IsMoved);
                    Assert.That(ControllerView0.MinFocusMove == 0);

                    Assert.That(ControllerView0.MaxFocusMove > 0);
                    Assert.That(ControllerView0.FocusedText != null);
                    ControllerView0.SetCaretPosition(0, true, out IsMoved);
                    Assert.That(IsMoved);

                    ControllerView0.SelectStringContent(ControllerView0.Focus.CellView.StateView.State, "ValueString", 0, 1);

                    //System.Diagnostics.Debug.Assert(false);
                    ControllerView0.CopySelection(DataObject);
                    ControllerView0.PrintSelection();
                    ControllerView0.CutSelection(DataObject, out bool IsDeleted);
                    Assert.That(IsDeleted);

                    string SurrogateString = " \u2028 \ud800\udc00 \ud800 ";
                    DataObject.SetData(typeof(string), SurrogateString);
                    System.Windows.Clipboard.SetDataObject(DataObject);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.SelectStringContent(ControllerView0.Focus.CellView.StateView.State, "ValueString", 0, 1);
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.ClearSelection();
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);


                    ControllerView0.SelectDiscreteContent(ControllerView0.Focus.CellView.StateView.State, "ValueEnum");

                    //System.Diagnostics.Debug.Assert(false);
                    ControllerView0.CopySelection(DataObject);
                    ControllerView0.PrintSelection();
                    ControllerView0.CutSelection(DataObject, out IsDeleted);
                    Assert.That(!IsDeleted);

                    System.Windows.Clipboard.SetDataObject(DataObject);

                    ControllerView0.SelectDiscreteContent(ControllerView0.Focus.CellView.StateView.State, "ValueEnum");
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(!IsChanged);

                    Controller.ChangeDiscreteValue(ControllerView0.Focus.CellView.StateView.State.ParentIndex, "ValueEnum", 1);
                    ControllerView0.SelectDiscreteContent(ControllerView0.Focus.CellView.StateView.State, "ValueEnum");
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);


                    ControllerView0.SelectComment(ControllerView0.Focus.CellView.StateView.State, 0, 1);

                    ControllerView0.CopySelection(DataObject);
                    ControllerView0.PrintSelection();
                    ControllerView0.CutSelection(DataObject, out IsDeleted);
                    Assert.That(IsDeleted);

                    System.Windows.Clipboard.SetDataObject(DataObject);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.SelectComment(ControllerView0.Focus.CellView.StateView.State, 0, 1);
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.ClearSelection();
                    ControllerView0.CopySelection(DataObject);
                    ControllerView0.PrintSelection();
                    ControllerView0.CutSelection(DataObject, out IsDeleted);
                    Assert.That(!IsDeleted);

                    //System.Diagnostics.Debug.Assert(false);
                    ControllerView0.SelectComment(ControllerView0.Focus.CellView.StateView.State, 0, 1);
                    ControllerView0.CopySelection(DataObject);
                    ControllerView0.PrintSelection();

                    int StringSearch = 0;
                    while (!(ControllerView0.Focus is ILayoutStringContentFocus))
                    {
                        StringSearch++;
                        ControllerView0.MoveFocus(+1, true, out IsMoved);

                        if (!IsMoved)
                            break;
                    }

                    ControllerView0.ClearSelection();
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.MoveFocus(-StringSearch, true, out IsMoved);

                    ControllerView0.MoveFocus(+1, true, out IsMoved);
                    Assert.That(ControllerView0.FocusedText != null);
                    Assert.That(!ControllerView0.IsUserVisible);

                    ControllerView0.MoveFocus(+1, true, out IsMoved);
                    Assert.That(ControllerView0.FocusedText != null);
                    Assert.That(!ControllerView0.IsUserVisible);

                    ControllerView0.SetCaretPosition(0, true, out IsMoved);
                    Assert.That(!IsMoved);

                    ControllerView0.ShowCaret(false, false);
                    ControllerView0.ShowCaret(false, true);
                    ControllerView0.ShowCaret(true, false);
                    ControllerView0.Invalidate();
                    ControllerView0.Draw(ControllerView0.RootStateView);
                    ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                    ControllerView0.Invalidate();
                    ControllerView0.Draw(ControllerView0.RootStateView);
                    ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);

                    Assert.That(!ControllerView0.ShowLineNumber);
                    ControllerView0.SetShowLineNumber(!ControllerView0.ShowLineNumber);
                    Assert.That(ControllerView0.ShowLineNumber);

                    //System.Diagnostics.Debug.Assert(false);
                    ControllerView0.Draw(ControllerView0.RootStateView);
                    ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);

                    ControllerView0.SetShowLineNumber(!ControllerView0.ShowLineNumber);
                    Assert.That(!ControllerView0.ShowLineNumber);

                    ControllerView0.ShowCaret(false, true);
                    ControllerView0.ShowCaret(true, true);

                    while (ControllerView0.MaxFocusMove > 0)
                    {
                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                        string FocusedText = ControllerView0.FocusedText;
                        TextStyles FocusedTextStyle = ControllerView0.FocusedTextStyle;
                        Assert.That(FocusedText != null || FocusedTextStyle == TextStyles.Default);

                        ControllerView0.ShowCaret(false, false);
                        ControllerView0.Draw(ControllerView0.RootStateView);
                        ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        ControllerView0.ShowCaret(true, false);
                        ControllerView0.Draw(ControllerView0.RootStateView);
                        ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        ControllerView0.ShowCaret(false, false);
                        ControllerView0.Draw(ControllerView0.RootStateView);
                        ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        ControllerView0.ShowCaret(true, true);
                        ControllerView0.Draw(ControllerView0.RootStateView);
                        ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        ControllerView0.ShowCaret(false, true);
                        ControllerView0.Draw(ControllerView0.RootStateView);
                        ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        ControllerView0.ShowCaret(true, false);
                        ControllerView0.Draw(ControllerView0.RootStateView);
                        ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                    }

                    Assert.That(ControllerView0.MaxFocusMove == 0);
                    ControllerView0.MoveFocus(+1, true, out IsMoved);
                    Assert.That(ControllerView0.MaxFocusMove == 0);

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    Assert.That(ControllerView0.MinFocusMove == 0);

                    //System.Diagnostics.Debug.Assert(false);

                    while (ControllerView0.MaxFocusMove > 0)
                    {
                        IFocusInner Inner;
                        IFocusInsertionChildIndex InsertionIndex;
                        IFocusCollectionInner CollectionInner;
                        IFocusBlockListInner BlockListInner;
                        IFocusListInner ListInner;
                        IFocusInsertionCollectionNodeIndex InsertionCollectionIndex;
                        IFocusBrowsingCollectionNodeIndex BrowsingCollectionIndex;
                        IFocusBrowsingExistingBlockNodeIndex ExistingBlockNodeIndex;
                        IFocusInsertionListNodeIndex ReplacementListNodeIndex, InsertionListNodeIndex;
                        int BlockIndex;
                        BaseNode.ReplicationStatus Replication;

                        bool IsUserVisible = ControllerView0.IsUserVisible;
                        bool IsNewItemInsertable = ControllerView0.IsNewItemInsertable(out CollectionInner, out InsertionCollectionIndex);
                        bool IsItemRemoveable = ControllerView0.IsItemRemoveable(out CollectionInner, out BrowsingCollectionIndex);
                        bool IsItemMoveable = ControllerView0.IsItemMoveable(-1, out CollectionInner, out BrowsingCollectionIndex);
                        bool IsItemSplittable = ControllerView0.IsItemSplittable(out BlockListInner, out ExistingBlockNodeIndex);
                        bool IsReplicationModifiable = ControllerView0.IsReplicationModifiable(out BlockListInner, out BlockIndex, out Replication);
                        bool IsItemMergeable = ControllerView0.IsItemMergeable(out BlockListInner, out ExistingBlockNodeIndex);
                        bool IsBlockMoveable = ControllerView0.IsBlockMoveable(-1, out BlockListInner, out BlockIndex);
                        bool IsItemSimplifiable = ControllerView0.IsItemSimplifiable(out Inner, out InsertionIndex);
                        bool IsItemComplexifiable = ControllerView0.IsItemComplexifiable(out IDictionary<IFocusInner, IList<IFocusInsertionChildNodeIndex>> IndexTable);
                        bool IsIdentifierSplittable = ControllerView0.IsIdentifierSplittable(out ListInner, out ReplacementListNodeIndex, out InsertionListNodeIndex);

                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }

                    ILayoutBlockListInner MainLeafBlocksInner = Controller.RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
                    while (!MainLeafBlocksInner.IsEmpty)
                    {
                        IWriteableBrowsingExistingBlockNodeIndex NodeIndex = MainLeafBlocksInner.IndexAt(0, 0) as IWriteableBrowsingExistingBlockNodeIndex;
                        Controller.Remove(MainLeafBlocksInner, NodeIndex);
                    }

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    Assert.That(ControllerView0.MinFocusMove == 0);

                    while (ControllerView0.MaxFocusMove > 0)
                    {
                        IFocusInner Inner;
                        IFocusInsertionChildIndex InsertionIndex;
                        IFocusCollectionInner CollectionInner;
                        IFocusBlockListInner BlockListInner;
                        IFocusListInner ListInner;
                        IFocusInsertionCollectionNodeIndex InsertionCollectionIndex;
                        IFocusBrowsingCollectionNodeIndex BrowsingCollectionIndex;
                        IFocusBrowsingExistingBlockNodeIndex ExistingBlockNodeIndex;
                        IFocusInsertionListNodeIndex ReplacementListNodeIndex, InsertionListNodeIndex;
                        int BlockIndex;
                        BaseNode.ReplicationStatus Replication;

                        bool IsUserVisible = ControllerView0.IsUserVisible;

                        if (ControllerView0.MaxCaretPosition > 0)
                            ControllerView0.SetCaretPosition(ControllerView0.MaxCaretPosition, true, out IsMoved);

                        bool IsNewItemInsertable = ControllerView0.IsNewItemInsertable(out CollectionInner, out InsertionCollectionIndex);
                        bool IsItemRemoveable = ControllerView0.IsItemRemoveable(out CollectionInner, out BrowsingCollectionIndex);
                        bool IsItemMoveable = ControllerView0.IsItemMoveable(-1, out CollectionInner, out BrowsingCollectionIndex);
                        bool IsItemSplittable = ControllerView0.IsItemSplittable(out BlockListInner, out ExistingBlockNodeIndex);
                        bool IsReplicationModifiable = ControllerView0.IsReplicationModifiable(out BlockListInner, out BlockIndex, out Replication);
                        bool IsItemMergeable = ControllerView0.IsItemMergeable(out BlockListInner, out ExistingBlockNodeIndex);
                        bool IsBlockMoveable = ControllerView0.IsBlockMoveable(-1, out BlockListInner, out BlockIndex);
                        bool IsItemSimplifiable = ControllerView0.IsItemSimplifiable(out Inner, out InsertionIndex);
                        bool IsItemComplexifiable = ControllerView0.IsItemComplexifiable(out IDictionary<IFocusInner, IList<IFocusInsertionChildNodeIndex>> IndexTable);
                        bool IsIdentifierSplittable = ControllerView0.IsIdentifierSplittable(out ListInner, out ReplacementListNodeIndex, out InsertionListNodeIndex);

                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }

                    ControllerView0.SetCommentDisplayMode(CommentDisplayModes.OnFocus);
                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);

                    ControllerView0.ForceShowComment(out IsMoved);
                    ControllerView0.SetUserVisible(true);
                    ControllerView0.SetUserVisible(false);

                    int MaxFocusMove = ControllerView0.MaxFocusMove;

                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        if (ControllerView0.Focus is ILayoutDiscreteContentFocus AsDiscreteContentFocus)
                        {
                            ControllerView0.SetUserVisible(true);
                            ControllerView0.SetUserVisible(false);
                            break;
                        }

                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                        Assert.That(IsMoved);
                    }

                    Assert.That(ControllerView0.MaxFocusMove > 0);

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    MaxFocusMove = ControllerView0.MaxFocusMove;
                    //System.Diagnostics.Debug.Assert(false);

                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        ControllerView0.ForceShowComment(out IsMoved);
                        Assert.That(!IsMoved || ControllerView0.CaretPosition >= 0);

                        if (ControllerView0.FocusedText != null && ControllerView0.FocusedText.Length == 0)
                        {
                            ControllerView0.SetCaretMode(CaretModes.Insertion, out IsChanged);
                            ControllerView0.SetCaretMode(CaretModes.Override, out IsChanged);
                        }

                        if (ControllerView0.CaretPosition >= 0)
                        {
                            for (int j = 0; j <= ControllerView0.MaxCaretPosition; j++)
                                ControllerView0.SetCaretPosition(j, true, out IsCaretMoved);
                        }

                        ControllerView0.SetCaretMode(CaretModes.Insertion, out IsChanged);
                        ControllerView0.SetCaretMode(CaretModes.Override, out IsChanged);
                        ControllerView0.SetCaretPosition(0, true, out IsCaretMoved);
                        ControllerView0.SetCaretPosition(ControllerView0.MaxCaretPosition, true, out IsCaretMoved);
                        ControllerView0.Draw(ControllerView0.RootStateView);
                        ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);

                        if (IsMoved)
                        {
                            ControllerView0.MoveFocus(+1, true, out IsMoved);
                            ControllerView0.Draw(ControllerView0.RootStateView);
                            ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        }

                        ControllerView0.MoveFocus(+1, true, out IsMoved);

                        ControllerView0.SetCaretPosition(0, true, out IsCaretMoved);
                        ControllerView0.SetCaretPosition(ControllerView0.MaxCaretPosition, true, out IsCaretMoved);
                        ControllerView0.Draw(ControllerView0.RootStateView);
                        ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                    }

                    //Assert.That(ControllerView0.MaxFocusMove == 0);
                    Assert.That(ControllerView0.MaxFocusMove == 9);

                    ControllerView0.MeasureAndArrange();
                    ControllerView0.Draw(ControllerView0.RootStateView);
                    ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);

                    for (int i = 0; i < MaxFocusMove; i++)
                        ControllerView0.MoveFocusVertically(-20, i % 2 == 0, out IsMoved);

                    for (int i = 0; i < MaxFocusMove; i++)
                        ControllerView0.MoveFocusVertically(+20, i % 2 == 0, out IsMoved);

                    ControllerView0.SetCaretMode(CaretModes.Override, out IsChanged);
                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);

                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        ControllerView0.ForceShowComment(out IsMoved);
                        ControllerView0.Draw(ControllerView0.RootStateView);
                        ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        ControllerView0.MoveFocusVertically(+10, i % 2 == 0, out IsMoved);
                    }

                    //System.Diagnostics.Debug.Assert(false);
                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);

                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        ControllerView0.Draw(ControllerView0.RootStateView);
                        ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        ControllerView0.SetCaretMode(CaretModes.Override, out IsChanged);
                        ControllerView0.MoveFocusVertically(+10, i % 2 == 0, out IsMoved);
                    }

                    //System.Diagnostics.Debug.Assert(false);
                    BaseNodeHelper.NodeTreeHelper.SetCommentText(RootNode.Documentation, "");
                    BaseNodeHelper.NodeTreeHelper.SetCommentText(RootNode.PlaceholderTree.Documentation, "");

                    //System.Diagnostics.Debug.Assert(false);
                    ILayoutOptionalInner OptionalTreeInner = Controller.RootState.PropertyToInner(nameof(Main.AssignedOptionalTree)) as ILayoutOptionalInner;
                    Controller.Unassign(OptionalTreeInner.ChildState.ParentIndex, out IsChanged);
                    Assert.That(IsChanged);

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    ControllerView0.MoveFocus(ControllerView0.MaxFocusMove, true, out IsMoved);
                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    MaxFocusMove = ControllerView0.MaxFocusMove;

                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        ControllerView0.ForceShowComment(out IsMoved);
                        Assert.That(!IsMoved || ControllerView0.CaretPosition >= 0);

                        ControllerView0.MeasureAndArrange();

                        if (ControllerView0.FocusedText != null && ControllerView0.FocusedText.Length == 0)
                        {
                            ControllerView0.SetCaretMode(CaretModes.Insertion, out IsChanged);
                            ControllerView0.SetCaretMode(CaretModes.Override, out IsChanged);
                        }

                        if (ControllerView0.CaretPosition >= 0)
                        {
                            for (int j = 0; j <= ControllerView0.MaxCaretPosition; j++)
                                ControllerView0.SetCaretPosition(j, true, out IsCaretMoved);
                        }

                        ControllerView0.SetCaretMode(CaretModes.Insertion, out IsChanged);
                        ControllerView0.SetCaretMode(CaretModes.Override, out IsChanged);
                        ControllerView0.SetCaretPosition(0, true, out IsCaretMoved);
                        ControllerView0.SetCaretPosition(ControllerView0.MaxCaretPosition, true, out IsCaretMoved);
                        ControllerView0.Draw(ControllerView0.RootStateView);
                        ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);

                        if (IsMoved)
                        {
                            ControllerView0.MoveFocus(+1, true, out IsMoved);
                            ControllerView0.Draw(ControllerView0.RootStateView);
                            ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        }

                        ControllerView0.MoveFocus(+1, true, out IsMoved);

                        ControllerView0.SetCaretPosition(0, true, out IsCaretMoved);
                        ControllerView0.SetCaretPosition(ControllerView0.MaxCaretPosition, true, out IsCaretMoved);
                        ControllerView0.Draw(ControllerView0.RootStateView);
                        ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                    }

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    MaxFocusMove = ControllerView0.MaxFocusMove;

                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        ControllerView0.ForceShowComment(out IsMoved);
                        ControllerView0.MeasureAndArrange();
                        ControllerView0.Draw(ControllerView0.RootStateView);
                        ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        ControllerView0.MoveFocusHorizontally(-1, true, out IsMoved);
                        ControllerView0.MeasureAndArrange();
                        ControllerView0.Draw(ControllerView0.RootStateView);
                        ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        ControllerView0.MoveFocusHorizontally(+1, true, out IsMoved);
                        ControllerView0.MeasureAndArrange();
                        ControllerView0.Draw(ControllerView0.RootStateView);
                        ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }

                    ControllerView0.SetFocusToPoint(-1.0, -1.0, true, out IsMoved);
                    Assert.That(!IsMoved);

                    CellOrigin = ControllerView0.Focus.CellView.CellOrigin;

                    ControllerView0.CellViewFromPoint(CellOrigin.X.Draw, CellOrigin.Y.Draw, out ILayoutVisibleCellView PointedCellView);
                    Assert.That(PointedCellView == ControllerView0.Focus.CellView);

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    Assert.That(IsMoved);

                    ControllerView0.SetFocusToPoint(CellOrigin.X.Draw, CellOrigin.Y.Draw, true, out IsMoved);
                    Assert.That(IsMoved);

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, false, out IsMoved);
                    Assert.That(!ControllerView0.IsSelectionEmpty);

                    MaxFocusMove = ControllerView0.MaxFocusMove;
                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        ControllerView0.ForceShowComment(out IsMoved);

                        ControllerView0.SetCaretPosition(0, true, out IsCaretMoved);
                        ControllerView0.SetCaretPosition(2, false, out IsCaretMoved);
                        ControllerView0.SetCaretPosition(3, false, out IsCaretMoved);
                        if (ControllerView0.Selection is ILayoutCommentSelection AsCommentSelection && AsCommentSelection.Start != AsCommentSelection.End)
                        {
                            Assert.That(AsCommentSelection.StateView != null);
                            AsCommentSelection.Update(AsCommentSelection.Start, AsCommentSelection.End);
                            AsCommentSelection.Update(AsCommentSelection.End, AsCommentSelection.Start);
                            AsCommentSelection.Copy(DataObject);
                            break;
                        }

                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    MaxFocusMove = ControllerView0.MaxFocusMove;
                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        ControllerView0.SetCaretPosition(0, true, out IsCaretMoved);
                        ControllerView0.SetCaretPosition(2, false, out IsCaretMoved);
                        ControllerView0.SetCaretPosition(3, false, out IsCaretMoved);
                        if (ControllerView0.Selection is IFocusStringContentSelection AsStringContentSelection && AsStringContentSelection.Start != AsStringContentSelection.End)
                        {
                            Assert.That(AsStringContentSelection.StateView != null);
                            Assert.That(AsStringContentSelection.PropertyName != null);
                            AsStringContentSelection.Update(AsStringContentSelection.Start, AsStringContentSelection.End);
                            AsStringContentSelection.Update(AsStringContentSelection.End, AsStringContentSelection.Start);
                            AsStringContentSelection.Copy(DataObject);
                            break;
                        }

                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }

                    ControllerView0.ClearSelection();
                    ILayoutNodeStateView SelectionStateView = ControllerView0.Selection.StateView;
                }
            }

            [Test]
            [Category("Coverage")]
            public static void LayoutRemoveBlockRange()
            {
                ControllerTools.ResetExpectedName();

                Main RootNode;
                ILayoutRootNodeIndex RootIndex;

                RootNode = CreateRoot(ValueGuid0, Imperfections.None);
                RootIndex = new LayoutRootNodeIndex(RootNode);

                LayoutController ControllerBase = LayoutController.Create(RootIndex);
                LayoutController Controller = LayoutController.Create(RootIndex);

                using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
                {
                    Assert.That(ControllerView0.Controller == Controller);

                    ILayoutNodeState RootState = Controller.RootState;
                    Assert.That(RootState != null);

                    LayoutNodeStateReadOnlyList AllChildren0 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                    Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                    ILayoutBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
                    Assert.That(LeafBlocksInner != null);
                    Assert.That(LeafBlocksInner.BlockStateList.Count == 3, $"New count: {LeafBlocksInner.BlockStateList.Count}");
                    Assert.That(Controller.IsBlockRangeRemoveable(LeafBlocksInner, 0, 2));

                    Controller.RemoveBlockRange(LeafBlocksInner, 0, 2);
                    Assert.That(LeafBlocksInner.Count == 1);

                    LayoutNodeStateReadOnlyList AllChildren2 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
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

                    System.Windows.IDataObject DataObject = new System.Windows.DataObject();
                    ControllerView0.SelectBlockList(RootState, nameof(Main.LeafBlocks), 1, 2);

                    ControllerView0.CopySelection(DataObject);
                    ControllerView0.PrintSelection();
                    ControllerView0.CutSelection(DataObject, out bool IsDeleted);
                    Assert.That(IsDeleted);

                    System.Windows.Clipboard.SetDataObject(DataObject);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.SelectBlockList(RootState, nameof(Main.LeafBlocks), 1, 2);
                    ControllerView0.PasteSelection(out bool IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
                }
            }

            [Test]
            [Category("Coverage")]
            public static void LayoutRemoveNodeRange()
            {
                ControllerTools.ResetExpectedName();

                Main RootNode;
                ILayoutRootNodeIndex RootIndex;

                RootNode = CreateRoot(ValueGuid0, Imperfections.None);
                RootIndex = new LayoutRootNodeIndex(RootNode);

                LayoutController ControllerBase = LayoutController.Create(RootIndex);
                LayoutController Controller = LayoutController.Create(RootIndex);

                using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
                {
                    Assert.That(ControllerView0.Controller == Controller);

                    ILayoutNodeState RootState = Controller.RootState;
                    Assert.That(RootState != null);

                    LayoutNodeStateReadOnlyList AllChildren0 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                    Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");


                    //System.Diagnostics.Debug.Assert(false);
                    ILayoutListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as ILayoutListInner;
                    Assert.That(LeafPathInner != null);
                    Assert.That(LeafPathInner.StateList.Count == 2, $"New count: {LeafPathInner.StateList.Count}");
                    Assert.That(Controller.IsNodeRangeRemoveable(LeafPathInner, -1, 0, 2));

                    Controller.RemoveNodeRange(LeafPathInner, -1, 0, 2);

                    LayoutNodeStateReadOnlyList AllChildren1 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                    Assert.That(AllChildren1.Count == AllChildren0.Count - 2, $"New count: {AllChildren1.Count}");

                    Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    Assert.That(!Controller.CanUndo);
                    Assert.That(Controller.CanRedo);

                    Controller.Redo();
                    Controller.Undo();



                    ILayoutBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
                    Assert.That(LeafBlocksInner != null);
                    Assert.That(LeafBlocksInner.BlockStateList.Count == 3, $"New count: {LeafBlocksInner.BlockStateList.Count}");
                    Assert.That(Controller.IsNodeRangeRemoveable(LeafBlocksInner, 1, 0, 2));

                    Controller.RemoveNodeRange(LeafBlocksInner, 1, 0, 2);

                    LayoutNodeStateReadOnlyList AllChildren2 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
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

                    System.Windows.IDataObject DataObject = new System.Windows.DataObject();

                    ControllerView0.SelectNode((RootState.InnerTable[nameof(Main.PlaceholderLeaf)] as ILayoutPlaceholderInner).ChildState);

                    ControllerView0.CopySelection(DataObject);
                    ControllerView0.PrintSelection();
                    ControllerView0.CutSelection(DataObject, out bool IsDeleted);
                    Assert.That(!IsDeleted);

                    System.Windows.Clipboard.SetDataObject(DataObject);

                    ControllerView0.SelectNode((RootState.InnerTable[nameof(Main.PlaceholderLeaf)] as ILayoutPlaceholderInner).ChildState);
                    ControllerView0.PasteSelection(out bool IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();



                    ControllerView0.SelectNodeList(RootState, nameof(Main.LeafPath), 0, 2);

                    ControllerView0.CopySelection(DataObject);
                    //System.Diagnostics.Debug.Assert(false);
                    ControllerView0.PrintSelection();
                    ControllerView0.CutSelection(DataObject, out IsDeleted);
                    Assert.That(IsDeleted);

                    System.Windows.Clipboard.SetDataObject(DataObject);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.SelectNodeList(RootState, nameof(Main.LeafPath), 0, 2);
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    //System.Diagnostics.Debug.Assert(false);
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.SelectNodeList(RootState, nameof(Main.LeafPath), 0, 0);
                    ControllerView0.CopySelection(DataObject);
                    ControllerView0.PrintSelection();
                    System.Windows.Clipboard.SetDataObject(DataObject);

                    ControllerView0.SelectNodeList(RootState, nameof(Main.LeafPath), 0, 2);
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();


                    ControllerView0.SelectBlockNodeList(RootState, nameof(Main.LeafBlocks), 1, 0, 1);

                    ControllerView0.CopySelection(DataObject);
                    ControllerView0.PrintSelection();
                    ControllerView0.CutSelection(DataObject, out IsDeleted);
                    Assert.That(IsDeleted);

                    System.Windows.Clipboard.SetDataObject(DataObject);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.SelectBlockNodeList(RootState, nameof(Main.LeafBlocks), 1, 0, 1);
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    //System.Diagnostics.Debug.Assert(false);
                    DataObject = new System.Windows.DataObject();
                    ControllerView0.SelectNode((RootState.InnerTable[nameof(Main.PlaceholderLeaf)] as IFocusPlaceholderInner).ChildState);
                    ControllerView0.CopySelection(DataObject);
                    ControllerView0.PrintSelection();
                    System.Windows.Clipboard.SetDataObject(DataObject);

                    ControllerView0.SelectBlockNodeList(RootState, nameof(Main.LeafBlocks), 1, 0, 1);
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
                }
            }
        }

        [Test]
        [Category("Coverage")]
        public static void LayoutMove()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                ILayoutListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as ILayoutListInner;
                Assert.That(LeafPathInner != null);

                ILayoutBrowsingListNodeIndex MovedLeafIndex0 = LeafPathInner.IndexAt(0) as ILayoutBrowsingListNodeIndex;
                Assert.That(Controller.Contains(MovedLeafIndex0));

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                LayoutNodeStateReadOnlyList AllChildren0 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Assert.That(Controller.IsMoveable(LeafPathInner, MovedLeafIndex0, +1));

                Controller.Move(LeafPathInner, MovedLeafIndex0, +1);
                Assert.That(Controller.Contains(MovedLeafIndex0));

                Assert.That(LeafPathInner.Count == PathCount);
                Assert.That(LeafPathInner.StateList.Count == PathCount);

                //System.Diagnostics.Debug.Assert(false);
                ILayoutBrowsingListNodeIndex NewLeafIndex0 = LeafPathInner.IndexAt(1) as ILayoutBrowsingListNodeIndex;
                Assert.That(NewLeafIndex0 == MovedLeafIndex0);

                LayoutNodeStateReadOnlyList AllChildren1 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));




                ILayoutBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
                Assert.That(LeafBlocksInner != null);

                ILayoutBrowsingExistingBlockNodeIndex MovedLeafIndex1 = LeafBlocksInner.IndexAt(1, 1) as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(MovedLeafIndex1));

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[1].StateList.Count;
                Assert.That(BlockNodeCount == 4, $"New count: {BlockNodeCount}");

                Assert.That(Controller.IsMoveable(LeafBlocksInner, MovedLeafIndex1, -1));
                Controller.Move(LeafBlocksInner, MovedLeafIndex1, -1);
                Assert.That(Controller.Contains(MovedLeafIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount);
                Assert.That(LeafBlocksInner.BlockStateList[1].StateList.Count == NodeCount);

                ILayoutBrowsingExistingBlockNodeIndex NewLeafIndex1 = LeafBlocksInner.IndexAt(1, 0) as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(NewLeafIndex1 == MovedLeafIndex1);

                LayoutNodeStateReadOnlyList AllChildren2 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void LayoutMoveBlock()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                LayoutNodeStateReadOnlyList AllChildren1 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == 19, $"New count: {AllChildren1.Count}");

                ILayoutBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
                Assert.That(LeafBlocksInner != null);

                ILayoutBrowsingExistingBlockNodeIndex MovedLeafIndex1 = LeafBlocksInner.IndexAt(1, 0) as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(MovedLeafIndex1));

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[1].StateList.Count;
                Assert.That(BlockNodeCount == 4, $"New count: {BlockNodeCount}");

                Assert.That(Controller.IsBlockMoveable(LeafBlocksInner, 1, -1));
                Controller.MoveBlock(LeafBlocksInner, 1, -1);
                Assert.That(Controller.Contains(MovedLeafIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == NodeCount);

                ILayoutBrowsingExistingBlockNodeIndex NewLeafIndex1 = LeafBlocksInner.IndexAt(0, 0) as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(NewLeafIndex1 == MovedLeafIndex1);

                LayoutNodeStateReadOnlyList AllChildren2 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void LayoutChangeDiscreteValue()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetEnumValue(RootState.Node, nameof(Main.ValueEnum)) == (int)BaseNode.CopySemantic.Value);

                Controller.ChangeDiscreteValue(RootIndex, nameof(Main.ValueEnum), (int)BaseNode.CopySemantic.Reference);

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetEnumValue(RootNode, nameof(Main.ValueEnum)) == (int)BaseNode.CopySemantic.Reference);

                ILayoutPlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as ILayoutPlaceholderInner;
                ILayoutPlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as ILayoutPlaceholderNodeState;

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
        public static void LayoutChangeText()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetString(RootState.Node, nameof(Main.ValueString)) == "s");

                Controller.ChangeText(RootIndex, nameof(Main.ValueString), "test");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetString(RootNode, nameof(Main.ValueString)) == "test");

                ILayoutPlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as ILayoutPlaceholderInner;
                ILayoutPlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as ILayoutPlaceholderNodeState;

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
        public static void LayoutChangeTextAndCaretPosition()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetString(RootState.Node, nameof(Main.ValueString)) == "s");

                Controller.ChangeTextAndCaretPosition(RootIndex, nameof(Main.ValueString), "test", 1, 2, false);
                Controller.ChangeTextAndCaretPosition(RootIndex, nameof(Main.ValueString), "test", 1, 2, true);

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetString(RootNode, nameof(Main.ValueString)) == "test");

                while (!(ControllerView0.Focus is ILayoutStringContentFocus))
                    ControllerView0.MoveFocus(+1, true, out bool IsMoved);

                ControllerView0.SetAutoFormatMode(AutoFormatModes.FirstOnly);
                ControllerView0.ChangeFocusedText("test", 3, false);
                ControllerView0.SetAutoFormatMode(AutoFormatModes.FirstOrAll);
                ControllerView0.ChangeFocusedText("test", 3, false);
                ControllerView0.SetAutoFormatMode(AutoFormatModes.AllLowercase);
                ControllerView0.ChangeFocusedText("test", 3, false);
                ControllerView0.SetAutoFormatMode(AutoFormatModes.None);
                ControllerView0.ChangeFocusedText("test", 3, false);

                ILayoutPlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as ILayoutPlaceholderInner;
                ILayoutPlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as ILayoutPlaceholderNodeState;

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
                Controller.Redo();
                Controller.Redo();
                Controller.Undo();
                Controller.Undo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void LayoutChangeComment()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetCommentText(RootState.Node) == "main doc");

                Controller.ChangeComment(RootIndex, "test");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetCommentText(RootNode) == "test");

                ILayoutPlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as ILayoutPlaceholderInner;
                ILayoutPlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as ILayoutPlaceholderNodeState;

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
        public static void LayoutChangeCommentAndCaretPosition()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetCommentText(RootState.Node) == "main doc");

                Controller.ChangeCommentAndCaretPosition(RootIndex, "test", 1, 2, false);
                Controller.ChangeCommentAndCaretPosition(RootIndex, "test", 2, 1, true);

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetCommentText(RootNode) == "test");

                ILayoutPlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as ILayoutPlaceholderInner;
                ILayoutPlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as ILayoutPlaceholderNodeState;

                ControllerView0.SetCommentDisplayMode(CommentDisplayModes.All);

                while (!(ControllerView0.Focus is ILayoutCommentFocus))
                    ControllerView0.MoveFocus(+1, true, out bool IsMoved);

                ControllerView0.ChangeFocusedText("test", 3, false);

                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Redo();
                Controller.Redo();
                Controller.Undo();
                Controller.Undo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void LayoutReplace()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Leaf NewItem0 = CreateLeaf(Guid.NewGuid());
                ILayoutInsertionListNodeIndex ReplacementIndex0 = new LayoutInsertionListNodeIndex(RootNode, nameof(Main.LeafPath), NewItem0, 0);

                ILayoutListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as ILayoutListInner;
                Assert.That(LeafPathInner != null);

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                LayoutNodeStateReadOnlyList AllChildren0 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Replace(LeafPathInner, ReplacementIndex0, out IWriteableBrowsingChildIndex NewItemIndex0);
                Assert.That(Controller.Contains(NewItemIndex0));

                Assert.That(LeafPathInner.Count == PathCount);
                Assert.That(LeafPathInner.StateList.Count == PathCount);

                ILayoutPlaceholderNodeState NewItemState0 = (ILayoutPlaceholderNodeState)LeafPathInner.StateList[0];
                Assert.That(NewItemState0.Node == NewItem0);
                Assert.That(NewItemState0.ParentIndex == NewItemIndex0);

                LayoutNodeStateReadOnlyList AllChildren1 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                Leaf NewItem1 = CreateLeaf(Guid.NewGuid());
                ILayoutInsertionExistingBlockNodeIndex ReplacementIndex1 = new LayoutInsertionExistingBlockNodeIndex(RootNode, nameof(Main.LeafBlocks), NewItem1, 0, 0);

                ILayoutBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
                Assert.That(LeafBlocksInner != null);

                ILayoutBlockState BlockState = (ILayoutBlockState)LeafBlocksInner.BlockStateList[0];

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = BlockState.StateList.Count;
                Assert.That(BlockNodeCount == 4);

                Controller.Replace(LeafBlocksInner, ReplacementIndex1, out IWriteableBrowsingChildIndex NewItemIndex1);
                Assert.That(Controller.Contains(NewItemIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount);
                Assert.That(BlockState.StateList.Count == NodeCount);

                ILayoutPlaceholderNodeState NewItemState1 = (ILayoutPlaceholderNodeState)BlockState.StateList[0];
                Assert.That(NewItemState1.Node == NewItem1);
                Assert.That(NewItemState1.ParentIndex == NewItemIndex1);

                LayoutNodeStateReadOnlyList AllChildren2 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                ILayoutPlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as ILayoutPlaceholderInner;
                Assert.That(PlaceholderTreeInner != null);

                ILayoutBrowsingPlaceholderNodeIndex ExistingIndex2 = PlaceholderTreeInner.ChildState.ParentIndex as ILayoutBrowsingPlaceholderNodeIndex;

                Tree NewItem2 = CreateTree();
                ILayoutInsertionPlaceholderNodeIndex ReplacementIndex2;
                ReplacementIndex2 = ExistingIndex2.ToInsertionIndex(RootNode, NewItem2) as ILayoutInsertionPlaceholderNodeIndex;

                Controller.Replace(PlaceholderTreeInner, ReplacementIndex2, out IWriteableBrowsingChildIndex NewItemIndex2);
                Assert.That(Controller.Contains(NewItemIndex2));

                ILayoutPlaceholderNodeState NewItemState2 = PlaceholderTreeInner.ChildState as ILayoutPlaceholderNodeState;
                Assert.That(NewItemState2.Node == NewItem2);
                Assert.That(NewItemState2.ParentIndex == NewItemIndex2);

                ILayoutBrowsingPlaceholderNodeIndex DuplicateExistingIndex2 = ReplacementIndex2.ToBrowsingIndex() as ILayoutBrowsingPlaceholderNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex2 as ILayoutBrowsingPlaceholderNodeIndex, DuplicateExistingIndex2));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex2, NewItemIndex2 as ILayoutBrowsingPlaceholderNodeIndex));

                LayoutNodeStateReadOnlyList AllChildren3 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count, $"New count: {AllChildren3.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                ILayoutPlaceholderInner PlaceholderLeafInner = NewItemState2.PropertyToInner(nameof(Tree.Placeholder)) as ILayoutPlaceholderInner;
                Assert.That(PlaceholderLeafInner != null);

                ILayoutBrowsingPlaceholderNodeIndex ExistingIndex3 = PlaceholderLeafInner.ChildState.ParentIndex as ILayoutBrowsingPlaceholderNodeIndex;

                Leaf NewItem3 = CreateLeaf(Guid.NewGuid());
                ILayoutInsertionPlaceholderNodeIndex ReplacementIndex3;
                ReplacementIndex3 = ExistingIndex3.ToInsertionIndex(NewItem2, NewItem3) as ILayoutInsertionPlaceholderNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(ReplacementIndex3, ReplacementIndex3));

                Controller.Replace(PlaceholderLeafInner, ReplacementIndex3, out IWriteableBrowsingChildIndex NewItemIndex3);
                Assert.That(Controller.Contains(NewItemIndex3));

                ILayoutPlaceholderNodeState NewItemState3 = PlaceholderLeafInner.ChildState as ILayoutPlaceholderNodeState;
                Assert.That(NewItemState3.Node == NewItem3);
                Assert.That(NewItemState3.ParentIndex == NewItemIndex3);
                Assert.That(NewItemState3.InnerTable != null);
                Assert.That(NewItemState3.CycleIndexList == null);

                LayoutNodeStateReadOnlyList AllChildren4 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren4.Count == AllChildren3.Count, $"New count: {AllChildren4.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));




                ILayoutOptionalInner OptionalLeafInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as ILayoutOptionalInner;
                Assert.That(OptionalLeafInner != null);

                ILayoutBrowsingOptionalNodeIndex ExistingIndex4 = OptionalLeafInner.ChildState.ParentIndex as ILayoutBrowsingOptionalNodeIndex;

                Leaf NewItem4 = CreateLeaf(Guid.NewGuid());
                ILayoutInsertionOptionalNodeIndex ReplacementIndex4;
                ReplacementIndex4 = ExistingIndex4.ToInsertionIndex(RootNode, NewItem4) as ILayoutInsertionOptionalNodeIndex;
                Assert.That(ReplacementIndex4.ParentNode == RootNode);
                Assert.That(ReplacementIndex4.PropertyName == OptionalLeafInner.PropertyName);
                Assert.That(CompareEqual.CoverIsEqual(ReplacementIndex4, ReplacementIndex4));

                Controller.Replace(OptionalLeafInner, ReplacementIndex4, out IWriteableBrowsingChildIndex NewItemIndex4);
                Assert.That(Controller.Contains(NewItemIndex4));

                Assert.That(OptionalLeafInner.IsAssigned);
                ILayoutOptionalNodeState NewItemState4 = OptionalLeafInner.ChildState as ILayoutOptionalNodeState;
                Assert.That(NewItemState4.Node == NewItem4);
                Assert.That(NewItemState4.ParentIndex == NewItemIndex4);
                Assert.That(NewItemState4.InnerTable != null);
                Assert.That(NewItemState4.CycleIndexList == null);

                ILayoutBrowsingOptionalNodeIndex DuplicateExistingIndex4 = ReplacementIndex4.ToBrowsingIndex() as ILayoutBrowsingOptionalNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex4 as ILayoutBrowsingOptionalNodeIndex, DuplicateExistingIndex4));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex4, NewItemIndex4 as ILayoutBrowsingOptionalNodeIndex));

                LayoutNodeStateReadOnlyList AllChildren5 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren5.Count == AllChildren4.Count, $"New count: {AllChildren5.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                ILayoutBrowsingOptionalNodeIndex ExistingIndex5 = OptionalLeafInner.ChildState.ParentIndex as ILayoutBrowsingOptionalNodeIndex;

                //System.Diagnostics.Debug.Assert(false);
                Leaf NewItem5 = CreateLeaf(Guid.NewGuid());
                ILayoutInsertionOptionalClearIndex ReplacementIndex5;
                ReplacementIndex5 = ExistingIndex5.ToInsertionIndex(RootNode, null) as ILayoutInsertionOptionalClearIndex;
                Assert.That(ReplacementIndex5.ParentNode == RootNode);
                Assert.That(ReplacementIndex5.PropertyName == OptionalLeafInner.PropertyName);
                Assert.That(CompareEqual.CoverIsEqual(ReplacementIndex5, ReplacementIndex5));

                Controller.Replace(OptionalLeafInner, ReplacementIndex5, out IWriteableBrowsingChildIndex NewItemIndex5);
                Assert.That(Controller.Contains(NewItemIndex5));

                Assert.That(!OptionalLeafInner.IsAssigned);
                ILayoutOptionalNodeState NewItemState5 = OptionalLeafInner.ChildState as ILayoutOptionalNodeState;
                Assert.That(NewItemState5.ParentIndex == NewItemIndex5);

                ILayoutBrowsingOptionalNodeIndex DuplicateExistingIndex5 = ReplacementIndex5.ToBrowsingIndex() as ILayoutBrowsingOptionalNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex5 as ILayoutBrowsingOptionalNodeIndex, DuplicateExistingIndex5));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex5, NewItemIndex5 as ILayoutBrowsingOptionalNodeIndex));

                LayoutNodeStateReadOnlyList AllChildren6 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren6.Count == AllChildren5.Count - 1, $"New count: {AllChildren6.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                //System.Diagnostics.Debug.Assert(false);
                ILayoutOptionalInner OptionalTreeInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalTree)) as ILayoutOptionalInner;
                Assert.That(OptionalTreeInner != null);
                Assert.That(OptionalTreeInner.IsAssigned);

                ILayoutOptionalNodeState OptionalTreeState = OptionalTreeInner.ChildState;

                ILayoutPlaceholderInner OptionalTreePlaceholderInner = OptionalTreeState.PropertyToInner(nameof(Tree.Placeholder)) as ILayoutPlaceholderInner;
                Assert.That(OptionalTreePlaceholderInner != null);

                ILayoutBrowsingPlaceholderNodeIndex ExistingIndex6 = OptionalTreePlaceholderInner.ChildState.ParentIndex as ILayoutBrowsingPlaceholderNodeIndex;

                Leaf NewItem6 = CreateLeaf(Guid.NewGuid());
                ILayoutInsertionPlaceholderNodeIndex ReplacementIndex6;
                ReplacementIndex6 = ExistingIndex6.ToInsertionIndex(OptionalTreeState.Node, NewItem6) as ILayoutInsertionPlaceholderNodeIndex;
                Assert.That(ReplacementIndex6.ParentNode == OptionalTreeState.Node);
                Assert.That(ReplacementIndex6.PropertyName == OptionalTreePlaceholderInner.PropertyName);

                Controller.Replace(OptionalTreePlaceholderInner, ReplacementIndex6, out IWriteableBrowsingChildIndex NewItemIndex6);
                Assert.That(Controller.Contains(NewItemIndex5));

                ILayoutPlaceholderNodeState NewItemState6 = OptionalTreePlaceholderInner.ChildState as ILayoutPlaceholderNodeState;
                Assert.That(NewItemState6.ParentIndex == NewItemIndex6);

                LayoutNodeStateReadOnlyList AllChildren7 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren7.Count == AllChildren6.Count, $"New count: {AllChildren7.Count}");

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
        public static void LayoutAssign()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            //System.Diagnostics.Debug.Assert(false);
            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                ILayoutOptionalInner UnassignedOptionalLeafInner = RootState.PropertyToInner(nameof(Main.UnassignedOptionalLeaf)) as ILayoutOptionalInner;
                Assert.That(UnassignedOptionalLeafInner != null);
                Assert.That(!UnassignedOptionalLeafInner.IsAssigned);

                ILayoutBrowsingOptionalNodeIndex AssignmentIndex0 = UnassignedOptionalLeafInner.ChildState.ParentIndex;
                Assert.That(AssignmentIndex0 != null);

                LayoutNodeStateReadOnlyList AllChildren0 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Assign(AssignmentIndex0, out bool IsChanged);
                Assert.That(IsChanged);
                Assert.That(UnassignedOptionalLeafInner.IsAssigned);

                LayoutNodeStateReadOnlyList AllChildren1 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Assign(AssignmentIndex0, out IsChanged);
                Assert.That(!IsChanged);
                Assert.That(UnassignedOptionalLeafInner.IsAssigned);

                LayoutNodeStateReadOnlyList AllChildren2 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Controller.Unassign(AssignmentIndex0, out IsChanged);
                Assert.That(IsChanged);
                Assert.That(!UnassignedOptionalLeafInner.IsAssigned);

                LayoutNodeStateReadOnlyList AllChildren3 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void LayoutUnassign()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                ILayoutOptionalInner AssignedOptionalLeafInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as ILayoutOptionalInner;
                Assert.That(AssignedOptionalLeafInner != null);
                Assert.That(AssignedOptionalLeafInner.IsAssigned);

                ILayoutBrowsingOptionalNodeIndex AssignmentIndex0 = AssignedOptionalLeafInner.ChildState.ParentIndex;
                Assert.That(AssignmentIndex0 != null);

                LayoutNodeStateReadOnlyList AllChildren0 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Unassign(AssignmentIndex0, out bool IsChanged);
                Assert.That(IsChanged);
                Assert.That(!AssignedOptionalLeafInner.IsAssigned);

                LayoutNodeStateReadOnlyList AllChildren1 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count - 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Unassign(AssignmentIndex0, out IsChanged);
                Assert.That(!IsChanged);
                Assert.That(!AssignedOptionalLeafInner.IsAssigned);

                LayoutNodeStateReadOnlyList AllChildren2 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Controller.Assign(AssignmentIndex0, out IsChanged);
                Assert.That(IsChanged);
                Assert.That(AssignedOptionalLeafInner.IsAssigned);

                LayoutNodeStateReadOnlyList AllChildren3 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void LayoutChangeReplication()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                //System.Diagnostics.Debug.Assert(false);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                ILayoutBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
                Assert.That(LeafBlocksInner != null);

                LayoutNodeStateReadOnlyList AllChildren0 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                ILayoutBlockState BlockState = (ILayoutBlockState)LeafBlocksInner.BlockStateList[0];
                Assert.That(BlockState != null);
                Assert.That(BlockState.ParentInner == LeafBlocksInner);
                BaseNode.IBlock ChildBlock = BlockState.ChildBlock;
                Assert.That(ChildBlock.Replication == BaseNode.ReplicationStatus.Normal);

                Controller.ChangeReplication(LeafBlocksInner, 0, BaseNode.ReplicationStatus.Replicated);

                Assert.That(ChildBlock.Replication == BaseNode.ReplicationStatus.Replicated);

                LayoutNodeStateReadOnlyList AllChildren1 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void LayoutSplit()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                ILayoutBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
                Assert.That(LeafBlocksInner != null);

                LayoutNodeStateReadOnlyList AllChildren0 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                ILayoutBlockState BlockState0 = (ILayoutBlockState)LeafBlocksInner.BlockStateList[0];
                Assert.That(BlockState0 != null);
                BaseNode.IBlock ChildBlock0 = BlockState0.ChildBlock;
                Assert.That(ChildBlock0.NodeList.Count == 1);

                ILayoutBlockState BlockState1 = (ILayoutBlockState)LeafBlocksInner.BlockStateList[1];
                Assert.That(BlockState1 != null);
                BaseNode.IBlock ChildBlock1 = BlockState1.ChildBlock;
                Assert.That(ChildBlock1.NodeList.Count == 2);

                Assert.That(LeafBlocksInner.Count == 4);
                Assert.That(LeafBlocksInner.BlockStateList.Count == 3);

                ILayoutBrowsingExistingBlockNodeIndex SplitIndex0 = LeafBlocksInner.IndexAt(1, 1) as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.IsSplittable(LeafBlocksInner, SplitIndex0));

                Controller.SplitBlock(LeafBlocksInner, SplitIndex0);

                Assert.That(LeafBlocksInner.BlockStateList.Count == 4);
                Assert.That(ChildBlock0 == LeafBlocksInner.BlockStateList[0].ChildBlock);
                Assert.That(ChildBlock1 == LeafBlocksInner.BlockStateList[2].ChildBlock);
                Assert.That(ChildBlock1.NodeList.Count == 1);

                ILayoutBlockState BlockState12 = (ILayoutBlockState)LeafBlocksInner.BlockStateList[1];
                Assert.That(BlockState12.ChildBlock.NodeList.Count == 1);

                LayoutNodeStateReadOnlyList AllChildren1 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void LayoutMerge()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                ILayoutBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
                Assert.That(LeafBlocksInner != null);

                LayoutNodeStateReadOnlyList AllChildren0 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                ILayoutBlockState BlockState0 = (ILayoutBlockState)LeafBlocksInner.BlockStateList[0];
                Assert.That(BlockState0 != null);
                BaseNode.IBlock ChildBlock0 = BlockState0.ChildBlock;
                Assert.That(ChildBlock0.NodeList.Count == 1);

                ILayoutBlockState BlockState1 = (ILayoutBlockState)LeafBlocksInner.BlockStateList[1];
                Assert.That(BlockState1 != null);
                BaseNode.IBlock ChildBlock1 = BlockState1.ChildBlock;
                Assert.That(ChildBlock1.NodeList.Count == 2);

                Assert.That(LeafBlocksInner.Count == 4);

                ILayoutBrowsingExistingBlockNodeIndex MergeIndex0 = LeafBlocksInner.IndexAt(1, 0) as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.IsMergeable(LeafBlocksInner, MergeIndex0));

                Assert.That(LeafBlocksInner.BlockStateList.Count == 3);

                Controller.MergeBlocks(LeafBlocksInner, MergeIndex0);

                Assert.That(LeafBlocksInner.BlockStateList.Count == 2);
                Assert.That(ChildBlock1 == LeafBlocksInner.BlockStateList[0].ChildBlock);
                Assert.That(ChildBlock1.NodeList.Count == 3);

                Assert.That(LeafBlocksInner.BlockStateList[0] == BlockState1);

                LayoutNodeStateReadOnlyList AllChildren1 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void LayoutExpand()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                LayoutNodeStateReadOnlyList AllChildren0 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Expand(RootIndex, out bool IsChanged);
                Assert.That(IsChanged);

                LayoutNodeStateReadOnlyList AllChildren1 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 2, $"New count: {AllChildren1.Count - AllChildren0.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(!IsChanged);

                LayoutNodeStateReadOnlyList AllChildren2 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                ILayoutOptionalInner OptionalLeafInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as ILayoutOptionalInner;
                Assert.That(OptionalLeafInner != null);

                ILayoutInsertionOptionalClearIndex ReplacementIndex5 = new LayoutInsertionOptionalClearIndex(RootNode, nameof(Main.AssignedOptionalLeaf));

                Controller.Replace(OptionalLeafInner, ReplacementIndex5, out IWriteableBrowsingChildIndex NewItemIndex5);
                Assert.That(Controller.Contains(NewItemIndex5));

                LayoutNodeStateReadOnlyList AllChildren3 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count - 1, $"New count: {AllChildren3.Count - AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                LayoutNodeStateReadOnlyList AllChildren4 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren4.Count == AllChildren3.Count + 1, $"New count: {AllChildren4.Count}");



                ILayoutBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
                Assert.That(LeafBlocksInner != null);

                ILayoutBrowsingExistingBlockNodeIndex RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                LayoutNodeStateReadOnlyList AllChildren5 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren5.Count == AllChildren4.Count - 10, $"New count: {AllChildren5.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(LeafBlocksInner.IsEmpty);

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(!IsChanged);

                LayoutNodeStateReadOnlyList AllChildren6 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren6.Count == AllChildren5.Count, $"New count: {AllChildren6.Count - AllChildren5.Count}");

                IDictionary<Type, string[]> WithExpandCollectionTable = BaseNodeHelper.NodeHelper.WithExpandCollectionTable as IDictionary<Type, string[]>;
                WithExpandCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafBlocks) });

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                LayoutNodeStateReadOnlyList AllChildren7 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void LayoutReduce()
        {
            ControllerTools.ResetExpectedName();

            CoverageLayoutFrame ForCoverage = new CoverageLayoutFrame();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;
            bool IsChanged;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                ILayoutBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
                Assert.That(LeafBlocksInner != null);

                ILayoutBrowsingExistingBlockNodeIndex RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(LeafBlocksInner.IsEmpty);

                LayoutNodeStateReadOnlyList AllChildren0 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 9, $"New count: {AllChildren0.Count}");

                IDictionary<Type, string[]> WithExpandCollectionTable = BaseNodeHelper.NodeHelper.WithExpandCollectionTable as IDictionary<Type, string[]>;
                WithExpandCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafBlocks) });

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                LayoutNodeStateReadOnlyList AllChildren1 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 5, $"New count: {AllChildren1.Count - AllChildren0.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                //System.Diagnostics.Debug.Assert(false);
                IFrameOptionalInner LeafOptionalInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IFrameOptionalInner;
                Assert.That(LeafOptionalInner != null);

                Leaf Leaf = LeafOptionalInner.ChildState.Node as Leaf;
                BaseNodeHelper.NodeTreeHelper.SetStringProperty(Leaf, "Text", "");


                //System.Diagnostics.Debug.Assert(false);
                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                LayoutNodeStateReadOnlyList AllChildren2 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count - 5, $"New count: {AllChildren2.Count - AllChildren1.Count}");

                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(!IsChanged);

                LayoutNodeStateReadOnlyList AllChildren3 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count, $"New count: {AllChildren3.Count - AllChildren2.Count}");

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                LayoutNodeStateReadOnlyList AllChildren4 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren4.Count == AllChildren3.Count + 5, $"New count: {AllChildren4.Count - AllChildren3.Count}");

                BaseNode.IBlock ChildBlock = LeafBlocksInner.BlockStateList[0].ChildBlock;
                Leaf FirstNode = ChildBlock.NodeList[0] as Leaf;
                Assert.That(FirstNode != null);
                BaseNodeHelper.NodeTreeHelper.SetString(FirstNode, nameof(Leaf.Text), "!");

                //System.Diagnostics.Debug.Assert(false);
                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                LayoutNodeStateReadOnlyList AllChildren5 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren5.Count == AllChildren4.Count - 2, $"New count: {AllChildren5.Count - AllChildren4.Count}");

                BaseNodeHelper.NodeTreeHelper.SetString(FirstNode, nameof(Leaf.Text), "");

                //System.Diagnostics.Debug.Assert(false);
                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                LayoutNodeStateReadOnlyList AllChildren6 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren6.Count == AllChildren5.Count - 3, $"New count: {AllChildren6.Count - AllChildren5.Count}");

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                WithExpandCollectionTable.Remove(Type.FromTypeof<Main>());

                //System.Diagnostics.Debug.Assert(false);
                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                LayoutNodeStateReadOnlyList AllChildren7 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void LayoutCanonicalize()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;
            bool IsChanged;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                //System.Diagnostics.Debug.Assert(false);
                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                ILayoutBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
                Assert.That(LeafBlocksInner != null);

                ILayoutBrowsingExistingBlockNodeIndex RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                Assert.That(Controller.CanUndo);
                LayoutOperationGroup LastOperation = (LayoutOperationGroup)Controller.OperationStack[Controller.RedoIndex - 1];
                Assert.That(LastOperation.MainOperation is ILayoutRemoveOperation);
                Assert.That(LastOperation.OperationList.Count > 0);
                Assert.That(LastOperation.Refresh == null);

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as ILayoutBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(LeafBlocksInner.IsEmpty);

                ILayoutListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as ILayoutListInner;
                Assert.That(LeafPathInner != null);
                Assert.That(LeafPathInner.Count == 2);

                ILayoutBrowsingListNodeIndex RemovedListLeafIndex = LeafPathInner.StateList[0].ParentIndex as ILayoutBrowsingListNodeIndex;
                Assert.That(Controller.Contains(RemovedListLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafPathInner, RemovedListLeafIndex));

                Controller.Remove(LeafPathInner, RemovedListLeafIndex);
                Assert.That(!Controller.Contains(RemovedListLeafIndex));

                IDictionary<Type, string[]> NeverEmptyCollectionTable = BaseNodeHelper.NodeHelper.NeverEmptyCollectionTable as IDictionary<Type, string[]>;
                NeverEmptyCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.PlaceholderTree) });

                RemovedListLeafIndex = LeafPathInner.StateList[0].ParentIndex as ILayoutBrowsingListNodeIndex;
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

                LayoutNodeStateReadOnlyList AllChildren0 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 12, $"New count: {AllChildren0.Count}");

                IDictionary<Type, string[]> WithExpandCollectionTable = BaseNodeHelper.NodeHelper.WithExpandCollectionTable as IDictionary<Type, string[]>;
                WithExpandCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafBlocks) });

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                LayoutNodeStateReadOnlyList AllChildren1 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 2, $"New count: {AllChildren1.Count - AllChildren0.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                //System.Diagnostics.Debug.Assert(false);
                IFrameOptionalInner LeafOptionalInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IFrameOptionalInner;
                Assert.That(LeafOptionalInner != null);

                Leaf Leaf = LeafOptionalInner.ChildState.Node as Leaf;
                BaseNodeHelper.NodeTreeHelper.SetStringProperty(Leaf, "Text", "");

                Controller.Canonicalize(out IsChanged);
                Assert.That(IsChanged);

                LayoutNodeStateReadOnlyList AllChildren2 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count - 2, $"New count: {AllChildren2.Count - AllChildren1.Count}");

                Controller.Undo();
                Controller.Redo();

                Controller.Canonicalize(out IsChanged);
                Assert.That(!IsChanged);

                LayoutNodeStateReadOnlyList AllChildren3 = (LayoutNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void LayoutReplaceWithCycle()
        {
            ControllerTools.ResetExpectedName();

            BaseNode.Class RootNode;
            ILayoutRootNodeIndex RootIndex;

            RootNode = BaseNodeHelper.NodeHelper.CreateSimpleClass("Class!");

            BaseNode.FunctionFeature FunctionFeature = BaseNodeHelper.NodeHelper.CreateEmptyFunctionFeature();
            BaseNode.PropertyFeature PropertyFeature = BaseNodeHelper.NodeHelper.CreateEmptyPropertyFeature();
            ((BaseNode.PropertyFeature)PropertyFeature).PropertyKind = BaseNode.UtilityType.WriteOnly;
            ((BaseNode.PropertyFeature)PropertyFeature).GetterBody.Assign();
            ((BaseNode.PropertyFeature)PropertyFeature).SetterBody.Assign();

            ((BaseNode.Class)RootNode).FeatureBlocks = BaseNodeHelper.BlockListHelper.CreateSimpleBlockList<BaseNode.Feature>(FunctionFeature);
            RootNode.FeatureBlocks.NodeBlockList[0].NodeList.Add(PropertyFeature);

            BaseNode.CommandInstruction FunctionFirstInstruction = BaseNodeHelper.NodeHelper.CreateSimpleCommandInstruction("test!") as BaseNode.CommandInstruction;
            BaseNode.FunctionFeature FirstFeature = (BaseNode.FunctionFeature)RootNode.FeatureBlocks.NodeBlockList[0].NodeList[0];
            BaseNode.QueryOverload FirstOverload = FirstFeature.OverloadBlocks.NodeBlockList[0].NodeList[0];
            BaseNode.EffectiveBody FirstOverloadBody = (BaseNode.EffectiveBody)FirstOverload.QueryBody;
            FirstOverloadBody.BodyInstructionBlocks = BaseNodeHelper.BlockListHelper.CreateSimpleBlockList<BaseNode.Instruction>(FunctionFirstInstruction);

            BaseNode.CommandInstruction PropertyFirstInstruction = BaseNodeHelper.NodeHelper.CreateSimpleCommandInstruction("test?") as BaseNode.CommandInstruction;
            BaseNode.EffectiveBody PropertyBody = ((BaseNode.PropertyFeature)PropertyFeature).GetterBody.Item as BaseNode.EffectiveBody;
            PropertyBody.BodyInstructionBlocks = BaseNodeHelper.BlockListHelper.CreateSimpleBlockList<BaseNode.Instruction>(PropertyFirstInstruction);

            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);
            bool IsMoved;

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                IFocusCyclableNodeState State;
                int CyclePosition;
                bool IsItemCyclableThrough;

                ControllerView0.SetCaretPosition(0, true, out IsMoved);
                Assert.That(!IsMoved);

                ControllerView0.SetCaretPosition(-1, true, out IsMoved);
                Assert.That(!IsMoved);

                ControllerView0.SetCaretPosition(1000, true, out IsMoved);
                Assert.That(IsMoved);

                ControllerView0.SetCaretPosition(1, true, out IsMoved);
                Assert.That(IsMoved);

                IsItemCyclableThrough = ControllerView0.IsItemCyclableThrough(out State, out CyclePosition);
                Assert.That(!IsItemCyclableThrough);

                while (ControllerView0.MaxFocusMove > 0 && !(ControllerView0.Focus.CellView.StateView.State.Node is BaseNode.FunctionFeature))
                    ControllerView0.MoveFocus(+1, true, out IsMoved);

                ILayoutNodeStateView StateView = ControllerView0.Focus.CellView.StateView;
                //Assert.That(ControllerView0.CollectionHasItems(StateView, nameof(BaseNode.IFunctionFeature.OverloadBlocks), 0));
                //Assert.That(ControllerView0.IsFirstItem(StateView));

                ILayoutNodeState CurrentState = StateView.State;
                Assert.That(CurrentState != null && CurrentState.Node is BaseNode.Feature);

                LayoutInsertionChildNodeIndexList CycleIndexList;
                int FeatureCycleCount = 14;
                IFocusBrowsingChildIndex NewItemIndex0;

                ControllerView0.SetUserVisible(true);
                ControllerView0.SetUserVisible(false);

                for (int i = 0; i < FeatureCycleCount; i++)
                {
                    IsItemCyclableThrough = ControllerView0.IsItemCyclableThrough(out State, out CyclePosition);
                    Assert.That(IsItemCyclableThrough);

                    CycleIndexList = ((ILayoutCyclableNodeState)State).CycleIndexList as LayoutInsertionChildNodeIndexList;

                    CyclePosition = (CyclePosition + 1) % CycleIndexList.Count;
                    Controller.Replace(State.ParentInner, CycleIndexList, CyclePosition, out NewItemIndex0);

                    ILayoutInsertionChildNodeIndex FirstInsertionChildNodeIndex = (ILayoutInsertionChildNodeIndex)CycleIndexList[0];

                    FocusInsertionChildNodeIndexList FocusInsertionChildNodeIndexList = CycleIndexList;
                    Assert.That(FocusInsertionChildNodeIndexList.Contains(FirstInsertionChildNodeIndex));
                    Assert.That(FocusInsertionChildNodeIndexList[0] == FirstInsertionChildNodeIndex);
                    Assert.That(FocusInsertionChildNodeIndexList.IndexOf(FirstInsertionChildNodeIndex) == 0);
                    IList<IFocusInsertionChildNodeIndex> FocusInsertionChildNodeIndexListAsList = FocusInsertionChildNodeIndexList;
                    Assert.That(FocusInsertionChildNodeIndexListAsList.Contains(FirstInsertionChildNodeIndex));
                    Assert.That(FocusInsertionChildNodeIndexListAsList[0] == FirstInsertionChildNodeIndex);
                    Assert.That(FocusInsertionChildNodeIndexListAsList.IndexOf(FirstInsertionChildNodeIndex) == 0);
                    ICollection<IFocusInsertionChildNodeIndex> FocusInsertionChildNodeIndexListAsCollection = FocusInsertionChildNodeIndexList;
                    Assert.That(!FocusInsertionChildNodeIndexListAsCollection.IsReadOnly);
                    Assert.That(FocusInsertionChildNodeIndexListAsCollection.Contains(FirstInsertionChildNodeIndex));
                    FocusInsertionChildNodeIndexListAsCollection.Remove(FirstInsertionChildNodeIndex);
                    FocusInsertionChildNodeIndexListAsList.Remove(FirstInsertionChildNodeIndex);
                    FocusInsertionChildNodeIndexListAsList.Insert(0, FirstInsertionChildNodeIndex);
                    FocusInsertionChildNodeIndexListAsCollection.CopyTo(new ILayoutInsertionChildNodeIndex[FocusInsertionChildNodeIndexListAsCollection.Count], 0);
                    IEnumerable<IFocusInsertionChildNodeIndex> FocusInsertionChildNodeIndexListAsEnumerable = FocusInsertionChildNodeIndexList;
                    FocusInsertionChildNodeIndexListAsEnumerable.GetEnumerator();
                    IReadOnlyList<IFocusInsertionChildNodeIndex> FocusInsertionChildNodeIndexListAsReadOnlyList = FocusInsertionChildNodeIndexList;
                    Assert.That(FocusInsertionChildNodeIndexListAsReadOnlyList[0] == FirstInsertionChildNodeIndex);
                }

                //System.Diagnostics.Debug.Assert(false);
                LayoutOperationGroupList LayoutOperationGroupList = DebugObjects.GetReferenceByInterface(Type.FromTypeof<LayoutOperationGroupList>()) as LayoutOperationGroupList;
                if (LayoutOperationGroupList != null)
                {
                    Assert.That(LayoutOperationGroupList.Count > 0);
                    LayoutOperationGroup FirstOperationGroup = (LayoutOperationGroup)LayoutOperationGroupList[0];
                    Assert.That(FirstOperationGroup.OperationList.Count > 0);
                    ILayoutReplaceWithCycleOperation FirstOperation = FirstOperationGroup.OperationList[0] as ILayoutReplaceWithCycleOperation;
                    Assert.That(FirstOperation != null);
                    Assert.That(FirstOperation.CycleIndexList != null);
                }

                for (int i = 0; i < FeatureCycleCount; i++)
                {
                    Assert.That(Controller.CanUndo);
                    Controller.Undo();
                }

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));

                int BodyCycleCount = 8;

                for (int i = 0; i < BodyCycleCount; i++)
                {
                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);

                    while (ControllerView0.MaxFocusMove > 0)
                    {
                        if (ControllerView0.Focus.CellView.StateView.State.Node is BaseNode.Identifier AsIdentifier && AsIdentifier.Text == FunctionFirstInstruction.Command.Path[0].Text)
                            break;

                        if (ControllerView0.Focus.CellView.Frame is ILayoutKeywordFrame AsLayoutableKeywordFrame && (AsLayoutableKeywordFrame.Text == "deferred" || AsLayoutableKeywordFrame.Text == "extern" || AsLayoutableKeywordFrame.Text == "precursor"))
                            break;

                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }

                    StateView = ControllerView0.Focus.CellView.StateView;
                    CurrentState = StateView.State;
                    if (CurrentState.Node is BaseNode.Identifier AsStateIdentifier && AsStateIdentifier.Text == FunctionFirstInstruction.Command.Path[0].Text)
                    {
                        /*
                        Assert.That(ControllerView0.IsFirstItem(StateView));

                        ILayoutNodeState ParentState = CurrentState.ParentState;
                        Assert.That(ControllerView0.StateViewTable.ContainsKey(ParentState));
                        ILayoutNodeStateView ParentStateView = ControllerView0.StateViewTable[ParentState];
                        Assert.That(ControllerView0.CollectionHasItems(ParentStateView, nameof(BaseNode.IQualifiedName.Path), 0));
                        */
                    }

                    IsItemCyclableThrough = ControllerView0.IsItemCyclableThrough(out State, out CyclePosition);
                    Assert.That(IsItemCyclableThrough);

                    CycleIndexList = State.CycleIndexList as LayoutInsertionChildNodeIndexList;

                    CyclePosition = (CyclePosition + 1) % CycleIndexList.Count;
                    Controller.Replace(State.ParentInner, CycleIndexList, CyclePosition, out NewItemIndex0);
                }

                for (int i = 0; i < BodyCycleCount; i++)
                {
                    Assert.That(Controller.CanUndo);
                    Controller.Undo();
                }

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));

                for (int i = 0; i < BodyCycleCount; i++)
                {
                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);

                    while (ControllerView0.MaxFocusMove > 0)
                    {
                        if (ControllerView0.Focus.CellView.StateView.State.Node is BaseNode.Identifier AsIdentifier && AsIdentifier.Text == PropertyFirstInstruction.Command.Path[0].Text)
                            break;

                        if (ControllerView0.Focus.CellView.Frame is ILayoutKeywordFrame AsLayoutableKeywordFrame && (AsLayoutableKeywordFrame.Text == "deferred" || AsLayoutableKeywordFrame.Text == "extern" || AsLayoutableKeywordFrame.Text == "precursor"))
                            break;

                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }

                    StateView = ControllerView0.Focus.CellView.StateView;
                    CurrentState = StateView.State;
                    if (CurrentState.Node is BaseNode.Identifier AsStateIdentifier && AsStateIdentifier.Text == PropertyFirstInstruction.Command.Path[0].Text)
                    {
                        /*
                        Assert.That(ControllerView0.IsFirstItem(StateView));

                        ILayoutNodeState ParentState = CurrentState.ParentState;
                        Assert.That(ControllerView0.StateViewTable.ContainsKey(ParentState));
                        ILayoutNodeStateView ParentStateView = ControllerView0.StateViewTable[ParentState];
                        Assert.That(ControllerView0.CollectionHasItems(ParentStateView, nameof(BaseNode.IQualifiedName.Path), 0));
                        */
                    }

                    IsItemCyclableThrough = ControllerView0.IsItemCyclableThrough(out State, out CyclePosition);
                    Assert.That(IsItemCyclableThrough);

                    CycleIndexList = State.CycleIndexList as LayoutInsertionChildNodeIndexList;

                    CyclePosition = (CyclePosition + 1) % CycleIndexList.Count;
                    Controller.Replace(State.ParentInner, CycleIndexList, CyclePosition, out NewItemIndex0);
                }

                for (int i = 0; i < BodyCycleCount; i++)
                {
                    Assert.That(Controller.CanUndo);
                    Controller.Undo();
                }

                ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                Assert.That(ControllerView0.MinFocusMove == 0);

                int MaxIdentifierSplit = 10;
                int MaxIdentifierMerge = 10;
                int IdentifierSplitCount = 0;
                int IdentifierMergeCount = 0;
                ControllerView0.SetUserVisible(true);

                //System.Diagnostics.Debug.Assert(false);

                while (ControllerView0.MaxFocusMove > 0)
                {
                    IFocusInner Inner;
                    IFocusInsertionChildIndex InsertionIndex;
                    IFocusCollectionInner CollectionInner;
                    IFocusBlockListInner BlockListInner;
                    IFocusListInner ListInner;
                    IFocusInsertionCollectionNodeIndex InsertionCollectionIndex;
                    IFocusBrowsingCollectionNodeIndex BrowsingCollectionIndex;
                    IFocusBrowsingExistingBlockNodeIndex ExistingBlockNodeIndex;
                    IFocusInsertionListNodeIndex ReplacementListNodeIndex, InsertionListNodeIndex;
                    int BlockIndex;
                    BaseNode.ReplicationStatus Replication;

                    bool IsUserVisible = ControllerView0.IsUserVisible;
                    bool IsNewItemInsertable = ControllerView0.IsNewItemInsertable(out CollectionInner, out InsertionCollectionIndex);
                    bool IsItemRemoveable = ControllerView0.IsItemRemoveable(out CollectionInner, out BrowsingCollectionIndex);
                    bool IsItemMoveable = ControllerView0.IsItemMoveable(-1, out CollectionInner, out BrowsingCollectionIndex);
                    bool IsItemSplittable = ControllerView0.IsItemSplittable(out BlockListInner, out ExistingBlockNodeIndex);
                    bool IsReplicationModifiable = ControllerView0.IsReplicationModifiable(out BlockListInner, out BlockIndex, out Replication);
                    bool IsItemMergeable = ControllerView0.IsItemMergeable(out BlockListInner, out ExistingBlockNodeIndex);
                    bool IsBlockMoveable = ControllerView0.IsBlockMoveable(-1, out BlockListInner, out BlockIndex);

                    bool IsItemSimplifiable = ControllerView0.IsItemSimplifiable(out Inner, out InsertionIndex);
                    if (IsItemSimplifiable && IdentifierMergeCount++ < MaxIdentifierMerge)
                    {
                        ControllerView0.Controller.Replace(Inner, InsertionIndex, out IWriteableBrowsingChildIndex nodeIndex);
                    }

                    bool IsItemComplexifiable = ControllerView0.IsItemComplexifiable(out IDictionary<IFocusInner, IList<IFocusInsertionChildNodeIndex>> IndexTable);
                    bool IsIdentifierSplittable = ControllerView0.IsIdentifierSplittable(out ListInner, out ReplacementListNodeIndex, out InsertionListNodeIndex);
                    if (IsIdentifierSplittable && IdentifierSplitCount++ < MaxIdentifierSplit)
                    {
                        ControllerView0.Controller.Replace(ListInner, ReplacementListNodeIndex, out IWriteableBrowsingChildIndex FirstIndex);
                        ControllerView0.Controller.Insert(ListInner, InsertionListNodeIndex, out IWriteableBrowsingCollectionNodeIndex SecondIndex);
                    }

                    ControllerView0.MoveFocus(+1, true, out IsMoved);
                }
            }
        }

        [Test]
        [Category("Coverage")]
        public static void LayoutSimplify()
        {
            ControllerTools.ResetExpectedName();

            BaseNode.Class RootNode;
            ILayoutRootNodeIndex RootIndex;
            bool IsMoved;
            RootNode = BaseNodeHelper.NodeHelper.CreateSimpleClass("Class!");

            BaseNode.FunctionFeature FunctionFeature = BaseNodeHelper.NodeHelper.CreateEmptyFunctionFeature();
            BaseNode.PropertyFeature PropertyFeature = BaseNodeHelper.NodeHelper.CreateEmptyPropertyFeature();
            ((BaseNode.PropertyFeature)PropertyFeature).PropertyKind = BaseNode.UtilityType.WriteOnly;
            ((BaseNode.PropertyFeature)PropertyFeature).GetterBody.Assign();
            ((BaseNode.PropertyFeature)PropertyFeature).SetterBody.Assign();

            ((BaseNode.Class)RootNode).FeatureBlocks = BaseNodeHelper.BlockListHelper.CreateSimpleBlockList<BaseNode.Feature>(FunctionFeature);
            RootNode.FeatureBlocks.NodeBlockList[0].NodeList.Add(PropertyFeature);

            List<BaseNode.Identifier> IdentifierList = new List<BaseNode.Identifier>();
            IdentifierList.Add(BaseNodeHelper.NodeHelper.CreateSimpleIdentifier("test1"));
            IdentifierList.Add(BaseNodeHelper.NodeHelper.CreateSimpleIdentifier("test2"));
            BaseNode.QualifiedName QualifiedName = BaseNodeHelper.NodeHelper.CreateQualifiedName(IdentifierList);
            BaseNode.CommandInstruction FunctionFirstInstruction = BaseNodeHelper.NodeHelper.CreateSimpleCommandInstruction("") as BaseNode.CommandInstruction;
            ((BaseNode.CommandInstruction)FunctionFirstInstruction).Command = QualifiedName;
            BaseNode.FunctionFeature FirstFeature = (BaseNode.FunctionFeature)RootNode.FeatureBlocks.NodeBlockList[0].NodeList[0];
            BaseNode.QueryOverload FirstOverload = FirstFeature.OverloadBlocks.NodeBlockList[0].NodeList[0];
            BaseNode.EffectiveBody FirstOverloadBody = (BaseNode.EffectiveBody)FirstOverload.QueryBody;
            FirstOverloadBody.BodyInstructionBlocks = BaseNodeHelper.BlockListHelper.CreateSimpleBlockList<BaseNode.Instruction>(FunctionFirstInstruction);

            BaseNode.CommandInstruction PropertyFirstInstruction = BaseNodeHelper.NodeHelper.CreateSimpleCommandInstruction("test?") as BaseNode.CommandInstruction;
            BaseNode.EffectiveBody PropertyBody = ((BaseNode.PropertyFeature)PropertyFeature).GetterBody.Item as BaseNode.EffectiveBody;
            PropertyBody.BodyInstructionBlocks = BaseNodeHelper.BlockListHelper.CreateSimpleBlockList<BaseNode.Instruction>(PropertyFirstInstruction);

            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                int MaxFocusMove = ControllerView0.MaxFocusMove;
                bool IsFocused = false;

                for (int i = 0; i < MaxFocusMove; i++)
                {
                    if (ControllerView0.Focus is ILayoutStringContentFocus AsTextFocus)
                    {
                        if (ControllerView0.FocusedText == "test1")
                        {
                            IsFocused = true;
                            break;
                        }
                    }

                    ControllerView0.MoveFocus(+1, true, out IsMoved);
                }

                Assert.That(IsFocused);
                Assert.That(ControllerView0.IsItemSimplifiable(out IFocusInner Inner, out IFocusInsertionChildIndex Index));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void LayoutPrune()
        {
            ControllerTools.ResetExpectedName();
            bool IsMoved;

            Main MainItemH = CreateRoot(ValueGuid0, Imperfections.None);
            Main MainItemV = CreateRoot(ValueGuid1, Imperfections.None);
            BaseNode.Document RootDocument = BaseNodeHelper.NodeHelper.CreateSimpleDocument("root doc", Guid.NewGuid());
            Root RootNode = new Root(RootDocument);
            BaseNode.IBlockList<Main> MainBlocksH = BaseNodeHelper.BlockListHelper.CreateSimpleBlockList<Main>(MainItemH);
            BaseNode.IBlockList<Main> MainBlocksV = BaseNodeHelper.BlockListHelper.CreateSimpleBlockList<Main>(MainItemV);
            BaseNodeHelper.NodeTreeHelperBlockList.SetReplication((BaseNode.IBlock)MainBlocksV.NodeBlockList[0], BaseNode.ReplicationStatus.Replicated);

            Main UnassignedOptionalMain = CreateRoot(ValueGuid2, Imperfections.None);
            Easly.IOptionalReference<Main> UnassignedOptional = BaseNodeHelper.OptionalReferenceHelper.CreateReference<Main>(UnassignedOptionalMain);

            IList<Leaf> LeafPathH = new List<Leaf>();
            Leaf FirstLeafH = CreateLeaf(Guid.NewGuid());
            LeafPathH.Add(FirstLeafH);

            IList<Leaf> LeafPathV = new List<Leaf>();
            Leaf FirstLeafV = CreateLeaf(Guid.NewGuid());
            LeafPathV.Add(FirstLeafV);

            BaseNodeHelper.NodeTreeHelperBlockList.SetBlockList(RootNode, nameof(Root.MainBlocksH), (BaseNode.IBlockList)MainBlocksH);
            BaseNodeHelper.NodeTreeHelperBlockList.SetBlockList(RootNode, nameof(Root.MainBlocksV), (BaseNode.IBlockList)MainBlocksV);
            BaseNodeHelper.NodeTreeHelperOptional.SetOptionalReference(RootNode, nameof(Root.UnassignedOptionalMain), (Easly.IOptionalReference)UnassignedOptional);
            BaseNodeHelper.NodeTreeHelper.SetString(RootNode, nameof(Root.ValueString), "root string");
            BaseNodeHelper.NodeTreeHelperList.SetChildNodeList(RootNode, nameof(Root.LeafPathH), (IList)LeafPathH);
            BaseNodeHelper.NodeTreeHelperList.SetChildNodeList(RootNode, nameof(Root.LeafPathV), (IList)LeafPathV);
            BaseNodeHelper.NodeTreeHelperOptional.SetOptionalReference(RootNode, nameof(Root.UnassignedOptionalLeaf), (Easly.IOptionalReference)BaseNodeHelper.OptionalReferenceHelper.CreateReference(new Leaf()));

            //System.Diagnostics.Debug.Assert(false);
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            System.Windows.IDataObject DataObject = new System.Windows.DataObject();

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ControllerView0.MeasureAndArrange();

                Assert.That(!ControllerView0.ShowLineNumber);
                ControllerView0.SetShowLineNumber(!ControllerView0.ShowLineNumber);
                Assert.That(ControllerView0.ShowLineNumber);

                //System.Diagnostics.Debug.Assert(false);
                ControllerView0.Draw(ControllerView0.RootStateView);
                ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);

                ControllerView0.SetShowLineNumber(!ControllerView0.ShowLineNumber);
                Assert.That(!ControllerView0.ShowLineNumber);

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                ILayoutBlockListInner MainInnerH = RootState.PropertyToInner(nameof(Root.MainBlocksH)) as ILayoutBlockListInner;
                Assert.That(MainInnerH != null);

                ILayoutBlockListInner MainInnerV = RootState.PropertyToInner(nameof(Root.MainBlocksV)) as ILayoutBlockListInner;
                Assert.That(MainInnerV != null);

                ILayoutBrowsingExistingBlockNodeIndex MainIndex = MainInnerH.IndexAt(0, 0) as ILayoutBrowsingExistingBlockNodeIndex;
                Controller.Remove(MainInnerH, MainIndex);

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                MainIndex = MainInnerH.IndexAt(0, 0) as ILayoutBrowsingExistingBlockNodeIndex;
                Controller.Remove(MainInnerH, MainIndex);

                Controller.Undo();
                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));

                MainIndex = MainInnerH.IndexAt(0, 0) as ILayoutBrowsingExistingBlockNodeIndex;
                Controller.Remove(MainInnerH, MainIndex);
                Controller.Undo();

                MainIndex = MainInnerV.IndexAt(0, 0) as ILayoutBrowsingExistingBlockNodeIndex;
                Controller.Remove(MainInnerV, MainIndex);
                Controller.Undo();

                ILayoutListInner LeafInnerH = RootState.PropertyToInner(nameof(Root.LeafPathH)) as ILayoutListInner;
                Assert.That(LeafInnerH != null);

                ILayoutBrowsingListNodeIndex LeafIndexH = LeafInnerH.IndexAt(0) as ILayoutBrowsingListNodeIndex;
                Controller.Remove(LeafInnerH, LeafIndexH);
                Controller.Undo();

                ILayoutListInner LeafInnerV = RootState.PropertyToInner(nameof(Root.LeafPathV)) as ILayoutListInner;
                Assert.That(LeafInnerV != null);

                ILayoutBrowsingListNodeIndex LeafIndexV = LeafInnerV.IndexAt(0) as ILayoutBrowsingListNodeIndex;
                Controller.Remove(LeafInnerV, LeafIndexV);
                Controller.Undo();

                ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                Assert.That(ControllerView0.MinFocusMove == 0);

                ILayoutOptionalInner OptionalMainInner = RootState.PropertyToInner(nameof(Root.UnassignedOptionalMain)) as ILayoutOptionalInner;
                Controller.Assign(OptionalMainInner.ChildState.ParentIndex, out bool IsChanged);
                Assert.That(IsChanged);

                while (ControllerView0.MaxFocusMove > 0)
                {
                    IFocusCollectionInner CollectionInner;
                    IFocusInsertionCollectionNodeIndex InsertionCollectionIndex;

                    bool IsNewItemInsertable = ControllerView0.IsNewItemInsertable(out CollectionInner, out InsertionCollectionIndex);

                    if (ControllerView0.MaxCaretPosition > 0)
                    {
                        ControllerView0.SetCaretPosition(ControllerView0.MaxCaretPosition, true, out IsMoved);
                        IsNewItemInsertable = ControllerView0.IsNewItemInsertable(out CollectionInner, out InsertionCollectionIndex);
                    }

                    ControllerView0.MoveFocus(+1, true, out IsMoved);
                }

                int MinBack = -ControllerView0.MinFocusMove;
                for (int i = 0; i < MinBack; i++)
                {
                    if (ControllerView0.Focus.CellView.StateView.State.Node == RootNode.UnassignedOptionalMain.Item.AssignedOptionalTree.Item.Placeholder)
                        break;

                    ControllerView0.MoveFocus(-1, true, out IsMoved);
                }

                Assert.That(ControllerView0.MinFocusMove < 0);

                Controller.Unassign(OptionalMainInner.ChildState.ParentIndex, out IsChanged);

                Leaf SecondLeafH = CreateLeaf(Guid.NewGuid());
                ILayoutInsertionListNodeIndex InsertionLeafIndexH = new LayoutInsertionListNodeIndex(RootNode, LeafInnerH.PropertyName, SecondLeafH, 0);
                Controller.Insert(LeafInnerH, InsertionLeafIndexH, out IWriteableBrowsingCollectionNodeIndex AsInsertedIndexH);

                Leaf SecondLeafV = CreateLeaf(Guid.NewGuid());
                ILayoutInsertionListNodeIndex InsertionLeafIndexV = new LayoutInsertionListNodeIndex(RootNode, LeafInnerV.PropertyName, SecondLeafV, 0);
                Controller.Insert(LeafInnerV, InsertionLeafIndexV, out IWriteableBrowsingCollectionNodeIndex AsInsertedIndexV);

                //System.Diagnostics.Debug.Assert(false);

                ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                int MaxFocusMove = ControllerView0.MaxFocusMove;

                for (int i = 0; i < MaxFocusMove; i++)
                {
                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    ControllerView0.MoveFocus(+i, true, out IsMoved);

                    int j;
                    for (j = 0; j < 20; j++)
                    {
                        ControllerView0.MoveFocus(+1, false, out IsMoved);
                        if (ControllerView0.Selection is ILayoutNodeListSelection AsNodeListSelection)
                        {
                            Assert.That(AsNodeListSelection.PropertyName != null);
                            Assert.That(AsNodeListSelection.StateView != null);
                            AsNodeListSelection.Update(AsNodeListSelection.StartIndex, AsNodeListSelection.EndIndex);
                            AsNodeListSelection.Update(AsNodeListSelection.EndIndex, AsNodeListSelection.StartIndex);
                            AsNodeListSelection.Copy(DataObject);
                            ControllerView0.MeasureAndArrange();
                            ControllerView0.Draw(ControllerView0.RootStateView);
                            ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                            break;
                        }
                    }
                }
            }
        }

        [Test]
        [Category("Coverage")]
        public static void LayoutSelection()
        {
            ControllerTools.ResetExpectedName();
            bool IsMoved;

            BaseNode.Document RootDocument = BaseNodeHelper.NodeHelper.CreateSimpleDocument("root doc", Guid.NewGuid());
            Root RootNode = new Root(RootDocument);

            BaseNode.IBlockList<Main> MainBlocksH = BaseNodeHelper.BlockListHelper.CreateEmptyBlockList<Main>();
            BaseNode.IBlock<Main> BlockH1 = BaseNodeHelper.BlockListHelper.CreateBlock<Main>(new List<Main>() { CreateRoot(ValueGuid0, Imperfections.None) });
            BaseNode.IBlock<Main> BlockH2 = BaseNodeHelper.BlockListHelper.CreateBlock<Main>(new List<Main>() { CreateRoot(ValueGuid1, Imperfections.None), CreateRoot(ValueGuid2, Imperfections.None) });
            MainBlocksH.NodeBlockList.Add(BlockH1);
            MainBlocksH.NodeBlockList.Add(BlockH2);

            BaseNode.IBlockList<Main> MainBlocksV = BaseNodeHelper.BlockListHelper.CreateEmptyBlockList<Main>();
            BaseNode.IBlock<Main> BlockV1 = BaseNodeHelper.BlockListHelper.CreateBlock<Main>(new List<Main>() { CreateRoot(ValueGuid3, Imperfections.None) });
            BaseNode.IBlock<Main> BlockV2 = BaseNodeHelper.BlockListHelper.CreateBlock<Main>(new List<Main>() { CreateRoot(ValueGuid4, Imperfections.None), CreateRoot(ValueGuid5, Imperfections.None) });
            MainBlocksV.NodeBlockList.Add(BlockV1);
            MainBlocksV.NodeBlockList.Add(BlockV2);

            Main UnassignedOptionalMain = CreateRoot(ValueGuid6, Imperfections.None);
            Easly.IOptionalReference<Main> UnassignedOptional = BaseNodeHelper.OptionalReferenceHelper.CreateReference<Main>(UnassignedOptionalMain);

            IList<Leaf> LeafPathH = new List<Leaf>();
            LeafPathH.Add(CreateLeaf(Guid.NewGuid()));
            LeafPathH.Add(CreateLeaf(Guid.NewGuid()));

            IList<Leaf> LeafPathV = new List<Leaf>();
            LeafPathV.Add(CreateLeaf(Guid.NewGuid()));
            LeafPathV.Add(CreateLeaf(Guid.NewGuid()));

            BaseNodeHelper.NodeTreeHelperBlockList.SetBlockList(RootNode, nameof(Root.MainBlocksH), (BaseNode.IBlockList)MainBlocksH);
            BaseNodeHelper.NodeTreeHelperBlockList.SetBlockList(RootNode, nameof(Root.MainBlocksV), (BaseNode.IBlockList)MainBlocksV);
            BaseNodeHelper.NodeTreeHelperOptional.SetOptionalReference(RootNode, nameof(Root.UnassignedOptionalMain), (Easly.IOptionalReference)UnassignedOptional);
            BaseNodeHelper.NodeTreeHelper.SetString(RootNode, nameof(Root.ValueString), "root string");
            BaseNodeHelper.NodeTreeHelperList.SetChildNodeList(RootNode, nameof(Root.LeafPathH), (IList)LeafPathH);
            BaseNodeHelper.NodeTreeHelperList.SetChildNodeList(RootNode, nameof(Root.LeafPathV), (IList)LeafPathV);
            BaseNodeHelper.NodeTreeHelperOptional.SetOptionalReference(RootNode, nameof(Root.UnassignedOptionalLeaf), (Easly.IOptionalReference)BaseNodeHelper.OptionalReferenceHelper.CreateReference(new Leaf()));

            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            System.Windows.IDataObject DataObject = new System.Windows.DataObject();

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                ControllerView0.SetCommentDisplayMode(CommentDisplayModes.All);
                ControllerView0.MeasureAndArrange();

                //System.Diagnostics.Debug.Assert(false);

                ILayoutBlockListInner BlockListInnerH = Controller.RootState.PropertyToInner(nameof(Root.MainBlocksH)) as ILayoutBlockListInner;
                Controller.ChangeReplication(BlockListInnerH, 1, BaseNode.ReplicationStatus.Replicated);

                ILayoutBlockListInner BlockListInnerV = Controller.RootState.PropertyToInner(nameof(Root.MainBlocksV)) as ILayoutBlockListInner;
                Controller.ChangeReplication(BlockListInnerV, 1, BaseNode.ReplicationStatus.Replicated);

                ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                int MaxFocusMove = ControllerView0.MaxFocusMove;

                for (int i = 0; i < MaxFocusMove; i++)
                {
                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    ControllerView0.MoveFocus(+i, true, out IsMoved);

                    int j;
                    for (j = 0; j < 4; j++)
                    {
                        ControllerView0.MoveFocus(+1, false, out IsMoved);

                        if (ControllerView0.Focus is ILayoutTextFocus)
                        {
                            string Text = ControllerView0.FocusedText;
                            Assert.That(Text != null);

                            if (Text.Length > 1)
                            {
                                ControllerView0.SetCaretPosition(0, true, out IsMoved);
                                ControllerView0.SetCaretPosition(1, false, out IsMoved);
                            }
                        }

                        if (ControllerView0.Selection is ILayoutEmptySelection AsEmptySelection)
                            Assert.That(AsEmptySelection.StateView == ControllerView0.RootStateView);

                        else if (ControllerView0.Selection is ILayoutNodeListSelection AsNodeListSelection)
                        {
                            Assert.That(AsNodeListSelection.PropertyName != null);
                            Assert.That(AsNodeListSelection.StateView != null);
                            AsNodeListSelection.Update(AsNodeListSelection.StartIndex, AsNodeListSelection.EndIndex);
                            AsNodeListSelection.Update(AsNodeListSelection.EndIndex, AsNodeListSelection.StartIndex);
                            AsNodeListSelection.Copy(DataObject);
                            ControllerView0.MeasureAndArrange();
                            ControllerView0.Draw(ControllerView0.RootStateView);
                            ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        }

                        else if (ControllerView0.Selection is ILayoutBlockNodeListSelection AsBlockNodeListSelection)
                        {
                            Assert.That(AsBlockNodeListSelection.PropertyName != null);
                            Assert.That(AsBlockNodeListSelection.StateView != null);
                            AsBlockNodeListSelection.Update(AsBlockNodeListSelection.StartIndex, AsBlockNodeListSelection.EndIndex);
                            AsBlockNodeListSelection.Update(AsBlockNodeListSelection.EndIndex, AsBlockNodeListSelection.StartIndex);
                            AsBlockNodeListSelection.Copy(DataObject);
                            ControllerView0.MeasureAndArrange();
                            ControllerView0.Draw(ControllerView0.RootStateView);
                            ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        }

                        else if (ControllerView0.Selection is ILayoutBlockListSelection AsBlockListSelection)
                        {
                            Assert.That(AsBlockListSelection.PropertyName != null);
                            Assert.That(AsBlockListSelection.StateView != null);
                            AsBlockListSelection.Update(AsBlockListSelection.StartIndex, AsBlockListSelection.EndIndex);
                            AsBlockListSelection.Update(AsBlockListSelection.EndIndex, AsBlockListSelection.StartIndex);
                            AsBlockListSelection.Copy(DataObject);
                            ControllerView0.MeasureAndArrange();
                            ControllerView0.Draw(ControllerView0.RootStateView);
                            ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        }

                        else if (ControllerView0.Selection is ILayoutTextSelection AsTextSelection)
                        {
                            Assert.That(AsTextSelection.StateView != null);
                            AsTextSelection.Update(AsTextSelection.Start, AsTextSelection.End);
                            AsTextSelection.Update(AsTextSelection.End, AsTextSelection.Start);
                            AsTextSelection.Copy(DataObject);
                            ControllerView0.MeasureAndArrange();
                            ControllerView0.Draw(ControllerView0.RootStateView);
                            ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        }

                        else if (ControllerView0.Selection is ILayoutDiscreteContentSelection AsDiscreteContentSelection)
                        {
                            Assert.That(AsDiscreteContentSelection.StateView != null);
                            ControllerView0.MeasureAndArrange();
                            ControllerView0.Draw(ControllerView0.RootStateView);
                            ControllerView0.Print(ControllerView0.RootStateView, Point.Origin);
                        }
                    }
                }
            }
        }

        [Test]
        [Category("Coverage")]
        public static void LayoutExtendSelection()
        {
            ControllerTools.ResetExpectedName();
            bool IsMoved;

            BaseNode.Document RootDocument = BaseNodeHelper.NodeHelper.CreateSimpleDocument("root doc", Guid.NewGuid());
            Root RootNode = new Root(RootDocument);

            BaseNode.IBlockList<Main> MainBlocksH = BaseNodeHelper.BlockListHelper.CreateEmptyBlockList<Main>();
            BaseNode.IBlock<Main> BlockH1 = BaseNodeHelper.BlockListHelper.CreateBlock<Main>(new List<Main>() { CreateRoot(ValueGuid0, Imperfections.None) });
            BaseNode.IBlock<Main> BlockH2 = BaseNodeHelper.BlockListHelper.CreateBlock<Main>(new List<Main>() { CreateRoot(ValueGuid1, Imperfections.None), CreateRoot(ValueGuid2, Imperfections.None) });
            MainBlocksH.NodeBlockList.Add(BlockH1);
            MainBlocksH.NodeBlockList.Add(BlockH2);

            BaseNode.IBlockList<Main> MainBlocksV = BaseNodeHelper.BlockListHelper.CreateEmptyBlockList<Main>();
            BaseNode.IBlock<Main> BlockV1 = BaseNodeHelper.BlockListHelper.CreateBlock<Main>(new List<Main>() { CreateRoot(ValueGuid3, Imperfections.None) });
            BaseNode.IBlock<Main> BlockV2 = BaseNodeHelper.BlockListHelper.CreateBlock<Main>(new List<Main>() { CreateRoot(ValueGuid4, Imperfections.None), CreateRoot(ValueGuid5, Imperfections.None) });
            MainBlocksV.NodeBlockList.Add(BlockV1);
            MainBlocksV.NodeBlockList.Add(BlockV2);

            Main UnassignedOptionalMain = CreateRoot(ValueGuid6, Imperfections.None);
            Easly.IOptionalReference<Main> UnassignedOptional = BaseNodeHelper.OptionalReferenceHelper.CreateReference<Main>(UnassignedOptionalMain);

            IList<Leaf> LeafPathH = new List<Leaf>();
            LeafPathH.Add(CreateLeaf(Guid.NewGuid()));
            LeafPathH.Add(CreateLeaf(Guid.NewGuid()));

            IList<Leaf> LeafPathV = new List<Leaf>();
            LeafPathV.Add(CreateLeaf(Guid.NewGuid()));
            LeafPathV.Add(CreateLeaf(Guid.NewGuid()));

            BaseNodeHelper.NodeTreeHelperBlockList.SetBlockList(RootNode, nameof(Root.MainBlocksH), (BaseNode.IBlockList)MainBlocksH);
            BaseNodeHelper.NodeTreeHelperBlockList.SetBlockList(RootNode, nameof(Root.MainBlocksV), (BaseNode.IBlockList)MainBlocksV);
            BaseNodeHelper.NodeTreeHelperOptional.SetOptionalReference(RootNode, nameof(Root.UnassignedOptionalMain), (Easly.IOptionalReference)UnassignedOptional);
            BaseNodeHelper.NodeTreeHelper.SetString(RootNode, nameof(Root.ValueString), "root string");
            BaseNodeHelper.NodeTreeHelperList.SetChildNodeList(RootNode, nameof(Root.LeafPathH), (IList)LeafPathH);
            BaseNodeHelper.NodeTreeHelperList.SetChildNodeList(RootNode, nameof(Root.LeafPathV), (IList)LeafPathV);
            BaseNodeHelper.NodeTreeHelperOptional.SetOptionalReference(RootNode, nameof(Root.UnassignedOptionalLeaf), (Easly.IOptionalReference)BaseNodeHelper.OptionalReferenceHelper.CreateReference(new Leaf()));

            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            using (LayoutControllerView ControllerView0 = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Assert.That(ControllerView0.Controller == Controller);

                //System.Diagnostics.Debug.Assert(false);

                ControllerView0.SetCommentDisplayMode(CommentDisplayModes.All);
                Assert.That(!ControllerView0.ShowBlockGeometry);
                ControllerView0.SetShowBlockGeometry(!ControllerView0.ShowBlockGeometry);
                Assert.That(ControllerView0.ShowBlockGeometry);
                ControllerView0.MeasureAndArrange();

                ILayoutBlockListInner BlockListInnerH = Controller.RootState.PropertyToInner(nameof(Root.MainBlocksH)) as ILayoutBlockListInner;
                Controller.ChangeReplication(BlockListInnerH, 1, BaseNode.ReplicationStatus.Replicated);

                ILayoutBlockListInner BlockListInnerV = Controller.RootState.PropertyToInner(nameof(Root.MainBlocksV)) as ILayoutBlockListInner;
                Controller.ChangeReplication(BlockListInnerV, 1, BaseNode.ReplicationStatus.Replicated);

                ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                int MaxFocusMove = ControllerView0.MaxFocusMove;

                for (int i = 0; i < MaxFocusMove; i++)
                {
                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    ControllerView0.MoveFocus(+i, true, out IsMoved);

                    bool IsChanged = true;
                    string FocusedText = ControllerView0.FocusedText;
                    if (FocusedText != null && FocusedText.Length > 1 && i % 2 == 0)
                        ControllerView0.SetCaretPosition(1, true, out IsMoved);

                    if (ControllerView0.Selection is ILayoutDiscreteContentSelection AsDiscreteContentSelection)
                    {
                        string DiscreteString = AsDiscreteContentSelection.ToString();
                    }
                    else if (ControllerView0.Selection is ILayoutEmptySelection AsEmptySelection)
                    {
                        string EmptyString = AsEmptySelection.ToString();
                    }

                    int j;
                    for (j = 0; j < 20; j++)
                    {
                        ControllerView0.ExtendSelection(out IsChanged);
                        if (!IsChanged)
                            break;

                        if (ControllerView0.Selection is ILayoutBlockNodeListSelection AsBlockNodeListSelection)
                        {
                            Assert.That(AsBlockNodeListSelection.PropertyName != null);
                            Assert.That(AsBlockNodeListSelection.StateView != null);
                        }

                        Assert.That(ControllerView0.SelectionExtension == j + 1);
                        Assert.That(ControllerView0.Selection != null);
                        string SelectionString = ControllerView0.Selection.ToString();

                        ControllerView0.MeasureAndArrange();
                        ControllerView0.Draw(ControllerView0.RootStateView);
                    }

                    Assert.That(!IsChanged);
                    ILayoutNodeSelection FullSelection = ControllerView0.Selection as ILayoutNodeSelection;
                    Assert.That(FullSelection != null);
                    Assert.That(FullSelection.StateView == ControllerView0.RootStateView);

                    int ExtendSteps = j;

                    for (j = 0; j < ExtendSteps; j++)
                    {
                        ControllerView0.ReduceSelection(out IsChanged);
                        Assert.That(IsChanged);

                        ControllerView0.MeasureAndArrange();
                        ControllerView0.Draw(ControllerView0.RootStateView);
                    }

                    ControllerView0.ReduceSelection(out IsChanged);
                    Assert.That(!IsChanged);
                }
            }
        }

        [Test]
        [Category("Coverage")]
        public static void LayoutCollections()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            ILayoutRootNodeIndex RootIndex;
            bool IsReadOnly;
            IReadOnlyBlockState FirstBlockState;
            IReadOnlyBrowsingBlockNodeIndex FirstBlockNodeIndex;
            IReadOnlyBrowsingListNodeIndex FirstListNodeIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new LayoutRootNodeIndex(RootNode);

            LayoutController ControllerBase = LayoutController.Create(RootIndex);
            LayoutController Controller = LayoutController.Create(RootIndex);

            ReadOnlyNodeStateDictionary ControllerStateTable = DebugObjects.GetReferenceByInterface(Type.FromTypeof<LayoutNodeStateDictionary>()) as ReadOnlyNodeStateDictionary;

            using (LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                Controller.Expand(Controller.RootIndex, out bool IsChanged);

                // IxxxBlockStateViewDictionary 

                ReadOnlyBlockStateViewDictionary ReadOnlyBlockStateViewTable = ControllerView.BlockStateViewTable;
                foreach (KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView> Entry in ReadOnlyBlockStateViewTable)
                {
                    ReadOnlyBlockStateView StateView = ReadOnlyBlockStateViewTable[Entry.Key];
                    ReadOnlyBlockStateViewTable.TryGetValue(Entry.Key, out ReadOnlyBlockStateView Value);
                    ((ICollection< KeyValuePair < IReadOnlyBlockState, ReadOnlyBlockStateView >>)ReadOnlyBlockStateViewTable).Contains(Entry);
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

                WriteableBlockStateViewDictionary WriteableBlockStateViewTable = ControllerView.BlockStateViewTable;
                foreach (KeyValuePair<IWriteableBlockState, WriteableBlockStateView> Entry in (ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>)WriteableBlockStateViewTable)
                {
                    WriteableBlockStateView StateView = (WriteableBlockStateView)WriteableBlockStateViewTable[Entry.Key];
                    WriteableBlockStateViewTable.TryGetValue(Entry.Key, out ReadOnlyBlockStateView Value);
                    ((ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>>)WriteableBlockStateViewTable).Contains(Entry);
                    WriteableBlockStateViewTable.Remove(Entry.Key);
                    WriteableBlockStateViewTable.Add(Entry.Key, Entry.Value);
                    ICollection<IWriteableBlockState> Keys = ((IDictionary<IWriteableBlockState, WriteableBlockStateView>)WriteableBlockStateViewTable).Keys;
                    ICollection<WriteableBlockStateView> Values = ((IDictionary<IWriteableBlockState, WriteableBlockStateView>)WriteableBlockStateViewTable).Values;

                    break;
                }
                IDictionary<IWriteableBlockState, WriteableBlockStateView> WriteableBlockStateViewTableAsDictionary = WriteableBlockStateViewTable;
                foreach (KeyValuePair<IWriteableBlockState, WriteableBlockStateView> Entry in WriteableBlockStateViewTableAsDictionary)
                {
                    WriteableBlockStateView StateView = WriteableBlockStateViewTableAsDictionary[Entry.Key];
                    break;
                }
                ICollection<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>> WriteableBlockStateViewTableAsCollection = WriteableBlockStateViewTable;
                IsReadOnly = WriteableBlockStateViewTableAsCollection.IsReadOnly;
                foreach (KeyValuePair<IWriteableBlockState, WriteableBlockStateView> Entry in WriteableBlockStateViewTableAsCollection)
                {
                    WriteableBlockStateViewTableAsCollection.Contains(Entry);
                    WriteableBlockStateViewTableAsCollection.Remove(Entry);
                    WriteableBlockStateViewTableAsCollection.Add(Entry);
                    WriteableBlockStateViewTableAsCollection.CopyTo(new KeyValuePair<IWriteableBlockState, WriteableBlockStateView>[WriteableBlockStateViewTableAsCollection.Count], 0);
                    break;
                }
                IEnumerable<KeyValuePair<IWriteableBlockState, WriteableBlockStateView>> WriteableBlockStateViewTableAsEnumerable = WriteableBlockStateViewTable;
                foreach (KeyValuePair<IWriteableBlockState, WriteableBlockStateView> Entry in WriteableBlockStateViewTableAsEnumerable)
                {
                    break;
                }

                FrameBlockStateViewDictionary FrameBlockStateViewTable = ControllerView.BlockStateViewTable;
                foreach (KeyValuePair<IFrameBlockState, FrameBlockStateView> Entry in (ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>)FrameBlockStateViewTable)
                {
                    FrameBlockStateView StateView = (FrameBlockStateView)FrameBlockStateViewTable[Entry.Key];
                    FrameBlockStateViewTable.TryGetValue(Entry.Key, out ReadOnlyBlockStateView Value);
                    ((ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>>)FrameBlockStateViewTable).Contains(Entry);
                    FrameBlockStateViewTable.Remove(Entry.Key);
                    FrameBlockStateViewTable.Add(Entry.Key, Entry.Value);
                    ICollection<IFrameBlockState> Keys = ((IDictionary<IFrameBlockState, FrameBlockStateView>)FrameBlockStateViewTable).Keys;
                    ICollection<FrameBlockStateView> Values = ((IDictionary<IFrameBlockState, FrameBlockStateView>)FrameBlockStateViewTable).Values;

                    break;
                }
                IDictionary<IFrameBlockState, FrameBlockStateView> FrameBlockStateViewTableAsDictionary = FrameBlockStateViewTable;
                foreach (KeyValuePair<IFrameBlockState, FrameBlockStateView> Entry in FrameBlockStateViewTableAsDictionary)
                {
                    FrameBlockStateView StateView = FrameBlockStateViewTableAsDictionary[Entry.Key];
                    break;
                }
                ICollection<KeyValuePair<IFrameBlockState, FrameBlockStateView>> FrameBlockStateViewTableAsCollection = FrameBlockStateViewTable;
                IsReadOnly = FrameBlockStateViewTableAsCollection.IsReadOnly;
                foreach (KeyValuePair<IFrameBlockState, FrameBlockStateView> Entry in FrameBlockStateViewTableAsCollection)
                {
                    FrameBlockStateViewTableAsCollection.Contains(Entry);
                    FrameBlockStateViewTableAsCollection.Remove(Entry);
                    FrameBlockStateViewTableAsCollection.Add(Entry);
                    FrameBlockStateViewTableAsCollection.CopyTo(new KeyValuePair<IFrameBlockState, FrameBlockStateView>[FrameBlockStateViewTableAsCollection.Count], 0);
                    break;
                }
                IEnumerable<KeyValuePair<IFrameBlockState, FrameBlockStateView>> FrameBlockStateViewTableAsEnumerable = FrameBlockStateViewTable;
                foreach (KeyValuePair<IFrameBlockState, FrameBlockStateView> Entry in FrameBlockStateViewTableAsEnumerable)
                {
                    break;
                }

                FocusBlockStateViewDictionary FocusBlockStateViewTable = ControllerView.BlockStateViewTable;
                foreach (KeyValuePair<IFocusBlockState, FocusBlockStateView> Entry in (ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>)FocusBlockStateViewTable)
                {
                    FocusBlockStateView StateView = (FocusBlockStateView)FocusBlockStateViewTable[Entry.Key];
                    FocusBlockStateViewTable.TryGetValue(Entry.Key, out ReadOnlyBlockStateView Value);
                    ((ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>)FocusBlockStateViewTable).Contains(Entry);
                    FocusBlockStateViewTable.Remove(Entry.Key);
                    FocusBlockStateViewTable.Add(Entry.Key, Entry.Value);
                    ICollection<IFocusBlockState> Keys = ((IDictionary<IFocusBlockState, FocusBlockStateView>)FocusBlockStateViewTable).Keys;
                    ICollection<FocusBlockStateView> Values = ((IDictionary<IFocusBlockState, FocusBlockStateView>)FocusBlockStateViewTable).Values;

                    break;
                }
                IDictionary<IFocusBlockState, FocusBlockStateView> FocusBlockStateViewTableAsDictionary = FocusBlockStateViewTable;
                foreach (KeyValuePair<IFocusBlockState, FocusBlockStateView> Entry in FocusBlockStateViewTableAsDictionary)
                {
                    FocusBlockStateView StateView = FocusBlockStateViewTableAsDictionary[Entry.Key];
                    break;
                }
                ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>> FocusBlockStateViewTableAsCollection = FocusBlockStateViewTable;
                IsReadOnly = FocusBlockStateViewTableAsCollection.IsReadOnly;
                foreach (KeyValuePair<IFocusBlockState, FocusBlockStateView> Entry in FocusBlockStateViewTableAsCollection)
                {
                    FocusBlockStateViewTableAsCollection.Contains(Entry);
                    FocusBlockStateViewTableAsCollection.Remove(Entry);
                    FocusBlockStateViewTableAsCollection.Add(Entry);
                    FocusBlockStateViewTableAsCollection.CopyTo(new KeyValuePair<IFocusBlockState, FocusBlockStateView>[FocusBlockStateViewTableAsCollection.Count], 0);
                    break;
                }
                IEnumerable<KeyValuePair<IFocusBlockState, FocusBlockStateView>> FocusBlockStateViewTableAsEnumerable = FocusBlockStateViewTable;
                foreach (KeyValuePair<IFocusBlockState, FocusBlockStateView> Entry in FocusBlockStateViewTableAsEnumerable)
                {
                    break;
                }

                // ILayoutBlockStateList

                ILayoutNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                ILayoutBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as ILayoutBlockListInner;
                Assert.That(LeafBlocksInner != null);

                ILayoutListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as ILayoutListInner;
                Assert.That(LeafPathInner != null);

                ILayoutPlaceholderNodeState FirstNodeState = LeafBlocksInner.FirstNodeState;
                LayoutBlockStateList DebugBlockStateList = DebugObjects.GetReferenceByInterface(Type.FromTypeof<LayoutBlockStateList>()) as LayoutBlockStateList;
                if (DebugBlockStateList != null)
                {
                    Assert.That(DebugBlockStateList.Count > 0);
                    FirstBlockState = DebugBlockStateList[0];
                    Assert.That(DebugBlockStateList.Contains(FirstBlockState));
                    Assert.That(DebugBlockStateList.IndexOf(FirstBlockState) == 0);
                    DebugBlockStateList.Remove(FirstBlockState);
                    DebugBlockStateList.Add(FirstBlockState);
                    DebugBlockStateList.Remove(FirstBlockState);
                    DebugBlockStateList.Insert(0, FirstBlockState);

                    IsReadOnly = ((ICollection<IReadOnlyBlockState>)DebugBlockStateList).IsReadOnly;
                    IsReadOnly = ((IList<IReadOnlyBlockState>)DebugBlockStateList).IsReadOnly;
                    DebugBlockStateList.CopyTo((IReadOnlyBlockState[])(new ILayoutBlockState[DebugBlockStateList.Count]), 0);
                    IEnumerable<IReadOnlyBlockState> BlockStateListAsReadOnlyEnumerable = DebugBlockStateList;
                    foreach (IReadOnlyBlockState Item in BlockStateListAsReadOnlyEnumerable)
                    {
                        break;
                    }
                    IList<IReadOnlyBlockState> BlockStateListAsReadOnlyIlist = DebugBlockStateList;
                    Assert.That(BlockStateListAsReadOnlyIlist[0] == FirstBlockState);
                    IReadOnlyList<IReadOnlyBlockState> BlockStateListAsReadOnlyIReadOnlylist = DebugBlockStateList;
                    Assert.That(BlockStateListAsReadOnlyIReadOnlylist[0] == FirstBlockState);

                    IsReadOnly = ((ICollection<IWriteableBlockState>)DebugBlockStateList).IsReadOnly;
                    IsReadOnly = ((IList<IWriteableBlockState>)DebugBlockStateList).IsReadOnly;
                    Assert.That(((WriteableBlockStateList)DebugBlockStateList)[0] == FirstBlockState);
                    DebugBlockStateList.CopyTo((IWriteableBlockState[])(new ILayoutBlockState[DebugBlockStateList.Count]), 0);
                    IEnumerable<IWriteableBlockState> BlockStateListAsWriteableEnumerable = DebugBlockStateList;
                    foreach (IWriteableBlockState Item in BlockStateListAsWriteableEnumerable)
                    {
                        break;
                    }
                    IList<IWriteableBlockState> BlockStateListAsWriteableIList = DebugBlockStateList;
                    Assert.That(BlockStateListAsWriteableIList[0] == FirstBlockState);
                    Assert.That(BlockStateListAsWriteableIList.Contains((IWriteableBlockState)FirstBlockState));
                    Assert.That(BlockStateListAsWriteableIList.IndexOf((IWriteableBlockState)FirstBlockState) == 0);
                    ICollection<IWriteableBlockState> BlockStateListAsWriteableICollection = DebugBlockStateList;
                    Assert.That(BlockStateListAsWriteableICollection.Contains((IWriteableBlockState)FirstBlockState));
                    BlockStateListAsWriteableICollection.Remove((IWriteableBlockState)FirstBlockState);
                    BlockStateListAsWriteableICollection.Add((IWriteableBlockState)FirstBlockState);
                    BlockStateListAsWriteableICollection.Remove((IWriteableBlockState)FirstBlockState);
                    BlockStateListAsWriteableIList.Insert(0, (IWriteableBlockState)FirstBlockState);
                    IReadOnlyList<IWriteableBlockState> BlockStateListAsWriteableIReadOnlylist = DebugBlockStateList;
                    Assert.That(BlockStateListAsWriteableIReadOnlylist[0] == FirstBlockState);
                    IEnumerator<IWriteableBlockState> DebugBlockStateListWriteableEnumerator = ((ICollection<IWriteableBlockState>)DebugBlockStateList).GetEnumerator();

                    IsReadOnly = ((ICollection<IFrameBlockState>)DebugBlockStateList).IsReadOnly;
                    IsReadOnly = ((IList<IFrameBlockState>)DebugBlockStateList).IsReadOnly;
                    Assert.That(((FrameBlockStateList)DebugBlockStateList)[0] == FirstBlockState);
                    DebugBlockStateList.CopyTo((IFrameBlockState[])(new ILayoutBlockState[DebugBlockStateList.Count]), 0);
                    IEnumerable<IFrameBlockState> BlockStateListAsFrameEnumerable = DebugBlockStateList;
                    foreach (IFrameBlockState Item in BlockStateListAsFrameEnumerable)
                    {
                        break;
                    }
                    IList<IFrameBlockState> BlockStateListAsFrameIList = DebugBlockStateList;
                    Assert.That(BlockStateListAsFrameIList[0] == FirstBlockState);
                    Assert.That(BlockStateListAsFrameIList.Contains((IFrameBlockState)FirstBlockState));
                    Assert.That(BlockStateListAsFrameIList.IndexOf((IFrameBlockState)FirstBlockState) == 0);
                    ICollection<IFrameBlockState> BlockStateListAsFrameICollection = DebugBlockStateList;
                    Assert.That(BlockStateListAsFrameICollection.Contains((IFrameBlockState)FirstBlockState));
                    BlockStateListAsFrameICollection.Remove((IFrameBlockState)FirstBlockState);
                    BlockStateListAsFrameICollection.Add((IFrameBlockState)FirstBlockState);
                    BlockStateListAsFrameICollection.Remove((IFrameBlockState)FirstBlockState);
                    BlockStateListAsFrameIList.Insert(0, (IFrameBlockState)FirstBlockState);
                    IReadOnlyList<IFrameBlockState> BlockStateListAsFrameIReadOnlylist = DebugBlockStateList;
                    Assert.That(BlockStateListAsFrameIReadOnlylist[0] == FirstBlockState);
                    IEnumerator<IFrameBlockState> DebugBlockStateListFrameEnumerator = ((ICollection<IFrameBlockState>)DebugBlockStateList).GetEnumerator();

                    IsReadOnly = ((ICollection<IFocusBlockState>)DebugBlockStateList).IsReadOnly;
                    IsReadOnly = ((IList<IFocusBlockState>)DebugBlockStateList).IsReadOnly;
                    Assert.That(((FocusBlockStateList)DebugBlockStateList)[0] == FirstBlockState);
                    DebugBlockStateList.CopyTo((IFocusBlockState[])(new ILayoutBlockState[DebugBlockStateList.Count]), 0);
                    IEnumerable<IFocusBlockState> BlockStateListAsFocusEnumerable = DebugBlockStateList;
                    foreach (IFocusBlockState Item in BlockStateListAsFocusEnumerable)
                    {
                        break;
                    }
                    IList<IFocusBlockState> BlockStateListAsFocusIList = DebugBlockStateList;
                    Assert.That(BlockStateListAsFocusIList[0] == FirstBlockState);
                    Assert.That(BlockStateListAsFocusIList.Contains((IFocusBlockState)FirstBlockState));
                    Assert.That(BlockStateListAsFocusIList.IndexOf((IFocusBlockState)FirstBlockState) == 0);
                    ICollection<IFocusBlockState> BlockStateListAsFocusICollection = DebugBlockStateList;
                    Assert.That(BlockStateListAsFocusICollection.Contains((IFocusBlockState)FirstBlockState));
                    BlockStateListAsFocusICollection.Remove((IFocusBlockState)FirstBlockState);
                    BlockStateListAsFocusICollection.Add((IFocusBlockState)FirstBlockState);
                    BlockStateListAsFocusICollection.Remove((IFocusBlockState)FirstBlockState);
                    BlockStateListAsFocusIList.Insert(0, (IFocusBlockState)FirstBlockState);
                    IReadOnlyList<IFocusBlockState> BlockStateListAsFocusIReadOnlylist = DebugBlockStateList;
                    Assert.That(BlockStateListAsFocusIReadOnlylist[0] == FirstBlockState);
                    IEnumerator<IFocusBlockState> DebugBlockStateListFocusEnumerator = ((ICollection<IFocusBlockState>)DebugBlockStateList).GetEnumerator();
                }

                // ILayoutBlockStateReadOnlyList

                LayoutBlockStateReadOnlyList LayoutBlockStateList = LeafBlocksInner.BlockStateList;
                Assert.That(LayoutBlockStateList.Count > 0);
                FirstBlockState = LayoutBlockStateList[0];
                Assert.That(LayoutBlockStateList.Contains(FirstBlockState));
                Assert.That(LayoutBlockStateList.IndexOf(FirstBlockState) == 0);
                Assert.That(LayoutBlockStateList.Contains((ILayoutBlockState)FirstBlockState));
                Assert.That(LayoutBlockStateList.IndexOf((ILayoutBlockState)FirstBlockState) == 0);

                IEnumerable<IWriteableBlockState> WriteableLayoutBlockStateListAsIEnumerable = LayoutBlockStateList;
                IEnumerator<IWriteableBlockState> WriteableLayoutBlockStateListAsIEnumerableEnumerator = WriteableLayoutBlockStateListAsIEnumerable.GetEnumerator();
                Assert.That(LayoutBlockStateList.Contains((IWriteableBlockState)FirstBlockState));
                Assert.That(LayoutBlockStateList.IndexOf((IWriteableBlockState)FirstBlockState) == 0);
                IReadOnlyList<IWriteableBlockState> WriteableLayoutBlockStateListAsIReadOnlyList = LayoutBlockStateList;
                Assert.That(WriteableLayoutBlockStateListAsIReadOnlyList[0] == FirstBlockState);

                IEnumerable<IFrameBlockState> FrameLayoutBlockStateListAsIEnumerable = LayoutBlockStateList;
                IEnumerator<IFrameBlockState> FrameLayoutBlockStateListAsIEnumerableEnumerator = FrameLayoutBlockStateListAsIEnumerable.GetEnumerator();
                Assert.That(LayoutBlockStateList.Contains((IFrameBlockState)FirstBlockState));
                Assert.That(LayoutBlockStateList.IndexOf((IFrameBlockState)FirstBlockState) == 0);
                IReadOnlyList<IFrameBlockState> FrameLayoutBlockStateListAsIReadOnlyList = LayoutBlockStateList;
                Assert.That(FrameLayoutBlockStateListAsIReadOnlyList[0] == FirstBlockState);

                IEnumerable<IFocusBlockState> FocusLayoutBlockStateListAsIEnumerable = LayoutBlockStateList;
                IEnumerator<IFocusBlockState> FocusLayoutBlockStateListAsIEnumerableEnumerator = FocusLayoutBlockStateListAsIEnumerable.GetEnumerator();
                Assert.That(LayoutBlockStateList.Contains((IFocusBlockState)FirstBlockState));
                Assert.That(LayoutBlockStateList.IndexOf((IFocusBlockState)FirstBlockState) == 0);
                IReadOnlyList<IFocusBlockState> FocusLayoutBlockStateListAsIReadOnlyList = LayoutBlockStateList;
                Assert.That(FocusLayoutBlockStateListAsIReadOnlyList[0] == FirstBlockState);

                // ILayoutBrowsingBlockNodeIndexList

                LayoutBrowsingBlockNodeIndexList BlockNodeIndexList = LeafBlocksInner.AllIndexes() as LayoutBrowsingBlockNodeIndexList;
                Assert.That(BlockNodeIndexList.Count > 0);
                FirstBlockNodeIndex = BlockNodeIndexList[0];
                Assert.That(BlockNodeIndexList.Contains(FirstBlockNodeIndex));
                Assert.That(BlockNodeIndexList.IndexOf(FirstBlockNodeIndex) == 0);
                BlockNodeIndexList.Remove(FirstBlockNodeIndex);
                BlockNodeIndexList.Add(FirstBlockNodeIndex);
                BlockNodeIndexList.Remove(FirstBlockNodeIndex);
                BlockNodeIndexList.Insert(0, FirstBlockNodeIndex);

                IsReadOnly = ((ICollection<IReadOnlyBrowsingBlockNodeIndex>)BlockNodeIndexList).IsReadOnly;
                IsReadOnly = ((IList<IReadOnlyBrowsingBlockNodeIndex>)BlockNodeIndexList).IsReadOnly;
                BlockNodeIndexList.CopyTo((IReadOnlyBrowsingBlockNodeIndex[])(new ILayoutBrowsingBlockNodeIndex[BlockNodeIndexList.Count]), 0);
                IEnumerable<IReadOnlyBrowsingBlockNodeIndex> BlockNodeIndexListAsReadOnlyEnumerable = BlockNodeIndexList;
                foreach (IReadOnlyBrowsingBlockNodeIndex Item in BlockNodeIndexListAsReadOnlyEnumerable)
                {
                    break;
                }
                IList<IReadOnlyBrowsingBlockNodeIndex> BlockNodeIndexListAsReadOnlyIList = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsReadOnlyIList[0] == FirstBlockNodeIndex);
                IReadOnlyList<IReadOnlyBrowsingBlockNodeIndex> BlockNodeIndexListAsReadOnlyIReadOnlylist = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsReadOnlyIReadOnlylist[0] == FirstBlockNodeIndex);

                IsReadOnly = ((ICollection<IWriteableBrowsingBlockNodeIndex>)BlockNodeIndexList).IsReadOnly;
                IsReadOnly = ((IList<IWriteableBrowsingBlockNodeIndex>)BlockNodeIndexList).IsReadOnly;
                Assert.That(((WriteableBrowsingBlockNodeIndexList)BlockNodeIndexList)[0] == FirstBlockNodeIndex);
                BlockNodeIndexList.CopyTo((IWriteableBrowsingBlockNodeIndex[])(new ILayoutBrowsingBlockNodeIndex[BlockNodeIndexList.Count]), 0);
                IEnumerable<IWriteableBrowsingBlockNodeIndex> BlockNodeIndexListAsWriteableEnumerable = BlockNodeIndexList;
                foreach (IWriteableBrowsingBlockNodeIndex Item in BlockNodeIndexListAsWriteableEnumerable)
                {
                    break;
                }
                IList<IWriteableBrowsingBlockNodeIndex> BlockNodeIndexListAsWriteableIList = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsWriteableIList[0] == FirstBlockNodeIndex);
                Assert.That(BlockNodeIndexListAsWriteableIList.Contains((IWriteableBrowsingBlockNodeIndex)FirstBlockNodeIndex));
                Assert.That(BlockNodeIndexListAsWriteableIList.IndexOf((IWriteableBrowsingBlockNodeIndex)FirstBlockNodeIndex) == 0);
                ICollection<IWriteableBrowsingBlockNodeIndex> BrowsingBlockNodeIndexListAsWriteableICollection = BlockNodeIndexList;
                Assert.That(BrowsingBlockNodeIndexListAsWriteableICollection.Contains((IWriteableBrowsingBlockNodeIndex)FirstBlockNodeIndex));
                BrowsingBlockNodeIndexListAsWriteableICollection.Remove((IWriteableBrowsingBlockNodeIndex)FirstBlockNodeIndex);
                BrowsingBlockNodeIndexListAsWriteableICollection.Add((IWriteableBrowsingBlockNodeIndex)FirstBlockNodeIndex);
                BrowsingBlockNodeIndexListAsWriteableICollection.Remove((IWriteableBrowsingBlockNodeIndex)FirstBlockNodeIndex);
                BlockNodeIndexListAsWriteableIList.Insert(0, (IWriteableBrowsingBlockNodeIndex)FirstBlockNodeIndex);
                IReadOnlyList<IWriteableBrowsingBlockNodeIndex> BlockNodeIndexListAsWriteableIReadOnlylist = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsWriteableIReadOnlylist[0] == FirstBlockNodeIndex);
                IEnumerator<IWriteableBrowsingBlockNodeIndex> BlockNodeIndexListWriteableEnumerator = ((ICollection<IWriteableBrowsingBlockNodeIndex>)BlockNodeIndexList).GetEnumerator();

                IsReadOnly = ((ICollection<IFrameBrowsingBlockNodeIndex>)BlockNodeIndexList).IsReadOnly;
                IsReadOnly = ((IList<IFrameBrowsingBlockNodeIndex>)BlockNodeIndexList).IsReadOnly;
                Assert.That(((FrameBrowsingBlockNodeIndexList)BlockNodeIndexList)[0] == FirstBlockNodeIndex);
                BlockNodeIndexList.CopyTo((IFrameBrowsingBlockNodeIndex[])(new ILayoutBrowsingBlockNodeIndex[BlockNodeIndexList.Count]), 0);
                IEnumerable<IFrameBrowsingBlockNodeIndex> BlockNodeIndexListAsFrameEnumerable = BlockNodeIndexList;
                foreach (IFrameBrowsingBlockNodeIndex Item in BlockNodeIndexListAsFrameEnumerable)
                {
                    break;
                }
                IList<IFrameBrowsingBlockNodeIndex> BlockNodeIndexListAsFrameIList = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsFrameIList[0] == FirstBlockNodeIndex);
                Assert.That(BlockNodeIndexListAsFrameIList.Contains((IFrameBrowsingBlockNodeIndex)FirstBlockNodeIndex));
                Assert.That(BlockNodeIndexListAsFrameIList.IndexOf((IFrameBrowsingBlockNodeIndex)FirstBlockNodeIndex) == 0);
                ICollection<IFrameBrowsingBlockNodeIndex> BrowsingBlockNodeIndexListAsFrameICollection = BlockNodeIndexList;
                Assert.That(BrowsingBlockNodeIndexListAsFrameICollection.Contains((IFrameBrowsingBlockNodeIndex)FirstBlockNodeIndex));
                BrowsingBlockNodeIndexListAsFrameICollection.Remove((IFrameBrowsingBlockNodeIndex)FirstBlockNodeIndex);
                BrowsingBlockNodeIndexListAsFrameICollection.Add((IFrameBrowsingBlockNodeIndex)FirstBlockNodeIndex);
                BrowsingBlockNodeIndexListAsFrameICollection.Remove((IFrameBrowsingBlockNodeIndex)FirstBlockNodeIndex);
                BlockNodeIndexListAsFrameIList.Insert(0, (IFrameBrowsingBlockNodeIndex)FirstBlockNodeIndex);
                IReadOnlyList<IFrameBrowsingBlockNodeIndex> BlockNodeIndexListAsFrameIReadOnlylist = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsFrameIReadOnlylist[0] == FirstBlockNodeIndex);
                ReadOnlyBrowsingBlockNodeIndexList FrameBlockNodeIndexListAsReadOnly = BlockNodeIndexList;
                Assert.That(FrameBlockNodeIndexListAsReadOnly[0] == FirstBlockNodeIndex);
                IEnumerator<IFrameBrowsingBlockNodeIndex> BlockNodeIndexListFrameEnumerator = ((ICollection<IFrameBrowsingBlockNodeIndex>)BlockNodeIndexList).GetEnumerator();

                IsReadOnly = ((ICollection<IFocusBrowsingBlockNodeIndex>)BlockNodeIndexList).IsReadOnly;
                IsReadOnly = ((IList<IFocusBrowsingBlockNodeIndex>)BlockNodeIndexList).IsReadOnly;
                Assert.That(((FocusBrowsingBlockNodeIndexList)BlockNodeIndexList)[0] == FirstBlockNodeIndex);
                BlockNodeIndexList.CopyTo((IFocusBrowsingBlockNodeIndex[])(new ILayoutBrowsingBlockNodeIndex[BlockNodeIndexList.Count]), 0);
                IEnumerable<IFocusBrowsingBlockNodeIndex> BlockNodeIndexListAsFocusEnumerable = BlockNodeIndexList;
                foreach (IFocusBrowsingBlockNodeIndex Item in BlockNodeIndexListAsFocusEnumerable)
                {
                    break;
                }
                IList<IFocusBrowsingBlockNodeIndex> BlockNodeIndexListAsFocusIList = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsFocusIList[0] == FirstBlockNodeIndex);
                Assert.That(BlockNodeIndexListAsFocusIList.Contains((IFocusBrowsingBlockNodeIndex)FirstBlockNodeIndex));
                Assert.That(BlockNodeIndexListAsFocusIList.IndexOf((IFocusBrowsingBlockNodeIndex)FirstBlockNodeIndex) == 0);
                ICollection<IFocusBrowsingBlockNodeIndex> BrowsingBlockNodeIndexListAsFocusICollection = BlockNodeIndexList;
                Assert.That(BrowsingBlockNodeIndexListAsFocusICollection.Contains((IFocusBrowsingBlockNodeIndex)FirstBlockNodeIndex));
                BrowsingBlockNodeIndexListAsFocusICollection.Remove((IFocusBrowsingBlockNodeIndex)FirstBlockNodeIndex);
                BrowsingBlockNodeIndexListAsFocusICollection.Add((IFocusBrowsingBlockNodeIndex)FirstBlockNodeIndex);
                BrowsingBlockNodeIndexListAsFocusICollection.Remove((IFocusBrowsingBlockNodeIndex)FirstBlockNodeIndex);
                BlockNodeIndexListAsFocusIList.Insert(0, (IFocusBrowsingBlockNodeIndex)FirstBlockNodeIndex);
                IReadOnlyList<IFocusBrowsingBlockNodeIndex> BlockNodeIndexListAsFocusIReadOnlylist = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsFocusIReadOnlylist[0] == FirstBlockNodeIndex);
                ReadOnlyBrowsingBlockNodeIndexList FocusBlockNodeIndexListAsReadOnly = BlockNodeIndexList;
                Assert.That(FocusBlockNodeIndexListAsReadOnly[0] == FirstBlockNodeIndex);
                IEnumerator<IFocusBrowsingBlockNodeIndex> BlockNodeIndexListFocusEnumerator = ((ICollection<IFocusBrowsingBlockNodeIndex>)BlockNodeIndexList).GetEnumerator();

                // ILayoutBrowsingListNodeIndexList

                LayoutBrowsingListNodeIndexList ListNodeIndexList = LeafPathInner.AllIndexes() as LayoutBrowsingListNodeIndexList;
                Assert.That(ListNodeIndexList.Count > 0);
                FirstListNodeIndex = ListNodeIndexList[0];
                Assert.That(ListNodeIndexList.Contains(FirstListNodeIndex));
                Assert.That(ListNodeIndexList.IndexOf(FirstListNodeIndex) == 0);
                ListNodeIndexList.Remove(FirstListNodeIndex);
                ListNodeIndexList.Add(FirstListNodeIndex);
                ListNodeIndexList.Remove(FirstListNodeIndex);
                ListNodeIndexList.Insert(0, FirstListNodeIndex);

                IsReadOnly = ((ICollection<IReadOnlyBrowsingListNodeIndex>)ListNodeIndexList).IsReadOnly;
                IsReadOnly = ((IList<IReadOnlyBrowsingListNodeIndex>)ListNodeIndexList).IsReadOnly;
                ListNodeIndexList.CopyTo((IReadOnlyBrowsingListNodeIndex[])(new ILayoutBrowsingListNodeIndex[ListNodeIndexList.Count]), 0);
                IEnumerable<IReadOnlyBrowsingListNodeIndex> ListNodeIndexListAsReadOnlyEnumerable = ListNodeIndexList;
                foreach (IReadOnlyBrowsingListNodeIndex Item in ListNodeIndexListAsReadOnlyEnumerable)
                {
                    break;
                }
                IList<IReadOnlyBrowsingListNodeIndex> ListNodeIndexListAsReadOnlyIList = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsReadOnlyIList[0] == FirstListNodeIndex);
                IReadOnlyList<IReadOnlyBrowsingListNodeIndex> ListNodeIndexListAsReadOnlyIReadOnlylist = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsReadOnlyIReadOnlylist[0] == FirstListNodeIndex);

                IsReadOnly = ((ICollection<IWriteableBrowsingListNodeIndex>)ListNodeIndexList).IsReadOnly;
                IsReadOnly = ((IList<IWriteableBrowsingListNodeIndex>)ListNodeIndexList).IsReadOnly;
                Assert.That(((WriteableBrowsingListNodeIndexList)ListNodeIndexList)[0] == FirstListNodeIndex);
                ListNodeIndexList.CopyTo((IWriteableBrowsingListNodeIndex[])(new ILayoutBrowsingListNodeIndex[ListNodeIndexList.Count]), 0);
                IEnumerable<IWriteableBrowsingListNodeIndex> ListNodeIndexListAsWriteableEnumerable = ListNodeIndexList;
                foreach (IWriteableBrowsingListNodeIndex Item in ListNodeIndexListAsWriteableEnumerable)
                {
                    break;
                }
                IList<IWriteableBrowsingListNodeIndex> ListNodeIndexListAsWriteableIList = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsWriteableIList[0] == FirstListNodeIndex);
                Assert.That(ListNodeIndexListAsWriteableIList.Contains((IWriteableBrowsingListNodeIndex)FirstListNodeIndex));
                Assert.That(ListNodeIndexListAsWriteableIList.IndexOf((IWriteableBrowsingListNodeIndex)FirstListNodeIndex) == 0);
                ICollection<IWriteableBrowsingListNodeIndex> BrowsingListNodeIndexListAsWriteableICollection = ListNodeIndexList;
                Assert.That(BrowsingListNodeIndexListAsWriteableICollection.Contains((IWriteableBrowsingListNodeIndex)FirstListNodeIndex));
                BrowsingListNodeIndexListAsWriteableICollection.Remove((IWriteableBrowsingListNodeIndex)FirstListNodeIndex);
                BrowsingListNodeIndexListAsWriteableICollection.Add((IWriteableBrowsingListNodeIndex)FirstListNodeIndex);
                BrowsingListNodeIndexListAsWriteableICollection.Remove((IWriteableBrowsingListNodeIndex)FirstListNodeIndex);
                ListNodeIndexListAsWriteableIList.Insert(0, (IWriteableBrowsingListNodeIndex)FirstListNodeIndex);
                IReadOnlyList<IWriteableBrowsingListNodeIndex> ListNodeIndexListAsWriteableIReadOnlylist = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsWriteableIReadOnlylist[0] == FirstListNodeIndex);
                IEnumerator<IWriteableBrowsingListNodeIndex> ListNodeIndexListWriteableEnumerator = ((ICollection<IWriteableBrowsingListNodeIndex>)ListNodeIndexList).GetEnumerator();

                IsReadOnly = ((ICollection<IFrameBrowsingListNodeIndex>)ListNodeIndexList).IsReadOnly;
                IsReadOnly = ((IList<IFrameBrowsingListNodeIndex>)ListNodeIndexList).IsReadOnly;
                Assert.That(((FrameBrowsingListNodeIndexList)ListNodeIndexList)[0] == FirstListNodeIndex);
                ListNodeIndexList.CopyTo((IFrameBrowsingListNodeIndex[])(new ILayoutBrowsingListNodeIndex[ListNodeIndexList.Count]), 0);
                IEnumerable<IFrameBrowsingListNodeIndex> ListNodeIndexListAsFrameEnumerable = ListNodeIndexList;
                foreach (IFrameBrowsingListNodeIndex Item in ListNodeIndexListAsFrameEnumerable)
                {
                    break;
                }
                IList<IFrameBrowsingListNodeIndex> ListNodeIndexListAsFrameIList = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsFrameIList[0] == FirstListNodeIndex);
                Assert.That(ListNodeIndexListAsFrameIList.Contains((IFrameBrowsingListNodeIndex)FirstListNodeIndex));
                Assert.That(ListNodeIndexListAsFrameIList.IndexOf((IFrameBrowsingListNodeIndex)FirstListNodeIndex) == 0);
                ICollection<IFrameBrowsingListNodeIndex> BrowsingListNodeIndexListAsFrameICollection = ListNodeIndexList;
                Assert.That(BrowsingListNodeIndexListAsFrameICollection.Contains((IFrameBrowsingListNodeIndex)FirstListNodeIndex));
                BrowsingListNodeIndexListAsFrameICollection.Remove((IFrameBrowsingListNodeIndex)FirstListNodeIndex);
                BrowsingListNodeIndexListAsFrameICollection.Add((IFrameBrowsingListNodeIndex)FirstListNodeIndex);
                BrowsingListNodeIndexListAsFrameICollection.Remove((IFrameBrowsingListNodeIndex)FirstListNodeIndex);
                ListNodeIndexListAsFrameIList.Insert(0, (IFrameBrowsingListNodeIndex)FirstListNodeIndex);
                IReadOnlyList<IFrameBrowsingListNodeIndex> ListNodeIndexListAsFrameIReadOnlylist = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsFrameIReadOnlylist[0] == FirstListNodeIndex);
                ReadOnlyBrowsingListNodeIndexList FrameListNodeIndexListAsReadOnly = ListNodeIndexList;
                Assert.That(FrameListNodeIndexListAsReadOnly[0] == FirstListNodeIndex);
                IEnumerator<IFrameBrowsingListNodeIndex> ListNodeIndexListFrameEnumerator = ((ICollection<IFrameBrowsingListNodeIndex>)ListNodeIndexList).GetEnumerator();

                IsReadOnly = ((ICollection<IFocusBrowsingListNodeIndex>)ListNodeIndexList).IsReadOnly;
                IsReadOnly = ((IList<IFocusBrowsingListNodeIndex>)ListNodeIndexList).IsReadOnly;
                Assert.That(((FocusBrowsingListNodeIndexList)ListNodeIndexList)[0] == FirstListNodeIndex);
                ListNodeIndexList.CopyTo((IFocusBrowsingListNodeIndex[])(new ILayoutBrowsingListNodeIndex[ListNodeIndexList.Count]), 0);
                IEnumerable<IFocusBrowsingListNodeIndex> ListNodeIndexListAsFocusEnumerable = ListNodeIndexList;
                foreach (IFocusBrowsingListNodeIndex Item in ListNodeIndexListAsFocusEnumerable)
                {
                    break;
                }
                IList<IFocusBrowsingListNodeIndex> ListNodeIndexListAsFocusIList = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsFocusIList[0] == FirstListNodeIndex);
                Assert.That(ListNodeIndexListAsFocusIList.Contains((IFocusBrowsingListNodeIndex)FirstListNodeIndex));
                Assert.That(ListNodeIndexListAsFocusIList.IndexOf((IFocusBrowsingListNodeIndex)FirstListNodeIndex) == 0);
                ICollection<IFocusBrowsingListNodeIndex> BrowsingListNodeIndexListAsFocusICollection = ListNodeIndexList;
                Assert.That(BrowsingListNodeIndexListAsFocusICollection.Contains((IFocusBrowsingListNodeIndex)FirstListNodeIndex));
                BrowsingListNodeIndexListAsFocusICollection.Remove((IFocusBrowsingListNodeIndex)FirstListNodeIndex);
                BrowsingListNodeIndexListAsFocusICollection.Add((IFocusBrowsingListNodeIndex)FirstListNodeIndex);
                BrowsingListNodeIndexListAsFocusICollection.Remove((IFocusBrowsingListNodeIndex)FirstListNodeIndex);
                ListNodeIndexListAsFocusIList.Insert(0, (IFocusBrowsingListNodeIndex)FirstListNodeIndex);
                IReadOnlyList<IFocusBrowsingListNodeIndex> ListNodeIndexListAsFocusIReadOnlylist = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsFocusIReadOnlylist[0] == FirstListNodeIndex);
                ReadOnlyBrowsingListNodeIndexList FocusListNodeIndexListAsReadOnly = ListNodeIndexList;
                Assert.That(FocusListNodeIndexListAsReadOnly[0] == FirstListNodeIndex);
                IEnumerator<IFocusBrowsingListNodeIndex> ListNodeIndexListFocusEnumerator = ((ICollection<IFocusBrowsingListNodeIndex>)ListNodeIndexList).GetEnumerator();

                // ILayoutIndexNodeStateDictionary

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
                    IDictionary<IReadOnlyIndex, IReadOnlyNodeState> ReadOnlyControllerStateTableAsDictionary = ControllerStateTable;
                    foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in ReadOnlyControllerStateTableAsDictionary)
                    {
                        IReadOnlyNodeState StateView = ReadOnlyControllerStateTableAsDictionary[Entry.Key];
                        Assert.That(ReadOnlyControllerStateTableAsDictionary.ContainsKey(Entry.Key));
                        break;
                    }
                    ICollection<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> ReadOnlyControllerStateTableAsCollection = ControllerStateTable;
                    IsReadOnly = ReadOnlyControllerStateTableAsCollection.IsReadOnly;
                    foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in ReadOnlyControllerStateTableAsCollection)
                    {
                        Assert.That(ReadOnlyControllerStateTableAsCollection.Contains(Entry));
                        ReadOnlyControllerStateTableAsCollection.Remove(Entry);
                        ReadOnlyControllerStateTableAsCollection.Add(Entry);
                        ReadOnlyControllerStateTableAsCollection.CopyTo(new KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>[ReadOnlyControllerStateTableAsCollection.Count], 0);
                        break;
                    }
                    IEnumerable<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> ReadOnlyControllerStateTableAsEnumerable = ControllerStateTable;
                    foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in ReadOnlyControllerStateTableAsEnumerable)
                    {
                        break;
                    }

                    WriteableNodeStateDictionary WriteableControllerStateTable = ControllerStateTable as WriteableNodeStateDictionary;
                    foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in (ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)WriteableControllerStateTable)
                    {
                        break;
                    }
                    IDictionary<IWriteableIndex, IWriteableNodeState> WriteableControllerStateTableAsDictionary = ControllerStateTable as IDictionary<IWriteableIndex, IWriteableNodeState>;
                    foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in WriteableControllerStateTableAsDictionary)
                    {
                        IWriteableNodeState StateView = WriteableControllerStateTableAsDictionary[Entry.Key];
                        Assert.That(WriteableControllerStateTableAsDictionary.ContainsKey(Entry.Key));
                        WriteableControllerStateTableAsDictionary.Remove(Entry.Key);
                        WriteableControllerStateTableAsDictionary.Add(Entry.Key, Entry.Value);
                        Assert.That(WriteableControllerStateTableAsDictionary.Keys != null);
                        Assert.That(WriteableControllerStateTableAsDictionary.Values != null);
                        Assert.That(WriteableControllerStateTableAsDictionary.TryGetValue(Entry.Key, out IWriteableNodeState Value));
                        break;
                    }
                    ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>> WriteableControllerStateTableAsCollection = ControllerStateTable as ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>;
                    IsReadOnly = WriteableControllerStateTableAsCollection.IsReadOnly;
                    foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in WriteableControllerStateTableAsCollection)
                    {
                        Assert.That(WriteableControllerStateTableAsCollection.Contains(Entry));
                        WriteableControllerStateTableAsCollection.Remove(Entry);
                        WriteableControllerStateTableAsCollection.Add(Entry);
                        WriteableControllerStateTableAsCollection.CopyTo(new KeyValuePair<IWriteableIndex, IWriteableNodeState>[WriteableControllerStateTableAsCollection.Count], 0);
                        break;
                    }
                    IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>> WriteableControllerStateTableAsEnumerable = ControllerStateTable as IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>;
                    foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in WriteableControllerStateTableAsEnumerable)
                    {
                        break;
                    }

                    foreach (KeyValuePair<IFrameIndex, IFrameNodeState> Entry in (ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>)ControllerStateTable)
                    {
                        break;
                    }
                    IDictionary<IFrameIndex, IFrameNodeState> FrameControllerStateTableAsDictionary = ControllerStateTable as IDictionary<IFrameIndex, IFrameNodeState>;
                    foreach (KeyValuePair<IFrameIndex, IFrameNodeState> Entry in FrameControllerStateTableAsDictionary)
                    {
                        IFrameNodeState StateView = FrameControllerStateTableAsDictionary[Entry.Key];
                        Assert.That(FrameControllerStateTableAsDictionary.ContainsKey(Entry.Key));
                        FrameControllerStateTableAsDictionary.Remove(Entry.Key);
                        FrameControllerStateTableAsDictionary.Add(Entry.Key, Entry.Value);
                        Assert.That(FrameControllerStateTableAsDictionary.Keys != null);
                        Assert.That(FrameControllerStateTableAsDictionary.Values != null);
                        Assert.That(FrameControllerStateTableAsDictionary.TryGetValue(Entry.Key, out IFrameNodeState Value));
                        break;
                    }
                    ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>> FrameControllerStateTableAsCollection = ControllerStateTable as ICollection<KeyValuePair<IFrameIndex, IFrameNodeState>>;
                    IsReadOnly = FrameControllerStateTableAsCollection.IsReadOnly;
                    foreach (KeyValuePair<IFrameIndex, IFrameNodeState> Entry in FrameControllerStateTableAsCollection)
                    {
                        Assert.That(FrameControllerStateTableAsCollection.Contains(Entry));
                        FrameControllerStateTableAsCollection.Remove(Entry);
                        FrameControllerStateTableAsCollection.Add(Entry);
                        FrameControllerStateTableAsCollection.CopyTo(new KeyValuePair<IFrameIndex, IFrameNodeState>[FrameControllerStateTableAsCollection.Count], 0);
                        break;
                    }
                    IEnumerable<KeyValuePair<IFrameIndex, IFrameNodeState>> FrameControllerStateTableAsEnumerable = ControllerStateTable as IEnumerable<KeyValuePair<IFrameIndex, IFrameNodeState>>;
                    foreach (KeyValuePair<IFrameIndex, IFrameNodeState> Entry in FrameControllerStateTableAsEnumerable)
                    {
                        break;
                    }

                    foreach (KeyValuePair<IFocusIndex, IFocusNodeState> Entry in (ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>)ControllerStateTable)
                    {
                        break;
                    }
                    IDictionary<IFocusIndex, IFocusNodeState> FocusControllerStateTableAsDictionary = ControllerStateTable as IDictionary<IFocusIndex, IFocusNodeState>;
                    foreach (KeyValuePair<IFocusIndex, IFocusNodeState> Entry in FocusControllerStateTableAsDictionary)
                    {
                        IFocusNodeState StateView = FocusControllerStateTableAsDictionary[Entry.Key];
                        Assert.That(FocusControllerStateTableAsDictionary.ContainsKey(Entry.Key));
                        FocusControllerStateTableAsDictionary.Remove(Entry.Key);
                        FocusControllerStateTableAsDictionary.Add(Entry.Key, Entry.Value);
                        Assert.That(FocusControllerStateTableAsDictionary.Keys != null);
                        Assert.That(FocusControllerStateTableAsDictionary.Values != null);
                        Assert.That(FocusControllerStateTableAsDictionary.TryGetValue(Entry.Key, out IFocusNodeState Value));
                        break;
                    }
                    ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>> FocusControllerStateTableAsCollection = ControllerStateTable as ICollection<KeyValuePair<IFocusIndex, IFocusNodeState>>;
                    IsReadOnly = FocusControllerStateTableAsCollection.IsReadOnly;
                    foreach (KeyValuePair<IFocusIndex, IFocusNodeState> Entry in FocusControllerStateTableAsCollection)
                    {
                        Assert.That(FocusControllerStateTableAsCollection.Contains(Entry));
                        FocusControllerStateTableAsCollection.Remove(Entry);
                        FocusControllerStateTableAsCollection.Add(Entry);
                        FocusControllerStateTableAsCollection.CopyTo(new KeyValuePair<IFocusIndex, IFocusNodeState>[FocusControllerStateTableAsCollection.Count], 0);
                        break;
                    }
                    IEnumerable<KeyValuePair<IFocusIndex, IFocusNodeState>> FocusControllerStateTableAsEnumerable = ControllerStateTable as IEnumerable<KeyValuePair<IFocusIndex, IFocusNodeState>>;
                    foreach (KeyValuePair<IFocusIndex, IFocusNodeState> Entry in FocusControllerStateTableAsEnumerable)
                    {
                        break;
                    }
                }

                // ILayoutIndexNodeStateReadOnlyDictionary

                ReadOnlyNodeStateReadOnlyDictionary ReadOnlyStateTable = Controller.StateTable;

                IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState> ReadOnlyStateTableAsDictionary = ReadOnlyStateTable;
                Assert.That(ReadOnlyStateTable.TryGetValue(RootIndex, out IReadOnlyNodeState ReadOnlyRootStateValue) == ReadOnlyStateTableAsDictionary.TryGetValue(RootIndex, out IReadOnlyNodeState ReadOnlyRootStateValueFromDictionary) && ReadOnlyRootStateValue == ReadOnlyRootStateValueFromDictionary);
                Assert.That(ReadOnlyStateTableAsDictionary.Keys != null);
                Assert.That(ReadOnlyStateTableAsDictionary.Values != null);
                ReadOnlyStateTableAsDictionary.GetEnumerator();

                WriteableNodeStateReadOnlyDictionary WriteableStateTable = Controller.StateTable;
                Assert.That(WriteableStateTable.ContainsKey(RootIndex));
                Assert.That(WriteableStateTable[RootIndex] == ReadOnlyStateTable[RootIndex]);
                WriteableStateTable.GetEnumerator();
                IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState> WriteableStateTableAsDictionary = ReadOnlyStateTable as IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>;
                Assert.That(WriteableStateTable.TryGetValue(RootIndex, out IReadOnlyNodeState WriteableRootStateValue) == WriteableStateTableAsDictionary.TryGetValue(RootIndex, out IWriteableNodeState WriteableRootStateValueFromDictionary) && WriteableRootStateValue == WriteableRootStateValueFromDictionary);
                Assert.That(WriteableStateTableAsDictionary.ContainsKey(RootIndex));
                Assert.That(WriteableStateTableAsDictionary[RootIndex] == ReadOnlyStateTable[RootIndex]);
                Assert.That(WriteableStateTableAsDictionary.Keys != null);
                Assert.That(WriteableStateTableAsDictionary.Values != null);
                IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>> WriteableStateTableAsEnumerable = ReadOnlyStateTable as IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>;
                WriteableStateTableAsEnumerable.GetEnumerator();

                FrameNodeStateReadOnlyDictionary FrameStateTable = Controller.StateTable;
                Assert.That(FrameStateTable.ContainsKey(RootIndex));
                Assert.That(FrameStateTable[RootIndex] == ReadOnlyStateTable[RootIndex]);
                FrameStateTable.GetEnumerator();
                IReadOnlyDictionary<IFrameIndex, IFrameNodeState> FrameStateTableAsDictionary = ReadOnlyStateTable as IReadOnlyDictionary<IFrameIndex, IFrameNodeState>;
                Assert.That(FrameStateTable.TryGetValue(RootIndex, out IReadOnlyNodeState FrameRootStateValue) == FrameStateTableAsDictionary.TryGetValue(RootIndex, out IFrameNodeState FrameRootStateValueFromDictionary) && FrameRootStateValue == FrameRootStateValueFromDictionary);
                Assert.That(FrameStateTableAsDictionary.ContainsKey(RootIndex));
                Assert.That(FrameStateTableAsDictionary[RootIndex] == ReadOnlyStateTable[RootIndex]);
                Assert.That(FrameStateTableAsDictionary.Keys != null);
                Assert.That(FrameStateTableAsDictionary.Values != null);
                IEnumerable<KeyValuePair<IFrameIndex, IFrameNodeState>> FrameStateTableAsEnumerable = ReadOnlyStateTable as IEnumerable<KeyValuePair<IFrameIndex, IFrameNodeState>>;
                FrameStateTableAsEnumerable.GetEnumerator();

                FocusNodeStateReadOnlyDictionary FocusStateTable = Controller.StateTable;
                Assert.That(FocusStateTable.ContainsKey(RootIndex));
                Assert.That(FocusStateTable[RootIndex] == ReadOnlyStateTable[RootIndex]);
                FocusStateTable.GetEnumerator();
                IReadOnlyDictionary<IFocusIndex, IFocusNodeState> FocusStateTableAsDictionary = ReadOnlyStateTable as IReadOnlyDictionary<IFocusIndex, IFocusNodeState>;
                Assert.That(FocusStateTable.TryGetValue(RootIndex, out IReadOnlyNodeState FocusRootStateValue) == FocusStateTableAsDictionary.TryGetValue(RootIndex, out IFocusNodeState FocusRootStateValueFromDictionary) && FocusRootStateValue == FocusRootStateValueFromDictionary);
                Assert.That(FocusStateTableAsDictionary.ContainsKey(RootIndex));
                Assert.That(FocusStateTableAsDictionary[RootIndex] == ReadOnlyStateTable[RootIndex]);
                Assert.That(FocusStateTableAsDictionary.Keys != null);
                Assert.That(FocusStateTableAsDictionary.Values != null);
                IEnumerable<KeyValuePair<IFocusIndex, IFocusNodeState>> FocusStateTableAsEnumerable = ReadOnlyStateTable as IEnumerable<KeyValuePair<IFocusIndex, IFocusNodeState>>;
                FocusStateTableAsEnumerable.GetEnumerator();

                // ILayoutInnerDictionary

                LayoutInnerDictionary<string> LayoutInnerTableModify = DebugObjects.GetReferenceByInterface(Type.FromTypeof<LayoutInnerDictionary<string>>()) as LayoutInnerDictionary<string>;
                Assert.That(LayoutInnerTableModify != null);
                Assert.That(LayoutInnerTableModify.Count > 0);

                IDictionary<string, IReadOnlyInner> ReadOnlyInnerTableModifyAsDictionary = LayoutInnerTableModify;
                Assert.That(ReadOnlyInnerTableModifyAsDictionary.Keys != null);
                Assert.That(ReadOnlyInnerTableModifyAsDictionary.Values != null);
                foreach (KeyValuePair<string, ILayoutInner> Entry in (ICollection<KeyValuePair<string, ILayoutInner>>)LayoutInnerTableModify)
                {
                    Assert.That(ReadOnlyInnerTableModifyAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(ReadOnlyInnerTableModifyAsDictionary[Entry.Key] == Entry.Value);
                }
                ICollection<KeyValuePair<string, IReadOnlyInner>> ReadOnlyInnerTableModifyAsCollection = LayoutInnerTableModify;
                Assert.That(!ReadOnlyInnerTableModifyAsCollection.IsReadOnly);
                IEnumerable<KeyValuePair<string, IReadOnlyInner>> ReadOnlyInnerTableModifyAsEnumerable = LayoutInnerTableModify;
                IEnumerator<KeyValuePair<string, IReadOnlyInner>> ReadOnlyInnerTableModifyAsEnumerableEnumerator = ReadOnlyInnerTableModifyAsEnumerable.GetEnumerator();
                foreach (KeyValuePair<string, IReadOnlyInner> Entry in ReadOnlyInnerTableModifyAsEnumerable)
                {
                    Assert.That(ReadOnlyInnerTableModifyAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(ReadOnlyInnerTableModifyAsDictionary[Entry.Key] == Entry.Value);
                    Assert.That(LayoutInnerTableModify.TryGetValue(Entry.Key, out IReadOnlyInner ReadOnlyInnerValue) == LayoutInnerTableModify.TryGetValue(Entry.Key, out IReadOnlyInner LayoutInnerValue));

                    Assert.That(((ICollection<KeyValuePair<string, IReadOnlyInner>>)LayoutInnerTableModify).Contains(Entry));
                    ((ICollection<KeyValuePair<string, IReadOnlyInner>>)LayoutInnerTableModify).Remove(Entry);
                    ((ICollection<KeyValuePair<string, IReadOnlyInner>>)LayoutInnerTableModify).Add(Entry);
                    ((ICollection<KeyValuePair<string, IReadOnlyInner>>)LayoutInnerTableModify).CopyTo(new KeyValuePair<string, IReadOnlyInner>[LayoutInnerTableModify.Count], 0);
                    break;
                }

                WriteableInnerDictionary<string> WriteableInnerTableModify = LayoutInnerTableModify;
                WriteableInnerTableModify.GetEnumerator();
                IDictionary<string, IWriteableInner> WriteableInnerTableModifyAsDictionary = LayoutInnerTableModify;
                Assert.That(WriteableInnerTableModifyAsDictionary.Keys != null);
                Assert.That(WriteableInnerTableModifyAsDictionary.Values != null);
                foreach (KeyValuePair<string, ILayoutInner> Entry in (ICollection<KeyValuePair<string, ILayoutInner>>)LayoutInnerTableModify)
                {
                    Assert.That(WriteableInnerTableModify[Entry.Key] == Entry.Value);
                    Assert.That(WriteableInnerTableModifyAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(WriteableInnerTableModifyAsDictionary[Entry.Key] == Entry.Value);
                    WriteableInnerTableModifyAsDictionary.Remove(Entry.Key);
                    WriteableInnerTableModifyAsDictionary.Add(Entry.Key, Entry.Value);
                    WriteableInnerTableModifyAsDictionary.TryGetValue(Entry.Key, out IWriteableInner WriteableInnerValue);
                    break;
                }
                ICollection<KeyValuePair<string, IWriteableInner>> WriteableInnerTableModifyAsCollection = LayoutInnerTableModify;
                Assert.That(!WriteableInnerTableModifyAsCollection.IsReadOnly);
                WriteableInnerTableModifyAsCollection.CopyTo(new KeyValuePair<string, IWriteableInner>[WriteableInnerTableModifyAsCollection.Count], 0);
                foreach (KeyValuePair<string, IWriteableInner> Entry in WriteableInnerTableModifyAsCollection)
                {
                    Assert.That(WriteableInnerTableModifyAsCollection.Contains(Entry));
                    WriteableInnerTableModifyAsCollection.Remove(Entry);
                    WriteableInnerTableModifyAsCollection.Add(Entry);
                    break;
                }
                IEnumerable<KeyValuePair<string, IWriteableInner>> WriteableInnerTableModifyAsEnumerable = LayoutInnerTableModify;
                IEnumerator<KeyValuePair<string, IWriteableInner>> WriteableInnerTableModifyAsEnumerableEnumerator = WriteableInnerTableModifyAsEnumerable.GetEnumerator();

                FrameInnerDictionary<string> FrameInnerTableModify = LayoutInnerTableModify;
                FrameInnerTableModify.GetEnumerator();
                IDictionary<string, IFrameInner> FrameInnerTableModifyAsDictionary = LayoutInnerTableModify;
                Assert.That(FrameInnerTableModifyAsDictionary.Keys != null);
                Assert.That(FrameInnerTableModifyAsDictionary.Values != null);
                foreach (KeyValuePair<string, ILayoutInner> Entry in (ICollection<KeyValuePair<string, ILayoutInner>>)LayoutInnerTableModify)
                {
                    Assert.That(FrameInnerTableModify[Entry.Key] == Entry.Value);
                    Assert.That(FrameInnerTableModifyAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(FrameInnerTableModifyAsDictionary[Entry.Key] == Entry.Value);
                    FrameInnerTableModifyAsDictionary.Remove(Entry.Key);
                    FrameInnerTableModifyAsDictionary.Add(Entry.Key, Entry.Value);
                    FrameInnerTableModifyAsDictionary.TryGetValue(Entry.Key, out IFrameInner FrameInnerValue);
                    break;
                }
                ICollection<KeyValuePair<string, IFrameInner>> FrameInnerTableModifyAsCollection = LayoutInnerTableModify;
                Assert.That(!FrameInnerTableModifyAsCollection.IsReadOnly);
                FrameInnerTableModifyAsCollection.CopyTo(new KeyValuePair<string, IFrameInner>[FrameInnerTableModifyAsCollection.Count], 0);
                foreach (KeyValuePair<string, IFrameInner> Entry in FrameInnerTableModifyAsCollection)
                {
                    Assert.That(FrameInnerTableModifyAsCollection.Contains(Entry));
                    FrameInnerTableModifyAsCollection.Remove(Entry);
                    FrameInnerTableModifyAsCollection.Add(Entry);
                    break;
                }
                IEnumerable<KeyValuePair<string, IFrameInner>> FrameInnerTableModifyAsEnumerable = LayoutInnerTableModify;
                IEnumerator<KeyValuePair<string, IFrameInner>> FrameInnerTableModifyAsEnumerableEnumerator = FrameInnerTableModifyAsEnumerable.GetEnumerator();

                FocusInnerDictionary<string> FocusInnerTableModify = LayoutInnerTableModify;
                FocusInnerTableModify.GetEnumerator();
                IDictionary<string, IFocusInner> FocusInnerTableModifyAsDictionary = LayoutInnerTableModify;
                Assert.That(FocusInnerTableModifyAsDictionary.Keys != null);
                Assert.That(FocusInnerTableModifyAsDictionary.Values != null);
                foreach (KeyValuePair<string, ILayoutInner> Entry in (ICollection<KeyValuePair<string, ILayoutInner>>)LayoutInnerTableModify)
                {
                    Assert.That(FocusInnerTableModify[Entry.Key] == Entry.Value);
                    Assert.That(FocusInnerTableModifyAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(FocusInnerTableModifyAsDictionary[Entry.Key] == Entry.Value);
                    FocusInnerTableModifyAsDictionary.Remove(Entry.Key);
                    FocusInnerTableModifyAsDictionary.Add(Entry.Key, Entry.Value);
                    FocusInnerTableModifyAsDictionary.TryGetValue(Entry.Key, out IFocusInner FocusInnerValue);
                    break;
                }
                ICollection<KeyValuePair<string, IFocusInner>> FocusInnerTableModifyAsCollection = LayoutInnerTableModify;
                Assert.That(!FocusInnerTableModifyAsCollection.IsReadOnly);
                FocusInnerTableModifyAsCollection.CopyTo(new KeyValuePair<string, IFocusInner>[FocusInnerTableModifyAsCollection.Count], 0);
                foreach (KeyValuePair<string, IFocusInner> Entry in FocusInnerTableModifyAsCollection)
                {
                    Assert.That(FocusInnerTableModifyAsCollection.Contains(Entry));
                    FocusInnerTableModifyAsCollection.Remove(Entry);
                    FocusInnerTableModifyAsCollection.Add(Entry);
                    break;
                }
                IEnumerable<KeyValuePair<string, IFocusInner>> FocusInnerTableModifyAsEnumerable = LayoutInnerTableModify;
                IEnumerator<KeyValuePair<string, IFocusInner>> FocusInnerTableModifyAsEnumerableEnumerator = FocusInnerTableModifyAsEnumerable.GetEnumerator();


                // ILayoutInnerReadOnlyDictionary

                LayoutInnerReadOnlyDictionary<string> LayoutInnerTable = RootState.InnerTable;

                IReadOnlyDictionary<string, IReadOnlyInner> ReadOnlyInnerTableAsDictionary = LayoutInnerTable;
                Assert.That(ReadOnlyInnerTableAsDictionary.Keys != null);
                Assert.That(ReadOnlyInnerTableAsDictionary.Values != null);
                foreach (KeyValuePair<string, ILayoutInner> Entry in (ICollection<KeyValuePair<string, ILayoutInner>>)LayoutInnerTable)
                {
                    Assert.That(LayoutInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner ReadOnlyInnerValue) == LayoutInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner LayoutInnerValue));
                    break;
                }

                WriteableInnerReadOnlyDictionary<string> WriteableInnerTable = RootState.InnerTable;
                IReadOnlyDictionary<string, IWriteableInner> WriteableInnerTableAsDictionary = LayoutInnerTable;
                Assert.That(WriteableInnerTableAsDictionary.Keys != null);
                Assert.That(WriteableInnerTableAsDictionary.Values != null);
                IEnumerable<KeyValuePair<string, IWriteableInner>> WriteableInnerTableAsIEnumerable = LayoutInnerTable;
                WriteableInnerTableAsIEnumerable.GetEnumerator();
                foreach (KeyValuePair<string, ILayoutInner> Entry in (ICollection<KeyValuePair<string, ILayoutInner>>)LayoutInnerTable)
                {
                    Assert.That(WriteableInnerTableAsDictionary[Entry.Key] == Entry.Value);
                    Assert.That(LayoutInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner WriteableInnerValue) == LayoutInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner LayoutInnerValue));
                    break;
                }

                FrameInnerReadOnlyDictionary<string> FrameInnerTable = RootState.InnerTable;
                IReadOnlyDictionary<string, IFrameInner> FrameInnerTableAsDictionary = LayoutInnerTable;
                Assert.That(FrameInnerTableAsDictionary.Keys != null);
                Assert.That(FrameInnerTableAsDictionary.Values != null);
                IEnumerable<KeyValuePair<string, IFrameInner>> FrameInnerTableAsIEnumerable = LayoutInnerTable;
                FrameInnerTableAsIEnumerable.GetEnumerator();
                foreach (KeyValuePair<string, ILayoutInner> Entry in (ICollection<KeyValuePair<string, ILayoutInner>>)LayoutInnerTable)
                {
                    Assert.That(FrameInnerTableAsDictionary[Entry.Key] == Entry.Value);
                    Assert.That(LayoutInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner FrameInnerValue) == LayoutInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner LayoutInnerValue));
                    break;
                }

                FocusInnerReadOnlyDictionary<string> FocusInnerTable = RootState.InnerTable;
                IReadOnlyDictionary<string, IFocusInner> FocusInnerTableAsDictionary = LayoutInnerTable;
                Assert.That(FocusInnerTableAsDictionary.Keys != null);
                Assert.That(FocusInnerTableAsDictionary.Values != null);
                IEnumerable<KeyValuePair<string, IFocusInner>> FocusInnerTableAsIEnumerable = LayoutInnerTable;
                FocusInnerTableAsIEnumerable.GetEnumerator();
                foreach (KeyValuePair<string, ILayoutInner> Entry in (ICollection<KeyValuePair<string, ILayoutInner>>)LayoutInnerTable)
                {
                    Assert.That(FocusInnerTableAsDictionary[Entry.Key] == Entry.Value);
                    Assert.That(LayoutInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner FocusInnerValue) == LayoutInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner LayoutInnerValue));
                    break;
                }

                // LayoutNodeStateList

                FirstNodeState = LeafPathInner.FirstNodeState;
                Assert.That(FirstNodeState != null);
                LayoutNodeStateList LayoutNodeStateListModify = DebugObjects.GetReferenceByInterface(Type.FromTypeof<LayoutNodeStateList>()) as LayoutNodeStateList;
                Assert.That(LayoutNodeStateListModify != null);
                Assert.That(LayoutNodeStateListModify.Count > 0);
                FirstNodeState = LayoutNodeStateListModify[0] as ILayoutPlaceholderNodeState;

                Assert.That(LayoutNodeStateListModify.Contains((IReadOnlyNodeState)FirstNodeState));
                Assert.That(LayoutNodeStateListModify.IndexOf((IReadOnlyNodeState)FirstNodeState) == 0);
                LayoutNodeStateListModify.Remove((IReadOnlyNodeState)FirstNodeState);
                LayoutNodeStateListModify.Insert(0, (IReadOnlyNodeState)FirstNodeState);
                LayoutNodeStateListModify.CopyTo((IReadOnlyNodeState[])(new ILayoutNodeState[LayoutNodeStateListModify.Count]), 0);
                ReadOnlyNodeStateList LayoutNodeStateListModifyAsReadOnly = LayoutNodeStateListModify as ReadOnlyNodeStateList;
                Assert.That(LayoutNodeStateListModifyAsReadOnly != null);
                Assert.That(LayoutNodeStateListModifyAsReadOnly[0] == LayoutNodeStateListModify[0]);
                IList<IReadOnlyNodeState> ReadOnlyNodeStateListModifyAsIList = LayoutNodeStateListModify as IList<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListModifyAsIList != null);
                Assert.That(ReadOnlyNodeStateListModifyAsIList[0] == LayoutNodeStateListModify[0]);
                IReadOnlyList<IReadOnlyNodeState> ReadOnlyNodeStateListModifyAsIReadOnlyList = LayoutNodeStateListModify as IReadOnlyList<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListModifyAsIReadOnlyList != null);
                Assert.That(ReadOnlyNodeStateListModifyAsIReadOnlyList[0] == LayoutNodeStateListModify[0]);
                ICollection<IReadOnlyNodeState> ReadOnlyNodeStateListModifyAsCollection = LayoutNodeStateListModify as ICollection<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListModifyAsCollection != null);
                Assert.That(!ReadOnlyNodeStateListModifyAsCollection.IsReadOnly);
                IEnumerable<IReadOnlyNodeState> ReadOnlyNodeStateListModifyAsEnumerable = LayoutNodeStateListModify as IEnumerable<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListModifyAsEnumerable != null);
                Assert.That(ReadOnlyNodeStateListModifyAsEnumerable.GetEnumerator() != null);

                WriteableNodeStateList LayoutNodeStateListModifyAsWriteable = LayoutNodeStateListModify as WriteableNodeStateList;
                Assert.That(LayoutNodeStateListModifyAsWriteable != null);
                Assert.That(LayoutNodeStateListModifyAsWriteable[0] == LayoutNodeStateListModify[0]);
                LayoutNodeStateListModifyAsWriteable.GetEnumerator();
                IList<IWriteableNodeState> WriteableNodeStateListModifyAsIList = LayoutNodeStateListModify as IList<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListModifyAsIList != null);
                Assert.That(WriteableNodeStateListModifyAsIList[0] == LayoutNodeStateListModify[0]);
                Assert.That(WriteableNodeStateListModifyAsIList.IndexOf(FirstNodeState) == 0);
                WriteableNodeStateListModifyAsIList.Remove(FirstNodeState);
                WriteableNodeStateListModifyAsIList.Insert(0, FirstNodeState);
                IReadOnlyList<IWriteableNodeState> WriteableNodeStateListModifyAsIReadOnlyList = LayoutNodeStateListModify as IReadOnlyList<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListModifyAsIReadOnlyList != null);
                Assert.That(WriteableNodeStateListModifyAsIReadOnlyList[0] == LayoutNodeStateListModify[0]);
                ICollection<IWriteableNodeState> WriteableNodeStateListModifyAsCollection = LayoutNodeStateListModify as ICollection<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListModifyAsCollection != null);
                Assert.That(!WriteableNodeStateListModifyAsCollection.IsReadOnly);
                Assert.That(WriteableNodeStateListModifyAsCollection.Contains(FirstNodeState));
                WriteableNodeStateListModifyAsCollection.Remove(FirstNodeState);
                WriteableNodeStateListModifyAsCollection.Add(FirstNodeState);
                WriteableNodeStateListModifyAsCollection.Remove(FirstNodeState);
                LayoutNodeStateListModify.Insert(0, FirstNodeState);
                WriteableNodeStateListModifyAsCollection.CopyTo(new ILayoutNodeState[WriteableNodeStateListModifyAsCollection.Count], 0);
                IEnumerable<IWriteableNodeState> WriteableNodeStateListModifyAsEnumerable = LayoutNodeStateListModify as IEnumerable<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListModifyAsEnumerable != null);
                Assert.That(WriteableNodeStateListModifyAsEnumerable.GetEnumerator() != null);

                FrameNodeStateList LayoutNodeStateListModifyAsFrame = LayoutNodeStateListModify as FrameNodeStateList;
                Assert.That(LayoutNodeStateListModifyAsFrame != null);
                Assert.That(LayoutNodeStateListModifyAsFrame[0] == LayoutNodeStateListModify[0]);
                LayoutNodeStateListModifyAsFrame.GetEnumerator();
                IList<IFrameNodeState> FrameNodeStateListModifyAsIList = LayoutNodeStateListModify as IList<IFrameNodeState>;
                Assert.That(FrameNodeStateListModifyAsIList != null);
                Assert.That(FrameNodeStateListModifyAsIList[0] == LayoutNodeStateListModify[0]);
                Assert.That(FrameNodeStateListModifyAsIList.IndexOf(FirstNodeState) == 0);
                FrameNodeStateListModifyAsIList.Remove(FirstNodeState);
                FrameNodeStateListModifyAsIList.Insert(0, FirstNodeState);
                IReadOnlyList<IFrameNodeState> FrameNodeStateListModifyAsIReadOnlyList = LayoutNodeStateListModify as IReadOnlyList<IFrameNodeState>;
                Assert.That(FrameNodeStateListModifyAsIReadOnlyList != null);
                Assert.That(FrameNodeStateListModifyAsIReadOnlyList[0] == LayoutNodeStateListModify[0]);
                ICollection<IFrameNodeState> FrameNodeStateListModifyAsCollection = LayoutNodeStateListModify as ICollection<IFrameNodeState>;
                Assert.That(FrameNodeStateListModifyAsCollection != null);
                Assert.That(!FrameNodeStateListModifyAsCollection.IsReadOnly);
                Assert.That(FrameNodeStateListModifyAsCollection.Contains(FirstNodeState));
                FrameNodeStateListModifyAsCollection.Remove(FirstNodeState);
                FrameNodeStateListModifyAsCollection.Add(FirstNodeState);
                FrameNodeStateListModifyAsCollection.Remove(FirstNodeState);
                LayoutNodeStateListModify.Insert(0, FirstNodeState);
                FrameNodeStateListModifyAsCollection.CopyTo(new ILayoutNodeState[FrameNodeStateListModifyAsCollection.Count], 0);
                IEnumerable<IFrameNodeState> FrameNodeStateListModifyAsEnumerable = LayoutNodeStateListModify as IEnumerable<IFrameNodeState>;
                Assert.That(FrameNodeStateListModifyAsEnumerable != null);
                Assert.That(FrameNodeStateListModifyAsEnumerable.GetEnumerator() != null);

                FocusNodeStateList LayoutNodeStateListModifyAsFocus = LayoutNodeStateListModify as FocusNodeStateList;
                Assert.That(LayoutNodeStateListModifyAsFocus != null);
                Assert.That(LayoutNodeStateListModifyAsFocus[0] == LayoutNodeStateListModify[0]);
                LayoutNodeStateListModifyAsFocus.GetEnumerator();
                IList<IFocusNodeState> FocusNodeStateListModifyAsIList = LayoutNodeStateListModify as IList<IFocusNodeState>;
                Assert.That(FocusNodeStateListModifyAsIList != null);
                Assert.That(FocusNodeStateListModifyAsIList[0] == LayoutNodeStateListModify[0]);
                Assert.That(FocusNodeStateListModifyAsIList.IndexOf(FirstNodeState) == 0);
                FocusNodeStateListModifyAsIList.Remove(FirstNodeState);
                FocusNodeStateListModifyAsIList.Insert(0, FirstNodeState);
                IReadOnlyList<IFocusNodeState> FocusNodeStateListModifyAsIReadOnlyList = LayoutNodeStateListModify as IReadOnlyList<IFocusNodeState>;
                Assert.That(FocusNodeStateListModifyAsIReadOnlyList != null);
                Assert.That(FocusNodeStateListModifyAsIReadOnlyList[0] == LayoutNodeStateListModify[0]);
                ICollection<IFocusNodeState> FocusNodeStateListModifyAsCollection = LayoutNodeStateListModify as ICollection<IFocusNodeState>;
                Assert.That(FocusNodeStateListModifyAsCollection != null);
                Assert.That(!FocusNodeStateListModifyAsCollection.IsReadOnly);
                Assert.That(FocusNodeStateListModifyAsCollection.Contains(FirstNodeState));
                FocusNodeStateListModifyAsCollection.Remove(FirstNodeState);
                FocusNodeStateListModifyAsCollection.Add(FirstNodeState);
                FocusNodeStateListModifyAsCollection.Remove(FirstNodeState);
                LayoutNodeStateListModify.Insert(0, FirstNodeState);
                FocusNodeStateListModifyAsCollection.CopyTo(new ILayoutNodeState[FocusNodeStateListModifyAsCollection.Count], 0);
                IEnumerable<IFocusNodeState> FocusNodeStateListModifyAsEnumerable = LayoutNodeStateListModify as IEnumerable<IFocusNodeState>;
                Assert.That(FocusNodeStateListModifyAsEnumerable != null);
                Assert.That(FocusNodeStateListModifyAsEnumerable.GetEnumerator() != null);

                // LayoutNodeStateReadOnlyList

                LayoutNodeStateReadOnlyList LayoutNodeStateList = LayoutNodeStateListModify.ToReadOnly() as LayoutNodeStateReadOnlyList;
                Assert.That(LayoutNodeStateList != null);
                Assert.That(LayoutNodeStateList.Count > 0);
                FirstNodeState = LayoutNodeStateList[0] as ILayoutPlaceholderNodeState;

                Assert.That(LayoutNodeStateList.Contains((IReadOnlyNodeState)FirstNodeState));
                Assert.That(LayoutNodeStateList.IndexOf((IReadOnlyNodeState)FirstNodeState) == 0);
                IReadOnlyList<IReadOnlyNodeState> ReadOnlyNodeStateListAsIReadOnlyList = LayoutNodeStateList as IReadOnlyList<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListAsIReadOnlyList[0] == FirstNodeState);
                IEnumerable<IReadOnlyNodeState> ReadOnlyNodeStateListAsEnumerable = LayoutNodeStateList as IEnumerable<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListAsEnumerable != null);
                Assert.That(ReadOnlyNodeStateListAsEnumerable.GetEnumerator() != null);

                WriteableNodeStateReadOnlyList WriteableNodeStateList = LayoutNodeStateList;
                Assert.That(WriteableNodeStateList.Contains(FirstNodeState));
                Assert.That(WriteableNodeStateList.IndexOf(FirstNodeState) == 0);
                Assert.That(WriteableNodeStateList[0] == LayoutNodeStateList[0]);
                WriteableNodeStateList.GetEnumerator();
                IReadOnlyList<IWriteableNodeState> WriteableNodeStateListAsIReadOnlyList = LayoutNodeStateList as IReadOnlyList<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListAsIReadOnlyList[0] == FirstNodeState);
                IEnumerable<IWriteableNodeState> WriteableNodeStateListAsEnumerable = LayoutNodeStateList as IEnumerable<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListAsEnumerable != null);
                Assert.That(WriteableNodeStateListAsEnumerable.GetEnumerator() != null);

                FrameNodeStateReadOnlyList FrameNodeStateList = LayoutNodeStateList;
                Assert.That(FrameNodeStateList.Contains(FirstNodeState));
                Assert.That(FrameNodeStateList.IndexOf(FirstNodeState) == 0);
                Assert.That(FrameNodeStateList[0] == LayoutNodeStateList[0]);
                FrameNodeStateList.GetEnumerator();
                IReadOnlyList<IFrameNodeState> FrameNodeStateListAsIReadOnlyList = LayoutNodeStateList as IReadOnlyList<IFrameNodeState>;
                Assert.That(FrameNodeStateListAsIReadOnlyList[0] == FirstNodeState);
                IEnumerable<IFrameNodeState> FrameNodeStateListAsEnumerable = LayoutNodeStateList as IEnumerable<IFrameNodeState>;
                Assert.That(FrameNodeStateListAsEnumerable != null);
                Assert.That(FrameNodeStateListAsEnumerable.GetEnumerator() != null);

                FocusNodeStateReadOnlyList FocusNodeStateList = LayoutNodeStateList;
                Assert.That(FocusNodeStateList.Contains(FirstNodeState));
                Assert.That(FocusNodeStateList.IndexOf(FirstNodeState) == 0);
                Assert.That(FocusNodeStateList[0] == LayoutNodeStateList[0]);
                FocusNodeStateList.GetEnumerator();
                IReadOnlyList<IFocusNodeState> FocusNodeStateListAsIReadOnlyList = LayoutNodeStateList as IReadOnlyList<IFocusNodeState>;
                Assert.That(FocusNodeStateListAsIReadOnlyList[0] == FirstNodeState);
                IEnumerable<IFocusNodeState> FocusNodeStateListAsEnumerable = LayoutNodeStateList as IEnumerable<IFocusNodeState>;
                Assert.That(FocusNodeStateListAsEnumerable != null);
                Assert.That(FocusNodeStateListAsEnumerable.GetEnumerator() != null);

                // ILayoutOperationGroupList

                LayoutOperationGroupReadOnlyList LayoutOperationStack = Controller.OperationStack;

                Assert.That(LayoutOperationStack.Count > 0);
                LayoutOperationGroup FirstOperationGroup = (LayoutOperationGroup)LayoutOperationStack[0];
                LayoutOperationGroupList LayoutOperationGroupList = DebugObjects.GetReferenceByInterface(Type.FromTypeof<LayoutOperationGroupList>()) as LayoutOperationGroupList;
                if (LayoutOperationGroupList != null)
                {
                    WriteableOperationGroupList WriteableOperationGroupList = LayoutOperationGroupList;
                    Assert.That(WriteableOperationGroupList.Count > 0);
                    Assert.That(WriteableOperationGroupList[0] == FirstOperationGroup);
                    WriteableOperationGroupList.GetEnumerator();
                    IList<WriteableOperationGroup> WriteableOperationGroupAsIList = WriteableOperationGroupList;
                    Assert.That(WriteableOperationGroupAsIList.Count > 0);
                    Assert.That(WriteableOperationGroupAsIList[0] == FirstOperationGroup);
                    Assert.That(WriteableOperationGroupAsIList.IndexOf(FirstOperationGroup) == 0);
                    WriteableOperationGroupAsIList.Remove(FirstOperationGroup);
                    WriteableOperationGroupAsIList.Insert(0, FirstOperationGroup);
                    ICollection<WriteableOperationGroup> WriteableOperationGroupAsICollection = WriteableOperationGroupList;
                    Assert.That(WriteableOperationGroupAsICollection.Count > 0);
                    Assert.That(!WriteableOperationGroupAsICollection.IsReadOnly);
                    Assert.That(WriteableOperationGroupAsICollection.Contains(FirstOperationGroup));
                    WriteableOperationGroupAsICollection.Remove(FirstOperationGroup);
                    WriteableOperationGroupAsICollection.Add(FirstOperationGroup);
                    WriteableOperationGroupAsICollection.Remove(FirstOperationGroup);
                    WriteableOperationGroupAsIList.Insert(0, FirstOperationGroup);
                    WriteableOperationGroupAsICollection.CopyTo(new LayoutOperationGroup[WriteableOperationGroupAsICollection.Count], 0);
                    IEnumerable<WriteableOperationGroup> WriteableOperationGroupAsIEnumerable = WriteableOperationGroupList;
                    WriteableOperationGroupAsIEnumerable.GetEnumerator();
                    IReadOnlyList<WriteableOperationGroup> WriteableOperationGroupAsIReadOnlyList = WriteableOperationGroupList;
                    Assert.That(WriteableOperationGroupAsIReadOnlyList.Count > 0);
                    Assert.That(WriteableOperationGroupAsIReadOnlyList[0] == FirstOperationGroup);

                    FrameOperationGroupList FrameOperationGroupList = LayoutOperationGroupList;
                    Assert.That(FrameOperationGroupList.Count > 0);
                    Assert.That(FrameOperationGroupList[0] == FirstOperationGroup);
                    FrameOperationGroupList.GetEnumerator();
                    IList<FrameOperationGroup> FrameOperationGroupAsIList = FrameOperationGroupList;
                    Assert.That(FrameOperationGroupAsIList.Count > 0);
                    Assert.That(FrameOperationGroupAsIList[0] == FirstOperationGroup);
                    Assert.That(FrameOperationGroupAsIList.IndexOf(FirstOperationGroup) == 0);
                    FrameOperationGroupAsIList.Remove(FirstOperationGroup);
                    FrameOperationGroupAsIList.Insert(0, FirstOperationGroup);
                    ICollection<FrameOperationGroup> FrameOperationGroupAsICollection = FrameOperationGroupList;
                    Assert.That(FrameOperationGroupAsICollection.Count > 0);
                    Assert.That(!FrameOperationGroupAsICollection.IsReadOnly);
                    Assert.That(FrameOperationGroupAsICollection.Contains(FirstOperationGroup));
                    FrameOperationGroupAsICollection.Remove(FirstOperationGroup);
                    FrameOperationGroupAsICollection.Add(FirstOperationGroup);
                    FrameOperationGroupAsICollection.Remove(FirstOperationGroup);
                    FrameOperationGroupAsIList.Insert(0, FirstOperationGroup);
                    FrameOperationGroupAsICollection.CopyTo(new LayoutOperationGroup[FrameOperationGroupAsICollection.Count], 0);
                    IEnumerable<FrameOperationGroup> FrameOperationGroupAsIEnumerable = FrameOperationGroupList;
                    FrameOperationGroupAsIEnumerable.GetEnumerator();
                    IReadOnlyList<FrameOperationGroup> FrameOperationGroupAsIReadOnlyList = FrameOperationGroupList;
                    Assert.That(FrameOperationGroupAsIReadOnlyList.Count > 0);
                    Assert.That(FrameOperationGroupAsIReadOnlyList[0] == FirstOperationGroup);

                    FocusOperationGroupList FocusOperationGroupList = LayoutOperationGroupList;
                    Assert.That(FocusOperationGroupList.Count > 0);
                    Assert.That(FocusOperationGroupList[0] == FirstOperationGroup);
                    FocusOperationGroupList.GetEnumerator();
                    IList<FocusOperationGroup> FocusOperationGroupAsIList = FocusOperationGroupList;
                    Assert.That(FocusOperationGroupAsIList.Count > 0);
                    Assert.That(FocusOperationGroupAsIList[0] == FirstOperationGroup);
                    Assert.That(FocusOperationGroupAsIList.IndexOf(FirstOperationGroup) == 0);
                    FocusOperationGroupAsIList.Remove(FirstOperationGroup);
                    FocusOperationGroupAsIList.Insert(0, FirstOperationGroup);
                    ICollection<FocusOperationGroup> FocusOperationGroupAsICollection = FocusOperationGroupList;
                    Assert.That(FocusOperationGroupAsICollection.Count > 0);
                    Assert.That(!FocusOperationGroupAsICollection.IsReadOnly);
                    Assert.That(FocusOperationGroupAsICollection.Contains(FirstOperationGroup));
                    FocusOperationGroupAsICollection.Remove(FirstOperationGroup);
                    FocusOperationGroupAsICollection.Add(FirstOperationGroup);
                    FocusOperationGroupAsICollection.Remove(FirstOperationGroup);
                    FocusOperationGroupAsIList.Insert(0, FirstOperationGroup);
                    FocusOperationGroupAsICollection.CopyTo(new LayoutOperationGroup[FocusOperationGroupAsICollection.Count], 0);
                    IEnumerable<FocusOperationGroup> FocusOperationGroupAsIEnumerable = FocusOperationGroupList;
                    FocusOperationGroupAsIEnumerable.GetEnumerator();
                    IReadOnlyList<FocusOperationGroup> FocusOperationGroupAsIReadOnlyList = FocusOperationGroupList;
                    Assert.That(FocusOperationGroupAsIReadOnlyList.Count > 0);
                    Assert.That(FocusOperationGroupAsIReadOnlyList[0] == FirstOperationGroup);
                }

                // ILayoutOperationGroupReadOnlyList

                WriteableOperationGroupReadOnlyList WriteableOperationStack = LayoutOperationStack;
                Assert.That(WriteableOperationStack.Contains(FirstOperationGroup));
                Assert.That(WriteableOperationStack.IndexOf(FirstOperationGroup) == 0);
                IEnumerable<WriteableOperationGroup> WriteableOperationStackAsIEnumerable = WriteableOperationStack;
                WriteableOperationStackAsIEnumerable.GetEnumerator();

                FrameOperationGroupReadOnlyList FrameOperationStack = LayoutOperationStack;
                Assert.That(FrameOperationStack.Contains(FirstOperationGroup));
                Assert.That(FrameOperationStack.IndexOf(FirstOperationGroup) == 0);
                Assert.That(FrameOperationStack[0] == FirstOperationGroup);
                FrameOperationStack.GetEnumerator();
                IEnumerable<FrameOperationGroup> FrameOperationStackAsIEnumerable = FrameOperationStack;
                FrameOperationStackAsIEnumerable.GetEnumerator();
                IReadOnlyList<FrameOperationGroup> FrameOperationStackAsIReadOnlyList = FrameOperationStack;
                Assert.That(FrameOperationStackAsIReadOnlyList[0] == FirstOperationGroup);

                FocusOperationGroupReadOnlyList FocusOperationStack = LayoutOperationStack;
                Assert.That(FocusOperationStack.Contains(FirstOperationGroup));
                Assert.That(FocusOperationStack.IndexOf(FirstOperationGroup) == 0);
                Assert.That(FocusOperationStack[0] == FirstOperationGroup);
                FocusOperationStack.GetEnumerator();
                IEnumerable<FocusOperationGroup> FocusOperationStackAsIEnumerable = FocusOperationStack;
                FocusOperationStackAsIEnumerable.GetEnumerator();
                IReadOnlyList<FocusOperationGroup> FocusOperationStackAsIReadOnlyList = FocusOperationStack;
                Assert.That(FocusOperationStackAsIReadOnlyList[0] == FirstOperationGroup);

                // ILayoutOperationList

                LayoutOperationReadOnlyList LayoutOperationReadOnlyList = FirstOperationGroup.OperationList;
                Assert.That(LayoutOperationReadOnlyList.Count > 0);
                ILayoutOperation FirstOperation = (ILayoutOperation)LayoutOperationReadOnlyList[0];
                LayoutOperationList LayoutOperationList = DebugObjects.GetReferenceByInterface(Type.FromTypeof<LayoutOperationList>()) as LayoutOperationList;
                if (LayoutOperationList != null)
                {
                    WriteableOperationList WriteableOperationList = LayoutOperationList;
                    Assert.That(WriteableOperationList.Count > 0);
                    Assert.That(WriteableOperationList[0] == FirstOperation);
                    IList<IWriteableOperation> WriteableOperationAsIList = WriteableOperationList;
                    Assert.That(WriteableOperationAsIList.Count > 0);
                    Assert.That(WriteableOperationAsIList[0] == FirstOperation);
                    Assert.That(WriteableOperationAsIList.IndexOf(FirstOperation) == 0);
                    WriteableOperationAsIList.Remove(FirstOperation);
                    WriteableOperationAsIList.Insert(0, FirstOperation);
                    ICollection<IWriteableOperation> WriteableOperationAsICollection = WriteableOperationList;
                    Assert.That(WriteableOperationAsICollection.Count > 0);
                    Assert.That(!WriteableOperationAsICollection.IsReadOnly);
                    Assert.That(WriteableOperationAsICollection.Contains(FirstOperation));
                    WriteableOperationAsICollection.Remove(FirstOperation);
                    WriteableOperationAsICollection.Add(FirstOperation);
                    WriteableOperationAsICollection.Remove(FirstOperation);
                    WriteableOperationAsIList.Insert(0, FirstOperation);
                    WriteableOperationAsICollection.CopyTo(new ILayoutOperation[WriteableOperationAsICollection.Count], 0);
                    IEnumerable<IWriteableOperation> WriteableOperationAsIEnumerable = WriteableOperationList;
                    WriteableOperationAsIEnumerable.GetEnumerator();
                    IReadOnlyList<IWriteableOperation> WriteableOperationAsIReadOnlyList = WriteableOperationList;
                    Assert.That(WriteableOperationAsIReadOnlyList.Count > 0);
                    Assert.That(WriteableOperationAsIReadOnlyList[0] == FirstOperation);

                    FrameOperationList FrameOperationList = LayoutOperationList;
                    Assert.That(FrameOperationList.Count > 0);
                    Assert.That(FrameOperationList[0] == FirstOperation);
                    FrameOperationList.GetEnumerator();
                    IList<IFrameOperation> FrameOperationAsIList = (IList<IFrameOperation>)FrameOperationList;
                    Assert.That(FrameOperationAsIList.Count > 0);
                    Assert.That(FrameOperationAsIList[0] == FirstOperation);
                    Assert.That(FrameOperationAsIList.IndexOf(FirstOperation) == 0);
                    FrameOperationAsIList.Remove(FirstOperation);
                    FrameOperationAsIList.Insert(0, FirstOperation);
                    ICollection<IFrameOperation> FrameOperationAsICollection = (ICollection<IFrameOperation>)FrameOperationList;
                    Assert.That(FrameOperationAsICollection.Count > 0);
                    Assert.That(!FrameOperationAsICollection.IsReadOnly);
                    Assert.That(FrameOperationAsICollection.Contains(FirstOperation));
                    FrameOperationAsICollection.Remove(FirstOperation);
                    FrameOperationAsICollection.Add(FirstOperation);
                    FrameOperationAsICollection.Remove(FirstOperation);
                    FrameOperationAsIList.Insert(0, FirstOperation);
                    FrameOperationAsICollection.CopyTo(new ILayoutOperation[FrameOperationAsICollection.Count], 0);
                    IEnumerable<IFrameOperation> FrameOperationAsIEnumerable = FrameOperationList;
                    FrameOperationAsIEnumerable.GetEnumerator();
                    IReadOnlyList<IFrameOperation> FrameOperationAsIReadOnlyList = FrameOperationList;
                    Assert.That(FrameOperationAsIReadOnlyList.Count > 0);
                    Assert.That(FrameOperationAsIReadOnlyList[0] == FirstOperation);

                    FocusOperationList FocusOperationList = LayoutOperationList;
                    Assert.That(FocusOperationList.Count > 0);
                    Assert.That(FocusOperationList[0] == FirstOperation);
                    FocusOperationList.GetEnumerator();
                    IList<IFocusOperation> FocusOperationAsIList = (IList<IFocusOperation>)FocusOperationList;
                    Assert.That(FocusOperationAsIList.Count > 0);
                    Assert.That(FocusOperationAsIList[0] == FirstOperation);
                    Assert.That(FocusOperationAsIList.IndexOf(FirstOperation) == 0);
                    FocusOperationAsIList.Remove(FirstOperation);
                    FocusOperationAsIList.Insert(0, FirstOperation);
                    ICollection<IFocusOperation> FocusOperationAsICollection = (ICollection<IFocusOperation>)FocusOperationList;
                    Assert.That(FocusOperationAsICollection.Count > 0);
                    Assert.That(!FocusOperationAsICollection.IsReadOnly);
                    Assert.That(FocusOperationAsICollection.Contains(FirstOperation));
                    FocusOperationAsICollection.Remove(FirstOperation);
                    FocusOperationAsICollection.Add(FirstOperation);
                    FocusOperationAsICollection.Remove(FirstOperation);
                    FocusOperationAsIList.Insert(0, FirstOperation);
                    FocusOperationAsICollection.CopyTo(new ILayoutOperation[FocusOperationAsICollection.Count], 0);
                    IEnumerable<IFocusOperation> FocusOperationAsIEnumerable = FocusOperationList;
                    FocusOperationAsIEnumerable.GetEnumerator();
                    IReadOnlyList<IFocusOperation> FocusOperationAsIReadOnlyList = FocusOperationList;
                    Assert.That(FocusOperationAsIReadOnlyList.Count > 0);
                    Assert.That(FocusOperationAsIReadOnlyList[0] == FirstOperation);
                }

                // ILayoutOperationReadOnlyList

                WriteableOperationReadOnlyList WriteableOperationReadOnlyList = LayoutOperationReadOnlyList;
                Assert.That(WriteableOperationReadOnlyList.Contains(FirstOperation));
                Assert.That(WriteableOperationReadOnlyList.IndexOf(FirstOperation) == 0);
                IEnumerable<IWriteableOperation> WriteableOperationReadOnlyListAsIEnumerable = WriteableOperationReadOnlyList;
                WriteableOperationReadOnlyListAsIEnumerable.GetEnumerator();

                FrameOperationReadOnlyList FrameOperationReadOnlyList = LayoutOperationReadOnlyList;
                Assert.That(FrameOperationReadOnlyList.Contains(FirstOperation));
                Assert.That(FrameOperationReadOnlyList.IndexOf(FirstOperation) == 0);
                Assert.That(FrameOperationReadOnlyList[0] == FirstOperation);
                FrameOperationReadOnlyList.GetEnumerator();
                IEnumerable<IFrameOperation> FrameOperationReadOnlyListAsIEnumerable = FrameOperationReadOnlyList;
                FrameOperationReadOnlyListAsIEnumerable.GetEnumerator();
                IReadOnlyList<IFrameOperation> FrameOperationReadOnlyListAsIReadOnlyList = FrameOperationReadOnlyList;
                Assert.That(FrameOperationReadOnlyListAsIReadOnlyList[0] == FirstOperation);

                FocusOperationReadOnlyList FocusOperationReadOnlyList = LayoutOperationReadOnlyList;
                Assert.That(FocusOperationReadOnlyList.Contains(FirstOperation));
                Assert.That(FocusOperationReadOnlyList.IndexOf(FirstOperation) == 0);
                Assert.That(FocusOperationReadOnlyList[0] == FirstOperation);
                FocusOperationReadOnlyList.GetEnumerator();
                IEnumerable<IFocusOperation> FocusOperationReadOnlyListAsIEnumerable = FocusOperationReadOnlyList;
                FocusOperationReadOnlyListAsIEnumerable.GetEnumerator();
                IReadOnlyList<IFocusOperation> FocusOperationReadOnlyListAsIReadOnlyList = FocusOperationReadOnlyList;
                Assert.That(FocusOperationReadOnlyListAsIReadOnlyList[0] == FirstOperation);

                // LayoutPlaceholderNodeStateList

                FirstNodeState = LeafPathInner.FirstNodeState;
                Assert.That(FirstNodeState != null);
                LayoutPlaceholderNodeStateList LayoutPlaceholderNodeStateListModify = DebugObjects.GetReferenceByInterface(Type.FromTypeof<LayoutPlaceholderNodeStateList>()) as LayoutPlaceholderNodeStateList;
                if (LayoutPlaceholderNodeStateListModify != null)
                {
                    Assert.That(LayoutPlaceholderNodeStateListModify.Count > 0);
                    FirstNodeState = LayoutPlaceholderNodeStateListModify[0] as ILayoutPlaceholderNodeState;

                    Assert.That(LayoutPlaceholderNodeStateListModify.Contains((IReadOnlyPlaceholderNodeState)FirstNodeState));
                    Assert.That(LayoutPlaceholderNodeStateListModify.IndexOf((IReadOnlyPlaceholderNodeState)FirstNodeState) == 0);
                    LayoutPlaceholderNodeStateListModify.Remove((IReadOnlyPlaceholderNodeState)FirstNodeState);
                    LayoutPlaceholderNodeStateListModify.Insert(0, (IReadOnlyPlaceholderNodeState)FirstNodeState);
                    LayoutPlaceholderNodeStateListModify.CopyTo((IReadOnlyPlaceholderNodeState[])(new ILayoutPlaceholderNodeState[LayoutPlaceholderNodeStateListModify.Count]), 0);
                    ReadOnlyPlaceholderNodeStateList LayoutPlaceholderNodeStateListModifyAsReadOnly = LayoutPlaceholderNodeStateListModify as ReadOnlyPlaceholderNodeStateList;
                    Assert.That(LayoutPlaceholderNodeStateListModifyAsReadOnly != null);
                    Assert.That(LayoutPlaceholderNodeStateListModifyAsReadOnly[0] == LayoutPlaceholderNodeStateListModify[0]);
                    IList<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListModifyAsIList = LayoutPlaceholderNodeStateListModify as IList<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsIList != null);
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsIList[0] == LayoutPlaceholderNodeStateListModify[0]);
                    IReadOnlyList<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListModifyAsIReadOnlyList = LayoutPlaceholderNodeStateListModify as IReadOnlyList<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsIReadOnlyList != null);
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsIReadOnlyList[0] == LayoutPlaceholderNodeStateListModify[0]);
                    ICollection<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListModifyAsCollection = LayoutPlaceholderNodeStateListModify as ICollection<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsCollection != null);
                    Assert.That(!ReadOnlyPlaceholderNodeStateListModifyAsCollection.IsReadOnly);
                    ReadOnlyPlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    ReadOnlyPlaceholderNodeStateListModifyAsCollection.Add(FirstNodeState);
                    ReadOnlyPlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    ReadOnlyPlaceholderNodeStateListModifyAsIList.Insert(0, FirstNodeState);
                    IEnumerable<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListModifyAsEnumerable = LayoutPlaceholderNodeStateListModify as IEnumerable<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsEnumerable != null);
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsEnumerable.GetEnumerator() != null);

                    WriteablePlaceholderNodeStateList LayoutPlaceholderNodeStateListModifyAsWriteable = LayoutPlaceholderNodeStateListModify as WriteablePlaceholderNodeStateList;
                    Assert.That(LayoutPlaceholderNodeStateListModifyAsWriteable != null);
                    Assert.That(LayoutPlaceholderNodeStateListModifyAsWriteable[0] == LayoutPlaceholderNodeStateListModify[0]);
                    LayoutPlaceholderNodeStateListModifyAsWriteable.GetEnumerator();
                    IList<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListModifyAsIList = LayoutPlaceholderNodeStateListModify as IList<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsIList != null);
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsIList[0] == LayoutPlaceholderNodeStateListModify[0]);
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsIList.IndexOf(FirstNodeState) == 0);
                    WriteablePlaceholderNodeStateListModifyAsIList.Remove(FirstNodeState);
                    WriteablePlaceholderNodeStateListModifyAsIList.Insert(0, FirstNodeState);
                    IReadOnlyList<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListModifyAsIReadOnlyList = LayoutPlaceholderNodeStateListModify as IReadOnlyList<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsIReadOnlyList != null);
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsIReadOnlyList[0] == LayoutPlaceholderNodeStateListModify[0]);
                    ICollection<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListModifyAsCollection = LayoutPlaceholderNodeStateListModify as ICollection<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsCollection != null);
                    Assert.That(!WriteablePlaceholderNodeStateListModifyAsCollection.IsReadOnly);
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsCollection.Contains(FirstNodeState));
                    WriteablePlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    WriteablePlaceholderNodeStateListModifyAsCollection.Add(FirstNodeState);
                    WriteablePlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    LayoutPlaceholderNodeStateListModify.Insert(0, FirstNodeState);
                    WriteablePlaceholderNodeStateListModifyAsCollection.CopyTo(new ILayoutPlaceholderNodeState[WriteablePlaceholderNodeStateListModifyAsCollection.Count], 0);
                    IEnumerable<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListModifyAsEnumerable = LayoutPlaceholderNodeStateListModify as IEnumerable<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsEnumerable != null);
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsEnumerable.GetEnumerator() != null);

                    FramePlaceholderNodeStateList LayoutPlaceholderNodeStateListModifyAsFrame = LayoutPlaceholderNodeStateListModify as FramePlaceholderNodeStateList;
                    Assert.That(LayoutPlaceholderNodeStateListModifyAsFrame != null);
                    Assert.That(LayoutPlaceholderNodeStateListModifyAsFrame[0] == LayoutPlaceholderNodeStateListModify[0]);
                    LayoutPlaceholderNodeStateListModifyAsFrame.GetEnumerator();
                    IList<IFramePlaceholderNodeState> FramePlaceholderNodeStateListModifyAsIList = LayoutPlaceholderNodeStateListModify as IList<IFramePlaceholderNodeState>;
                    Assert.That(FramePlaceholderNodeStateListModifyAsIList != null);
                    Assert.That(FramePlaceholderNodeStateListModifyAsIList[0] == LayoutPlaceholderNodeStateListModify[0]);
                    Assert.That(FramePlaceholderNodeStateListModifyAsIList.IndexOf(FirstNodeState) == 0);
                    FramePlaceholderNodeStateListModifyAsIList.Remove(FirstNodeState);
                    FramePlaceholderNodeStateListModifyAsIList.Insert(0, FirstNodeState);
                    IReadOnlyList<IFramePlaceholderNodeState> FramePlaceholderNodeStateListModifyAsIReadOnlyList = LayoutPlaceholderNodeStateListModify as IReadOnlyList<IFramePlaceholderNodeState>;
                    Assert.That(FramePlaceholderNodeStateListModifyAsIReadOnlyList != null);
                    Assert.That(FramePlaceholderNodeStateListModifyAsIReadOnlyList[0] == LayoutPlaceholderNodeStateListModify[0]);
                    ICollection<IFramePlaceholderNodeState> FramePlaceholderNodeStateListModifyAsCollection = LayoutPlaceholderNodeStateListModify as ICollection<IFramePlaceholderNodeState>;
                    Assert.That(FramePlaceholderNodeStateListModifyAsCollection != null);
                    Assert.That(!FramePlaceholderNodeStateListModifyAsCollection.IsReadOnly);
                    Assert.That(FramePlaceholderNodeStateListModifyAsCollection.Contains(FirstNodeState));
                    FramePlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    FramePlaceholderNodeStateListModifyAsCollection.Add(FirstNodeState);
                    FramePlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    LayoutPlaceholderNodeStateListModify.Insert(0, FirstNodeState);
                    FramePlaceholderNodeStateListModifyAsCollection.CopyTo(new ILayoutPlaceholderNodeState[FramePlaceholderNodeStateListModifyAsCollection.Count], 0);
                    IEnumerable<IFramePlaceholderNodeState> FramePlaceholderNodeStateListModifyAsEnumerable = LayoutPlaceholderNodeStateListModify as IEnumerable<IFramePlaceholderNodeState>;
                    Assert.That(FramePlaceholderNodeStateListModifyAsEnumerable != null);
                    Assert.That(FramePlaceholderNodeStateListModifyAsEnumerable.GetEnumerator() != null);

                    FocusPlaceholderNodeStateList LayoutPlaceholderNodeStateListModifyAsFocus = LayoutPlaceholderNodeStateListModify as FocusPlaceholderNodeStateList;
                    Assert.That(LayoutPlaceholderNodeStateListModifyAsFocus != null);
                    Assert.That(LayoutPlaceholderNodeStateListModifyAsFocus[0] == LayoutPlaceholderNodeStateListModify[0]);
                    LayoutPlaceholderNodeStateListModifyAsFocus.GetEnumerator();
                    IList<IFocusPlaceholderNodeState> FocusPlaceholderNodeStateListModifyAsIList = LayoutPlaceholderNodeStateListModify as IList<IFocusPlaceholderNodeState>;
                    Assert.That(FocusPlaceholderNodeStateListModifyAsIList != null);
                    Assert.That(FocusPlaceholderNodeStateListModifyAsIList[0] == LayoutPlaceholderNodeStateListModify[0]);
                    Assert.That(FocusPlaceholderNodeStateListModifyAsIList.IndexOf(FirstNodeState) == 0);
                    FocusPlaceholderNodeStateListModifyAsIList.Remove(FirstNodeState);
                    FocusPlaceholderNodeStateListModifyAsIList.Insert(0, FirstNodeState);
                    IReadOnlyList<IFocusPlaceholderNodeState> FocusPlaceholderNodeStateListModifyAsIReadOnlyList = LayoutPlaceholderNodeStateListModify as IReadOnlyList<IFocusPlaceholderNodeState>;
                    Assert.That(FocusPlaceholderNodeStateListModifyAsIReadOnlyList != null);
                    Assert.That(FocusPlaceholderNodeStateListModifyAsIReadOnlyList[0] == LayoutPlaceholderNodeStateListModify[0]);
                    ICollection<IFocusPlaceholderNodeState> FocusPlaceholderNodeStateListModifyAsCollection = LayoutPlaceholderNodeStateListModify as ICollection<IFocusPlaceholderNodeState>;
                    Assert.That(FocusPlaceholderNodeStateListModifyAsCollection != null);
                    Assert.That(!FocusPlaceholderNodeStateListModifyAsCollection.IsReadOnly);
                    Assert.That(FocusPlaceholderNodeStateListModifyAsCollection.Contains(FirstNodeState));
                    FocusPlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    FocusPlaceholderNodeStateListModifyAsCollection.Add(FirstNodeState);
                    FocusPlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    LayoutPlaceholderNodeStateListModify.Insert(0, FirstNodeState);
                    FocusPlaceholderNodeStateListModifyAsCollection.CopyTo(new ILayoutPlaceholderNodeState[FocusPlaceholderNodeStateListModifyAsCollection.Count], 0);
                    IEnumerable<IFocusPlaceholderNodeState> FocusPlaceholderNodeStateListModifyAsEnumerable = LayoutPlaceholderNodeStateListModify as IEnumerable<IFocusPlaceholderNodeState>;
                    Assert.That(FocusPlaceholderNodeStateListModifyAsEnumerable != null);
                    Assert.That(FocusPlaceholderNodeStateListModifyAsEnumerable.GetEnumerator() != null);
                }

                // LayoutPlaceholderNodeStateReadOnlyList

                LayoutPlaceholderNodeStateReadOnlyList LayoutPlaceholderNodeStateList = LayoutPlaceholderNodeStateListModify != null ? LayoutPlaceholderNodeStateListModify.ToReadOnly() as LayoutPlaceholderNodeStateReadOnlyList : null;
                if (LayoutPlaceholderNodeStateList != null)
                {
                    Assert.That(LayoutPlaceholderNodeStateList.Count > 0);
                    FirstNodeState = LayoutPlaceholderNodeStateList[0] as ILayoutPlaceholderNodeState;

                    Assert.That(LayoutPlaceholderNodeStateList.Contains((IReadOnlyPlaceholderNodeState)FirstNodeState));
                    Assert.That(LayoutPlaceholderNodeStateList.IndexOf((IReadOnlyPlaceholderNodeState)FirstNodeState) == 0);
                    IReadOnlyList<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListAsIReadOnlyList = LayoutPlaceholderNodeStateList as IReadOnlyList<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListAsIReadOnlyList[0] == FirstNodeState);
                    IEnumerable<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListAsEnumerable = LayoutPlaceholderNodeStateList as IEnumerable<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListAsEnumerable != null);
                    Assert.That(ReadOnlyPlaceholderNodeStateListAsEnumerable.GetEnumerator() != null);

                    WriteablePlaceholderNodeStateReadOnlyList WriteablePlaceholderNodeStateList = LayoutPlaceholderNodeStateList;
                    Assert.That(WriteablePlaceholderNodeStateList.Contains(FirstNodeState));
                    Assert.That(WriteablePlaceholderNodeStateList.IndexOf(FirstNodeState) == 0);
                    Assert.That(WriteablePlaceholderNodeStateList[0] == LayoutPlaceholderNodeStateList[0]);
                    WriteablePlaceholderNodeStateList.GetEnumerator();
                    IReadOnlyList<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListAsIReadOnlyList = LayoutPlaceholderNodeStateList as IReadOnlyList<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListAsIReadOnlyList[0] == FirstNodeState);
                    IEnumerable<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListAsEnumerable = LayoutPlaceholderNodeStateList as IEnumerable<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListAsEnumerable != null);
                    Assert.That(WriteablePlaceholderNodeStateListAsEnumerable.GetEnumerator() != null);

                    FramePlaceholderNodeStateReadOnlyList FramePlaceholderNodeStateList = LayoutPlaceholderNodeStateList;
                    Assert.That(FramePlaceholderNodeStateList.Contains(FirstNodeState));
                    Assert.That(FramePlaceholderNodeStateList.IndexOf(FirstNodeState) == 0);
                    Assert.That(FramePlaceholderNodeStateList[0] == LayoutPlaceholderNodeStateList[0]);
                    FramePlaceholderNodeStateList.GetEnumerator();
                    IReadOnlyList<IFramePlaceholderNodeState> FramePlaceholderNodeStateListAsIReadOnlyList = LayoutPlaceholderNodeStateList as IReadOnlyList<IFramePlaceholderNodeState>;
                    Assert.That(FramePlaceholderNodeStateListAsIReadOnlyList[0] == FirstNodeState);
                    IEnumerable<IFramePlaceholderNodeState> FramePlaceholderNodeStateListAsEnumerable = LayoutPlaceholderNodeStateList as IEnumerable<IFramePlaceholderNodeState>;
                    Assert.That(FramePlaceholderNodeStateListAsEnumerable != null);
                    Assert.That(FramePlaceholderNodeStateListAsEnumerable.GetEnumerator() != null);

                    FocusPlaceholderNodeStateReadOnlyList FocusPlaceholderNodeStateList = LayoutPlaceholderNodeStateList;
                    Assert.That(FocusPlaceholderNodeStateList.Contains(FirstNodeState));
                    Assert.That(FocusPlaceholderNodeStateList.IndexOf(FirstNodeState) == 0);
                    Assert.That(FocusPlaceholderNodeStateList[0] == LayoutPlaceholderNodeStateList[0]);
                    FocusPlaceholderNodeStateList.GetEnumerator();
                    IReadOnlyList<IFocusPlaceholderNodeState> FocusPlaceholderNodeStateListAsIReadOnlyList = LayoutPlaceholderNodeStateList as IReadOnlyList<IFocusPlaceholderNodeState>;
                    Assert.That(FocusPlaceholderNodeStateListAsIReadOnlyList[0] == FirstNodeState);
                    IEnumerable<IFocusPlaceholderNodeState> FocusPlaceholderNodeStateListAsEnumerable = LayoutPlaceholderNodeStateList as IEnumerable<IFocusPlaceholderNodeState>;
                    Assert.That(FocusPlaceholderNodeStateListAsEnumerable != null);
                    Assert.That(FocusPlaceholderNodeStateListAsEnumerable.GetEnumerator() != null);
                }

                // ILayoutStateViewDictionary

                LayoutNodeStateViewDictionary LayoutStateViewTable = ControllerView.StateViewTable;
                WriteableNodeStateViewDictionary WriteableStateViewTable = ControllerView.StateViewTable;
                WriteableStateViewTable.GetEnumerator();
                FrameNodeStateViewDictionary FrameStateViewTable = ControllerView.StateViewTable;
                FrameStateViewTable.GetEnumerator();

                IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView> ReadOnlyStateViewTableAsDictionary = LayoutStateViewTable;
                Assert.That(ReadOnlyStateViewTableAsDictionary != null);
                Assert.That(ReadOnlyStateViewTableAsDictionary.TryGetValue(RootState, out IReadOnlyNodeStateView StateViewTableAsDictionaryValue) == LayoutStateViewTable.TryGetValue(RootState, out IReadOnlyNodeStateView StateViewTableValue));
                Assert.That(ReadOnlyStateViewTableAsDictionary.Keys != null);
                Assert.That(ReadOnlyStateViewTableAsDictionary.Values != null);
                ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> ReadOnlyStateViewTableAsCollection = LayoutStateViewTable;
                Assert.That(!ReadOnlyStateViewTableAsCollection.IsReadOnly);
                foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in ReadOnlyStateViewTableAsCollection)
                {
                    Assert.That(ReadOnlyStateViewTableAsCollection.Contains(Entry));
                    ReadOnlyStateViewTableAsCollection.Remove(Entry);
                    ReadOnlyStateViewTableAsCollection.Add(Entry);
                    ReadOnlyStateViewTableAsCollection.CopyTo(new KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>[LayoutStateViewTable.Count], 0);
                    break;
                }

                ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> WriteableStateViewTableAsCollection = LayoutStateViewTable;
                Assert.That(!WriteableStateViewTableAsCollection.IsReadOnly);
                IDictionary<IWriteableNodeState, IWriteableNodeStateView> WriteableStateViewTableAsDictionary = LayoutStateViewTable;
                Assert.That(WriteableStateViewTableAsDictionary != null);
                Assert.That(WriteableStateViewTableAsDictionary.TryGetValue(RootState, out IWriteableNodeStateView WriteableStateViewTableAsDictionaryValue) == LayoutStateViewTable.TryGetValue(RootState, out StateViewTableValue));
                Assert.That(WriteableStateViewTableAsDictionary.Keys != null);
                Assert.That(WriteableStateViewTableAsDictionary.Values != null);
                foreach (KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> Entry in WriteableStateViewTableAsCollection)
                {
                    Assert.That(WriteableStateViewTableAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(WriteableStateViewTableAsDictionary[Entry.Key] == Entry.Value);
                    WriteableStateViewTableAsDictionary.Remove(Entry.Key);
                    WriteableStateViewTableAsDictionary.Add(Entry.Key, Entry.Value);
                    Assert.That(WriteableStateViewTableAsCollection.Contains(Entry));
                    WriteableStateViewTableAsCollection.Remove(Entry);
                    WriteableStateViewTableAsCollection.Add(Entry);
                    WriteableStateViewTableAsCollection.CopyTo(new KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>[LayoutStateViewTable.Count], 0);

                    break;
                }
                IEnumerable<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> WriteableStateViewTableAsEnumerable = LayoutStateViewTable;
                WriteableStateViewTableAsEnumerable.GetEnumerator();

                ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>> FrameStateViewTableAsCollection = LayoutStateViewTable;
                Assert.That(!FrameStateViewTableAsCollection.IsReadOnly);
                IDictionary<IFrameNodeState, IFrameNodeStateView> FrameStateViewTableAsDictionary = LayoutStateViewTable;
                Assert.That(FrameStateViewTableAsDictionary != null);
                Assert.That(FrameStateViewTableAsDictionary.TryGetValue(RootState, out IFrameNodeStateView FrameStateViewTableAsDictionaryValue) == LayoutStateViewTable.TryGetValue(RootState, out StateViewTableValue));
                Assert.That(FrameStateViewTableAsDictionary.Keys != null);
                Assert.That(FrameStateViewTableAsDictionary.Values != null);
                foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in ReadOnlyStateViewTableAsCollection)
                {
                    Assert.That(FrameStateViewTableAsDictionary.ContainsKey((IFrameNodeState)Entry.Key));
                    FrameStateViewTableAsDictionary.Remove((IFrameNodeState)Entry.Key);
                    FrameStateViewTableAsDictionary.Add((IFrameNodeState)Entry.Key, (IFrameNodeStateView)Entry.Value);

                    break;
                }
                foreach (KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry in FrameStateViewTableAsCollection)
                {
                    Assert.That(FrameStateViewTableAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(FrameStateViewTableAsDictionary[Entry.Key] == Entry.Value);
                    FrameStateViewTableAsDictionary.Remove(Entry.Key);
                    FrameStateViewTableAsDictionary.Add(Entry.Key, Entry.Value);
                    Assert.That(FrameStateViewTableAsCollection.Contains(Entry));
                    FrameStateViewTableAsCollection.Remove(Entry);
                    FrameStateViewTableAsCollection.Add(Entry);
                    FrameStateViewTableAsCollection.CopyTo(new KeyValuePair<IFrameNodeState, IFrameNodeStateView>[LayoutStateViewTable.Count], 0);

                    break;
                }
                IEnumerable<KeyValuePair<IFrameNodeState, IFrameNodeStateView>> FrameStateViewTableAsEnumerable = LayoutStateViewTable;
                FrameStateViewTableAsEnumerable.GetEnumerator();

                ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>> FocusStateViewTableAsCollection = LayoutStateViewTable;
                Assert.That(!FocusStateViewTableAsCollection.IsReadOnly);
                IDictionary<IFocusNodeState, IFocusNodeStateView> FocusStateViewTableAsDictionary = LayoutStateViewTable;
                Assert.That(FocusStateViewTableAsDictionary != null);
                Assert.That(FocusStateViewTableAsDictionary.TryGetValue(RootState, out IFocusNodeStateView FocusStateViewTableAsDictionaryValue) == LayoutStateViewTable.TryGetValue(RootState, out StateViewTableValue));
                Assert.That(FocusStateViewTableAsDictionary.Keys != null);
                Assert.That(FocusStateViewTableAsDictionary.Values != null);
                foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in ReadOnlyStateViewTableAsCollection)
                {
                    Assert.That(FocusStateViewTableAsDictionary.ContainsKey((IFocusNodeState)Entry.Key));
                    FocusStateViewTableAsDictionary.Remove((IFocusNodeState)Entry.Key);
                    FocusStateViewTableAsDictionary.Add((IFocusNodeState)Entry.Key, (IFocusNodeStateView)Entry.Value);

                    break;
                }
                foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in FocusStateViewTableAsCollection)
                {
                    Assert.That(FocusStateViewTableAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(FocusStateViewTableAsDictionary[Entry.Key] == Entry.Value);
                    FocusStateViewTableAsDictionary.Remove(Entry.Key);
                    FocusStateViewTableAsDictionary.Add(Entry.Key, Entry.Value);
                    Assert.That(FocusStateViewTableAsCollection.Contains(Entry));
                    FocusStateViewTableAsCollection.Remove(Entry);
                    FocusStateViewTableAsCollection.Add(Entry);
                    FocusStateViewTableAsCollection.CopyTo(new KeyValuePair<IFocusNodeState, IFocusNodeStateView>[LayoutStateViewTable.Count], 0);

                    break;
                }
                IEnumerable<KeyValuePair<IFocusNodeState, IFocusNodeStateView>> FocusStateViewTableAsEnumerable = LayoutStateViewTable;
                FocusStateViewTableAsEnumerable.GetEnumerator();
            }

            ILayoutTemplateSet LayoutTemplateSet = TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet;
            using (LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, TestDebug.CoverageLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default))
            {
                // ILayoutAssignableCellViewDictionary

                LayoutAssignableCellViewDictionary<string> ActualCellViewTable = DebugObjects.GetReferenceByInterface(Type.FromTypeof<LayoutAssignableCellViewDictionary<string>>()) as LayoutAssignableCellViewDictionary<string>;
                if (ActualCellViewTable != null)
                {
                    FrameAssignableCellViewDictionary<string> FrameActualCellViewTable = ActualCellViewTable;
                    IDictionary<string, IFrameAssignableCellView> FrameActualCellViewTableAsDictionary = FrameActualCellViewTable;
                    Assert.That(FrameActualCellViewTableAsDictionary.Keys != null);
                    Assert.That(FrameActualCellViewTableAsDictionary.Values != null);
                    ICollection<KeyValuePair<string, IFrameAssignableCellView>> FrameActualCellViewTableAsCollection = FrameActualCellViewTable;
                    FrameActualCellViewTableAsCollection.CopyTo(new KeyValuePair<string, IFrameAssignableCellView>[FrameActualCellViewTableAsCollection.Count], 0);
                    Assert.That(!FrameActualCellViewTableAsCollection.IsReadOnly);
                    foreach (KeyValuePair<string, IFrameAssignableCellView> Entry in FrameActualCellViewTable)
                    {
                        Assert.That(FrameActualCellViewTable[Entry.Key] == Entry.Value);
                        Assert.That(FrameActualCellViewTableAsDictionary[Entry.Key] == Entry.Value);
                        Assert.That(FrameActualCellViewTable.TryGetValue(Entry.Key, out IFrameAssignableCellView FrameCellView) == ActualCellViewTable.TryGetValue(Entry.Key, out IFrameAssignableCellView LayoutCellView));
                        Assert.That(FrameActualCellViewTableAsCollection.Contains(Entry));
                        FrameActualCellViewTableAsDictionary.Remove(Entry.Key);
                        FrameActualCellViewTableAsDictionary.Add(Entry.Key, Entry.Value);
                        FrameActualCellViewTableAsCollection.Remove(Entry);
                        FrameActualCellViewTableAsCollection.Add(Entry);
                        break;
                    }
                    IEnumerable<KeyValuePair<string, IFrameAssignableCellView>> FrameActualCellViewTableAsEnumerable = FrameActualCellViewTable;
                    FrameActualCellViewTableAsEnumerable.GetEnumerator();

                    FocusAssignableCellViewDictionary<string> FocusActualCellViewTable = ActualCellViewTable;
                    IDictionary<string, IFocusAssignableCellView> FocusActualCellViewTableAsDictionary = FocusActualCellViewTable;
                    Assert.That(FocusActualCellViewTableAsDictionary.Keys != null);
                    Assert.That(FocusActualCellViewTableAsDictionary.Values != null);
                    ICollection<KeyValuePair<string, IFocusAssignableCellView>> FocusActualCellViewTableAsCollection = FocusActualCellViewTable;
                    FocusActualCellViewTableAsCollection.CopyTo(new KeyValuePair<string, IFocusAssignableCellView>[FocusActualCellViewTableAsCollection.Count], 0);
                    Assert.That(!FocusActualCellViewTableAsCollection.IsReadOnly);
                    foreach (KeyValuePair<string, IFocusAssignableCellView> Entry in (ICollection<KeyValuePair<string, IFocusAssignableCellView>>)FocusActualCellViewTable)
                    {
                        Assert.That(FocusActualCellViewTable[Entry.Key] == Entry.Value);
                        Assert.That(FocusActualCellViewTableAsDictionary[Entry.Key] == Entry.Value);
                        Assert.That(FocusActualCellViewTable.TryGetValue(Entry.Key, out IFrameAssignableCellView FocusCellView) == ActualCellViewTable.TryGetValue(Entry.Key, out IFrameAssignableCellView LayoutCellView));
                        Assert.That(FocusActualCellViewTableAsCollection.Contains(Entry));
                        FocusActualCellViewTableAsDictionary.Remove(Entry.Key);
                        FocusActualCellViewTableAsDictionary.Add(Entry.Key, Entry.Value);
                        FocusActualCellViewTableAsCollection.Remove(Entry);
                        FocusActualCellViewTableAsCollection.Add(Entry);
                        break;
                    }
                    IEnumerable<KeyValuePair<string, IFocusAssignableCellView>> FocusActualCellViewTableAsEnumerable = FocusActualCellViewTable;
                    FocusActualCellViewTableAsEnumerable.GetEnumerator();

                    // ILayoutAssignableCellViewReadOnlyDictionary

                    LayoutAssignableCellViewReadOnlyDictionary<string> ActualCellViewTableReadOnly = ActualCellViewTable.ToReadOnly() as LayoutAssignableCellViewReadOnlyDictionary<string>;

                    FrameAssignableCellViewReadOnlyDictionary<string> FrameActualCellViewTableReadOnly = ActualCellViewTableReadOnly;
                    IReadOnlyDictionary<string, IFrameAssignableCellView> FrameActualCellViewTableReadOnlyAsDictionary = ActualCellViewTableReadOnly;
                    Assert.That(FrameActualCellViewTableReadOnlyAsDictionary.Keys != null);
                    Assert.That(FrameActualCellViewTableReadOnlyAsDictionary.Values != null);
                    foreach (KeyValuePair<string, IFrameAssignableCellView> Entry in FrameActualCellViewTableReadOnlyAsDictionary)
                    {
                        Assert.That(FrameActualCellViewTableReadOnly[Entry.Key] == ActualCellViewTableReadOnly[Entry.Key]);
                        Assert.That(FrameActualCellViewTableReadOnlyAsDictionary[Entry.Key] == ActualCellViewTableReadOnly[Entry.Key]);
                        Assert.That(FrameActualCellViewTableReadOnlyAsDictionary.TryGetValue(Entry.Key, out IFrameAssignableCellView FrameCellView) == ActualCellViewTable.TryGetValue(Entry.Key, out IFrameAssignableCellView LayoutCellView));
                        FrameActualCellViewTableReadOnly.GetEnumerator();
                        break;
                    }

                    FocusAssignableCellViewReadOnlyDictionary<string> FocusActualCellViewTableReadOnly = ActualCellViewTableReadOnly;
                    IReadOnlyDictionary<string, IFocusAssignableCellView> FocusActualCellViewTableReadOnlyAsDictionary = ActualCellViewTableReadOnly;
                    Assert.That(FocusActualCellViewTableReadOnlyAsDictionary.Keys != null);
                    Assert.That(FocusActualCellViewTableReadOnlyAsDictionary.Values != null);
                    foreach (KeyValuePair<string, IFocusAssignableCellView> Entry in FocusActualCellViewTableReadOnlyAsDictionary)
                    {
                        Assert.That(FocusActualCellViewTableReadOnly[Entry.Key] == ActualCellViewTableReadOnly[Entry.Key]);
                        Assert.That(FocusActualCellViewTableReadOnlyAsDictionary[Entry.Key] == ActualCellViewTableReadOnly[Entry.Key]);
                        Assert.That(FocusActualCellViewTableReadOnlyAsDictionary.TryGetValue(Entry.Key, out IFocusAssignableCellView FocusCellView) == ActualCellViewTable.TryGetValue(Entry.Key, out IFrameAssignableCellView LayoutCellView));
                        FocusActualCellViewTableReadOnly.GetEnumerator();
                        break;
                    }

                    // LayoutCellViewList

                    //System.Diagnostics.Debug.Assert(false);
                    Assert.That(ActualCellViewTable.ContainsKey("LeafPath"));
                    Assert.That(ActualCellViewTable.ContainsKey("LeafBlocks"));
                    ILayoutCellViewCollection CellViewCollection = ActualCellViewTable["LeafPath"] as ILayoutCellViewCollection;
                    Assert.That(CellViewCollection != null);
                    LayoutCellViewList CellViewList = CellViewCollection.CellViewList;
                    Assert.That(CellViewList.Count > 0);
                    CellViewCollection = ActualCellViewTable["LeafBlocks"] as ILayoutCellViewCollection;
                    Assert.That(CellViewCollection != null);
                    CellViewList = CellViewCollection.CellViewList;
                    Assert.That(CellViewList.Count > 0);
                    ILayoutCellView FirstCellView = (ILayoutCellView)CellViewList[0];

                    FrameCellViewList FrameCellViewList = CellViewList;
                    Assert.That(FrameCellViewList[0] == FirstCellView);
                    IList<IFrameCellView> FrameCellViewListAsList = FrameCellViewList;
                    Assert.That(FrameCellViewListAsList.Contains(FirstCellView));
                    Assert.That(FrameCellViewListAsList[0] == FirstCellView);
                    Assert.That(FrameCellViewListAsList.IndexOf(FirstCellView) == 0);
                    FrameCellViewListAsList.Remove(FirstCellView);
                    FrameCellViewListAsList.Insert(0, FirstCellView);
                    ICollection<IFrameCellView> FrameCellViewListAsCollection = FrameCellViewList;
                    FrameCellViewListAsCollection.CopyTo(new ILayoutCellView[FrameCellViewListAsCollection.Count], 0);
                    Assert.That(!FrameCellViewListAsCollection.IsReadOnly);
                    FrameCellViewListAsCollection.Remove(FirstCellView);
                    FrameCellViewListAsCollection.Add(FirstCellView);
                    FrameCellViewListAsCollection.Remove(FirstCellView);
                    CellViewList.Insert(0, FirstCellView);
                    IEnumerable<IFrameCellView> FrameCellViewListAsEnumerable = FrameCellViewList;
                    FrameCellViewListAsEnumerable.GetEnumerator();
                    IReadOnlyList<IFrameCellView> FrameCellViewListAsReadOnlyList = FrameCellViewList;
                    Assert.That(FrameCellViewListAsReadOnlyList[0] == FirstCellView);

                    FocusCellViewList FocusCellViewList = CellViewList;
                    Assert.That(FocusCellViewList[0] == FirstCellView);
                    IList<IFocusCellView> FocusCellViewListAsList = FocusCellViewList;
                    Assert.That(FocusCellViewListAsList.Contains(FirstCellView));
                    Assert.That(FocusCellViewListAsList[0] == FirstCellView);
                    Assert.That(FocusCellViewListAsList.IndexOf(FirstCellView) == 0);
                    FocusCellViewListAsList.Remove(FirstCellView);
                    FocusCellViewListAsList.Insert(0, FirstCellView);
                    ICollection<IFocusCellView> FocusCellViewListAsCollection = FocusCellViewList;
                    FocusCellViewListAsCollection.CopyTo(new ILayoutCellView[FocusCellViewListAsCollection.Count], 0);
                    Assert.That(!FocusCellViewListAsCollection.IsReadOnly);
                    FocusCellViewListAsCollection.Remove(FirstCellView);
                    FocusCellViewListAsCollection.Add(FirstCellView);
                    FocusCellViewListAsCollection.Remove(FirstCellView);
                    CellViewList.Insert(0, FirstCellView);
                    IEnumerable<IFocusCellView> FocusCellViewListAsEnumerable = FocusCellViewList;
                    FocusCellViewListAsEnumerable.GetEnumerator();
                    IReadOnlyList<IFocusCellView> FocusCellViewListAsReadOnlyList = FocusCellViewList;
                    Assert.That(FocusCellViewListAsReadOnlyList[0] == FirstCellView);

                    // ILayoutFrameList 

                    ILayoutHorizontalPanelFrame HorizontalPanelFrame = GetFirstHorizontalFrame(CellViewCollection.StateView.Template.Root);
                    Assert.That(HorizontalPanelFrame != null);

                    LayoutFrameList FrameList = HorizontalPanelFrame.Items;
                    Assert.That(FrameList.Count > 0);
                    ILayoutFrame FirstFrame = (ILayoutFrame)FrameList[0];

                    FrameFrameList FrameFrameList = FrameList;
                    Assert.That(FrameFrameList[0] == FirstFrame);
                    IList<IFrameFrame> FrameFrameListAsList = FrameFrameList;
                    Assert.That(FrameFrameListAsList[0] == FirstFrame);
                    Assert.That(FrameFrameListAsList.IndexOf(FirstFrame) == 0);
                    ICollection<IFrameFrame> FrameFrameListAsCollection = FrameFrameList;
                    Assert.That(!FrameFrameListAsCollection.IsReadOnly);
                    Assert.That(FrameFrameListAsCollection.Contains(FirstFrame));
                    FrameFrameListAsCollection.Remove(FirstFrame);
                    FrameFrameListAsCollection.Add(FirstFrame);
                    FrameFrameListAsCollection.Remove(FirstFrame);
                    FrameFrameListAsList.Insert(0, FirstFrame);
                    FrameFrameListAsCollection.CopyTo(new ILayoutFrame[FrameFrameListAsCollection.Count], 0);
                    IEnumerable<IFrameFrame> FrameFrameListAsEnumerable = FrameFrameList;
                    FrameFrameListAsEnumerable.GetEnumerator();
                    IReadOnlyList<IFrameFrame> FrameFrameListAsReadOnlyList = FrameFrameList;
                    Assert.That(FrameFrameListAsReadOnlyList[0] == FirstFrame);

                    FocusFrameList FocusFrameList = FrameList;
                    Assert.That(FocusFrameList[0] == FirstFrame);
                    IList<IFocusFrame> FocusFrameListAsList = FocusFrameList;
                    Assert.That(FocusFrameListAsList[0] == FirstFrame);
                    Assert.That(FocusFrameListAsList.IndexOf(FirstFrame) == 0);
                    ICollection<IFocusFrame> FocusFrameListAsCollection = FocusFrameList;
                    Assert.That(!FocusFrameListAsCollection.IsReadOnly);
                    Assert.That(FocusFrameListAsCollection.Contains(FirstFrame));
                    FocusFrameListAsCollection.Remove(FirstFrame);
                    FocusFrameListAsCollection.Add(FirstFrame);
                    FocusFrameListAsCollection.Remove(FirstFrame);
                    FocusFrameListAsList.Insert(0, FirstFrame);
                    FocusFrameListAsCollection.CopyTo(new ILayoutFrame[FocusFrameListAsCollection.Count], 0);
                    IEnumerable<IFocusFrame> FocusFrameListAsEnumerable = FocusFrameList;
                    FocusFrameListAsEnumerable.GetEnumerator();
                    IReadOnlyList<IFocusFrame> FocusFrameListAsReadOnlyList = FocusFrameList;
                    Assert.That(FocusFrameListAsReadOnlyList[0] == FirstFrame);

                    //System.Diagnostics.Debug.Assert(false);
                    List<ILayoutFrame> FullFrameList = new List<ILayoutFrame>();
                    EnumerateFrames(FullFrameList, CellViewCollection.StateView.Template.Root);

                    foreach (ILayoutFrame Frame in FullFrameList)
                    {
                        if (Frame is IFocusTextValueFrame AsTextFrame)
                        {
                            bool AutoFormat = AsTextFrame.AutoFormat;
                        }
                    }

                    // ILayoutKeywordFrameList

                    ILayoutDiscreteFrame FirstDiscreteFrame = null;
                    foreach (ILayoutFrame Item in FrameList)
                        if (Item is ILayoutDiscreteFrame)
                        {
                            FirstDiscreteFrame = Item as ILayoutDiscreteFrame;
                            break;
                        }
                    Assert.That(FirstDiscreteFrame != null);
                    LayoutKeywordFrameList KeywordFrameList = FirstDiscreteFrame.Items;
                    Assert.That(KeywordFrameList.Count > 0);
                    ILayoutKeywordFrame FirstKeywordFrame = (ILayoutKeywordFrame)KeywordFrameList[0];

                    FrameKeywordFrameList FrameKeywordFrameList = KeywordFrameList;
                    FrameKeywordFrameList.GetEnumerator();
                    Assert.That(FrameKeywordFrameList[0] == FirstKeywordFrame);
                    IList<IFrameKeywordFrame> FrameKeywordFrameListAsList = FrameKeywordFrameList;
                    Assert.That(FrameKeywordFrameListAsList[0] == FirstKeywordFrame);
                    Assert.That(FrameKeywordFrameListAsList.IndexOf(FirstKeywordFrame) == 0);
                    ICollection<IFrameKeywordFrame> FrameKeywordFrameListAsCollection = FrameKeywordFrameList;
                    Assert.That(!FrameKeywordFrameListAsCollection.IsReadOnly);
                    Assert.That(FrameKeywordFrameListAsCollection.Contains(FirstKeywordFrame));
                    FrameKeywordFrameListAsCollection.Remove(FirstKeywordFrame);
                    FrameKeywordFrameListAsCollection.Add(FirstKeywordFrame);
                    FrameKeywordFrameListAsCollection.Remove(FirstKeywordFrame);
                    FrameKeywordFrameListAsList.Insert(0, FirstKeywordFrame);
                    FrameKeywordFrameListAsCollection.CopyTo(new ILayoutKeywordFrame[FrameKeywordFrameListAsCollection.Count], 0);
                    IEnumerable<IFrameKeywordFrame> FrameKeywordFrameListAsEnumerable = FrameKeywordFrameList;
                    FrameKeywordFrameListAsEnumerable.GetEnumerator();
                    IReadOnlyList<IFrameKeywordFrame> FrameKeywordFrameListAsReadOnlyList = FrameKeywordFrameList;
                    Assert.That(FrameKeywordFrameListAsReadOnlyList[0] == FirstKeywordFrame);

                    FocusKeywordFrameList FocusKeywordFrameList = KeywordFrameList;
                    FocusKeywordFrameList.GetEnumerator();
                    Assert.That(FocusKeywordFrameList[0] == FirstKeywordFrame);
                    IList<IFocusKeywordFrame> FocusKeywordFrameListAsList = FocusKeywordFrameList;
                    Assert.That(FocusKeywordFrameListAsList[0] == FirstKeywordFrame);
                    Assert.That(FocusKeywordFrameListAsList.IndexOf(FirstKeywordFrame) == 0);
                    ICollection<IFocusKeywordFrame> FocusKeywordFrameListAsCollection = FocusKeywordFrameList;
                    Assert.That(!FocusKeywordFrameListAsCollection.IsReadOnly);
                    Assert.That(FocusKeywordFrameListAsCollection.Contains(FirstKeywordFrame));
                    FocusKeywordFrameListAsCollection.Remove(FirstKeywordFrame);
                    FocusKeywordFrameListAsCollection.Add(FirstKeywordFrame);
                    FocusKeywordFrameListAsCollection.Remove(FirstKeywordFrame);
                    FocusKeywordFrameListAsList.Insert(0, FirstKeywordFrame);
                    FocusKeywordFrameListAsCollection.CopyTo(new ILayoutKeywordFrame[FocusKeywordFrameListAsCollection.Count], 0);
                    IEnumerable<IFocusKeywordFrame> FocusKeywordFrameListAsEnumerable = FocusKeywordFrameList;
                    FocusKeywordFrameListAsEnumerable.GetEnumerator();
                    IReadOnlyList<IFocusKeywordFrame> FocusKeywordFrameListAsReadOnlyList = FocusKeywordFrameList;
                    Assert.That(FocusKeywordFrameListAsReadOnlyList[0] == FirstKeywordFrame);
                }

                // ILayoutVisibleCellViewList
                ControllerView.MeasureAndArrange();

                LayoutVisibleCellViewList VisibleCellViewList = new LayoutVisibleCellViewList();
                ControllerView.EnumerateVisibleCellViews((IFrameVisibleCellView item) => ListCellViews(item, VisibleCellViewList), out IFrameVisibleCellView FoundCellView, false);
                Assert.That(VisibleCellViewList.Count > 0);
                ILayoutVisibleCellView FirstVisibleCellView = (ILayoutVisibleCellView)VisibleCellViewList[0];

                ControllerView.RootStateView.UpdateActualCellsSize();

                foreach (ILayoutVisibleCellView CellView in VisibleCellViewList)
                {
                    CellView.Draw();
                    CellView.Print(Point.Origin);
                }

                FrameVisibleCellViewList FrameVisibleCellViewList = VisibleCellViewList;
                FrameVisibleCellViewList.GetEnumerator();
                Assert.That(FrameVisibleCellViewList[0] == FirstVisibleCellView);
                IList<IFrameVisibleCellView> FrameVisibleCellViewListAsList = FrameVisibleCellViewList;
                Assert.That(FrameVisibleCellViewListAsList[0] == FirstVisibleCellView);
                Assert.That(FrameVisibleCellViewListAsList.IndexOf(FirstVisibleCellView) == 0);
                ICollection<IFrameVisibleCellView> FrameVisibleCellViewListAsCollection = FrameVisibleCellViewList;
                Assert.That(!FrameVisibleCellViewListAsCollection.IsReadOnly);
                FrameVisibleCellViewListAsCollection.Contains(FirstVisibleCellView);
                FrameVisibleCellViewListAsCollection.Remove(FirstVisibleCellView);
                FrameVisibleCellViewListAsCollection.Add(FirstVisibleCellView);
                FrameVisibleCellViewListAsCollection.Remove(FirstVisibleCellView);
                FrameVisibleCellViewListAsList.Insert(0, FirstVisibleCellView);
                FrameVisibleCellViewListAsCollection.CopyTo(new ILayoutVisibleCellView[FrameVisibleCellViewListAsCollection.Count], 0);
                IEnumerable<IFrameVisibleCellView> FrameVisibleCellViewListAsEnumerable = FrameVisibleCellViewList;
                FrameVisibleCellViewListAsEnumerable.GetEnumerator();
                IReadOnlyList<IFrameVisibleCellView> FrameVisibleCellViewListAsReadOnlyList = FrameVisibleCellViewList;
                Assert.That(FrameVisibleCellViewListAsReadOnlyList[0] == FirstVisibleCellView);

                FocusVisibleCellViewList FocusVisibleCellViewList = VisibleCellViewList;
                FocusVisibleCellViewList.GetEnumerator();
                Assert.That(FocusVisibleCellViewList[0] == FirstVisibleCellView);
                IList<IFocusVisibleCellView> FocusVisibleCellViewListAsList = FocusVisibleCellViewList;
                Assert.That(FocusVisibleCellViewListAsList[0] == FirstVisibleCellView);
                Assert.That(FocusVisibleCellViewListAsList.IndexOf(FirstVisibleCellView) == 0);
                ICollection<IFocusVisibleCellView> FocusVisibleCellViewListAsCollection = FocusVisibleCellViewList;
                Assert.That(!FocusVisibleCellViewListAsCollection.IsReadOnly);
                FocusVisibleCellViewListAsCollection.Contains(FirstVisibleCellView);
                FocusVisibleCellViewListAsCollection.Remove(FirstVisibleCellView);
                FocusVisibleCellViewListAsCollection.Add(FirstVisibleCellView);
                FocusVisibleCellViewListAsCollection.Remove(FirstVisibleCellView);
                FocusVisibleCellViewListAsList.Insert(0, FirstVisibleCellView);
                FocusVisibleCellViewListAsCollection.CopyTo(new ILayoutVisibleCellView[FocusVisibleCellViewListAsCollection.Count], 0);
                IEnumerable<IFocusVisibleCellView> FocusVisibleCellViewListAsEnumerable = FocusVisibleCellViewList;
                FocusVisibleCellViewListAsEnumerable.GetEnumerator();
                IReadOnlyList<IFocusVisibleCellView> FocusVisibleCellViewListAsReadOnlyList = FocusVisibleCellViewList;
                Assert.That(FocusVisibleCellViewListAsReadOnlyList[0] == FirstVisibleCellView);

                // ILayoutFocusList

                LayoutFocusList FocusList = DebugObjects.GetReferenceByInterface(Type.FromTypeof<LayoutFocusList>()) as LayoutFocusList;
                if (FocusList != null)
                {
                    Assert.That(FocusList.Count > 0);
                    ILayoutFocus FirstFocus = (ILayoutFocus)FocusList[0];

                    FocusFocusList FocusFocusList = FocusList;
                    Assert.That(FocusFocusList.Contains(FirstFocus));
                    Assert.That(FocusFocusList[0] == FirstFocus);
                    Assert.That(FocusFocusList.IndexOf(FirstFocus) == 0);
                    IList<IFocusFocus> FocusFocusListAsList = FocusFocusList;
                    Assert.That(FocusFocusListAsList.Contains(FirstFocus));
                    Assert.That(FocusFocusListAsList[0] == FirstFocus);
                    Assert.That(FocusFocusListAsList.IndexOf(FirstFocus) == 0);
                    ICollection<IFocusFocus> FocusFocusListAsCollection = FocusFocusList;
                    Assert.That(!FocusFocusListAsCollection.IsReadOnly);
                    Assert.That(FocusFocusListAsCollection.Contains(FirstFocus));
                    FocusFocusListAsCollection.Remove(FirstFocus);
                    FocusFocusListAsCollection.Add(FirstFocus);
                    FocusFocusListAsList.Remove(FirstFocus);
                    FocusFocusListAsList.Insert(0, FirstFocus);
                    FocusFocusListAsCollection.CopyTo(new ILayoutFocus[FocusFocusListAsCollection.Count], 0);
                    IEnumerable<IFocusFocus> FocusFocusListAsEnumerable = FocusFocusList;
                    FocusFocusListAsEnumerable.GetEnumerator();
                    IReadOnlyList<IFocusFocus> FocusFocusListAsReadOnlyList = FocusFocusList;
                    Assert.That(FocusFocusListAsReadOnlyList[0] == FirstFocus);
                }
            }

            // ILayoutTemplateDictionary

            LayoutTemplateDictionary NodeTemplateDictionary = TestDebug.CoverageLayoutTemplateSet.NodeTemplateDictionary;
            Assert.That(NodeTemplateDictionary.ContainsKey(Type.FromTypeof<Leaf>()));
            ILayoutTemplate LeafTemplate = (ILayoutTemplate)NodeTemplateDictionary[Type.FromTypeof<Leaf>()];

            FrameTemplateDictionary FrameNodeTemplateDictionary = NodeTemplateDictionary;
            IDictionary<Type, IFrameTemplate> FrameNodeTemplateDictionaryAsDictionary = FrameNodeTemplateDictionary;
            Assert.That(FrameNodeTemplateDictionaryAsDictionary.Keys != null);
            Assert.That(FrameNodeTemplateDictionaryAsDictionary.Values != null);
            Assert.That(FrameNodeTemplateDictionaryAsDictionary.ContainsKey(Type.FromTypeof<Leaf>()));
            FrameNodeTemplateDictionaryAsDictionary.Remove(Type.FromTypeof<Leaf>());
            FrameNodeTemplateDictionaryAsDictionary.Add(Type.FromTypeof<Leaf>(), LeafTemplate);
            Assert.That(FrameNodeTemplateDictionaryAsDictionary.TryGetValue(Type.FromTypeof<Leaf>(), out IFrameTemplate AsFrameTemplate) == NodeTemplateDictionary.TryGetValue(Type.FromTypeof<Leaf>(), out IFrameTemplate AsFrameLayoutTemplate));
            ICollection<KeyValuePair<Type, IFrameTemplate>> FrameNodeTemplateDictionaryAsCollection = FrameNodeTemplateDictionary;
            Assert.That(!FrameNodeTemplateDictionaryAsCollection.IsReadOnly);
            foreach (KeyValuePair<Type, IFrameTemplate> Entry in FrameNodeTemplateDictionary)
            {
                Assert.That(FrameNodeTemplateDictionaryAsCollection.Contains(Entry));
                FrameNodeTemplateDictionaryAsCollection.Remove(Entry);
                FrameNodeTemplateDictionaryAsCollection.Add(Entry);
                break;
            }
            FrameNodeTemplateDictionaryAsCollection.CopyTo(new KeyValuePair<Type, IFrameTemplate>[FrameNodeTemplateDictionaryAsCollection.Count], 0);

            FocusTemplateDictionary FocusNodeTemplateDictionary = NodeTemplateDictionary;
            Assert.That(FocusNodeTemplateDictionary[Type.FromTypeof<Leaf>()] != null);
            IDictionary<Type, IFocusTemplate> FocusNodeTemplateDictionaryAsDictionary = FocusNodeTemplateDictionary;
            Assert.That(FocusNodeTemplateDictionaryAsDictionary.Keys != null);
            Assert.That(FocusNodeTemplateDictionaryAsDictionary.Values != null);
            Assert.That(FocusNodeTemplateDictionaryAsDictionary.ContainsKey(Type.FromTypeof<Leaf>()));
            Assert.That(FocusNodeTemplateDictionaryAsDictionary[Type.FromTypeof<Leaf>()] != null);
            FocusNodeTemplateDictionaryAsDictionary.Remove(Type.FromTypeof<Leaf>());
            FocusNodeTemplateDictionaryAsDictionary.Add(Type.FromTypeof<Leaf>(), LeafTemplate);
            Assert.That(FocusNodeTemplateDictionaryAsDictionary.TryGetValue(Type.FromTypeof<Leaf>(), out IFocusTemplate AsFocusTemplate) == NodeTemplateDictionary.TryGetValue(Type.FromTypeof<Leaf>(), out IFrameTemplate AsFocusLayoutTemplate));
            ICollection<KeyValuePair<Type, IFocusTemplate>> FocusNodeTemplateDictionaryAsCollection = FocusNodeTemplateDictionary;
            Assert.That(!FocusNodeTemplateDictionaryAsCollection.IsReadOnly);
            foreach (KeyValuePair<Type, IFocusTemplate> Entry in (ICollection<KeyValuePair<Type, IFocusTemplate>>)FocusNodeTemplateDictionary)
            {
                Assert.That(FocusNodeTemplateDictionaryAsCollection.Contains(Entry));
                FocusNodeTemplateDictionaryAsCollection.Remove(Entry);
                FocusNodeTemplateDictionaryAsCollection.Add(Entry);
                break;
            }
            FocusNodeTemplateDictionaryAsCollection.CopyTo(new KeyValuePair<Type, IFocusTemplate>[FocusNodeTemplateDictionaryAsCollection.Count], 0);
            IEnumerable<KeyValuePair<Type, IFocusTemplate>> FocusNodeTemplateDictionaryAsEnumerable = FocusNodeTemplateDictionary;
            FocusNodeTemplateDictionaryAsEnumerable.GetEnumerator();

            // ILayoutTemplateReadOnlyDictionary

            LayoutTemplateReadOnlyDictionary NodeTemplateDictionaryReadOnly = LayoutTemplateSet.NodeTemplateTable;
            LayoutTemplateReadOnlyDictionary BlockTemplateTableReadOnly = LayoutTemplateSet.BlockTemplateTable;

            FrameTemplateReadOnlyDictionary FrameNodeTemplateDictionaryReadOnly = NodeTemplateDictionaryReadOnly;
            FrameNodeTemplateDictionaryReadOnly.GetEnumerator();
            IReadOnlyDictionary<Type, IFrameTemplate> FrameNodeTemplateDictionaryReadOnlyAsDictionary = FrameNodeTemplateDictionaryReadOnly;
            Assert.That(FrameNodeTemplateDictionaryReadOnlyAsDictionary.ContainsKey(Type.FromTypeof<Leaf>()));
            Assert.That(FrameNodeTemplateDictionaryReadOnlyAsDictionary[Type.FromTypeof<Leaf>()] != null);
            Assert.That(FrameNodeTemplateDictionaryReadOnlyAsDictionary.Keys != null);
            Assert.That(FrameNodeTemplateDictionaryReadOnlyAsDictionary.Values != null);
            Assert.That(FrameNodeTemplateDictionaryReadOnlyAsDictionary.TryGetValue(Type.FromTypeof<Leaf>(), out AsFrameTemplate) == NodeTemplateDictionary.TryGetValue(Type.FromTypeof<Leaf>(), out AsFrameLayoutTemplate));
            IEnumerable<KeyValuePair<Type, IFrameTemplate>> FrameNodeTemplateDictionaryReadOnlyAsEnumerable = FrameNodeTemplateDictionaryReadOnly;
            FrameNodeTemplateDictionaryReadOnlyAsEnumerable.GetEnumerator();

            FocusTemplateReadOnlyDictionary FocusNodeTemplateDictionaryReadOnly = NodeTemplateDictionaryReadOnly;
            FocusNodeTemplateDictionaryReadOnly.GetEnumerator();
            IReadOnlyDictionary<Type, IFocusTemplate> FocusNodeTemplateDictionaryReadOnlyAsDictionary = FocusNodeTemplateDictionaryReadOnly;
            Assert.That(FocusNodeTemplateDictionaryReadOnlyAsDictionary.ContainsKey(Type.FromTypeof<Leaf>()));
            Assert.That(FocusNodeTemplateDictionaryReadOnlyAsDictionary[Type.FromTypeof<Leaf>()] != null);
            Assert.That(FocusNodeTemplateDictionaryReadOnlyAsDictionary.Keys != null);
            Assert.That(FocusNodeTemplateDictionaryReadOnlyAsDictionary.Values != null);
            Assert.That(FocusNodeTemplateDictionaryReadOnlyAsDictionary.TryGetValue(Type.FromTypeof<Leaf>(), out AsFocusTemplate) == NodeTemplateDictionary.TryGetValue(Type.FromTypeof<Leaf>(), out AsFocusLayoutTemplate));
            IEnumerable<KeyValuePair<Type, IFocusTemplate>> FocusNodeTemplateDictionaryReadOnlyAsEnumerable = FocusNodeTemplateDictionaryReadOnly;
            FocusNodeTemplateDictionaryReadOnlyAsEnumerable.GetEnumerator();

            // ILayoutTemplateList 

            LayoutTemplateList TemplateList = TestDebug.CoverageLayoutTemplateSet.Templates;
            Assert.That(TemplateList.Count > 0);
            ILayoutTemplate FirstTemplate = (ILayoutTemplate)TemplateList[0];

            FrameTemplateList FrameTemplateList = TemplateList;
            FrameTemplateList.GetEnumerator();
            Assert.That(FrameTemplateList[0] == FirstTemplate);
            IList<IFrameTemplate> FrameTemplateListAsList = FrameTemplateList;
            Assert.That(FrameTemplateListAsList[0] == FirstTemplate);
            Assert.That(FrameTemplateListAsList.IndexOf(FirstTemplate) == 0);
            ICollection<IFrameTemplate> FrameTemplateListAsCollection = FrameTemplateList;
            Assert.That(!FrameTemplateListAsCollection.IsReadOnly);
            FrameTemplateListAsCollection.Contains(FirstTemplate);
            FrameTemplateListAsCollection.Remove(FirstTemplate);
            FrameTemplateListAsCollection.Add(FirstTemplate);
            FrameTemplateListAsCollection.Remove(FirstTemplate);
            FrameTemplateListAsList.Insert(0, FirstTemplate);
            FrameTemplateListAsCollection.CopyTo(new ILayoutTemplate[FrameTemplateListAsCollection.Count], 0);
            IEnumerable<IFrameTemplate> FrameTemplateListAsEnumerable = FrameTemplateList;
            FrameTemplateListAsEnumerable.GetEnumerator();
            IReadOnlyList<IFrameTemplate> FrameTemplateListAsReadOnlyList = FrameTemplateList;
            Assert.That(FrameTemplateListAsReadOnlyList[0] == FirstTemplate);

            FocusTemplateList FocusTemplateList = TemplateList;
            FocusTemplateList.GetEnumerator();
            Assert.That(FocusTemplateList[0] == FirstTemplate);
            IList<IFocusTemplate> FocusTemplateListAsList = FocusTemplateList;
            Assert.That(FocusTemplateListAsList[0] == FirstTemplate);
            Assert.That(FocusTemplateListAsList.IndexOf(FirstTemplate) == 0);
            ICollection<IFocusTemplate> FocusTemplateListAsCollection = FocusTemplateList;
            Assert.That(!FocusTemplateListAsCollection.IsReadOnly);
            FocusTemplateListAsCollection.Contains(FirstTemplate);
            FocusTemplateListAsCollection.Remove(FirstTemplate);
            FocusTemplateListAsCollection.Add(FirstTemplate);
            FocusTemplateListAsCollection.Remove(FirstTemplate);
            FocusTemplateListAsList.Insert(0, FirstTemplate);
            FocusTemplateListAsCollection.CopyTo(new ILayoutTemplate[FocusTemplateListAsCollection.Count], 0);
            IEnumerable<IFocusTemplate> FocusTemplateListAsEnumerable = FocusTemplateList;
            FocusTemplateListAsEnumerable.GetEnumerator();
            IReadOnlyList<IFocusTemplate> FocusTemplateListAsReadOnlyList = FocusTemplateList;
            Assert.That(FocusTemplateListAsReadOnlyList[0] == FirstTemplate);

            // ILayoutCycleManagerList

            LayoutCycleManagerList CycleManagerList = Controller.CycleManagerList;
            Assert.That(CycleManagerList.Count > 0);
            ILayoutCycleManager FirstCycleManager = (ILayoutCycleManager)CycleManagerList[0];

            FocusCycleManagerList FocusCycleManagerList = CycleManagerList;
            Assert.That(FocusCycleManagerList.Contains(FirstCycleManager));
            Assert.That(FocusCycleManagerList[0] == FirstCycleManager);
            Assert.That(FocusCycleManagerList.IndexOf(FirstCycleManager) == 0);
            IList<IFocusCycleManager> FocusCycleManagerListAsList = FocusCycleManagerList;
            Assert.That(FocusCycleManagerListAsList.Contains(FirstCycleManager));
            Assert.That(FocusCycleManagerListAsList[0] == FirstCycleManager);
            Assert.That(FocusCycleManagerListAsList.IndexOf(FirstCycleManager) == 0);
            ICollection<IFocusCycleManager> FocusCycleManagerListAsCollection = FocusCycleManagerList;
            Assert.That(!FocusCycleManagerListAsCollection.IsReadOnly);
            Assert.That(FocusCycleManagerListAsCollection.Contains(FirstCycleManager));
            FocusCycleManagerListAsCollection.Remove(FirstCycleManager);
            FocusCycleManagerListAsCollection.Add(FirstCycleManager);
            FocusCycleManagerListAsList.Remove(FirstCycleManager);
            FocusCycleManagerListAsList.Insert(0, FirstCycleManager);
            FocusCycleManagerListAsCollection.CopyTo(new ILayoutCycleManager[FocusCycleManagerListAsCollection.Count], 0);
            IEnumerable<IFocusCycleManager> FocusCycleManagerListAsEnumerable = FocusCycleManagerList;
            FocusCycleManagerListAsEnumerable.GetEnumerator();
            IReadOnlyList<IFocusCycleManager> FocusCycleManagerListAsReadOnlyList = FocusCycleManagerList;
            Assert.That(FocusCycleManagerListAsReadOnlyList[0] == FirstCycleManager);

            foreach (KeyValuePair<Type, ILayoutTemplate> TemplateEntry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)NodeTemplateDictionary)
                if (TemplateEntry.Key.IsTypeof<Root>())
                {
                    // ILayoutFrameSelectorList 

                    ILayoutHorizontalPanelFrame RootFrame = TemplateEntry.Value.Root as ILayoutHorizontalPanelFrame;
                    foreach (ILayoutFrame Frame in RootFrame.Items)
                        if (Frame is ILayoutFrameWithSelector AsFrameWithSelector && AsFrameWithSelector.Selectors.Count > 0)
                        {
                            LayoutFrameSelectorList FrameSelectorList = AsFrameWithSelector.Selectors;

                            Assert.That(FrameSelectorList.Count > 0);
                            ILayoutFrameSelector FirstFrameSelector = (ILayoutFrameSelector)FrameSelectorList[0];

                            FocusFrameSelectorList FocusFrameSelectorList = FrameSelectorList;
                            Assert.That(FocusFrameSelectorList.Contains(FirstFrameSelector));
                            Assert.That(FocusFrameSelectorList[0] == FirstFrameSelector);
                            Assert.That(FocusFrameSelectorList.IndexOf(FirstFrameSelector) == 0);
                            IList<IFocusFrameSelector> FocusFrameSelectorListAsList = FocusFrameSelectorList;
                            Assert.That(FocusFrameSelectorListAsList.Contains(FirstFrameSelector));
                            Assert.That(FocusFrameSelectorListAsList[0] == FirstFrameSelector);
                            Assert.That(FocusFrameSelectorListAsList.IndexOf(FirstFrameSelector) == 0);
                            ICollection<IFocusFrameSelector> FocusFrameSelectorListAsCollection = FocusFrameSelectorList;
                            Assert.That(!FocusFrameSelectorListAsCollection.IsReadOnly);
                            Assert.That(FocusFrameSelectorListAsCollection.Contains(FirstFrameSelector));
                            FocusFrameSelectorListAsCollection.Remove(FirstFrameSelector);
                            FocusFrameSelectorListAsCollection.Add(FirstFrameSelector);
                            FocusFrameSelectorListAsList.Remove(FirstFrameSelector);
                            FocusFrameSelectorListAsList.Insert(0, FirstFrameSelector);
                            FocusFrameSelectorListAsCollection.CopyTo(new ILayoutFrameSelector[FocusFrameSelectorListAsCollection.Count], 0);
                            IEnumerable<IFocusFrameSelector> FocusFrameSelectorListAsEnumerable = FocusFrameSelectorList;
                            FocusFrameSelectorListAsEnumerable.GetEnumerator();
                            IReadOnlyList<IFocusFrameSelector> FocusFrameSelectorListAsReadOnlyList = FocusFrameSelectorList;
                            Assert.That(FocusFrameSelectorListAsReadOnlyList[0] == FirstFrameSelector);

                            break;
                        }
                }

            foreach (KeyValuePair<Type, ILayoutTemplate> TemplateEntry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)NodeTemplateDictionary)
                if (TemplateEntry.Key.IsTypeof<Main>())
                {
                    // ILayoutNodeFrameVisibilityList

                    ILayoutHorizontalPanelFrame RootFrame = GetFirstHorizontalFrame(TemplateEntry.Value.Root);
                    foreach (ILayoutFrame Frame in RootFrame.Items)
                        if (Frame is ILayoutKeywordFrame AsKeywordFrame && AsKeywordFrame.Visibility is ILayoutMixedFrameVisibility AsMixedFrameVisibility && AsMixedFrameVisibility.Items.Count > 0)
                        {
                            LayoutNodeFrameVisibilityList NodeFrameVisibilityList = AsMixedFrameVisibility.Items;

                            Assert.That(NodeFrameVisibilityList.Count > 0);
                            ILayoutNodeFrameVisibility FirstNodeFrameVisibility = (ILayoutNodeFrameVisibility)NodeFrameVisibilityList[0];

                            FocusNodeFrameVisibilityList FocusNodeFrameVisibilityList = NodeFrameVisibilityList;
                            Assert.That(FocusNodeFrameVisibilityList.Contains(FirstNodeFrameVisibility));
                            Assert.That(FocusNodeFrameVisibilityList[0] == FirstNodeFrameVisibility);
                            Assert.That(FocusNodeFrameVisibilityList.IndexOf(FirstNodeFrameVisibility) == 0);
                            IList<IFocusNodeFrameVisibility> FocusNodeFrameVisibilityListAsList = FocusNodeFrameVisibilityList;
                            Assert.That(FocusNodeFrameVisibilityListAsList.Contains(FirstNodeFrameVisibility));
                            Assert.That(FocusNodeFrameVisibilityListAsList[0] == FirstNodeFrameVisibility);
                            Assert.That(FocusNodeFrameVisibilityListAsList.IndexOf(FirstNodeFrameVisibility) == 0);
                            ICollection<IFocusNodeFrameVisibility> FocusNodeFrameVisibilityListAsCollection = FocusNodeFrameVisibilityList;
                            Assert.That(!FocusNodeFrameVisibilityListAsCollection.IsReadOnly);
                            Assert.That(FocusNodeFrameVisibilityListAsCollection.Contains(FirstNodeFrameVisibility));
                            FocusNodeFrameVisibilityListAsCollection.Remove(FirstNodeFrameVisibility);
                            FocusNodeFrameVisibilityListAsCollection.Add(FirstNodeFrameVisibility);
                            FocusNodeFrameVisibilityListAsList.Remove(FirstNodeFrameVisibility);
                            FocusNodeFrameVisibilityListAsList.Insert(0, FirstNodeFrameVisibility);
                            FocusNodeFrameVisibilityListAsCollection.CopyTo(new ILayoutNodeFrameVisibility[FocusNodeFrameVisibilityListAsCollection.Count], 0);
                            IEnumerable<IFocusNodeFrameVisibility> FocusNodeFrameVisibilityListAsEnumerable = FocusNodeFrameVisibilityList;
                            FocusNodeFrameVisibilityListAsEnumerable.GetEnumerator();
                            IReadOnlyList<IFocusNodeFrameVisibility> FocusNodeFrameVisibilityListAsReadOnlyList = FocusNodeFrameVisibilityList;
                            Assert.That(FocusNodeFrameVisibilityListAsReadOnlyList[0] == FirstNodeFrameVisibility);

                            break;
                        }
                }

            foreach (KeyValuePair<Type, ILayoutTemplate> TemplateEntry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)NodeTemplateDictionary)
                if (TemplateEntry.Key.IsTypeof<BaseNode.DeferredBody>())
                {
                    // ILayoutFrameSelectorList 

                    ILayoutSelectionFrame RootFrame = TemplateEntry.Value.Root as ILayoutSelectionFrame;
                    LayoutSelectableFrameList SelectableFrameList = RootFrame.Items;

                    Assert.That(SelectableFrameList.Count > 0);
                    ILayoutSelectableFrame FirstSelectableFrame = (ILayoutSelectableFrame)SelectableFrameList[0];

                    FocusSelectableFrameList FocusSelectableFrameList = SelectableFrameList;
                    Assert.That(FocusSelectableFrameList.Contains(FirstSelectableFrame));
                    Assert.That(FocusSelectableFrameList[0] == FirstSelectableFrame);
                    Assert.That(FocusSelectableFrameList.IndexOf(FirstSelectableFrame) == 0);
                    IList<IFocusSelectableFrame> FocusSelectableFrameListAsList = FocusSelectableFrameList;
                    Assert.That(FocusSelectableFrameListAsList.Contains(FirstSelectableFrame));
                    Assert.That(FocusSelectableFrameListAsList[0] == FirstSelectableFrame);
                    Assert.That(FocusSelectableFrameListAsList.IndexOf(FirstSelectableFrame) == 0);
                    ICollection<IFocusSelectableFrame> FocusSelectableFrameListAsCollection = FocusSelectableFrameList;
                    Assert.That(!FocusSelectableFrameListAsCollection.IsReadOnly);
                    Assert.That(FocusSelectableFrameListAsCollection.Contains(FirstSelectableFrame));
                    FocusSelectableFrameListAsCollection.Remove(FirstSelectableFrame);
                    FocusSelectableFrameListAsCollection.Add(FirstSelectableFrame);
                    FocusSelectableFrameListAsList.Remove(FirstSelectableFrame);
                    FocusSelectableFrameListAsList.Insert(0, FirstSelectableFrame);
                    FocusSelectableFrameListAsCollection.CopyTo(new ILayoutSelectableFrame[FocusSelectableFrameListAsCollection.Count], 0);
                    IEnumerable<IFocusSelectableFrame> FocusSelectableFrameListAsEnumerable = FocusSelectableFrameList;
                    FocusSelectableFrameListAsEnumerable.GetEnumerator();
                    IReadOnlyList<IFocusSelectableFrame> FocusSelectableFrameListAsReadOnlyList = FocusSelectableFrameList;
                    Assert.That(FocusSelectableFrameListAsReadOnlyList[0] == FirstSelectableFrame);

                    break;
                }

            LayoutTemplateDictionary BlockTemplateDictionary = TestDebug.CoverageLayoutTemplateSet.BlockTemplateDictionary;
            foreach (KeyValuePair<Type, ILayoutTemplate> TemplateEntry in (ICollection<KeyValuePair<Type, ILayoutTemplate>>)BlockTemplateDictionary)
            {
                ILayoutPanelFrame RootFrame = TemplateEntry.Value.Root as ILayoutPanelFrame;
                foreach (ILayoutFrame Frame in RootFrame.Items)
                    if (Frame is ILayoutCollectionPlaceholderFrame AsCollectionPlaceholderFrame)
                    {
                        LayoutFrameSelectorList Selectors = AsCollectionPlaceholderFrame.Selectors;
                        break;
                    }
            }
        }

        public static ILayoutHorizontalPanelFrame GetFirstHorizontalFrame(ILayoutFrame frame)
        {
            if (frame is ILayoutHorizontalPanelFrame)
                return frame as ILayoutHorizontalPanelFrame;

            ILayoutVerticalPanelFrame VerticalPanelFrame = frame as ILayoutVerticalPanelFrame;
            Assert.That(VerticalPanelFrame != null);

            ILayoutHorizontalPanelFrame HorizontalPanelFrame = null;
            foreach (ILayoutFrame Item in VerticalPanelFrame.Items)
                if (Item is ILayoutHorizontalPanelFrame)
                {
                    HorizontalPanelFrame = Item as ILayoutHorizontalPanelFrame;
                    break;
                }

            return HorizontalPanelFrame;
        }

        public static void EnumerateFrames(List<ILayoutFrame> frameList, ILayoutFrame rootFrame)
        {
            if (rootFrame is ILayoutPanelFrame AsPanelFrame)
            {
                foreach (ILayoutFrame Item in AsPanelFrame.Items)
                    EnumerateFrames(frameList, Item);
            }
            else
                frameList.Add(rootFrame);
        }

        [Test]
        [Category("Coverage")]
        public static void LayoutSelectorTable()
        {
            BaseNode.Class Root = BaseNodeHelper.NodeHelper.CreateEmptyNode(Type.FromTypeof<BaseNode.Class>()) as BaseNode.Class;
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(Root);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, EaslyEdit.CustomLayoutTemplateSet.LayoutTemplateSet, TestDebug.LayoutDrawPrintContext.Default);
        }
    }
}
