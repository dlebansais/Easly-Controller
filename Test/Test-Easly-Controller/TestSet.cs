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

namespace Test
{
    [TestFixture]
    public class TestSet
    {
        #region Setup
        [OneTimeSetUp]
        public static void InitTestSession()
        {
            CultureInfo enUS = CultureInfo.CreateSpecificCulture("en-US");
            CultureInfo.DefaultThreadCurrentCulture = enUS;
            CultureInfo.DefaultThreadCurrentUICulture = enUS;
            Thread.CurrentThread.CurrentCulture = enUS;
            Thread.CurrentThread.CurrentUICulture = enUS;

            Assembly EaslyControllerAssembly;

            try
            {
                EaslyControllerAssembly = Assembly.Load("Easly-Controller");
            }
            catch
            {
                EaslyControllerAssembly = null;
            }
            Assume.That(EaslyControllerAssembly != null);

            string RootPath;
            if (File.Exists("./Easly-Controller/bin/x64/Travis/test.easly"))
                RootPath = "./Easly-Controller/bin/x64/Travis/";
            else
                RootPath = "./";

            FileNameTable = new List<string>();
            FirstRootNode = null;
            AddEaslyFiles(RootPath);
        }

        static void AddEaslyFiles(string path)
        {
            foreach (string FileName in Directory.GetFiles(path, "*.easly"))
            {
                FileNameTable.Add(FileName.Replace("\\", "/"));

                if (FirstRootNode == null)
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    {
                        Serializer Serializer = new Serializer();
                        INode RootNode = (INode)Serializer.Deserialize(fs);

                        FirstRootNode = RootNode;
                    }
                }
            }

            foreach (string Folder in Directory.GetDirectories(path))
                AddEaslyFiles(Folder);
        }

        static IEnumerable<int> FileIndexRange()
        {
            for (int i = 0; i < 172; i++)
                yield return i;
        }

        static int RandValue = 0;

        static void SeedRand(int seed)
        {
            RandValue = seed;
        }

        static int RandNext(int maxValue)
        {
            RandValue = (int)(5478541UL + (ulong)RandValue * 872143693217UL);
            if (RandValue < 0)
                RandValue = -RandValue;

            return RandValue % maxValue;
        }

        static List<string> FileNameTable;
        static INode FirstRootNode;
        #endregion

        static bool TestOff = false;
        const int TestRepeatCount = 5;

        #region Sanity Check
        [Test]
        public static void TestInit()
        {
            if (TestOff)
                return;

            ControllerTools.ResetExpectedName();

            IReadOnlyRootNodeIndex RootIndex = new ReadOnlyRootNodeIndex(FirstRootNode);
            IReadOnlyController Controller = ReadOnlyController.Create(RootIndex);

            Assert.That(Controller != null, "Sanity Check #0");

            if (Controller != null)
            {
                Assert.That(Controller.RootIndex == RootIndex, "Sanity Check #1");
                Assert.That(Controller.RootState != null, "Sanity Check #2");
                Assert.That(Controller.RootState?.Node == FirstRootNode, "Sanity Check #3");
                Assert.That(Controller.Contains(RootIndex), "Sanity Check #4");
                Assert.That(Controller.IndexToState(RootIndex) == Controller.RootState, "Sanity Check #5");
            }
        }
        #endregion

        #region ReadOnly
        #if READONLY
        [Test]
        [TestCaseSource(nameof(FileIndexRange))]
        public static void ReadOnly(int index)
        {
            if (TestOff)
                return;

            string Name = null;
            INode RootNode = null;
            int n = index;
            foreach (string FileName in FileNameTable)
            {
                if (n == 0)
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    {
                        Name = FileName;
                        Serializer Serializer = new Serializer();
                        RootNode = Serializer.Deserialize(fs) as INode;
                    }
                    break;
                }

                n--;
            }

            if (n > 0)
                throw new ArgumentOutOfRangeException($"{n} / {FileNameTable.Count}");
            TestReadOnly(index, Name, RootNode);
        }
        #endif

        public static void TestReadOnly(int index, string name, INode rootNode)
        {
            ControllerTools.ResetExpectedName();

            TestReadOnlyStats(index, name, rootNode);
        }

        public static void TestReadOnlyStats(int index, string name, INode rootNode)
        {
            IReadOnlyRootNodeIndex RootIndex = new ReadOnlyRootNodeIndex(rootNode);
            IReadOnlyController Controller = ReadOnlyController.Create(RootIndex);

            Stats Stats = new Stats();
            BrowseNode(Controller, RootIndex, Stats);

            if (name.EndsWith("test.easly"))
            {
                const int ExpectedNodeCount = 155;
                const int ExpectedPlaceholderNodeCount = 142;
                const int ExpectedOptionalNodeCount = 12;
                const int ExpectedAssignedOptionalNodeCount = 4;
                const int ExpectedListCount = 5;
                const int ExpectedBlockListCount = 96;

                Assert.That(Stats.NodeCount == ExpectedNodeCount, $"{name} - Failed to browse tree. Expected: {ExpectedNodeCount} node(s), Found: {Stats.NodeCount}");
                Assert.That(Stats.PlaceholderNodeCount == ExpectedPlaceholderNodeCount, $"Failed to browse tree. Expected: {ExpectedPlaceholderNodeCount} placeholder node(s), Found: {Stats.PlaceholderNodeCount}");
                Assert.That(Stats.OptionalNodeCount == ExpectedOptionalNodeCount, $"Failed to browse tree. Expected: {ExpectedOptionalNodeCount } optional node(s), Found: {Stats.OptionalNodeCount}");
                Assert.That(Stats.AssignedOptionalNodeCount == ExpectedAssignedOptionalNodeCount, $"Failed to browse tree. Expected: {ExpectedAssignedOptionalNodeCount} assigned optional node(s), Found: {Stats.AssignedOptionalNodeCount}");
                Assert.That(Stats.ListCount == ExpectedListCount, $"Failed to browse tree. Expected: {ExpectedListCount} list(s), Found: {Stats.ListCount}");
                Assert.That(Stats.BlockListCount == ExpectedBlockListCount, $"Failed to browse tree. Expected: {ExpectedBlockListCount} block list(s), Found: {Stats.BlockListCount}");
            }

            Assert.That(Controller.Stats.NodeCount == Stats.NodeCount, $"Invalid controller state. Expected: {Stats.NodeCount} node(s), Found: {Controller.Stats.NodeCount}");
            Assert.That(Controller.Stats.PlaceholderNodeCount == Stats.PlaceholderNodeCount, $"Invalid controller state. Expected: {Stats.PlaceholderNodeCount} placeholder node(s), Found: {Controller.Stats.PlaceholderNodeCount}");
            Assert.That(Controller.Stats.OptionalNodeCount == Stats.OptionalNodeCount, $"Invalid controller state. Expected: {Stats.OptionalNodeCount } optional node(s), Found: {Controller.Stats.OptionalNodeCount}");
            Assert.That(Controller.Stats.AssignedOptionalNodeCount == Stats.AssignedOptionalNodeCount, $"Invalid controller state. Expected: {Stats.AssignedOptionalNodeCount } assigned optional node(s), Found: {Controller.Stats.AssignedOptionalNodeCount}");
            Assert.That(Controller.Stats.ListCount == Stats.ListCount, $"Invalid controller state. Expected: {Stats.ListCount} list(s), Found: {Controller.Stats.ListCount}");
            Assert.That(Controller.Stats.BlockListCount == Stats.BlockListCount, $"Invalid controller state. Expected: {Stats.BlockListCount} block list(s), Found: {Controller.Stats.BlockListCount}");
        }

        static void BrowseNode(IReadOnlyController controller, IReadOnlyIndex index, Stats stats)
        {
            Assert.That(index != null, "ReadOnly #0");
            Assert.That(controller.Contains(index), "ReadOnly #1");
            IReadOnlyNodeState State = controller.IndexToState(index);
            Assert.That(State != null, "ReadOnly #2");
            Assert.That(State.ParentIndex == index, "ReadOnly #4");

            INode Node;

            if (State is IReadOnlyPlaceholderNodeState AsPlaceholderState)
                Node = AsPlaceholderState.Node;
            else if (State is IReadOnlyPatternState AsPatternState)
                Node = AsPatternState.Node;
            else if (State is IReadOnlySourceState AsSourceState)
                Node = AsSourceState.Node;
            else
            {
                Assert.That(State is IReadOnlyOptionalNodeState, "ReadOnly #5");
                IReadOnlyOptionalNodeState AsOptionalState = (IReadOnlyOptionalNodeState)State;
                IReadOnlyOptionalInner ParentInner = AsOptionalState.ParentInner;

                Assert.That(ParentInner.IsAssigned, "ReadOnly #6");

                Node = AsOptionalState.Node;
            }

            stats.NodeCount++;

            Type ChildNodeType;
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(Node);

            foreach (string PropertyName in PropertyNames)
            {
                if (NodeTreeHelperChild.IsChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    stats.PlaceholderNodeCount++;

                    IReadOnlyPlaceholderInner Inner = (IReadOnlyPlaceholderInner)State.PropertyToInner(PropertyName);
                    IReadOnlyNodeState ChildState = Inner.ChildState;
                    IReadOnlyIndex ChildIndex = ChildState.ParentIndex;
                    BrowseNode(controller, ChildIndex, stats);
                }

                else if (NodeTreeHelperOptional.IsOptionalChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    stats.OptionalNodeCount++;

                    NodeTreeHelperOptional.GetChildNode(Node, PropertyName, out bool IsAssigned, out INode ChildNode);
                    if (IsAssigned)
                    {
                        stats.AssignedOptionalNodeCount++;

                        IReadOnlyOptionalInner Inner = (IReadOnlyOptionalInner)State.PropertyToInner(PropertyName);
                        IReadOnlyNodeState ChildState = Inner.ChildState;
                        IReadOnlyIndex ChildIndex = ChildState.ParentIndex;
                        BrowseNode(controller, ChildIndex, stats);
                    }
                    else
                        stats.NodeCount++;
                }

                else if (NodeTreeHelperList.IsNodeListProperty(Node, PropertyName, out ChildNodeType))
                {
                    stats.ListCount++;

                    IReadOnlyListInner Inner = (IReadOnlyListInner)State.PropertyToInner(PropertyName);

                    for (int i = 0; i < Inner.StateList.Count; i++)
                    {
                        stats.PlaceholderNodeCount++;

                        IReadOnlyPlaceholderNodeState ChildState = Inner.StateList[i];
                        IReadOnlyIndex ChildIndex = ChildState.ParentIndex;
                        BrowseNode(controller, ChildIndex, stats);
                    }
                }

                else if (NodeTreeHelperBlockList.IsBlockListProperty(Node, PropertyName, out Type ChildInterfaceType, out ChildNodeType))
                {
                    stats.BlockListCount++;

                    IReadOnlyBlockListInner Inner = (IReadOnlyBlockListInner)State.PropertyToInner(PropertyName);

                    for (int BlockIndex = 0; BlockIndex < Inner.BlockStateList.Count; BlockIndex++)
                    {
                        IReadOnlyBlockState BlockState = Inner.BlockStateList[BlockIndex];

                        stats.PlaceholderNodeCount++;
                        BrowseNode(controller, BlockState.PatternIndex, stats);

                        stats.PlaceholderNodeCount++;
                        BrowseNode(controller, BlockState.SourceIndex, stats);

                        for (int i = 0; i < BlockState.StateList.Count; i++)
                        {
                            stats.PlaceholderNodeCount++;

                            IReadOnlyPlaceholderNodeState ChildState = BlockState.StateList[i];
                            IReadOnlyIndex ChildIndex = ChildState.ParentIndex;
                            BrowseNode(controller, ChildIndex, stats);
                        }
                    }
                }

                else
                {
                    Type NodeType = Node.GetType();
                    PropertyInfo Info = NodeType.GetProperty(PropertyName);

                    if (Info.PropertyType == typeof(IDocument))
                    {
                    }
                    else if (Info.PropertyType == typeof(bool))
                    {
                    }
                    else if (Info.PropertyType.IsEnum)
                    {
                    }
                    else if (Info.PropertyType == typeof(string))
                    {
                    }
                    else if (Info.PropertyType == typeof(Guid))
                    {
                    }
                    else
                    {
                        Assert.That(false, $"State Tree unexpected property: {Info.PropertyType.Name}");
                    }
                }
            }
        }
        #endregion

        #region Views
        #if VIEWS
        [Test]
        [TestCaseSource(nameof(FileIndexRange))]
        public static void StateViews(int index)
        {
            if (TestOff)
                return;

            string Name = null;
            INode RootNode = null;
            int n = index;
            foreach (string FileName in FileNameTable)
            {
                if (n == 0)
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    {
                        Name = FileName;
                        Serializer Serializer = new Serializer();
                        RootNode = Serializer.Deserialize(fs) as INode;
                    }
                    break;
                }

                n--;
            }

            if (n > 0)
                throw new ArgumentOutOfRangeException($"{n} / {FileNameTable.Count}");
            TestStateView(index, RootNode);
        }
        #endif

        public static void TestStateView(int index, INode rootNode)
        {
            ControllerTools.ResetExpectedName();

            IReadOnlyRootNodeIndex RootIndex = new ReadOnlyRootNodeIndex(rootNode);
            IReadOnlyController Controller = ReadOnlyController.Create(RootIndex);
            IReadOnlyControllerView ControllerView = ReadOnlyControllerView.Create(Controller);

            Assert.That(ControllerView.StateViewTable.ContainsKey(Controller.RootState), $"Views #0");
            Assert.That(ControllerView.StateViewTable.Count == Controller.Stats.NodeCount, $"Views #1");

            foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in ControllerView.StateViewTable)
            {
                IReadOnlyNodeState State = Entry.Key;
                Assert.That(ControllerView.StateViewTable.ContainsKey(Controller.RootState), $"Views #2, state={State}");

                IReadOnlyNodeStateView View = Entry.Value;
                Assert.That(View.State == State, $"Views #3");
            }

            IReadOnlyControllerView ControllerView2 = ReadOnlyControllerView.Create(Controller);
            Assert.That(ControllerView2.IsEqual(CompareEqual.New(), ControllerView), $"Views #4");
        }
        #endregion

        #region Writeable
        #if WRITEABLE
        [Test]
        [TestCaseSource(nameof(FileIndexRange))]
        public static void Writeable(int index)
        {
            if (TestOff)
                return;

            string Name = null;
            INode RootNode = null;
            int n = index;
            foreach (string FileName in FileNameTable)
            {
                if (n == 0)
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    {
                        Name = FileName;
                        Serializer Serializer = new Serializer();
                        RootNode = Serializer.Deserialize(fs) as INode;
                    }
                    break;
                }

                n--;
            }

            if (n > 0)
                throw new ArgumentOutOfRangeException($"{n} / {FileNameTable.Count}");
            TestWriteable(index, Name, RootNode);
        }
        #endif
        public static void TestWriteable(int index, string name, INode rootNode)
        {
            ControllerTools.ResetExpectedName();

            TestWriteableStats(index, name, rootNode, out Stats Stats);

            Random rand = new Random(0x123456);

            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

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

        public static void TestWriteableStats(int index, string name, INode rootNode, out Stats stats)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);

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

        public static void TestWriteableInsert(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => InsertAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool InsertAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            if (inner is IWriteableListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
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

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    if (RandNext(2) == 0)
                    {
                        int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        IWriteableBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];
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

                        IPattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                        IIdentifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
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
                IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                IWriteableController NewController = WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableRemove(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => RemoveAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool RemoveAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            if (inner is IWriteableListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int Index = RandNext(AsListInner.StateList.Count);
                    IWriteableNodeState ChildState = AsListInner.StateList[Index];
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
                    IWriteableBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];
                    int Index = RandNext(BlockState.StateList.Count);
                    IWriteableNodeState ChildState = BlockState.StateList[Index];
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
                IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                IWriteableController NewController = WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
                
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableRemoveBlockRange(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => RemoveBlockRangeAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool RemoveBlockRangeAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

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
                IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                IWriteableController NewController = WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableReplaceBlockRange(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => ReplaceBlockRangeAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool ReplaceBlockRangeAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int FirstBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int LastBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count + 1);

                    if (FirstBlockIndex > LastBlockIndex)
                        FirstBlockIndex = LastBlockIndex;

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IPattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                    IIdentifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
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
                IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                IWriteableController NewController = WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableInsertBlockRange(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => InsertBlockRangeAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool InsertBlockRangeAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int InsertedIndex = RandNext(AsBlockListInner.BlockStateList.Count);

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IPattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                    IIdentifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
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
                IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                IWriteableController NewController = WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableRemoveNodeRange(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => RemoveNodeRangeAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool RemoveNodeRangeAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

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
                IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                IWriteableController NewController = WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableReplaceNodeRange(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => ReplaceNodeRangeAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool ReplaceNodeRangeAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int FirstNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count);
                    int LastNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count + 1);

                    if (FirstNodeIndex > LastNodeIndex)
                        FirstNodeIndex = LastNodeIndex;

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
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

                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
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
                IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                IWriteableController NewController = WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableInsertNodeRange(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => InsertNodeRangeAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool InsertNodeRangeAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int InsertedNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count);

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
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

                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    IWriteableInsertionListNodeIndex ExistingNodeIndex = new WriteableInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, InsertedNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    Controller.InsertNodeRange(AsListInner, BlockIndex, InsertedNodeIndex, IndexList);
                    IsModified = true;
                }
            }

            if (IsModified)
            {
                IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                IWriteableController NewController = WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableReplace(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => ReplaceAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool ReplaceAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            bool IsModified = false;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            if (inner is IWriteablePlaceholderInner AsPlaceholderInner)
            {
                INode NewNode = NodeHelper.DeepCloneNode(AsPlaceholderInner.ChildState.Node, cloneCommentGuid: false);
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
                INode NewNode = NodeHelper.CreateDefaultFromInterface(NodeInterfaceType);
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
                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
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

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IWriteableBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];
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
                IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                IWriteableController NewController = WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableAssign(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => AssignAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool AssignAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            if (inner is IWriteableOptionalInner AsOptionalInner)
            {
                IWriteableOptionalNodeState ChildState = AsOptionalInner.ChildState;
                Assert.That(ChildState != null);

                IWriteableBrowsingOptionalNodeIndex OptionalIndex = ChildState.ParentIndex;
                Assert.That(Controller.Contains(OptionalIndex));

                IOptionalReference Optional = OptionalIndex.Optional;
                Assert.That(Optional != null);

                if (Optional.HasItem)
                {
                    Controller.Assign(OptionalIndex, out bool IsChanged);
                    Assert.That(Optional.IsAssigned);
                    Assert.That(AsOptionalInner.IsAssigned);
                    Assert.That(Optional.Item == ChildState.Node);

                    IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                    IWriteableController NewController = WriteableController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    if (IsChanged)
                    {
                        Controller.Undo();

                        Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                        IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                        Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                    }
                }
            }

            return false;
        }

        public static void TestWriteableUnassign(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => UnassignAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool UnassignAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

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

                IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                IWriteableController NewController = WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                if (IsChanged)
                {
                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestWriteableChangeReplication(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => ChangeReplicationAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool ChangeReplicationAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IWriteableBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];

                    ReplicationStatus Replication = (ReplicationStatus)RandNext(2);
                    Controller.ChangeReplication(AsBlockListInner, BlockIndex, Replication);

                    IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                    IWriteableController NewController = WriteableController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestWriteableChangeDiscreteValue(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseValues(Controller, RootIndex, (IWriteableIndex nodeIndex, string propertyName) => ChangeDiscreteValueAndCompare(ControllerView, RandNext(WriteableMaxTestCount), nodeIndex, propertyName));
        }

        static bool ChangeDiscreteValueAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableIndex nodeIndex, string propertyName)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            IWriteableNodeState State = (IWriteableNodeState)Controller.IndexToState(nodeIndex);
            State.PropertyToValue(propertyName, out object Value, out int MinValue, out int MaxValue);

            int OldValue = (int)Value;
            int NewValue = OldValue + 1;
            if (NewValue > MaxValue)
                NewValue = MinValue;

            Controller.ChangeDiscreteValue(nodeIndex, propertyName, OldValue);

            IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController NewController = WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            Controller.Undo();

            Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Property: {propertyName}, Old Value: {OldValue}");
            IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
            Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

            return false;
        }

        public static void TestWriteableSplit(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => SplitAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool SplitAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int SplitBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IWriteableBlockState BlockState = AsBlockListInner.BlockStateList[SplitBlockIndex];
                    if (BlockState.StateList.Count > 1)
                    {
                        int SplitIndex = 1 + RandNext(BlockState.StateList.Count - 1);
                        IWriteableBrowsingExistingBlockNodeIndex NodeIndex = (IWriteableBrowsingExistingBlockNodeIndex)AsBlockListInner.IndexAt(SplitBlockIndex, SplitIndex);
                        Controller.SplitBlock(AsBlockListInner, NodeIndex);

                        IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                        Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                        IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                        IWriteableController NewController = WriteableController.Create(NewRootIndex);
                        Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                        Controller.Undo();

                        Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                        IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                        Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                        Controller.Redo();

                        Assert.That(AsBlockListInner.BlockStateList.Count > 0);
                        int OldBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        int NewBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        int Direction = NewBlockIndex - OldBlockIndex;

                        Assert.That(Controller.IsBlockMoveable(AsBlockListInner, OldBlockIndex, Direction));

                        Controller.MoveBlock(AsBlockListInner, OldBlockIndex, Direction);

                        IWriteableControllerView NewViewAfterMove = WriteableControllerView.Create(Controller);
                        Assert.That(NewViewAfterMove.IsEqual(CompareEqual.New(), controllerView));

                        IWriteableRootNodeIndex NewRootIndexAfterMove = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                        IWriteableController NewControllerAfterMove = WriteableController.Create(NewRootIndexAfterMove);
                        Assert.That(NewControllerAfterMove.IsEqual(CompareEqual.New(), Controller));
                    }
                }
            }

            return false;
        }

        public static void TestWriteableMerge(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => MergeAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool MergeAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 1)
                {
                    int MergeBlockIndex = 1 + RandNext(AsBlockListInner.BlockStateList.Count - 1);
                    IWriteableBlockState BlockState = AsBlockListInner.BlockStateList[MergeBlockIndex];

                    IWriteableBrowsingExistingBlockNodeIndex NodeIndex = (IWriteableBrowsingExistingBlockNodeIndex)AsBlockListInner.IndexAt(MergeBlockIndex, 0);
                    Controller.MergeBlocks(AsBlockListInner, NodeIndex);

                    IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                    IWriteableController NewController = WriteableController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestWriteableMove(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => MoveAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool MoveAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            bool IsModified = false;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

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
                    IWriteableBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];

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
                IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                IWriteableController NewController = WriteableController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestWriteableMoveBlock(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => MoveBlockAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool MoveBlockAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 1)
                {
                    int OldIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int NewIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int Direction = NewIndex - OldIndex;

                    Assert.That(Controller.IsBlockMoveable(AsBlockListInner, OldIndex, Direction));

                    Controller.MoveBlock(AsBlockListInner, OldIndex, Direction);

                    IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
                    IWriteableController NewController = WriteableController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestWriteableExpand(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => ExpandAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool ExpandAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            IWriteableNodeIndex NodeIndex;
            IWriteablePlaceholderNodeState State;

            if (inner is IWriteablePlaceholderInner AsPlaceholderInner)
            {
                NodeIndex = AsPlaceholderInner.ChildState.ParentIndex as IWriteableNodeIndex;
                Assert.That(NodeIndex != null);

                State = Controller.IndexToState(NodeIndex) as IWriteablePlaceholderNodeState;
                Assert.That(State != null);

                NodeTreeHelper.GetArgumentBlocks(State.Node, out IDictionary<string, IBlockList<IArgument, Argument>> ArgumentBlocksTable);
                if (ArgumentBlocksTable.Count == 0)
                    return true;
            }
            else
                return true;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            Controller.Expand(NodeIndex, out bool IsChanged);

            IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController NewController = WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                Controller.Redo();
            }

            Controller.Expand(NodeIndex, out IsChanged);

            NewController = WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            Controller.Reduce(NodeIndex, out IsChanged);

            NewController = WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            return false;
        }

        public static void TestWriteableReduce(int index, INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            WriteableTestCount = 0;
            WriteableBrowseNode(Controller, RootIndex, (IWriteableInner inner) => ReduceAndCompare(ControllerView, RandNext(WriteableMaxTestCount), inner));
        }

        static bool ReduceAndCompare(IWriteableControllerView controllerView, int TestIndex, IWriteableInner inner)
        {
            if (WriteableTestCount++ < TestIndex)
                return true;

            IWriteableController Controller = controllerView.Controller;
            IWriteableNodeIndex NodeIndex;
            IWriteablePlaceholderNodeState State;

            if (inner is IWriteablePlaceholderInner AsPlaceholderInner)
            {
                NodeIndex = AsPlaceholderInner.ChildState.ParentIndex as IWriteableNodeIndex;
                Assert.That(NodeIndex != null);

                State = Controller.IndexToState(NodeIndex) as IWriteablePlaceholderNodeState;
                Assert.That(State != null);

                NodeTreeHelper.GetArgumentBlocks(State.Node, out IDictionary<string, IBlockList<IArgument, Argument>> ArgumentBlocksTable);
                if (ArgumentBlocksTable.Count == 0)
                    return true;
            }
            else
                return true;

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            Controller.Reduce(NodeIndex, out bool IsChanged);

            IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController NewController = WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                Controller.Redo();
            }

            Controller.Reduce(NodeIndex, out IsChanged);

            NewController = WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            Controller.Expand(NodeIndex, out IsChanged);

            NewController = WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            return false;
        }

        public static void TestWriteableCanonicalize(INode rootNode)
        {
            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            IWriteableRootNodeIndex OldRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController OldController = WriteableController.Create(OldRootIndex);

            Controller.Canonicalize(out bool IsChanged);

            IWriteableControllerView NewView = WriteableControllerView.Create(Controller);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            IWriteableRootNodeIndex NewRootIndex = new WriteableRootNodeIndex(Controller.RootIndex.Node);
            IWriteableController NewController = WriteableController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
            
            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Root Node: {rootNode}");
                IWriteableControllerView OldView = WriteableControllerView.Create(Controller);
                Assert.That(OldView.IsEqual(CompareEqual.New(), ControllerView));
            }
        }

        static bool WriteableBrowseNode(IWriteableController controller, IWriteableIndex index, Func<IWriteableInner, bool> test)
        {
            Assert.That(index != null, "Writeable #0");
            Assert.That(controller.Contains(index), "Writeable #1");
            IWriteableNodeState State = (IWriteableNodeState)controller.IndexToState(index);
            Assert.That(State != null, "Writeable #2");
            Assert.That(State.ParentIndex == index, "Writeable #4");

            INode Node;

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
                    NodeTreeHelperOptional.GetChildNode(Node, PropertyName, out bool IsAssigned, out INode ChildNode);
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
                        IWriteablePlaceholderNodeState ChildState = Inner.StateList[i];
                        IWriteableIndex ChildIndex = ChildState.ParentIndex;
                        if (!WriteableBrowseNode(controller, ChildIndex, test))
                            return false;
                    }
                }

                else if (NodeTreeHelperBlockList.IsBlockListProperty(Node, PropertyName, out Type ChildInterfaceType, out ChildNodeType))
                {
                    IWriteableBlockListInner Inner = (IWriteableBlockListInner)State.PropertyToInner(PropertyName);
                    if (!test(Inner))
                        return false;

                    for (int BlockIndex = 0; BlockIndex < Inner.BlockStateList.Count; BlockIndex++)
                    {
                        IWriteableBlockState BlockState = Inner.BlockStateList[BlockIndex];
                        if (!WriteableBrowseNode(controller, BlockState.PatternIndex, test))
                            return false;
                        if (!WriteableBrowseNode(controller, BlockState.SourceIndex, test))
                            return false;

                        for (int i = 0; i < BlockState.StateList.Count; i++)
                        {
                            IWriteablePlaceholderNodeState ChildState = BlockState.StateList[i];
                            IWriteableIndex ChildIndex = ChildState.ParentIndex;
                            if (!WriteableBrowseNode(controller, ChildIndex, test))
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        static bool WriteableBrowseValues(IWriteableController controller, IWriteableIndex index, Func<IWriteableIndex, string, bool> test)
        {
            Assert.That(index != null, "Writeable #7");
            Assert.That(controller.Contains(index), "Writeable #8");
            IWriteableNodeState State = (IWriteableNodeState)controller.IndexToState(index);
            Assert.That(State != null, "Writeable #9");
            Assert.That(State.ParentIndex == index, "Writeable #10");

            INode Node;

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
                    NodeTreeHelperOptional.GetChildNode(Node, PropertyName, out bool IsAssigned, out INode ChildNode);
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
                        IWriteablePlaceholderNodeState ChildState = Inner.StateList[i];
                        IWriteableIndex ChildIndex = ChildState.ParentIndex;
                        if (!WriteableBrowseValues(controller, ChildIndex, test))
                            return false;
                    }
                }

                else if (NodeTreeHelperBlockList.IsBlockListProperty(Node, PropertyName, out Type ChildInterfaceType, out ChildNodeType))
                {
                    IWriteableBlockListInner Inner = (IWriteableBlockListInner)State.PropertyToInner(PropertyName);

                    for (int BlockIndex = 0; BlockIndex < Inner.BlockStateList.Count; BlockIndex++)
                    {
                        IWriteableBlockState BlockState = Inner.BlockStateList[BlockIndex];
                        if (!WriteableBrowseValues(controller, BlockState.PatternIndex, test))
                            return false;
                        if (!WriteableBrowseValues(controller, BlockState.SourceIndex, test))
                            return false;

                        for (int i = 0; i < BlockState.StateList.Count; i++)
                        {
                            IWriteablePlaceholderNodeState ChildState = BlockState.StateList[i];
                            IWriteableIndex ChildIndex = ChildState.ParentIndex;
                            if (!WriteableBrowseValues(controller, ChildIndex, test))
                                return false;
                        }
                    }
                }
            }

            return true;
        }
        #endregion

        #region Frame
#if FRAME
        [Test]
        [TestCaseSource(nameof(FileIndexRange))]
        public static void Frame(int index)
        {
            if (TestOff)
                return;

            string Name = null;
            INode RootNode = null;
            int n = index;
            foreach (string FileName in FileNameTable)
            {
                if (n == 0)
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    {
                        Name = FileName;
                        Serializer Serializer = new Serializer();
                        RootNode = Serializer.Deserialize(fs) as INode;
                    }
                    break;
                }

                n--;
            }

            if (n > 0)
                throw new ArgumentOutOfRangeException($"{n} / {FileNameTable.Count}");
            TestFrame(index, Name, RootNode);
        }
#endif

        public static void TestFrame(int index, string name, INode rootNode)
        {
            ControllerTools.ResetExpectedName();

            TestFrameStats(index, name, rootNode, out Stats Stats);

            Random rand = new Random(0x123456);

            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);

            IFrameControllerView ControllerView;

            if (CustomFrameTemplateSet.FrameTemplateSet != null)
            {
                ControllerView = FrameControllerView.Create(Controller, CustomFrameTemplateSet.FrameTemplateSet);

                if (FrameExpectedLastLineTable.ContainsKey(name))
                {
                    int ExpectedLastLineNumber = FrameExpectedLastLineTable[name];
                    Assert.That(ControllerView.LastLineNumber == ExpectedLastLineNumber, $"Last line number for {name}: {ControllerView.LastLineNumber}, expected: {ExpectedLastLineNumber}");
                }
                else
                {
                    using (FileStream fs = new FileStream("lines.txt", FileMode.Append, FileAccess.Write))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine($"{{ \"{name}\", {ControllerView.LastLineNumber} }},");
                    }
                }

                TestFrameCellViewList(ControllerView, name);
            }
            else
                ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, JustCount);
            FrameMaxTestCount = FrameTestCount;

            for (int i = 0; i < TestRepeatCount; i++)
            {
                TestFrameInsert(index, rootNode);
                TestFrameRemove(index, rootNode);
                TestFrameRemoveBlockRange(index, rootNode);
                TestFrameReplaceBlockRange(index, rootNode);
                TestFrameInsertBlockRange(index, rootNode);
                TestFrameRemoveNodeRange(index, rootNode);
                TestFrameReplaceNodeRange(index, rootNode);
                TestFrameInsertNodeRange(index, rootNode);
                TestFrameReplace(index, rootNode);
                TestFrameAssign(index, rootNode);
                TestFrameUnassign(index, rootNode);
                TestFrameChangeReplication(index, rootNode);
                TestFrameSplit(index, rootNode);
                TestFrameMerge(index, rootNode);
                TestFrameMove(index, rootNode);
                TestFrameExpand(index, rootNode);
                TestFrameReduce(index, rootNode);
            }

            TestFrameCanonicalize(rootNode);
        }

        public static void TestFrameCellViewList(IFrameControllerView controllerView, string name)
        {
            IFrameVisibleCellViewList CellViewList = new FrameVisibleCellViewList();
            controllerView.EnumerateVisibleCellViews((IFrameVisibleCellView item) => ListCellViews(item, CellViewList), out IFrameVisibleCellView FoundCellView, false);

            Assert.That(controllerView.LastLineNumber >= 1);
            Assert.That(controllerView.LastColumnNumber >= 1);

            IFrameVisibleCellView[,] CellViewGrid = new IFrameVisibleCellView[controllerView.LastLineNumber, controllerView.LastColumnNumber];

            foreach (IFrameVisibleCellView CellView in CellViewList)
            {
                int LineNumber = CellView.LineNumber - 1;
                int ColumnNumber = CellView.ColumnNumber - 1;

                Assert.That(LineNumber >= 0);
                Assert.That(LineNumber < controllerView.LastLineNumber);
                Assert.That(ColumnNumber >= 0);
                Assert.That(ColumnNumber < controllerView.LastColumnNumber);

                Assert.That(CellViewGrid[LineNumber, ColumnNumber] == null);
                CellViewGrid[LineNumber, ColumnNumber] = CellView;

                IFrameFrame Frame = CellView.Frame;
                INode ChildNode = CellView.StateView.State.Node;
                string PropertyName;

                switch (CellView)
                {
                    case IFrameDiscreteContentFocusableCellView AsDiscreteContentFocusable: // Enum, bool
                        PropertyName = AsDiscreteContentFocusable.PropertyName;
                        Assert.That(NodeTreeHelper.IsEnumProperty(ChildNode, PropertyName) || NodeTreeHelper.IsBooleanProperty(ChildNode, PropertyName));
                        break;
                    case IFrameStringContentFocusableCellView AsTextFocusable: // String
                        PropertyName = AsTextFocusable.PropertyName;
                        Assert.That(NodeTreeHelper.IsStringProperty(ChildNode, PropertyName) && PropertyName == "Text");
                        break;
                    case IFrameFocusableCellView AsFrameable: // Insert
                        Assert.That((Frame is IFrameInsertFrame) || (Frame is IFrameKeywordFrame AsFocusableKeywordFrame && AsFocusableKeywordFrame.IsFocusable));
                        break;
                    case IFrameVisibleCellView AsVisible: // Others
                        Assert.That(((Frame is IFrameKeywordFrame AsKeywordFrame && !AsKeywordFrame.IsFocusable) && !string.IsNullOrEmpty(AsKeywordFrame.Text)) || (Frame is IFrameSymbolFrame AsSymbolFrame));
                        break;
                }
            }

            int IndexFirstLine = -1;
            for (int i = 0; i < controllerView.LastColumnNumber; i++)
                if (CellViewGrid[0, i] != null)
                {
                    IndexFirstLine = i;
                    break;
                }
            Assert.That(IndexFirstLine >= 0);

            int IndexFirstColumn = -1;
            for (int i = 0; i < controllerView.LastLineNumber; i++)
                if (CellViewGrid[0, 1] != null)
                {
                    IndexFirstColumn = i;
                    break;
                }
            Assert.That(IndexFirstColumn >= 0);

            int IndexLastLine = -1;
            for (int i = 0; i < controllerView.LastColumnNumber; i++)
                if (CellViewGrid[controllerView.LastLineNumber - 1, i] != null)
                {
                    IndexLastLine = i;
                    break;
                }
            Assert.That(IndexLastLine >= 0);

            int IndexLastColumn = -1;
            for (int i = 0; i < controllerView.LastLineNumber; i++)
                if (CellViewGrid[i, controllerView.LastColumnNumber - 1] != null)
                {
                    IndexLastColumn = i;
                    break;
                }
            Assert.That(IndexLastColumn >= 0);
        }

        public static Dictionary<string, int> FrameExpectedLastLineTable = new Dictionary<string, int>()
        {
            { "./test.easly", 193 },
            { "./EaslyExamples/CoreEditor/Classes/Agent Expression.easly", 193 },
            { "./EaslyExamples/CoreEditor/Classes/Basic Key Event Handler.easly", 855 },
            { "./EaslyExamples/CoreEditor/Classes/Block Editor Node Management.easly", 62 },
            { "./EaslyExamples/CoreEditor/Classes/Block Editor Node.easly", 162 },
            { "./EaslyExamples/CoreEditor/Classes/Block List Editor Node Management.easly", 62 },
            { "./EaslyExamples/CoreEditor/Classes/Block List Editor Node.easly", 252 },
            { "./EaslyExamples/CoreEditor/Classes/Control Key Event Handler.easly", 150 },
            { "./EaslyExamples/CoreEditor/Classes/Control With Decoration Management.easly", 644 },
            { "./EaslyExamples/CoreEditor/Classes/Decoration.easly", 47 },
            { "./EaslyExamples/CoreEditor/Classes/Editor Node Management.easly", 124 },
            { "./EaslyExamples/CoreEditor/Classes/Editor Node.easly", 17 },
            { "./EaslyExamples/CoreEditor/Classes/Horizontal Separator.easly", 35 },
            { "./EaslyExamples/CoreEditor/Classes/Identifier Key Event Handler.easly", 56 },
            { "./EaslyExamples/CoreEditor/Classes/Insertion Position.easly", 34 },
            { "./EaslyExamples/CoreEditor/Classes/Key Descriptor.easly", 83 },
            { "./EaslyExamples/CoreEditor/Classes/Node Selection.easly", 67 },
            { "./EaslyExamples/CoreEditor/Classes/Node With Default.easly", 27 },
            { "./EaslyExamples/CoreEditor/Classes/Properties Show Options.easly", 120 },
            { "./EaslyExamples/CoreEditor/Classes/Property Changed Notifier.easly", 57 },
            { "./EaslyExamples/CoreEditor/Classes/Replace Notification.easly", 44 },
            { "./EaslyExamples/CoreEditor/Classes/Simplify Notification.easly", 29 },
            { "./EaslyExamples/CoreEditor/Classes/Specialized Decoration.easly", 66 },
            { "./EaslyExamples/CoreEditor/Classes/Toggle Notification.easly", 41 },
            { "./EaslyExamples/CoreEditor/Libraries/Constructs.easly", 5 },
            { "./EaslyExamples/CoreEditor/Libraries/Nodes.easly", 5 },
            { "./EaslyExamples/CoreEditor/Libraries/SSC Editor.easly", 21 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Agent Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Anchor Kinds.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Anchored Type.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Argument.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/As Long As Instruction.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Assertion Tag Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Assertion.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Assignment Argument.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Assignment Instruction.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Assignment Type Argument.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Attachment Instruction.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Attachment.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Attribute Feature.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Binary Operator Expression.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Block List.easly", 30 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Block.easly", 17 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Body.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Check Instruction.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Class Constant Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Class Replicate.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Class.easly", 99 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Clone Of Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Clone Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Cloneable Status.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Command Instruction.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Command Overload Type.easly", 51 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Command Overload.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Comparable Status.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Comparison Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Conditional.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Conformance Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Constant Feature.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Constraint.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Continuation.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Copy Semantic.easly", 34 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Create Instruction.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Creation Feature.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Debug Instruction.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Deferred Body.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Discrete.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Effective Body.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Entity Declaration.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Entity Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Equality Expression.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Equality Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Event Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Exception Handler.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Export Change.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Export Status.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Export.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Expression.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Extern Body.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Feature.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/For Loop Instruction.easly", 55 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Function Feature.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Function Type.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Generic Type.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Generic.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Global Replicate.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Identifier.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/If Then Else Instruction.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Import Type.easly", 34 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Import.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Index Assignment Instruction.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Index Query Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Indexer Feature.easly", 55 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Indexer Type.easly", 75 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Inheritance.easly", 71 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Initialized Object Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Inspect Instruction.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Instruction.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Iteration Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Keyword Anchored Type.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Keyword Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Keyword.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Library.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Manifest Character Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Manifest Number Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Manifest String Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Name.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Named Feature.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/New Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Node.easly", 12 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Object Type.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Old Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Once Choice.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Over Loop Instruction.easly", 51 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Parameter End Status.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Pattern.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Positional Argument.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Positional Type Argument.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Precursor Body.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Precursor Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Precursor Index Assignment Instruction.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Precursor Index Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Precursor Instruction.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Preprocessor Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Preprocessor Macro.easly", 40 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Procedure Feature.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Procedure Type.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Property Feature.easly", 51 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Property Type.easly", 59 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Qualified Name.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Query Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Query Overload Type.easly", 55 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Query Overload.easly", 55 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Raise Event Instruction.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Range.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Release Instruction.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Rename.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Replication Status.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Result Of Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Root.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Scope.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Shareable Type.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Sharing Type.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Simple Type.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Throw Instruction.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Tuple Type.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Type Argument.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Typedef.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Unary Operator Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Utility Type.easly", 34 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/With.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Libraries/Constructs.easly", 5 },
            { "./EaslyExamples/EaslyCoreLanguage/Libraries/Nodes.easly", 6 },
            { "./EaslyExamples/EaslyCoreLanguage/Libraries/SSC Language.easly", 15 },
            { "./EaslyExamples/EaslyCoreLanguage/Replicates/SSC Core Language Nodes.easly", 1 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/.NET Event.easly", 43 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.ComponentModel.PropertyChangedEventArgs.easly", 44 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.ComponentModel.PropertyChangedEventHandler.easly", 48 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Controls.Orientation.easly", 33 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Controls.TextBox.easly", 73 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.DependencyObject.easly", 20 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.FrameworkElement.easly", 60 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Input.FocusNavigationDirection.easly", 39 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Input.Key.easly", 72 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Input.Keyboard.easly", 68 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Input.KeyEventArgs.easly", 40 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Input.TraversalRequest.easly", 40 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.InputElement.easly", 20 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Media.VisualTreeHelper.easly", 68 },
            { "./EaslyExamples/MicrosoftDotNet/Libraries/.NET Classes.easly", 5 },
            { "./EaslyExamples/MicrosoftDotNet/Libraries/.NET Enums.easly", 5 },
            { "./EaslyExamples/Verification/Verification Example.easly", 80 },
        };

        public static void TestFrameCanonicalize(INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            Controller.Canonicalize(out bool IsChanged);

            IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController NewController = FrameController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        static int FrameTestCount = 0;
        static int FrameMaxTestCount = 0;

        public static bool JustCount(IFrameInner inner)
        {
            FrameTestCount++;
            return true;
        }

        public static void TestFrameStats(int index, string name, INode rootNode, out Stats stats)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);

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

        public static void TestFrameInsert(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => InsertAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool InsertAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    int Index = RandNext(AsListInner.StateList.Count + 1);
                    IFrameInsertionListNodeIndex NodeIndex = new FrameInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, Index);
                    Controller.Insert(AsListInner, NodeIndex, out IWriteableBrowsingCollectionNodeIndex InsertedIndex);
                    Assert.That(Controller.Contains(InsertedIndex));

                    IFramePlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as IFramePlaceholderNodeState;
                    Assert.That(ChildState != null);
                    Assert.That(ChildState.Node == NewNode);

                    IsModified = true;
                }
            }
            else if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    if (RandNext(2) == 0)
                    {
                        int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        IFrameBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];
                        int Index = RandNext(BlockState.StateList.Count + 1);

                        IFrameInsertionExistingBlockNodeIndex NodeIndex = new FrameInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, Index);
                        Controller.Insert(AsBlockListInner, NodeIndex, out IWriteableBrowsingCollectionNodeIndex InsertedIndex);
                        Assert.That(Controller.Contains(InsertedIndex));

                        IFramePlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as IFramePlaceholderNodeState;
                        Assert.That(ChildState != null);
                        Assert.That(ChildState.Node == NewNode);

                        IsModified = true;
                    }
                    else
                    {
                        int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count + 1);

                        IPattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                        IIdentifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
                        IFrameInsertionNewBlockNodeIndex NodeIndex = new FrameInsertionNewBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, ReplicationPattern, SourceIdentifier);
                        Controller.Insert(AsBlockListInner, NodeIndex, out IWriteableBrowsingCollectionNodeIndex InsertedIndex);
                        Assert.That(Controller.Contains(InsertedIndex));

                        IFramePlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as IFramePlaceholderNodeState;
                        Assert.That(ChildState != null);
                        Assert.That(ChildState.Node == NewNode);

                        IsModified = true;
                    }
                }
            }

            if (IsModified)
            {
                IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                IFrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameReplace(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => ReplaceAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool ReplaceAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFramePlaceholderInner AsPlaceholderInner)
            {
                INode NewNode = NodeHelper.DeepCloneNode(AsPlaceholderInner.ChildState.Node, cloneCommentGuid: false);
                Assert.That(NewNode != null, $"Type: {AsPlaceholderInner.InterfaceType}");

                IFrameInsertionPlaceholderNodeIndex NodeIndex = new FrameInsertionPlaceholderNodeIndex(AsPlaceholderInner.Owner.Node, AsPlaceholderInner.PropertyName, NewNode);
                Controller.Replace(AsPlaceholderInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                Assert.That(Controller.Contains(InsertedIndex));

                IFramePlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as IFramePlaceholderNodeState;
                Assert.That(ChildState != null);
                Assert.That(ChildState.Node == NewNode);

                IsModified = true;
            }
            else if (inner is IFrameOptionalInner AsOptionalInner)
            {
                IFrameOptionalNodeState State = AsOptionalInner.ChildState;
                IOptionalReference Optional = State.ParentIndex.Optional;
                Type NodeInterfaceType = Optional.GetType().GetGenericArguments()[0];
                INode NewNode = NodeHelper.CreateDefaultFromInterface(NodeInterfaceType);
                Assert.That(NewNode != null, $"Type: {AsOptionalInner.InterfaceType}");

                IFrameInsertionOptionalNodeIndex NodeIndex = new FrameInsertionOptionalNodeIndex(AsOptionalInner.Owner.Node, AsOptionalInner.PropertyName, NewNode);
                Controller.Replace(AsOptionalInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                Assert.That(Controller.Contains(InsertedIndex));

                IFrameOptionalNodeState ChildState = Controller.IndexToState(InsertedIndex) as IFrameOptionalNodeState;
                Assert.That(ChildState != null);
                Assert.That(ChildState.Node == NewNode);

                IsModified = true;
            }
            else if (inner is IFrameListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    int Index = RandNext(AsListInner.StateList.Count);
                    IFrameInsertionListNodeIndex NodeIndex = new FrameInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, Index);
                    Controller.Replace(AsListInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                    Assert.That(Controller.Contains(InsertedIndex));

                    IFramePlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as IFramePlaceholderNodeState;
                    Assert.That(ChildState != null);
                    Assert.That(ChildState.Node == NewNode);

                    IsModified = true;
                }
            }
            else if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IFrameBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];
                    int Index = RandNext(BlockState.StateList.Count);

                    IFrameInsertionExistingBlockNodeIndex NodeIndex = new FrameInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, Index);
                    Controller.Replace(AsBlockListInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                    Assert.That(Controller.Contains(InsertedIndex));

                    IFramePlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as IFramePlaceholderNodeState;
                    Assert.That(ChildState != null);
                    Assert.That(ChildState.Node == NewNode);

                    IsModified = true;
                }
            }

            if (IsModified)
            {
                IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                IFrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameRemove(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => RemoveAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool RemoveAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int Index = RandNext(AsListInner.StateList.Count);
                    IFrameNodeState ChildState = AsListInner.StateList[Index];
                    IFrameBrowsingListNodeIndex NodeIndex = ChildState.ParentIndex as IFrameBrowsingListNodeIndex;
                    Assert.That(NodeIndex != null);

                    if (Controller.IsRemoveable(AsListInner, NodeIndex))
                    {
                        Controller.Remove(AsListInner, NodeIndex);
                        IsModified = true;
                    }
                }
            }
            else if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IFrameBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];
                    int Index = RandNext(BlockState.StateList.Count);
                    IFrameNodeState ChildState = BlockState.StateList[Index];
                    IFrameBrowsingExistingBlockNodeIndex NodeIndex = ChildState.ParentIndex as IFrameBrowsingExistingBlockNodeIndex;
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
                IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                IFrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameRemoveBlockRange(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => RemoveBlockRangeAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool RemoveBlockRangeAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    //System.Diagnostics.Debug.Assert(false);
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
                IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                IFrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameReplaceBlockRange(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => ReplaceBlockRangeAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool ReplaceBlockRangeAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int FirstBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int LastBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count + 1);

                    if (FirstBlockIndex > LastBlockIndex)
                        FirstBlockIndex = LastBlockIndex;

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IPattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                    IIdentifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
                    IFrameInsertionNewBlockNodeIndex NewNodeIndex = new FrameInsertionNewBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, FirstBlockIndex, ReplicationPattern, SourceIdentifier);

                    NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IFrameInsertionExistingBlockNodeIndex ExistingNodeIndex = new FrameInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, FirstBlockIndex, 1);

                    List<IWriteableInsertionBlockNodeIndex> IndexList = new List<IWriteableInsertionBlockNodeIndex>() { NewNodeIndex, ExistingNodeIndex };
                    Controller.ReplaceBlockRange(AsBlockListInner, FirstBlockIndex, LastBlockIndex, IndexList);
                    IsModified = true;
                }
            }

            if (IsModified)
            {
                IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                IFrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameInsertBlockRange(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => InsertBlockRangeAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool InsertBlockRangeAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int InsertedIndex = RandNext(AsBlockListInner.BlockStateList.Count);

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IPattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                    IIdentifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
                    IFrameInsertionNewBlockNodeIndex NewNodeIndex = new FrameInsertionNewBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, InsertedIndex, ReplicationPattern, SourceIdentifier);

                    NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IFrameInsertionExistingBlockNodeIndex ExistingNodeIndex = new FrameInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, InsertedIndex, 1);

                    List<IWriteableInsertionBlockNodeIndex> IndexList = new List<IWriteableInsertionBlockNodeIndex>() { NewNodeIndex, ExistingNodeIndex };
                    Controller.InsertBlockRange(AsBlockListInner, InsertedIndex, IndexList);
                    IsModified = true;
                }
            }

            if (IsModified)
            {
                IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                IFrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameRemoveNodeRange(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => RemoveNodeRangeAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool RemoveNodeRangeAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
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
                        Controller.RemoveNodeRange(AsBlockListInner, BlockIndex, FirstNodeIndex, LastNodeIndex);
                        IsModified = FirstNodeIndex < LastNodeIndex;
                    }
                }
            }

            else if (inner is IFrameListInner AsListInner)
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
                IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                IFrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameReplaceNodeRange(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => ReplaceNodeRangeAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool ReplaceNodeRangeAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int FirstNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count);
                    int LastNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count + 1);

                    if (FirstNodeIndex > LastNodeIndex)
                        FirstNodeIndex = LastNodeIndex;

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IFrameInsertionExistingBlockNodeIndex ExistingNodeIndex = new FrameInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, FirstNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    if (Controller.IsNodeRangeRemoveable(AsBlockListInner, BlockIndex, FirstNodeIndex, LastNodeIndex))
                    {
                        Controller.ReplaceNodeRange(AsBlockListInner, BlockIndex, FirstNodeIndex, LastNodeIndex, IndexList);
                        IsModified = IndexList.Count > 0 || FirstNodeIndex < LastNodeIndex;
                    }
                }
            }

            else if (inner is IFrameListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int BlockIndex = -1;
                    int FirstNodeIndex = RandNext(AsListInner.StateList.Count);
                    int LastNodeIndex = RandNext(AsListInner.StateList.Count + 1);

                    if (FirstNodeIndex > LastNodeIndex)
                        FirstNodeIndex = LastNodeIndex;

                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    IFrameInsertionListNodeIndex ExistingNodeIndex = new FrameInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, FirstNodeIndex);
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
                IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                IFrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameInsertNodeRange(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => InsertNodeRangeAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool InsertNodeRangeAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int InsertedNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count);

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IFrameInsertionExistingBlockNodeIndex ExistingNodeIndex = new FrameInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, InsertedNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    Controller.InsertNodeRange(AsBlockListInner, BlockIndex, InsertedNodeIndex, IndexList);
                    IsModified = true;
                }
            }

            else if (inner is IFrameListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int BlockIndex = -1;
                    int InsertedNodeIndex = RandNext(AsListInner.StateList.Count);

                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    IFrameInsertionListNodeIndex ExistingNodeIndex = new FrameInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, InsertedNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    Controller.InsertNodeRange(AsListInner, BlockIndex, InsertedNodeIndex, IndexList);
                    IsModified = true;
                }
            }

            if (IsModified)
            {
                IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                IFrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameAssign(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => AssignAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool AssignAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameOptionalInner AsOptionalInner)
            {
                IFrameOptionalNodeState ChildState = AsOptionalInner.ChildState;
                Assert.That(ChildState != null);

                IFrameBrowsingOptionalNodeIndex OptionalIndex = ChildState.ParentIndex;
                Assert.That(Controller.Contains(OptionalIndex));

                IOptionalReference Optional = OptionalIndex.Optional;
                Assert.That(Optional != null);

                if (Optional.HasItem)
                {
                    Controller.Assign(OptionalIndex, out bool IsChanged);
                    Assert.That(Optional.IsAssigned);
                    Assert.That(AsOptionalInner.IsAssigned);
                    Assert.That(Optional.Item == ChildState.Node);

                    IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                    IFrameController NewController = FrameController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    if (IsChanged)
                    {
                        Controller.Undo();

                        Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                        IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                        Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                    }
                }
            }

            return false;
        }

        public static void TestFrameUnassign(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => UnassignAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool UnassignAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameOptionalInner AsOptionalInner)
            {
                IFrameOptionalNodeState ChildState = AsOptionalInner.ChildState;
                Assert.That(ChildState != null);

                IFrameBrowsingOptionalNodeIndex OptionalIndex = ChildState.ParentIndex;
                Assert.That(Controller.Contains(OptionalIndex));

                IOptionalReference Optional = OptionalIndex.Optional;
                Assert.That(Optional != null);

                Controller.Unassign(OptionalIndex, out bool IsChanged);
                Assert.That(!Optional.IsAssigned);
                Assert.That(!AsOptionalInner.IsAssigned);

                IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                IFrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                if (IsChanged)
                {
                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestFrameChangeReplication(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => ChangeReplicationAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool ChangeReplicationAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IFrameBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];

                    ReplicationStatus Replication = (ReplicationStatus)RandNext(2);
                    Controller.ChangeReplication(AsBlockListInner, BlockIndex, Replication);

                    IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                    IFrameController NewController = FrameController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestFrameSplit(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => SplitAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool SplitAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int SplitBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IFrameBlockState BlockState = AsBlockListInner.BlockStateList[SplitBlockIndex];
                    if (BlockState.StateList.Count > 1)
                    {
                        int SplitIndex = 1 + RandNext(BlockState.StateList.Count - 1);
                        IFrameBrowsingExistingBlockNodeIndex NodeIndex = (IFrameBrowsingExistingBlockNodeIndex)AsBlockListInner.IndexAt(SplitBlockIndex, SplitIndex);
                        Controller.SplitBlock(AsBlockListInner, NodeIndex);

                        IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                        Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                        IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                        IFrameController NewController = FrameController.Create(NewRootIndex);
                        Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                        Controller.Undo();

                        Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                        IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                        Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                        Controller.Redo();

                        Assert.That(AsBlockListInner.BlockStateList.Count > 0);
                        int OldBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        int NewBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        int Direction = NewBlockIndex - OldBlockIndex;

                        Assert.That(Controller.IsBlockMoveable(AsBlockListInner, OldBlockIndex, Direction));

                        Controller.MoveBlock(AsBlockListInner, OldBlockIndex, Direction);

                        IFrameControllerView NewViewAfterMove = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                        Assert.That(NewViewAfterMove.IsEqual(CompareEqual.New(), controllerView));

                        IFrameRootNodeIndex NewRootIndexAfterMove = new FrameRootNodeIndex(Controller.RootIndex.Node);
                        IFrameController NewControllerAfterMove = FrameController.Create(NewRootIndexAfterMove);
                        Assert.That(NewControllerAfterMove.IsEqual(CompareEqual.New(), Controller));
                    }
                }
            }

            return false;
        }

        public static void TestFrameMerge(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => MergeAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool MergeAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 1)
                {
                    int MergeBlockIndex = 1 + RandNext(AsBlockListInner.BlockStateList.Count - 1);
                    IFrameBlockState BlockState = AsBlockListInner.BlockStateList[MergeBlockIndex];

                    IFrameBrowsingExistingBlockNodeIndex NodeIndex = (IFrameBrowsingExistingBlockNodeIndex)AsBlockListInner.IndexAt(MergeBlockIndex, 0);
                    Controller.MergeBlocks(AsBlockListInner, NodeIndex);

                    IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                    IFrameController NewController = FrameController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestFrameMove(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => MoveAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool MoveAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int OldIndex = RandNext(AsListInner.StateList.Count);
                    int NewIndex = RandNext(AsListInner.StateList.Count);
                    int Direction = NewIndex - OldIndex;

                    IFrameBrowsingListNodeIndex NodeIndex = AsListInner.IndexAt(OldIndex) as IFrameBrowsingListNodeIndex;
                    Assert.That(NodeIndex != null);

                    Assert.That(Controller.IsMoveable(AsListInner, NodeIndex, Direction));

                    Controller.Move(AsListInner, NodeIndex, Direction);
                    Assert.That(Controller.Contains(NodeIndex));

                    IsModified = true;
                }
            }
            else if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IFrameBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];

                    int OldIndex = RandNext(BlockState.StateList.Count);
                    int NewIndex = RandNext(BlockState.StateList.Count);
                    int Direction = NewIndex - OldIndex;

                    IFrameBrowsingExistingBlockNodeIndex NodeIndex = AsBlockListInner.IndexAt(BlockIndex, OldIndex) as IFrameBrowsingExistingBlockNodeIndex;
                    Assert.That(NodeIndex != null);

                    Assert.That(Controller.IsMoveable(AsBlockListInner, NodeIndex, Direction));

                    Controller.Move(AsBlockListInner, NodeIndex, Direction);
                    Assert.That(Controller.Contains(NodeIndex));

                    IsModified = true;
                }
            }

            if (IsModified)
            {
                IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                IFrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameExpand(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => ExpandAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool ExpandAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;
            IFrameNodeIndex NodeIndex;
            IFramePlaceholderNodeState State;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFramePlaceholderInner AsPlaceholderInner)
            {
                NodeIndex = AsPlaceholderInner.ChildState.ParentIndex as IFrameNodeIndex;
                Assert.That(NodeIndex != null);

                State = Controller.IndexToState(NodeIndex) as IFramePlaceholderNodeState;
                Assert.That(State != null);

                NodeTreeHelper.GetArgumentBlocks(State.Node, out IDictionary<string, IBlockList<IArgument, Argument>> ArgumentBlocksTable);
                if (ArgumentBlocksTable.Count == 0)
                    return true;
            }
            else
                return true;

            Controller.Expand(NodeIndex, out bool IsChanged);

            IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController NewController = FrameController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                Controller.Redo();
            }

            Controller.Expand(NodeIndex, out IsChanged);

            NewController = FrameController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            Controller.Reduce(NodeIndex, out IsChanged);

            NewController = FrameController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            return false;
        }

        public static void TestFrameReduce(int index, INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => ReduceAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool ReduceAndCompare(IFrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            IFrameController Controller = controllerView.Controller;
            IFrameNodeIndex NodeIndex;
            IFramePlaceholderNodeState State;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFramePlaceholderInner AsPlaceholderInner)
            {
                NodeIndex = AsPlaceholderInner.ChildState.ParentIndex as IFrameNodeIndex;
                Assert.That(NodeIndex != null);

                State = Controller.IndexToState(NodeIndex) as IFramePlaceholderNodeState;
                Assert.That(State != null);

                NodeTreeHelper.GetArgumentBlocks(State.Node, out IDictionary<string, IBlockList<IArgument, Argument>> ArgumentBlocksTable);
                if (ArgumentBlocksTable.Count == 0)
                    return true;
            }
            else
                return true;

            Controller.Reduce(NodeIndex, out bool IsChanged);

            IFrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController NewController = FrameController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            Controller.Reduce(NodeIndex, out IsChanged);

            NewController = FrameController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            Controller.Expand(NodeIndex, out IsChanged);

            NewController = FrameController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            return false;
        }

        static bool FrameBrowseNode(IFrameController controller, IFrameIndex index, Func<IFrameInner, bool> test)
        {
            Assert.That(index != null, "Frame #0");
            Assert.That(controller.Contains(index), "Frame #1");
            IFrameNodeState State = (IFrameNodeState)controller.IndexToState(index);
            Assert.That(State != null, "Frame #2");
            Assert.That(State.ParentIndex == index, "Frame #4");

            INode Node;

            if (State is IFramePlaceholderNodeState AsPlaceholderState)
                Node = AsPlaceholderState.Node;
            else if (State is IFramePatternState AsPatternState)
                Node = AsPatternState.Node;
            else if (State is IFrameSourceState AsSourceState)
                Node = AsSourceState.Node;
            else
            {
                Assert.That(State is IFrameOptionalNodeState, "Frame #5");
                IFrameOptionalNodeState AsOptionalState = (IFrameOptionalNodeState)State;
                IFrameOptionalInner ParentInner = AsOptionalState.ParentInner;

                Assert.That(ParentInner.IsAssigned, "Frame #6");

                Node = AsOptionalState.Node;
            }

            Type ChildNodeType;
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(Node);

            foreach (string PropertyName in PropertyNames)
            {
                if (NodeTreeHelperChild.IsChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    IFramePlaceholderInner Inner = (IFramePlaceholderInner)State.PropertyToInner(PropertyName);
                    if (!test(Inner))
                        return false;

                    IFrameNodeState ChildState = Inner.ChildState;
                    IFrameIndex ChildIndex = ChildState.ParentIndex;
                    if (!FrameBrowseNode(controller, ChildIndex, test))
                        return false;
                }

                else if (NodeTreeHelperOptional.IsOptionalChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelperOptional.GetChildNode(Node, PropertyName, out bool IsAssigned, out INode ChildNode);
                    if (IsAssigned)
                    {
                        IFrameOptionalInner Inner = (IFrameOptionalInner)State.PropertyToInner(PropertyName);
                        if (!test(Inner))
                            return false;

                        IFrameNodeState ChildState = Inner.ChildState;
                        IFrameIndex ChildIndex = ChildState.ParentIndex;
                        if (!FrameBrowseNode(controller, ChildIndex, test))
                            return false;
                    }
                }

                else if (NodeTreeHelperList.IsNodeListProperty(Node, PropertyName, out ChildNodeType))
                {
                    IFrameListInner Inner = (IFrameListInner)State.PropertyToInner(PropertyName);
                    if (!test(Inner))
                        return false;

                    for (int i = 0; i < Inner.StateList.Count; i++)
                    {
                        IFramePlaceholderNodeState ChildState = Inner.StateList[i];
                        IFrameIndex ChildIndex = ChildState.ParentIndex;
                        if (!FrameBrowseNode(controller, ChildIndex, test))
                            return false;
                    }
                }

                else if (NodeTreeHelperBlockList.IsBlockListProperty(Node, PropertyName, out Type ChildInterfaceType, out ChildNodeType))
                {
                    IFrameBlockListInner Inner = (IFrameBlockListInner)State.PropertyToInner(PropertyName);
                    if (!test(Inner))
                        return false;

                    for (int BlockIndex = 0; BlockIndex < Inner.BlockStateList.Count; BlockIndex++)
                    {
                        IFrameBlockState BlockState = Inner.BlockStateList[BlockIndex];
                        if (!FrameBrowseNode(controller, BlockState.PatternIndex, test))
                            return false;
                        if (!FrameBrowseNode(controller, BlockState.SourceIndex, test))
                            return false;

                        for (int i = 0; i < BlockState.StateList.Count; i++)
                        {
                            IFramePlaceholderNodeState ChildState = BlockState.StateList[i];
                            IFrameIndex ChildIndex = ChildState.ParentIndex;
                            if (!FrameBrowseNode(controller, ChildIndex, test))
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        private static bool ListCellViews(IFrameVisibleCellView cellview, IFrameVisibleCellViewList cellViewList)
        {
            cellViewList.Add(cellview);
            return false;
        }
        #endregion

        #region Focus
#if FOCUS
        [Test]
        [TestCaseSource(nameof(FileIndexRange))]
        public static void Focus(int index)
        {
            if (TestOff)
                return;

            string Name = null;
            INode RootNode = null;
            int n = index;
            foreach (string FileName in FileNameTable)
            {
                if (n == 0)
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    {
                        Name = FileName;
                        Serializer Serializer = new Serializer();
                        RootNode = Serializer.Deserialize(fs) as INode;
                    }
                    break;
                }

                n--;
            }

            if (n > 0)
                throw new ArgumentOutOfRangeException($"{n} / {FileNameTable.Count}");
            TestFocus(index, Name, RootNode);
        }
#endif

        public static void TestFocus(int index, string name, INode rootNode)
        {
            ControllerTools.ResetExpectedName();

            TestFocusStats(index, name, rootNode, out Stats Stats);

            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);

            IFocusControllerView ControllerView;

            if (CustomFocusTemplateSet.FocusTemplateSet != null)
            {
                ControllerView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);

                if (FocusExpectedLastLineTable.ContainsKey(name))
                {
                    int ExpectedLastLineNumber = FocusExpectedLastLineTable[name];
                    //Assert.That(ControllerView.LastLineNumber == ExpectedLastLineNumber, $"Last line number for {name}: {ControllerView.LastLineNumber}, expected: {ExpectedLastLineNumber}");
                }
                else
                {
                    /*
                    using (FileStream fs = new FileStream("lines.txt", FileMode.Append, FileAccess.Write))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine($"{{ \"{name}\", {ControllerView.LastLineNumber} }},");
                    }*/
                }

                TestFocusCellViewList(ControllerView, name);
            }
            else
                ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, JustCount);
            FocusMaxTestCount = FocusTestCount;

            for (int i = 0; i < TestRepeatCount; i++)
            {
                SeedRand(0x12 + index * 256 + i * 65536);
                TestFocusInsert(index, rootNode);
                TestFocusRemove(index, rootNode);
                TestFocusRemoveBlockRange(index, rootNode);
                TestFocusReplaceBlockRange(index, rootNode);
                TestFocusInsertBlockRange(index, rootNode);
                TestFocusRemoveNodeRange(index, rootNode);
                TestFocusReplaceNodeRange(index, rootNode);
                TestFocusInsertNodeRange(index, rootNode);
                TestFocusReplace(index, rootNode);
                TestFocusAssign(index, rootNode);
                TestFocusUnassign(index, rootNode);
                TestFocusChangeReplication(index, rootNode);
                TestFocusSplit(index, rootNode);
                TestFocusMerge(index, rootNode);
                TestFocusMove(index, rootNode);
                TestFocusExpand(index, rootNode);
                TestFocusReduce(index, rootNode);
            }

            TestFocusCanonicalize(rootNode);

            FocusTestNewItemInsertable(rootNode);
            FocusTestItemRemoveable(rootNode);
            FocusTestItemMoveable(rootNode);
            FocusTestBlockMoveable(rootNode);
            FocusTestItemSplittable(rootNode);
            FocusTestItemMergeable(rootNode);
            FocusTestItemCyclable(rootNode);
            FocusTestItemSimplifiable(rootNode);
            FocusTestItemComplexifiable(rootNode);
            FocusTestIdentifierSplittable(rootNode);
            FocusTestReplicationModifiable(rootNode);
        }

        public static void TestFocusCellViewList(IFocusControllerView controllerView, string name)
        {
            IFocusVisibleCellViewList CellViewList = new FocusVisibleCellViewList();
            controllerView.EnumerateVisibleCellViews((IFrameVisibleCellView item) => ListCellViews(item, CellViewList), out IFrameVisibleCellView FoundCellView, false);

            Assert.That(controllerView.LastLineNumber >= 1);
            Assert.That(controllerView.LastColumnNumber >= 1);

            IFocusVisibleCellView[,] CellViewGrid = new IFocusVisibleCellView[controllerView.LastLineNumber, controllerView.LastColumnNumber];

            foreach (IFocusVisibleCellView CellView in CellViewList)
            {
                int LineNumber = CellView.LineNumber - 1;
                int ColumnNumber = CellView.ColumnNumber - 1;

                Assert.That(LineNumber >= 0);
                Assert.That(LineNumber < controllerView.LastLineNumber);
                Assert.That(ColumnNumber >= 0);
                Assert.That(ColumnNumber < controllerView.LastColumnNumber);

                Assert.That(CellViewGrid[LineNumber, ColumnNumber] == null);
                CellViewGrid[LineNumber, ColumnNumber] = CellView;

                IFocusFrame Frame = CellView.Frame;
                INode ChildNode = CellView.StateView.State.Node;
                string PropertyName;

                switch (CellView)
                {
                    case IFocusDiscreteContentFocusableCellView AsDiscreteContentFocusable: // Enum, bool
                        PropertyName = AsDiscreteContentFocusable.PropertyName;
                        Assert.That(NodeTreeHelper.IsEnumProperty(ChildNode, PropertyName) || NodeTreeHelper.IsBooleanProperty(ChildNode, PropertyName));
                        break;
                    case IFocusStringContentFocusableCellView AsTextFocusable: // String
                        PropertyName = AsTextFocusable.PropertyName;
                        Assert.That(NodeTreeHelper.IsStringProperty(ChildNode, PropertyName) && PropertyName == "Text");
                        break;
                    case IFocusFocusableCellView AsFocusable: // Insert
                        Assert.That((Frame is IFocusInsertFrame) || (Frame is IFocusKeywordFrame AsFocusableKeywordFrame && AsFocusableKeywordFrame.IsFocusable));
                        break;
                    case IFocusVisibleCellView AsVisible: // Others
                        Assert.That(((Frame is IFocusKeywordFrame AsKeywordFrame && !AsKeywordFrame.IsFocusable) && !string.IsNullOrEmpty(AsKeywordFrame.Text)) || (Frame is IFocusSymbolFrame AsSymbolFrame));
                        break;
                }
            }

            int IndexFirstLine = -1;
            for (int i = 0; i < controllerView.LastColumnNumber; i++)
                if (CellViewGrid[0, i] != null)
                {
                    IndexFirstLine = i;
                    break;
                }
            Assert.That(IndexFirstLine >= 0);

            int IndexFirstColumn = -1;
            for (int i = 0; i < controllerView.LastLineNumber; i++)
                if (CellViewGrid[0, 1] != null)
                {
                    IndexFirstColumn = i;
                    break;
                }
            Assert.That(IndexFirstColumn >= 0);

            int IndexLastLine = -1;
            for (int i = 0; i < controllerView.LastColumnNumber; i++)
                if (CellViewGrid[controllerView.LastLineNumber - 1, i] != null)
                {
                    IndexLastLine = i;
                    break;
                }
            Assert.That(IndexLastLine >= 0);

            int IndexLastColumn = -1;
            for (int i = 0; i < controllerView.LastLineNumber; i++)
                if (CellViewGrid[i, controllerView.LastColumnNumber - 1] != null)
                {
                    IndexLastColumn = i;
                    break;
                }
            Assert.That(IndexLastColumn >= 0);
        }

        public static Dictionary<string, int> FocusExpectedLastLineTable = new Dictionary<string, int>()
        {
            { "./test.easly", 193 },
            { "./EaslyExamples/CoreEditor/Classes/Agent Expression.easly", 193 },
            { "./EaslyExamples/CoreEditor/Classes/Basic Key Event Handler.easly", 855 },
            { "./EaslyExamples/CoreEditor/Classes/Block Editor Node Management.easly", 62 },
            { "./EaslyExamples/CoreEditor/Classes/Block Editor Node.easly", 162 },
            { "./EaslyExamples/CoreEditor/Classes/Block List Editor Node Management.easly", 62 },
            { "./EaslyExamples/CoreEditor/Classes/Block List Editor Node.easly", 252 },
            { "./EaslyExamples/CoreEditor/Classes/Control Key Event Handler.easly", 150 },
            { "./EaslyExamples/CoreEditor/Classes/Control With Decoration Management.easly", 644 },
            { "./EaslyExamples/CoreEditor/Classes/Decoration.easly", 47 },
            { "./EaslyExamples/CoreEditor/Classes/Editor Node Management.easly", 124 },
            { "./EaslyExamples/CoreEditor/Classes/Editor Node.easly", 17 },
            { "./EaslyExamples/CoreEditor/Classes/Horizontal Separator.easly", 35 },
            { "./EaslyExamples/CoreEditor/Classes/Identifier Key Event Handler.easly", 56 },
            { "./EaslyExamples/CoreEditor/Classes/Insertion Position.easly", 34 },
            { "./EaslyExamples/CoreEditor/Classes/Key Descriptor.easly", 83 },
            { "./EaslyExamples/CoreEditor/Classes/Node Selection.easly", 67 },
            { "./EaslyExamples/CoreEditor/Classes/Node With Default.easly", 27 },
            { "./EaslyExamples/CoreEditor/Classes/Properties Show Options.easly", 120 },
            { "./EaslyExamples/CoreEditor/Classes/Property Changed Notifier.easly", 57 },
            { "./EaslyExamples/CoreEditor/Classes/Replace Notification.easly", 44 },
            { "./EaslyExamples/CoreEditor/Classes/Simplify Notification.easly", 29 },
            { "./EaslyExamples/CoreEditor/Classes/Specialized Decoration.easly", 66 },
            { "./EaslyExamples/CoreEditor/Classes/Toggle Notification.easly", 41 },
            { "./EaslyExamples/CoreEditor/Libraries/Constructs.easly", 5 },
            { "./EaslyExamples/CoreEditor/Libraries/Nodes.easly", 5 },
            { "./EaslyExamples/CoreEditor/Libraries/SSC Editor.easly", 21 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Agent Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Anchor Kinds.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Anchored Type.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Argument.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/As Long As Instruction.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Assertion Tag Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Assertion.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Assignment Argument.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Assignment Instruction.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Assignment Type Argument.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Attachment Instruction.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Attachment.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Attribute Feature.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Binary Operator Expression.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Block List.easly", 30 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Block.easly", 17 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Body.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Check Instruction.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Class Constant Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Class Replicate.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Class.easly", 99 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Clone Of Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Clone Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Cloneable Status.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Command Instruction.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Command Overload Type.easly", 51 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Command Overload.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Comparable Status.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Comparison Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Conditional.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Conformance Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Constant Feature.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Constraint.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Continuation.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Copy Semantic.easly", 34 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Create Instruction.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Creation Feature.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Debug Instruction.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Deferred Body.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Discrete.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Effective Body.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Entity Declaration.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Entity Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Equality Expression.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Equality Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Event Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Exception Handler.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Export Change.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Export Status.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Export.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Expression.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Extern Body.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Feature.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/For Loop Instruction.easly", 55 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Function Feature.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Function Type.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Generic Type.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Generic.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Global Replicate.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Identifier.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/If Then Else Instruction.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Import Type.easly", 34 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Import.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Index Assignment Instruction.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Index Query Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Indexer Feature.easly", 55 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Indexer Type.easly", 75 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Inheritance.easly", 71 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Initialized Object Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Inspect Instruction.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Instruction.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Iteration Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Keyword Anchored Type.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Keyword Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Keyword.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Library.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Manifest Character Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Manifest Number Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Manifest String Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Name.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Named Feature.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/New Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Node.easly", 12 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Object Type.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Old Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Once Choice.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Over Loop Instruction.easly", 51 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Parameter End Status.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Pattern.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Positional Argument.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Positional Type Argument.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Precursor Body.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Precursor Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Precursor Index Assignment Instruction.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Precursor Index Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Precursor Instruction.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Preprocessor Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Preprocessor Macro.easly", 40 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Procedure Feature.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Procedure Type.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Property Feature.easly", 51 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Property Type.easly", 59 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Qualified Name.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Query Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Query Overload Type.easly", 55 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Query Overload.easly", 55 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Raise Event Instruction.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Range.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Release Instruction.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Rename.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Replication Status.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Result Of Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Root.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Scope.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Shareable Type.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Sharing Type.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Simple Type.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Throw Instruction.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Tuple Type.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Type Argument.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Typedef.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Unary Operator Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Utility Type.easly", 34 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/With.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Libraries/Constructs.easly", 5 },
            { "./EaslyExamples/EaslyCoreLanguage/Libraries/Nodes.easly", 6 },
            { "./EaslyExamples/EaslyCoreLanguage/Libraries/SSC Language.easly", 15 },
            { "./EaslyExamples/EaslyCoreLanguage/Replicates/SSC Core Language Nodes.easly", 1 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/.NET Event.easly", 43 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.ComponentModel.PropertyChangedEventArgs.easly", 44 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.ComponentModel.PropertyChangedEventHandler.easly", 48 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Controls.Orientation.easly", 33 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Controls.TextBox.easly", 73 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.DependencyObject.easly", 20 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.FocusworkElement.easly", 60 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Input.FocusNavigationDirection.easly", 39 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Input.Key.easly", 72 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Input.Keyboard.easly", 68 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Input.KeyEventArgs.easly", 40 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Input.TraversalRequest.easly", 40 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.InputElement.easly", 20 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Media.VisualTreeHelper.easly", 68 },
            { "./EaslyExamples/MicrosoftDotNet/Libraries/.NET Classes.easly", 5 },
            { "./EaslyExamples/MicrosoftDotNet/Libraries/.NET Enums.easly", 5 },
            { "./EaslyExamples/Verification/Verification Example.easly", 80 },
        };

        static int FocusTestCount = 0;
        static int FocusMaxTestCount = 0;

        public static bool JustCount(IFocusInner inner)
        {
            FocusTestCount++;
            return true;
        }

        public static void TestFocusStats(int index, string name, INode rootNode, out Stats stats)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);

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

        public static void TestFocusInsert(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => InsertAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool InsertAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            MoveFocusRandomly(controllerView);

            IFocusController Controller = controllerView.Controller;
            bool IsModified = false;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    int Index = RandNext(AsListInner.StateList.Count + 1);
                    IFocusInsertionListNodeIndex NodeIndex = new FocusInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, Index);
                    Controller.Insert(AsListInner, NodeIndex, out IWriteableBrowsingCollectionNodeIndex InsertedIndex);
                    Assert.That(Controller.Contains(InsertedIndex));

                    IFocusPlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as IFocusPlaceholderNodeState;
                    Assert.That(ChildState != null);
                    Assert.That(ChildState.Node == NewNode);

                    IsModified = true;
                }
            }
            else if (inner is IFocusBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    if (RandNext(2) == 0)
                    {
                        int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        IFocusBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];
                        int Index = RandNext(BlockState.StateList.Count + 1);

                        IFocusInsertionExistingBlockNodeIndex NodeIndex = new FocusInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, Index);
                        Controller.Insert(AsBlockListInner, NodeIndex, out IWriteableBrowsingCollectionNodeIndex InsertedIndex);
                        Assert.That(Controller.Contains(InsertedIndex));

                        IFocusPlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as IFocusPlaceholderNodeState;
                        Assert.That(ChildState != null);
                        Assert.That(ChildState.Node == NewNode);

                        IsModified = true;
                    }
                    else
                    {
                        int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count + 1);

                        IPattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                        IIdentifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
                        IFocusInsertionNewBlockNodeIndex NodeIndex = new FocusInsertionNewBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, ReplicationPattern, SourceIdentifier);
                        Controller.Insert(AsBlockListInner, NodeIndex, out IWriteableBrowsingCollectionNodeIndex InsertedIndex);
                        Assert.That(Controller.Contains(InsertedIndex));

                        IFocusPlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as IFocusPlaceholderNodeState;
                        Assert.That(ChildState != null);
                        Assert.That(ChildState.Node == NewNode);

                        IsModified = true;
                    }
                }
            }

            if (IsModified)
            {
                IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
                IFocusController NewController = FocusController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFocusReplace(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => ReplaceAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool ReplaceAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            MoveFocusRandomly(controllerView);

            IFocusController Controller = controllerView.Controller;
            bool IsModified = false;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusPlaceholderInner AsPlaceholderInner)
            {
                INode NewNode = NodeHelper.DeepCloneNode(AsPlaceholderInner.ChildState.Node, cloneCommentGuid: false);
                Assert.That(NewNode != null, $"Type: {AsPlaceholderInner.InterfaceType}");

                IFocusInsertionPlaceholderNodeIndex NodeIndex = new FocusInsertionPlaceholderNodeIndex(AsPlaceholderInner.Owner.Node, AsPlaceholderInner.PropertyName, NewNode);
                Controller.Replace(AsPlaceholderInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                Assert.That(Controller.Contains(InsertedIndex));

                IFocusPlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as IFocusPlaceholderNodeState;
                Assert.That(ChildState != null);
                Assert.That(ChildState.Node == NewNode);

                IsModified = true;
            }
            else if (inner is IFocusOptionalInner AsOptionalInner)
            {
                IFocusOptionalNodeState State = AsOptionalInner.ChildState;
                IOptionalReference Optional = State.ParentIndex.Optional;
                Type NodeInterfaceType = Optional.GetType().GetGenericArguments()[0];
                INode NewNode = NodeHelper.CreateDefaultFromInterface(NodeInterfaceType);
                Assert.That(NewNode != null, $"Type: {AsOptionalInner.InterfaceType}");

                IFocusInsertionOptionalNodeIndex NodeIndex = new FocusInsertionOptionalNodeIndex(AsOptionalInner.Owner.Node, AsOptionalInner.PropertyName, NewNode);
                Controller.Replace(AsOptionalInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                Assert.That(Controller.Contains(InsertedIndex));

                IFocusOptionalNodeState ChildState = Controller.IndexToState(InsertedIndex) as IFocusOptionalNodeState;
                Assert.That(ChildState != null);
                Assert.That(ChildState.Node == NewNode);

                IsModified = true;
            }
            else if (inner is IFocusListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    int Index = RandNext(AsListInner.StateList.Count);
                    IFocusInsertionListNodeIndex NodeIndex = new FocusInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, Index);
                    Controller.Replace(AsListInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                    Assert.That(Controller.Contains(InsertedIndex));

                    IFocusPlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as IFocusPlaceholderNodeState;
                    Assert.That(ChildState != null);
                    Assert.That(ChildState.Node == NewNode);

                    IsModified = true;
                }
            }
            else if (inner is IFocusBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IFocusBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];
                    int Index = RandNext(BlockState.StateList.Count);

                    IFocusInsertionExistingBlockNodeIndex NodeIndex = new FocusInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, Index);
                    Controller.Replace(AsBlockListInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                    Assert.That(Controller.Contains(InsertedIndex));

                    IFocusPlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as IFocusPlaceholderNodeState;
                    Assert.That(ChildState != null);
                    Assert.That(ChildState.Node == NewNode);

                    IsModified = true;
                }
            }

            if (IsModified)
            {
                IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
                IFocusController NewController = FocusController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFocusRemove(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => RemoveAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool RemoveAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            MoveFocusRandomly(controllerView);

            IFocusController Controller = controllerView.Controller;
            bool IsModified = false;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int Index = RandNext(AsListInner.StateList.Count);
                    IFocusNodeState ChildState = AsListInner.StateList[Index];
                    IFocusBrowsingListNodeIndex NodeIndex = ChildState.ParentIndex as IFocusBrowsingListNodeIndex;
                    Assert.That(NodeIndex != null);

                    if (Controller.IsRemoveable(AsListInner, NodeIndex))
                    {
                        Controller.Remove(AsListInner, NodeIndex);
                        IsModified = true;
                    }
                }
            }
            else if (inner is IFocusBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IFocusBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];
                    int Index = RandNext(BlockState.StateList.Count);
                    IFocusNodeState ChildState = BlockState.StateList[Index];
                    IFocusBrowsingExistingBlockNodeIndex NodeIndex = ChildState.ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
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
                IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
                IFocusController NewController = FocusController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFocusRemoveBlockRange(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => RemoveBlockRangeAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool RemoveBlockRangeAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            IFocusController Controller = controllerView.Controller;
            bool IsModified = false;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusBlockListInner AsBlockListInner)
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
                IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
                IFocusController NewController = FocusController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFocusReplaceBlockRange(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => ReplaceBlockRangeAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool ReplaceBlockRangeAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            IFocusController Controller = controllerView.Controller;
            bool IsModified = false;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int FirstBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int LastBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count + 1);

                    if (FirstBlockIndex > LastBlockIndex)
                        FirstBlockIndex = LastBlockIndex;

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IPattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                    IIdentifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
                    IFocusInsertionNewBlockNodeIndex NewNodeIndex = new FocusInsertionNewBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, FirstBlockIndex, ReplicationPattern, SourceIdentifier);

                    NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IFocusInsertionExistingBlockNodeIndex ExistingNodeIndex = new FocusInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, FirstBlockIndex, 1);

                    List<IWriteableInsertionBlockNodeIndex> IndexList = new List<IWriteableInsertionBlockNodeIndex>() { NewNodeIndex, ExistingNodeIndex };
                    Controller.ReplaceBlockRange(AsBlockListInner, FirstBlockIndex, LastBlockIndex, IndexList);
                    IsModified = true;
                }
            }

            if (IsModified)
            {
                IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
                IFocusController NewController = FocusController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFocusInsertBlockRange(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => InsertBlockRangeAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool InsertBlockRangeAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            IFocusController Controller = controllerView.Controller;
            bool IsModified = false;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int InsertedIndex = RandNext(AsBlockListInner.BlockStateList.Count);

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IPattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                    IIdentifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
                    IFocusInsertionNewBlockNodeIndex NewNodeIndex = new FocusInsertionNewBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, InsertedIndex, ReplicationPattern, SourceIdentifier);

                    NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IFocusInsertionExistingBlockNodeIndex ExistingNodeIndex = new FocusInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, InsertedIndex, 1);

                    List<IWriteableInsertionBlockNodeIndex> IndexList = new List<IWriteableInsertionBlockNodeIndex>() { NewNodeIndex, ExistingNodeIndex };
                    Controller.InsertBlockRange(AsBlockListInner, InsertedIndex, IndexList);
                    IsModified = true;
                }
            }

            if (IsModified)
            {
                IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
                IFocusController NewController = FocusController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFocusRemoveNodeRange(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => RemoveNodeRangeAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool RemoveNodeRangeAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            IFocusController Controller = controllerView.Controller;
            bool IsModified = false;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusBlockListInner AsBlockListInner)
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
                        Controller.RemoveNodeRange(AsBlockListInner, BlockIndex, FirstNodeIndex, LastNodeIndex);
                        IsModified = FirstNodeIndex < LastNodeIndex;
                    }
                }
            }

            else if (inner is IFocusListInner AsListInner)
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
                IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
                IFocusController NewController = FocusController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFocusReplaceNodeRange(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => ReplaceNodeRangeAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool ReplaceNodeRangeAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            IFocusController Controller = controllerView.Controller;
            bool IsModified = false;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int FirstNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count);
                    int LastNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count + 1);

                    if (FirstNodeIndex > LastNodeIndex)
                        FirstNodeIndex = LastNodeIndex;

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IFocusInsertionExistingBlockNodeIndex ExistingNodeIndex = new FocusInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, FirstNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    if (Controller.IsNodeRangeRemoveable(AsBlockListInner, BlockIndex, FirstNodeIndex, LastNodeIndex))
                    {
                        Controller.ReplaceNodeRange(AsBlockListInner, BlockIndex, FirstNodeIndex, LastNodeIndex, IndexList);
                        IsModified = IndexList.Count > 0 || FirstNodeIndex < LastNodeIndex;
                    }
                }
            }

            else if (inner is IFocusListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int BlockIndex = -1;
                    int FirstNodeIndex = RandNext(AsListInner.StateList.Count);
                    int LastNodeIndex = RandNext(AsListInner.StateList.Count + 1);

                    if (FirstNodeIndex > LastNodeIndex)
                        FirstNodeIndex = LastNodeIndex;

                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    IFocusInsertionListNodeIndex ExistingNodeIndex = new FocusInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, FirstNodeIndex);
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
                IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
                IFocusController NewController = FocusController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFocusInsertNodeRange(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => InsertNodeRangeAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool InsertNodeRangeAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            IFocusController Controller = controllerView.Controller;
            bool IsModified = false;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int InsertedNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count);

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IFocusInsertionExistingBlockNodeIndex ExistingNodeIndex = new FocusInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, InsertedNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    Controller.InsertNodeRange(AsBlockListInner, BlockIndex, InsertedNodeIndex, IndexList);
                    IsModified = true;
                }
            }

            else if (inner is IFocusListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int BlockIndex = -1;
                    int InsertedNodeIndex = RandNext(AsListInner.StateList.Count);

                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    IFocusInsertionListNodeIndex ExistingNodeIndex = new FocusInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, InsertedNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    Controller.InsertNodeRange(AsListInner, BlockIndex, InsertedNodeIndex, IndexList);
                    IsModified = true;
                }
            }

            if (IsModified)
            {
                IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
                IFocusController NewController = FocusController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFocusAssign(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => AssignAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool AssignAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            MoveFocusRandomly(controllerView);

            IFocusController Controller = controllerView.Controller;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusOptionalInner AsOptionalInner)
            {
                IFocusOptionalNodeState ChildState = AsOptionalInner.ChildState;
                Assert.That(ChildState != null);

                IFocusBrowsingOptionalNodeIndex OptionalIndex = ChildState.ParentIndex;
                Assert.That(Controller.Contains(OptionalIndex));

                IOptionalReference Optional = OptionalIndex.Optional;
                Assert.That(Optional != null);

                if (Optional.HasItem)
                {
                    Controller.Assign(OptionalIndex, out bool IsChanged);
                    Assert.That(Optional.IsAssigned);
                    Assert.That(AsOptionalInner.IsAssigned);
                    Assert.That(Optional.Item == ChildState.Node);

                    IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
                    IFocusController NewController = FocusController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    if (IsChanged)
                    {
                        Controller.Undo();

                        Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                        IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                        Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                    }
                }
            }

            return false;
        }

        public static void TestFocusUnassign(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => UnassignAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool UnassignAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            MoveFocusRandomly(controllerView);

            IFocusController Controller = controllerView.Controller;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusOptionalInner AsOptionalInner)
            {
                IFocusOptionalNodeState ChildState = AsOptionalInner.ChildState;
                Assert.That(ChildState != null);

                IFocusBrowsingOptionalNodeIndex OptionalIndex = ChildState.ParentIndex;
                Assert.That(Controller.Contains(OptionalIndex));

                IOptionalReference Optional = OptionalIndex.Optional;
                Assert.That(Optional != null);

                Controller.Unassign(OptionalIndex, out bool IsChanged);
                Assert.That(!Optional.IsAssigned);
                Assert.That(!AsOptionalInner.IsAssigned);

                IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
                IFocusController NewController = FocusController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                if (IsChanged)
                {
                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestFocusChangeReplication(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => ChangeReplicationAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool ChangeReplicationAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            MoveFocusRandomly(controllerView);

            IFocusController Controller = controllerView.Controller;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IFocusBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];

                    ReplicationStatus Replication = (ReplicationStatus)RandNext(2);
                    Controller.ChangeReplication(AsBlockListInner, BlockIndex, Replication);

                    IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
                    IFocusController NewController = FocusController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestFocusSplit(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => SplitAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool SplitAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            MoveFocusRandomly(controllerView);

            IFocusController Controller = controllerView.Controller;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int SplitBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IFocusBlockState BlockState = AsBlockListInner.BlockStateList[SplitBlockIndex];
                    if (BlockState.StateList.Count > 1)
                    {
                        int SplitIndex = 1 + RandNext(BlockState.StateList.Count - 1);
                        IFocusBrowsingExistingBlockNodeIndex NodeIndex = (IFocusBrowsingExistingBlockNodeIndex)AsBlockListInner.IndexAt(SplitBlockIndex, SplitIndex);
                        Controller.SplitBlock(AsBlockListInner, NodeIndex);

                        IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                        Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                        IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
                        IFocusController NewController = FocusController.Create(NewRootIndex);
                        Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                        Controller.Undo();

                        Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                        IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                        Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                        Assert.That(AsBlockListInner.BlockStateList.Count > 0);
                        int OldBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        int NewBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        int Direction = NewBlockIndex - OldBlockIndex;

                        Assert.That(Controller.IsBlockMoveable(AsBlockListInner, OldBlockIndex, Direction));

                        Controller.MoveBlock(AsBlockListInner, OldBlockIndex, Direction);

                        IFocusControllerView NewViewAfterMove = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                        Assert.That(NewViewAfterMove.IsEqual(CompareEqual.New(), controllerView));

                        IFocusRootNodeIndex NewRootIndexAfterMove = new FocusRootNodeIndex(Controller.RootIndex.Node);
                        IFocusController NewControllerAfterMove = FocusController.Create(NewRootIndexAfterMove);
                        Assert.That(NewControllerAfterMove.IsEqual(CompareEqual.New(), Controller));
                    }
                }
            }

            return false;
        }

        public static void TestFocusMerge(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => MergeAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool MergeAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            MoveFocusRandomly(controllerView);

            IFocusController Controller = controllerView.Controller;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 1)
                {
                    int MergeBlockIndex = 1 + RandNext(AsBlockListInner.BlockStateList.Count - 1);
                    IFocusBlockState BlockState = AsBlockListInner.BlockStateList[MergeBlockIndex];

                    IFocusBrowsingExistingBlockNodeIndex NodeIndex = (IFocusBrowsingExistingBlockNodeIndex)AsBlockListInner.IndexAt(MergeBlockIndex, 0);
                    Controller.MergeBlocks(AsBlockListInner, NodeIndex);

                    IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
                    IFocusController NewController = FocusController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestFocusMove(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => MoveAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool MoveAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            MoveFocusRandomly(controllerView);

            IFocusController Controller = controllerView.Controller;
            bool IsModified = false;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int OldIndex = RandNext(AsListInner.StateList.Count);
                    int NewIndex = RandNext(AsListInner.StateList.Count);
                    int Direction = NewIndex - OldIndex;

                    IFocusBrowsingListNodeIndex NodeIndex = AsListInner.IndexAt(OldIndex) as IFocusBrowsingListNodeIndex;
                    Assert.That(NodeIndex != null);

                    Assert.That(Controller.IsMoveable(AsListInner, NodeIndex, Direction));

                    Controller.Move(AsListInner, NodeIndex, Direction);
                    Assert.That(Controller.Contains(NodeIndex));

                    IsModified = true;
                }
            }
            else if (inner is IFocusBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IFocusBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];

                    int OldIndex = RandNext(BlockState.StateList.Count);
                    int NewIndex = RandNext(BlockState.StateList.Count);
                    int Direction = NewIndex - OldIndex;

                    IFocusBrowsingExistingBlockNodeIndex NodeIndex = AsBlockListInner.IndexAt(BlockIndex, OldIndex) as IFocusBrowsingExistingBlockNodeIndex;
                    Assert.That(NodeIndex != null);

                    Assert.That(Controller.IsMoveable(AsBlockListInner, NodeIndex, Direction));

                    Controller.Move(AsBlockListInner, NodeIndex, Direction);
                    Assert.That(Controller.Contains(NodeIndex));

                    IsModified = true;
                }
            }

            if (IsModified)
            {
                IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
                IFocusController NewController = FocusController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFocusExpand(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => ExpandAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool ExpandAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            MoveFocusRandomly(controllerView);

            IFocusController Controller = controllerView.Controller;
            IFocusNodeIndex NodeIndex;
            IFocusPlaceholderNodeState State;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusPlaceholderInner AsPlaceholderInner)
            {
                NodeIndex = AsPlaceholderInner.ChildState.ParentIndex as IFocusNodeIndex;
                Assert.That(NodeIndex != null);

                State = Controller.IndexToState(NodeIndex) as IFocusPlaceholderNodeState;
                Assert.That(State != null);

                NodeTreeHelper.GetArgumentBlocks(State.Node, out IDictionary<string, IBlockList<IArgument, Argument>> ArgumentBlocksTable);
                if (ArgumentBlocksTable.Count == 0)
                    return true;
            }
            else
                return true;

            Controller.Expand(NodeIndex, out bool IsChanged);

            IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                Controller.Redo();
            }

            Controller.Expand(NodeIndex, out IsChanged);

            NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            Controller.Reduce(NodeIndex, out IsChanged);

            NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            return false;
        }

        public static void TestFocusReduce(int index, INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            FocusTestCount = 0;
            FocusBrowseNode(Controller, RootIndex, (IFocusInner inner) => ReduceAndCompare(ControllerView, RandNext(FocusMaxTestCount), inner));
        }

        static bool ReduceAndCompare(IFocusControllerView controllerView, int TestIndex, IFocusInner inner)
        {
            if (FocusTestCount++ < TestIndex)
                return true;

            MoveFocusRandomly(controllerView);

            IFocusController Controller = controllerView.Controller;
            IFocusNodeIndex NodeIndex;
            IFocusPlaceholderNodeState State;

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            if (inner is IFocusPlaceholderInner AsPlaceholderInner)
            {
                NodeIndex = AsPlaceholderInner.ChildState.ParentIndex as IFocusNodeIndex;
                Assert.That(NodeIndex != null);

                State = Controller.IndexToState(NodeIndex) as IFocusPlaceholderNodeState;
                Assert.That(State != null);

                NodeTreeHelper.GetArgumentBlocks(State.Node, out IDictionary<string, IBlockList<IArgument, Argument>> ArgumentBlocksTable);
                if (ArgumentBlocksTable.Count == 0)
                    return true;
            }
            else
                return true;

            Controller.Reduce(NodeIndex, out bool IsChanged);

            IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                Controller.Redo();
            }

            Controller.Reduce(NodeIndex, out IsChanged);

            NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            Controller.Expand(NodeIndex, out IsChanged);

            NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            return false;
        }

        public static void TestFocusCanonicalize(INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            IFocusRootNodeIndex OldRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController OldController = FocusController.Create(OldRootIndex);

            Controller.Canonicalize(out bool IsChanged);

            IFocusControllerView NewView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"RootNode: {rootNode}");
                IFocusControllerView OldView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), ControllerView));
            }
        }

        public static void FocusTestNewItemInsertable(INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);

            int Min = ControllerView.MinFocusMove;
            int Max = ControllerView.MaxFocusMove;
            bool IsMoved;

            for (int i = 0; i < (Max - Min) + 10; i++)
            {
                ControllerView.MoveFocus(+1, true, out IsMoved);

                if (ControllerView.IsNewItemInsertable(out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index))
                    Controller.Insert(inner, index, out IWriteableBrowsingCollectionNodeIndex nodeIndex);
            }

            for (int i = 0; i < 10; i++)
            {
                Min = ControllerView.MinFocusMove;
                Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out IsMoved);

                if (ControllerView.IsNewItemInsertable(out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index))
                    Controller.Insert(inner, index, out IWriteableBrowsingCollectionNodeIndex nodeIndex);
            }

            IFocusControllerView NewView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void FocusTestItemRemoveable(INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemRemoveable(out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index))
                    Controller.Remove(inner, index);
            }

            IFocusControllerView NewView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void FocusTestItemMoveable(INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                Direction = (RandNext(2) * 2) - 1;

                if (ControllerView.IsItemMoveable(Direction, out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index))
                    Controller.Move(inner, index, Direction);
            }

            IFocusControllerView NewView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void FocusTestBlockMoveable(INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                Direction = (RandNext(2) * 2) - 1;

                if (ControllerView.IsBlockMoveable(Direction, out IFocusBlockListInner Inner, out int BlockIndex))
                    Controller.MoveBlock(Inner, BlockIndex, Direction);
            }

            IFocusControllerView NewView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void FocusTestItemSplittable(INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemSplittable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index))
                    Controller.SplitBlock(inner, index);
            }

            IFocusControllerView NewView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void FocusTestItemMergeable(INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemMergeable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index))
                    Controller.MergeBlocks(inner, index);
            }

            IFocusControllerView NewView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void FocusTestItemCyclable(INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemCyclableThrough(out IFocusCyclableNodeState state, out int cyclePosition))
                    Controller.Replace(state.ParentInner, state.CycleIndexList, cyclePosition, out IFocusBrowsingChildIndex nodeIndex);
            }

            IFocusControllerView NewView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void FocusTestItemSimplifiable(INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);

            List<IFocusInner> InnerList = new List<IFocusInner>();
            List<IFocusInsertionChildIndex> IndexList = new List<IFocusInsertionChildIndex>();
            List<int> nList = new List<int>();

            for (int i = 0; i < /*200*/89; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemSimplifiable(out IFocusInner Inner, out IFocusInsertionChildIndex Index))
                {
                    InnerList.Add(Inner);
                    IndexList.Add(Index);
                    nList.Add(i);

                    //System.Diagnostics.Debug.Assert(i < 88);
                    Controller.Replace(Inner, Index, out IWriteableBrowsingChildIndex NodeIndex);
                    //break;
                }
            }
            
            foreach (IFocusInner<IFocusBrowsingChildIndex> Inner in InnerList)
            {
                string s = $"{Inner.Owner.Node}: {Inner.PropertyName}";
                System.Diagnostics.Debug.WriteLine(s);
            }
            /*
            IFocusControllerView NewView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));*/
        }

        public static void FocusTestItemComplexifiable(INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);

            for (int i = 0; i < 200; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemComplexifiable(out IDictionary<IFocusInner, IList<IFocusInsertionChildNodeIndex>> IndexTable))
                {
                    int Total = 0;
                    foreach (KeyValuePair<IFocusInner, IList<IFocusInsertionChildNodeIndex>> Entry in IndexTable)
                        Total += Entry.Value.Count;

                    int Choice = RandNext(Total);
                    foreach (KeyValuePair<IFocusInner, IList<IFocusInsertionChildNodeIndex>> Entry in IndexTable)
                        foreach (IFocusInsertionChildNodeIndex Index in Entry.Value)
                            if (Choice == 0)
                            {
                                Controller.Replace(Entry.Key, Index, out IWriteableBrowsingChildIndex NodeIndex);
                                break;
                            }
                            else
                                Choice--;
                }
            }
        }

        public static void FocusTestIdentifierSplittable(INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);

            for (int i = 0; i < 200; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsIdentifierSplittable(out IFocusListInner Inner, out IFocusInsertionListNodeIndex ReplaceIndex, out IFocusInsertionListNodeIndex InsertIndex))
                {
                    Controller.Replace(Inner, ReplaceIndex, out IWriteableBrowsingChildIndex FirstNodeIndex);
                    Controller.Insert(Inner, InsertIndex, out IWriteableBrowsingCollectionNodeIndex SecondNodeIndex);
                }
            }

            IFocusControllerView NewView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void FocusTestReplicationModifiable(INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);

            ReplicationStatus Replication;

            for (int i = 0; i < 200; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsReplicationModifiable(out IFocusBlockListInner Inner, out int BlockIndex, out Replication))
                {
                    switch (Replication)
                    {
                        case ReplicationStatus.Normal:
                            Replication = ReplicationStatus.Replicated;
                            break;
                        case ReplicationStatus.Replicated:
                            Replication = ReplicationStatus.Normal;
                            break;
                    }

                    Controller.ChangeReplication(Inner, BlockIndex, Replication);
                }
            }

            IFocusControllerView NewView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);

            CompareEqual comparer = CompareEqual.New();
            bool IsEq = NewView.IsEqual(comparer, ControllerView);
            System.Diagnostics.Debug.Assert(IsEq);

            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController NewController = FocusController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        static bool FocusBrowseNode(IFocusController controller, IFocusIndex index, Func<IFocusInner, bool> test)
        {
            Assert.That(index != null, "Focus #0");
            Assert.That(controller.Contains(index), "Focus #1");
            IFocusNodeState State = (IFocusNodeState)controller.IndexToState(index);
            Assert.That(State != null, "Focus #2");
            Assert.That(State.ParentIndex == index, "Focus #4");

            INode Node;

            if (State is IFocusPlaceholderNodeState AsPlaceholderState)
                Node = AsPlaceholderState.Node;
            else if (State is IFocusPatternState AsPatternState)
                Node = AsPatternState.Node;
            else if (State is IFocusSourceState AsSourceState)
                Node = AsSourceState.Node;
            else
            {
                Assert.That(State is IFocusOptionalNodeState, "Focus #5");
                IFocusOptionalNodeState AsOptionalState = (IFocusOptionalNodeState)State;
                IFocusOptionalInner ParentInner = AsOptionalState.ParentInner;

                Assert.That(ParentInner.IsAssigned, "Focus #6");

                Node = AsOptionalState.Node;
            }

            Type ChildNodeType;
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(Node);

            foreach (string PropertyName in PropertyNames)
            {
                if (NodeTreeHelperChild.IsChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    IFocusPlaceholderInner Inner = (IFocusPlaceholderInner)State.PropertyToInner(PropertyName);
                    if (!test(Inner))
                        return false;

                    IFocusNodeState ChildState = Inner.ChildState;
                    IFocusIndex ChildIndex = ChildState.ParentIndex;
                    if (!FocusBrowseNode(controller, ChildIndex, test))
                        return false;
                }

                else if (NodeTreeHelperOptional.IsOptionalChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelperOptional.GetChildNode(Node, PropertyName, out bool IsAssigned, out INode ChildNode);
                    if (IsAssigned)
                    {
                        IFocusOptionalInner Inner = (IFocusOptionalInner)State.PropertyToInner(PropertyName);
                        if (!test(Inner))
                            return false;

                        IFocusNodeState ChildState = Inner.ChildState;
                        IFocusIndex ChildIndex = ChildState.ParentIndex;
                        if (!FocusBrowseNode(controller, ChildIndex, test))
                            return false;
                    }
                }

                else if (NodeTreeHelperList.IsNodeListProperty(Node, PropertyName, out ChildNodeType))
                {
                    IFocusListInner Inner = (IFocusListInner)State.PropertyToInner(PropertyName);
                    if (!test(Inner))
                        return false;

                    for (int i = 0; i < Inner.StateList.Count; i++)
                    {
                        IFocusPlaceholderNodeState ChildState = Inner.StateList[i];
                        IFocusIndex ChildIndex = ChildState.ParentIndex;
                        if (!FocusBrowseNode(controller, ChildIndex, test))
                            return false;
                    }
                }

                else if (NodeTreeHelperBlockList.IsBlockListProperty(Node, PropertyName, out Type ChildInterfaceType, out ChildNodeType))
                {
                    IFocusBlockListInner Inner = (IFocusBlockListInner)State.PropertyToInner(PropertyName);
                    if (!test(Inner))
                        return false;

                    for (int BlockIndex = 0; BlockIndex < Inner.BlockStateList.Count; BlockIndex++)
                    {
                        IFocusBlockState BlockState = Inner.BlockStateList[BlockIndex];
                        if (!FocusBrowseNode(controller, BlockState.PatternIndex, test))
                            return false;
                        if (!FocusBrowseNode(controller, BlockState.SourceIndex, test))
                            return false;

                        for (int i = 0; i < BlockState.StateList.Count; i++)
                        {
                            IFocusPlaceholderNodeState ChildState = BlockState.StateList[i];
                            IFocusIndex ChildIndex = ChildState.ParentIndex;
                            if (!FocusBrowseNode(controller, ChildIndex, test))
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        static void MoveFocusRandomly(IFocusControllerView controllerView)
        {
            int MinFocusMove = controllerView.MinFocusMove;
            int MaxFocusMove = controllerView.MaxFocusMove;

            int Direction = MinFocusMove + RandNext(MaxFocusMove - MinFocusMove + 1);
            controllerView.MoveFocus(Direction, true, out bool IsMoved);
        }
        #endregion

        #region Layout
#if LAYOUT
        [Test]
        [TestCaseSource(nameof(FileIndexRange))]
        public static void Layout(int index)
        {
            if (TestOff)
                return;

            string Name = null;
            INode RootNode = null;
            int n = index;
            foreach (string FileName in FileNameTable)
            {
                if (n == 0)
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    {
                        Name = FileName;
                        Serializer Serializer = new Serializer();
                        RootNode = Serializer.Deserialize(fs) as INode;
                    }
                    break;
                }

                n--;
            }

            if (n > 0)
                throw new ArgumentOutOfRangeException($"{n} / {FileNameTable.Count}");
            TestLayout(index, Name, RootNode);
        }
#endif

        public static void TestLayout(int index, string name, INode rootNode)
        {
            ControllerTools.ResetExpectedName();

            TestLayoutStats(index, name, rootNode, out Stats Stats);

            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);

            ILayoutControllerView ControllerView;

            if (CustomLayoutTemplateSet.LayoutTemplateSet != null)
            {
                ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

                if (LayoutExpectedLastLineTable.ContainsKey(name))
                {
                    int ExpectedLastLineNumber = LayoutExpectedLastLineTable[name];
                    //Assert.That(ControllerView.LastLineNumber == ExpectedLastLineNumber, $"Last line number for {name}: {ControllerView.LastLineNumber}, expected: {ExpectedLastLineNumber}");
                }
                else
                {
                    /*
                    using (FileStream fs = new FileStream("lines.txt", FileMode.Append, FileAccess.Write))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine($"{{ \"{name}\", {ControllerView.LastLineNumber} }},");
                    }*/
                }

                TestLayoutCellViewList(ControllerView, name);
            }
            else
                ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, JustCount);
            LayoutMaxTestCount = LayoutTestCount;

            for (int i = 0; i < TestRepeatCount; i++)
            {
                SeedRand(0x12 + index * 256 + i * 65536);
                TestLayoutInsert(index, rootNode);
                TestLayoutRemove(index, rootNode);
                TestLayoutRemoveBlockRange(index, rootNode);
                TestLayoutReplaceBlockRange(index, rootNode);
                TestLayoutInsertBlockRange(index, rootNode);
                TestLayoutRemoveNodeRange(index, rootNode);
                TestLayoutReplaceNodeRange(index, rootNode);
                TestLayoutInsertNodeRange(index, rootNode);
                TestLayoutReplace(index, rootNode);
                TestLayoutAssign(index, rootNode);
                TestLayoutUnassign(index, rootNode);
                TestLayoutChangeReplication(index, rootNode);
                TestLayoutSplit(index, rootNode);
                TestLayoutMerge(index, rootNode);
                TestLayoutMove(index, rootNode);
                TestLayoutExpand(index, rootNode);
                TestLayoutReduce(index, rootNode);
            }

            TestLayoutCanonicalize(rootNode);

            LayoutTestNewItemInsertable(rootNode);
            LayoutTestItemRemoveable(rootNode);
            LayoutTestItemMoveable(rootNode);
            LayoutTestBlockMoveable(rootNode);
            LayoutTestItemSplittable(rootNode);
            LayoutTestItemMergeable(rootNode);
            LayoutTestItemCyclable(rootNode);
            LayoutTestItemSimplifiable(rootNode);
            LayoutTestItemComplexifiable(rootNode);
            LayoutTestIdentifierSplittable(rootNode);
            LayoutTestReplicationModifiable(rootNode);
        }

        public static void TestLayoutCellViewList(ILayoutControllerView controllerView, string name)
        {
            ILayoutVisibleCellViewList CellViewList = new LayoutVisibleCellViewList();
            controllerView.EnumerateVisibleCellViews((IFrameVisibleCellView item) => ListCellViews(item, CellViewList), out IFrameVisibleCellView FoundCellView, false);
            controllerView.Draw(controllerView.RootStateView);

            Assert.That(controllerView.LastLineNumber >= 1);
            Assert.That(controllerView.LastColumnNumber >= 1);

            ILayoutVisibleCellView[,] CellViewGrid = new ILayoutVisibleCellView[controllerView.LastLineNumber, controllerView.LastColumnNumber];

            foreach (ILayoutVisibleCellView CellView in CellViewList)
            {
                int LineNumber = CellView.LineNumber - 1;
                int ColumnNumber = CellView.ColumnNumber - 1;

                Assert.That(LineNumber >= 0);
                Assert.That(LineNumber < controllerView.LastLineNumber);
                Assert.That(ColumnNumber >= 0);
                Assert.That(ColumnNumber < controllerView.LastColumnNumber);

                Assert.That(CellViewGrid[LineNumber, ColumnNumber] == null);
                CellViewGrid[LineNumber, ColumnNumber] = CellView;

                ILayoutFrame Frame = CellView.Frame;
                INode ChildNode = CellView.StateView.State.Node;
                string PropertyName;

                switch (CellView)
                {
                    case ILayoutDiscreteContentFocusableCellView AsDiscreteContentFocusable: // Enum, bool
                        PropertyName = AsDiscreteContentFocusable.PropertyName;
                        Assert.That(NodeTreeHelper.IsEnumProperty(ChildNode, PropertyName) || NodeTreeHelper.IsBooleanProperty(ChildNode, PropertyName));
                        break;
                    case ILayoutStringContentFocusableCellView AsTextFocusable: // String
                        PropertyName = AsTextFocusable.PropertyName;
                        Assert.That(NodeTreeHelper.IsStringProperty(ChildNode, PropertyName) && PropertyName == "Text");
                        break;
                    case ILayoutFocusableCellView AsFocusable: // Insert
                        Assert.That((Frame is ILayoutInsertFrame) || (Frame is ILayoutCommentFrame) || (Frame is ILayoutKeywordFrame AsFocusableKeywordFrame && AsFocusableKeywordFrame.IsFocusable));
                        break;
                    case ILayoutVisibleCellView AsVisible: // Others
                        Assert.That(((Frame is ILayoutKeywordFrame AsKeywordFrame && !AsKeywordFrame.IsFocusable) && !string.IsNullOrEmpty(AsKeywordFrame.Text)) || (Frame is ILayoutSymbolFrame AsSymbolFrame));
                        break;
                }
            }

            int IndexFirstLine = -1;
            for (int i = 0; i < controllerView.LastColumnNumber; i++)
                if (CellViewGrid[0, i] != null)
                {
                    IndexFirstLine = i;
                    break;
                }
            Assert.That(IndexFirstLine >= 0);

            int IndexFirstColumn = -1;
            for (int i = 0; i < controllerView.LastLineNumber; i++)
                if (CellViewGrid[0, i] != null)
                {
                    IndexFirstColumn = i;
                    break;
                }
            Assert.That(IndexFirstColumn >= 0);

            int IndexLastLine = -1;
            for (int i = 0; i < controllerView.LastColumnNumber; i++)
                if (CellViewGrid[controllerView.LastLineNumber - 1, i] != null)
                {
                    IndexLastLine = i;
                    break;
                }
            Assert.That(IndexLastLine >= 0);

            int IndexLastColumn = -1;
            for (int i = 0; i < controllerView.LastLineNumber; i++)
                if (CellViewGrid[i, controllerView.LastColumnNumber - 1] != null)
                {
                    IndexLastColumn = i;
                    break;
                }
            Assert.That(IndexLastColumn >= 0);
        }

        public static Dictionary<string, int> LayoutExpectedLastLineTable = new Dictionary<string, int>()
        {
            { "./test.easly", 193 },
            { "./EaslyExamples/CoreEditor/Classes/Agent Expression.easly", 193 },
            { "./EaslyExamples/CoreEditor/Classes/Basic Key Event Handler.easly", 855 },
            { "./EaslyExamples/CoreEditor/Classes/Block Editor Node Management.easly", 62 },
            { "./EaslyExamples/CoreEditor/Classes/Block Editor Node.easly", 162 },
            { "./EaslyExamples/CoreEditor/Classes/Block List Editor Node Management.easly", 62 },
            { "./EaslyExamples/CoreEditor/Classes/Block List Editor Node.easly", 252 },
            { "./EaslyExamples/CoreEditor/Classes/Control Key Event Handler.easly", 150 },
            { "./EaslyExamples/CoreEditor/Classes/Control With Decoration Management.easly", 644 },
            { "./EaslyExamples/CoreEditor/Classes/Decoration.easly", 47 },
            { "./EaslyExamples/CoreEditor/Classes/Editor Node Management.easly", 124 },
            { "./EaslyExamples/CoreEditor/Classes/Editor Node.easly", 17 },
            { "./EaslyExamples/CoreEditor/Classes/Horizontal Separator.easly", 35 },
            { "./EaslyExamples/CoreEditor/Classes/Identifier Key Event Handler.easly", 56 },
            { "./EaslyExamples/CoreEditor/Classes/Insertion Position.easly", 34 },
            { "./EaslyExamples/CoreEditor/Classes/Key Descriptor.easly", 83 },
            { "./EaslyExamples/CoreEditor/Classes/Node Selection.easly", 67 },
            { "./EaslyExamples/CoreEditor/Classes/Node With Default.easly", 27 },
            { "./EaslyExamples/CoreEditor/Classes/Properties Show Options.easly", 120 },
            { "./EaslyExamples/CoreEditor/Classes/Property Changed Notifier.easly", 57 },
            { "./EaslyExamples/CoreEditor/Classes/Replace Notification.easly", 44 },
            { "./EaslyExamples/CoreEditor/Classes/Simplify Notification.easly", 29 },
            { "./EaslyExamples/CoreEditor/Classes/Specialized Decoration.easly", 66 },
            { "./EaslyExamples/CoreEditor/Classes/Toggle Notification.easly", 41 },
            { "./EaslyExamples/CoreEditor/Libraries/Constructs.easly", 5 },
            { "./EaslyExamples/CoreEditor/Libraries/Nodes.easly", 5 },
            { "./EaslyExamples/CoreEditor/Libraries/SSC Editor.easly", 21 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Agent Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Anchor Kinds.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Anchored Type.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Argument.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/As Long As Instruction.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Assertion Tag Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Assertion.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Assignment Argument.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Assignment Instruction.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Assignment Type Argument.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Attachment Instruction.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Attachment.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Attribute Feature.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Binary Operator Expression.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Block List.easly", 30 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Block.easly", 17 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Body.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Check Instruction.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Class Constant Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Class Replicate.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Class.easly", 99 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Clone Of Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Clone Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Cloneable Status.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Command Instruction.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Command Overload Type.easly", 51 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Command Overload.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Comparable Status.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Comparison Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Conditional.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Conformance Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Constant Feature.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Constraint.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Continuation.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Copy Semantic.easly", 34 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Create Instruction.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Creation Feature.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Debug Instruction.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Deferred Body.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Discrete.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Effective Body.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Entity Declaration.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Entity Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Equality Expression.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Equality Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Event Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Exception Handler.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Export Change.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Export Status.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Export.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Expression.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Extern Body.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Feature.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/For Loop Instruction.easly", 55 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Function Feature.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Function Type.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Generic Type.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Generic.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Global Replicate.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Identifier.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/If Then Else Instruction.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Import Type.easly", 34 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Import.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Index Assignment Instruction.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Index Query Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Indexer Feature.easly", 55 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Indexer Type.easly", 75 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Inheritance.easly", 71 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Initialized Object Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Inspect Instruction.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Instruction.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Iteration Type.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Keyword Anchored Type.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Keyword Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Keyword.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Library.easly", 47 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Manifest Character Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Manifest Number Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Manifest String Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Name.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Named Feature.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/New Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Node.easly", 12 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Object Type.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Old Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Once Choice.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Over Loop Instruction.easly", 51 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Parameter End Status.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Pattern.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Positional Argument.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Positional Type Argument.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Precursor Body.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Precursor Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Precursor Index Assignment Instruction.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Precursor Index Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Precursor Instruction.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Preprocessor Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Preprocessor Macro.easly", 40 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Procedure Feature.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Procedure Type.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Property Feature.easly", 51 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Property Type.easly", 59 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Qualified Name.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Query Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Query Overload Type.easly", 55 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Query Overload.easly", 55 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Raise Event Instruction.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Range.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Release Instruction.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Rename.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Replication Status.easly", 33 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Result Of Expression.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Root.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Scope.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Shareable Type.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Sharing Type.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Simple Type.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Throw Instruction.easly", 43 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Tuple Type.easly", 35 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Type Argument.easly", 29 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Typedef.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Unary Operator Expression.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/Utility Type.easly", 34 },
            { "./EaslyExamples/EaslyCoreLanguage/Classes/With.easly", 39 },
            { "./EaslyExamples/EaslyCoreLanguage/Libraries/Constructs.easly", 5 },
            { "./EaslyExamples/EaslyCoreLanguage/Libraries/Nodes.easly", 6 },
            { "./EaslyExamples/EaslyCoreLanguage/Libraries/SSC Language.easly", 15 },
            { "./EaslyExamples/EaslyCoreLanguage/Replicates/SSC Core Language Nodes.easly", 1 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/.NET Event.easly", 43 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.ComponentModel.PropertyChangedEventArgs.easly", 44 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.ComponentModel.PropertyChangedEventHandler.easly", 48 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Controls.Orientation.easly", 33 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Controls.TextBox.easly", 73 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.DependencyObject.easly", 20 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.LayoutworkElement.easly", 60 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Input.LayoutNavigationDirection.easly", 39 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Input.Key.easly", 72 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Input.Keyboard.easly", 68 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Input.KeyEventArgs.easly", 40 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Input.TraversalRequest.easly", 40 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.InputElement.easly", 20 },
            { "./EaslyExamples/MicrosoftDotNet/Classes/System.Windows.Media.VisualTreeHelper.easly", 68 },
            { "./EaslyExamples/MicrosoftDotNet/Libraries/.NET Classes.easly", 5 },
            { "./EaslyExamples/MicrosoftDotNet/Libraries/.NET Enums.easly", 5 },
            { "./EaslyExamples/Verification/Verification Example.easly", 80 },
        };

        static int LayoutTestCount = 0;
        static int LayoutMaxTestCount = 0;

        public static bool JustCount(ILayoutInner inner)
        {
            LayoutTestCount++;
            return true;
        }

        public static void TestLayoutStats(int index, string name, INode rootNode, out Stats stats)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);

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

        public static void TestLayoutInsert(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => InsertAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool InsertAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            ILayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    int Index = RandNext(AsListInner.StateList.Count + 1);
                    ILayoutInsertionListNodeIndex NodeIndex = new LayoutInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, Index);
                    Controller.Insert(AsListInner, NodeIndex, out IWriteableBrowsingCollectionNodeIndex InsertedIndex);
                    Assert.That(Controller.Contains(InsertedIndex));

                    ILayoutPlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as ILayoutPlaceholderNodeState;
                    Assert.That(ChildState != null);
                    Assert.That(ChildState.Node == NewNode);

                    IsModified = true;
                }
            }
            else if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    if (RandNext(2) == 0)
                    {
                        int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        ILayoutBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];
                        int Index = RandNext(BlockState.StateList.Count + 1);

                        ILayoutInsertionExistingBlockNodeIndex NodeIndex = new LayoutInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, Index);
                        Controller.Insert(AsBlockListInner, NodeIndex, out IWriteableBrowsingCollectionNodeIndex InsertedIndex);
                        Assert.That(Controller.Contains(InsertedIndex));

                        ILayoutPlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as ILayoutPlaceholderNodeState;
                        Assert.That(ChildState != null);
                        Assert.That(ChildState.Node == NewNode);

                        IsModified = true;
                    }
                    else
                    {
                        int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count + 1);

                        IPattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                        IIdentifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
                        ILayoutInsertionNewBlockNodeIndex NodeIndex = new LayoutInsertionNewBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, ReplicationPattern, SourceIdentifier);
                        Controller.Insert(AsBlockListInner, NodeIndex, out IWriteableBrowsingCollectionNodeIndex InsertedIndex);
                        Assert.That(Controller.Contains(InsertedIndex));

                        ILayoutPlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as ILayoutPlaceholderNodeState;
                        Assert.That(ChildState != null);
                        Assert.That(ChildState.Node == NewNode);

                        IsModified = true;
                    }
                }
            }

            if (IsModified)
            {
                ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                ILayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutReplace(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => ReplaceAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool ReplaceAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            ILayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutPlaceholderInner AsPlaceholderInner)
            {
                INode NewNode = NodeHelper.DeepCloneNode(AsPlaceholderInner.ChildState.Node, cloneCommentGuid: false);
                Assert.That(NewNode != null, $"Type: {AsPlaceholderInner.InterfaceType}");

                ILayoutInsertionPlaceholderNodeIndex NodeIndex = new LayoutInsertionPlaceholderNodeIndex(AsPlaceholderInner.Owner.Node, AsPlaceholderInner.PropertyName, NewNode);
                Controller.Replace(AsPlaceholderInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                Assert.That(Controller.Contains(InsertedIndex));

                ILayoutPlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as ILayoutPlaceholderNodeState;
                Assert.That(ChildState != null);
                Assert.That(ChildState.Node == NewNode);

                IsModified = true;
            }
            else if (inner is ILayoutOptionalInner AsOptionalInner)
            {
                ILayoutOptionalNodeState State = AsOptionalInner.ChildState;
                IOptionalReference Optional = State.ParentIndex.Optional;
                Type NodeInterfaceType = Optional.GetType().GetGenericArguments()[0];
                INode NewNode = NodeHelper.CreateDefaultFromInterface(NodeInterfaceType);
                Assert.That(NewNode != null, $"Type: {AsOptionalInner.InterfaceType}");

                ILayoutInsertionOptionalNodeIndex NodeIndex = new LayoutInsertionOptionalNodeIndex(AsOptionalInner.Owner.Node, AsOptionalInner.PropertyName, NewNode);
                Controller.Replace(AsOptionalInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                Assert.That(Controller.Contains(InsertedIndex));

                ILayoutOptionalNodeState ChildState = Controller.IndexToState(InsertedIndex) as ILayoutOptionalNodeState;
                Assert.That(ChildState != null);
                Assert.That(ChildState.Node == NewNode);

                IsModified = true;
            }
            else if (inner is ILayoutListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    int Index = RandNext(AsListInner.StateList.Count);
                    ILayoutInsertionListNodeIndex NodeIndex = new LayoutInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, Index);
                    Controller.Replace(AsListInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                    Assert.That(Controller.Contains(InsertedIndex));

                    ILayoutPlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as ILayoutPlaceholderNodeState;
                    Assert.That(ChildState != null);
                    Assert.That(ChildState.Node == NewNode);

                    IsModified = true;
                }
            }
            else if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    ILayoutBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];
                    int Index = RandNext(BlockState.StateList.Count);

                    ILayoutInsertionExistingBlockNodeIndex NodeIndex = new LayoutInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, Index);
                    Controller.Replace(AsBlockListInner, NodeIndex, out IWriteableBrowsingChildIndex InsertedIndex);
                    Assert.That(Controller.Contains(InsertedIndex));

                    ILayoutPlaceholderNodeState ChildState = Controller.IndexToState(InsertedIndex) as ILayoutPlaceholderNodeState;
                    Assert.That(ChildState != null);
                    Assert.That(ChildState.Node == NewNode);

                    IsModified = true;
                }
            }

            if (IsModified)
            {
                ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                ILayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutRemove(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => RemoveAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool RemoveAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            ILayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int Index = RandNext(AsListInner.StateList.Count);
                    ILayoutNodeState ChildState = AsListInner.StateList[Index];
                    ILayoutBrowsingListNodeIndex NodeIndex = ChildState.ParentIndex as ILayoutBrowsingListNodeIndex;
                    Assert.That(NodeIndex != null);

                    if (Controller.IsRemoveable(AsListInner, NodeIndex))
                    {
                        Controller.Remove(AsListInner, NodeIndex);
                        IsModified = true;
                    }
                }
            }
            else if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    ILayoutBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];
                    int Index = RandNext(BlockState.StateList.Count);
                    ILayoutNodeState ChildState = BlockState.StateList[Index];
                    ILayoutBrowsingExistingBlockNodeIndex NodeIndex = ChildState.ParentIndex as ILayoutBrowsingExistingBlockNodeIndex;
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
                ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                ILayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutRemoveBlockRange(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => RemoveBlockRangeAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool RemoveBlockRangeAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            ILayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
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
                ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                ILayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutReplaceBlockRange(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => ReplaceBlockRangeAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool ReplaceBlockRangeAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            ILayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int FirstBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int LastBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count + 1);

                    if (FirstBlockIndex > LastBlockIndex)
                        FirstBlockIndex = LastBlockIndex;

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IPattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                    IIdentifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
                    ILayoutInsertionNewBlockNodeIndex NewNodeIndex = new LayoutInsertionNewBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, FirstBlockIndex, ReplicationPattern, SourceIdentifier);

                    NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    ILayoutInsertionExistingBlockNodeIndex ExistingNodeIndex = new LayoutInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, FirstBlockIndex, 1);

                    List<IWriteableInsertionBlockNodeIndex> IndexList = new List<IWriteableInsertionBlockNodeIndex>() { NewNodeIndex, ExistingNodeIndex };
                    Controller.ReplaceBlockRange(AsBlockListInner, FirstBlockIndex, LastBlockIndex, IndexList);
                    IsModified = true;
                }
            }

            if (IsModified)
            {
                ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                ILayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutInsertBlockRange(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => InsertBlockRangeAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool InsertBlockRangeAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            ILayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int InsertedIndex = RandNext(AsBlockListInner.BlockStateList.Count);

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    IPattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                    IIdentifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
                    ILayoutInsertionNewBlockNodeIndex NewNodeIndex = new LayoutInsertionNewBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, InsertedIndex, ReplicationPattern, SourceIdentifier);

                    NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    ILayoutInsertionExistingBlockNodeIndex ExistingNodeIndex = new LayoutInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, InsertedIndex, 1);

                    List<IWriteableInsertionBlockNodeIndex> IndexList = new List<IWriteableInsertionBlockNodeIndex>() { NewNodeIndex, ExistingNodeIndex };
                    Controller.InsertBlockRange(AsBlockListInner, InsertedIndex, IndexList);
                    IsModified = true;
                }
            }

            if (IsModified)
            {
                ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                ILayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutRemoveNodeRange(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => RemoveNodeRangeAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool RemoveNodeRangeAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            ILayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
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
                        Controller.RemoveNodeRange(AsBlockListInner, BlockIndex, FirstNodeIndex, LastNodeIndex);
                        IsModified = FirstNodeIndex < LastNodeIndex;
                    }
                }
            }

            else if (inner is ILayoutListInner AsListInner)
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
                ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                ILayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutReplaceNodeRange(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => ReplaceNodeRangeAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool ReplaceNodeRangeAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            ILayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int FirstNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count);
                    int LastNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count + 1);

                    if (FirstNodeIndex > LastNodeIndex)
                        FirstNodeIndex = LastNodeIndex;

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    ILayoutInsertionExistingBlockNodeIndex ExistingNodeIndex = new LayoutInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, FirstNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    if (Controller.IsNodeRangeRemoveable(AsBlockListInner, BlockIndex, FirstNodeIndex, LastNodeIndex))
                    {
                        Controller.ReplaceNodeRange(AsBlockListInner, BlockIndex, FirstNodeIndex, LastNodeIndex, IndexList);
                        IsModified = IndexList.Count > 0 || FirstNodeIndex < LastNodeIndex;
                    }
                }
            }

            else if (inner is ILayoutListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int BlockIndex = -1;
                    int FirstNodeIndex = RandNext(AsListInner.StateList.Count);
                    int LastNodeIndex = RandNext(AsListInner.StateList.Count + 1);

                    if (FirstNodeIndex > LastNodeIndex)
                        FirstNodeIndex = LastNodeIndex;

                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    ILayoutInsertionListNodeIndex ExistingNodeIndex = new LayoutInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, FirstNodeIndex);
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
                ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                ILayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutInsertNodeRange(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => InsertNodeRangeAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool InsertNodeRangeAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            ILayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int InsertedNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count);

                    INode NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    ILayoutInsertionExistingBlockNodeIndex ExistingNodeIndex = new LayoutInsertionExistingBlockNodeIndex(AsBlockListInner.Owner.Node, AsBlockListInner.PropertyName, NewNode, BlockIndex, InsertedNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    Controller.InsertNodeRange(AsBlockListInner, BlockIndex, InsertedNodeIndex, IndexList);
                    IsModified = true;
                }
            }

            else if (inner is ILayoutListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int BlockIndex = -1;
                    int InsertedNodeIndex = RandNext(AsListInner.StateList.Count);

                    INode NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    ILayoutInsertionListNodeIndex ExistingNodeIndex = new LayoutInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, InsertedNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    Controller.InsertNodeRange(AsListInner, BlockIndex, InsertedNodeIndex, IndexList);
                    IsModified = true;
                }
            }

            if (IsModified)
            {
                ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                ILayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutAssign(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => AssignAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool AssignAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            ILayoutController Controller = controllerView.Controller;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutOptionalInner AsOptionalInner)
            {
                ILayoutOptionalNodeState ChildState = AsOptionalInner.ChildState;
                Assert.That(ChildState != null);

                ILayoutBrowsingOptionalNodeIndex OptionalIndex = ChildState.ParentIndex;
                Assert.That(Controller.Contains(OptionalIndex));

                IOptionalReference Optional = OptionalIndex.Optional;
                Assert.That(Optional != null);

                if (Optional.HasItem)
                {
                    Controller.Assign(OptionalIndex, out bool IsChanged);
                    Assert.That(Optional.IsAssigned);
                    Assert.That(AsOptionalInner.IsAssigned);
                    Assert.That(Optional.Item == ChildState.Node);

                    ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                    ILayoutController NewController = LayoutController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    if (IsChanged)
                    {
                        Controller.Undo();

                        Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                        ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                        Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                    }
                }
            }

            return false;
        }

        public static void TestLayoutUnassign(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => UnassignAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool UnassignAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            ILayoutController Controller = controllerView.Controller;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutOptionalInner AsOptionalInner)
            {
                ILayoutOptionalNodeState ChildState = AsOptionalInner.ChildState;
                Assert.That(ChildState != null);

                ILayoutBrowsingOptionalNodeIndex OptionalIndex = ChildState.ParentIndex;
                Assert.That(Controller.Contains(OptionalIndex));

                IOptionalReference Optional = OptionalIndex.Optional;
                Assert.That(Optional != null);

                Controller.Unassign(OptionalIndex, out bool IsChanged);
                Assert.That(!Optional.IsAssigned);
                Assert.That(!AsOptionalInner.IsAssigned);

                ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                ILayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                if (IsChanged)
                {
                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestLayoutChangeReplication(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => ChangeReplicationAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool ChangeReplicationAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            ILayoutController Controller = controllerView.Controller;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    ILayoutBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];

                    ReplicationStatus Replication = (ReplicationStatus)RandNext(2);
                    Controller.ChangeReplication(AsBlockListInner, BlockIndex, Replication);

                    ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                    ILayoutController NewController = LayoutController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestLayoutSplit(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => SplitAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool SplitAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            ILayoutController Controller = controllerView.Controller;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int SplitBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    ILayoutBlockState BlockState = AsBlockListInner.BlockStateList[SplitBlockIndex];
                    if (BlockState.StateList.Count > 1)
                    {
                        int SplitIndex = 1 + RandNext(BlockState.StateList.Count - 1);
                        ILayoutBrowsingExistingBlockNodeIndex NodeIndex = (ILayoutBrowsingExistingBlockNodeIndex)AsBlockListInner.IndexAt(SplitBlockIndex, SplitIndex);
                        Controller.SplitBlock(AsBlockListInner, NodeIndex);

                        ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                        Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                        ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                        ILayoutController NewController = LayoutController.Create(NewRootIndex);
                        Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                        Controller.Undo();

                        Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                        ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                        Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                        Assert.That(AsBlockListInner.BlockStateList.Count > 0);
                        int OldBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        int NewBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        int Direction = NewBlockIndex - OldBlockIndex;

                        Assert.That(Controller.IsBlockMoveable(AsBlockListInner, OldBlockIndex, Direction));

                        Controller.MoveBlock(AsBlockListInner, OldBlockIndex, Direction);

                        ILayoutControllerView NewViewAfterMove = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                        Assert.That(NewViewAfterMove.IsEqual(CompareEqual.New(), controllerView));

                        ILayoutRootNodeIndex NewRootIndexAfterMove = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                        ILayoutController NewControllerAfterMove = LayoutController.Create(NewRootIndexAfterMove);
                        Assert.That(NewControllerAfterMove.IsEqual(CompareEqual.New(), Controller));
                    }
                }
            }

            return false;
        }

        public static void TestLayoutMerge(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => MergeAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool MergeAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            ILayoutController Controller = controllerView.Controller;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 1)
                {
                    int MergeBlockIndex = 1 + RandNext(AsBlockListInner.BlockStateList.Count - 1);
                    ILayoutBlockState BlockState = AsBlockListInner.BlockStateList[MergeBlockIndex];

                    ILayoutBrowsingExistingBlockNodeIndex NodeIndex = (ILayoutBrowsingExistingBlockNodeIndex)AsBlockListInner.IndexAt(MergeBlockIndex, 0);
                    Controller.MergeBlocks(AsBlockListInner, NodeIndex);

                    ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                    ILayoutController NewController = LayoutController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestLayoutMove(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => MoveAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool MoveAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            ILayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int OldIndex = RandNext(AsListInner.StateList.Count);
                    int NewIndex = RandNext(AsListInner.StateList.Count);
                    int Direction = NewIndex - OldIndex;

                    ILayoutBrowsingListNodeIndex NodeIndex = AsListInner.IndexAt(OldIndex) as ILayoutBrowsingListNodeIndex;
                    Assert.That(NodeIndex != null);

                    Assert.That(Controller.IsMoveable(AsListInner, NodeIndex, Direction));

                    Controller.Move(AsListInner, NodeIndex, Direction);
                    Assert.That(Controller.Contains(NodeIndex));

                    IsModified = true;
                }
            }
            else if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    ILayoutBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];

                    int OldIndex = RandNext(BlockState.StateList.Count);
                    int NewIndex = RandNext(BlockState.StateList.Count);
                    int Direction = NewIndex - OldIndex;

                    ILayoutBrowsingExistingBlockNodeIndex NodeIndex = AsBlockListInner.IndexAt(BlockIndex, OldIndex) as ILayoutBrowsingExistingBlockNodeIndex;
                    Assert.That(NodeIndex != null);

                    Assert.That(Controller.IsMoveable(AsBlockListInner, NodeIndex, Direction));

                    Controller.Move(AsBlockListInner, NodeIndex, Direction);
                    Assert.That(Controller.Contains(NodeIndex));

                    IsModified = true;
                }
            }

            if (IsModified)
            {
                ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                ILayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutExpand(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => ExpandAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool ExpandAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            ILayoutController Controller = controllerView.Controller;
            ILayoutNodeIndex NodeIndex;
            ILayoutPlaceholderNodeState State;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutPlaceholderInner AsPlaceholderInner)
            {
                NodeIndex = AsPlaceholderInner.ChildState.ParentIndex as ILayoutNodeIndex;
                Assert.That(NodeIndex != null);

                State = Controller.IndexToState(NodeIndex) as ILayoutPlaceholderNodeState;
                Assert.That(State != null);

                NodeTreeHelper.GetArgumentBlocks(State.Node, out IDictionary<string, IBlockList<IArgument, Argument>> ArgumentBlocksTable);
                if (ArgumentBlocksTable.Count == 0)
                    return true;
            }
            else
                return true;

            Controller.Expand(NodeIndex, out bool IsChanged);

            ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                Controller.Redo();
            }

            Controller.Expand(NodeIndex, out IsChanged);

            NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            Controller.Reduce(NodeIndex, out IsChanged);

            NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            return false;
        }

        public static void TestLayoutReduce(int index, INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => ReduceAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool ReduceAndCompare(ILayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            ILayoutController Controller = controllerView.Controller;
            ILayoutNodeIndex NodeIndex;
            ILayoutPlaceholderNodeState State;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutPlaceholderInner AsPlaceholderInner)
            {
                NodeIndex = AsPlaceholderInner.ChildState.ParentIndex as ILayoutNodeIndex;
                Assert.That(NodeIndex != null);

                State = Controller.IndexToState(NodeIndex) as ILayoutPlaceholderNodeState;
                Assert.That(State != null);

                NodeTreeHelper.GetArgumentBlocks(State.Node, out IDictionary<string, IBlockList<IArgument, Argument>> ArgumentBlocksTable);
                if (ArgumentBlocksTable.Count == 0)
                    return true;
            }
            else
                return true;

            Controller.Reduce(NodeIndex, out bool IsChanged);

            ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                Controller.Redo();
            }

            Controller.Reduce(NodeIndex, out IsChanged);

            NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            Controller.Expand(NodeIndex, out IsChanged);

            NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            return false;
        }

        public static void TestLayoutCanonicalize(INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController OldController = LayoutController.Create(OldRootIndex);

            Controller.Canonicalize(out bool IsChanged);

            ILayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"RootNode: {rootNode}");
                ILayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), ControllerView));
            }
        }

        public static void LayoutTestNewItemInsertable(INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            int Min = ControllerView.MinFocusMove;
            int Max = ControllerView.MaxFocusMove;
            bool IsMoved;

            for (int i = 0; i < (Max - Min) + 10; i++)
            {
                ControllerView.MoveFocus(+1, true, out IsMoved);

                if (ControllerView.IsNewItemInsertable(out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index))
                    Controller.Insert(inner, index, out IWriteableBrowsingCollectionNodeIndex nodeIndex);
            }

            for (int i = 0; i < 10; i++)
            {
                Min = ControllerView.MinFocusMove;
                Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out IsMoved);

                if (ControllerView.IsNewItemInsertable(out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index))
                    Controller.Insert(inner, index, out IWriteableBrowsingCollectionNodeIndex nodeIndex);
            }

            ILayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestItemRemoveable(INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemRemoveable(out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index))
                    Controller.Remove(inner, index);
            }

            ILayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestItemMoveable(INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                Direction = (RandNext(2) * 2) - 1;

                if (ControllerView.IsItemMoveable(Direction, out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index))
                    Controller.Move(inner, index, Direction);
            }

            ILayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestBlockMoveable(INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                Direction = (RandNext(2) * 2) - 1;

                if (ControllerView.IsBlockMoveable(Direction, out IFocusBlockListInner Inner, out int BlockIndex))
                    Controller.MoveBlock(Inner, BlockIndex, Direction);
            }

            ILayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestItemSplittable(INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemSplittable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index))
                    Controller.SplitBlock(inner, index);
            }

            ILayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestItemMergeable(INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemMergeable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index))
                    Controller.MergeBlocks(inner, index);
            }

            ILayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestItemCyclable(INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemCyclableThrough(out IFocusCyclableNodeState state, out int cyclePosition))
                    Controller.Replace(state.ParentInner, state.CycleIndexList, cyclePosition, out IFocusBrowsingChildIndex nodeIndex);
            }

            ILayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestItemSimplifiable(INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            List<ILayoutInner> InnerList = new List<ILayoutInner>();
            List<ILayoutInsertionChildIndex> IndexList = new List<ILayoutInsertionChildIndex>();
            List<int> nList = new List<int>();

            total++;
            if (total == 0x03)
            {
                total = 0x03;
                //System.Diagnostics.Debug.Assert(false);
            }

            for (int i = 0; i < /*200*/89; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemSimplifiable(out IFocusInner Inner, out IFocusInsertionChildIndex Index))
                {
                    InnerList.Add((ILayoutInner)Inner);
                    IndexList.Add((ILayoutInsertionChildIndex)Index);
                    nList.Add(i);

                    //System.Diagnostics.Debug.Assert(i < 88);
                    Controller.Replace(Inner, Index, out IWriteableBrowsingChildIndex NodeIndex);
                    //break;
                }
            }

            foreach (ILayoutInner<ILayoutBrowsingChildIndex> Inner in InnerList)
            {
                string s = $"{Inner.Owner.Node}: {Inner.PropertyName}";
                System.Diagnostics.Debug.WriteLine(s);
            }
            /*
            ILayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));*/
        }
        static int total = 0;

        public static void LayoutTestItemComplexifiable(INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            for (int i = 0; i < 200; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemComplexifiable(out IDictionary<IFocusInner, IList<IFocusInsertionChildNodeIndex>> IndexTable))
                {
                    int Total = 0;
                    foreach (KeyValuePair<IFocusInner, IList<IFocusInsertionChildNodeIndex>> Entry in IndexTable)
                        Total += Entry.Value.Count;

                    int Choice = RandNext(Total);
                    foreach (KeyValuePair<IFocusInner, IList<IFocusInsertionChildNodeIndex>> Entry in IndexTable)
                        foreach (IFocusInsertionChildNodeIndex Index in Entry.Value)
                            if (Choice == 0)
                            {
                                Controller.Replace(Entry.Key, Index, out IWriteableBrowsingChildIndex NodeIndex);
                                break;
                            }
                            else
                                Choice--;
                }
            }
        }

        public static void LayoutTestIdentifierSplittable(INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            for (int i = 0; i < 200; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsIdentifierSplittable(out IFocusListInner Inner, out IFocusInsertionListNodeIndex ReplaceIndex, out IFocusInsertionListNodeIndex InsertIndex))
                    Controller.SplitIdentifier(Inner, ReplaceIndex, InsertIndex, out IWriteableBrowsingListNodeIndex FirstIndex, out IWriteableBrowsingListNodeIndex SecondIndex);
            }

            ILayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestReplicationModifiable(INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            ReplicationStatus Replication;

            for (int i = 0; i < 200; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsReplicationModifiable(out IFocusBlockListInner Inner, out int BlockIndex, out Replication))
                {
                    switch (Replication)
                    {
                        case ReplicationStatus.Normal:
                            Replication = ReplicationStatus.Replicated;
                            break;
                        case ReplicationStatus.Replicated:
                            Replication = ReplicationStatus.Normal;
                            break;
                    }

                    Controller.ChangeReplication(Inner, BlockIndex, Replication);
                }
            }

            ILayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            CompareEqual comparer = CompareEqual.New();
            bool IsEq = NewView.IsEqual(comparer, ControllerView);
            System.Diagnostics.Debug.Assert(IsEq);

            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            ILayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        static bool LayoutBrowseNode(ILayoutController controller, ILayoutIndex index, Func<ILayoutInner, bool> test)
        {
            Assert.That(index != null, "Layout #0");
            Assert.That(controller.Contains(index), "Layout #1");
            ILayoutNodeState State = (ILayoutNodeState)controller.IndexToState(index);
            Assert.That(State != null, "Layout #2");
            Assert.That(State.ParentIndex == index, "Layout #4");

            INode Node;

            if (State is ILayoutPlaceholderNodeState AsPlaceholderState)
                Node = AsPlaceholderState.Node;
            else if (State is ILayoutPatternState AsPatternState)
                Node = AsPatternState.Node;
            else if (State is ILayoutSourceState AsSourceState)
                Node = AsSourceState.Node;
            else
            {
                Assert.That(State is ILayoutOptionalNodeState, "Layout #5");
                ILayoutOptionalNodeState AsOptionalState = (ILayoutOptionalNodeState)State;
                ILayoutOptionalInner ParentInner = AsOptionalState.ParentInner;

                Assert.That(ParentInner.IsAssigned, "Layout #6");

                Node = AsOptionalState.Node;
            }

            Type ChildNodeType;
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(Node);

            foreach (string PropertyName in PropertyNames)
            {
                if (NodeTreeHelperChild.IsChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    ILayoutPlaceholderInner Inner = (ILayoutPlaceholderInner)State.PropertyToInner(PropertyName);
                    if (!test(Inner))
                        return false;

                    ILayoutNodeState ChildState = Inner.ChildState;
                    ILayoutIndex ChildIndex = ChildState.ParentIndex;
                    if (!LayoutBrowseNode(controller, ChildIndex, test))
                        return false;
                }

                else if (NodeTreeHelperOptional.IsOptionalChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelperOptional.GetChildNode(Node, PropertyName, out bool IsAssigned, out INode ChildNode);
                    if (IsAssigned)
                    {
                        ILayoutOptionalInner Inner = (ILayoutOptionalInner)State.PropertyToInner(PropertyName);
                        if (!test(Inner))
                            return false;

                        ILayoutNodeState ChildState = Inner.ChildState;
                        ILayoutIndex ChildIndex = ChildState.ParentIndex;
                        if (!LayoutBrowseNode(controller, ChildIndex, test))
                            return false;
                    }
                }

                else if (NodeTreeHelperList.IsNodeListProperty(Node, PropertyName, out ChildNodeType))
                {
                    ILayoutListInner Inner = (ILayoutListInner)State.PropertyToInner(PropertyName);
                    if (!test(Inner))
                        return false;

                    for (int i = 0; i < Inner.StateList.Count; i++)
                    {
                        ILayoutPlaceholderNodeState ChildState = Inner.StateList[i];
                        ILayoutIndex ChildIndex = ChildState.ParentIndex;
                        if (!LayoutBrowseNode(controller, ChildIndex, test))
                            return false;
                    }
                }

                else if (NodeTreeHelperBlockList.IsBlockListProperty(Node, PropertyName, out Type ChildInterfaceType, out ChildNodeType))
                {
                    ILayoutBlockListInner Inner = (ILayoutBlockListInner)State.PropertyToInner(PropertyName);
                    if (!test(Inner))
                        return false;

                    for (int BlockIndex = 0; BlockIndex < Inner.BlockStateList.Count; BlockIndex++)
                    {
                        ILayoutBlockState BlockState = Inner.BlockStateList[BlockIndex];
                        if (!LayoutBrowseNode(controller, BlockState.PatternIndex, test))
                            return false;
                        if (!LayoutBrowseNode(controller, BlockState.SourceIndex, test))
                            return false;

                        for (int i = 0; i < BlockState.StateList.Count; i++)
                        {
                            ILayoutPlaceholderNodeState ChildState = BlockState.StateList[i];
                            ILayoutIndex ChildIndex = ChildState.ParentIndex;
                            if (!LayoutBrowseNode(controller, ChildIndex, test))
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        static void MoveLayoutRandomly(ILayoutControllerView controllerView)
        {
            int MinFocusMove = controllerView.MinFocusMove;
            int MaxFocusMove = controllerView.MaxFocusMove;

            int Direction = MinFocusMove + RandNext(MaxFocusMove - MinFocusMove + 1);
            controllerView.MoveFocus(Direction, true, out bool IsMoved);
        }
        #endregion
    }
}
