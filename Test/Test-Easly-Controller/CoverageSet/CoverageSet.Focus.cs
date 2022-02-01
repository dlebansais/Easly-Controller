using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using EaslyController;
using EaslyController.Constants;
using EaslyController.Controller;
using EaslyController.Focus;
using EaslyController.Frame;
using EaslyController.Layout;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using NUnit.Framework;

namespace Coverage
{
    [TestFixture]
    public partial class CoverageSet
    {
        [Test]
        [Category("Coverage")]
        public static void FocusCreation()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;
            FocusController Controller;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

            try
            {
                RootIndex = new FocusRootNodeIndex(RootNode);
                Controller = FocusController.Create(RootIndex);
            }
            catch (Exception e)
            {
                Assert.Fail($"#0: {e}");
            }

            RootNode = CreateRoot(ValueGuid0, Imperfections.BadGuid);
            Assert.That(!BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode, throwOnInvalid: false));

            try
            {
                RootIndex = new FocusRootNodeIndex(RootNode);
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
        public static void FocusProperties()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex0;
            IFocusRootNodeIndex RootIndex1;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

            RootIndex0 = new FocusRootNodeIndex(RootNode);
            Assert.That(RootIndex0.Node == RootNode);
            Assert.That(RootIndex0.IsEqual(CompareEqual.New(), RootIndex0));

            RootIndex1 = new FocusRootNodeIndex(RootNode);
            Assert.That(RootIndex1.Node == RootNode);
            Assert.That(CompareEqual.CoverIsEqual(RootIndex0, RootIndex1));

            FocusController Controller0 = FocusController.Create(RootIndex0);
            Assert.That(Controller0.RootIndex == RootIndex0);

            Stats Stats = Controller0.Stats;
            Assert.That(Stats.NodeCount >= 0);
            Assert.That(Stats.PlaceholderNodeCount >= 0);
            Assert.That(Stats.OptionalNodeCount >= 0);
            Assert.That(Stats.AssignedOptionalNodeCount >= 0);
            Assert.That(Stats.ListCount >= 0);
            Assert.That(Stats.BlockListCount >= 0);
            Assert.That(Stats.BlockCount >= 0);

            IFocusPlaceholderNodeState RootState = Controller0.RootState;
            Assert.That(RootState.ParentIndex == RootIndex0);

            Assert.That(Controller0.Contains(RootIndex0));
            Assert.That(Controller0.IndexToState(RootIndex0) == RootState);

            Assert.That(RootState.InnerTable.Count == 8, $"New count: {RootState.InnerTable.Count}");
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.PlaceholderTree)));
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.PlaceholderLeaf)));
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.UnassignedOptionalLeaf)));
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.EmptyOptionalLeaf)));
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.AssignedOptionalTree)));
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.AssignedOptionalLeaf)));
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.LeafBlocks)));
            Assert.That(RootState.InnerTable.ContainsKey(nameof(Main.LeafPath)));

            IFocusPlaceholderInner MainPlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IFocusPlaceholderInner;
            Assert.That(MainPlaceholderTreeInner != null);
            Assert.That(MainPlaceholderTreeInner.InterfaceType == typeof(Tree));
            Assert.That(MainPlaceholderTreeInner.ChildState != null);
            Assert.That(MainPlaceholderTreeInner.ChildState.ParentInner == MainPlaceholderTreeInner);

            IFocusPlaceholderInner MainPlaceholderLeafInner = RootState.PropertyToInner(nameof(Main.PlaceholderLeaf)) as IFocusPlaceholderInner;
            Assert.That(MainPlaceholderLeafInner != null);
            Assert.That(MainPlaceholderLeafInner.InterfaceType == typeof(Leaf));
            Assert.That(MainPlaceholderLeafInner.ChildState != null);
            Assert.That(MainPlaceholderLeafInner.ChildState.ParentInner == MainPlaceholderLeafInner);

            IFocusOptionalInner MainUnassignedOptionalInner = RootState.PropertyToInner(nameof(Main.UnassignedOptionalLeaf)) as IFocusOptionalInner;
            Assert.That(MainUnassignedOptionalInner != null);
            Assert.That(MainUnassignedOptionalInner.InterfaceType == typeof(Leaf));
            Assert.That(!MainUnassignedOptionalInner.IsAssigned);
            Assert.That(MainUnassignedOptionalInner.ChildState != null);
            Assert.That(MainUnassignedOptionalInner.ChildState.ParentInner == MainUnassignedOptionalInner);

            IFocusOptionalInner MainAssignedOptionalTreeInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalTree)) as IFocusOptionalInner;
            Assert.That(MainAssignedOptionalTreeInner != null);
            Assert.That(MainAssignedOptionalTreeInner.InterfaceType == typeof(Tree));
            Assert.That(MainAssignedOptionalTreeInner.IsAssigned);

            IFocusNodeState AssignedOptionalTreeState = MainAssignedOptionalTreeInner.ChildState;
            Assert.That(AssignedOptionalTreeState != null);
            Assert.That(AssignedOptionalTreeState.ParentInner == MainAssignedOptionalTreeInner);
            Assert.That(AssignedOptionalTreeState.ParentState == RootState);

            FocusNodeStateReadOnlyList AssignedOptionalTreeAllChildren = AssignedOptionalTreeState.GetAllChildren() as FocusNodeStateReadOnlyList;
            Assert.That(AssignedOptionalTreeAllChildren != null);
            Assert.That(AssignedOptionalTreeAllChildren.Count == 2, $"New count: {AssignedOptionalTreeAllChildren.Count}");

            IFocusOptionalInner MainAssignedOptionalLeafInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IFocusOptionalInner;
            Assert.That(MainAssignedOptionalLeafInner != null);
            Assert.That(MainAssignedOptionalLeafInner.InterfaceType == typeof(Leaf));
            Assert.That(MainAssignedOptionalLeafInner.IsAssigned);
            Assert.That(MainAssignedOptionalLeafInner.ChildState != null);
            Assert.That(MainAssignedOptionalLeafInner.ChildState.ParentInner == MainAssignedOptionalLeafInner);

            IFocusBlockListInner MainLeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
            Assert.That(MainLeafBlocksInner != null);
            Assert.That(!MainLeafBlocksInner.IsNeverEmpty);
            Assert.That(!MainLeafBlocksInner.IsEmpty);
            Assert.That(!MainLeafBlocksInner.IsSingle);
            Assert.That(MainLeafBlocksInner.InterfaceType == typeof(Leaf));
            Assert.That(MainLeafBlocksInner.BlockType == typeof(BaseNode.IBlock<Leaf>));
            Assert.That(MainLeafBlocksInner.ItemType == typeof(Leaf));
            Assert.That(MainLeafBlocksInner.Count == 4);
            Assert.That(MainLeafBlocksInner.BlockStateList != null);
            Assert.That(MainLeafBlocksInner.BlockStateList.Count == 3);
            Assert.That(MainLeafBlocksInner.AllIndexes().Count == MainLeafBlocksInner.Count);

            IFocusBlockState LeafBlock = (IFocusBlockState)MainLeafBlocksInner.BlockStateList[0];
            Assert.That(LeafBlock != null);
            Assert.That(LeafBlock.StateList != null);
            Assert.That(LeafBlock.StateList.Count == 1);
            Assert.That(MainLeafBlocksInner.FirstNodeState == LeafBlock.StateList[0]);
            Assert.That(MainLeafBlocksInner.IndexAt(0, 0) == MainLeafBlocksInner.FirstNodeState.ParentIndex);

            IFocusPlaceholderInner PatternInner = LeafBlock.PropertyToInner(nameof(BaseNode.IBlock.ReplicationPattern)) as IFocusPlaceholderInner;
            Assert.That(PatternInner != null);

            IFocusPlaceholderInner SourceInner = LeafBlock.PropertyToInner(nameof(BaseNode.IBlock.SourceIdentifier)) as IFocusPlaceholderInner;
            Assert.That(SourceInner != null);

            IFocusPatternState PatternState = LeafBlock.PatternState;
            Assert.That(PatternState != null);
            Assert.That(PatternState.ParentBlockState == LeafBlock);
            Assert.That(PatternState.ParentInner == PatternInner);
            Assert.That(PatternState.ParentIndex == LeafBlock.PatternIndex);
            Assert.That(PatternState.ParentState == RootState);
            Assert.That(PatternState.InnerTable.Count == 0);
            Assert.That(PatternState is IFocusNodeState AsPlaceholderPatternNodeState && AsPlaceholderPatternNodeState.ParentIndex == LeafBlock.PatternIndex);
            Assert.That(PatternState.GetAllChildren().Count == 1);

            IFocusSourceState SourceState = LeafBlock.SourceState;
            Assert.That(SourceState != null);
            Assert.That(SourceState.ParentBlockState == LeafBlock);
            Assert.That(SourceState.ParentInner == SourceInner);
            Assert.That(SourceState.ParentIndex == LeafBlock.SourceIndex);
            Assert.That(SourceState.ParentState == RootState);
            Assert.That(SourceState.InnerTable.Count == 0);
            Assert.That(SourceState is IFocusNodeState AsPlaceholderSourceNodeState && AsPlaceholderSourceNodeState.ParentIndex == LeafBlock.SourceIndex);
            Assert.That(SourceState.GetAllChildren().Count == 1);

            Assert.That(MainLeafBlocksInner.FirstNodeState == LeafBlock.StateList[0]);

            IFocusListInner MainLeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFocusListInner;
            Assert.That(MainLeafPathInner != null);
            Assert.That(!MainLeafPathInner.IsNeverEmpty);
            Assert.That(MainLeafPathInner.InterfaceType == typeof(Leaf));
            Assert.That(MainLeafPathInner.Count == 2);
            Assert.That(MainLeafPathInner.StateList != null);
            Assert.That(MainLeafPathInner.StateList.Count == 2);
            Assert.That(MainLeafPathInner.FirstNodeState == MainLeafPathInner.StateList[0]);
            Assert.That(MainLeafPathInner.IndexAt(0) == MainLeafPathInner.FirstNodeState.ParentIndex);
            Assert.That(MainLeafPathInner.AllIndexes().Count == MainLeafPathInner.Count);

            FocusNodeStateReadOnlyList AllChildren = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
            Assert.That(AllChildren.Count == 19, $"New count: {AllChildren.Count}");

            IFocusPlaceholderInner PlaceholderInner = RootState.InnerTable[nameof(Main.PlaceholderLeaf)] as IFocusPlaceholderInner;
            Assert.That(PlaceholderInner != null);

            IFocusBrowsingPlaceholderNodeIndex PlaceholderNodeIndex = PlaceholderInner.ChildState.ParentIndex as IFocusBrowsingPlaceholderNodeIndex;
            Assert.That(PlaceholderNodeIndex != null);
            Assert.That(Controller0.Contains(PlaceholderNodeIndex));

            IFocusOptionalInner UnassignedOptionalInner = RootState.InnerTable[nameof(Main.UnassignedOptionalLeaf)] as IFocusOptionalInner;
            Assert.That(UnassignedOptionalInner != null);

            IFocusBrowsingOptionalNodeIndex UnassignedOptionalNodeIndex = UnassignedOptionalInner.ChildState.ParentIndex;
            Assert.That(UnassignedOptionalNodeIndex != null);
            Assert.That(Controller0.Contains(UnassignedOptionalNodeIndex));
            Assert.That(Controller0.IsAssigned(UnassignedOptionalNodeIndex) == false);

            IFocusOptionalInner AssignedOptionalInner = RootState.InnerTable[nameof(Main.AssignedOptionalLeaf)] as IFocusOptionalInner;
            Assert.That(AssignedOptionalInner != null);

            IFocusBrowsingOptionalNodeIndex AssignedOptionalNodeIndex = AssignedOptionalInner.ChildState.ParentIndex;
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

            FocusController Controller1 = FocusController.Create(RootIndex0);
            Assert.That(Controller0.IsEqual(CompareEqual.New(), Controller0));

            //System.Diagnostics.Debug.Assert(false);
            Assert.That(CompareEqual.CoverIsEqual(Controller0, Controller1));

            Assert.That(!Controller0.CanUndo);
            Assert.That(!Controller0.CanRedo);
        }

        [Test]
        [Category("Coverage")]
        public static void FocusClone()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode = CreateRoot(ValueGuid0, Imperfections.None);

            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(RootNode);
            Assert.That(RootIndex != null);

            FocusController Controller = FocusController.Create(RootIndex);
            Assert.That(Controller != null);

            IFocusPlaceholderNodeState RootState = Controller.RootState;
            Assert.That(RootState != null);

            BaseNode.Node ClonedNode = RootState.CloneNode();
            Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(ClonedNode));

            IFocusRootNodeIndex CloneRootIndex = new FocusRootNodeIndex(ClonedNode);
            Assert.That(CloneRootIndex != null);

            FocusController CloneController = FocusController.Create(CloneRootIndex);
            Assert.That(CloneController != null);

            IFocusPlaceholderNodeState CloneRootState = Controller.RootState;
            Assert.That(CloneRootState != null);

            FocusNodeStateReadOnlyList AllChildren = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
            FocusNodeStateReadOnlyList CloneAllChildren = (FocusNodeStateReadOnlyList)CloneRootState.GetAllChildren();
            Assert.That(AllChildren.Count == CloneAllChildren.Count);
        }

        [Test]
        [Category("Coverage")]
        public static void FocusInsert()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFocusListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFocusListInner;
                Assert.That(LeafPathInner != null);

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                IFocusBrowsingListNodeIndex ExistingIndex = LeafPathInner.IndexAt(0) as IFocusBrowsingListNodeIndex;

                Leaf NewItem0 = CreateLeaf(Guid.NewGuid());

                IFocusInsertionListNodeIndex InsertionIndex0;
                InsertionIndex0 = ExistingIndex.ToInsertionIndex(RootNode, NewItem0) as IFocusInsertionListNodeIndex;
                Assert.That(InsertionIndex0.ParentNode == RootNode);
                Assert.That(InsertionIndex0.Node == NewItem0);
                Assert.That(CompareEqual.CoverIsEqual(InsertionIndex0, InsertionIndex0));

                FocusNodeStateReadOnlyList AllChildren0 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Insert(LeafPathInner, InsertionIndex0, out IWriteableBrowsingCollectionNodeIndex NewItemIndex0);
                Assert.That(Controller.Contains(NewItemIndex0));

                IFocusBrowsingListNodeIndex DuplicateExistingIndex0 = InsertionIndex0.ToBrowsingIndex() as IFocusBrowsingListNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex0 as IFocusBrowsingListNodeIndex, DuplicateExistingIndex0));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex0, NewItemIndex0 as IFocusBrowsingListNodeIndex));

                Assert.That(LeafPathInner.Count == PathCount + 1);
                Assert.That(LeafPathInner.StateList.Count == PathCount + 1);

                IFocusPlaceholderNodeState NewItemState0 = (IFocusPlaceholderNodeState)LeafPathInner.StateList[0];
                Assert.That(NewItemState0.Node == NewItem0);
                Assert.That(NewItemState0.ParentIndex == NewItemIndex0);

                FocusNodeStateReadOnlyList AllChildren1 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                IFocusBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
                Assert.That(LeafBlocksInner != null);

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[0].StateList.Count;
                Assert.That(BlockNodeCount == 4);

                IFocusBrowsingExistingBlockNodeIndex ExistingIndex1 = LeafBlocksInner.IndexAt(0, 0) as IFocusBrowsingExistingBlockNodeIndex;

                Leaf NewItem1 = CreateLeaf(Guid.NewGuid());
                IFocusInsertionExistingBlockNodeIndex InsertionIndex1;
                InsertionIndex1 = ExistingIndex1.ToInsertionIndex(RootNode, NewItem1) as IFocusInsertionExistingBlockNodeIndex;
                Assert.That(InsertionIndex1.ParentNode == RootNode);
                Assert.That(InsertionIndex1.Node == NewItem1);
                Assert.That(CompareEqual.CoverIsEqual(InsertionIndex1, InsertionIndex1));

                Controller.Insert(LeafBlocksInner, InsertionIndex1, out IWriteableBrowsingCollectionNodeIndex NewItemIndex1);
                Assert.That(Controller.Contains(NewItemIndex1));

                IFocusBrowsingExistingBlockNodeIndex DuplicateExistingIndex1 = InsertionIndex1.ToBrowsingIndex() as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex1 as IFocusBrowsingExistingBlockNodeIndex, DuplicateExistingIndex1));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex1, NewItemIndex1 as IFocusBrowsingExistingBlockNodeIndex));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount + 1);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == NodeCount + 1);

                IFocusPlaceholderNodeState NewItemState1 = (IFocusPlaceholderNodeState)LeafBlocksInner.BlockStateList[0].StateList[0];
                Assert.That(NewItemState1.Node == NewItem1);
                Assert.That(NewItemState1.ParentIndex == NewItemIndex1);
                Assert.That(NewItemState1.ParentState == RootState);

                FocusNodeStateReadOnlyList AllChildren2 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count + 1, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));




                Leaf NewItem2 = CreateLeaf(Guid.NewGuid());
                BaseNode.Pattern NewPattern = BaseNodeHelper.NodeHelper.CreateSimplePattern("");
                BaseNode.Identifier NewSource = BaseNodeHelper.NodeHelper.CreateSimpleIdentifier("");

                IFocusInsertionNewBlockNodeIndex InsertionIndex2 = new FocusInsertionNewBlockNodeIndex(RootNode, nameof(Main.LeafBlocks), NewItem2, 0, NewPattern, NewSource);
                Assert.That(CompareEqual.CoverIsEqual(InsertionIndex2, InsertionIndex2));

                int BlockCount = LeafBlocksInner.BlockStateList.Count;
                Assert.That(BlockCount == 3);

                Controller.Insert(LeafBlocksInner, InsertionIndex2, out IWriteableBrowsingCollectionNodeIndex NewItemIndex2);
                Assert.That(Controller.Contains(NewItemIndex2));

                IFocusBrowsingExistingBlockNodeIndex DuplicateExistingIndex2 = InsertionIndex2.ToBrowsingIndex() as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex2 as IFocusBrowsingExistingBlockNodeIndex, DuplicateExistingIndex2));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex2, NewItemIndex2 as IFocusBrowsingExistingBlockNodeIndex));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount + 2);
                Assert.That(LeafBlocksInner.BlockStateList.Count == BlockCount + 1);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == 1, $"Count: {LeafBlocksInner.BlockStateList[0].StateList.Count}");
                Assert.That(LeafBlocksInner.BlockStateList[1].StateList.Count == 2, $"Count: {LeafBlocksInner.BlockStateList[1].StateList.Count}");
                Assert.That(LeafBlocksInner.BlockStateList[2].StateList.Count == 2, $"Count: {LeafBlocksInner.BlockStateList[2].StateList.Count}");

                IFocusPlaceholderNodeState NewItemState2 = (IFocusPlaceholderNodeState)LeafBlocksInner.BlockStateList[0].StateList[0];
                Assert.That(NewItemState2.Node == NewItem2);
                Assert.That(NewItemState2.ParentIndex == NewItemIndex2);

                FocusNodeStateReadOnlyList AllChildren3 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FocusRemove()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFocusListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFocusListInner;
                Assert.That(LeafPathInner != null);

                IFocusBrowsingListNodeIndex RemovedLeafIndex0 = LeafPathInner.StateList[0].ParentIndex as IFocusBrowsingListNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex0));

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                FocusNodeStateReadOnlyList AllChildren0 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Assert.That(Controller.IsRemoveable(LeafPathInner, RemovedLeafIndex0));

                Controller.Remove(LeafPathInner, RemovedLeafIndex0);
                Assert.That(!Controller.Contains(RemovedLeafIndex0));

                Assert.That(LeafPathInner.Count == PathCount - 1);
                Assert.That(LeafPathInner.StateList.Count == PathCount - 1);

                FocusNodeStateReadOnlyList AllChildren1 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count - 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                RemovedLeafIndex0 = LeafPathInner.StateList[0].ParentIndex as IFocusBrowsingListNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex0));

                Assert.That(LeafPathInner.Count == 1);

                Assert.That(Controller.IsRemoveable(LeafPathInner, RemovedLeafIndex0));

                IDictionary<Type, string[]> NeverEmptyCollectionTable = BaseNodeHelper.NodeHelper.NeverEmptyCollectionTable as IDictionary<Type, string[]>;
                NeverEmptyCollectionTable.Add(typeof(Main), new string[] { nameof(Main.LeafPath) });
                Assert.That(!Controller.IsRemoveable(LeafPathInner, RemovedLeafIndex0));



                IFocusBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFocusBrowsingExistingBlockNodeIndex RemovedLeafIndex1 = LeafBlocksInner.BlockStateList[1].StateList[0].ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex1));

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[1].StateList.Count;
                Assert.That(BlockNodeCount == 4, $"New count: {BlockNodeCount}");

                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex1));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex1);
                Assert.That(!Controller.Contains(RemovedLeafIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount - 1);
                Assert.That(LeafBlocksInner.BlockStateList[1].StateList.Count == NodeCount - 1);

                FocusNodeStateReadOnlyList AllChildren2 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count - 1, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                IFocusBrowsingExistingBlockNodeIndex RemovedLeafIndex2 = LeafBlocksInner.BlockStateList[1].StateList[0].ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex2));


                int BlockCount = LeafBlocksInner.BlockStateList.Count;
                Assert.That(BlockCount == 3);

                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex2));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex2);
                Assert.That(!Controller.Contains(RemovedLeafIndex2));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount - 2);
                Assert.That(LeafBlocksInner.BlockStateList.Count == BlockCount - 1);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == 1, $"Count: {LeafBlocksInner.BlockStateList[0].StateList.Count}");

                FocusNodeStateReadOnlyList AllChildren3 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count - 3, $"New count: {AllChildren3.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));


                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();


                NeverEmptyCollectionTable.Remove(typeof(Main));
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
        public class FocusFixtureRequiringSTA
        {
            [Test]
            [Category("Coverage")]
            public static void FocusViews()
            {
                ControllerTools.ResetExpectedName();

                Main RootNode;
                IFocusRootNodeIndex RootIndex;
                bool IsMoved;
                bool IsCaretMoved;

                RootNode = CreateRoot(ValueGuid0, Imperfections.None);
                RootIndex = new FocusRootNodeIndex(RootNode);

                FocusController Controller = FocusController.Create(RootIndex);
                IFocusTemplateSet DefaultTemplateSet = FocusTemplateSet.Default;
                DefaultTemplateSet = FocusTemplateSet.Default;

                IFocusTemplateSet FocusCustomTemplateSet = TestDebug.CoverageFocusTemplateSet.FocusTemplateSet;
                FocusFrameSelectorList FrameSelectorList = null;
                foreach (KeyValuePair<Type, IFocusTemplate> TemplateEntry in (ICollection<KeyValuePair<Type, IFocusTemplate>>)FocusCustomTemplateSet.NodeTemplateTable)
                    if (TemplateEntry.Key == typeof(Root))
                    {
                        IFocusNodeTemplate Template = TemplateEntry.Value as IFocusNodeTemplate;
                        Assert.That(Template != null);

                        IFocusHorizontalPanelFrame RootFrame = Template.Root as IFocusHorizontalPanelFrame;
                        foreach (IFocusFrame Frame in RootFrame.Items)
                            if (Frame is IFocusFrameWithSelector AsFrameWithSelector && AsFrameWithSelector.Selectors.Count > 0)
                            {
                                if (AsFrameWithSelector.Selectors[0].SelectorType == typeof(Leaf))
                                {
                                    FrameSelectorList = AsFrameWithSelector.Selectors;
                                    break;
                                }
                            }
                    }

                List<FocusFrameSelectorList> SelectorStack;

                Assert.That(FrameSelectorList != null);
                Assert.That(FrameSelectorList.Count > 0);
                foreach (KeyValuePair<Type, IFocusTemplate> TemplateEntry in (ICollection<KeyValuePair<Type, IFocusTemplate>>)FocusCustomTemplateSet.NodeTemplateTable)
                    if (TemplateEntry.Key == typeof(Leaf))
                    {
                        IFocusNodeTemplate Template = TemplateEntry.Value as IFocusNodeTemplate;
                        Assert.That(Template != null);

                        SelectorStack = new List<FocusFrameSelectorList>();
                        SelectorStack.Add(FrameSelectorList);

                        Template.PropertyToFrame("Text", SelectorStack);
                        Template.GetCommentFrame(SelectorStack);
                        break;
                    }
                Assert.That(CompareEqual.CoverIsEqual(FrameSelectorList, FrameSelectorList));

                SelectorStack = new List<FocusFrameSelectorList>();
                FocusCustomTemplateSet.PropertyToFrame(Controller.RootState, "PlaceholderTree", SelectorStack);
                FocusCustomTemplateSet.GetCommentFrame(Controller.RootState, SelectorStack);

                System.Windows.IDataObject DataObject = new System.Windows.DataObject();

                using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, FocusCustomTemplateSet))
                {
                    Assert.That(ControllerView0.Controller == Controller);
                    Assert.That(ControllerView0.RootStateView == ControllerView0.StateViewTable[Controller.RootState]);
                    Assert.That(ControllerView0.TemplateSet == TestDebug.CoverageFocusTemplateSet.FocusTemplateSet);
                    Assert.That(ControllerView0.CaretMode == CaretModes.Insertion);
                    Assert.That(ControllerView0.AutoFormatMode == AutoFormatModes.None);

                    Assert.That(ControllerView0.IsSelectionEmpty);

                    bool IsChanged;
                    ControllerView0.SetCaretMode(CaretModes.Override, out IsChanged);
                    Assert.That(ControllerView0.CaretMode == CaretModes.Override);
                    Assert.That(IsChanged);

                    ControllerView0.SetCaretMode(CaretModes.Insertion, out IsChanged);
                    Assert.That(ControllerView0.CaretMode == CaretModes.Insertion);
                    Assert.That(IsChanged);

                    ControllerView0.SetCaretMode(CaretModes.Override, out IsChanged);
                    Assert.That(ControllerView0.CaretMode == CaretModes.Override);
                    Assert.That(IsChanged);

                    ControllerView0.SetCaretPosition(1000, true, out IsMoved);
                    Assert.That(IsMoved);
                    ControllerView0.SetCaretPosition(0, true, out IsMoved);
                    Assert.That(IsMoved);
                    Assert.That(ControllerView0.CaretPosition == ControllerView0.CaretAnchorPosition);

                    ControllerView0.SetAutoFormatMode(AutoFormatModes.FirstOnly);
                    Assert.That(ControllerView0.AutoFormatMode == AutoFormatModes.FirstOnly);

                    using (FocusControllerView ControllerView1 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
                    {
                        Assert.That(ControllerView0.IsEqual(CompareEqual.New(), ControllerView0));
                        Assert.That(CompareEqual.CoverIsEqual(ControllerView0, ControllerView1));
                    }

                    foreach (KeyValuePair<IFocusBlockState, FocusBlockStateView> Entry in (ICollection<KeyValuePair<IFocusBlockState, FocusBlockStateView>>)ControllerView0.BlockStateViewTable)
                    {
                        IFocusBlockState BlockState = Entry.Key;
                        Assert.That(BlockState != null);

                        FocusBlockStateView BlockStateView = Entry.Value;
                        Assert.That(BlockStateView != null);
                        Assert.That(BlockStateView.BlockState == BlockState);

                        Assert.That(BlockStateView.ControllerView == ControllerView0);
                    }

                    foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in (ICollection<KeyValuePair<IFocusNodeState, IFocusNodeStateView>>)ControllerView0.StateViewTable)
                    {
                        IFocusNodeState State = Entry.Key;
                        Assert.That(State != null);

                        IFocusNodeStateView StateView = Entry.Value;
                        Assert.That(StateView != null);
                        Assert.That(StateView.State == State);

                        IFocusIndex ParentIndex = State.ParentIndex;
                        Assert.That(ParentIndex != null);

                        Assert.That(Controller.Contains(ParentIndex));
                        Assert.That(StateView.ControllerView == ControllerView0);

                        switch (StateView)
                        {
                            case IFocusPatternStateView AsPatternStateView:
                                Assert.That(AsPatternStateView.State == State);
                                Assert.That(AsPatternStateView is IFocusNodeStateView AsPlaceholderPatternNodeStateView && AsPlaceholderPatternNodeStateView.State == State);
                                break;

                            case IFocusSourceStateView AsSourceStateView:
                                Assert.That(AsSourceStateView.State == State);
                                Assert.That(AsSourceStateView is IFocusNodeStateView AsPlaceholderSourceNodeStateView && AsPlaceholderSourceNodeStateView.State == State);
                                break;

                            case IFocusPlaceholderNodeStateView AsPlaceholderNodeStateView:
                                Assert.That(AsPlaceholderNodeStateView.State == State);
                                break;

                            case IFocusOptionalNodeStateView AsOptionalNodeStateView:
                                Assert.That(AsOptionalNodeStateView.State == State);
                                break;
                        }
                    }

                    FocusVisibleCellViewList VisibleCellViewList = new FocusVisibleCellViewList();
                    ControllerView0.EnumerateVisibleCellViews((IFrameVisibleCellView item) => ListCellViews(item, VisibleCellViewList), out IFrameVisibleCellView FoundCellView, false);
                    ControllerView0.PrintCellViewTree(true);

                    Assert.That(ControllerView0.MinFocusMove == 0);

                    ControllerView0.SetCommentDisplayMode(CommentDisplayModes.All);
                    VisibleCellViewList.Clear();
                    ControllerView0.EnumerateVisibleCellViews((IFrameVisibleCellView item) => ListCellViews(item, VisibleCellViewList), out FoundCellView, false);
                    ControllerView0.PrintCellViewTree(true);

                    foreach (IFocusVisibleCellView CellView in VisibleCellViewList)
                        Assert.That(CompareEqual.CoverIsEqual(CellView, CellView));

                    Assert.That(ControllerView0.MinFocusMove == -2);
                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    Assert.That(IsMoved);
                    Assert.That(ControllerView0.MinFocusMove == 0);

                    ControllerView0.MoveFocus(-1, true, out IsMoved);
                    Assert.That(!IsMoved);
                    Assert.That(ControllerView0.MinFocusMove == 0);

                    Assert.That(ControllerView0.MaxFocusMove > 0);
                    Assert.That(ControllerView0.FocusedText != null);
                    //System.Diagnostics.Debug.Assert(false);
                    ControllerView0.SetCaretPosition(0, true, out IsMoved);
                    Assert.That(IsMoved);

                    ControllerView0.SelectStringContent(ControllerView0.Focus.CellView.StateView.State, "ValueString", 0, 1);
                    ControllerView0.SelectStringContent(ControllerView0.Focus.CellView.StateView.State, "ValueString", 1, 0);

                    //System.Diagnostics.Debug.Assert(false);
                    ControllerView0.CopySelection(DataObject);
                    ControllerView0.CutSelection(DataObject, out bool IsDeleted);
                    Assert.That(IsDeleted);

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

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();


                    ControllerView0.SelectDiscreteContent(ControllerView0.Focus.CellView.StateView.State, "ValueEnum");

                    //System.Diagnostics.Debug.Assert(false);
                    ControllerView0.CopySelection(DataObject);
                    ControllerView0.CutSelection(DataObject, out IsDeleted);
                    Assert.That(!IsDeleted);

                    System.Windows.Clipboard.SetDataObject(DataObject);

                    ControllerView0.SelectDiscreteContent(ControllerView0.Focus.CellView.StateView.State, "ValueEnum");
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(!IsChanged);


                    ControllerView0.SelectComment(ControllerView0.Focus.CellView.StateView.State, 0, 1);
                    ControllerView0.SelectComment(ControllerView0.Focus.CellView.StateView.State, 1, 0);

                    ControllerView0.CopySelection(DataObject);
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


                    ControllerView0.MoveFocus(+1, true, out IsMoved);
                    Assert.That(ControllerView0.FocusedText != null);
                    Assert.That(!ControllerView0.IsUserVisible);

                    ControllerView0.MoveFocus(+1, true, out IsMoved);
                    Assert.That(ControllerView0.FocusedText != null);
                    Assert.That(!ControllerView0.IsUserVisible);

                    ControllerView0.SetCaretPosition(0, true, out IsMoved);
                    Assert.That(!IsMoved);

                    while (ControllerView0.MaxFocusMove > 0)
                        ControllerView0.MoveFocus(+1, true, out IsMoved);

                    Assert.That(ControllerView0.MaxFocusMove == 0);
                    ControllerView0.MoveFocus(+1, true, out IsMoved);
                    Assert.That(ControllerView0.MaxFocusMove == 0);

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

                    IFocusBlockListInner MainLeafBlocksInner = Controller.RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
                    while (!MainLeafBlocksInner.IsEmpty)
                    {
                        IWriteableBrowsingExistingBlockNodeIndex NodeIndex = MainLeafBlocksInner.IndexAt(0, 0) as IWriteableBrowsingExistingBlockNodeIndex;
                        Controller.Remove(MainLeafBlocksInner, NodeIndex);
                    }

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
                        if (ControllerView0.Focus is IFocusDiscreteContentFocus AsDiscreteContentFocus)
                        {
                            ControllerView0.SetUserVisible(true);
                            ControllerView0.SetUserVisible(false);
                            break;
                        }

                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                        Assert.That(IsMoved);
                    }

                    Assert.That(ControllerView0.MaxFocusMove > 0);

                    //System.Diagnostics.Debug.Assert(false);
                    BaseNodeHelper.NodeTreeHelper.SetCommentText(RootNode.Documentation, "");

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);

                    MaxFocusMove = ControllerView0.MaxFocusMove;

                    for (int i = 0; i < MaxFocusMove && ControllerView0.Focus is IFocusCommentFocus; i++)
                    {
                        IFocusTextFocus AsTextFocus = ControllerView0.Focus as IFocusCommentFocus;
                        Assert.That(AsTextFocus != null);
                        Assert.That(AsTextFocus.CellView != null);

                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }

                    ControllerView0.SetCaretMode(CaretModes.Override, out IsChanged);
                    ControllerView0.ForceShowComment(out IsMoved);
                    ControllerView0.SetCaretPosition(0, true, out IsCaretMoved);

                    BaseNodeHelper.NodeTreeHelper.SetCommentText(RootNode.Documentation, "test");

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    MaxFocusMove = ControllerView0.MaxFocusMove;

                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        ControllerView0.ForceShowComment(out IsMoved);

                        Assert.That(!IsMoved || ControllerView0.CaretPosition >= 0);
                        if (ControllerView0.CaretPosition >= 0)
                        {
                            for (int j = 0; j <= ControllerView0.MaxCaretPosition; j++)
                                ControllerView0.SetCaretPosition(j, true, out IsCaretMoved);
                        }

                        ControllerView0.SetCaretMode(CaretModes.Insertion, out IsChanged);
                        ControllerView0.SetCaretMode(CaretModes.Override, out IsChanged);
                        ControllerView0.SetCaretPosition(0, true, out IsCaretMoved);
                        ControllerView0.SetCaretPosition(ControllerView0.MaxCaretPosition, true, out IsCaretMoved);

                        if (IsMoved)
                            ControllerView0.MoveFocus(+1, true, out IsMoved);

                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }

                    Assert.That(ControllerView0.MaxFocusMove == 0);

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, false, out IsMoved);
                    Assert.That(!ControllerView0.IsSelectionEmpty);

                    MaxFocusMove = ControllerView0.MaxFocusMove;
                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        ControllerView0.ForceShowComment(out IsMoved);

                        ControllerView0.SetCaretPosition(0, true, out IsCaretMoved);
                        ControllerView0.SetCaretPosition(2, false, out IsCaretMoved);
                        ControllerView0.SetCaretPosition(3, false, out IsCaretMoved);
                        if (ControllerView0.Selection is IFocusCommentSelection AsCommentSelection && AsCommentSelection.Start != AsCommentSelection.End)
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
                            Assert.That(AsStringContentSelection.PropertyName != null);
                            AsStringContentSelection.Update(AsStringContentSelection.Start, AsStringContentSelection.End);
                            AsStringContentSelection.Update(AsStringContentSelection.End, AsStringContentSelection.Start);
                            AsStringContentSelection.Copy(DataObject);
                            break;
                        }

                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }
                }
            }

            [Test]
            [Category("Coverage")]
            public static void FocusRemoveBlockRange()
            {
                ControllerTools.ResetExpectedName();

                Main RootNode;
                IFocusRootNodeIndex RootIndex;

                RootNode = CreateRoot(ValueGuid0, Imperfections.None);
                RootIndex = new FocusRootNodeIndex(RootNode);

                FocusController ControllerBase = FocusController.Create(RootIndex);
                FocusController Controller = FocusController.Create(RootIndex);

                using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
                {
                    Assert.That(ControllerView0.Controller == Controller);

                    IFocusNodeState RootState = Controller.RootState;
                    Assert.That(RootState != null);

                    FocusNodeStateReadOnlyList AllChildren0 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                    Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                    IFocusBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
                    Assert.That(LeafBlocksInner != null);
                    Assert.That(LeafBlocksInner.BlockStateList.Count == 3, $"New count: {LeafBlocksInner.BlockStateList.Count}");
                    Assert.That(Controller.IsBlockRangeRemoveable(LeafBlocksInner, 0, 2));

                    Controller.RemoveBlockRange(LeafBlocksInner, 0, 2);
                    Assert.That(LeafBlocksInner.Count == 1);

                    FocusNodeStateReadOnlyList AllChildren2 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                    Assert.That(AllChildren2.Count == AllChildren0.Count - 7, $"New count: {AllChildren2.Count}");

                    Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    IDictionary<Type, string[]> NeverEmptyCollectionTable = BaseNodeHelper.NodeHelper.NeverEmptyCollectionTable as IDictionary<Type, string[]>;
                    NeverEmptyCollectionTable.Add(typeof(Main), new string[] { nameof(Main.LeafBlocks) });

                    Assert.That(!Controller.IsBlockRangeRemoveable(LeafBlocksInner, 0, 3));

                    NeverEmptyCollectionTable.Remove(typeof(Main));
                    Assert.That(Controller.IsBlockRangeRemoveable(LeafBlocksInner, 0, 3));

                    Assert.That(!Controller.CanUndo);
                    Assert.That(Controller.CanRedo);

                    Controller.Redo();
                    Controller.Undo();

                    System.Windows.IDataObject DataObject = new System.Windows.DataObject();

                    //System.Diagnostics.Debug.Assert(false);
                    ControllerView0.SelectBlockList(RootState, nameof(Main.LeafBlocks), 1, 2);

                    ControllerView0.CopySelection(DataObject);
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
            public static void FocusRemoveNodeRange()
            {
                ControllerTools.ResetExpectedName();

                Main RootNode;
                IFocusRootNodeIndex RootIndex;

                RootNode = CreateRoot(ValueGuid0, Imperfections.None);
                RootIndex = new FocusRootNodeIndex(RootNode);

                FocusController ControllerBase = FocusController.Create(RootIndex);
                FocusController Controller = FocusController.Create(RootIndex);

                using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
                {
                    Assert.That(ControllerView0.Controller == Controller);

                    IFocusNodeState RootState = Controller.RootState;
                    Assert.That(RootState != null);

                    FocusNodeStateReadOnlyList AllChildren0 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                    Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");


                    //System.Diagnostics.Debug.Assert(false);
                    IFocusListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFocusListInner;
                    Assert.That(LeafPathInner != null);
                    Assert.That(LeafPathInner.StateList.Count == 2, $"New count: {LeafPathInner.StateList.Count}");
                    Assert.That(Controller.IsNodeRangeRemoveable(LeafPathInner, -1, 0, 2));

                    Controller.RemoveNodeRange(LeafPathInner, -1, 0, 2);

                    FocusNodeStateReadOnlyList AllChildren1 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                    Assert.That(AllChildren1.Count == AllChildren0.Count - 2, $"New count: {AllChildren1.Count}");

                    Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    Assert.That(!Controller.CanUndo);
                    Assert.That(Controller.CanRedo);

                    Controller.Redo();
                    Controller.Undo();


                    IFocusBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
                    Assert.That(LeafBlocksInner != null);
                    Assert.That(LeafBlocksInner.BlockStateList.Count == 3, $"New count: {LeafBlocksInner.BlockStateList.Count}");
                    Assert.That(Controller.IsNodeRangeRemoveable(LeafBlocksInner, 1, 0, 2));

                    Controller.RemoveNodeRange(LeafBlocksInner, 1, 0, 2);

                    FocusNodeStateReadOnlyList AllChildren2 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                    Assert.That(AllChildren2.Count == AllChildren1.Count - 2, $"New count: {AllChildren2.Count}");

                    Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    Assert.That(!Controller.CanUndo);
                    Assert.That(Controller.CanRedo);

                    Controller.Redo();
                    Controller.Undo();

                    IDictionary<Type, string[]> NeverEmptyCollectionTable = BaseNodeHelper.NodeHelper.NeverEmptyCollectionTable as IDictionary<Type, string[]>;
                    NeverEmptyCollectionTable.Add(typeof(Main), new string[] { nameof(Main.LeafBlocks) });

                    Controller.RemoveBlockRange(LeafBlocksInner, 2, 3);
                    Controller.RemoveBlockRange(LeafBlocksInner, 0, 1);
                    Assert.That(!Controller.IsNodeRangeRemoveable(LeafBlocksInner, 0, 0, 2));

                    NeverEmptyCollectionTable.Remove(typeof(Main));
                    Assert.That(Controller.IsNodeRangeRemoveable(LeafBlocksInner, 0, 0, 2));

                    Controller.Undo();
                    Controller.Undo();

                    System.Windows.IDataObject DataObject = new System.Windows.DataObject();

                    ControllerView0.SelectNode((RootState.InnerTable[nameof(Main.PlaceholderLeaf)] as IFocusPlaceholderInner).ChildState);

                    ControllerView0.CopySelection(DataObject);
                    ControllerView0.CutSelection(DataObject, out bool IsDeleted);
                    Assert.That(!IsDeleted);

                    System.Windows.Clipboard.SetDataObject(DataObject);

                    //System.Diagnostics.Debug.Assert(false);
                    ControllerView0.SelectNode((RootState.InnerTable[nameof(Main.PlaceholderLeaf)] as IFocusPlaceholderInner).ChildState);
                    ControllerView0.PasteSelection(out bool IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.ClearSelection();
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.SelectNodeList(RootState, nameof(Main.LeafPath), 0, 2);
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();

                    ControllerView0.SelectBlockNodeList(RootState, nameof(Main.LeafBlocks), 1, 0, 0);
                    ControllerView0.CopySelection(DataObject);

                    ControllerView0.SelectBlockNodeList(RootState, nameof(Main.LeafBlocks), 1, 0, 2);
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();


                    ControllerView0.SelectNodeList(RootState, nameof(Main.LeafPath), 0, 2);

                    ControllerView0.CopySelection(DataObject);
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
                    System.Windows.Clipboard.SetDataObject(DataObject);

                    //System.Diagnostics.Debug.Assert(false);
                    ControllerView0.SelectNodeList(RootState, nameof(Main.LeafPath), 0, 2);
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(IsChanged);

                    Assert.That(Controller.CanUndo);
                    Controller.Undo();


                    ControllerView0.SelectBlockNodeList(RootState, nameof(Main.LeafBlocks), 1, 0, 1);

                    ControllerView0.CopySelection(DataObject);
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
                    DataObject.SetData(ClipboardHelper.ClipboardFormatNode, new byte[0]);
                    System.Windows.Clipboard.SetDataObject(DataObject);

                    ControllerView0.SelectBlockNodeList(RootState, nameof(Main.LeafBlocks), 1, 0, 1);
                    ControllerView0.PasteSelection(out IsChanged);
                    Assert.That(!IsChanged);

                    Assert.That(!Controller.CanUndo);

                    Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
                }
            }

            [Test]
            [Category("Coverage")]
            public static void FocusSelection()
            {
                ControllerTools.ResetExpectedName();

                BaseNode.Class RootNode;
                IFocusRootNodeIndex RootIndex;
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
                BaseNode.CommandInstruction PropertySecondInstruction = BaseNodeHelper.NodeHelper.CreateSimpleCommandInstruction("test?") as BaseNode.CommandInstruction;
                BaseNode.EffectiveBody PropertyBody = ((BaseNode.PropertyFeature)PropertyFeature).GetterBody.Item as BaseNode.EffectiveBody;
                PropertyBody.BodyInstructionBlocks = BaseNodeHelper.BlockListHelper.CreateSimpleBlockList<BaseNode.Instruction>(PropertyFirstInstruction);
                PropertyBody.BodyInstructionBlocks.NodeBlockList[0].NodeList.Add(PropertySecondInstruction);

                BaseNode.CommandInstruction PropertyThirdInstruction = BaseNodeHelper.NodeHelper.CreateSimpleCommandInstruction("test?") as BaseNode.CommandInstruction;
                List<BaseNode.Instruction> NodeList = new List<BaseNode.Instruction>();
                NodeList.Add(PropertyThirdInstruction);
                BaseNode.IBlock<BaseNode.Instruction> NewBlock = BaseNodeHelper.BlockListHelper.CreateBlock<BaseNode.Instruction>(NodeList);
                PropertyBody.BodyInstructionBlocks.NodeBlockList.Add(NewBlock);

                RootIndex = new FocusRootNodeIndex(RootNode);

                FocusController ControllerBase = FocusController.Create(RootIndex);
                FocusController Controller = FocusController.Create(RootIndex);

                System.Windows.IDataObject DataObject = new System.Windows.DataObject();

                using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
                {
                    int MaxFocusMove;

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    MaxFocusMove = ControllerView0.MaxFocusMove;

                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        if (ControllerView0.Focus.CellView.StateView.State.ParentIndex is IFocusBrowsingListNodeIndex AsBlockNodeIndex)
                        {
                            int j;
                            for (j = 0; j < 20; j++)
                            {
                                ControllerView0.MoveFocus(+1, false, out IsMoved);
                                if (ControllerView0.Selection is IFocusNodeListSelection AsNodeListSelection)
                                {
                                    Assert.That(AsNodeListSelection.PropertyName != null);
                                    AsNodeListSelection.Update(AsNodeListSelection.StartIndex, AsNodeListSelection.EndIndex);
                                    AsNodeListSelection.Update(AsNodeListSelection.EndIndex, AsNodeListSelection.StartIndex);
                                    AsNodeListSelection.Copy(DataObject);
                                    break;
                                }
                            }
                            if (j >= 10)
                                ControllerView0.MoveFocus(-5, true, out IsMoved);
                        }
                        else
                            ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    MaxFocusMove = ControllerView0.MaxFocusMove;

                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        Controller.IsChildState(Controller.RootState, ControllerView0.Focus.CellView.StateView.State, out IReadOnlyIndex Index);

                        if (Index is IFocusBrowsingExistingBlockNodeIndex AsBlockNodeIndex)
                        {
                            int j;
                            for (j = 0; j < 20; j++)
                            {
                                ControllerView0.MoveFocus(+1, false, out IsMoved);
                                if (ControllerView0.Selection is IFocusBlockNodeListSelection AsBlockNodeListSelection)
                                {
                                    Assert.That(AsBlockNodeListSelection.PropertyName != null);
                                    Assert.That(AsBlockNodeListSelection.BlockIndex >= 0);
                                    AsBlockNodeListSelection.Update(AsBlockNodeListSelection.StartIndex, AsBlockNodeListSelection.EndIndex);
                                    AsBlockNodeListSelection.Update(AsBlockNodeListSelection.EndIndex, AsBlockNodeListSelection.StartIndex);
                                    AsBlockNodeListSelection.Copy(DataObject);
                                    break;
                                }
                            }
                            if (j >= 10)
                                ControllerView0.MoveFocus(-5, true, out IsMoved);
                        }
                        else
                            ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    MaxFocusMove = ControllerView0.MaxFocusMove;

                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        string s = ControllerView0.FocusedText;
                        if (s == "test?")
                            break;

                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }

                    //System.Diagnostics.Debug.Assert(false);

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    MaxFocusMove = ControllerView0.MaxFocusMove;

                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        ControllerView0.MoveFocus(+1, false, out IsMoved);
                        if (ControllerView0.Focus is IFocusStringContentFocus AsStringContentFocus && ControllerView0.FocusedText == "test1")
                        {
                            ControllerView0.ClearSelection();
                            ControllerView0.PasteSelection(out bool IsChanged);
                            break;
                        }
                    }

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    MaxFocusMove = ControllerView0.MaxFocusMove;

                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        string s = ControllerView0.FocusedText;
                        if (s == "test?")
                            break;

                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }

                    MaxFocusMove = ControllerView0.MaxFocusMove;

                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        ControllerView0.MoveFocus(+1, false, out IsMoved);
                        if (ControllerView0.Selection is IFocusBlockListSelection AsBlockListSelection)
                        {
                            Assert.That(AsBlockListSelection.PropertyName != null);
                            AsBlockListSelection.Update(AsBlockListSelection.StartIndex, AsBlockListSelection.EndIndex);
                            AsBlockListSelection.Update(AsBlockListSelection.EndIndex, AsBlockListSelection.StartIndex);
                            AsBlockListSelection.Copy(DataObject);
                            System.Windows.Clipboard.SetDataObject(DataObject);
                            break;
                        }
                    }

                    ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                    MaxFocusMove = ControllerView0.MaxFocusMove;

                    for (int i = 0; i < MaxFocusMove; i++)
                    {
                        ControllerView0.MoveFocus(+1, false, out IsMoved);
                        if (ControllerView0.Focus is IFocusStringContentFocus AsStringContentFocus && ControllerView0.FocusedText == "test1")
                        {
                            ControllerView0.ClearSelection();
                            ControllerView0.PasteSelection(out bool IsChanged);
                            break;
                        }
                    }
                }
            }
        }

        [Test]
        [Category("Coverage")]
        public static void FocusMove()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFocusListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFocusListInner;
                Assert.That(LeafPathInner != null);

                IFocusBrowsingListNodeIndex MovedLeafIndex0 = LeafPathInner.IndexAt(0) as IFocusBrowsingListNodeIndex;
                Assert.That(Controller.Contains(MovedLeafIndex0));

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                FocusNodeStateReadOnlyList AllChildren0 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Assert.That(Controller.IsMoveable(LeafPathInner, MovedLeafIndex0, +1));

                Controller.Move(LeafPathInner, MovedLeafIndex0, +1);
                Assert.That(Controller.Contains(MovedLeafIndex0));

                Assert.That(LeafPathInner.Count == PathCount);
                Assert.That(LeafPathInner.StateList.Count == PathCount);

                //System.Diagnostics.Debug.Assert(false);
                IFocusBrowsingListNodeIndex NewLeafIndex0 = LeafPathInner.IndexAt(1) as IFocusBrowsingListNodeIndex;
                Assert.That(NewLeafIndex0 == MovedLeafIndex0);

                FocusNodeStateReadOnlyList AllChildren1 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));




                IFocusBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFocusBrowsingExistingBlockNodeIndex MovedLeafIndex1 = LeafBlocksInner.IndexAt(1, 1) as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(MovedLeafIndex1));

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[1].StateList.Count;
                Assert.That(BlockNodeCount == 4, $"New count: {BlockNodeCount}");

                Assert.That(Controller.IsMoveable(LeafBlocksInner, MovedLeafIndex1, -1));
                Controller.Move(LeafBlocksInner, MovedLeafIndex1, -1);
                Assert.That(Controller.Contains(MovedLeafIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount);
                Assert.That(LeafBlocksInner.BlockStateList[1].StateList.Count == NodeCount);

                IFocusBrowsingExistingBlockNodeIndex NewLeafIndex1 = LeafBlocksInner.IndexAt(1, 0) as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(NewLeafIndex1 == MovedLeafIndex1);

                FocusNodeStateReadOnlyList AllChildren2 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FocusMoveBlock()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                FocusNodeStateReadOnlyList AllChildren1 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == 19, $"New count: {AllChildren1.Count}");

                IFocusBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFocusBrowsingExistingBlockNodeIndex MovedLeafIndex1 = LeafBlocksInner.IndexAt(1, 0) as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(MovedLeafIndex1));

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[1].StateList.Count;
                Assert.That(BlockNodeCount == 4, $"New count: {BlockNodeCount}");

                Assert.That(Controller.IsBlockMoveable(LeafBlocksInner, 1, -1));
                Controller.MoveBlock(LeafBlocksInner, 1, -1);
                Assert.That(Controller.Contains(MovedLeafIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == NodeCount);

                IFocusBrowsingExistingBlockNodeIndex NewLeafIndex1 = LeafBlocksInner.IndexAt(0, 0) as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(NewLeafIndex1 == MovedLeafIndex1);

                FocusNodeStateReadOnlyList AllChildren2 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FocusChangeDiscreteValue()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetEnumValue(RootState.Node, nameof(Main.ValueEnum)) == (int)BaseNode.CopySemantic.Value);

                Controller.ChangeDiscreteValue(RootIndex, nameof(Main.ValueEnum), (int)BaseNode.CopySemantic.Reference);

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetEnumValue(RootNode, nameof(Main.ValueEnum)) == (int)BaseNode.CopySemantic.Reference);

                IFocusPlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IFocusPlaceholderInner;
                IFocusPlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as IFocusPlaceholderNodeState;

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
        public static void FocusChangeText()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetString(RootState.Node, nameof(Main.ValueString)) == "s");

                Controller.ChangeText(RootIndex, nameof(Main.ValueString), "test");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetString(RootNode, nameof(Main.ValueString)) == "test");

                IFocusPlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IFocusPlaceholderInner;
                IFocusPlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as IFocusPlaceholderNodeState;

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
        public static void FocusChangeTextAndCaretPosition()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetString(RootState.Node, nameof(Main.ValueString)) == "s");

                Controller.ChangeTextAndCaretPosition(RootIndex, nameof(Main.ValueString), "test", 1, 2, false);
                Controller.ChangeTextAndCaretPosition(RootIndex, nameof(Main.ValueString), "test", 2, 1, true);

                while (!(ControllerView0.Focus is IFocusStringContentFocus))
                    ControllerView0.MoveFocus(+1, true, out bool IsMoved);

                //System.Diagnostics.Debug.Assert(false);
                ControllerView0.SetAutoFormatMode(AutoFormatModes.FirstOnly);
                ControllerView0.ChangeFocusedText("test test", 3, false);
                Assert.That(ControllerView0.FocusedText == "Test Test");
                ControllerView0.SetAutoFormatMode(AutoFormatModes.FirstOrAll);
                ControllerView0.ChangeFocusedText("test TEST", 3, false);
                Assert.That(ControllerView0.FocusedText == "Test TEST");
                ControllerView0.SetAutoFormatMode(AutoFormatModes.AllLowercase);
                ControllerView0.ChangeFocusedText("test TEST", 3, false);
                Assert.That(ControllerView0.FocusedText == "test test");
                ControllerView0.SetAutoFormatMode(AutoFormatModes.None);
                ControllerView0.ChangeFocusedText("test TEST", 3, false);

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(ControllerView0.FocusedText == "test TEST");

                IFocusPlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IFocusPlaceholderInner;
                IFocusPlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as IFocusPlaceholderNodeState;

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

                //System.Diagnostics.Debug.Assert(false);
                IFocusOptionalInner AssignedOptionalLeafInner = Controller.RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IFocusOptionalInner;
                Assert.That(AssignedOptionalLeafInner.IsAssigned);
                IFocusBrowsingOptionalNodeIndex AssignedOptionalLeafIndex = AssignedOptionalLeafInner.ChildState.ParentIndex;
                Controller.ChangeTextAndCaretPosition(AssignedOptionalLeafIndex, nameof(Leaf.Text), "test", 1, 2, false);

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void FocusChangeComment()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetCommentText(RootState.Node) == "main doc");

                //System.Diagnostics.Debug.Assert(false);
                Controller.ChangeComment(RootIndex, "");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetCommentText(RootNode) == "");

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Controller.ChangeComment(RootIndex, "test");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetCommentText(RootNode) == "test");

                IFocusPlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IFocusPlaceholderInner;
                IFocusPlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as IFocusPlaceholderNodeState;

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
        public static void FocusChangeCommentAndCaretPosition()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetCommentText(RootState.Node) == "main doc");

                Controller.ChangeCommentAndCaretPosition(RootIndex, "test", 1, 2, false);
                Controller.ChangeCommentAndCaretPosition(RootIndex, "test", 2, 1, true);

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetCommentText(RootNode) == "test");

                ControllerView0.SetCommentDisplayMode(CommentDisplayModes.All);

                while (!(ControllerView0.Focus is IFocusCommentFocus))
                    ControllerView0.MoveFocus(+1, true, out bool IsMoved);

                ControllerView0.ChangeFocusedText("test", 3, false);

                IFocusPlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IFocusPlaceholderInner;
                IFocusPlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as IFocusPlaceholderNodeState;

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
        public static void FocusReplace()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Leaf NewItem0 = CreateLeaf(Guid.NewGuid());
                IFocusInsertionListNodeIndex ReplacementIndex0 = new FocusInsertionListNodeIndex(RootNode, nameof(Main.LeafPath), NewItem0, 0);

                IFocusListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFocusListInner;
                Assert.That(LeafPathInner != null);

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                FocusNodeStateReadOnlyList AllChildren0 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Replace(LeafPathInner, ReplacementIndex0, out IWriteableBrowsingChildIndex NewItemIndex0);
                Assert.That(Controller.Contains(NewItemIndex0));

                Assert.That(LeafPathInner.Count == PathCount);
                Assert.That(LeafPathInner.StateList.Count == PathCount);

                IFocusPlaceholderNodeState NewItemState0 = (IFocusPlaceholderNodeState)LeafPathInner.StateList[0];
                Assert.That(NewItemState0.Node == NewItem0);
                Assert.That(NewItemState0.ParentIndex == NewItemIndex0);

                FocusNodeStateReadOnlyList AllChildren1 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                Leaf NewItem1 = CreateLeaf(Guid.NewGuid());
                IFocusInsertionExistingBlockNodeIndex ReplacementIndex1 = new FocusInsertionExistingBlockNodeIndex(RootNode, nameof(Main.LeafBlocks), NewItem1, 0, 0);

                IFocusBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFocusBlockState BlockState = (IFocusBlockState)LeafBlocksInner.BlockStateList[0];

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = BlockState.StateList.Count;
                Assert.That(BlockNodeCount == 4);

                Controller.Replace(LeafBlocksInner, ReplacementIndex1, out IWriteableBrowsingChildIndex NewItemIndex1);
                Assert.That(Controller.Contains(NewItemIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount);
                Assert.That(BlockState.StateList.Count == NodeCount);

                IFocusPlaceholderNodeState NewItemState1 = (IFocusPlaceholderNodeState)BlockState.StateList[0];
                Assert.That(NewItemState1.Node == NewItem1);
                Assert.That(NewItemState1.ParentIndex == NewItemIndex1);

                FocusNodeStateReadOnlyList AllChildren2 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                IFocusPlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IFocusPlaceholderInner;
                Assert.That(PlaceholderTreeInner != null);

                IFocusBrowsingPlaceholderNodeIndex ExistingIndex2 = PlaceholderTreeInner.ChildState.ParentIndex as IFocusBrowsingPlaceholderNodeIndex;

                Tree NewItem2 = CreateTree();
                IFocusInsertionPlaceholderNodeIndex ReplacementIndex2;
                ReplacementIndex2 = ExistingIndex2.ToInsertionIndex(RootNode, NewItem2) as IFocusInsertionPlaceholderNodeIndex;

                Controller.Replace(PlaceholderTreeInner, ReplacementIndex2, out IWriteableBrowsingChildIndex NewItemIndex2);
                Assert.That(Controller.Contains(NewItemIndex2));

                IFocusPlaceholderNodeState NewItemState2 = PlaceholderTreeInner.ChildState as IFocusPlaceholderNodeState;
                Assert.That(NewItemState2.Node == NewItem2);
                Assert.That(NewItemState2.ParentIndex == NewItemIndex2);

                IFocusBrowsingPlaceholderNodeIndex DuplicateExistingIndex2 = ReplacementIndex2.ToBrowsingIndex() as IFocusBrowsingPlaceholderNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex2 as IFocusBrowsingPlaceholderNodeIndex, DuplicateExistingIndex2));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex2, NewItemIndex2 as IFocusBrowsingPlaceholderNodeIndex));

                FocusNodeStateReadOnlyList AllChildren3 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count, $"New count: {AllChildren3.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                IFocusPlaceholderInner PlaceholderLeafInner = NewItemState2.PropertyToInner(nameof(Tree.Placeholder)) as IFocusPlaceholderInner;
                Assert.That(PlaceholderLeafInner != null);

                IFocusBrowsingPlaceholderNodeIndex ExistingIndex3 = PlaceholderLeafInner.ChildState.ParentIndex as IFocusBrowsingPlaceholderNodeIndex;

                Leaf NewItem3 = CreateLeaf(Guid.NewGuid());
                IFocusInsertionPlaceholderNodeIndex ReplacementIndex3;
                ReplacementIndex3 = ExistingIndex3.ToInsertionIndex(NewItem2, NewItem3) as IFocusInsertionPlaceholderNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(ReplacementIndex3, ReplacementIndex3));

                Controller.Replace(PlaceholderLeafInner, ReplacementIndex3, out IWriteableBrowsingChildIndex NewItemIndex3);
                Assert.That(Controller.Contains(NewItemIndex3));

                IFocusPlaceholderNodeState NewItemState3 = PlaceholderLeafInner.ChildState as IFocusPlaceholderNodeState;
                Assert.That(NewItemState3.Node == NewItem3);
                Assert.That(NewItemState3.ParentIndex == NewItemIndex3);

                FocusNodeStateReadOnlyList AllChildren4 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren4.Count == AllChildren3.Count, $"New count: {AllChildren4.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));




                IFocusOptionalInner OptionalLeafInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IFocusOptionalInner;
                Assert.That(OptionalLeafInner != null);

                IFocusBrowsingOptionalNodeIndex ExistingIndex4 = OptionalLeafInner.ChildState.ParentIndex as IFocusBrowsingOptionalNodeIndex;

                Leaf NewItem4 = CreateLeaf(Guid.NewGuid());
                IFocusInsertionOptionalNodeIndex ReplacementIndex4;
                ReplacementIndex4 = ExistingIndex4.ToInsertionIndex(RootNode, NewItem4) as IFocusInsertionOptionalNodeIndex;
                Assert.That(ReplacementIndex4.ParentNode == RootNode);
                Assert.That(ReplacementIndex4.PropertyName == OptionalLeafInner.PropertyName);
                Assert.That(CompareEqual.CoverIsEqual(ReplacementIndex4, ReplacementIndex4));

                Controller.Replace(OptionalLeafInner, ReplacementIndex4, out IWriteableBrowsingChildIndex NewItemIndex4);
                Assert.That(Controller.Contains(NewItemIndex4));

                Assert.That(OptionalLeafInner.IsAssigned);
                IFocusOptionalNodeState NewItemState4 = OptionalLeafInner.ChildState as IFocusOptionalNodeState;
                Assert.That(NewItemState4.Node == NewItem4);
                Assert.That(NewItemState4.ParentIndex == NewItemIndex4);

                IFocusBrowsingOptionalNodeIndex DuplicateExistingIndex4 = ReplacementIndex4.ToBrowsingIndex() as IFocusBrowsingOptionalNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex4 as IFocusBrowsingOptionalNodeIndex, DuplicateExistingIndex4));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex4, NewItemIndex4 as IFocusBrowsingOptionalNodeIndex));

                FocusNodeStateReadOnlyList AllChildren5 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren5.Count == AllChildren4.Count, $"New count: {AllChildren5.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                IFocusBrowsingOptionalNodeIndex ExistingIndex5 = OptionalLeafInner.ChildState.ParentIndex as IFocusBrowsingOptionalNodeIndex;

                //System.Diagnostics.Debug.Assert(false);
                Leaf NewItem5 = CreateLeaf(Guid.NewGuid());
                IFocusInsertionOptionalClearIndex ReplacementIndex5;
                ReplacementIndex5 = ExistingIndex5.ToInsertionIndex(RootNode, null) as IFocusInsertionOptionalClearIndex;
                Assert.That(ReplacementIndex5.ParentNode == RootNode);
                Assert.That(ReplacementIndex5.PropertyName == OptionalLeafInner.PropertyName);
                Assert.That(CompareEqual.CoverIsEqual(ReplacementIndex5, ReplacementIndex5));

                Controller.Replace(OptionalLeafInner, ReplacementIndex5, out IWriteableBrowsingChildIndex NewItemIndex5);
                Assert.That(Controller.Contains(NewItemIndex5));

                Assert.That(!OptionalLeafInner.IsAssigned);
                IFocusOptionalNodeState NewItemState5 = OptionalLeafInner.ChildState as IFocusOptionalNodeState;
                Assert.That(NewItemState5.ParentIndex == NewItemIndex5);

                IFocusBrowsingOptionalNodeIndex DuplicateExistingIndex5 = ReplacementIndex5.ToBrowsingIndex() as IFocusBrowsingOptionalNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex5 as IFocusBrowsingOptionalNodeIndex, DuplicateExistingIndex5));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex5, NewItemIndex5 as IFocusBrowsingOptionalNodeIndex));

                FocusNodeStateReadOnlyList AllChildren6 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FocusAssign()
        {
            //System.Diagnostics.Debug.Assert(false);
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            //System.Diagnostics.Debug.Assert(false);
            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFocusOptionalInner UnassignedOptionalLeafInner = RootState.PropertyToInner(nameof(Main.UnassignedOptionalLeaf)) as IFocusOptionalInner;
                Assert.That(UnassignedOptionalLeafInner != null);
                Assert.That(!UnassignedOptionalLeafInner.IsAssigned);

                IFocusBrowsingOptionalNodeIndex AssignmentIndex0 = UnassignedOptionalLeafInner.ChildState.ParentIndex;
                Assert.That(AssignmentIndex0 != null);

                FocusNodeStateReadOnlyList AllChildren0 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Assign(AssignmentIndex0, out bool IsChanged);
                Assert.That(IsChanged);
                Assert.That(UnassignedOptionalLeafInner.IsAssigned);

                FocusNodeStateReadOnlyList AllChildren1 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Assign(AssignmentIndex0, out IsChanged);
                Assert.That(!IsChanged);
                Assert.That(UnassignedOptionalLeafInner.IsAssigned);

                FocusNodeStateReadOnlyList AllChildren2 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Controller.Unassign(AssignmentIndex0, out IsChanged);
                Assert.That(IsChanged);
                Assert.That(!UnassignedOptionalLeafInner.IsAssigned);

                FocusNodeStateReadOnlyList AllChildren3 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FocusUnassign()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFocusOptionalInner AssignedOptionalLeafInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IFocusOptionalInner;
                Assert.That(AssignedOptionalLeafInner != null);
                Assert.That(AssignedOptionalLeafInner.IsAssigned);

                IFocusBrowsingOptionalNodeIndex AssignmentIndex0 = AssignedOptionalLeafInner.ChildState.ParentIndex;
                Assert.That(AssignmentIndex0 != null);

                FocusNodeStateReadOnlyList AllChildren0 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Unassign(AssignmentIndex0, out bool IsChanged);
                Assert.That(IsChanged);
                Assert.That(!AssignedOptionalLeafInner.IsAssigned);

                FocusNodeStateReadOnlyList AllChildren1 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count - 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Unassign(AssignmentIndex0, out IsChanged);
                Assert.That(!IsChanged);
                Assert.That(!AssignedOptionalLeafInner.IsAssigned);

                FocusNodeStateReadOnlyList AllChildren2 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Controller.Assign(AssignmentIndex0, out IsChanged);
                Assert.That(IsChanged);
                Assert.That(AssignedOptionalLeafInner.IsAssigned);

                FocusNodeStateReadOnlyList AllChildren3 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FocusChangeReplication()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;
            bool IsMoved;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFocusBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
                Assert.That(LeafBlocksInner != null);

                FocusNodeStateReadOnlyList AllChildren0 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                IFocusBlockState BlockState = (IFocusBlockState)LeafBlocksInner.BlockStateList[0];
                Assert.That(BlockState != null);
                Assert.That(BlockState.ParentInner == LeafBlocksInner);
                BaseNode.IBlock ChildBlock = BlockState.ChildBlock;
                Assert.That(ChildBlock.Replication == BaseNode.ReplicationStatus.Normal);

                Controller.ChangeReplication(LeafBlocksInner, 0, BaseNode.ReplicationStatus.Replicated);

                Assert.That(ChildBlock.Replication == BaseNode.ReplicationStatus.Replicated);

                FocusNodeStateReadOnlyList AllChildren1 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                //System.Diagnostics.Debug.Assert(false);
                ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                int MaxFocusMove = ControllerView0.MaxFocusMove;

                bool IsReplicationModifiable = false;
                for (int i = 0; i < MaxFocusMove; i++)
                {
                    IsReplicationModifiable |= ControllerView0.IsReplicationModifiable(out IFocusBlockListInner Inner, out int BlockIndex, out BaseNode.ReplicationStatus Replication);

                    if (ControllerView0.Focus.CellView.StateView.State is IFocusPatternState AsPatternState)
                    {
                        Controller.ChangeTextAndCaretPosition(AsPatternState.ParentIndex, nameof(BaseNode.Pattern.Text), "test", 0, 1, false);
                        Assert.That(Controller.CanUndo);
                        Controller.Undo();
                    }

                    else if (ControllerView0.Focus.CellView.StateView.State is IFocusSourceState AsSourceState)
                    {
                        Controller.ChangeTextAndCaretPosition(AsSourceState.ParentIndex, nameof(BaseNode.Identifier.Text), "test", 0, 1, false);
                        Assert.That(Controller.CanUndo);
                        Controller.Undo();
                    }

                    ControllerView0.MoveFocus(+1, true, out IsMoved);
                }

                Assert.That(IsReplicationModifiable);

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
        public static void FocusSplit()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFocusBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
                Assert.That(LeafBlocksInner != null);

                FocusNodeStateReadOnlyList AllChildren0 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                IFocusBlockState BlockState0 = (IFocusBlockState)LeafBlocksInner.BlockStateList[0];
                Assert.That(BlockState0 != null);
                BaseNode.IBlock ChildBlock0 = BlockState0.ChildBlock;
                Assert.That(ChildBlock0.NodeList.Count == 1);

                IFocusBlockState BlockState1 = (IFocusBlockState)LeafBlocksInner.BlockStateList[1];
                Assert.That(BlockState1 != null);
                BaseNode.IBlock ChildBlock1 = BlockState1.ChildBlock;
                Assert.That(ChildBlock1.NodeList.Count == 2);

                Assert.That(LeafBlocksInner.Count == 4);
                Assert.That(LeafBlocksInner.BlockStateList.Count == 3);

                IFocusBrowsingExistingBlockNodeIndex SplitIndex0 = LeafBlocksInner.IndexAt(1, 1) as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.IsSplittable(LeafBlocksInner, SplitIndex0));

                Controller.SplitBlock(LeafBlocksInner, SplitIndex0);

                Assert.That(LeafBlocksInner.BlockStateList.Count == 4);
                Assert.That(ChildBlock0 == LeafBlocksInner.BlockStateList[0].ChildBlock);
                Assert.That(ChildBlock1 == LeafBlocksInner.BlockStateList[2].ChildBlock);
                Assert.That(ChildBlock1.NodeList.Count == 1);

                IFocusBlockState BlockState12 = (IFocusBlockState)LeafBlocksInner.BlockStateList[1];
                Assert.That(BlockState12.ChildBlock.NodeList.Count == 1);

                FocusNodeStateReadOnlyList AllChildren1 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FocusMerge()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFocusBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
                Assert.That(LeafBlocksInner != null);

                FocusNodeStateReadOnlyList AllChildren0 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                IFocusBlockState BlockState0 = (IFocusBlockState)LeafBlocksInner.BlockStateList[0];
                Assert.That(BlockState0 != null);
                BaseNode.IBlock ChildBlock0 = BlockState0.ChildBlock;
                Assert.That(ChildBlock0.NodeList.Count == 1);

                IFocusBlockState BlockState1 = (IFocusBlockState)LeafBlocksInner.BlockStateList[1];
                Assert.That(BlockState1 != null);
                BaseNode.IBlock ChildBlock1 = BlockState1.ChildBlock;
                Assert.That(ChildBlock1.NodeList.Count == 2);

                Assert.That(LeafBlocksInner.Count == 4);

                IFocusBrowsingExistingBlockNodeIndex MergeIndex0 = LeafBlocksInner.IndexAt(1, 0) as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.IsMergeable(LeafBlocksInner, MergeIndex0));

                Assert.That(LeafBlocksInner.BlockStateList.Count == 3);

                Controller.MergeBlocks(LeafBlocksInner, MergeIndex0);

                Assert.That(LeafBlocksInner.BlockStateList.Count == 2);
                Assert.That(ChildBlock1 == LeafBlocksInner.BlockStateList[0].ChildBlock);
                Assert.That(ChildBlock1.NodeList.Count == 3);

                Assert.That(LeafBlocksInner.BlockStateList[0] == BlockState1);

                FocusNodeStateReadOnlyList AllChildren1 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FocusExpand()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                FocusNodeStateReadOnlyList AllChildren0 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Expand(RootIndex, out bool IsChanged);
                Assert.That(IsChanged);

                FocusNodeStateReadOnlyList AllChildren1 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 2, $"New count: {AllChildren1.Count - AllChildren0.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(!IsChanged);

                FocusNodeStateReadOnlyList AllChildren2 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                IFocusOptionalInner OptionalLeafInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IFocusOptionalInner;
                Assert.That(OptionalLeafInner != null);

                IFocusInsertionOptionalClearIndex ReplacementIndex5 = new FocusInsertionOptionalClearIndex(RootNode, nameof(Main.AssignedOptionalLeaf));

                Controller.Replace(OptionalLeafInner, ReplacementIndex5, out IWriteableBrowsingChildIndex NewItemIndex5);
                Assert.That(Controller.Contains(NewItemIndex5));

                FocusNodeStateReadOnlyList AllChildren3 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count - 1, $"New count: {AllChildren3.Count - AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FocusNodeStateReadOnlyList AllChildren4 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren4.Count == AllChildren3.Count + 1, $"New count: {AllChildren4.Count}");



                IFocusBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFocusBrowsingExistingBlockNodeIndex RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                FocusNodeStateReadOnlyList AllChildren5 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren5.Count == AllChildren4.Count - 10, $"New count: {AllChildren5.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(LeafBlocksInner.IsEmpty);

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(!IsChanged);

                FocusNodeStateReadOnlyList AllChildren6 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren6.Count == AllChildren5.Count, $"New count: {AllChildren6.Count}");

                IDictionary<Type, string[]> WithExpandCollectionTable = BaseNodeHelper.NodeHelper.WithExpandCollectionTable as IDictionary<Type, string[]>;
                WithExpandCollectionTable.Add(typeof(Main), new string[] { nameof(Main.LeafBlocks) });

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FocusNodeStateReadOnlyList AllChildren7 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren7.Count == AllChildren6.Count + 3, $"New count: {AllChildren7.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(!LeafBlocksInner.IsEmpty);
                Assert.That(LeafBlocksInner.IsSingle);

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                WithExpandCollectionTable.Remove(typeof(Main));

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
        public static void FocusReduce()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;
            bool IsChanged;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFocusBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFocusBrowsingExistingBlockNodeIndex RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(LeafBlocksInner.IsEmpty);

                FocusNodeStateReadOnlyList AllChildren0 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 9, $"New count: {AllChildren0.Count}");

                IDictionary<Type, string[]> WithExpandCollectionTable = BaseNodeHelper.NodeHelper.WithExpandCollectionTable as IDictionary<Type, string[]>;
                WithExpandCollectionTable.Add(typeof(Main), new string[] { nameof(Main.LeafBlocks) });

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FocusNodeStateReadOnlyList AllChildren1 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 5, $"New count: {AllChildren1.Count - AllChildren0.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                //System.Diagnostics.Debug.Assert(false);
                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FocusNodeStateReadOnlyList AllChildren2 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count - 4, $"New count: {AllChildren2.Count - AllChildren1.Count}");

                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(!IsChanged);

                FocusNodeStateReadOnlyList AllChildren3 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count, $"New count: {AllChildren3.Count}");

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FocusNodeStateReadOnlyList AllChildren4 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren4.Count == AllChildren3.Count + 4, $"New count: {AllChildren4.Count - AllChildren3.Count}");

                //System.Diagnostics.Debug.Assert(false);
                BaseNode.IBlock ChildBlock = LeafBlocksInner.BlockStateList[0].ChildBlock;
                Leaf FirstNode = ChildBlock.NodeList[0] as Leaf;
                Assert.That(FirstNode != null);
                BaseNodeHelper.NodeTreeHelper.SetString(FirstNode, nameof(Leaf.Text), "!");

                IFocusOptionalInner LeafOptionalInner = RootState.PropertyToInner(nameof(Main.EmptyOptionalLeaf)) as IFocusOptionalInner;
                Assert.That(LeafOptionalInner != null);
                Assert.That(LeafOptionalInner.IsAssigned);
                BaseNodeHelper.NodeTreeHelper.SetString(LeafOptionalInner.ChildState.Node, nameof(Leaf.Text), "!");

                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(!IsChanged);

                FocusNodeStateReadOnlyList AllChildren5 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren5.Count == AllChildren4.Count, $"New count: {AllChildren5.Count}");

                BaseNodeHelper.NodeTreeHelper.SetString(FirstNode, nameof(Leaf.Text), "");

                //System.Diagnostics.Debug.Assert(false);
                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FocusNodeStateReadOnlyList AllChildren6 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren6.Count == AllChildren5.Count - 3, $"New count: {AllChildren6.Count}");

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                WithExpandCollectionTable.Remove(typeof(Main));

                //System.Diagnostics.Debug.Assert(false);
                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(!IsChanged);

                FocusNodeStateReadOnlyList AllChildren7 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren7.Count == AllChildren6.Count + 3, $"New count: {AllChildren7.Count}");

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                WithExpandCollectionTable.Add(typeof(Main), new string[] { nameof(Main.LeafBlocks) });

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

                WithExpandCollectionTable.Remove(typeof(Main));

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
        public static void FocusCanonicalize()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;
            bool IsChanged;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFocusBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFocusBrowsingExistingBlockNodeIndex RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                Assert.That(Controller.CanUndo);
                FocusOperationGroup LastOperation = (FocusOperationGroup)Controller.OperationStack[Controller.RedoIndex - 1];
                Assert.That(LastOperation.MainOperation is IFocusRemoveOperation);
                Assert.That(LastOperation.OperationList.Count > 0);
                Assert.That(LastOperation.Refresh == null);

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(LeafBlocksInner.IsEmpty);

                IFocusListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFocusListInner;
                Assert.That(LeafPathInner != null);
                Assert.That(LeafPathInner.Count == 2);

                IFocusBrowsingListNodeIndex RemovedListLeafIndex = LeafPathInner.StateList[0].ParentIndex as IFocusBrowsingListNodeIndex;
                Assert.That(Controller.Contains(RemovedListLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafPathInner, RemovedListLeafIndex));

                Controller.Remove(LeafPathInner, RemovedListLeafIndex);
                Assert.That(!Controller.Contains(RemovedListLeafIndex));

                IDictionary<Type, string[]> NeverEmptyCollectionTable = BaseNodeHelper.NodeHelper.NeverEmptyCollectionTable as IDictionary<Type, string[]>;
                NeverEmptyCollectionTable.Add(typeof(Main), new string[] { nameof(Main.PlaceholderTree) });

                RemovedListLeafIndex = LeafPathInner.StateList[0].ParentIndex as IFocusBrowsingListNodeIndex;
                Assert.That(Controller.Contains(RemovedListLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafPathInner, RemovedListLeafIndex));

                Controller.Remove(LeafPathInner, RemovedListLeafIndex);
                Assert.That(!Controller.Contains(RemovedListLeafIndex));
                Assert.That(LeafPathInner.Count == 0);

                NeverEmptyCollectionTable.Remove(typeof(Main));

                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();

                FocusNodeStateReadOnlyList AllChildren0 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 12, $"New count: {AllChildren0.Count}");

                IDictionary<Type, string[]> WithExpandCollectionTable = BaseNodeHelper.NodeHelper.WithExpandCollectionTable as IDictionary<Type, string[]>;
                WithExpandCollectionTable.Add(typeof(Main), new string[] { nameof(Main.LeafBlocks) });

                IFocusOptionalInner LeafOptionalInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IFocusOptionalInner;
                Assert.That(LeafOptionalInner != null);


                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FocusNodeStateReadOnlyList AllChildren1 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 2, $"New count: {AllChildren1.Count - AllChildren0.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Leaf Leaf = LeafOptionalInner.ChildState.Node as Leaf;
                BaseNodeHelper.NodeTreeHelper.SetStringProperty(Leaf, "Text", "");

                Controller.Canonicalize(out IsChanged);
                Assert.That(IsChanged);

                //System.Diagnostics.Debug.Assert(false);

                FocusNodeStateReadOnlyList AllChildren2 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count - 2, $"New count: {AllChildren2.Count - AllChildren1.Count}");

                Controller.Undo();
                Controller.Redo();

                Controller.Canonicalize(out IsChanged);
                Assert.That(!IsChanged);

                FocusNodeStateReadOnlyList AllChildren3 = (FocusNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count, $"New count: {AllChildren3.Count}");

                Assert.That(Controller.CanUndo);
                Controller.Undo();
                Assert.That(Controller.CanUndo);
                Controller.Undo();

                NeverEmptyCollectionTable.Add(typeof(Main), new string[] { nameof(Main.LeafBlocks) });
                Assert.That(LeafBlocksInner.BlockStateList.Count == 1);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == 1, LeafBlocksInner.BlockStateList[0].StateList.Count.ToString());

                Controller.Canonicalize(out IsChanged);
                Assert.That(IsChanged);

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                NeverEmptyCollectionTable.Remove(typeof(Main));

                WithExpandCollectionTable.Remove(typeof(Main));

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
        public static void FocusReplaceWithCycle()
        {
            ControllerTools.ResetExpectedName();

            BaseNode.Class RootNode;
            IFocusRootNodeIndex RootIndex;
            bool IsMoved;
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

            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
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

                IFocusNodeStateView StateView = ControllerView0.Focus.CellView.StateView;
                //Assert.That(ControllerView0.CollectionHasItems(StateView, nameof(BaseNode.IFunctionFeature.OverloadBlocks), 0));
                //Assert.That(ControllerView0.IsFirstItem(StateView));

                IFocusNodeState CurrentState = StateView.State;
                Assert.That(CurrentState != null && CurrentState.Node is BaseNode.Feature);

                FocusInsertionChildNodeIndexList CycleIndexList;
                int FeatureCycleCount = 14;
                IFocusBrowsingChildIndex NewItemIndex0;

                ControllerView0.SetUserVisible(true);
                ControllerView0.SetUserVisible(false);

                //System.Diagnostics.Debug.Assert(false);

                for (int i = 0; i < FeatureCycleCount; i++)
                {
                    IsItemCyclableThrough = ControllerView0.IsItemCyclableThrough(out State, out CyclePosition);
                    System.Diagnostics.Debug.Assert(IsItemCyclableThrough);
                    Assert.That(IsItemCyclableThrough);

                    CycleIndexList = State.CycleIndexList;

                    CyclePosition = (CyclePosition + 1) % CycleIndexList.Count;
                    Controller.Replace(State.ParentInner, CycleIndexList, CyclePosition, out NewItemIndex0);
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

                        if (ControllerView0.Focus.CellView.Frame is IFocusKeywordFrame AsFocusableKeywordFrame && (AsFocusableKeywordFrame.Text == "deferred" || AsFocusableKeywordFrame.Text == "extern" || AsFocusableKeywordFrame.Text == "precursor"))
                            break;

                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }

                    StateView = ControllerView0.Focus.CellView.StateView;
                    CurrentState = StateView.State;
                    if (CurrentState.Node is BaseNode.Identifier AsStateIdentifier && AsStateIdentifier.Text == FunctionFirstInstruction.Command.Path[0].Text)
                    {
                        /*
                        Assert.That(ControllerView0.IsFirstItem(StateView));

                        IFocusNodeState ParentState = CurrentState.ParentState;
                        Assert.That(ControllerView0.StateViewTable.ContainsKey(ParentState));
                        IFocusNodeStateView ParentStateView = ControllerView0.StateViewTable[ParentState];
                        Assert.That(ControllerView0.CollectionHasItems(ParentStateView, nameof(BaseNode.IQualifiedName.Path), 0));
                        */
                    }

                    IsItemCyclableThrough = ControllerView0.IsItemCyclableThrough(out State, out CyclePosition);
                    Assert.That(IsItemCyclableThrough);

                    CycleIndexList = State.CycleIndexList;

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

                        if (ControllerView0.Focus.CellView.Frame is IFocusKeywordFrame AsFocusableKeywordFrame && (AsFocusableKeywordFrame.Text == "deferred" || AsFocusableKeywordFrame.Text == "extern" || AsFocusableKeywordFrame.Text == "precursor"))
                            break;

                        ControllerView0.MoveFocus(+1, true, out IsMoved);
                    }

                    StateView = ControllerView0.Focus.CellView.StateView;
                    CurrentState = StateView.State;
                    if (CurrentState.Node is BaseNode.Identifier AsStateIdentifier && AsStateIdentifier.Text == PropertyFirstInstruction.Command.Path[0].Text)
                    {
                        /*
                        Assert.That(ControllerView0.IsFirstItem(StateView));

                        IFocusNodeState ParentState = CurrentState.ParentState;
                        Assert.That(ControllerView0.StateViewTable.ContainsKey(ParentState));
                        IFocusNodeStateView ParentStateView = ControllerView0.StateViewTable[ParentState];
                        Assert.That(ControllerView0.CollectionHasItems(ParentStateView, nameof(BaseNode.IQualifiedName.Path), 0));
                        */
                    }

                    IsItemCyclableThrough = ControllerView0.IsItemCyclableThrough(out State, out CyclePosition);
                    Assert.That(IsItemCyclableThrough);

                    CycleIndexList = State.CycleIndexList;

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
                        Controller.SplitIdentifier(ListInner, ReplacementListNodeIndex, InsertionListNodeIndex, out IWriteableBrowsingListNodeIndex FirstIndex, out IWriteableBrowsingListNodeIndex SecondIndex);

                    ControllerView0.MoveFocus(+1, true, out IsMoved);
                }
            }
        }

        [Test]
        [Category("Coverage")]
        public static void FocusSimplify()
        {
            ControllerTools.ResetExpectedName();

            BaseNode.Class RootNode;
            IFocusRootNodeIndex RootIndex;
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

            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                int MaxFocusMove = ControllerView0.MaxFocusMove;
                bool IsFocused = false;

                for (int i = 0; i < MaxFocusMove; i++)
                {
                    if (ControllerView0.Focus is IFocusStringContentFocus AsStringContentFocus)
                    {
                        IFocusTextFocus AsTextFocus = AsStringContentFocus as IFocusTextFocus;
                        Assert.That(AsTextFocus != null);
                        Assert.That(AsTextFocus.CellView != null);

                        if (ControllerView0.FocusedText == "test1")
                        {
                            IsFocused = true;
                            break;
                        }
                    }

                    ControllerView0.MoveFocus(+1, true, out IsMoved);
                }

                Assert.That(IsFocused);
                bool IsItemSimplifiable = ControllerView0.IsItemSimplifiable(out IFocusInner Inner, out IFocusInsertionChildIndex Index);
                Assert.That(IsItemSimplifiable);

                Controller.Replace(Inner, Index, out IWriteableBrowsingChildIndex NodeIndex);

                bool IsItemComplexifiable = ControllerView0.IsItemComplexifiable(out IDictionary<IFocusInner, IList<IFocusInsertionChildNodeIndex>> IndexTable);
                Assert.That(IsItemComplexifiable);
            }
        }

        [Test]
        [Category("Coverage")]
        public static void FocusPrune()
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
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            using (FocusControllerView ControllerView0 = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFocusBlockListInner MainInnerH = RootState.PropertyToInner(nameof(Root.MainBlocksH)) as IFocusBlockListInner;
                Assert.That(MainInnerH != null);

                IFocusBlockListInner MainInnerV = RootState.PropertyToInner(nameof(Root.MainBlocksV)) as IFocusBlockListInner;
                Assert.That(MainInnerV != null);

                IFocusBrowsingExistingBlockNodeIndex MainIndex = MainInnerH.IndexAt(0, 0) as IFocusBrowsingExistingBlockNodeIndex;
                Controller.Remove(MainInnerH, MainIndex);

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                MainIndex = MainInnerH.IndexAt(0, 0) as IFocusBrowsingExistingBlockNodeIndex;
                Controller.Remove(MainInnerH, MainIndex);

                Controller.Undo();
                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));

                MainIndex = MainInnerH.IndexAt(0, 0) as IFocusBrowsingExistingBlockNodeIndex;
                Controller.Remove(MainInnerH, MainIndex);
                Controller.Undo();

                MainIndex = MainInnerV.IndexAt(0, 0) as IFocusBrowsingExistingBlockNodeIndex;
                Controller.Remove(MainInnerV, MainIndex);
                Controller.Undo();

                IFocusListInner LeafInnerH = RootState.PropertyToInner(nameof(Root.LeafPathH)) as IFocusListInner;
                Assert.That(LeafInnerH != null);

                IFocusBrowsingListNodeIndex LeafIndexH = LeafInnerH.IndexAt(0) as IFocusBrowsingListNodeIndex;
                Controller.Remove(LeafInnerH, LeafIndexH);
                Controller.Undo();

                IFocusListInner LeafInnerV = RootState.PropertyToInner(nameof(Root.LeafPathV)) as IFocusListInner;
                Assert.That(LeafInnerV != null);

                IFocusBrowsingListNodeIndex LeafIndexV = LeafInnerV.IndexAt(0) as IFocusBrowsingListNodeIndex;
                Controller.Remove(LeafInnerV, LeafIndexV);
                Controller.Undo();

                //System.Diagnostics.Debug.Assert(false);
                ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                Assert.That(ControllerView0.MinFocusMove == 0);

                IFocusOptionalInner OptionalMainInner = RootState.PropertyToInner(nameof(Root.UnassignedOptionalMain)) as IFocusOptionalInner;
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

                //System.Diagnostics.Debug.Assert(false);

                IFocusNodeState FirstMainState = (IFocusNodeState)MainInnerV.BlockStateList[0].StateList[0];
                IFocusBlockListInner LeafBlockInner = FirstMainState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;

                Controller.ChangeReplication(LeafBlockInner, 0, BaseNode.ReplicationStatus.Replicated);

                ControllerView0.MoveFocus(ControllerView0.MinFocusMove, true, out IsMoved);
                int MaxFocusMove = ControllerView0.MaxFocusMove;

                for (int i = 0; i < MaxFocusMove; i++)
                {
                    if (ControllerView0.Focus.CellView.StateView.State is IFocusPatternState AsPatternState)
                    {
                        Controller.ChangeTextAndCaretPosition(AsPatternState.ParentIndex, nameof(BaseNode.Pattern.Text), "test", 0, 1, false);
                        Assert.That(Controller.CanUndo);
                        Controller.Undo();
                    }

                    else if (ControllerView0.Focus.CellView.StateView.State is IFocusSourceState AsSourceState)
                    {
                        Controller.ChangeTextAndCaretPosition(AsSourceState.ParentIndex, nameof(BaseNode.Identifier.Text), "test", 0, 1, false);
                        Assert.That(Controller.CanUndo);
                        Controller.Undo();
                    }

                    ControllerView0.MoveFocus(+1, true, out IsMoved);
                }
            }
        }

        [Test]
        [Category("Coverage")]
        public static void FocusCollections()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFocusRootNodeIndex RootIndex;
            bool IsReadOnly;
            IReadOnlyBlockState FirstBlockState;
            IReadOnlyBrowsingBlockNodeIndex FirstBlockNodeIndex;
            IReadOnlyBrowsingListNodeIndex FirstListNodeIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FocusRootNodeIndex(RootNode);

            FocusController ControllerBase = FocusController.Create(RootIndex);
            FocusController Controller = FocusController.Create(RootIndex);

            ReadOnlyNodeStateDictionary ControllerStateTable = DebugObjects.GetReferenceByInterface(typeof(FocusNodeStateDictionary)) as ReadOnlyNodeStateDictionary;

            using (FocusControllerView ControllerView = FocusControllerView.Create(Controller, TestDebug.CoverageFocusTemplateSet.FocusTemplateSet))
            {
                Controller.Expand(Controller.RootIndex, out bool IsChanged);

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

                // IFocusBlockStateList

                IFocusNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFocusBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFocusBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFocusListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFocusListInner;
                Assert.That(LeafPathInner != null);

                IFocusPlaceholderNodeState FirstNodeState = LeafBlocksInner.FirstNodeState;
                FocusBlockStateList DebugBlockStateList = DebugObjects.GetReferenceByInterface(typeof(FocusBlockStateList)) as FocusBlockStateList;
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
                    DebugBlockStateList.CopyTo((IReadOnlyBlockState[])(new IFocusBlockState[DebugBlockStateList.Count]), 0);

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
                    DebugBlockStateList.CopyTo((IWriteableBlockState[])(new IFocusBlockState[DebugBlockStateList.Count]), 0);

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
                    DebugBlockStateList.CopyTo((IFrameBlockState[])(new IFocusBlockState[DebugBlockStateList.Count]), 0);

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
                }

                // IFocusBlockStateReadOnlyList

                FocusBlockStateReadOnlyList FocusBlockStateList = LeafBlocksInner.BlockStateList;
                Assert.That(FocusBlockStateList.Count > 0);
                FirstBlockState = FocusBlockStateList[0];
                Assert.That(FocusBlockStateList.Contains(FirstBlockState));
                Assert.That(FocusBlockStateList.IndexOf(FirstBlockState) == 0);
                Assert.That(FocusBlockStateList.Contains((IFocusBlockState)FirstBlockState));
                Assert.That(FocusBlockStateList.IndexOf((IFocusBlockState)FirstBlockState) == 0);

                IEnumerable<IWriteableBlockState> WriteableFocusBlockStateListAsIEnumerable = FocusBlockStateList;
                IEnumerator<IWriteableBlockState> WriteableFocusBlockStateListAsIEnumerableEnumerator = WriteableFocusBlockStateListAsIEnumerable.GetEnumerator();
                Assert.That(FocusBlockStateList.Contains((IWriteableBlockState)FirstBlockState));
                Assert.That(FocusBlockStateList.IndexOf((IWriteableBlockState)FirstBlockState) == 0);
                IReadOnlyList<IWriteableBlockState> WriteableFocusBlockStateListAsIReadOnlyList = FocusBlockStateList;
                Assert.That(WriteableFocusBlockStateListAsIReadOnlyList[0] == FirstBlockState);

                IEnumerable<IFrameBlockState> FrameFocusBlockStateListAsIEnumerable = FocusBlockStateList;
                IEnumerator<IFrameBlockState> FrameFocusBlockStateListAsIEnumerableEnumerator = FrameFocusBlockStateListAsIEnumerable.GetEnumerator();
                Assert.That(FocusBlockStateList.Contains((IFrameBlockState)FirstBlockState));
                Assert.That(FocusBlockStateList.IndexOf((IFrameBlockState)FirstBlockState) == 0);
                IReadOnlyList<IFrameBlockState> FrameFocusBlockStateListAsIReadOnlyList = FocusBlockStateList;
                Assert.That(FrameFocusBlockStateListAsIReadOnlyList[0] == FirstBlockState);

                // IFocusBrowsingBlockNodeIndexList

                FocusBrowsingBlockNodeIndexList BlockNodeIndexList = LeafBlocksInner.AllIndexes() as FocusBrowsingBlockNodeIndexList;
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
                BlockNodeIndexList.CopyTo((IReadOnlyBrowsingBlockNodeIndex[])(new IFocusBrowsingBlockNodeIndex[BlockNodeIndexList.Count]), 0);
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
                BlockNodeIndexList.CopyTo((IWriteableBrowsingBlockNodeIndex[])(new IFocusBrowsingBlockNodeIndex[BlockNodeIndexList.Count]), 0);
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
                BlockNodeIndexList.CopyTo((IFrameBrowsingBlockNodeIndex[])(new IFocusBrowsingBlockNodeIndex[BlockNodeIndexList.Count]), 0);
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
                ReadOnlyBrowsingBlockNodeIndexList BlockNodeIndexListAsReadOnly = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsReadOnly[0] == FirstBlockNodeIndex);
                IEnumerator<IFrameBrowsingBlockNodeIndex> BlockNodeIndexListFrameEnumerator = ((ICollection<IFrameBrowsingBlockNodeIndex>)BlockNodeIndexList).GetEnumerator();

                // IFocusBrowsingListNodeIndexList

                FocusBrowsingListNodeIndexList ListNodeIndexList = LeafPathInner.AllIndexes() as FocusBrowsingListNodeIndexList;
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
                ListNodeIndexList.CopyTo((IReadOnlyBrowsingListNodeIndex[])(new IFocusBrowsingListNodeIndex[ListNodeIndexList.Count]), 0);
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
                ListNodeIndexList.CopyTo((IWriteableBrowsingListNodeIndex[])(new IFocusBrowsingListNodeIndex[ListNodeIndexList.Count]), 0);
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
                ListNodeIndexList.CopyTo((IFrameBrowsingListNodeIndex[])(new IFocusBrowsingListNodeIndex[ListNodeIndexList.Count]), 0);
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
                ReadOnlyBrowsingListNodeIndexList ListNodeIndexListAsReadOnly = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsReadOnly[0] == FirstListNodeIndex);
                IEnumerator<IFrameBrowsingListNodeIndex> ListNodeIndexListFrameEnumerator = ((ICollection<IFrameBrowsingListNodeIndex>)ListNodeIndexList).GetEnumerator();

                // IFocusIndexNodeStateDictionary
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
                }

                // IFocusIndexNodeStateReadOnlyDictionary

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

                // IFocusInnerDictionary

                FocusInnerDictionary<string> FocusInnerTableModify = DebugObjects.GetReferenceByInterface(typeof(FocusInnerDictionary<string>)) as FocusInnerDictionary<string>;
                Assert.That(FocusInnerTableModify != null);
                Assert.That(FocusInnerTableModify.Count > 0);


                IDictionary<string, IReadOnlyInner> ReadOnlyInnerTableModifyAsDictionary = FocusInnerTableModify;
                Assert.That(ReadOnlyInnerTableModifyAsDictionary.Keys != null);
                Assert.That(ReadOnlyInnerTableModifyAsDictionary.Values != null);
                foreach (KeyValuePair<string, IFocusInner> Entry in (ICollection<KeyValuePair<string, IFocusInner>>)FocusInnerTableModify)
                {
                    Assert.That(ReadOnlyInnerTableModifyAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(ReadOnlyInnerTableModifyAsDictionary[Entry.Key] == Entry.Value);
                }
                ICollection<KeyValuePair<string, IReadOnlyInner>> ReadOnlyInnerTableModifyAsCollection = FocusInnerTableModify;
                Assert.That(!ReadOnlyInnerTableModifyAsCollection.IsReadOnly);
                IEnumerable<KeyValuePair<string, IReadOnlyInner>> ReadOnlyInnerTableModifyAsEnumerable = FocusInnerTableModify;
                IEnumerator<KeyValuePair<string, IReadOnlyInner>> ReadOnlyInnerTableModifyAsEnumerableEnumerator = ReadOnlyInnerTableModifyAsEnumerable.GetEnumerator();
                foreach (KeyValuePair<string, IReadOnlyInner> Entry in ReadOnlyInnerTableModifyAsEnumerable)
                {
                    Assert.That(ReadOnlyInnerTableModifyAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(ReadOnlyInnerTableModifyAsDictionary[Entry.Key] == Entry.Value);
                    Assert.That(FocusInnerTableModify.TryGetValue(Entry.Key, out IReadOnlyInner ReadOnlyInnerValue) == FocusInnerTableModify.TryGetValue(Entry.Key, out IReadOnlyInner FocusInnerValue));

                    Assert.That(((ICollection<KeyValuePair<string, IReadOnlyInner>>)FocusInnerTableModify).Contains(Entry));
                    ((ICollection<KeyValuePair<string, IReadOnlyInner>>)FocusInnerTableModify).Remove(Entry);
                    ((ICollection<KeyValuePair<string, IReadOnlyInner>>)FocusInnerTableModify).Add(Entry);
                    ((ICollection<KeyValuePair<string, IReadOnlyInner>>)FocusInnerTableModify).CopyTo(new KeyValuePair<string, IReadOnlyInner>[FocusInnerTableModify.Count], 0);
                    break;
                }


                WriteableInnerDictionary<string> WriteableInnerTableModify = FocusInnerTableModify;
                WriteableInnerTableModify.GetEnumerator();
                IDictionary<string, IWriteableInner> WriteableInnerTableModifyAsDictionary = FocusInnerTableModify;
                Assert.That(WriteableInnerTableModifyAsDictionary.Keys != null);
                Assert.That(WriteableInnerTableModifyAsDictionary.Values != null);
                foreach (KeyValuePair<string, IFocusInner> Entry in (ICollection<KeyValuePair<string, IFocusInner>>)FocusInnerTableModify)
                {
                    Assert.That(WriteableInnerTableModify[Entry.Key] == Entry.Value);
                    Assert.That(WriteableInnerTableModifyAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(WriteableInnerTableModifyAsDictionary[Entry.Key] == Entry.Value);
                    WriteableInnerTableModifyAsDictionary.Remove(Entry.Key);
                    WriteableInnerTableModifyAsDictionary.Add(Entry.Key, Entry.Value);
                    WriteableInnerTableModifyAsDictionary.TryGetValue(Entry.Key, out IWriteableInner WriteableInnerValue);
                    break;
                }
                ICollection<KeyValuePair<string, IWriteableInner>> WriteableInnerTableModifyAsCollection = FocusInnerTableModify;
                Assert.That(!WriteableInnerTableModifyAsCollection.IsReadOnly);
                WriteableInnerTableModifyAsCollection.CopyTo(new KeyValuePair<string, IWriteableInner>[WriteableInnerTableModifyAsCollection.Count], 0);
                foreach (KeyValuePair<string, IWriteableInner> Entry in WriteableInnerTableModifyAsCollection)
                {
                    Assert.That(WriteableInnerTableModifyAsCollection.Contains(Entry));
                    WriteableInnerTableModifyAsCollection.Remove(Entry);
                    WriteableInnerTableModifyAsCollection.Add(Entry);
                    break;
                }
                IEnumerable<KeyValuePair<string, IWriteableInner>> WriteableInnerTableModifyAsEnumerable = FocusInnerTableModify;
                IEnumerator<KeyValuePair<string, IWriteableInner>> WriteableInnerTableModifyAsEnumerableEnumerator = WriteableInnerTableModifyAsEnumerable.GetEnumerator();


                FrameInnerDictionary<string> FrameInnerTableModify = FocusInnerTableModify;
                FrameInnerTableModify.GetEnumerator();
                IDictionary<string, IFrameInner> FrameInnerTableModifyAsDictionary = FocusInnerTableModify;
                Assert.That(FrameInnerTableModifyAsDictionary.Keys != null);
                Assert.That(FrameInnerTableModifyAsDictionary.Values != null);
                foreach (KeyValuePair<string, IFocusInner> Entry in (ICollection<KeyValuePair<string, IFocusInner>>)FocusInnerTableModify)
                {
                    Assert.That(FrameInnerTableModify[Entry.Key] == Entry.Value);
                    Assert.That(FrameInnerTableModifyAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(FrameInnerTableModifyAsDictionary[Entry.Key] == Entry.Value);
                    FrameInnerTableModifyAsDictionary.Remove(Entry.Key);
                    FrameInnerTableModifyAsDictionary.Add(Entry.Key, Entry.Value);
                    FrameInnerTableModifyAsDictionary.TryGetValue(Entry.Key, out IFrameInner FrameInnerValue);
                    break;
                }
                ICollection<KeyValuePair<string, IFrameInner>> FrameInnerTableModifyAsCollection = FocusInnerTableModify;
                Assert.That(!FrameInnerTableModifyAsCollection.IsReadOnly);
                FrameInnerTableModifyAsCollection.CopyTo(new KeyValuePair<string, IFrameInner>[FrameInnerTableModifyAsCollection.Count], 0);
                foreach (KeyValuePair<string, IFrameInner> Entry in FrameInnerTableModifyAsCollection)
                {
                    Assert.That(FrameInnerTableModifyAsCollection.Contains(Entry));
                    FrameInnerTableModifyAsCollection.Remove(Entry);
                    FrameInnerTableModifyAsCollection.Add(Entry);
                    break;
                }
                IEnumerable<KeyValuePair<string, IFrameInner>> FrameInnerTableModifyAsEnumerable = FocusInnerTableModify;
                IEnumerator<KeyValuePair<string, IFrameInner>> FrameInnerTableModifyAsEnumerableEnumerator = FrameInnerTableModifyAsEnumerable.GetEnumerator();


                // IFocusInnerReadOnlyDictionary

                FocusInnerReadOnlyDictionary<string> FocusInnerTable = RootState.InnerTable;

                IReadOnlyDictionary<string, IReadOnlyInner> ReadOnlyInnerTableAsDictionary = FocusInnerTable;
                Assert.That(ReadOnlyInnerTableAsDictionary.Keys != null);
                Assert.That(ReadOnlyInnerTableAsDictionary.Values != null);
                foreach (KeyValuePair<string, IFocusInner> Entry in (ICollection<KeyValuePair<string, IFocusInner>>)FocusInnerTable)
                {
                    Assert.That(FocusInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner ReadOnlyInnerValue) == FocusInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner FocusInnerValue));
                    break;
                }


                WriteableInnerReadOnlyDictionary<string> WriteableInnerTable = RootState.InnerTable;
                IReadOnlyDictionary<string, IWriteableInner> WriteableInnerTableAsDictionary = FocusInnerTable;
                Assert.That(WriteableInnerTableAsDictionary.Keys != null);
                Assert.That(WriteableInnerTableAsDictionary.Values != null);
                IEnumerable<KeyValuePair<string, IWriteableInner>> WriteableInnerTableAsIEnumerable = FocusInnerTable;
                WriteableInnerTableAsIEnumerable.GetEnumerator();
                foreach (KeyValuePair<string, IFocusInner> Entry in (ICollection<KeyValuePair<string, IFocusInner>>)FocusInnerTable)
                {
                    Assert.That(WriteableInnerTableAsDictionary[Entry.Key] == Entry.Value);
                    Assert.That(FocusInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner WriteableInnerValue) == FocusInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner FocusInnerValue));
                    break;
                }


                FrameInnerReadOnlyDictionary<string> FrameInnerTable = RootState.InnerTable;
                IReadOnlyDictionary<string, IFrameInner> FrameInnerTableAsDictionary = FocusInnerTable;
                Assert.That(FrameInnerTableAsDictionary.Keys != null);
                Assert.That(FrameInnerTableAsDictionary.Values != null);
                IEnumerable<KeyValuePair<string, IFrameInner>> FrameInnerTableAsIEnumerable = FocusInnerTable;
                FrameInnerTableAsIEnumerable.GetEnumerator();
                foreach (KeyValuePair<string, IFocusInner> Entry in (ICollection<KeyValuePair<string, IFocusInner>>)FocusInnerTable)
                {
                    Assert.That(FrameInnerTableAsDictionary[Entry.Key] == Entry.Value);
                    Assert.That(FocusInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner FrameInnerValue) == FocusInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner FocusInnerValue));
                    break;
                }

                // FocusNodeStateList

                FirstNodeState = LeafPathInner.FirstNodeState;
                Assert.That(FirstNodeState != null);
                FocusNodeStateList FocusNodeStateListModify = DebugObjects.GetReferenceByInterface(typeof(FocusNodeStateList)) as FocusNodeStateList;
                Assert.That(FocusNodeStateListModify != null);
                Assert.That(FocusNodeStateListModify.Count > 0);
                FirstNodeState = FocusNodeStateListModify[0] as IFocusPlaceholderNodeState;

                Assert.That(FocusNodeStateListModify.Contains((IReadOnlyNodeState)FirstNodeState));
                Assert.That(FocusNodeStateListModify.IndexOf((IReadOnlyNodeState)FirstNodeState) == 0);
                FocusNodeStateListModify.Remove((IReadOnlyNodeState)FirstNodeState);
                FocusNodeStateListModify.Insert(0, (IReadOnlyNodeState)FirstNodeState);
                FocusNodeStateListModify.CopyTo((IReadOnlyNodeState[])(new IFocusNodeState[FocusNodeStateListModify.Count]), 0);
                ReadOnlyNodeStateList FocusNodeStateListModifyAsReadOnly = FocusNodeStateListModify as ReadOnlyNodeStateList;
                Assert.That(FocusNodeStateListModifyAsReadOnly != null);
                Assert.That(FocusNodeStateListModifyAsReadOnly[0] == FocusNodeStateListModify[0]);
                IList<IReadOnlyNodeState> ReadOnlyNodeStateListModifyAsIList = FocusNodeStateListModify as IList<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListModifyAsIList != null);
                Assert.That(ReadOnlyNodeStateListModifyAsIList[0] == FocusNodeStateListModify[0]);
                IReadOnlyList<IReadOnlyNodeState> ReadOnlyNodeStateListModifyAsIReadOnlyList = FocusNodeStateListModify as IReadOnlyList<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListModifyAsIReadOnlyList != null);
                Assert.That(ReadOnlyNodeStateListModifyAsIReadOnlyList[0] == FocusNodeStateListModify[0]);
                ICollection<IReadOnlyNodeState> ReadOnlyNodeStateListModifyAsCollection = FocusNodeStateListModify as ICollection<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListModifyAsCollection != null);
                Assert.That(!ReadOnlyNodeStateListModifyAsCollection.IsReadOnly);
                IEnumerable<IReadOnlyNodeState> ReadOnlyNodeStateListModifyAsEnumerable = FocusNodeStateListModify as IEnumerable<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListModifyAsEnumerable != null);
                Assert.That(ReadOnlyNodeStateListModifyAsEnumerable.GetEnumerator() != null);


                WriteableNodeStateList FocusNodeStateListModifyAsWriteable = FocusNodeStateListModify as WriteableNodeStateList;
                Assert.That(FocusNodeStateListModifyAsWriteable != null);
                Assert.That(FocusNodeStateListModifyAsWriteable[0] == FocusNodeStateListModify[0]);
                FocusNodeStateListModifyAsWriteable.GetEnumerator();
                IList<IWriteableNodeState> WriteableNodeStateListModifyAsIList = FocusNodeStateListModify as IList<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListModifyAsIList != null);
                Assert.That(WriteableNodeStateListModifyAsIList[0] == FocusNodeStateListModify[0]);
                Assert.That(WriteableNodeStateListModifyAsIList.IndexOf(FirstNodeState) == 0);
                WriteableNodeStateListModifyAsIList.Remove(FirstNodeState);
                WriteableNodeStateListModifyAsIList.Insert(0, FirstNodeState);
                IReadOnlyList<IWriteableNodeState> WriteableNodeStateListModifyAsIReadOnlyList = FocusNodeStateListModify as IReadOnlyList<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListModifyAsIReadOnlyList != null);
                Assert.That(WriteableNodeStateListModifyAsIReadOnlyList[0] == FocusNodeStateListModify[0]);
                ICollection<IWriteableNodeState> WriteableNodeStateListModifyAsCollection = FocusNodeStateListModify as ICollection<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListModifyAsCollection != null);
                Assert.That(!WriteableNodeStateListModifyAsCollection.IsReadOnly);
                Assert.That(WriteableNodeStateListModifyAsCollection.Contains(FirstNodeState));
                WriteableNodeStateListModifyAsCollection.Remove(FirstNodeState);
                WriteableNodeStateListModifyAsCollection.Add(FirstNodeState);
                WriteableNodeStateListModifyAsCollection.Remove(FirstNodeState);
                FocusNodeStateListModify.Insert(0, FirstNodeState);
                WriteableNodeStateListModifyAsCollection.CopyTo(new IFocusNodeState[WriteableNodeStateListModifyAsCollection.Count], 0);
                IEnumerable<IWriteableNodeState> WriteableNodeStateListModifyAsEnumerable = FocusNodeStateListModify as IEnumerable<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListModifyAsEnumerable != null);
                Assert.That(WriteableNodeStateListModifyAsEnumerable.GetEnumerator() != null);


                FrameNodeStateList FocusNodeStateListModifyAsFrame = FocusNodeStateListModify as FrameNodeStateList;
                Assert.That(FocusNodeStateListModifyAsFrame != null);
                Assert.That(FocusNodeStateListModifyAsFrame[0] == FocusNodeStateListModify[0]);
                FocusNodeStateListModifyAsFrame.GetEnumerator();
                IList<IFrameNodeState> FrameNodeStateListModifyAsIList = FocusNodeStateListModify as IList<IFrameNodeState>;
                Assert.That(FrameNodeStateListModifyAsIList != null);
                Assert.That(FrameNodeStateListModifyAsIList[0] == FocusNodeStateListModify[0]);
                Assert.That(FrameNodeStateListModifyAsIList.IndexOf(FirstNodeState) == 0);
                FrameNodeStateListModifyAsIList.Remove(FirstNodeState);
                FrameNodeStateListModifyAsIList.Insert(0, FirstNodeState);
                IReadOnlyList<IFrameNodeState> FrameNodeStateListModifyAsIReadOnlyList = FocusNodeStateListModify as IReadOnlyList<IFrameNodeState>;
                Assert.That(FrameNodeStateListModifyAsIReadOnlyList != null);
                Assert.That(FrameNodeStateListModifyAsIReadOnlyList[0] == FocusNodeStateListModify[0]);
                ICollection<IFrameNodeState> FrameNodeStateListModifyAsCollection = FocusNodeStateListModify as ICollection<IFrameNodeState>;
                Assert.That(FrameNodeStateListModifyAsCollection != null);
                Assert.That(!FrameNodeStateListModifyAsCollection.IsReadOnly);
                Assert.That(FrameNodeStateListModifyAsCollection.Contains(FirstNodeState));
                FrameNodeStateListModifyAsCollection.Remove(FirstNodeState);
                FrameNodeStateListModifyAsCollection.Add(FirstNodeState);
                FrameNodeStateListModifyAsCollection.Remove(FirstNodeState);
                FocusNodeStateListModify.Insert(0, FirstNodeState);
                FrameNodeStateListModifyAsCollection.CopyTo(new IFocusNodeState[FrameNodeStateListModifyAsCollection.Count], 0);
                IEnumerable<IFrameNodeState> FrameNodeStateListModifyAsEnumerable = FocusNodeStateListModify as IEnumerable<IFrameNodeState>;
                Assert.That(FrameNodeStateListModifyAsEnumerable != null);
                Assert.That(FrameNodeStateListModifyAsEnumerable.GetEnumerator() != null);

                // FocusNodeStateReadOnlyList

                FocusNodeStateReadOnlyList FocusNodeStateList = FocusNodeStateListModify.ToReadOnly() as FocusNodeStateReadOnlyList;
                Assert.That(FocusNodeStateList != null);
                Assert.That(FocusNodeStateList.Count > 0);
                FirstNodeState = FocusNodeStateList[0] as IFocusPlaceholderNodeState;

                Assert.That(FocusNodeStateList.Contains((IReadOnlyNodeState)FirstNodeState));
                Assert.That(FocusNodeStateList.IndexOf((IReadOnlyNodeState)FirstNodeState) == 0);
                IReadOnlyList<IReadOnlyNodeState> ReadOnlyNodeStateListAsIReadOnlyList = FocusNodeStateList as IReadOnlyList<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListAsIReadOnlyList[0] == FirstNodeState);
                IEnumerable<IReadOnlyNodeState> ReadOnlyNodeStateListAsEnumerable = FocusNodeStateList as IEnumerable<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListAsEnumerable != null);
                Assert.That(ReadOnlyNodeStateListAsEnumerable.GetEnumerator() != null);


                WriteableNodeStateReadOnlyList WriteableNodeStateList = FocusNodeStateList;
                Assert.That(WriteableNodeStateList.Contains(FirstNodeState));
                Assert.That(WriteableNodeStateList.IndexOf(FirstNodeState) == 0);
                Assert.That(WriteableNodeStateList[0] == FocusNodeStateList[0]);
                WriteableNodeStateList.GetEnumerator();
                IReadOnlyList<IWriteableNodeState> WriteableNodeStateListAsIReadOnlyList = FocusNodeStateList as IReadOnlyList<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListAsIReadOnlyList[0] == FirstNodeState);
                IEnumerable<IWriteableNodeState> WriteableNodeStateListAsEnumerable = FocusNodeStateList as IEnumerable<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListAsEnumerable != null);
                Assert.That(WriteableNodeStateListAsEnumerable.GetEnumerator() != null);


                FrameNodeStateReadOnlyList FrameNodeStateList = FocusNodeStateList;
                Assert.That(FrameNodeStateList.Contains(FirstNodeState));
                Assert.That(FrameNodeStateList.IndexOf(FirstNodeState) == 0);
                Assert.That(FrameNodeStateList[0] == FocusNodeStateList[0]);
                FrameNodeStateList.GetEnumerator();
                IReadOnlyList<IFrameNodeState> FrameNodeStateListAsIReadOnlyList = FocusNodeStateList as IReadOnlyList<IFrameNodeState>;
                Assert.That(FrameNodeStateListAsIReadOnlyList[0] == FirstNodeState);
                IEnumerable<IFrameNodeState> FrameNodeStateListAsEnumerable = FocusNodeStateList as IEnumerable<IFrameNodeState>;
                Assert.That(FrameNodeStateListAsEnumerable != null);
                Assert.That(FrameNodeStateListAsEnumerable.GetEnumerator() != null);

                // IFocusOperationGroupList

                FocusOperationGroupReadOnlyList FocusOperationStack = Controller.OperationStack;

                //System.Diagnostics.Debug.Assert(false);
                Assert.That(FocusOperationStack.Count > 0);
                FocusOperationGroup FirstOperationGroup = (FocusOperationGroup)FocusOperationStack[0];
                FocusOperationGroupList FocusOperationGroupList = DebugObjects.GetReferenceByInterface(typeof(FocusOperationGroupList)) as FocusOperationGroupList;
                if (FocusOperationGroupList != null)
                {
                    WriteableOperationGroupList WriteableOperationGroupList = FocusOperationGroupList;
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
                    WriteableOperationGroupAsICollection.CopyTo(new FocusOperationGroup[WriteableOperationGroupAsICollection.Count], 0);
                    IEnumerable<WriteableOperationGroup> WriteableOperationGroupAsIEnumerable = WriteableOperationGroupList;
                    WriteableOperationGroupAsIEnumerable.GetEnumerator();
                    IReadOnlyList<WriteableOperationGroup> WriteableOperationGroupAsIReadOnlyList = WriteableOperationGroupList;
                    Assert.That(WriteableOperationGroupAsIReadOnlyList.Count > 0);
                    Assert.That(WriteableOperationGroupAsIReadOnlyList[0] == FirstOperationGroup);

                    FrameOperationGroupList FrameOperationGroupList = FocusOperationGroupList;
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
                    FrameOperationGroupAsICollection.CopyTo(new FocusOperationGroup[FrameOperationGroupAsICollection.Count], 0);
                    IEnumerable<FrameOperationGroup> FrameOperationGroupAsIEnumerable = FrameOperationGroupList;
                    FrameOperationGroupAsIEnumerable.GetEnumerator();
                    IReadOnlyList<FrameOperationGroup> FrameOperationGroupAsIReadOnlyList = FrameOperationGroupList;
                    Assert.That(FrameOperationGroupAsIReadOnlyList.Count > 0);
                    Assert.That(FrameOperationGroupAsIReadOnlyList[0] == FirstOperationGroup);
                }

                // IFocusOperationGroupReadOnlyList

                WriteableOperationGroupReadOnlyList WriteableOperationStack = FocusOperationStack;
                Assert.That(WriteableOperationStack.Contains(FirstOperationGroup));
                Assert.That(WriteableOperationStack.IndexOf(FirstOperationGroup) == 0);
                IEnumerable<WriteableOperationGroup> WriteableOperationStackAsIEnumerable = WriteableOperationStack;
                WriteableOperationStackAsIEnumerable.GetEnumerator();


                FrameOperationGroupReadOnlyList FrameOperationStack = FocusOperationStack;
                Assert.That(FrameOperationStack.Contains(FirstOperationGroup));
                Assert.That(FrameOperationStack.IndexOf(FirstOperationGroup) == 0);
                Assert.That(FrameOperationStack[0] == FirstOperationGroup);
                FrameOperationStack.GetEnumerator();
                IEnumerable<FrameOperationGroup> FrameOperationStackAsIEnumerable = FrameOperationStack;
                FrameOperationStackAsIEnumerable.GetEnumerator();
                IReadOnlyList<FrameOperationGroup> FrameOperationStackAsIReadOnlyList = FrameOperationStack;
                Assert.That(FrameOperationStackAsIReadOnlyList[0] == FirstOperationGroup);

                // IFocusOperationList

                FocusOperationReadOnlyList FocusOperationReadOnlyList = FirstOperationGroup.OperationList;
                Assert.That(FocusOperationReadOnlyList.Count > 0);
                IFocusOperation FirstOperation = (IFocusOperation)FocusOperationReadOnlyList[0];
                FocusOperationList FocusOperationList = DebugObjects.GetReferenceByInterface(typeof(FocusOperationList)) as FocusOperationList;
                if (FocusOperationList != null)
                {
                    WriteableOperationList WriteableOperationList = FocusOperationList;
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
                    WriteableOperationAsICollection.CopyTo(new IFocusOperation[WriteableOperationAsICollection.Count], 0);
                    IEnumerable<IWriteableOperation> WriteableOperationAsIEnumerable = WriteableOperationList;
                    WriteableOperationAsIEnumerable.GetEnumerator();
                    IReadOnlyList<IWriteableOperation> WriteableOperationAsIReadOnlyList = WriteableOperationList;
                    Assert.That(WriteableOperationAsIReadOnlyList.Count > 0);
                    Assert.That(WriteableOperationAsIReadOnlyList[0] == FirstOperation);


                    FrameOperationList FrameOperationList = FocusOperationList;
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
                    FrameOperationAsICollection.CopyTo(new IFocusOperation[FrameOperationAsICollection.Count], 0);
                    IEnumerable<IFrameOperation> FrameOperationAsIEnumerable = FrameOperationList;
                    FrameOperationAsIEnumerable.GetEnumerator();
                    IReadOnlyList<IFrameOperation> FrameOperationAsIReadOnlyList = FrameOperationList;
                    Assert.That(FrameOperationAsIReadOnlyList.Count > 0);
                    Assert.That(FrameOperationAsIReadOnlyList[0] == FirstOperation);
                }

                // IFocusOperationReadOnlyList
                WriteableOperationReadOnlyList WriteableOperationReadOnlyList = FocusOperationReadOnlyList;
                Assert.That(WriteableOperationReadOnlyList.Contains(FirstOperation));
                Assert.That(WriteableOperationReadOnlyList.IndexOf(FirstOperation) == 0);
                IEnumerable<IWriteableOperation> WriteableOperationReadOnlyListAsIEnumerable = WriteableOperationReadOnlyList;
                WriteableOperationReadOnlyListAsIEnumerable.GetEnumerator();


                FrameOperationReadOnlyList FrameOperationReadOnlyList = FocusOperationReadOnlyList;
                Assert.That(FrameOperationReadOnlyList.Contains(FirstOperation));
                Assert.That(FrameOperationReadOnlyList.IndexOf(FirstOperation) == 0);
                Assert.That(FrameOperationReadOnlyList[0] == FirstOperation);
                FrameOperationReadOnlyList.GetEnumerator();
                IEnumerable<IFrameOperation> FrameOperationReadOnlyListAsIEnumerable = FrameOperationReadOnlyList;
                FrameOperationReadOnlyListAsIEnumerable.GetEnumerator();
                IReadOnlyList<IFrameOperation> FrameOperationReadOnlyListAsIReadOnlyList = FrameOperationReadOnlyList;
                Assert.That(FrameOperationReadOnlyListAsIReadOnlyList[0] == FirstOperation);

                // FocusPlaceholderNodeStateList

                FirstNodeState = LeafPathInner.FirstNodeState;
                Assert.That(FirstNodeState != null);
                FocusPlaceholderNodeStateList FocusPlaceholderNodeStateListModify = DebugObjects.GetReferenceByInterface(typeof(FocusPlaceholderNodeStateList)) as FocusPlaceholderNodeStateList;
                if (FocusPlaceholderNodeStateListModify != null)
                {
                    Assert.That(FocusPlaceholderNodeStateListModify.Count > 0);
                    FirstNodeState = FocusPlaceholderNodeStateListModify[0] as IFocusPlaceholderNodeState;

                    Assert.That(FocusPlaceholderNodeStateListModify.Contains((IReadOnlyPlaceholderNodeState)FirstNodeState));
                    Assert.That(FocusPlaceholderNodeStateListModify.IndexOf((IReadOnlyPlaceholderNodeState)FirstNodeState) == 0);
                    FocusPlaceholderNodeStateListModify.Remove((IReadOnlyPlaceholderNodeState)FirstNodeState);
                    FocusPlaceholderNodeStateListModify.Insert(0, (IReadOnlyPlaceholderNodeState)FirstNodeState);
                    FocusPlaceholderNodeStateListModify.CopyTo((IReadOnlyPlaceholderNodeState[])(new IFocusPlaceholderNodeState[FocusPlaceholderNodeStateListModify.Count]), 0);
                    ReadOnlyPlaceholderNodeStateList FocusPlaceholderNodeStateListModifyAsReadOnly = FocusPlaceholderNodeStateListModify as ReadOnlyPlaceholderNodeStateList;
                    Assert.That(FocusPlaceholderNodeStateListModifyAsReadOnly != null);
                    Assert.That(FocusPlaceholderNodeStateListModifyAsReadOnly[0] == FocusPlaceholderNodeStateListModify[0]);
                    IList<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListModifyAsIList = FocusPlaceholderNodeStateListModify as IList<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsIList != null);
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsIList[0] == FocusPlaceholderNodeStateListModify[0]);
                    IReadOnlyList<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListModifyAsIReadOnlyList = FocusPlaceholderNodeStateListModify as IReadOnlyList<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsIReadOnlyList != null);
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsIReadOnlyList[0] == FocusPlaceholderNodeStateListModify[0]);
                    ICollection<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListModifyAsCollection = FocusPlaceholderNodeStateListModify as ICollection<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsCollection != null);
                    Assert.That(!ReadOnlyPlaceholderNodeStateListModifyAsCollection.IsReadOnly);
                    ReadOnlyPlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    ReadOnlyPlaceholderNodeStateListModifyAsCollection.Add(FirstNodeState);
                    ReadOnlyPlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    ReadOnlyPlaceholderNodeStateListModifyAsIList.Insert(0, FirstNodeState);
                    IEnumerable<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListModifyAsEnumerable = FocusPlaceholderNodeStateListModify as IEnumerable<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsEnumerable != null);
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsEnumerable.GetEnumerator() != null);


                    WriteablePlaceholderNodeStateList FocusPlaceholderNodeStateListModifyAsWriteable = FocusPlaceholderNodeStateListModify as WriteablePlaceholderNodeStateList;
                    Assert.That(FocusPlaceholderNodeStateListModifyAsWriteable != null);
                    Assert.That(FocusPlaceholderNodeStateListModifyAsWriteable[0] == FocusPlaceholderNodeStateListModify[0]);
                    FocusPlaceholderNodeStateListModifyAsWriteable.GetEnumerator();
                    IList<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListModifyAsIList = FocusPlaceholderNodeStateListModify as IList<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsIList != null);
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsIList[0] == FocusPlaceholderNodeStateListModify[0]);
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsIList.IndexOf(FirstNodeState) == 0);
                    WriteablePlaceholderNodeStateListModifyAsIList.Remove(FirstNodeState);
                    WriteablePlaceholderNodeStateListModifyAsIList.Insert(0, FirstNodeState);
                    IReadOnlyList<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListModifyAsIReadOnlyList = FocusPlaceholderNodeStateListModify as IReadOnlyList<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsIReadOnlyList != null);
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsIReadOnlyList[0] == FocusPlaceholderNodeStateListModify[0]);
                    ICollection<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListModifyAsCollection = FocusPlaceholderNodeStateListModify as ICollection<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsCollection != null);
                    Assert.That(!WriteablePlaceholderNodeStateListModifyAsCollection.IsReadOnly);
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsCollection.Contains(FirstNodeState));
                    WriteablePlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    WriteablePlaceholderNodeStateListModifyAsCollection.Add(FirstNodeState);
                    WriteablePlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    FocusPlaceholderNodeStateListModify.Insert(0, FirstNodeState);
                    WriteablePlaceholderNodeStateListModifyAsCollection.CopyTo(new IFocusPlaceholderNodeState[WriteablePlaceholderNodeStateListModifyAsCollection.Count], 0);
                    IEnumerable<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListModifyAsEnumerable = FocusPlaceholderNodeStateListModify as IEnumerable<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsEnumerable != null);
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsEnumerable.GetEnumerator() != null);


                    FramePlaceholderNodeStateList FocusPlaceholderNodeStateListModifyAsFrame = FocusPlaceholderNodeStateListModify as FramePlaceholderNodeStateList;
                    Assert.That(FocusPlaceholderNodeStateListModifyAsFrame != null);
                    Assert.That(FocusPlaceholderNodeStateListModifyAsFrame[0] == FocusPlaceholderNodeStateListModify[0]);
                    FocusPlaceholderNodeStateListModifyAsFrame.GetEnumerator();
                    IList<IFramePlaceholderNodeState> FramePlaceholderNodeStateListModifyAsIList = FocusPlaceholderNodeStateListModify as IList<IFramePlaceholderNodeState>;
                    Assert.That(FramePlaceholderNodeStateListModifyAsIList != null);
                    Assert.That(FramePlaceholderNodeStateListModifyAsIList[0] == FocusPlaceholderNodeStateListModify[0]);
                    Assert.That(FramePlaceholderNodeStateListModifyAsIList.IndexOf(FirstNodeState) == 0);
                    FramePlaceholderNodeStateListModifyAsIList.Remove(FirstNodeState);
                    FramePlaceholderNodeStateListModifyAsIList.Insert(0, FirstNodeState);
                    IReadOnlyList<IFramePlaceholderNodeState> FramePlaceholderNodeStateListModifyAsIReadOnlyList = FocusPlaceholderNodeStateListModify as IReadOnlyList<IFramePlaceholderNodeState>;
                    Assert.That(FramePlaceholderNodeStateListModifyAsIReadOnlyList != null);
                    Assert.That(FramePlaceholderNodeStateListModifyAsIReadOnlyList[0] == FocusPlaceholderNodeStateListModify[0]);
                    ICollection<IFramePlaceholderNodeState> FramePlaceholderNodeStateListModifyAsCollection = FocusPlaceholderNodeStateListModify as ICollection<IFramePlaceholderNodeState>;
                    Assert.That(FramePlaceholderNodeStateListModifyAsCollection != null);
                    Assert.That(!FramePlaceholderNodeStateListModifyAsCollection.IsReadOnly);
                    Assert.That(FramePlaceholderNodeStateListModifyAsCollection.Contains(FirstNodeState));
                    FramePlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    FramePlaceholderNodeStateListModifyAsCollection.Add(FirstNodeState);
                    FramePlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    FocusPlaceholderNodeStateListModify.Insert(0, FirstNodeState);
                    FramePlaceholderNodeStateListModifyAsCollection.CopyTo(new IFocusPlaceholderNodeState[FramePlaceholderNodeStateListModifyAsCollection.Count], 0);
                    IEnumerable<IFramePlaceholderNodeState> FramePlaceholderNodeStateListModifyAsEnumerable = FocusPlaceholderNodeStateListModify as IEnumerable<IFramePlaceholderNodeState>;
                    Assert.That(FramePlaceholderNodeStateListModifyAsEnumerable != null);
                    Assert.That(FramePlaceholderNodeStateListModifyAsEnumerable.GetEnumerator() != null);
                }

                // FocusPlaceholderNodeStateReadOnlyList

                FocusPlaceholderNodeStateReadOnlyList FocusPlaceholderNodeStateList = FocusPlaceholderNodeStateListModify != null ? FocusPlaceholderNodeStateListModify.ToReadOnly() as FocusPlaceholderNodeStateReadOnlyList : null;
                if (FocusPlaceholderNodeStateList != null)
                {
                    Assert.That(FocusPlaceholderNodeStateList.Count > 0);
                    FirstNodeState = FocusPlaceholderNodeStateList[0] as IFocusPlaceholderNodeState;

                    Assert.That(FocusPlaceholderNodeStateList.Contains((IReadOnlyPlaceholderNodeState)FirstNodeState));
                    Assert.That(FocusPlaceholderNodeStateList.IndexOf((IReadOnlyPlaceholderNodeState)FirstNodeState) == 0);
                    IReadOnlyList<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListAsIReadOnlyList = FocusPlaceholderNodeStateList as IReadOnlyList<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListAsIReadOnlyList[0] == FirstNodeState);
                    IEnumerable<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListAsEnumerable = FocusPlaceholderNodeStateList as IEnumerable<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListAsEnumerable != null);
                    Assert.That(ReadOnlyPlaceholderNodeStateListAsEnumerable.GetEnumerator() != null);


                    WriteablePlaceholderNodeStateReadOnlyList WriteablePlaceholderNodeStateList = FocusPlaceholderNodeStateList;
                    Assert.That(WriteablePlaceholderNodeStateList.Contains(FirstNodeState));
                    Assert.That(WriteablePlaceholderNodeStateList.IndexOf(FirstNodeState) == 0);
                    Assert.That(WriteablePlaceholderNodeStateList[0] == FocusPlaceholderNodeStateList[0]);
                    WriteablePlaceholderNodeStateList.GetEnumerator();
                    IReadOnlyList<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListAsIReadOnlyList = FocusPlaceholderNodeStateList as IReadOnlyList<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListAsIReadOnlyList[0] == FirstNodeState);
                    IEnumerable<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListAsEnumerable = FocusPlaceholderNodeStateList as IEnumerable<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListAsEnumerable != null);
                    Assert.That(WriteablePlaceholderNodeStateListAsEnumerable.GetEnumerator() != null);


                    FramePlaceholderNodeStateReadOnlyList FramePlaceholderNodeStateList = FocusPlaceholderNodeStateList;
                    Assert.That(FramePlaceholderNodeStateList.Contains(FirstNodeState));
                    Assert.That(FramePlaceholderNodeStateList.IndexOf(FirstNodeState) == 0);
                    Assert.That(FramePlaceholderNodeStateList[0] == FocusPlaceholderNodeStateList[0]);
                    FramePlaceholderNodeStateList.GetEnumerator();
                    IReadOnlyList<IFramePlaceholderNodeState> FramePlaceholderNodeStateListAsIReadOnlyList = FocusPlaceholderNodeStateList as IReadOnlyList<IFramePlaceholderNodeState>;
                    Assert.That(FramePlaceholderNodeStateListAsIReadOnlyList[0] == FirstNodeState);
                    IEnumerable<IFramePlaceholderNodeState> FramePlaceholderNodeStateListAsEnumerable = FocusPlaceholderNodeStateList as IEnumerable<IFramePlaceholderNodeState>;
                    Assert.That(FramePlaceholderNodeStateListAsEnumerable != null);
                    Assert.That(FramePlaceholderNodeStateListAsEnumerable.GetEnumerator() != null);
                }

                // IFocusStateViewDictionary
                FocusNodeStateViewDictionary FocusStateViewTable = ControllerView.StateViewTable;
                WriteableNodeStateViewDictionary WriteableStateViewTable = ControllerView.StateViewTable;
                WriteableStateViewTable.GetEnumerator();
                FrameNodeStateViewDictionary FrameStateViewTable = ControllerView.StateViewTable;
                FrameStateViewTable.GetEnumerator();

                IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView> ReadOnlyStateViewTableAsDictionary = FocusStateViewTable;
                Assert.That(ReadOnlyStateViewTableAsDictionary != null);
                Assert.That(ReadOnlyStateViewTableAsDictionary.TryGetValue(RootState, out IReadOnlyNodeStateView StateViewTableAsDictionaryValue) == FocusStateViewTable.TryGetValue(RootState, out IReadOnlyNodeStateView StateViewTableValue));
                Assert.That(ReadOnlyStateViewTableAsDictionary.Keys != null);
                Assert.That(ReadOnlyStateViewTableAsDictionary.Values != null);
                ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> ReadOnlyStateViewTableAsCollection = FocusStateViewTable;
                Assert.That(!ReadOnlyStateViewTableAsCollection.IsReadOnly);
                foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in ReadOnlyStateViewTableAsCollection)
                {
                    Assert.That(ReadOnlyStateViewTableAsCollection.Contains(Entry));
                    ReadOnlyStateViewTableAsCollection.Remove(Entry);
                    ReadOnlyStateViewTableAsCollection.Add(Entry);
                    ReadOnlyStateViewTableAsCollection.CopyTo(new KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>[FocusStateViewTable.Count], 0);
                    break;
                }


                ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> WriteableStateViewTableAsCollection = FocusStateViewTable;
                Assert.That(!WriteableStateViewTableAsCollection.IsReadOnly);
                IDictionary<IWriteableNodeState, IWriteableNodeStateView> WriteableStateViewTableAsDictionary = FocusStateViewTable;
                Assert.That(WriteableStateViewTableAsDictionary != null);
                Assert.That(WriteableStateViewTableAsDictionary.TryGetValue(RootState, out IWriteableNodeStateView WriteableStateViewTableAsDictionaryValue) == FocusStateViewTable.TryGetValue(RootState, out StateViewTableValue));
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
                    WriteableStateViewTableAsCollection.CopyTo(new KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>[FocusStateViewTable.Count], 0);

                    break;
                }
                IEnumerable<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> WriteableStateViewTableAsEnumerable = FocusStateViewTable;
                WriteableStateViewTableAsEnumerable.GetEnumerator();


                ICollection<KeyValuePair<IFrameNodeState, IFrameNodeStateView>> FrameStateViewTableAsCollection = FocusStateViewTable;
                Assert.That(!FrameStateViewTableAsCollection.IsReadOnly);
                IDictionary<IFrameNodeState, IFrameNodeStateView> FrameStateViewTableAsDictionary = FocusStateViewTable;
                Assert.That(FrameStateViewTableAsDictionary != null);
                Assert.That(FrameStateViewTableAsDictionary.TryGetValue(RootState, out IFrameNodeStateView FrameStateViewTableAsDictionaryValue) == FocusStateViewTable.TryGetValue(RootState, out StateViewTableValue));
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
                    FrameStateViewTableAsCollection.CopyTo(new KeyValuePair<IFrameNodeState, IFrameNodeStateView>[FocusStateViewTable.Count], 0);

                    break;
                }
                IEnumerable<KeyValuePair<IFrameNodeState, IFrameNodeStateView>> FrameStateViewTableAsEnumerable = FocusStateViewTable;
                FrameStateViewTableAsEnumerable.GetEnumerator();
            }

            IFocusTemplateSet FocusTemplateSet = TestDebug.CoverageFocusTemplateSet.FocusTemplateSet;
            using (FocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet))
            {
                // IFocusAssignableCellViewDictionary

                FocusAssignableCellViewDictionary<string> ActualCellViewTable = DebugObjects.GetReferenceByInterface(typeof(FocusAssignableCellViewDictionary<string>)) as FocusAssignableCellViewDictionary<string>;
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
                        Assert.That(FrameActualCellViewTable.TryGetValue(Entry.Key, out IFrameAssignableCellView FrameCellView) == ActualCellViewTable.TryGetValue(Entry.Key, out IFrameAssignableCellView FocusCellView));
                        Assert.That(FrameActualCellViewTableAsCollection.Contains(Entry));
                        FrameActualCellViewTableAsCollection.Remove(Entry);
                        FrameActualCellViewTableAsCollection.Add(Entry);
                        break;
                    }

                    // IFocusAssignableCellViewReadOnlyDictionary

                    FocusAssignableCellViewReadOnlyDictionary<string> FocusActualCellViewTableReadOnly = ActualCellViewTable.ToReadOnly() as FocusAssignableCellViewReadOnlyDictionary<string>;

                    IReadOnlyDictionary<string, IFrameAssignableCellView> FrameActualCellViewTableReadOnlyAsDictionary = FocusActualCellViewTableReadOnly;
                    Assert.That(FrameActualCellViewTableReadOnlyAsDictionary.Keys != null);
                    Assert.That(FrameActualCellViewTableReadOnlyAsDictionary.Values != null);
                    foreach (KeyValuePair<string, IFrameAssignableCellView> Entry in FrameActualCellViewTableReadOnlyAsDictionary)
                    {
                        Assert.That(FrameActualCellViewTableReadOnlyAsDictionary.TryGetValue(Entry.Key, out IFrameAssignableCellView FrameCellView) == ActualCellViewTable.TryGetValue(Entry.Key, out IFrameAssignableCellView FocusCellView));
                        break;
                    }

                    // FocusCellViewList

                    Assert.That(ActualCellViewTable.ContainsKey("LeafBlocks"));
                    IFocusCellViewCollection CellViewCollection = ActualCellViewTable["LeafBlocks"] as IFocusCellViewCollection;
                    Assert.That(CellViewCollection != null);
                    FocusCellViewList CellViewList = CellViewCollection.CellViewList;
                    Assert.That(CellViewList.Count > 0);
                    IFocusCellView FirstCellView = (IFocusCellView)CellViewList[0];

                    FrameCellViewList FrameCellViewList = CellViewList;
                    IList<IFrameCellView> FrameCellViewListAsList = FrameCellViewList;
                    Assert.That(FrameCellViewListAsList[0] == FirstCellView);
                    ICollection<IFrameCellView> FrameCellViewListAsCollection = FrameCellViewList;
                    FrameCellViewListAsCollection.CopyTo(new IFocusCellView[FrameCellViewListAsCollection.Count], 0);
                    Assert.That(!FrameCellViewListAsCollection.IsReadOnly);
                    FrameCellViewListAsCollection.Remove(FirstCellView);
                    FrameCellViewListAsCollection.Add(FirstCellView);
                    FrameCellViewListAsCollection.Remove(FirstCellView);
                    CellViewList.Insert(0, FirstCellView);
                    IReadOnlyList<IFrameCellView> FrameCellViewListAsReadOnlyList = FrameCellViewList;
                    Assert.That(FrameCellViewListAsReadOnlyList[0] == FirstCellView);

                    // IFocusFrameList 

                    IFocusHorizontalPanelFrame HorizontalPanelFrame = CellViewCollection.StateView.Template.Root as IFocusHorizontalPanelFrame;
                    Assert.That(HorizontalPanelFrame != null);
                    FocusFrameList FrameList = HorizontalPanelFrame.Items;
                    Assert.That(FrameList.Count > 0);
                    IFocusFrame FirstFrame = (IFocusFrame)FrameList[0];

                    //System.Diagnostics.Debug.Assert(false);
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
                    FrameFrameListAsCollection.CopyTo(new IFocusFrame[FrameFrameListAsCollection.Count], 0);
                    IReadOnlyList<IFrameFrame> FrameFrameListAsReadOnlyList = FrameFrameList;
                    Assert.That(FrameFrameListAsReadOnlyList[0] == FirstFrame);

                    // IFocusKeywordFrameList

                    IFocusDiscreteFrame FirstDiscreteFrame = null;
                    foreach (IFocusFrame Item in FrameList)
                        if (Item is IFocusDiscreteFrame)
                        {
                            FirstDiscreteFrame = Item as IFocusDiscreteFrame;
                            break;
                        }
                    Assert.That(FirstDiscreteFrame != null);
                    FocusKeywordFrameList KeywordFrameList  = FirstDiscreteFrame.Items;
                    Assert.That(KeywordFrameList.Count > 0);
                    IFocusKeywordFrame FirstKeywordFrame = (IFocusKeywordFrame)KeywordFrameList[0];


                    FrameKeywordFrameList FrameKeywordFrameList = KeywordFrameList;
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
                    FrameKeywordFrameListAsCollection.CopyTo(new IFocusKeywordFrame[FrameKeywordFrameListAsCollection.Count], 0);
                    IReadOnlyList<IFrameKeywordFrame> FrameKeywordFrameListAsReadOnlyList = FrameKeywordFrameList;
                    Assert.That(FrameKeywordFrameListAsReadOnlyList[0] == FirstKeywordFrame);
                }

                // IFocusVisibleCellViewList

                FocusVisibleCellViewList VisibleCellViewList = new FocusVisibleCellViewList();
                ControllerView.EnumerateVisibleCellViews((IFrameVisibleCellView item) => ListCellViews(item, VisibleCellViewList), out IFrameVisibleCellView FoundCellView, false);
                Assert.That(VisibleCellViewList.Count> 0);
                IFocusVisibleCellView FirstVisibleCellView = (IFocusVisibleCellView)VisibleCellViewList[0];

                FrameVisibleCellViewList FrameVisibleCellViewList = VisibleCellViewList;
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
                FrameVisibleCellViewListAsCollection.CopyTo(new IFocusVisibleCellView[FrameVisibleCellViewListAsCollection.Count], 0);
                IEnumerable<IFrameVisibleCellView> FrameVisibleCellViewListAsEnumerable = FrameVisibleCellViewList;
                FrameVisibleCellViewListAsEnumerable.GetEnumerator();
                IReadOnlyList<IFrameVisibleCellView> FrameVisibleCellViewListAsReadOnlyList = FrameVisibleCellViewList;
                Assert.That(FrameVisibleCellViewListAsReadOnlyList[0] == FirstVisibleCellView);
            }

            // IFocusTemplateDictionary

            FocusTemplateDictionary NodeTemplateDictionary = TestDebug.CoverageFocusTemplateSet.NodeTemplateDictionary;
            Assert.That(NodeTemplateDictionary.ContainsKey(typeof(Leaf)));
            IFocusTemplate LeafTemplate = (IFocusTemplate)NodeTemplateDictionary[typeof(Leaf)];

            FrameTemplateDictionary FrameNodeTemplateDictionary = NodeTemplateDictionary;
            IDictionary<Type, IFrameTemplate> FrameNodeTemplateDictionaryAsDictionary = FrameNodeTemplateDictionary;
            Assert.That(FrameNodeTemplateDictionaryAsDictionary.Keys != null);
            Assert.That(FrameNodeTemplateDictionaryAsDictionary.Values != null);
            Assert.That(FrameNodeTemplateDictionaryAsDictionary.ContainsKey(typeof(Leaf)));
            FrameNodeTemplateDictionaryAsDictionary.Remove(typeof(Leaf));
            FrameNodeTemplateDictionaryAsDictionary.Add(typeof(Leaf), LeafTemplate);
            Assert.That(FrameNodeTemplateDictionaryAsDictionary.TryGetValue(typeof(Leaf), out IFrameTemplate AsFrameTemplate) == NodeTemplateDictionary.TryGetValue(typeof(Leaf), out IFrameTemplate AsFocusTemplate));
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

            // IFocusTemplateReadOnlyDictionary

            FocusTemplateReadOnlyDictionary NodeTemplateDictionaryReadOnly = FocusTemplateSet.NodeTemplateTable;
            FocusTemplateReadOnlyDictionary BlockTemplateTableReadOnly = FocusTemplateSet.BlockTemplateTable;

            FrameTemplateReadOnlyDictionary FrameNodeTemplateDictionaryReadOnly = NodeTemplateDictionaryReadOnly;
            IReadOnlyDictionary<Type, IFrameTemplate> FrameNodeTemplateDictionaryReadOnlyAsDictionary = FrameNodeTemplateDictionaryReadOnly;
            Assert.That(FrameNodeTemplateDictionaryReadOnlyAsDictionary.ContainsKey(typeof(Leaf)));
            Assert.That(FrameNodeTemplateDictionaryReadOnlyAsDictionary.Keys != null);
            Assert.That(FrameNodeTemplateDictionaryReadOnlyAsDictionary.Values != null);
            Assert.That(FrameNodeTemplateDictionaryReadOnlyAsDictionary.TryGetValue(typeof(Leaf), out AsFrameTemplate) == NodeTemplateDictionary.TryGetValue(typeof(Leaf), out AsFocusTemplate));

            // IFocusTemplateList 

            FocusTemplateList TemplateList = TestDebug.CoverageFocusTemplateSet.Templates;
            Assert.That(TemplateList.Count > 0);
            IFocusTemplate FirstTemplate = (IFocusTemplate)TemplateList[0];

            FrameTemplateList FrameTemplateList = TemplateList;
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
            FrameTemplateListAsCollection.CopyTo(new IFocusTemplate[FrameTemplateListAsCollection.Count], 0);
            IEnumerable<IFrameTemplate> FrameTemplateListAsEnumerable = FrameTemplateList;
            FrameTemplateListAsEnumerable.GetEnumerator();
            IReadOnlyList<IFrameTemplate> FrameTemplateListAsReadOnlyList = FrameTemplateList;
            Assert.That(FrameTemplateListAsReadOnlyList[0] == FirstTemplate);
        }
    }
}
