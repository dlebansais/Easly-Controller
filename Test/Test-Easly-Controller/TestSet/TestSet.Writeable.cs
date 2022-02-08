namespace Test
{
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;
    using NUnit.Framework;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using PolySerializer;
    using System.Collections.Generic;
    using System;
    using EaslyController;
    using EaslyController.Writeable;
    using EaslyController.Frame;
    using Easly;
    using EaslyController.Focus;
    using TestDebug;
    using EaslyController.Layout;
    using EaslyEdit;
    using System.Text;

    [TestFixture]
    public partial class TestSet
    {
#if WRITEABLE
        [Test]
        [TestCaseSource(nameof(FileIndexRange))]
        public static void Writeable(int index)
        {
            if (TestOff)
                return;

            if (index >= FileNameTable.Count)
                throw new ArgumentOutOfRangeException($"{index} / {FileNameTable.Count}");

            string FileName = FileNameTable[index];
            Node RootNode;

            using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            {
                RootNode = DeserializeAndFix(fs);
            }

            TestWriteable(index, FileName, RootNode);
        }
#endif
        public static void TestWriteable(int index, string name, Node rootNode)
        {
            ControllerTools.ResetExpectedName();

            TestWriteableStats(index, name, rootNode, out Stats Stats);

            Random rand = new Random(0x123456);

            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, JustCount);
            WriteableMaxTestCount = WriteableTestCount;

            for (int i = 0; i < TestRepeatCount; i++)
            {
                TestWriteableInsert(index, rootNode);
                TestWriteableRemove(index, rootNode);
                TestWriteableRemoveBlockRange(index, rootNode);
                TestWriteableReplaceBlockRange(index, rootNode);
                TestWriteableInsertBlockRange(index, rootNode);
                TestWriteableRemoveNodeRange(index, rootNode);
                TestWriteableReplaceNodeRange(index, rootNode);
                TestWriteableInsertNodeRange(index, rootNode);
                TestWriteableReplace(index, rootNode);
                TestWriteableAssign(index, rootNode);
                TestWriteableUnassign(index, rootNode);
                TestWriteableChangeReplication(index, rootNode);
                TestWriteableChangeDiscreteValue(index, rootNode);
                TestWriteableSplit(index, rootNode);
                TestWriteableMerge(index, rootNode);
                TestWriteableMove(index, rootNode);
                TestWriteableMoveBlock(index, rootNode);
                TestWriteableExpand(index, rootNode);
                TestWriteableReduce(index, rootNode);
            }

            TestWriteableCanonicalize(rootNode);
        }

        static int WriteableTestCount = 0;
        static int WriteableMaxTestCount = 0;

        public static bool JustCount(IWriteableInner inner)
        {
            WriteableTestCount++;
            return true;
        }

        public static void TestWriteableStats(int index, string name, Node rootNode, out Stats stats)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);

            stats = new Stats();
            BrowseNode(Controller, RootIndex, stats);

            if (name.EndsWith("test.easly"))
            {
                const int ExpectedNodeCount = 155;
                const int ExpectedPlaceholderNodeCount = 142;
                const int ExpectedOptionalNodeCount = 12;
                const int ExpectedAssignedOptionalNodeCount = 4;
                const int ExpectedListCount = 5;
                const int ExpectedBlockListCount = 96;

                Assert.That(stats.NodeCount == ExpectedNodeCount, $"Failed to browse tree. Expected: {ExpectedNodeCount} node(s), Found: {stats.NodeCount}");
                Assert.That(stats.PlaceholderNodeCount == ExpectedPlaceholderNodeCount, $"Failed to browse tree. Expected: {ExpectedPlaceholderNodeCount} placeholder node(s), Found: {stats.PlaceholderNodeCount}");
                Assert.That(stats.OptionalNodeCount == ExpectedOptionalNodeCount, $"Failed to browse tree. Expected: {ExpectedOptionalNodeCount } optional node(s), Found: {stats.OptionalNodeCount}");
                Assert.That(stats.AssignedOptionalNodeCount == ExpectedAssignedOptionalNodeCount, $"Failed to browse tree. Expected: {ExpectedAssignedOptionalNodeCount} assigned optional node(s), Found: {stats.AssignedOptionalNodeCount}");
                Assert.That(stats.ListCount == ExpectedListCount, $"Failed to browse tree. Expected: {ExpectedListCount} list(s), Found: {stats.ListCount}");
                Assert.That(stats.BlockListCount == ExpectedBlockListCount, $"Failed to browse tree. Expected: {ExpectedBlockListCount} block list(s), Found: {stats.BlockListCount}");
            }

            Assert.That(Controller.Stats.NodeCount == stats.NodeCount, $"Invalid controller state. Expected: {stats.NodeCount} node(s), Found: {Controller.Stats.NodeCount}");
            Assert.That(Controller.Stats.PlaceholderNodeCount == stats.PlaceholderNodeCount, $"Invalid controller state. Expected: {stats.PlaceholderNodeCount} placeholder node(s), Found: {Controller.Stats.PlaceholderNodeCount}");
            Assert.That(Controller.Stats.OptionalNodeCount == stats.OptionalNodeCount, $"Invalid controller state. Expected: {stats.OptionalNodeCount } optional node(s), Found: {Controller.Stats.OptionalNodeCount}");
            Assert.That(Controller.Stats.AssignedOptionalNodeCount == stats.AssignedOptionalNodeCount, $"Invalid controller state. Expected: {stats.AssignedOptionalNodeCount } assigned optional node(s), Found: {Controller.Stats.AssignedOptionalNodeCount}");
            Assert.That(Controller.Stats.ListCount == stats.ListCount, $"Invalid controller state. Expected: {stats.ListCount} list(s), Found: {Controller.Stats.ListCount}");
            Assert.That(Controller.Stats.BlockListCount == stats.BlockListCount, $"Invalid controller state. Expected: {stats.BlockListCount} block list(s), Found: {Controller.Stats.BlockListCount}");
        }

        public static void TestWriteableInsert(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => InsertAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool InsertAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteableListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    Node NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    int Index = RandNext(AsListInner.StateList.Count + 1);
                    IWriteableInsertionListNodeIndex NodeIndex = new WriteableInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, Index);
                    Controller.Insert(AsListInner, NodeIndex, out IWriteableBrowsingCollectionNodeIndex InsertedIndex);
                    Assert.That(Controller.Contains(InsertedIndex));

                    IWriteablePlaceholderNodeState ChildState = (IWriteablePlaceholderNodeState)Controller.IndexToState(InsertedIndex);
                    Assert.That(ChildState != null);
                    Assert.That(ChildState.Node == NewNode);

                    IsModified = true;
                }
            }
            else if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    Node NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    if (RandNext(2) == 0)
                    {
                        int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        IWriteableBlockState BlockState = (IWriteableBlockState)AsBlockListInner.BlockStateList[BlockIndex];
                        int Index = RandNext(BlockState.StateList.Count + 1);

                        IWriteableInsertionExistingBlockNodeIndex NodeIndex = new WriteableInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, Index);
                        Controller.Insert(AsBlockListInner, NodeIndex, out IWriteableBrowsingCollectionNodeIndex InsertedIndex);
                        Assert.That(Controller.Contains(InsertedIndex));

                        IWriteablePlaceholderNodeState ChildState = (IWriteablePlaceholderNodeState)Controller.IndexToState(InsertedIndex);
                        Assert.That(ChildState != null);
                        Assert.That(ChildState.Node == NewNode);

                        IsModified = true;
                    }
                    else
                    {
                        int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count + 1);

                        Pattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                        Identifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
                        IWriteableInsertionNewBlockNodeIndex NodeIndex = new WriteableInsertionNewBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, ReplicationPattern, SourceIdentifier);
                        Controller.Insert(AsBlockListInner, NodeIndex, out IWriteableBrowsingCollectionNodeIndex InsertedIndex);
                        Assert.That(Controller.Contains(InsertedIndex));

                        IWriteablePlaceholderNodeState ChildState = (IWriteablePlaceholderNodeState)Controller.IndexToState(InsertedIndex);
                        Assert.That(ChildState != null);
                        Assert.That(ChildState.Node == NewNode);

                        IsModified = true;
                    }
                }
            }

            if (IsModified)
            {
                WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableRemove(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => RemoveAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool RemoveAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteableListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int Index = RandNext(AsListInner.StateList.Count);
                    IWriteableNodeState ChildState = (IWriteableNodeState)AsListInner.StateList[Index];
                    IWriteableBrowsingListNodeIndex NodeIndex = (IWriteableBrowsingListNodeIndex)ChildState.ParentIndex;
                    Assert.That(NodeIndex != null);

                    if (Controller.IsRemoveable(AsListInner, NodeIndex))
                    {
                        Controller.Remove(AsListInner, NodeIndex);
                        IsModified = true;
                    }
                }
            }
            else if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IWriteableBlockState BlockState = (IWriteableBlockState)AsBlockListInner.BlockStateList[BlockIndex];
                    int Index = RandNext(BlockState.StateList.Count);
                    IWriteableNodeState ChildState = (IWriteableNodeState)BlockState.StateList[Index];
                    IWriteableBrowsingExistingBlockNodeIndex NodeIndex = (IWriteableBrowsingExistingBlockNodeIndex)ChildState.ParentIndex;
                    Assert.That(NodeIndex != null);

                    if (Controller.IsRemoveable(AsBlockListInner, NodeIndex))
                    {
                        Controller.Remove(AsBlockListInner, NodeIndex);
                        IsModified = true;
                    }
                }
            }

            if (IsModified)
            {
                WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
                
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableRemoveBlockRange(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => RemoveBlockRangeAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool RemoveBlockRangeAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int FirstBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int LastBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count + 1);

                    if (FirstBlockIndex > LastBlockIndex)
                        FirstBlockIndex = LastBlockIndex;

                    if (Controller.IsBlockRangeRemoveable(AsBlockListInner, FirstBlockIndex, LastBlockIndex))
                    {
                        Controller.RemoveBlockRange(AsBlockListInner, FirstBlockIndex, LastBlockIndex);
                        IsModified = FirstBlockIndex < LastBlockIndex;
                    }
                }
            }

            if (IsModified)
            {
                WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableReplaceBlockRange(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => ReplaceBlockRangeAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool ReplaceBlockRangeAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int FirstBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int LastBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count + 1);

                    if (FirstBlockIndex > LastBlockIndex)
                        FirstBlockIndex = LastBlockIndex;

                    Node NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    Pattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                    Identifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
                    IWriteableInsertionNewBlockNodeIndex NewNodeIndex = new WriteableInsertionNewBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, FirstBlockIndex, ReplicationPattern, SourceIdentifier);

                    NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IWriteableInsertionExistingBlockNodeIndex ExistingNodeIndex = new WriteableInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, FirstBlockIndex, 1);

                    List<IWriteableInsertionBlockNodeIndex> IndexList = new List<IWriteableInsertionBlockNodeIndex>() { NewNodeIndex, ExistingNodeIndex };
                    Controller.ReplaceBlockRange(AsBlockListInner, FirstBlockIndex, LastBlockIndex, IndexList);
                    IsModified = true;
                }
            }

            if (IsModified)
            {
                WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableInsertBlockRange(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => InsertBlockRangeAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool InsertBlockRangeAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int InsertedIndex = RandNext(AsBlockListInner.BlockStateList.Count);

                    Node NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    Pattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                    Identifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
                    IWriteableInsertionNewBlockNodeIndex NewNodeIndex = new WriteableInsertionNewBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, InsertedIndex, ReplicationPattern, SourceIdentifier);

                    NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IWriteableInsertionExistingBlockNodeIndex ExistingNodeIndex = new WriteableInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, InsertedIndex, 1);

                    List<IWriteableInsertionBlockNodeIndex> IndexList = new List<IWriteableInsertionBlockNodeIndex>() { NewNodeIndex, ExistingNodeIndex };
                    Controller.InsertBlockRange(AsBlockListInner, InsertedIndex, IndexList);
                    IsModified = true;
                }
            }

            if (IsModified)
            {
                WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableRemoveNodeRange(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => RemoveNodeRangeAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool RemoveNodeRangeAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int FirstNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count);
                    int LastNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count + 1);

                    if (FirstNodeIndex > LastNodeIndex)
                        FirstNodeIndex = LastNodeIndex;

                    if (Controller.IsNodeRangeRemoveable(AsBlockListInner, BlockIndex, FirstNodeIndex, LastNodeIndex))
                    {
                        Controller.RemoveNodeRange(AsBlockListInner,BlockIndex, FirstNodeIndex, LastNodeIndex);
                        IsModified = FirstNodeIndex < LastNodeIndex;
                    }
                }
            }

            else if (inner is IWriteableListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int BlockIndex = -1;
                    int FirstNodeIndex = RandNext(AsListInner.StateList.Count);
                    int LastNodeIndex = RandNext(AsListInner.StateList.Count + 1);

                    if (FirstNodeIndex > LastNodeIndex)
                        FirstNodeIndex = LastNodeIndex;

                    if (Controller.IsNodeRangeRemoveable(AsListInner, BlockIndex, FirstNodeIndex, LastNodeIndex))
                    {
                        Controller.RemoveNodeRange(AsListInner, BlockIndex, FirstNodeIndex, LastNodeIndex);
                        IsModified = FirstNodeIndex < LastNodeIndex;
                    }
                }
            }

            if (IsModified)
            {
                WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableReplaceNodeRange(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => ReplaceNodeRangeAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool ReplaceNodeRangeAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int FirstNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count);
                    int LastNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count + 1);

                    if (FirstNodeIndex > LastNodeIndex)
                        FirstNodeIndex = LastNodeIndex;

                    Node NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IWriteableInsertionExistingBlockNodeIndex ExistingNodeIndex = new WriteableInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, FirstNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    if (Controller.IsNodeRangeRemoveable(AsBlockListInner, BlockIndex, FirstNodeIndex, LastNodeIndex))
                    {
                        Controller.ReplaceNodeRange(AsBlockListInner, BlockIndex, FirstNodeIndex, LastNodeIndex, IndexList);
                        IsModified = IndexList.Count > 0 || FirstNodeIndex < LastNodeIndex;
                    }
                }
            }

            else if (inner is IWriteableListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int BlockIndex = -1;
                    int FirstNodeIndex = RandNext(AsListInner.StateList.Count);
                    int LastNodeIndex = RandNext(AsListInner.StateList.Count + 1);

                    if (FirstNodeIndex > LastNodeIndex)
                        FirstNodeIndex = LastNodeIndex;

                    Node NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    IWriteableInsertionListNodeIndex ExistingNodeIndex = new WriteableInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, FirstNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    if (Controller.IsNodeRangeRemoveable(AsListInner, BlockIndex, FirstNodeIndex, LastNodeIndex))
                    {
                        Controller.ReplaceNodeRange(AsListInner, BlockIndex, FirstNodeIndex, LastNodeIndex, IndexList);
                        IsModified = IndexList.Count > 0 || FirstNodeIndex < LastNodeIndex;
                    }
                }
            }

            if (IsModified)
            {
                WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableInsertNodeRange(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => InsertNodeRangeAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool InsertNodeRangeAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int InsertedNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count);

                    Node NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IWriteableInsertionExistingBlockNodeIndex ExistingNodeIndex = new WriteableInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, InsertedNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    Controller.InsertNodeRange(AsBlockListInner, BlockIndex, InsertedNodeIndex, IndexList);
                    IsModified = true;
                }
            }

            else if (inner is IWriteableListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int BlockIndex = -1;
                    int InsertedNodeIndex = RandNext(AsListInner.StateList.Count);

                    Node NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    IWriteableInsertionListNodeIndex ExistingNodeIndex = new WriteableInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, InsertedNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    Controller.InsertNodeRange(AsListInner, BlockIndex, InsertedNodeIndex, IndexList);
                    IsModified = true;
                }
            }

            if (IsModified)
            {
                WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableReplace(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => ReplaceAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool ReplaceAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteablePlaceholderInner AsPlaceholderInner)
            {
                Node NewNode = NodeHelper.DeepCloneNode(AsPlaceholderInner.ChildState.Node, cloneCommentGuid: false);
                Assert.That(NewNode != null, $"Type: {AsPlaceholderInner.InterfaceType}");

                IWriteableInsertionPlaceholderNodeIndex NodeIndex = new WriteableInsertionPlaceholderNodeIndex(AsPlaceholderInner.Owner.Node, AsPlaceholderInner.PropertyName, NewNode);
                Controller.Replace(AsPlaceholderInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                Assert.That(Controller.Contains(InsertedIndex));

                IWriteablePlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as IWriteablePlaceholderNodeState;
                Assert.That(ChildState != null);
                Assert.That(ChildState.Node == NewNode);

                IsModified = true;
            }
            else if (inner is IWriteableOptionalInner AsOptionalInner)
            {
                IWriteableOptionalNodeState State = AsOptionalInner.ChildState;
                IOptionalReference Optional = State.ParentIndex.Optional;
                Type NodeInterfaceType = Optional.GetType().GetGenericArguments()[0];
                Node NewNode = NodeHelper.CreateDefaultFromType(NodeInterfaceType);
                Assert.That(NewNode != null, $"Type: {AsOptionalInner.InterfaceType}");

                IWriteableInsertionOptionalNodeIndex NodeIndex = new WriteableInsertionOptionalNodeIndex(AsOptionalInner.Owner.Node, AsOptionalInner.PropertyName, NewNode);
                Controller.Replace(AsOptionalInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                Assert.That(Controller.Contains(InsertedIndex));

                IWriteableOptionalNodeState ChildState = Controller.IndexToState(InsertedIndex) as IWriteableOptionalNodeState;
                Assert.That(ChildState != null);
                Assert.That(ChildState.Node == NewNode);

                IsModified = true;
            }
            else if (inner is IWriteableListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    Node NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    int Index = RandNext(AsListInner.StateList.Count);
                    IWriteableInsertionListNodeIndex NodeIndex = new WriteableInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, Index);
                    Controller.Replace(AsListInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                    Assert.That(Controller.Contains(InsertedIndex));

                    IWriteablePlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as IWriteablePlaceholderNodeState;
                    Assert.That(ChildState != null);
                    Assert.That(ChildState.Node == NewNode);

                    IsModified = true;
                }
            }
            else if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    Node NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IWriteableBlockState BlockState = (IWriteableBlockState)AsBlockListInner.BlockStateList[BlockIndex];
                    int Index = RandNext(BlockState.StateList.Count);

                    IWriteableInsertionExistingBlockNodeIndex NodeIndex = new WriteableInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, Index);
                    Controller.Replace(AsBlockListInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                    Assert.That(Controller.Contains(InsertedIndex));

                    IWriteablePlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as IWriteablePlaceholderNodeState;
                    Assert.That(ChildState != null);
                    Assert.That(ChildState.Node == NewNode);

                    IsModified = true;
                }
            }

            if (IsModified)
            {
                WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableAssign(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => AssignAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool AssignAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteableOptionalInner AsOptionalInner)
            {
                IWriteableOptionalNodeState ChildState = AsOptionalInner.ChildState;
                Assert.That(ChildState != null);

                IWriteableBrowsingOptionalNodeIndex OptionalIndex = ChildState.ParentIndex;
                Assert.That(Controller.Contains(OptionalIndex));

                IOptionalReference Optional = OptionalIndex.Optional;
                Assert.That(Optional != null);

                Controller.Assign(OptionalIndex, out bool IsChanged);
                Assert.That(Optional.IsAssigned);
                Assert.That(AsOptionalInner.IsAssigned);
                Assert.That(Optional.Item == ChildState.Node);

                WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                if (IsChanged)
                {
                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestWriteableUnassign(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => UnassignAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool UnassignAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteableOptionalInner AsOptionalInner)
            {
                IWriteableOptionalNodeState ChildState = AsOptionalInner.ChildState;
                Assert.That(ChildState != null);

                IWriteableBrowsingOptionalNodeIndex OptionalIndex = ChildState.ParentIndex;
                Assert.That(Controller.Contains(OptionalIndex));

                IOptionalReference Optional = OptionalIndex.Optional;
                Assert.That(Optional != null);

                Controller.Unassign(OptionalIndex, out bool IsChanged);
                Assert.That(!Optional.IsAssigned);
                Assert.That(!AsOptionalInner.IsAssigned);

                WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                if (IsChanged)
                {
                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestWriteableChangeReplication(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => ChangeReplicationAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool ChangeReplicationAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IWriteableBlockState BlockState = (IWriteableBlockState)AsBlockListInner.BlockStateList[BlockIndex];

                    ReplicationStatus Replication = (ReplicationStatus)RandNext(2);
                    Controller.ChangeReplication(AsBlockListInner, BlockIndex, Replication);

                    WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                    WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestWriteableChangeDiscreteValue(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseValues(Controller, RootIndex, (IWriteableIndex nodeIndex, string propertyName) => ChangeDiscreteValueAndCompare(ControllerView, RandNext(WriteableMaxTestCount), nodeIndex, propertyName));
        }

        static bool ChangeDiscreteValueAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableIndex nodeIndex, string propertyName)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            IWriteableNodeState State = (IWriteableNodeState)Controller.IndexToState(nodeIndex);
            State.PropertyToValue(propertyName, out object Value, out int MinValue, out int MaxValue);

            int OldValue = (int)Value;
            int NewValue = OldValue + 1;
            if (NewValue > MaxValue)
                NewValue = MinValue;

            Controller.ChangeDiscreteValue(nodeIndex, propertyName, OldValue);

            WriteableControllerView NewView = WriteableControllerView.Create(Controller);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            Controller.Undo();

            Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Property: {propertyName}, Old Value: {OldValue}");
            WriteableControllerView OldView = WriteableControllerView.Create(Controller);
            Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

            return false;
        }

        public static void TestWriteableSplit(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => SplitAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool SplitAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int SplitBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IWriteableBlockState BlockState = (IWriteableBlockState)AsBlockListInner.BlockStateList[SplitBlockIndex];
                    if (BlockState.StateList.Count > 1)
                    {
                        int SplitIndex = 1 + RandNext(BlockState.StateList.Count - 1);
                        IWriteableBrowsingExistingBlockNodeIndex NodeIndex = (IWriteableBrowsingExistingBlockNodeIndex)AsBlockListInner.IndexAt(SplitBlockIndex, SplitIndex);
                        Controller.SplitBlock(AsBlockListInner, NodeIndex);

                        WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                        Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                        IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                        WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                        Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                        Controller.Undo();

                        Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                        WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                        Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                        Controller.Redo();

                        Assert.That(AsBlockListInner.BlockStateList.Count > 0);
                        int OldBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        int NewBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        int Direction = NewBlockIndex - OldBlockIndex;

                        Assert.That(Controller.IsBlockMoveable(AsBlockListInner, OldBlockIndex, Direction));

                        Controller.MoveBlock(AsBlockListInner, OldBlockIndex, Direction);

                        WriteableControllerView NewViewAfterMove = WriteableControllerView.Create(Controller);
                        Assert.That(NewViewAfterMove.IsEqual(CompareEqual.New(), controllerView));

                        IWriteableRootNodeIndex NewRootIndexAfterMove = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                        WriteableController NewControllerAfterMove = (WriteableController)WriteableController.Create(NewRootIndexAfterMove);
                        Assert.That(NewControllerAfterMove.IsEqual(CompareEqual.New(), Controller));
                    }
                }
            }

            return false;
        }

        public static void TestWriteableMerge(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => MergeAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool MergeAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 1)
                {
                    int MergeBlockIndex = 1 + RandNext(AsBlockListInner.BlockStateList.Count - 1);
                    IWriteableBlockState BlockState = (IWriteableBlockState)AsBlockListInner.BlockStateList[MergeBlockIndex];

                    IWriteableBrowsingExistingBlockNodeIndex NodeIndex = (IWriteableBrowsingExistingBlockNodeIndex)AsBlockListInner.IndexAt(MergeBlockIndex, 0);
                    Controller.MergeBlocks(AsBlockListInner, NodeIndex);

                    WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                    WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestWriteableMove(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => MoveAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool MoveAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            bool IsModified = false;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteableListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int OldIndex = RandNext(AsListInner.StateList.Count);
                    int NewIndex = RandNext(AsListInner.StateList.Count);
                    int Direction = NewIndex - OldIndex;

                    IWriteableBrowsingListNodeIndex NodeIndex = AsListInner.IndexAt(OldIndex) as IWriteableBrowsingListNodeIndex;
                    Assert.That(NodeIndex != null);

                    Assert.That(Controller.IsMoveable(AsListInner, NodeIndex, Direction));

                    Controller.Move(AsListInner, NodeIndex, Direction);
                    Assert.That(Controller.Contains(NodeIndex));

                    IsModified = true;
                }
            }
            else if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IWriteableBlockState BlockState = (IWriteableBlockState)AsBlockListInner.BlockStateList[BlockIndex];

                    int OldIndex = RandNext(BlockState.StateList.Count);
                    int NewIndex = RandNext(BlockState.StateList.Count);
                    int Direction = NewIndex - OldIndex;

                    IWriteableBrowsingExistingBlockNodeIndex NodeIndex = AsBlockListInner.IndexAt(BlockIndex, OldIndex) as IWriteableBrowsingExistingBlockNodeIndex;
                    Assert.That(NodeIndex != null);

                    Assert.That(Controller.IsMoveable(AsBlockListInner, NodeIndex, Direction));

                    Controller.Move(AsBlockListInner, NodeIndex, Direction);
                    Assert.That(Controller.Contains(NodeIndex));

                    IsModified = true;
                }
            }

            if (IsModified)
            {
                WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableMoveBlock(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => MoveBlockAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool MoveBlockAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 1)
                {
                    int OldIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int NewIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int Direction = NewIndex - OldIndex;

                    Assert.That(Controller.IsBlockMoveable(AsBlockListInner, OldIndex, Direction));

                    Controller.MoveBlock(AsBlockListInner, OldIndex, Direction);

                    WriteableControllerView NewView = WriteableControllerView.Create(Controller);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                    WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestWriteableExpand(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => ExpandAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool ExpandAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            IWriteableNodeIndex NodeIndex;
            IWriteablePlaceholderNodeState State;

            if (inner is IWriteablePlaceholderInner AsPlaceholderInner)
            {
                NodeIndex = AsPlaceholderInner.ChildState.ParentIndex as IWriteableNodeIndex;
                Assert.That(NodeIndex != null);

                State = Controller.IndexToState(NodeIndex) as IWriteablePlaceholderNodeState;
                Assert.That(State != null);

                NodeTreeHelper.GetArgumentBlocks(State.Node, out IDictionary<string, IBlockList<Argument>> ArgumentBlocksTable);
                if (ArgumentBlocksTable.Count == 0)
                    return true;
            }
            else
                return true;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            Controller.Expand(NodeIndex, out bool IsChanged);

            WriteableControllerView NewView = WriteableControllerView.Create(Controller);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                Controller.Redo();
            }

            Controller.Expand(NodeIndex, out IsChanged);

            NewController = (WriteableController)WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            Controller.Reduce(NodeIndex, out IsChanged);

            NewController = (WriteableController)WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            return false;
        }

        public static void TestWriteableReduce(int index, Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => ReduceAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool ReduceAndCompare(WriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            WriteableController Controller = controllerView.Controller;
            IWriteableNodeIndex NodeIndex;
            IWriteablePlaceholderNodeState State;

            if (inner is IWriteablePlaceholderInner AsPlaceholderInner)
            {
                NodeIndex = AsPlaceholderInner.ChildState.ParentIndex as IWriteableNodeIndex;
                Assert.That(NodeIndex != null);

                State = Controller.IndexToState(NodeIndex) as IWriteablePlaceholderNodeState;
                Assert.That(State != null);

                NodeTreeHelper.GetArgumentBlocks(State.Node, out IDictionary<string, IBlockList<Argument>> ArgumentBlocksTable);
                if (ArgumentBlocksTable.Count == 0)
                    return true;
            }
            else
                return true;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            Controller.Reduce(NodeIndex, out bool IsChanged);

            WriteableControllerView NewView = WriteableControllerView.Create(Controller);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                Controller.Redo();
            }

            Controller.Reduce(NodeIndex, out IsChanged);

            NewController = (WriteableController)WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            Controller.Expand(NodeIndex, out IsChanged);

            NewController = (WriteableController)WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            return false;
        }

        public static void TestWriteableCanonicalize(Node rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            WriteableController Controller = (WriteableController)WriteableController.Create(RootIndex);
            WriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController OldController = (WriteableController)WriteableController.Create(OldRootIndex);

            Controller.Canonicalize(out bool IsChanged);

            WriteableControllerView NewView = WriteableControllerView.Create(Controller);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            WriteableController NewController = (WriteableController)WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
            
            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Root Node: {rootNode}");
                WriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), ControllerView));
            }
        }

        static bool WriteableBrowseNode(WriteableController controller, IWriteableIndex index, Func<IWriteableInner, bool> test)
        {
            Assert.That(index != null, "Writeable #0");
            Assert.That(controller.Contains(index), "Writeable #1");
            IWriteableNodeState State = (IWriteableNodeState)controller.IndexToState(index);
            Assert.That(State != null, "Writeable #2");
            Assert.That(State.ParentIndex == index, "Writeable #4");

            Node Node;

            if (State is IWriteablePlaceholderNodeState AsPlaceholderState)
                Node = AsPlaceholderState.Node;
            else if (State is IWriteablePatternState AsPatternState)
                Node = AsPatternState.Node;
            else if (State is IWriteableSourceState AsSourceState)
                Node = AsSourceState.Node;
            else
            {
                Assert.That(State is IWriteableOptionalNodeState, "Writeable #5");
                IWriteableOptionalNodeState AsOptionalState = (IWriteableOptionalNodeState)State;
                IWriteableOptionalInner ParentInner = AsOptionalState.ParentInner;

                Assert.That(ParentInner.IsAssigned, "Writeable #6");

                Node = AsOptionalState.Node;
            }

            Type ChildNodeType;
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(Node);

            foreach (string PropertyName in PropertyNames)
            {
                if (NodeTreeHelperChild.IsChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    IWriteablePlaceholderInner Inner = (IWriteablePlaceholderInner)State.PropertyToInner(PropertyName);
                    if (!test(Inner))
                        return false;

                    IWriteableNodeState ChildState = Inner.ChildState;
                    IWriteableIndex ChildIndex = ChildState.ParentIndex;
                    if (!WriteableBrowseNode(controller, ChildIndex, test))
                        return false;
                }

                else if (NodeTreeHelperOptional.IsOptionalChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelperOptional.GetChildNode(Node, PropertyName, out bool IsAssigned, out Node ChildNode);
                    if (IsAssigned)
                    {
                        IWriteableOptionalInner Inner = (IWriteableOptionalInner)State.PropertyToInner(PropertyName);
                        if (!test(Inner))
                            return false;

                        IWriteableNodeState ChildState = Inner.ChildState;
                        IWriteableIndex ChildIndex = ChildState.ParentIndex;
                        if (!WriteableBrowseNode(controller, ChildIndex, test))
                            return false;
                    }
                }

                else if (NodeTreeHelperList.IsNodeListProperty(Node, PropertyName, out ChildNodeType))
                {
                    IWriteableListInner Inner = (IWriteableListInner)State.PropertyToInner(PropertyName);
                    if (!test(Inner))
                        return false;

                    for (int i = 0; i < Inner.StateList.Count; i++)
                    {
                        IWriteablePlaceholderNodeState ChildState = (IWriteablePlaceholderNodeState)Inner.StateList[i];
                        IWriteableIndex ChildIndex = ChildState.ParentIndex;
                        if (!WriteableBrowseNode(controller, ChildIndex, test))
                            return false;
                    }
                }

                else if (NodeTreeHelperBlockList.IsBlockListProperty(Node, PropertyName, /*out Type ChildInterfaceType,*/ out ChildNodeType))
                {
                    IWriteableBlockListInner Inner = (IWriteableBlockListInner)State.PropertyToInner(PropertyName);
                    if (!test(Inner))
                        return false;

                    for (int BlockIndex = 0; BlockIndex < Inner.BlockStateList.Count; BlockIndex++)
                    {
                        IWriteableBlockState BlockState = (IWriteableBlockState)Inner.BlockStateList[BlockIndex];
                        if (!WriteableBrowseNode(controller, BlockState.PatternIndex, test))
                            return false;
                        if (!WriteableBrowseNode(controller, BlockState.SourceIndex, test))
                            return false;

                        for (int i = 0; i < BlockState.StateList.Count; i++)
                        {
                            IWriteablePlaceholderNodeState ChildState = (IWriteablePlaceholderNodeState)BlockState.StateList[i];
                            IWriteableIndex ChildIndex = ChildState.ParentIndex;
                            if (!WriteableBrowseNode(controller, ChildIndex, test))
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        static bool WriteableBrowseValues(WriteableController controller, IWriteableIndex index, Func<IWriteableIndex, string, bool> test)
        {
            Assert.That(index != null, "Writeable #7");
            Assert.That(controller.Contains(index), "Writeable #8");
            IWriteableNodeState State = (IWriteableNodeState)controller.IndexToState(index);
            Assert.That(State != null, "Writeable #9");
            Assert.That(State.ParentIndex == index, "Writeable #10");

            Node Node;

            if (State is IWriteablePlaceholderNodeState AsPlaceholderState)
                Node = AsPlaceholderState.Node;
            else if (State is IWriteablePatternState AsPatternState)
                Node = AsPatternState.Node;
            else if (State is IWriteableSourceState AsSourceState)
                Node = AsSourceState.Node;
            else
            {
                Assert.That(State is IWriteableOptionalNodeState, "Writeable #11");
                IWriteableOptionalNodeState AsOptionalState = (IWriteableOptionalNodeState)State;
                IWriteableOptionalInner ParentInner = AsOptionalState.ParentInner;

                Assert.That(ParentInner.IsAssigned, "Writeable #12");

                Node = AsOptionalState.Node;
            }

            Type ChildNodeType;
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(Node);

            foreach (string PropertyName in PropertyNames)
            {
                if (NodeTreeHelper.IsEnumProperty(Node, PropertyName))
                {
                    if (!test(index, PropertyName))
                        return false;
                }

                else if (NodeTreeHelperChild.IsChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    IWriteablePlaceholderInner Inner = (IWriteablePlaceholderInner)State.PropertyToInner(PropertyName);

                    IWriteableNodeState ChildState = Inner.ChildState;
                    IWriteableIndex ChildIndex = ChildState.ParentIndex;
                    if (!WriteableBrowseValues(controller, ChildIndex, test))
                        return false;
                }

                else if (NodeTreeHelperOptional.IsOptionalChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelperOptional.GetChildNode(Node, PropertyName, out bool IsAssigned, out Node ChildNode);
                    if (IsAssigned)
                    {
                        IWriteableOptionalInner Inner = (IWriteableOptionalInner)State.PropertyToInner(PropertyName);

                        IWriteableNodeState ChildState = Inner.ChildState;
                        IWriteableIndex ChildIndex = ChildState.ParentIndex;
                        if (!WriteableBrowseValues(controller, ChildIndex, test))
                            return false;
                    }
                }

                else if (NodeTreeHelperList.IsNodeListProperty(Node, PropertyName, out ChildNodeType))
                {
                    IWriteableListInner Inner = (IWriteableListInner)State.PropertyToInner(PropertyName);

                    for (int i = 0; i < Inner.StateList.Count; i++)
                    {
                        IWriteablePlaceholderNodeState ChildState = (IWriteablePlaceholderNodeState)Inner.StateList[i];
                        IWriteableIndex ChildIndex = ChildState.ParentIndex;
                        if (!WriteableBrowseValues(controller, ChildIndex, test))
                            return false;
                    }
                }

                else if (NodeTreeHelperBlockList.IsBlockListProperty(Node, PropertyName, /*out Type ChildInterfaceType,*/ out ChildNodeType))
                {
                    IWriteableBlockListInner Inner = (IWriteableBlockListInner)State.PropertyToInner(PropertyName);

                    for (int BlockIndex = 0; BlockIndex < Inner.BlockStateList.Count; BlockIndex++)
                    {
                        IWriteableBlockState BlockState = (IWriteableBlockState)Inner.BlockStateList[BlockIndex];
                        if (!WriteableBrowseValues(controller, BlockState.PatternIndex, test))
                            return false;
                        if (!WriteableBrowseValues(controller, BlockState.SourceIndex, test))
                            return false;

                        for (int i = 0; i < BlockState.StateList.Count; i++)
                        {
                            IWriteablePlaceholderNodeState ChildState = (IWriteablePlaceholderNodeState)BlockState.StateList[i];
                            IWriteableIndex ChildIndex = ChildState.ParentIndex;
                            if (!WriteableBrowseValues(controller, ChildIndex, test))
                                return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
