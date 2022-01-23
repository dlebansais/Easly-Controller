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
        public static void ReadOnlyCreation()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IReadOnlyRootNodeIndex RootIndex;
            ReadOnlyController Controller;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

            try
            {
                RootIndex = new ReadOnlyRootNodeIndex(RootNode);
                Controller = ReadOnlyController.Create(RootIndex);
            }
            catch (Exception e)
            {
                Assert.Fail($"#0: {e}");
            }

            RootNode = CreateRoot(ValueGuid0, Imperfections.BadGuid);
            Assert.That(!BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode, throwOnInvalid: false));

            try
            {
                RootIndex = new ReadOnlyRootNodeIndex(RootNode);
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
        public static void ReadOnlyProperties()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IReadOnlyRootNodeIndex RootIndex0;
            IReadOnlyRootNodeIndex RootIndex1;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(RootNode));

            RootIndex0 = new ReadOnlyRootNodeIndex(RootNode);
            Assert.That(RootIndex0.Node == RootNode);
            Assert.That(RootIndex0.IsEqual(CompareEqual.New(), RootIndex0));

            RootIndex1 = new ReadOnlyRootNodeIndex(RootNode);
            Assert.That(RootIndex1.Node == RootNode);
            Assert.That(CompareEqual.CoverIsEqual(RootIndex0, RootIndex1));

            ReadOnlyController Controller0 = ReadOnlyController.Create(RootIndex0);
            Assert.That(Controller0.RootIndex == RootIndex0);

            Stats Stats = Controller0.Stats;
            Assert.That(Stats.NodeCount >= 0);
            Assert.That(Stats.PlaceholderNodeCount >= 0);
            Assert.That(Stats.OptionalNodeCount >= 0);
            Assert.That(Stats.AssignedOptionalNodeCount >= 0);
            Assert.That(Stats.ListCount >= 0);
            Assert.That(Stats.BlockListCount >= 0);
            Assert.That(Stats.BlockCount >= 0);

            IReadOnlyPlaceholderNodeState RootState = Controller0.RootState;
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

            IReadOnlyPlaceholderInner MainPlaceholderTreeInner = (IReadOnlyPlaceholderInner)RootState.PropertyToInner(nameof(Main.PlaceholderTree));
            Assert.That(MainPlaceholderTreeInner != null);
            if (MainPlaceholderTreeInner != null)
            {
                Assert.That(MainPlaceholderTreeInner.InterfaceType == typeof(Tree));
                Assert.That(MainPlaceholderTreeInner.ChildState != null);
                Assert.That(MainPlaceholderTreeInner.ChildState?.ParentInner == MainPlaceholderTreeInner);
            }

            IReadOnlyPlaceholderInner MainPlaceholderLeafInner = (IReadOnlyPlaceholderInner)RootState.PropertyToInner(nameof(Main.PlaceholderLeaf));
            Assert.That(MainPlaceholderLeafInner != null);
            if (MainPlaceholderLeafInner != null)
            {
                Assert.That(MainPlaceholderLeafInner.InterfaceType == typeof(Leaf));
                Assert.That(MainPlaceholderLeafInner.ChildState != null);
                Assert.That(MainPlaceholderLeafInner.ChildState?.ParentInner == MainPlaceholderLeafInner);
            }

            IReadOnlyOptionalInner MainUnassignedOptionalInner = (IReadOnlyOptionalInner)RootState.PropertyToInner(nameof(Main.UnassignedOptionalLeaf));
            Assert.That(MainUnassignedOptionalInner != null);
            if (MainUnassignedOptionalInner != null)
            {
                Assert.That(MainUnassignedOptionalInner.InterfaceType == typeof(Leaf));
                Assert.That(!MainUnassignedOptionalInner.IsAssigned);
                Assert.That(MainUnassignedOptionalInner.ChildState != null);
                Assert.That(MainUnassignedOptionalInner.ChildState?.ParentInner == MainUnassignedOptionalInner);
            }

            IReadOnlyOptionalInner MainAssignedOptionalTreeInner = (IReadOnlyOptionalInner)RootState.PropertyToInner(nameof(Main.AssignedOptionalTree));
            Assert.That(MainAssignedOptionalTreeInner != null);
            if (MainAssignedOptionalTreeInner != null)
            {
                Assert.That(MainAssignedOptionalTreeInner.InterfaceType == typeof(Tree));
                Assert.That(MainAssignedOptionalTreeInner.IsAssigned);
            }

            IReadOnlyNodeState AssignedOptionalTreeState = MainAssignedOptionalTreeInner.ChildState;
            Assert.That(AssignedOptionalTreeState != null);
            if (AssignedOptionalTreeState != null)
            {
                Assert.That(AssignedOptionalTreeState.ParentInner == MainAssignedOptionalTreeInner);
                Assert.That(AssignedOptionalTreeState.ParentState == RootState);
            }

            ReadOnlyNodeStateReadOnlyList AssignedOptionalTreeAllChildren = AssignedOptionalTreeState.GetAllChildren();
            Assert.That(AssignedOptionalTreeAllChildren.Count == 2, $"New count: {AssignedOptionalTreeAllChildren.Count}");

            IReadOnlyOptionalInner MainAssignedOptionalLeafInner = (IReadOnlyOptionalInner)RootState.PropertyToInner(nameof(Main.AssignedOptionalLeaf));
            Assert.That(MainAssignedOptionalLeafInner != null);
            if (MainAssignedOptionalLeafInner != null)
            {
                Assert.That(MainAssignedOptionalLeafInner.InterfaceType == typeof(Leaf));
                Assert.That(MainAssignedOptionalLeafInner.IsAssigned);
                Assert.That(MainAssignedOptionalLeafInner.ChildState != null);
                Assert.That(MainAssignedOptionalLeafInner.ChildState.ParentInner == MainAssignedOptionalLeafInner);
            }

            IReadOnlyBlockListInner MainLeafBlocksInner = (IReadOnlyBlockListInner)RootState.PropertyToInner(nameof(Main.LeafBlocks));
            Assert.That(MainLeafBlocksInner != null);
            if (MainLeafBlocksInner != null)
            {
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
            }

            IReadOnlyBlockState LeafBlock = MainLeafBlocksInner.BlockStateList[0];
            Assert.That(LeafBlock != null);
            if (LeafBlock != null)
            {
                Assert.That(LeafBlock.StateList != null);
                Assert.That(LeafBlock.StateList.Count == 1);
                Assert.That(MainLeafBlocksInner.FirstNodeState == LeafBlock.StateList[0]);
                Assert.That(MainLeafBlocksInner.IndexAt(0, 0) == MainLeafBlocksInner.FirstNodeState.ParentIndex);
                Assert.That(LeafBlock.Comment == "");
            }

            IReadOnlyPlaceholderInner PatternInner = (IReadOnlyPlaceholderInner)LeafBlock.PropertyToInner(nameof(BaseNode.IBlock.ReplicationPattern));
            Assert.That(PatternInner != null);

            IReadOnlyPlaceholderInner SourceInner = (IReadOnlyPlaceholderInner)LeafBlock.PropertyToInner(nameof(BaseNode.IBlock.SourceIdentifier));
            Assert.That(SourceInner != null);

            IReadOnlyPatternState PatternState = LeafBlock.PatternState;
            Assert.That(PatternState != null);
            if (PatternState != null)
            {
                Assert.That(PatternState.ParentInner == PatternInner);
                Assert.That(PatternState.ParentIndex == LeafBlock.PatternIndex);
            }

            IReadOnlySourceState SourceState = LeafBlock.SourceState;
            Assert.That(SourceState != null);
            if (SourceState != null)
            {
                Assert.That(SourceState.ParentInner == SourceInner);
                Assert.That(SourceState.ParentIndex == LeafBlock.SourceIndex);
            }

            Assert.That(MainLeafBlocksInner.FirstNodeState == LeafBlock.StateList[0]);

            IReadOnlyListInner MainLeafPathInner = (IReadOnlyListInner)RootState.PropertyToInner(nameof(Main.LeafPath));
            Assert.That(MainLeafPathInner != null);
            if (MainLeafPathInner != null)
            {
                Assert.That(!MainLeafPathInner.IsNeverEmpty);
                Assert.That(MainLeafPathInner.InterfaceType == typeof(Leaf));
                Assert.That(MainLeafPathInner.Count == 2);
                Assert.That(MainLeafPathInner.StateList != null);
                Assert.That(MainLeafPathInner.StateList?.Count == 2);
                Assert.That(MainLeafPathInner.FirstNodeState == MainLeafPathInner.StateList[0]);
                Assert.That(MainLeafPathInner.IndexAt(0) == MainLeafPathInner.FirstNodeState.ParentIndex);
                Assert.That(MainLeafPathInner.AllIndexes().Count == MainLeafPathInner.Count);
            }

            ReadOnlyNodeStateReadOnlyList AllChildren = RootState.GetAllChildren();
            Assert.That(AllChildren.Count == 19, $"New count: {AllChildren.Count}");

            IReadOnlyPlaceholderInner PlaceholderInner = (IReadOnlyPlaceholderInner)RootState.InnerTable[nameof(Main.PlaceholderLeaf)];
            Assert.That(PlaceholderInner != null);

            IReadOnlyBrowsingPlaceholderNodeIndex PlaceholderNodeIndex = (IReadOnlyBrowsingPlaceholderNodeIndex)PlaceholderInner.ChildState.ParentIndex;
            Assert.That(PlaceholderNodeIndex != null);
            Assert.That(Controller0.Contains(PlaceholderNodeIndex));

            IReadOnlyOptionalInner UnassignedOptionalInner = (IReadOnlyOptionalInner)RootState.InnerTable[nameof(Main.UnassignedOptionalLeaf)];
            Assert.That(UnassignedOptionalInner != null);

            IReadOnlyBrowsingOptionalNodeIndex UnassignedOptionalNodeIndex = UnassignedOptionalInner.ChildState.ParentIndex;
            Assert.That(UnassignedOptionalNodeIndex != null);
            Assert.That(Controller0.Contains(UnassignedOptionalNodeIndex));
            Assert.That(Controller0.IsAssigned(UnassignedOptionalNodeIndex) == false);

            IReadOnlyOptionalInner AssignedOptionalInner = (IReadOnlyOptionalInner)RootState.InnerTable[nameof(Main.AssignedOptionalLeaf)];
            Assert.That(AssignedOptionalInner != null);

            IReadOnlyBrowsingOptionalNodeIndex AssignedOptionalNodeIndex = AssignedOptionalInner.ChildState.ParentIndex;
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
            
            ReadOnlyController Controller1 = ReadOnlyController.Create(RootIndex0);
            Assert.That(Controller0.IsEqual(CompareEqual.New(), Controller0));

            Assert.That(CompareEqual.CoverIsEqual(Controller0, Controller1));
        }

        [Test]
        [Category("Coverage")]
        public static void ReadOnlyClone()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode = CreateRoot(ValueGuid0, Imperfections.None);

            IReadOnlyRootNodeIndex RootIndex = new ReadOnlyRootNodeIndex(RootNode);
            Assert.That(RootIndex != null);

            ReadOnlyController Controller = ReadOnlyController.Create(RootIndex);
            Assert.That(Controller != null);

            IReadOnlyPlaceholderNodeState RootState = Controller.RootState;
            Assert.That(RootState != null);

            BaseNode.Node ClonedNode = RootState.CloneNode();
            Assert.That(BaseNodeHelper.NodeTreeDiagnostic.IsValid(ClonedNode));

            IReadOnlyRootNodeIndex CloneRootIndex = new ReadOnlyRootNodeIndex(ClonedNode);
            Assert.That(CloneRootIndex != null);

            ReadOnlyController CloneController = ReadOnlyController.Create(CloneRootIndex);
            Assert.That(CloneController != null);

            IReadOnlyPlaceholderNodeState CloneRootState = Controller.RootState;
            Assert.That(CloneRootState != null);

            ReadOnlyNodeStateReadOnlyList AllChildren = RootState.GetAllChildren();
            ReadOnlyNodeStateReadOnlyList CloneAllChildren = CloneRootState.GetAllChildren();
            Assert.That(AllChildren.Count == CloneAllChildren.Count);
        }

        [Test]
        [Category("Coverage")]
        public static void ReadOnlyViews()
        {
            ControllerTools.ResetExpectedName();

            Main RootNode;
            IReadOnlyRootNodeIndex RootIndex;

            RootNode = CreateRoot(ValueGuid0, Imperfections.None);
            RootIndex = new ReadOnlyRootNodeIndex(RootNode);

            ReadOnlyController Controller = ReadOnlyController.Create(RootIndex);

            using (ReadOnlyControllerView ControllerView0 = ReadOnlyControllerView.Create(Controller))
            {
                Assert.That(ControllerView0.Controller == Controller);
                Assert.That(ControllerView0.RootStateView == ControllerView0.StateViewTable[Controller.RootState]);

                using (ReadOnlyControllerView ControllerView1 = ReadOnlyControllerView.Create(Controller))
                {
                    Assert.That(ControllerView0.IsEqual(CompareEqual.New(), ControllerView0));
                    Assert.That(CompareEqual.CoverIsEqual(ControllerView0, ControllerView1));
                }

                foreach (KeyValuePair<IReadOnlyBlockState, ReadOnlyBlockStateView> Entry in ControllerView0.BlockStateViewTable)
                {
                    IReadOnlyBlockState BlockState = Entry.Key;
                    Assert.That(BlockState != null);

                    ReadOnlyBlockStateView BlockStateView = Entry.Value;
                    Assert.That(BlockStateView != null);

                    Assert.That(BlockStateView.ControllerView == ControllerView0);
                }

                foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in ControllerView0.StateViewTable)
                {
                    IReadOnlyNodeState State = Entry.Key;
                    Assert.That(State != null);
                    Assert.That(State.Comment != null);

                    IReadOnlyNodeStateView StateView = Entry.Value;
                    Assert.That(StateView != null);
                    Assert.That(StateView.State == State);

                    IReadOnlyIndex ParentIndex = State.ParentIndex;
                    Assert.That(ParentIndex != null);

                    Assert.That(Controller.Contains(ParentIndex));
                    Assert.That(StateView.ControllerView == ControllerView0);

                    switch (StateView)
                    {
                        case ReadOnlyPatternStateView AsPatternStateView:
                            Assert.That(AsPatternStateView.State == State);
                            break;

                        case ReadOnlySourceStateView AsSourceStateView:
                            Assert.That(AsSourceStateView.State == State);
                            break;

                        case ReadOnlyPlaceholderNodeStateView AsPlaceholderNodeStateView:
                            Assert.That(AsPlaceholderNodeStateView.State == State);
                            break;

                        case ReadOnlyOptionalNodeStateView AsOptionalNodeStateView:
                            Assert.That(AsOptionalNodeStateView.State == State);
                            break;
                    }
                }
            }
        }
    }
}
