using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
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
        public static void FrameCreation()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;
            FrameController Controller;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

            try
            {
                RootIndex = new FrameRootNodeIndex(RootNode);
                Controller = FrameController.Create(RootIndex);
            }
            catch (Exception e)
            {
                Assert.Fail($"#0: {e}");
            }

            RootNode = CreateRoot(ValueGuid0, Imperfections.BadGuid);
            Assert.That(!BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode, throwOnInvalid: false));

            try
            {
                RootIndex = new FrameRootNodeIndex(RootNode);
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
        public static void FrameProperties()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex0;
            IFrameRootNodeIndex RootIndex1;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

            RootIndex0 = new FrameRootNodeIndex(RootNode);
            Assert.That(RootIndex0.Node == RootNode);
            Assert.That(RootIndex0.IsEqual(CompareEqual.New(), RootIndex0));

            RootIndex1 = new FrameRootNodeIndex(RootNode);
            Assert.That(RootIndex1.Node == RootNode);
            Assert.That(CompareEqual.CoverIsEqual(RootIndex0, RootIndex1));

            FrameController Controller0 = FrameController.Create(RootIndex0);
            Assert.That(Controller0.RootIndex == RootIndex0);

            Stats Stats = Controller0.Stats;
            Assert.That(Stats.NodeCount >= 0);
            Assert.That(Stats.PlaceholderNodeCount >= 0);
            Assert.That(Stats.OptionalNodeCount >= 0);
            Assert.That(Stats.AssignedOptionalNodeCount >= 0);
            Assert.That(Stats.ListCount >= 0);
            Assert.That(Stats.BlockListCount >= 0);
            Assert.That(Stats.BlockCount >= 0);

            IFramePlaceholderNodeState RootState = Controller0.RootState;
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

            IFramePlaceholderInner MainPlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IFramePlaceholderInner;
            Assert.That(MainPlaceholderTreeInner != null);
            Assert.That(MainPlaceholderTreeInner.InterfaceType.IsTypeof<Tree>());
            Assert.That(MainPlaceholderTreeInner.ChildState != null);
            Assert.That(MainPlaceholderTreeInner.ChildState.ParentInner == MainPlaceholderTreeInner);

            IFramePlaceholderInner MainPlaceholderLeafInner = RootState.PropertyToInner(nameof(Main.PlaceholderLeaf)) as IFramePlaceholderInner;
            Assert.That(MainPlaceholderLeafInner != null);
            Assert.That(MainPlaceholderLeafInner.InterfaceType.IsTypeof<Leaf>());
            Assert.That(MainPlaceholderLeafInner.ChildState != null);
            Assert.That(MainPlaceholderLeafInner.ChildState.ParentInner == MainPlaceholderLeafInner);

            IFrameOptionalInner MainUnassignedOptionalInner = RootState.PropertyToInner(nameof(Main.UnassignedOptionalLeaf)) as IFrameOptionalInner;
            Assert.That(MainUnassignedOptionalInner != null);
            Assert.That(MainUnassignedOptionalInner.InterfaceType.IsTypeof<Leaf>());
            Assert.That(!MainUnassignedOptionalInner.IsAssigned);
            Assert.That(MainUnassignedOptionalInner.ChildState != null);
            Assert.That(MainUnassignedOptionalInner.ChildState.ParentInner == MainUnassignedOptionalInner);

            IFrameOptionalInner MainAssignedOptionalTreeInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalTree)) as IFrameOptionalInner;
            Assert.That(MainAssignedOptionalTreeInner != null);
            Assert.That(MainAssignedOptionalTreeInner.InterfaceType.IsTypeof<Tree>());
            Assert.That(MainAssignedOptionalTreeInner.IsAssigned);

            IFrameNodeState AssignedOptionalTreeState = MainAssignedOptionalTreeInner.ChildState;
            Assert.That(AssignedOptionalTreeState != null);
            Assert.That(AssignedOptionalTreeState.ParentInner == MainAssignedOptionalTreeInner);
            Assert.That(AssignedOptionalTreeState.ParentState == RootState);

            FrameNodeStateReadOnlyList AssignedOptionalTreeAllChildren = AssignedOptionalTreeState.GetAllChildren() as FrameNodeStateReadOnlyList;
            Assert.That(AssignedOptionalTreeAllChildren != null);
            Assert.That(AssignedOptionalTreeAllChildren.Count == 2, $"New count: {AssignedOptionalTreeAllChildren.Count}");

            IFrameOptionalInner MainAssignedOptionalLeafInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IFrameOptionalInner;
            Assert.That(MainAssignedOptionalLeafInner != null);
            Assert.That(MainAssignedOptionalLeafInner.InterfaceType.IsTypeof<Leaf>());
            Assert.That(MainAssignedOptionalLeafInner.IsAssigned);
            Assert.That(MainAssignedOptionalLeafInner.ChildState != null);
            Assert.That(MainAssignedOptionalLeafInner.ChildState.ParentInner == MainAssignedOptionalLeafInner);

            IFrameBlockListInner MainLeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFrameBlockListInner;
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

            IFrameBlockState LeafBlock = (IFrameBlockState)MainLeafBlocksInner.BlockStateList[0];
            Assert.That(LeafBlock != null);
            Assert.That(LeafBlock.StateList != null);
            Assert.That(LeafBlock.StateList.Count == 1);
            Assert.That(MainLeafBlocksInner.FirstNodeState == LeafBlock.StateList[0]);
            Assert.That(MainLeafBlocksInner.IndexAt(0, 0) == MainLeafBlocksInner.FirstNodeState.ParentIndex);

            IFramePlaceholderInner PatternInner = LeafBlock.PropertyToInner(nameof(BaseNode.IBlock.ReplicationPattern)) as IFramePlaceholderInner;
            Assert.That(PatternInner != null);

            IFramePlaceholderInner SourceInner = LeafBlock.PropertyToInner(nameof(BaseNode.IBlock.SourceIdentifier)) as IFramePlaceholderInner;
            Assert.That(SourceInner != null);

            IFramePatternState PatternState = LeafBlock.PatternState;
            Assert.That(PatternState != null);
            Assert.That(PatternState.ParentBlockState == LeafBlock);
            Assert.That(PatternState.ParentInner == PatternInner);
            Assert.That(PatternState.ParentIndex == LeafBlock.PatternIndex);
            Assert.That(PatternState.ParentState == RootState);
            Assert.That(PatternState.InnerTable.Count == 0);
            Assert.That(PatternState is IFrameNodeState AsPlaceholderPatternNodeState && AsPlaceholderPatternNodeState.ParentIndex == LeafBlock.PatternIndex);
            Assert.That(PatternState.GetAllChildren().Count == 1);

            IFrameSourceState SourceState = LeafBlock.SourceState;
            Assert.That(SourceState != null);
            Assert.That(SourceState.ParentBlockState == LeafBlock);
            Assert.That(SourceState.ParentInner == SourceInner);
            Assert.That(SourceState.ParentIndex == LeafBlock.SourceIndex);
            Assert.That(SourceState.ParentState == RootState);
            Assert.That(SourceState.InnerTable.Count == 0);
            Assert.That(SourceState is IFrameNodeState AsPlaceholderSourceNodeState && AsPlaceholderSourceNodeState.ParentIndex == LeafBlock.SourceIndex);
            Assert.That(SourceState.GetAllChildren().Count == 1);

            Assert.That(MainLeafBlocksInner.FirstNodeState == LeafBlock.StateList[0]);

            IFrameListInner MainLeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFrameListInner;
            Assert.That(MainLeafPathInner != null);
            Assert.That(!MainLeafPathInner.IsNeverEmpty);
            Assert.That(MainLeafPathInner.InterfaceType.IsTypeof<Leaf>());
            Assert.That(MainLeafPathInner.Count == 2);
            Assert.That(MainLeafPathInner.StateList != null);
            Assert.That(MainLeafPathInner.StateList.Count == 2);
            Assert.That(MainLeafPathInner.FirstNodeState == MainLeafPathInner.StateList[0]);
            Assert.That(MainLeafPathInner.IndexAt(0) == MainLeafPathInner.FirstNodeState.ParentIndex);
            Assert.That(MainLeafPathInner.AllIndexes().Count == MainLeafPathInner.Count);

            FrameNodeStateReadOnlyList AllChildren = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
            Assert.That(AllChildren.Count == 19, $"New count: {AllChildren.Count}");

            IFramePlaceholderInner PlaceholderInner = RootState.InnerTable[nameof(Main.PlaceholderLeaf)] as IFramePlaceholderInner;
            Assert.That(PlaceholderInner != null);

            IFrameBrowsingPlaceholderNodeIndex PlaceholderNodeIndex = PlaceholderInner.ChildState.ParentIndex as IFrameBrowsingPlaceholderNodeIndex;
            Assert.That(PlaceholderNodeIndex != null);
            Assert.That(Controller0.Contains(PlaceholderNodeIndex));

            IFrameOptionalInner UnassignedOptionalInner = RootState.InnerTable[nameof(Main.UnassignedOptionalLeaf)] as IFrameOptionalInner;
            Assert.That(UnassignedOptionalInner != null);

            IFrameBrowsingOptionalNodeIndex UnassignedOptionalNodeIndex = UnassignedOptionalInner.ChildState.ParentIndex;
            Assert.That(UnassignedOptionalNodeIndex != null);
            Assert.That(Controller0.Contains(UnassignedOptionalNodeIndex));
            Assert.That(Controller0.IsAssigned(UnassignedOptionalNodeIndex) == false);

            IFrameOptionalInner AssignedOptionalInner = RootState.InnerTable[nameof(Main.AssignedOptionalLeaf)] as IFrameOptionalInner;
            Assert.That(AssignedOptionalInner != null);

            IFrameBrowsingOptionalNodeIndex AssignedOptionalNodeIndex = AssignedOptionalInner.ChildState.ParentIndex;
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

            FrameController Controller1 = FrameController.Create(RootIndex0);
            Assert.That(Controller0.IsEqual(CompareEqual.New(), Controller0));

            //System.Diagnostics.Debug.Assert(false);
            Assert.That(CompareEqual.CoverIsEqual(Controller0, Controller1));

            Assert.That(!Controller0.CanUndo);
            Assert.That(!Controller0.CanRedo);
        }

        [Test]
        [Category("Coverage")]
        public static void FrameClone()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode = CreateRoot(ValueGuid0, Imperfections.None);

            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(RootNode);
            Assert.That(RootIndex != null);

            FrameController Controller = FrameController.Create(RootIndex);
            Assert.That(Controller != null);

            IFramePlaceholderNodeState RootState = Controller.RootState;
            Assert.That(RootState != null);

            BaseNode.Node ClonedNode = RootState.CloneNode();
            Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(ClonedNode));

            IFrameRootNodeIndex CloneRootIndex = new FrameRootNodeIndex(ClonedNode);
            Assert.That(CloneRootIndex != null);

            FrameController CloneController = FrameController.Create(CloneRootIndex);
            Assert.That(CloneController != null);

            IFramePlaceholderNodeState CloneRootState = Controller.RootState;
            Assert.That(CloneRootState != null);

            FrameNodeStateReadOnlyList AllChildren = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
            FrameNodeStateReadOnlyList CloneAllChildren = (FrameNodeStateReadOnlyList)CloneRootState.GetAllChildren();
            Assert.That(AllChildren.Count == CloneAllChildren.Count);
        }

        [Test]
        [Category("Coverage")]
        public static void FrameViews()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController Controller = FrameController.Create(RootIndex);
            IFrameTemplateSet DefaultTemplateSet = FrameTemplateSet.Default;
            DefaultTemplateSet = FrameTemplateSet.Default;

            IFrameTemplateSet FrameCustomTemplateSet = TestDebug.CoverageFrameTemplateSet.FrameTemplateSet;

            //System.Diagnostics.Debug.Assert(false);
            foreach (KeyValuePair<Type, IFrameTemplate> TemplateEntry in FrameCustomTemplateSet.NodeTemplateTable)
                if (TemplateEntry.Key.IsTypeof<Leaf>())
                {
                    IFrameNodeTemplate Template = TemplateEntry.Value as IFrameNodeTemplate;
                    Assert.That(Template != null);

                    Template.PropertyToFrame("Text");
                    Template.GetCommentFrame();
                    break;
                }

            FrameCustomTemplateSet.PropertyToFrame(Controller.RootState, "PlaceholderTree");
            FrameCustomTemplateSet.GetCommentFrame(Controller.RootState);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);
                Assert.That(ControllerView0.RootStateView == ControllerView0.StateViewTable[Controller.RootState]);

                using (FrameControllerView ControllerView1 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
                {
                    Assert.That(ControllerView0.IsEqual(CompareEqual.New(), ControllerView0));
                    Assert.That(CompareEqual.CoverIsEqual(ControllerView0, ControllerView1));
                }

                foreach (IFrameBlockState BlockState in ControllerView0.BlockStateViewTable.Keys)
                {
                    Assert.That(BlockState != null);

                    FrameBlockStateView BlockStateView = (FrameBlockStateView)ControllerView0.BlockStateViewTable[BlockState];
                    Assert.That(BlockStateView != null);
                    Assert.That(BlockStateView.BlockState == BlockState);

                    Assert.That(BlockStateView.ControllerView == ControllerView0);
                }

                foreach (IFrameNodeState State in ControllerView0.StateViewTable.Keys)
                {
                    Assert.That(State != null);

                    IFrameNodeStateView StateView = (IFrameNodeStateView)ControllerView0.StateViewTable[State];
                    Assert.That(StateView != null);
                    Assert.That(StateView.State == State);

                    IFrameIndex ParentIndex = State.ParentIndex;
                    Assert.That(ParentIndex != null);

                    Assert.That(Controller.Contains(ParentIndex));
                    Assert.That(StateView.ControllerView == ControllerView0);

                    switch (StateView)
                    {
                        case FramePatternStateView AsPatternStateView:
                            Assert.That(AsPatternStateView.State == State);
                            Assert.That(AsPatternStateView is IFrameNodeStateView AsPlaceholderPatternNodeStateView && AsPlaceholderPatternNodeStateView.State == State);
                            break;

                        case FrameSourceStateView AsSourceStateView:
                            Assert.That(AsSourceStateView.State == State);
                            Assert.That(AsSourceStateView is IFrameNodeStateView AsPlaceholderSourceNodeStateView && AsPlaceholderSourceNodeStateView.State == State);
                            break;

                        case FramePlaceholderNodeStateView AsPlaceholderNodeStateView:
                            Assert.That(AsPlaceholderNodeStateView.State == State);
                            break;

                        case FrameOptionalNodeStateView AsOptionalNodeStateView:
                            Assert.That(AsOptionalNodeStateView.State == State);
                            break;
                    }
                }

                FrameVisibleCellViewList VisibleCellViewList = new FrameVisibleCellViewList();
                ControllerView0.EnumerateVisibleCellViews((IFrameVisibleCellView item) => ListCellViews(item, VisibleCellViewList), out IFrameVisibleCellView FoundCellView, false);
                ControllerView0.PrintCellViewTree(true);

                ControllerView0.SetCommentDisplayMode(CommentDisplayModes.All);
                VisibleCellViewList.Clear();
                ControllerView0.EnumerateVisibleCellViews((IFrameVisibleCellView item) => ListCellViews(item, VisibleCellViewList), out FoundCellView, false);
                ControllerView0.PrintCellViewTree(true);

                //System.Diagnostics.Debug.Assert(false);
                foreach (IFrameVisibleCellView CellView in VisibleCellViewList)
                    Assert.That(CompareEqual.CoverIsEqual(CellView, CellView));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void FrameInsert()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFrameListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFrameListInner;
                Assert.That(LeafPathInner != null);

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                IFrameBrowsingListNodeIndex ExistingIndex = LeafPathInner.IndexAt(0) as IFrameBrowsingListNodeIndex;

                Leaf NewItem0 = CreateLeaf(Guid.NewGuid());

                IFrameInsertionListNodeIndex InsertionIndex0;
                InsertionIndex0 = ExistingIndex.ToInsertionIndex(RootNode, NewItem0) as IFrameInsertionListNodeIndex;
                Assert.That(InsertionIndex0.ParentNode == RootNode);
                Assert.That(InsertionIndex0.Node == NewItem0);
                Assert.That(CompareEqual.CoverIsEqual(InsertionIndex0, InsertionIndex0));

                FrameNodeStateReadOnlyList AllChildren0 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Insert(LeafPathInner, InsertionIndex0, out IWriteableBrowsingCollectionNodeIndex NewItemIndex0);
                Assert.That(Controller.Contains(NewItemIndex0));

                IFrameBrowsingListNodeIndex DuplicateExistingIndex0 = InsertionIndex0.ToBrowsingIndex() as IFrameBrowsingListNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex0 as IFrameBrowsingListNodeIndex, DuplicateExistingIndex0));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex0, NewItemIndex0 as IFrameBrowsingListNodeIndex));

                Assert.That(LeafPathInner.Count == PathCount + 1);
                Assert.That(LeafPathInner.StateList.Count == PathCount + 1);

                IFramePlaceholderNodeState NewItemState0 = (IFramePlaceholderNodeState)LeafPathInner.StateList[0];
                Assert.That(NewItemState0.Node == NewItem0);
                Assert.That(NewItemState0.ParentIndex == NewItemIndex0);

                FrameNodeStateReadOnlyList AllChildren1 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                IFrameBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFrameBlockListInner;
                Assert.That(LeafBlocksInner != null);

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[0].StateList.Count;
                Assert.That(BlockNodeCount == 4);

                IFrameBrowsingExistingBlockNodeIndex ExistingIndex1 = LeafBlocksInner.IndexAt(0, 0) as IFrameBrowsingExistingBlockNodeIndex;

                Leaf NewItem1 = CreateLeaf(Guid.NewGuid());
                IFrameInsertionExistingBlockNodeIndex InsertionIndex1;
                InsertionIndex1 = ExistingIndex1.ToInsertionIndex(RootNode, NewItem1) as IFrameInsertionExistingBlockNodeIndex;
                Assert.That(InsertionIndex1.ParentNode == RootNode);
                Assert.That(InsertionIndex1.Node == NewItem1);
                Assert.That(CompareEqual.CoverIsEqual(InsertionIndex1, InsertionIndex1));

                Controller.Insert(LeafBlocksInner, InsertionIndex1, out IWriteableBrowsingCollectionNodeIndex NewItemIndex1);
                Assert.That(Controller.Contains(NewItemIndex1));

                IFrameBrowsingExistingBlockNodeIndex DuplicateExistingIndex1 = InsertionIndex1.ToBrowsingIndex() as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex1 as IFrameBrowsingExistingBlockNodeIndex, DuplicateExistingIndex1));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex1, NewItemIndex1 as IFrameBrowsingExistingBlockNodeIndex));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount + 1);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == NodeCount + 1);

                IFramePlaceholderNodeState NewItemState1 = (IFramePlaceholderNodeState)LeafBlocksInner.BlockStateList[0].StateList[0];
                Assert.That(NewItemState1.Node == NewItem1);
                Assert.That(NewItemState1.ParentIndex == NewItemIndex1);
                Assert.That(NewItemState1.ParentState == RootState);

                FrameNodeStateReadOnlyList AllChildren2 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count + 1, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));




                Leaf NewItem2 = CreateLeaf(Guid.NewGuid());
                BaseNode.Pattern NewPattern = BaseNodeHelper.NodeHelper.CreateSimplePattern("");
                BaseNode.Identifier NewSource = BaseNodeHelper.NodeHelper.CreateSimpleIdentifier("");

                IFrameInsertionNewBlockNodeIndex InsertionIndex2 = new FrameInsertionNewBlockNodeIndex(RootNode, nameof(Main.LeafBlocks), NewItem2, 0, NewPattern, NewSource);
                Assert.That(CompareEqual.CoverIsEqual(InsertionIndex2, InsertionIndex2));

                int BlockCount = LeafBlocksInner.BlockStateList.Count;
                Assert.That(BlockCount == 3);

                Controller.Insert(LeafBlocksInner, InsertionIndex2, out IWriteableBrowsingCollectionNodeIndex NewItemIndex2);
                Assert.That(Controller.Contains(NewItemIndex2));

                IFrameBrowsingExistingBlockNodeIndex DuplicateExistingIndex2 = InsertionIndex2.ToBrowsingIndex() as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex2 as IFrameBrowsingExistingBlockNodeIndex, DuplicateExistingIndex2));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex2, NewItemIndex2 as IFrameBrowsingExistingBlockNodeIndex));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount + 2);
                Assert.That(LeafBlocksInner.BlockStateList.Count == BlockCount + 1);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == 1, $"Count: {LeafBlocksInner.BlockStateList[0].StateList.Count}");
                Assert.That(LeafBlocksInner.BlockStateList[1].StateList.Count == 2, $"Count: {LeafBlocksInner.BlockStateList[1].StateList.Count}");
                Assert.That(LeafBlocksInner.BlockStateList[2].StateList.Count == 2, $"Count: {LeafBlocksInner.BlockStateList[2].StateList.Count}");

                IFramePlaceholderNodeState NewItemState2 = (IFramePlaceholderNodeState)LeafBlocksInner.BlockStateList[0].StateList[0];
                Assert.That(NewItemState2.Node == NewItem2);
                Assert.That(NewItemState2.ParentIndex == NewItemIndex2);

                FrameNodeStateReadOnlyList AllChildren3 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FrameRemove()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFrameListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFrameListInner;
                Assert.That(LeafPathInner != null);

                IFrameBrowsingListNodeIndex RemovedLeafIndex0 = LeafPathInner.StateList[0].ParentIndex as IFrameBrowsingListNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex0));

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                FrameNodeStateReadOnlyList AllChildren0 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Assert.That(Controller.IsRemoveable(LeafPathInner, RemovedLeafIndex0));

                Controller.Remove(LeafPathInner, RemovedLeafIndex0);
                Assert.That(!Controller.Contains(RemovedLeafIndex0));

                Assert.That(LeafPathInner.Count == PathCount - 1);
                Assert.That(LeafPathInner.StateList.Count == PathCount - 1);

                FrameNodeStateReadOnlyList AllChildren1 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count - 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                RemovedLeafIndex0 = LeafPathInner.StateList[0].ParentIndex as IFrameBrowsingListNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex0));

                Assert.That(LeafPathInner.Count == 1);

                Assert.That(Controller.IsRemoveable(LeafPathInner, RemovedLeafIndex0));

                IDictionary<Type, string[]> NeverEmptyCollectionTable = BaseNodeHelper.NodeHelper.NeverEmptyCollectionTable as IDictionary<Type, string[]>;
                NeverEmptyCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafPath) });
                Assert.That(!Controller.IsRemoveable(LeafPathInner, RemovedLeafIndex0));



                IFrameBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFrameBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFrameBrowsingExistingBlockNodeIndex RemovedLeafIndex1 = LeafBlocksInner.BlockStateList[1].StateList[0].ParentIndex as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex1));

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[1].StateList.Count;
                Assert.That(BlockNodeCount == 4, $"New count: {BlockNodeCount}");

                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex1));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex1);
                Assert.That(!Controller.Contains(RemovedLeafIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount - 1);
                Assert.That(LeafBlocksInner.BlockStateList[1].StateList.Count == NodeCount - 1);

                FrameNodeStateReadOnlyList AllChildren2 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count - 1, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                IFrameBrowsingExistingBlockNodeIndex RemovedLeafIndex2 = LeafBlocksInner.BlockStateList[1].StateList[0].ParentIndex as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex2));


                int BlockCount = LeafBlocksInner.BlockStateList.Count;
                Assert.That(BlockCount == 3);

                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex2));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex2);
                Assert.That(!Controller.Contains(RemovedLeafIndex2));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount - 2);
                Assert.That(LeafBlocksInner.BlockStateList.Count == BlockCount - 1);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == 1, $"Count: {LeafBlocksInner.BlockStateList[0].StateList.Count}");

                FrameNodeStateReadOnlyList AllChildren3 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FrameRemoveBlockRange()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                FrameNodeStateReadOnlyList AllChildren0 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                IFrameBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFrameBlockListInner;
                Assert.That(LeafBlocksInner != null);
                Assert.That(LeafBlocksInner.BlockStateList.Count == 3, $"New count: {LeafBlocksInner.BlockStateList.Count}");
                Assert.That(Controller.IsBlockRangeRemoveable(LeafBlocksInner, 0, 2));

                Controller.RemoveBlockRange(LeafBlocksInner, 0, 2);
                Assert.That(LeafBlocksInner.Count == 1);

                FrameNodeStateReadOnlyList AllChildren2 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FrameRemoveNodeRange()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                FrameNodeStateReadOnlyList AllChildren0 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");


                //System.Diagnostics.Debug.Assert(false);
                IFrameListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFrameListInner;
                Assert.That(LeafPathInner != null);
                Assert.That(LeafPathInner.StateList.Count == 2, $"New count: {LeafPathInner.StateList.Count}");
                Assert.That(Controller.IsNodeRangeRemoveable(LeafPathInner, -1, 0, 2));

                Controller.RemoveNodeRange(LeafPathInner, -1, 0, 2);

                FrameNodeStateReadOnlyList AllChildren1 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count - 2, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();


                //System.Diagnostics.Debug.Assert(false);
                IFrameBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFrameBlockListInner;
                Assert.That(LeafBlocksInner != null);
                Assert.That(LeafBlocksInner.BlockStateList.Count == 3, $"New count: {LeafBlocksInner.BlockStateList.Count}");
                Assert.That(Controller.IsNodeRangeRemoveable(LeafBlocksInner, 1, 0, 2));

                Controller.RemoveNodeRange(LeafBlocksInner, 1, 0, 2);

                FrameNodeStateReadOnlyList AllChildren2 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FrameMove()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFrameListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFrameListInner;
                Assert.That(LeafPathInner != null);

                IFrameBrowsingListNodeIndex MovedLeafIndex0 = LeafPathInner.IndexAt(0) as IFrameBrowsingListNodeIndex;
                Assert.That(Controller.Contains(MovedLeafIndex0));

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                FrameNodeStateReadOnlyList AllChildren0 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Assert.That(Controller.IsMoveable(LeafPathInner, MovedLeafIndex0, +1));

                Controller.Move(LeafPathInner, MovedLeafIndex0, +1);
                Assert.That(Controller.Contains(MovedLeafIndex0));

                Assert.That(LeafPathInner.Count == PathCount);
                Assert.That(LeafPathInner.StateList.Count == PathCount);

                //System.Diagnostics.Debug.Assert(false);
                IFrameBrowsingListNodeIndex NewLeafIndex0 = LeafPathInner.IndexAt(1) as IFrameBrowsingListNodeIndex;
                Assert.That(NewLeafIndex0 == MovedLeafIndex0);

                FrameNodeStateReadOnlyList AllChildren1 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));




                IFrameBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFrameBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFrameBrowsingExistingBlockNodeIndex MovedLeafIndex1 = LeafBlocksInner.IndexAt(1, 1) as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(MovedLeafIndex1));

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[1].StateList.Count;
                Assert.That(BlockNodeCount == 4, $"New count: {BlockNodeCount}");

                Assert.That(Controller.IsMoveable(LeafBlocksInner, MovedLeafIndex1, -1));
                Controller.Move(LeafBlocksInner, MovedLeafIndex1, -1);
                Assert.That(Controller.Contains(MovedLeafIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount);
                Assert.That(LeafBlocksInner.BlockStateList[1].StateList.Count == NodeCount);

                IFrameBrowsingExistingBlockNodeIndex NewLeafIndex1 = LeafBlocksInner.IndexAt(1, 0) as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(NewLeafIndex1 == MovedLeafIndex1);

                FrameNodeStateReadOnlyList AllChildren2 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FrameMoveBlock()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                FrameNodeStateReadOnlyList AllChildren1 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == 19, $"New count: {AllChildren1.Count}");

                IFrameBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFrameBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFrameBrowsingExistingBlockNodeIndex MovedLeafIndex1 = LeafBlocksInner.IndexAt(1, 0) as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(MovedLeafIndex1));

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = LeafBlocksInner.BlockStateList[1].StateList.Count;
                Assert.That(BlockNodeCount == 4, $"New count: {BlockNodeCount}");

                Assert.That(Controller.IsBlockMoveable(LeafBlocksInner, 1, -1));
                Controller.MoveBlock(LeafBlocksInner, 1, -1);
                Assert.That(Controller.Contains(MovedLeafIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount);
                Assert.That(LeafBlocksInner.BlockStateList[0].StateList.Count == NodeCount);

                IFrameBrowsingExistingBlockNodeIndex NewLeafIndex1 = LeafBlocksInner.IndexAt(0, 0) as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(NewLeafIndex1 == MovedLeafIndex1);

                FrameNodeStateReadOnlyList AllChildren2 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FrameChangeDiscreteValue()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetEnumValue(RootState.Node, nameof(Main.ValueEnum)) == (int)BaseNode.CopySemantic.Value);

                Controller.ChangeDiscreteValue(RootIndex, nameof(Main.ValueEnum), (int)BaseNode.CopySemantic.Reference);

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetEnumValue(RootNode, nameof(Main.ValueEnum)) == (int)BaseNode.CopySemantic.Reference);

                IFramePlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IFramePlaceholderInner;
                IFramePlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as IFramePlaceholderNodeState;

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
        public static void FrameChangeText()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetString(RootState.Node, nameof(Main.ValueString)) == "s");

                Controller.ChangeText(RootIndex, nameof(Main.ValueString), "test");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetString(RootNode, nameof(Main.ValueString)) == "test");

                IFramePlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IFramePlaceholderInner;
                IFramePlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as IFramePlaceholderNodeState;

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
        public static void FrameChangeComment()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Assert.That(BaseNodeHelper.NodeTreeHelper.GetCommentText(RootState.Node) == "main doc");

                Controller.ChangeComment(RootIndex, "test");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(BaseNodeHelper.NodeTreeHelper.GetCommentText(RootNode) == "test");

                IFramePlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IFramePlaceholderInner;
                IFramePlaceholderNodeState PlaceholderTreeState = PlaceholderTreeInner.ChildState as IFramePlaceholderNodeState;

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
        public static void FrameReplace()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                Leaf NewItem0 = CreateLeaf(Guid.NewGuid());
                IFrameInsertionListNodeIndex ReplacementIndex0 = new FrameInsertionListNodeIndex(RootNode, nameof(Main.LeafPath), NewItem0, 0);

                IFrameListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFrameListInner;
                Assert.That(LeafPathInner != null);

                int PathCount = LeafPathInner.Count;
                Assert.That(PathCount == 2);

                FrameNodeStateReadOnlyList AllChildren0 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Replace(LeafPathInner, ReplacementIndex0, out IWriteableBrowsingChildIndex NewItemIndex0);
                Assert.That(Controller.Contains(NewItemIndex0));

                Assert.That(LeafPathInner.Count == PathCount);
                Assert.That(LeafPathInner.StateList.Count == PathCount);

                IFramePlaceholderNodeState NewItemState0 = (IFramePlaceholderNodeState)LeafPathInner.StateList[0];
                Assert.That(NewItemState0.Node == NewItem0);
                Assert.That(NewItemState0.ParentIndex == NewItemIndex0);

                FrameNodeStateReadOnlyList AllChildren1 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                Leaf NewItem1 = CreateLeaf(Guid.NewGuid());
                IFrameInsertionExistingBlockNodeIndex ReplacementIndex1 = new FrameInsertionExistingBlockNodeIndex(RootNode, nameof(Main.LeafBlocks), NewItem1, 0, 0);

                IFrameBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFrameBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFrameBlockState BlockState = (IFrameBlockState)LeafBlocksInner.BlockStateList[0];

                int BlockNodeCount = LeafBlocksInner.Count;
                int NodeCount = BlockState.StateList.Count;
                Assert.That(BlockNodeCount == 4);

                Controller.Replace(LeafBlocksInner, ReplacementIndex1, out IWriteableBrowsingChildIndex NewItemIndex1);
                Assert.That(Controller.Contains(NewItemIndex1));

                Assert.That(LeafBlocksInner.Count == BlockNodeCount);
                Assert.That(BlockState.StateList.Count == NodeCount);

                IFramePlaceholderNodeState NewItemState1 = (IFramePlaceholderNodeState)BlockState.StateList[0];
                Assert.That(NewItemState1.Node == NewItem1);
                Assert.That(NewItemState1.ParentIndex == NewItemIndex1);

                FrameNodeStateReadOnlyList AllChildren2 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                IFramePlaceholderInner PlaceholderTreeInner = RootState.PropertyToInner(nameof(Main.PlaceholderTree)) as IFramePlaceholderInner;
                Assert.That(PlaceholderTreeInner != null);

                IFrameBrowsingPlaceholderNodeIndex ExistingIndex2 = PlaceholderTreeInner.ChildState.ParentIndex as IFrameBrowsingPlaceholderNodeIndex;

                Tree NewItem2 = CreateTree();
                IFrameInsertionPlaceholderNodeIndex ReplacementIndex2;
                ReplacementIndex2 = ExistingIndex2.ToInsertionIndex(RootNode, NewItem2) as IFrameInsertionPlaceholderNodeIndex;

                Controller.Replace(PlaceholderTreeInner, ReplacementIndex2, out IWriteableBrowsingChildIndex NewItemIndex2);
                Assert.That(Controller.Contains(NewItemIndex2));

                IFramePlaceholderNodeState NewItemState2 = PlaceholderTreeInner.ChildState as IFramePlaceholderNodeState;
                Assert.That(NewItemState2.Node == NewItem2);
                Assert.That(NewItemState2.ParentIndex == NewItemIndex2);

                IFrameBrowsingPlaceholderNodeIndex DuplicateExistingIndex2 = ReplacementIndex2.ToBrowsingIndex() as IFrameBrowsingPlaceholderNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex2 as IFrameBrowsingPlaceholderNodeIndex, DuplicateExistingIndex2));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex2, NewItemIndex2 as IFrameBrowsingPlaceholderNodeIndex));

                FrameNodeStateReadOnlyList AllChildren3 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count, $"New count: {AllChildren3.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                IFramePlaceholderInner PlaceholderLeafInner = NewItemState2.PropertyToInner(nameof(Tree.Placeholder)) as IFramePlaceholderInner;
                Assert.That(PlaceholderLeafInner != null);

                IFrameBrowsingPlaceholderNodeIndex ExistingIndex3 = PlaceholderLeafInner.ChildState.ParentIndex as IFrameBrowsingPlaceholderNodeIndex;

                Leaf NewItem3 = CreateLeaf(Guid.NewGuid());
                IFrameInsertionPlaceholderNodeIndex ReplacementIndex3;
                ReplacementIndex3 = ExistingIndex3.ToInsertionIndex(NewItem2, NewItem3) as IFrameInsertionPlaceholderNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(ReplacementIndex3, ReplacementIndex3));

                Controller.Replace(PlaceholderLeafInner, ReplacementIndex3, out IWriteableBrowsingChildIndex NewItemIndex3);
                Assert.That(Controller.Contains(NewItemIndex3));

                IFramePlaceholderNodeState NewItemState3 = PlaceholderLeafInner.ChildState as IFramePlaceholderNodeState;
                Assert.That(NewItemState3.Node == NewItem3);
                Assert.That(NewItemState3.ParentIndex == NewItemIndex3);

                FrameNodeStateReadOnlyList AllChildren4 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren4.Count == AllChildren3.Count, $"New count: {AllChildren4.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));




                IFrameOptionalInner OptionalLeafInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IFrameOptionalInner;
                Assert.That(OptionalLeafInner != null);

                IFrameBrowsingOptionalNodeIndex ExistingIndex4 = OptionalLeafInner.ChildState.ParentIndex as IFrameBrowsingOptionalNodeIndex;

                Leaf NewItem4 = CreateLeaf(Guid.NewGuid());
                IFrameInsertionOptionalNodeIndex ReplacementIndex4;
                ReplacementIndex4 = ExistingIndex4.ToInsertionIndex(RootNode, NewItem4) as IFrameInsertionOptionalNodeIndex;
                Assert.That(ReplacementIndex4.ParentNode == RootNode);
                Assert.That(ReplacementIndex4.PropertyName == OptionalLeafInner.PropertyName);
                Assert.That(CompareEqual.CoverIsEqual(ReplacementIndex4, ReplacementIndex4));

                Controller.Replace(OptionalLeafInner, ReplacementIndex4, out IWriteableBrowsingChildIndex NewItemIndex4);
                Assert.That(Controller.Contains(NewItemIndex4));

                Assert.That(OptionalLeafInner.IsAssigned);
                IFrameOptionalNodeState NewItemState4 = OptionalLeafInner.ChildState as IFrameOptionalNodeState;
                Assert.That(NewItemState4.Node == NewItem4);
                Assert.That(NewItemState4.ParentIndex == NewItemIndex4);

                IFrameBrowsingOptionalNodeIndex DuplicateExistingIndex4 = ReplacementIndex4.ToBrowsingIndex() as IFrameBrowsingOptionalNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex4 as IFrameBrowsingOptionalNodeIndex, DuplicateExistingIndex4));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex4, NewItemIndex4 as IFrameBrowsingOptionalNodeIndex));

                FrameNodeStateReadOnlyList AllChildren5 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren5.Count == AllChildren4.Count, $"New count: {AllChildren5.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));



                IFrameBrowsingOptionalNodeIndex ExistingIndex5 = OptionalLeafInner.ChildState.ParentIndex as IFrameBrowsingOptionalNodeIndex;

                Leaf NewItem5 = CreateLeaf(Guid.NewGuid());
                IFrameInsertionOptionalClearIndex ReplacementIndex5;
                ReplacementIndex5 = ExistingIndex5.ToInsertionIndex(RootNode, null) as IFrameInsertionOptionalClearIndex;
                Assert.That(ReplacementIndex5.ParentNode == RootNode);
                Assert.That(ReplacementIndex5.PropertyName == OptionalLeafInner.PropertyName);
                Assert.That(CompareEqual.CoverIsEqual(ReplacementIndex5, ReplacementIndex5));

                Controller.Replace(OptionalLeafInner, ReplacementIndex5, out IWriteableBrowsingChildIndex NewItemIndex5);
                Assert.That(Controller.Contains(NewItemIndex5));

                Assert.That(!OptionalLeafInner.IsAssigned);
                IFrameOptionalNodeState NewItemState5 = OptionalLeafInner.ChildState as IFrameOptionalNodeState;
                Assert.That(NewItemState5.ParentIndex == NewItemIndex5);

                IFrameBrowsingOptionalNodeIndex DuplicateExistingIndex5 = ReplacementIndex5.ToBrowsingIndex() as IFrameBrowsingOptionalNodeIndex;
                Assert.That(CompareEqual.CoverIsEqual(NewItemIndex5 as IFrameBrowsingOptionalNodeIndex, DuplicateExistingIndex5));
                Assert.That(CompareEqual.CoverIsEqual(DuplicateExistingIndex5, NewItemIndex5 as IFrameBrowsingOptionalNodeIndex));

                FrameNodeStateReadOnlyList AllChildren6 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FrameAssign()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            //System.Diagnostics.Debug.Assert(false);
            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFrameOptionalInner UnassignedOptionalLeafInner = RootState.PropertyToInner(nameof(Main.UnassignedOptionalLeaf)) as IFrameOptionalInner;
                Assert.That(UnassignedOptionalLeafInner != null);
                Assert.That(!UnassignedOptionalLeafInner.IsAssigned);

                IFrameBrowsingOptionalNodeIndex AssignmentIndex0 = UnassignedOptionalLeafInner.ChildState.ParentIndex;
                Assert.That(AssignmentIndex0 != null);

                FrameNodeStateReadOnlyList AllChildren0 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Assign(AssignmentIndex0, out bool IsChanged);
                Assert.That(IsChanged);
                Assert.That(UnassignedOptionalLeafInner.IsAssigned);

                FrameNodeStateReadOnlyList AllChildren1 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Assign(AssignmentIndex0, out IsChanged);
                Assert.That(!IsChanged);
                Assert.That(UnassignedOptionalLeafInner.IsAssigned);

                FrameNodeStateReadOnlyList AllChildren2 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Controller.Unassign(AssignmentIndex0, out IsChanged);
                Assert.That(IsChanged);
                Assert.That(!UnassignedOptionalLeafInner.IsAssigned);

                FrameNodeStateReadOnlyList AllChildren3 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FrameUnassign()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFrameOptionalInner AssignedOptionalLeafInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IFrameOptionalInner;
                Assert.That(AssignedOptionalLeafInner != null);
                Assert.That(AssignedOptionalLeafInner.IsAssigned);

                IFrameBrowsingOptionalNodeIndex AssignmentIndex0 = AssignedOptionalLeafInner.ChildState.ParentIndex;
                Assert.That(AssignmentIndex0 != null);

                FrameNodeStateReadOnlyList AllChildren0 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Unassign(AssignmentIndex0, out bool IsChanged);
                Assert.That(IsChanged);
                Assert.That(!AssignedOptionalLeafInner.IsAssigned);

                FrameNodeStateReadOnlyList AllChildren1 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count - 1, $"New count: {AllChildren1.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Unassign(AssignmentIndex0, out IsChanged);
                Assert.That(!IsChanged);
                Assert.That(!AssignedOptionalLeafInner.IsAssigned);

                FrameNodeStateReadOnlyList AllChildren2 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Controller.Assign(AssignmentIndex0, out IsChanged);
                Assert.That(IsChanged);
                Assert.That(AssignedOptionalLeafInner.IsAssigned);

                FrameNodeStateReadOnlyList AllChildren3 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FrameChangeReplication()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFrameBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFrameBlockListInner;
                Assert.That(LeafBlocksInner != null);

                FrameNodeStateReadOnlyList AllChildren0 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                IFrameBlockState BlockState = (IFrameBlockState)LeafBlocksInner.BlockStateList[0];
                Assert.That(BlockState != null);
                Assert.That(BlockState.ParentInner == LeafBlocksInner);
                BaseNode.IBlock ChildBlock = BlockState.ChildBlock;
                Assert.That(ChildBlock.Replication == BaseNode.ReplicationStatus.Normal);

                Controller.ChangeReplication(LeafBlocksInner, 0, BaseNode.ReplicationStatus.Replicated);

                Assert.That(ChildBlock.Replication == BaseNode.ReplicationStatus.Replicated);

                FrameNodeStateReadOnlyList AllChildren1 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FrameSplit()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFrameBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFrameBlockListInner;
                Assert.That(LeafBlocksInner != null);

                FrameNodeStateReadOnlyList AllChildren0 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                IFrameBlockState BlockState0 = (IFrameBlockState)LeafBlocksInner.BlockStateList[0];
                Assert.That(BlockState0 != null);
                BaseNode.IBlock ChildBlock0 = BlockState0.ChildBlock;
                Assert.That(ChildBlock0.NodeList.Count == 1);

                IFrameBlockState BlockState1 = (IFrameBlockState)LeafBlocksInner.BlockStateList[1];
                Assert.That(BlockState1 != null);
                BaseNode.IBlock ChildBlock1 = BlockState1.ChildBlock;
                Assert.That(ChildBlock1.NodeList.Count == 2);

                Assert.That(LeafBlocksInner.Count == 4);
                Assert.That(LeafBlocksInner.BlockStateList.Count == 3);

                IFrameBrowsingExistingBlockNodeIndex SplitIndex0 = LeafBlocksInner.IndexAt(1, 1) as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.IsSplittable(LeafBlocksInner, SplitIndex0));

                Controller.SplitBlock(LeafBlocksInner, SplitIndex0);

                Assert.That(LeafBlocksInner.BlockStateList.Count == 4);
                Assert.That(ChildBlock0 == LeafBlocksInner.BlockStateList[0].ChildBlock);
                Assert.That(ChildBlock1 == LeafBlocksInner.BlockStateList[2].ChildBlock);
                Assert.That(ChildBlock1.NodeList.Count == 1);

                IFrameBlockState BlockState12 = (IFrameBlockState)LeafBlocksInner.BlockStateList[1];
                Assert.That(BlockState12.ChildBlock.NodeList.Count == 1);

                FrameNodeStateReadOnlyList AllChildren1 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FrameMerge()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFrameBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFrameBlockListInner;
                Assert.That(LeafBlocksInner != null);

                FrameNodeStateReadOnlyList AllChildren0 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                IFrameBlockState BlockState0 = (IFrameBlockState)LeafBlocksInner.BlockStateList[0];
                Assert.That(BlockState0 != null);
                BaseNode.IBlock ChildBlock0 = BlockState0.ChildBlock;
                Assert.That(ChildBlock0.NodeList.Count == 1);

                IFrameBlockState BlockState1 = (IFrameBlockState)LeafBlocksInner.BlockStateList[1];
                Assert.That(BlockState1 != null);
                BaseNode.IBlock ChildBlock1 = BlockState1.ChildBlock;
                Assert.That(ChildBlock1.NodeList.Count == 2);

                Assert.That(LeafBlocksInner.Count == 4);

                IFrameBrowsingExistingBlockNodeIndex MergeIndex0 = LeafBlocksInner.IndexAt(1, 0) as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.IsMergeable(LeafBlocksInner, MergeIndex0));

                Assert.That(LeafBlocksInner.BlockStateList.Count == 3);

                Controller.MergeBlocks(LeafBlocksInner, MergeIndex0);

                Assert.That(LeafBlocksInner.BlockStateList.Count == 2);
                Assert.That(ChildBlock1 == LeafBlocksInner.BlockStateList[0].ChildBlock);
                Assert.That(ChildBlock1.NodeList.Count == 3);

                Assert.That(LeafBlocksInner.BlockStateList[0] == BlockState1);

                FrameNodeStateReadOnlyList AllChildren1 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FrameExpand()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                FrameNodeStateReadOnlyList AllChildren0 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 19, $"New count: {AllChildren0.Count}");

                Controller.Expand(RootIndex, out bool IsChanged);
                Assert.That(IsChanged);

                FrameNodeStateReadOnlyList AllChildren1 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 2, $"New count: {AllChildren1.Count - AllChildren0.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(!IsChanged);

                FrameNodeStateReadOnlyList AllChildren2 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count, $"New count: {AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                IFrameOptionalInner OptionalLeafInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IFrameOptionalInner;
                Assert.That(OptionalLeafInner != null);

                IFrameInsertionOptionalClearIndex ReplacementIndex5 = new FrameInsertionOptionalClearIndex(RootNode, nameof(Main.AssignedOptionalLeaf));

                Controller.Replace(OptionalLeafInner, ReplacementIndex5, out IWriteableBrowsingChildIndex NewItemIndex5);
                Assert.That(Controller.Contains(NewItemIndex5));

                FrameNodeStateReadOnlyList AllChildren3 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count - 1, $"New count: {AllChildren3.Count - AllChildren2.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FrameNodeStateReadOnlyList AllChildren4 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren4.Count == AllChildren3.Count + 1, $"New count: {AllChildren4.Count}");



                IFrameBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFrameBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFrameBrowsingExistingBlockNodeIndex RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                FrameNodeStateReadOnlyList AllChildren5 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren5.Count == AllChildren4.Count - 10, $"New count: {AllChildren5.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(LeafBlocksInner.IsEmpty);

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(!IsChanged);

                FrameNodeStateReadOnlyList AllChildren6 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren6.Count == AllChildren5.Count, $"New count: {AllChildren6.Count - AllChildren5.Count}");

                IDictionary<Type, string[]> WithExpandCollectionTable = BaseNodeHelper.NodeHelper.WithExpandCollectionTable as IDictionary<Type, string[]>;
                WithExpandCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafBlocks) });

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FrameNodeStateReadOnlyList AllChildren7 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren7.Count == AllChildren6.Count + 3, $"New count: {AllChildren7.Count}");

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
        public static void FrameReduce()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;
            bool IsChanged;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFrameBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFrameBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFrameBrowsingExistingBlockNodeIndex RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(LeafBlocksInner.IsEmpty);

                FrameNodeStateReadOnlyList AllChildren0 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 9, $"New count: {AllChildren0.Count}");

                IDictionary<Type, string[]> WithExpandCollectionTable = BaseNodeHelper.NodeHelper.WithExpandCollectionTable as IDictionary<Type, string[]>;
                WithExpandCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafBlocks) });

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FrameNodeStateReadOnlyList AllChildren1 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 5, $"New count: {AllChildren1.Count - AllChildren0.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FrameNodeStateReadOnlyList AllChildren2 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count - 4, $"New count: {AllChildren2.Count - AllChildren1.Count}");

                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(!IsChanged);

                FrameNodeStateReadOnlyList AllChildren3 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count, $"New count: {AllChildren3.Count}");

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FrameNodeStateReadOnlyList AllChildren4 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren4.Count == AllChildren3.Count + 4, $"New count: {AllChildren4.Count - AllChildren3.Count}");

                BaseNode.IBlock ChildBlock = LeafBlocksInner.BlockStateList[0].ChildBlock;
                Leaf FirstNode = ChildBlock.NodeList[0] as Leaf;
                Assert.That(FirstNode != null);
                BaseNodeHelper.NodeTreeHelper.SetString(FirstNode, nameof(Coverage.Leaf.Text), "!");


                //System.Diagnostics.Debug.Assert(false);
                IFrameOptionalInner LeafOptionalInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IFrameOptionalInner;
                Assert.That(LeafOptionalInner != null);

                Leaf Leaf = LeafOptionalInner.ChildState.Node as Leaf;
                BaseNodeHelper.NodeTreeHelper.SetStringProperty(Leaf, "Text", "");


                //System.Diagnostics.Debug.Assert(false);
                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FrameNodeStateReadOnlyList AllChildren5 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren5.Count == AllChildren4.Count - 2, $"New count: {AllChildren5.Count - AllChildren4.Count}");

                BaseNodeHelper.NodeTreeHelper.SetString(FirstNode, nameof(Leaf.Text), "");

                //System.Diagnostics.Debug.Assert(false);
                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FrameNodeStateReadOnlyList AllChildren6 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren6.Count == AllChildren5.Count - 3, $"New count: {AllChildren6.Count - AllChildren5.Count}");

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                WithExpandCollectionTable.Remove(Type.FromTypeof<Main>());

                //System.Diagnostics.Debug.Assert(false);
                Controller.Reduce(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FrameNodeStateReadOnlyList AllChildren7 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
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
        public static void FrameCanonicalize()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;
            bool IsChanged;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFrameBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFrameBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFrameBrowsingExistingBlockNodeIndex RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                Assert.That(Controller.CanUndo);
                FrameOperationGroup LastOperation = (FrameOperationGroup)Controller.OperationStack[Controller.RedoIndex - 1];
                Assert.That(LastOperation.MainOperation is IFrameRemoveOperation);
                Assert.That(LastOperation.OperationList.Count > 0);
                Assert.That(LastOperation.Refresh == null);

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                RemovedLeafIndex = LeafBlocksInner.BlockStateList[0].StateList[0].ParentIndex as IFrameBrowsingExistingBlockNodeIndex;
                Assert.That(Controller.Contains(RemovedLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafBlocksInner, RemovedLeafIndex));

                Controller.Remove(LeafBlocksInner, RemovedLeafIndex);
                Assert.That(!Controller.Contains(RemovedLeafIndex));

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));
                Assert.That(LeafBlocksInner.IsEmpty);

                IFrameListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFrameListInner;
                Assert.That(LeafPathInner != null);
                Assert.That(LeafPathInner.Count == 2);

                IFrameBrowsingListNodeIndex RemovedListLeafIndex = LeafPathInner.StateList[0].ParentIndex as IFrameBrowsingListNodeIndex;
                Assert.That(Controller.Contains(RemovedListLeafIndex));
                Assert.That(Controller.IsRemoveable(LeafPathInner, RemovedListLeafIndex));

                Controller.Remove(LeafPathInner, RemovedListLeafIndex);
                Assert.That(!Controller.Contains(RemovedListLeafIndex));

                IDictionary<Type, string[]> NeverEmptyCollectionTable = BaseNodeHelper.NodeHelper.NeverEmptyCollectionTable as IDictionary<Type, string[]>;
                NeverEmptyCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.PlaceholderTree) });

                RemovedListLeafIndex = LeafPathInner.StateList[0].ParentIndex as IFrameBrowsingListNodeIndex;
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

                FrameNodeStateReadOnlyList AllChildren0 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren0.Count == 12, $"New count: {AllChildren0.Count}");

                IDictionary<Type, string[]> WithExpandCollectionTable = BaseNodeHelper.NodeHelper.WithExpandCollectionTable as IDictionary<Type, string[]>;
                WithExpandCollectionTable.Add(Type.FromTypeof<Main>(), new string[] { nameof(Main.LeafBlocks) });

                Controller.Expand(RootIndex, out IsChanged);
                Assert.That(IsChanged);

                FrameNodeStateReadOnlyList AllChildren1 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren1.Count == AllChildren0.Count + 2, $"New count: {AllChildren1.Count - AllChildren0.Count}");

                Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));


                //System.Diagnostics.Debug.Assert(false);
                IFrameOptionalInner LeafOptionalInner = RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf)) as IFrameOptionalInner;
                Assert.That(LeafOptionalInner != null);

                Leaf Leaf = LeafOptionalInner.ChildState.Node as Leaf;
                BaseNodeHelper.NodeTreeHelper.SetStringProperty(Leaf, "Text", "");


                Controller.Canonicalize(out IsChanged);
                Assert.That(IsChanged);

                FrameNodeStateReadOnlyList AllChildren2 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren2.Count == AllChildren1.Count - 2, $"New count: {AllChildren2.Count - AllChildren1.Count}");

                Controller.Undo();
                Controller.Redo();

                Controller.Canonicalize(out IsChanged);
                Assert.That(!IsChanged);

                FrameNodeStateReadOnlyList AllChildren3 = (FrameNodeStateReadOnlyList)RootState.GetAllChildren();
                Assert.That(AllChildren3.Count == AllChildren2.Count, $"New count: {AllChildren3.Count}");

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
        public static void FramePrune()
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
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            using (FrameControllerView ControllerView0 = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
            {
                Assert.That(ControllerView0.Controller == Controller);

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFrameBlockListInner MainInnerH = RootState.PropertyToInner(nameof(Root.MainBlocksH)) as IFrameBlockListInner;
                Assert.That(MainInnerH != null);

                IFrameBrowsingExistingBlockNodeIndex MainIndex = MainInnerH.IndexAt(0, 0) as IFrameBrowsingExistingBlockNodeIndex;
                Controller.Remove(MainInnerH, MainIndex);

                Assert.That(Controller.CanUndo);
                Controller.Undo();

                Assert.That(!Controller.CanUndo);
                Assert.That(Controller.CanRedo);

                Controller.Redo();
                Controller.Undo();

                MainIndex = MainInnerH.IndexAt(0, 0) as IFrameBrowsingExistingBlockNodeIndex;
                Controller.Remove(MainInnerH, MainIndex);

                Controller.Undo();
                Controller.Redo();
                Controller.Undo();

                Assert.That(ControllerBase.IsEqual(CompareEqual.New(), Controller));
            }
        }

        [Test]
        [Category("Coverage")]
        public static void FrameCollections()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IFrameRootNodeIndex RootIndex;
            bool IsReadOnly;
            IReadOnlyBlockState FirstBlockState;
            IReadOnlyBrowsingBlockNodeIndex FirstBlockNodeIndex;
            IReadOnlyBrowsingListNodeIndex FirstListNodeIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new FrameRootNodeIndex(RootNode);

            FrameController ControllerBase = FrameController.Create(RootIndex);
            FrameController Controller = FrameController.Create(RootIndex);

            ReadOnlyNodeStateDictionary ControllerStateTable = DebugObjects.GetReferenceByInterface(Type.FromTypeof<FrameNodeStateDictionary>()) as ReadOnlyNodeStateDictionary;

            using (FrameControllerView ControllerView = FrameControllerView.Create(Controller, TestDebug.CoverageFrameTemplateSet.FrameTemplateSet))
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

                // IFrameBlockStateList

                IFrameNodeState RootState = Controller.RootState;
                Assert.That(RootState != null);

                IFrameBlockListInner LeafBlocksInner = RootState.PropertyToInner(nameof(Main.LeafBlocks)) as IFrameBlockListInner;
                Assert.That(LeafBlocksInner != null);

                IFrameListInner LeafPathInner = RootState.PropertyToInner(nameof(Main.LeafPath)) as IFrameListInner;
                Assert.That(LeafPathInner != null);

                IFramePlaceholderNodeState FirstNodeState = LeafBlocksInner.FirstNodeState;
                FrameBlockStateList DebugBlockStateList = DebugObjects.GetReferenceByInterface(Type.FromTypeof<FrameBlockStateList>()) as FrameBlockStateList;
                if (DebugBlockStateList != null)
                {
                    Assert.That(DebugBlockStateList.Count > 0);
                    IsReadOnly = ((ICollection<IReadOnlyBlockState>)DebugBlockStateList).IsReadOnly;
                    IsReadOnly = ((IList<IReadOnlyBlockState>)DebugBlockStateList).IsReadOnly;
                    IsReadOnly = ((ICollection<IWriteableBlockState>)DebugBlockStateList).IsReadOnly;
                    IsReadOnly = ((IList<IWriteableBlockState>)DebugBlockStateList).IsReadOnly;
                    FirstBlockState = DebugBlockStateList[0];
                    Assert.That(((WriteableBlockStateList)DebugBlockStateList)[0] == FirstBlockState);
                    Assert.That(DebugBlockStateList.Contains(FirstBlockState));
                    Assert.That(DebugBlockStateList.IndexOf(FirstBlockState) == 0);
                    DebugBlockStateList.Remove(FirstBlockState);
                    DebugBlockStateList.Add(FirstBlockState);
                    DebugBlockStateList.Remove(FirstBlockState);
                    DebugBlockStateList.Insert(0, FirstBlockState);
                    DebugBlockStateList.CopyTo((IReadOnlyBlockState[])(new IFrameBlockState[DebugBlockStateList.Count]), 0);
                    DebugBlockStateList.CopyTo((IWriteableBlockState[])(new IFrameBlockState[DebugBlockStateList.Count]), 0);

                    IEnumerable<IReadOnlyBlockState> BlockStateListAsReadOnlyEnumerable = DebugBlockStateList;
                    foreach (IReadOnlyBlockState Item in BlockStateListAsReadOnlyEnumerable)
                    {
                        break;
                    }

                    IEnumerable<IWriteableBlockState> BlockStateListAsWriteableEnumerable = DebugBlockStateList;
                    foreach (IWriteableBlockState Item in BlockStateListAsWriteableEnumerable)
                    {
                        break;
                    }

                    IList<IReadOnlyBlockState> BlockStateListAsReadOnlyIlist = DebugBlockStateList;
                    Assert.That(BlockStateListAsReadOnlyIlist[0] == FirstBlockState);

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

                    IReadOnlyList<IReadOnlyBlockState> BlockStateListAsReadOnlyIReadOnlylist = DebugBlockStateList;
                    Assert.That(BlockStateListAsReadOnlyIReadOnlylist[0] == FirstBlockState);

                    IReadOnlyList<IWriteableBlockState> BlockStateListAsWriteableIReadOnlylist = DebugBlockStateList;
                    Assert.That(BlockStateListAsWriteableIReadOnlylist[0] == FirstBlockState);

                    IEnumerator<IWriteableBlockState> DebugBlockStateListWriteableEnumerator = ((ICollection<IWriteableBlockState>)DebugBlockStateList).GetEnumerator();
                }

                FrameBlockStateReadOnlyList FrameBlockStateList = LeafBlocksInner.BlockStateList;
                Assert.That(FrameBlockStateList.Count > 0);
                FirstBlockState = FrameBlockStateList[0];
                Assert.That(FrameBlockStateList.Contains(FirstBlockState));
                Assert.That(FrameBlockStateList.IndexOf(FirstBlockState) == 0);
                Assert.That(FrameBlockStateList.Contains((IWriteableBlockState)FirstBlockState));
                Assert.That(FrameBlockStateList.IndexOf((IWriteableBlockState)FirstBlockState) == 0);
                Assert.That(FrameBlockStateList.Contains((IFrameBlockState)FirstBlockState));
                Assert.That(FrameBlockStateList.IndexOf((IFrameBlockState)FirstBlockState) == 0);

                IEnumerable<IWriteableBlockState> FrameBlockStateListAsIEnumerable = FrameBlockStateList;
                IEnumerator<IWriteableBlockState> FrameBlockStateListAsIEnumerableEnumerator = FrameBlockStateListAsIEnumerable.GetEnumerator();

                IReadOnlyList<IWriteableBlockState> FrameBlockStateListAsIReadOnlyList = FrameBlockStateList;
                Assert.That(FrameBlockStateListAsIReadOnlyList[0] == FirstBlockState);

                // IFrameBrowsingBlockNodeIndexList

                FrameBrowsingBlockNodeIndexList BlockNodeIndexList = LeafBlocksInner.AllIndexes() as FrameBrowsingBlockNodeIndexList;
                Assert.That(BlockNodeIndexList.Count > 0);
                IsReadOnly = ((ICollection<IReadOnlyBrowsingBlockNodeIndex>)BlockNodeIndexList).IsReadOnly;
                IsReadOnly = ((IList<IReadOnlyBrowsingBlockNodeIndex>)BlockNodeIndexList).IsReadOnly;
                IsReadOnly = ((ICollection<IWriteableBrowsingBlockNodeIndex>)BlockNodeIndexList).IsReadOnly;
                IsReadOnly = ((IList<IWriteableBrowsingBlockNodeIndex>)BlockNodeIndexList).IsReadOnly;
                FirstBlockNodeIndex = BlockNodeIndexList[0];
                Assert.That(((WriteableBrowsingBlockNodeIndexList)BlockNodeIndexList)[0] == FirstBlockNodeIndex);
                Assert.That(BlockNodeIndexList.Contains(FirstBlockNodeIndex));
                Assert.That(BlockNodeIndexList.IndexOf(FirstBlockNodeIndex) == 0);
                BlockNodeIndexList.Remove(FirstBlockNodeIndex);
                BlockNodeIndexList.Add(FirstBlockNodeIndex);
                BlockNodeIndexList.Remove(FirstBlockNodeIndex);
                BlockNodeIndexList.Insert(0, FirstBlockNodeIndex);
                BlockNodeIndexList.CopyTo((IReadOnlyBrowsingBlockNodeIndex[])(new IFrameBrowsingBlockNodeIndex[BlockNodeIndexList.Count]), 0);
                BlockNodeIndexList.CopyTo((IWriteableBrowsingBlockNodeIndex[])(new IFrameBrowsingBlockNodeIndex[BlockNodeIndexList.Count]), 0);

                IEnumerable<IReadOnlyBrowsingBlockNodeIndex> BlockNodeIndexListAsReadOnlyEnumerable = BlockNodeIndexList;
                foreach (IReadOnlyBrowsingBlockNodeIndex Item in BlockNodeIndexListAsReadOnlyEnumerable)
                {
                    break;
                }

                IEnumerable<IWriteableBrowsingBlockNodeIndex> BlockNodeIndexListAsWriteableEnumerable = BlockNodeIndexList;
                foreach (IWriteableBrowsingBlockNodeIndex Item in BlockNodeIndexListAsWriteableEnumerable)
                {
                    break;
                }

                IList<IReadOnlyBrowsingBlockNodeIndex> BlockNodeIndexListAsReadOnlyIList = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsReadOnlyIList[0] == FirstBlockNodeIndex);

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

                IReadOnlyList<IReadOnlyBrowsingBlockNodeIndex> BlockNodeIndexListAsReadOnlyIReadOnlylist = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsReadOnlyIReadOnlylist[0] == FirstBlockNodeIndex);

                IReadOnlyList<IWriteableBrowsingBlockNodeIndex> BlockNodeIndexListAsWriteableIReadOnlylist = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsWriteableIReadOnlylist[0] == FirstBlockNodeIndex);

                ReadOnlyBrowsingBlockNodeIndexList BlockNodeIndexListAsReadOnly = BlockNodeIndexList;
                Assert.That(BlockNodeIndexListAsReadOnly[0] == FirstBlockNodeIndex);

                IEnumerator<IWriteableBrowsingBlockNodeIndex> BlockNodeIndexListWriteableEnumerator = ((ICollection<IWriteableBrowsingBlockNodeIndex>)BlockNodeIndexList).GetEnumerator();

                // IFrameBrowsingListNodeIndexList

                FrameBrowsingListNodeIndexList ListNodeIndexList = LeafPathInner.AllIndexes() as FrameBrowsingListNodeIndexList;
                Assert.That(ListNodeIndexList.Count > 0);
                IsReadOnly = ((ICollection<IReadOnlyBrowsingListNodeIndex>)ListNodeIndexList).IsReadOnly;
                IsReadOnly = ((IList<IReadOnlyBrowsingListNodeIndex>)ListNodeIndexList).IsReadOnly;
                IsReadOnly = ((ICollection<IWriteableBrowsingListNodeIndex>)ListNodeIndexList).IsReadOnly;
                IsReadOnly = ((IList<IWriteableBrowsingListNodeIndex>)ListNodeIndexList).IsReadOnly;
                FirstListNodeIndex = ListNodeIndexList[0];
                Assert.That(((WriteableBrowsingListNodeIndexList)ListNodeIndexList)[0] == FirstListNodeIndex);
                Assert.That(ListNodeIndexList.Contains(FirstListNodeIndex));
                Assert.That(ListNodeIndexList.IndexOf(FirstListNodeIndex) == 0);
                ListNodeIndexList.Remove(FirstListNodeIndex);
                ListNodeIndexList.Add(FirstListNodeIndex);
                ListNodeIndexList.Remove(FirstListNodeIndex);
                ListNodeIndexList.Insert(0, FirstListNodeIndex);
                ListNodeIndexList.CopyTo((IReadOnlyBrowsingListNodeIndex[])(new IFrameBrowsingListNodeIndex[ListNodeIndexList.Count]), 0);
                ListNodeIndexList.CopyTo((IWriteableBrowsingListNodeIndex[])(new IFrameBrowsingListNodeIndex[ListNodeIndexList.Count]), 0);

                IEnumerable<IReadOnlyBrowsingListNodeIndex> ListNodeIndexListAsReadOnlyEnumerable = ListNodeIndexList;
                foreach (IReadOnlyBrowsingListNodeIndex Item in ListNodeIndexListAsReadOnlyEnumerable)
                {
                    break;
                }

                IEnumerable<IWriteableBrowsingListNodeIndex> ListNodeIndexListAsWriteableEnumerable = ListNodeIndexList;
                foreach (IWriteableBrowsingListNodeIndex Item in ListNodeIndexListAsWriteableEnumerable)
                {
                    break;
                }

                IList<IReadOnlyBrowsingListNodeIndex> ListNodeIndexListAsReadOnlyIList = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsReadOnlyIList[0] == FirstListNodeIndex);

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

                IReadOnlyList<IReadOnlyBrowsingListNodeIndex> ListNodeIndexListAsReadOnlyIReadOnlylist = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsReadOnlyIReadOnlylist[0] == FirstListNodeIndex);

                IReadOnlyList<IWriteableBrowsingListNodeIndex> ListNodeIndexListAsWriteableIReadOnlylist = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsWriteableIReadOnlylist[0] == FirstListNodeIndex);

                ReadOnlyBrowsingListNodeIndexList ListNodeIndexListAsReadOnly = ListNodeIndexList;
                Assert.That(ListNodeIndexListAsReadOnly[0] == FirstListNodeIndex);

                IEnumerator<IWriteableBrowsingListNodeIndex> ListNodeIndexListWriteableEnumerator = ((ICollection<IWriteableBrowsingListNodeIndex>)ListNodeIndexList).GetEnumerator();

                // IFrameIndexNodeStateDictionary
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

                    WriteableNodeStateDictionary WriteableControllerStateTable = ControllerStateTable as WriteableNodeStateDictionary;
                    foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in (ICollection<KeyValuePair<IWriteableIndex, IWriteableNodeState>>)WriteableControllerStateTable)
                    {
                        break;
                    }

                    IDictionary<IReadOnlyIndex, IReadOnlyNodeState> ReadOnlyControllerStateTableAsDictionary = ControllerStateTable;
                    foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in ReadOnlyControllerStateTableAsDictionary)
                    {
                        IReadOnlyNodeState StateView = ReadOnlyControllerStateTableAsDictionary[Entry.Key];
                        Assert.That(ReadOnlyControllerStateTableAsDictionary.ContainsKey(Entry.Key));
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

                    IEnumerable<KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState>> ReadOnlyControllerStateTableAsEnumerable = ControllerStateTable;
                    foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in ReadOnlyControllerStateTableAsEnumerable)
                    {
                        break;
                    }

                    IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>> WriteableControllerStateTableAsEnumerable = ControllerStateTable as IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>;
                    foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in WriteableControllerStateTableAsEnumerable)
                    {
                        break;
                    }
                }

                // IFrameIndexNodeStateReadOnlyDictionary

                ReadOnlyNodeStateReadOnlyDictionary ReadOnlyStateTable = Controller.StateTable;
                WriteableNodeStateReadOnlyDictionary WriteableStateTable = Controller.StateTable;
                Assert.That(WriteableStateTable.ContainsKey(RootIndex));
                Assert.That(WriteableStateTable[RootIndex] == ReadOnlyStateTable[RootIndex]);
                WriteableStateTable.GetEnumerator();

                IReadOnlyDictionary<IReadOnlyIndex, IReadOnlyNodeState> ReadOnlyStateTableAsDictionary = ReadOnlyStateTable;
                Assert.That(ReadOnlyStateTable.TryGetValue(RootIndex, out IReadOnlyNodeState ReadOnlyRootStateValue) == ReadOnlyStateTableAsDictionary.TryGetValue(RootIndex, out IReadOnlyNodeState ReadOnlyRootStateValueFromDictionary) && ReadOnlyRootStateValue == ReadOnlyRootStateValueFromDictionary);
                Assert.That(ReadOnlyStateTableAsDictionary.Keys != null);
                Assert.That(ReadOnlyStateTableAsDictionary.Values != null);
                ReadOnlyStateTableAsDictionary.GetEnumerator();

                IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState> WriteableStateTableAsDictionary = ReadOnlyStateTable as IReadOnlyDictionary<IWriteableIndex, IWriteableNodeState>;
                Assert.That(WriteableStateTable.TryGetValue(RootIndex, out IReadOnlyNodeState WriteableRootStateValue) == WriteableStateTableAsDictionary.TryGetValue(RootIndex, out IWriteableNodeState WriteableRootStateValueFromDictionary) && WriteableRootStateValue == WriteableRootStateValueFromDictionary);
                Assert.That(WriteableStateTableAsDictionary.ContainsKey(RootIndex));
                Assert.That(WriteableStateTableAsDictionary[RootIndex] == ReadOnlyStateTable[RootIndex]);
                Assert.That(WriteableStateTableAsDictionary.Keys != null);
                Assert.That(WriteableStateTableAsDictionary.Values != null);

                IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>> WriteableStateTableAsEnumerable = ReadOnlyStateTable as IEnumerable<KeyValuePair<IWriteableIndex, IWriteableNodeState>>;
                WriteableStateTableAsEnumerable.GetEnumerator();

                // IFrameInnerDictionary

                FrameInnerDictionary<string> FrameInnerTableModify = DebugObjects.GetReferenceByInterface(Type.FromTypeof<FrameInnerDictionary<string>>()) as FrameInnerDictionary<string>;
                Assert.That(FrameInnerTableModify != null);
                Assert.That(FrameInnerTableModify.Count > 0);

                WriteableInnerDictionary<string> WriteableInnerTableModify = FrameInnerTableModify;
                WriteableInnerTableModify.GetEnumerator();

                IDictionary<string, IReadOnlyInner> ReadOnlyInnerTableModifyAsDictionary = FrameInnerTableModify;
                Assert.That(ReadOnlyInnerTableModifyAsDictionary.Keys != null);
                Assert.That(ReadOnlyInnerTableModifyAsDictionary.Values != null);

                foreach (KeyValuePair<string, IFrameInner> Entry in (ICollection<KeyValuePair<string, IFrameInner>>)FrameInnerTableModify)
                {
                    Assert.That(ReadOnlyInnerTableModifyAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(ReadOnlyInnerTableModifyAsDictionary[Entry.Key] == Entry.Value);
                }

                IDictionary<string, IWriteableInner> WriteableInnerTableModifyAsDictionary = FrameInnerTableModify;
                Assert.That(WriteableInnerTableModifyAsDictionary.Keys != null);
                Assert.That(WriteableInnerTableModifyAsDictionary.Values != null);

                foreach (KeyValuePair<string, IFrameInner> Entry in (ICollection<KeyValuePair<string, IFrameInner>>)FrameInnerTableModify)
                {
                    Assert.That(WriteableInnerTableModify[Entry.Key] == Entry.Value);
                    Assert.That(WriteableInnerTableModifyAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(WriteableInnerTableModifyAsDictionary[Entry.Key] == Entry.Value);
                    WriteableInnerTableModifyAsDictionary.Remove(Entry.Key);
                    WriteableInnerTableModifyAsDictionary.Add(Entry.Key, Entry.Value);
                    WriteableInnerTableModifyAsDictionary.TryGetValue(Entry.Key, out IWriteableInner InnerValue);
                    break;
                }

                ICollection<KeyValuePair<string, IReadOnlyInner>> ReadOnlyInnerTableModifyAsCollection = FrameInnerTableModify;
                Assert.That(!ReadOnlyInnerTableModifyAsCollection.IsReadOnly);

                ICollection<KeyValuePair<string, IWriteableInner>> WriteableInnerTableModifyAsCollection = FrameInnerTableModify;
                Assert.That(!WriteableInnerTableModifyAsCollection.IsReadOnly);
                WriteableInnerTableModifyAsCollection.CopyTo(new KeyValuePair<string, IWriteableInner>[WriteableInnerTableModifyAsCollection.Count], 0);

                foreach (KeyValuePair<string, IWriteableInner> Entry in WriteableInnerTableModifyAsCollection)
                {
                    Assert.That(WriteableInnerTableModifyAsCollection.Contains(Entry));
                    WriteableInnerTableModifyAsCollection.Remove(Entry);
                    WriteableInnerTableModifyAsCollection.Add(Entry);
                    break;
                }

                IEnumerable<KeyValuePair<string, IReadOnlyInner>> ReadOnlyInnerTableModifyAsEnumerable = FrameInnerTableModify;
                IEnumerator<KeyValuePair<string, IReadOnlyInner>> ReadOnlyInnerTableModifyAsEnumerableEnumerator = ReadOnlyInnerTableModifyAsEnumerable.GetEnumerator();

                foreach (KeyValuePair<string, IReadOnlyInner> Entry in ReadOnlyInnerTableModifyAsEnumerable)
                {
                    Assert.That(ReadOnlyInnerTableModifyAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(ReadOnlyInnerTableModifyAsDictionary[Entry.Key] == Entry.Value);
                    Assert.That(FrameInnerTableModify.TryGetValue(Entry.Key, out IReadOnlyInner ReadOnlyInnerValue) == FrameInnerTableModify.TryGetValue(Entry.Key, out IReadOnlyInner FrameInnerValue));

                    Assert.That(((ICollection<KeyValuePair<string, IReadOnlyInner>>)FrameInnerTableModify).Contains(Entry));
                    ((ICollection<KeyValuePair<string, IReadOnlyInner>>)FrameInnerTableModify).Remove(Entry);
                    ((ICollection<KeyValuePair<string, IReadOnlyInner>>)FrameInnerTableModify).Add(Entry);
                    ((ICollection<KeyValuePair<string, IReadOnlyInner>>)FrameInnerTableModify).CopyTo(new KeyValuePair<string, IReadOnlyInner>[FrameInnerTableModify.Count], 0);
                    break;
                }

                IEnumerable<KeyValuePair<string, IWriteableInner>> WriteableInnerTableModifyAsEnumerable = FrameInnerTableModify;
                IEnumerator<KeyValuePair<string, IWriteableInner>> WriteableInnerTableModifyAsEnumerableEnumerator = WriteableInnerTableModifyAsEnumerable.GetEnumerator();

                // IFrameInnerReadOnlyDictionary

                FrameInnerReadOnlyDictionary<string> FrameInnerTable = RootState.InnerTable;
                WriteableInnerReadOnlyDictionary<string> WriteableInnerTable = RootState.InnerTable;

                IReadOnlyDictionary<string, IReadOnlyInner> ReadOnlyInnerTableAsDictionary = FrameInnerTable;
                Assert.That(ReadOnlyInnerTableAsDictionary.Keys != null);
                Assert.That(ReadOnlyInnerTableAsDictionary.Values != null);

                IReadOnlyDictionary<string, IWriteableInner> WriteableInnerTableAsDictionary = FrameInnerTable;
                Assert.That(WriteableInnerTableAsDictionary.Keys != null);
                Assert.That(WriteableInnerTableAsDictionary.Values != null);

                IEnumerable<KeyValuePair<string, IWriteableInner>> WriteableInnerTableAsIEnumerable = FrameInnerTable;
                WriteableInnerTableAsIEnumerable.GetEnumerator();

                foreach (KeyValuePair<string, IFrameInner> Entry in (ICollection<KeyValuePair<string, IFrameInner>>)FrameInnerTable)
                {
                    Assert.That(WriteableInnerTableAsDictionary[Entry.Key] == Entry.Value);
                    Assert.That(FrameInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner ReadOnlyInnerValue) == FrameInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner FrameInnerValue));
                    Assert.That(FrameInnerTable.TryGetValue(Entry.Key, out IReadOnlyInner WriteableInnerValue) == FrameInnerTable.TryGetValue(Entry.Key, out FrameInnerValue));
                    break;
                }

                // FrameNodeStateList

                FirstNodeState = LeafPathInner.FirstNodeState;
                Assert.That(FirstNodeState != null);

                FrameNodeStateList FrameNodeStateListModify = DebugObjects.GetReferenceByInterface(Type.FromTypeof<FrameNodeStateList>()) as FrameNodeStateList;
                Assert.That(FrameNodeStateListModify != null);
                Assert.That(FrameNodeStateListModify.Count > 0);
                FirstNodeState = FrameNodeStateListModify[0] as IFramePlaceholderNodeState;
                Assert.That(FrameNodeStateListModify.Contains((IReadOnlyNodeState)FirstNodeState));
                Assert.That(FrameNodeStateListModify.IndexOf((IReadOnlyNodeState)FirstNodeState) == 0);

                FrameNodeStateListModify.Remove((IReadOnlyNodeState)FirstNodeState);
                FrameNodeStateListModify.Insert(0, (IReadOnlyNodeState)FirstNodeState);
                FrameNodeStateListModify.CopyTo((IReadOnlyNodeState[])(new IFrameNodeState[FrameNodeStateListModify.Count]), 0);

                ReadOnlyNodeStateList FrameNodeStateListModifyAsReadOnly = FrameNodeStateListModify as ReadOnlyNodeStateList;
                Assert.That(FrameNodeStateListModifyAsReadOnly != null);
                Assert.That(FrameNodeStateListModifyAsReadOnly[0] == FrameNodeStateListModify[0]);

                WriteableNodeStateList FrameNodeStateListModifyAsWriteable = FrameNodeStateListModify as WriteableNodeStateList;
                Assert.That(FrameNodeStateListModifyAsWriteable != null);
                Assert.That(FrameNodeStateListModifyAsWriteable[0] == FrameNodeStateListModify[0]);
                FrameNodeStateListModifyAsWriteable.GetEnumerator();

                IList<IReadOnlyNodeState> ReadOnlyNodeStateListModifyAsIList = FrameNodeStateListModify as IList<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListModifyAsIList != null);
                Assert.That(ReadOnlyNodeStateListModifyAsIList[0] == FrameNodeStateListModify[0]);

                IList<IWriteableNodeState> WriteableNodeStateListModifyAsIList = FrameNodeStateListModify as IList<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListModifyAsIList != null);
                Assert.That(WriteableNodeStateListModifyAsIList[0] == FrameNodeStateListModify[0]);
                Assert.That(WriteableNodeStateListModifyAsIList.IndexOf(FirstNodeState) == 0);
                WriteableNodeStateListModifyAsIList.Remove(FirstNodeState);
                WriteableNodeStateListModifyAsIList.Insert(0, FirstNodeState);

                IReadOnlyList<IReadOnlyNodeState> ReadOnlyNodeStateListModifyAsIReadOnlyList = FrameNodeStateListModify as IReadOnlyList<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListModifyAsIReadOnlyList != null);
                Assert.That(ReadOnlyNodeStateListModifyAsIReadOnlyList[0] == FrameNodeStateListModify[0]);

                IReadOnlyList<IWriteableNodeState> WriteableNodeStateListModifyAsIReadOnlyList = FrameNodeStateListModify as IReadOnlyList<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListModifyAsIReadOnlyList != null);
                Assert.That(WriteableNodeStateListModifyAsIReadOnlyList[0] == FrameNodeStateListModify[0]);

                ICollection<IReadOnlyNodeState> ReadOnlyNodeStateListModifyAsCollection = FrameNodeStateListModify as ICollection<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListModifyAsCollection != null);
                Assert.That(!ReadOnlyNodeStateListModifyAsCollection.IsReadOnly);

                ICollection<IWriteableNodeState> WriteableNodeStateListModifyAsCollection = FrameNodeStateListModify as ICollection<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListModifyAsCollection != null);
                Assert.That(!WriteableNodeStateListModifyAsCollection.IsReadOnly);
                Assert.That(WriteableNodeStateListModifyAsCollection.Contains(FirstNodeState));
                WriteableNodeStateListModifyAsCollection.Remove(FirstNodeState);
                WriteableNodeStateListModifyAsCollection.Add(FirstNodeState);
                WriteableNodeStateListModifyAsCollection.Remove(FirstNodeState);
                FrameNodeStateListModify.Insert(0, FirstNodeState);
                WriteableNodeStateListModifyAsCollection.CopyTo(new IFrameNodeState[WriteableNodeStateListModifyAsCollection.Count], 0);

                IEnumerable<IReadOnlyNodeState> ReadOnlyNodeStateListModifyAsEnumerable = FrameNodeStateListModify as IEnumerable<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListModifyAsEnumerable != null);
                Assert.That(ReadOnlyNodeStateListModifyAsEnumerable.GetEnumerator() != null);

                IEnumerable<IWriteableNodeState> WriteableNodeStateListModifyAsEnumerable = FrameNodeStateListModify as IEnumerable<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListModifyAsEnumerable != null);
                Assert.That(WriteableNodeStateListModifyAsEnumerable.GetEnumerator() != null);

                // FrameNodeStateReadOnlyList

                FrameNodeStateReadOnlyList FrameNodeStateList = FrameNodeStateListModify.ToReadOnly() as FrameNodeStateReadOnlyList;
                Assert.That(FrameNodeStateList != null);
                Assert.That(FrameNodeStateList.Count > 0);
                FirstNodeState = FrameNodeStateList[0] as IFramePlaceholderNodeState;
                Assert.That(FrameNodeStateList.Contains((IReadOnlyNodeState)FirstNodeState));
                Assert.That(FrameNodeStateList.IndexOf((IReadOnlyNodeState)FirstNodeState) == 0);

                WriteableNodeStateReadOnlyList WriteableNodeStateList = FrameNodeStateList;
                Assert.That(WriteableNodeStateList.Contains(FirstNodeState));
                Assert.That(WriteableNodeStateList.IndexOf(FirstNodeState) == 0);
                Assert.That(WriteableNodeStateList[0] == FrameNodeStateList[0]);
                WriteableNodeStateList.GetEnumerator();

                IReadOnlyList<IReadOnlyNodeState> ReadOnlyNodeStateListAsIReadOnlyList = FrameNodeStateList as IReadOnlyList<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListAsIReadOnlyList[0] == FirstNodeState);

                IReadOnlyList<IWriteableNodeState> WriteableNodeStateListAsIReadOnlyList = FrameNodeStateList as IReadOnlyList<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListAsIReadOnlyList[0] == FirstNodeState);

                IEnumerable<IReadOnlyNodeState> ReadOnlyNodeStateListAsEnumerable = FrameNodeStateList as IEnumerable<IReadOnlyNodeState>;
                Assert.That(ReadOnlyNodeStateListAsEnumerable != null);
                Assert.That(ReadOnlyNodeStateListAsEnumerable.GetEnumerator() != null);

                IEnumerable<IWriteableNodeState> WriteableNodeStateListAsEnumerable = FrameNodeStateList as IEnumerable<IWriteableNodeState>;
                Assert.That(WriteableNodeStateListAsEnumerable != null);
                Assert.That(WriteableNodeStateListAsEnumerable.GetEnumerator() != null);

                // IFrameOperationGroupList

                FrameOperationGroupReadOnlyList FrameOperationStack = Controller.OperationStack;

                Assert.That(FrameOperationStack.Count > 0);
                FrameOperationGroup FirstOperationGroup = (FrameOperationGroup)FrameOperationStack[0];

                FrameOperationGroupList FrameOperationGroupList = DebugObjects.GetReferenceByInterface(Type.FromTypeof<FrameOperationGroupList>()) as FrameOperationGroupList;
                if (FrameOperationGroupList != null)
                {
                    WriteableOperationGroupList WriteableOperationGroupList = FrameOperationGroupList;
                    Assert.That(WriteableOperationGroupList.Count > 0);
                    Assert.That(WriteableOperationGroupList[0] == FirstOperationGroup);

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
                    WriteableOperationGroupAsICollection.CopyTo(new FrameOperationGroup[WriteableOperationGroupAsICollection.Count], 0);

                    IEnumerable<WriteableOperationGroup> WriteableOperationGroupAsIEnumerable = WriteableOperationGroupList;
                    WriteableOperationGroupAsIEnumerable.GetEnumerator();

                    IReadOnlyList<WriteableOperationGroup> WriteableOperationGroupAsIReadOnlyList = WriteableOperationGroupList;
                    Assert.That(WriteableOperationGroupAsIReadOnlyList.Count > 0);
                    Assert.That(WriteableOperationGroupAsIReadOnlyList[0] == FirstOperationGroup);
                }

                // IFrameOperationGroupReadOnlyList

                WriteableOperationGroupReadOnlyList WriteableOperationStack = FrameOperationStack;
                Assert.That(WriteableOperationStack.Contains(FirstOperationGroup));
                Assert.That(WriteableOperationStack.IndexOf(FirstOperationGroup) == 0);

                IEnumerable<WriteableOperationGroup> WriteableOperationStackAsIEnumerable = WriteableOperationStack;
                WriteableOperationStackAsIEnumerable.GetEnumerator();

                // IFrameOperationList

                FrameOperationReadOnlyList FrameOperationReadOnlyList = FirstOperationGroup.OperationList;

                Assert.That(FrameOperationReadOnlyList.Count > 0);
                IFrameOperation FirstOperation = (IFrameOperation)FrameOperationReadOnlyList[0];

                FrameOperationList FrameOperationList = DebugObjects.GetReferenceByInterface(Type.FromTypeof<FrameOperationList>()) as FrameOperationList;
                if (FrameOperationList != null)
                {
                    WriteableOperationList WriteableOperationList = FrameOperationList;
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
                    WriteableOperationAsICollection.CopyTo(new IFrameOperation[WriteableOperationAsICollection.Count], 0);

                    IEnumerable<IWriteableOperation> WriteableOperationAsIEnumerable = WriteableOperationList;
                    WriteableOperationAsIEnumerable.GetEnumerator();

                    IReadOnlyList<IWriteableOperation> WriteableOperationAsIReadOnlyList = WriteableOperationList;
                    Assert.That(WriteableOperationAsIReadOnlyList.Count > 0);
                    Assert.That(WriteableOperationAsIReadOnlyList[0] == FirstOperation);
                }

                // IFrameOperationReadOnlyList
                WriteableOperationReadOnlyList WriteableOperationReadOnlyList = FrameOperationReadOnlyList;
                Assert.That(WriteableOperationReadOnlyList.Contains(FirstOperation));
                Assert.That(WriteableOperationReadOnlyList.IndexOf(FirstOperation) == 0);

                IEnumerable<IWriteableOperation> WriteableOperationReadOnlyListAsIEnumerable = WriteableOperationReadOnlyList;
                WriteableOperationReadOnlyListAsIEnumerable.GetEnumerator();

                // FramePlaceholderNodeStateList

                //System.Diagnostics.Debug.Assert(false);
                FirstNodeState = LeafPathInner.FirstNodeState;
                Assert.That(FirstNodeState != null);

                FramePlaceholderNodeStateList FramePlaceholderNodeStateListModify = DebugObjects.GetReferenceByInterface(Type.FromTypeof<FramePlaceholderNodeStateList>()) as FramePlaceholderNodeStateList;
                if (FramePlaceholderNodeStateListModify != null)
                {
                    Assert.That(FramePlaceholderNodeStateListModify.Count > 0);
                    FirstNodeState = FramePlaceholderNodeStateListModify[0] as IFramePlaceholderNodeState;
                    Assert.That(FramePlaceholderNodeStateListModify.Contains((IReadOnlyPlaceholderNodeState)FirstNodeState));
                    Assert.That(FramePlaceholderNodeStateListModify.IndexOf((IReadOnlyPlaceholderNodeState)FirstNodeState) == 0);

                    FramePlaceholderNodeStateListModify.Remove((IReadOnlyPlaceholderNodeState)FirstNodeState);
                    FramePlaceholderNodeStateListModify.Insert(0, (IReadOnlyPlaceholderNodeState)FirstNodeState);
                    FramePlaceholderNodeStateListModify.CopyTo((IReadOnlyPlaceholderNodeState[])(new IFramePlaceholderNodeState[FramePlaceholderNodeStateListModify.Count]), 0);

                    ReadOnlyPlaceholderNodeStateList FramePlaceholderNodeStateListModifyAsReadOnly = FramePlaceholderNodeStateListModify as ReadOnlyPlaceholderNodeStateList;
                    Assert.That(FramePlaceholderNodeStateListModifyAsReadOnly != null);
                    Assert.That(FramePlaceholderNodeStateListModifyAsReadOnly[0] == FramePlaceholderNodeStateListModify[0]);

                    WriteablePlaceholderNodeStateList FramePlaceholderNodeStateListModifyAsWriteable = FramePlaceholderNodeStateListModify as WriteablePlaceholderNodeStateList;
                    Assert.That(FramePlaceholderNodeStateListModifyAsWriteable != null);
                    Assert.That(FramePlaceholderNodeStateListModifyAsWriteable[0] == FramePlaceholderNodeStateListModify[0]);
                    FramePlaceholderNodeStateListModifyAsWriteable.GetEnumerator();

                    IList<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListModifyAsIList = FramePlaceholderNodeStateListModify as IList<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsIList != null);
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsIList[0] == FramePlaceholderNodeStateListModify[0]);

                    IList<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListModifyAsIList = FramePlaceholderNodeStateListModify as IList<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsIList != null);
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsIList[0] == FramePlaceholderNodeStateListModify[0]);
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsIList.IndexOf(FirstNodeState) == 0);
                    WriteablePlaceholderNodeStateListModifyAsIList.Remove(FirstNodeState);
                    WriteablePlaceholderNodeStateListModifyAsIList.Insert(0, FirstNodeState);

                    IReadOnlyList<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListModifyAsIReadOnlyList = FramePlaceholderNodeStateListModify as IReadOnlyList<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsIReadOnlyList != null);
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsIReadOnlyList[0] == FramePlaceholderNodeStateListModify[0]);

                    IReadOnlyList<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListModifyAsIReadOnlyList = FramePlaceholderNodeStateListModify as IReadOnlyList<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsIReadOnlyList != null);
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsIReadOnlyList[0] == FramePlaceholderNodeStateListModify[0]);

                    ICollection<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListModifyAsCollection = FramePlaceholderNodeStateListModify as ICollection<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsCollection != null);
                    Assert.That(!ReadOnlyPlaceholderNodeStateListModifyAsCollection.IsReadOnly);
                    ReadOnlyPlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    ReadOnlyPlaceholderNodeStateListModifyAsCollection.Add(FirstNodeState);
                    ReadOnlyPlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    WriteablePlaceholderNodeStateListModifyAsIList.Insert(0, FirstNodeState);

                    ICollection<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListModifyAsCollection = FramePlaceholderNodeStateListModify as ICollection<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsCollection != null);
                    Assert.That(!WriteablePlaceholderNodeStateListModifyAsCollection.IsReadOnly);
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsCollection.Contains(FirstNodeState));
                    WriteablePlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    WriteablePlaceholderNodeStateListModifyAsCollection.Add(FirstNodeState);
                    WriteablePlaceholderNodeStateListModifyAsCollection.Remove(FirstNodeState);
                    FramePlaceholderNodeStateListModify.Insert(0, FirstNodeState);
                    WriteablePlaceholderNodeStateListModifyAsCollection.CopyTo(new IFramePlaceholderNodeState[WriteablePlaceholderNodeStateListModifyAsCollection.Count], 0);

                    IEnumerable<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListModifyAsEnumerable = FramePlaceholderNodeStateListModify as IEnumerable<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsEnumerable != null);
                    Assert.That(ReadOnlyPlaceholderNodeStateListModifyAsEnumerable.GetEnumerator() != null);

                    IEnumerable<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListModifyAsEnumerable = FramePlaceholderNodeStateListModify as IEnumerable<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsEnumerable != null);
                    Assert.That(WriteablePlaceholderNodeStateListModifyAsEnumerable.GetEnumerator() != null);
                }

                // FramePlaceholderNodeStateReadOnlyList

                FramePlaceholderNodeStateReadOnlyList FramePlaceholderNodeStateList = FramePlaceholderNodeStateListModify != null ? FramePlaceholderNodeStateListModify.ToReadOnly() as FramePlaceholderNodeStateReadOnlyList : null;

                if (FramePlaceholderNodeStateList != null)
                {
                    Assert.That(FramePlaceholderNodeStateList.Count > 0);
                    FirstNodeState = FramePlaceholderNodeStateList[0] as IFramePlaceholderNodeState;
                    Assert.That(FramePlaceholderNodeStateList.Contains((IReadOnlyPlaceholderNodeState)FirstNodeState));
                    Assert.That(FramePlaceholderNodeStateList.IndexOf((IReadOnlyPlaceholderNodeState)FirstNodeState) == 0);

                    WriteablePlaceholderNodeStateReadOnlyList WriteablePlaceholderNodeStateList = FramePlaceholderNodeStateList;
                    Assert.That(WriteablePlaceholderNodeStateList.Contains(FirstNodeState));
                    Assert.That(WriteablePlaceholderNodeStateList.IndexOf(FirstNodeState) == 0);
                    Assert.That(WriteablePlaceholderNodeStateList[0] == FramePlaceholderNodeStateList[0]);
                    WriteablePlaceholderNodeStateList.GetEnumerator();

                    IReadOnlyList<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListAsIReadOnlyList = FramePlaceholderNodeStateList as IReadOnlyList<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListAsIReadOnlyList[0] == FirstNodeState);

                    IReadOnlyList<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListAsIReadOnlyList = FramePlaceholderNodeStateList as IReadOnlyList<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListAsIReadOnlyList[0] == FirstNodeState);

                    IEnumerable<IReadOnlyPlaceholderNodeState> ReadOnlyPlaceholderNodeStateListAsEnumerable = FramePlaceholderNodeStateList as IEnumerable<IReadOnlyPlaceholderNodeState>;
                    Assert.That(ReadOnlyPlaceholderNodeStateListAsEnumerable != null);
                    Assert.That(ReadOnlyPlaceholderNodeStateListAsEnumerable.GetEnumerator() != null);

                    IEnumerable<IWriteablePlaceholderNodeState> WriteablePlaceholderNodeStateListAsEnumerable = FramePlaceholderNodeStateList as IEnumerable<IWriteablePlaceholderNodeState>;
                    Assert.That(WriteablePlaceholderNodeStateListAsEnumerable != null);
                    Assert.That(WriteablePlaceholderNodeStateListAsEnumerable.GetEnumerator() != null);
                }

                // IFrameStateViewDictionary
                FrameNodeStateViewDictionary FrameStateViewTable = ControllerView.StateViewTable;
                WriteableNodeStateViewDictionary WriteableStateViewTable = ControllerView.StateViewTable;
                WriteableStateViewTable.GetEnumerator();

                IDictionary<IReadOnlyNodeState, IReadOnlyNodeStateView> ReadOnlyStateViewTableAsDictionary = FrameStateViewTable;
                Assert.That(ReadOnlyStateViewTableAsDictionary != null);
                Assert.That(ReadOnlyStateViewTableAsDictionary.TryGetValue(RootState, out IReadOnlyNodeStateView StateViewTableAsDictionaryValue) == FrameStateViewTable.TryGetValue(RootState, out IReadOnlyNodeStateView StateViewTableValue));
                Assert.That(ReadOnlyStateViewTableAsDictionary.Keys != null);
                Assert.That(ReadOnlyStateViewTableAsDictionary.Values != null);

                ICollection<KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>> ReadOnlyStateViewTableAsCollection = FrameStateViewTable;
                Assert.That(!ReadOnlyStateViewTableAsCollection.IsReadOnly);

                ICollection<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> WriteableStateViewTableAsCollection = FrameStateViewTable;
                Assert.That(!WriteableStateViewTableAsCollection.IsReadOnly);

                IDictionary<IWriteableNodeState, IWriteableNodeStateView> WriteableStateViewTableAsDictionary = FrameStateViewTable;
                Assert.That(WriteableStateViewTableAsDictionary != null);
                Assert.That(WriteableStateViewTableAsDictionary.TryGetValue(RootState, out IWriteableNodeStateView WriteableStateViewTableAsDictionaryValue) == FrameStateViewTable.TryGetValue(RootState, out StateViewTableValue));
                Assert.That(WriteableStateViewTableAsDictionary.Keys != null);
                Assert.That(WriteableStateViewTableAsDictionary.Values != null);

                foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in ReadOnlyStateViewTableAsCollection)
                {
                    Assert.That(ReadOnlyStateViewTableAsCollection.Contains(Entry));
                    ReadOnlyStateViewTableAsCollection.Remove(Entry);
                    ReadOnlyStateViewTableAsCollection.Add(Entry);
                    ReadOnlyStateViewTableAsCollection.CopyTo(new KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView>[FrameStateViewTable.Count], 0);

                    Assert.That(WriteableStateViewTableAsDictionary.ContainsKey((IWriteableNodeState)Entry.Key));
                    WriteableStateViewTableAsDictionary.Remove((IWriteableNodeState)Entry.Key);
                    WriteableStateViewTableAsDictionary.Add((IWriteableNodeState)Entry.Key, (IWriteableNodeStateView)Entry.Value);

                    break;
                }

                foreach (KeyValuePair<IWriteableNodeState, IWriteableNodeStateView> Entry in WriteableStateViewTableAsCollection)
                {
                    Assert.That(WriteableStateViewTableAsDictionary.ContainsKey(Entry.Key));
                    Assert.That(WriteableStateViewTableAsDictionary[Entry.Key] == Entry.Value);
                    WriteableStateViewTableAsDictionary.Remove(Entry.Key);
                    WriteableStateViewTableAsDictionary.Add(Entry.Key, Entry.Value);

                    Assert.That(WriteableStateViewTableAsCollection.Contains(Entry));
                    WriteableStateViewTableAsCollection.Remove(Entry);
                    WriteableStateViewTableAsCollection.Add(Entry);
                    WriteableStateViewTableAsCollection.CopyTo(new KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>[FrameStateViewTable.Count], 0);

                    break;
                }

                IEnumerable<KeyValuePair<IWriteableNodeState, IWriteableNodeStateView>> WriteableStateViewTableAsEnumerable = FrameStateViewTable;
                WriteableStateViewTableAsEnumerable.GetEnumerator();

            }
        }

        private static bool ListCellViews(IFrameVisibleCellView cellview, FrameVisibleCellViewList cellViewList)
        {
            cellViewList.Add(cellview);
            return false;
        }
    }
}
