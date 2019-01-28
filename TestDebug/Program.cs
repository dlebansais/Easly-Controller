using BaseNode;
using BaseNodeHelper;
using EaslyController;
using EaslyController.Focus;
using EaslyController.Frame;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using PolySerializer;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace TestDebug
{
    class Program
    {
        #region Init
        static void Main(string[] args)
        {
            string CurrentDirectory = Environment.CurrentDirectory;
            if (CurrentDirectory.Contains("Debug") || CurrentDirectory.Contains("Release"))
                CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);
            if (CurrentDirectory.Contains("x64"))
                CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);
            if (CurrentDirectory.Contains("bin"))
                CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);

            CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);
            CurrentDirectory = Path.Combine(CurrentDirectory, "Test");

            Serializer Serializer = new Serializer();
            Test(Serializer, CurrentDirectory);
        }

        static void Test(Serializer Serializer, string Folder)
        {
            foreach (string Subfolder in Directory.GetDirectories(Folder))
                Test(Serializer, Subfolder);

            foreach (string FileName in Directory.GetFiles(Folder, "*.easly"))
            {
                Console.WriteLine(FileName);

                //TestReadOnly(Serializer, FileName);
                TestWriteable(Serializer, FileName);
                //TestFrame(Serializer, FileName);
                //TestFocus(Serializer, FileName);
            }
        }
        #endregion

        #region Tools
        static bool DebugLine = false;

        static byte[] GetData(INode node)
        {
            byte[] Data;
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer Serializer = new Serializer();
                Serializer.Format = SerializationFormat.TextOnly;

                Serializer.Serialize(ms, node);

                ms.Seek(0, SeekOrigin.Begin);
                Data = new byte[ms.Length];
                ms.Read(Data, 0, Data.Length);

                ms.Seek(0, SeekOrigin.Begin);

                using (StreamReader sr = new StreamReader(ms))
                {
                    for (; ; )
                    {
                        string s = sr.ReadLine();
                        if (s == null)
                            break;

                        if (DebugLine)
                            Debug.WriteLine(s);
                    }
                }
            }

            return Data;
        }

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int memcmp(byte[] b1, byte[] b2, long count);

        static bool ByteArrayCompare(byte[] b1, byte[] b2)
        {
            // Validate buffers are the same length.
            // This also ensures that the count does not exceed the length of either buffer.  
            return b1.Length == b2.Length && memcmp(b1, b2, b1.Length) == 0;
        }
        #endregion

        #region ReadOnly
        static void TestReadOnly(Serializer serializer, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                INode RootNode = serializer.Deserialize(fs) as INode;
                INode ClonedNode = NodeHelper.DeepCloneNode(RootNode);
                Debug.Assert(NodeHelper.NodeHash(RootNode) == NodeHelper.NodeHash(ClonedNode));

                TestReadOnly(RootNode);
            }
        }

        static void TestReadOnly(INode rootNode)
        {
            ControllerTools.ResetExpectedName();

            IReadOnlyRootNodeIndex RootIndex = new ReadOnlyRootNodeIndex(rootNode);
            IReadOnlyController Controller = ReadOnlyController.Create(RootIndex);
            Stats Stats = Controller.Stats;

            ulong h0 = NodeHelper.NodeHash(rootNode);
            byte[] RootData = GetData(rootNode);

            INode RootNodeClone1 = Controller.RootState.CloneNode();
            ulong h1 = NodeHelper.NodeHash(RootNodeClone1);
            byte[] RootCloneData1 = GetData(RootNodeClone1);

            Debug.Assert(ByteArrayCompare(RootData, RootCloneData1));
            Debug.Assert(h0 == h1);

            using (IReadOnlyControllerView ControllerView1 = ReadOnlyControllerView.Create(Controller))
            {
                using (IReadOnlyControllerView ControllerView2 = ReadOnlyControllerView.Create(Controller))
                {
                    INode RootNodeClone2 = Controller.RootState.CloneNode();
                    ulong h2 = NodeHelper.NodeHash(RootNodeClone2);
                    byte[] RootCloneData2 = GetData(RootNodeClone2);

                    Debug.Assert(ByteArrayCompare(RootData, RootCloneData2));
                    Debug.Assert(h1 == h2);

                    Debug.Assert(ControllerView1.IsEqual(CompareEqual.New(), ControllerView2));

                    IReadOnlyRootNodeIndex RootIndex2 = new ReadOnlyRootNodeIndex(rootNode);
                    IReadOnlyController Controller2 = ReadOnlyController.Create(RootIndex);
                    Debug.Assert(Controller.IsEqual(CompareEqual.New(), Controller2));
                }
            }
        }
        #endregion

        #region Writeable
        static void TestWriteable(Serializer serializer, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                INode RootNode = serializer.Deserialize(fs) as INode;
                INode ClonedNode = NodeHelper.DeepCloneNode(RootNode);
                ulong Hash1 = NodeHelper.NodeHash(RootNode);
                ulong Hash2 = NodeHelper.NodeHash(ClonedNode);
                Debug.Assert(Hash1 == Hash2);

                TestWriteable(RootNode);
            }
        }

        static void TestWriteableGR(IGlobalReplicate rootNode)
        {
            ControllerTools.ResetExpectedName();

            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            Stats Stats = Controller.Stats;
            IWriteableController ControllerCheck;

            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            IWriteableNodeState RootState = Controller.RootState;
            IWriteableInnerReadOnlyDictionary<string> InnerTable = RootState.InnerTable;

            IWriteableListInner<IWriteableBrowsingListNodeIndex> ListInner2 = (IWriteableListInner<IWriteableBrowsingListNodeIndex>)InnerTable[nameof(IGlobalReplicate.Patterns)];
            if (ListInner2.StateList.Count > 30)
            {
                IPattern TestNode = ListInner2.StateList[31].Node as IPattern;

                IWriteableBrowsingListNodeIndex InsertIndex0 = (IWriteableBrowsingListNodeIndex)ListInner2.IndexAt(31);
                Controller.Move(ListInner2, InsertIndex0, -5);

                ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));
            }

        }

        static void TestWriteable(INode rootNode)
        {
            if (!(rootNode is IClass))
            {
                if (!(rootNode is IGlobalReplicate))
                    return;

                TestWriteableGR(rootNode as IGlobalReplicate);
                return;
            }

            ControllerTools.ResetExpectedName();

            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            Stats Stats = Controller.Stats;
            IWriteableController ControllerCheck;

            INode RootNodeClone = Controller.RootState.CloneNode();
            ulong h1 = NodeHelper.NodeHash(rootNode);
            ulong h2 = NodeHelper.NodeHash(RootNodeClone);

            byte[] RootData = GetData(rootNode);
            byte[] RootCloneData = GetData(RootNodeClone);

            bool IsEqual = ByteArrayCompare(RootData, RootCloneData);
            Debug.Assert(IsEqual);
            Debug.Assert(h1 == h2);

            IWriteableControllerView ControllerView = WriteableControllerView.Create(Controller);

            IWriteableNodeState RootState = Controller.RootState;
            IWriteableInnerReadOnlyDictionary<string> InnerTable = RootState.InnerTable;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> ListInner = (IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>)InnerTable[nameof(IClass.ImportBlocks)];

            IPattern PatternNode = NodeHelper.CreateEmptyPattern();
            IIdentifier SourceNode = NodeHelper.CreateEmptyIdentifier();
            IImport FirstNode = NodeHelper.CreateSimpleImport("x", "x", ImportType.Latest);

            WriteableInsertionNewBlockNodeIndex InsertIndex0 = new WriteableInsertionNewBlockNodeIndex(rootNode, ListInner.PropertyName, FirstNode, 0, PatternNode, SourceNode);
            Controller.Insert(ListInner, InsertIndex0, out IWriteableBrowsingCollectionNodeIndex InsertedIndex0);

            ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IImport SecondNode = NodeHelper.CreateSimpleImport("y", "y", ImportType.Latest);

            WriteableInsertionExistingBlockNodeIndex InsertIndex1 = new WriteableInsertionExistingBlockNodeIndex(rootNode, ListInner.PropertyName, SecondNode, 0, 1);
            Controller.Insert(ListInner, InsertIndex1, out IWriteableBrowsingCollectionNodeIndex InsertedIndex1);

            ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            Debug.Assert(ControllerView.StateViewTable.Count == Controller.Stats.NodeCount);

            IWriteableControllerView ControllerView2 = WriteableControllerView.Create(Controller);
            Debug.Assert(ControllerView2.IsEqual(CompareEqual.New(), ControllerView));

            Controller.ChangeReplication(ListInner, 0, ReplicationStatus.Replicated);

            ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IImport ThirdNode = NodeHelper.CreateSimpleImport("z", "z", ImportType.Latest);

            WriteableInsertionExistingBlockNodeIndex InsertIndex3 = new WriteableInsertionExistingBlockNodeIndex(rootNode, ListInner.PropertyName, ThirdNode, 0, 1);
            Controller.Replace(ListInner, InsertIndex3, out IWriteableBrowsingChildIndex InsertedIndex3);

            ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IImport FourthNode = NodeHelper.CreateSimpleImport("a", "a", ImportType.Latest);

            WriteableInsertionExistingBlockNodeIndex InsertIndex4 = new WriteableInsertionExistingBlockNodeIndex(rootNode, ListInner.PropertyName, FourthNode, 0, 0);
            Controller.Replace(ListInner, InsertIndex4, out IWriteableBrowsingChildIndex InsertedIndex4);

            ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IWriteableControllerView ControllerView3 = WriteableControllerView.Create(Controller);
            Debug.Assert(ControllerView3.IsEqual(CompareEqual.New(), ControllerView));

            IName FifthNode = NodeHelper.CreateSimpleName("a");

            IWriteableSingleInner<IWriteableBrowsingChildIndex> ChildInner = (IWriteableSingleInner<IWriteableBrowsingChildIndex>)InnerTable[nameof(IClass.EntityName)];
            WriteableInsertionPlaceholderNodeIndex InsertIndex5 = new WriteableInsertionPlaceholderNodeIndex(rootNode, ChildInner.PropertyName, FifthNode);
            Controller.Replace(ChildInner, InsertIndex5, out IWriteableBrowsingChildIndex InsertedIndex5);

            ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IWriteableControllerView ControllerView4 = WriteableControllerView.Create(Controller);
            Debug.Assert(ControllerView4.IsEqual(CompareEqual.New(), ControllerView));

            IIdentifier SixthNode = NodeHelper.CreateSimpleIdentifier("b");

            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> OptionalInner = (IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>)InnerTable[nameof(IClass.FromIdentifier)];
            WriteableInsertionOptionalNodeIndex InsertIndex6 = new WriteableInsertionOptionalNodeIndex(rootNode, OptionalInner.PropertyName, SixthNode);
            Controller.Replace(OptionalInner, InsertIndex6, out IWriteableBrowsingChildIndex InsertedIndex6);

            ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IWriteableControllerView ControllerView5 = WriteableControllerView.Create(Controller);
            Debug.Assert(ControllerView5.IsEqual(CompareEqual.New(), ControllerView));

            IWriteableBrowsingBlockNodeIndex InsertIndex7 = (IWriteableBrowsingBlockNodeIndex)ListInner.IndexAt(0, 0);
            Controller.Remove(ListInner, InsertIndex7);

            ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IWriteableControllerView ControllerView7 = WriteableControllerView.Create(Controller);
            Debug.Assert(ControllerView7.IsEqual(CompareEqual.New(), ControllerView));

            IWriteableBrowsingBlockNodeIndex InsertIndex8 = (IWriteableBrowsingBlockNodeIndex)ListInner.IndexAt(0, 0);
            Controller.Remove(ListInner, InsertIndex8);

            ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IWriteableControllerView ControllerView8 = WriteableControllerView.Create(Controller);
            Debug.Assert(ControllerView8.IsEqual(CompareEqual.New(), ControllerView));

            Controller.Unassign(OptionalInner.ChildState.ParentIndex);

            ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IWriteableControllerView ControllerView9 = WriteableControllerView.Create(Controller);
            Debug.Assert(ControllerView9.IsEqual(CompareEqual.New(), ControllerView));

            Controller.Assign(OptionalInner.ChildState.ParentIndex);

            ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IWriteableControllerView ControllerView10 = WriteableControllerView.Create(Controller);
            Debug.Assert(ControllerView10.IsEqual(CompareEqual.New(), ControllerView));

            if (ListInner.BlockStateList.Count >= 2)
            {
                IWriteableBrowsingExistingBlockNodeIndex SplitIndex1 = (IWriteableBrowsingExistingBlockNodeIndex)ListInner.IndexAt(0, 1);
                Controller.SplitBlock(ListInner, SplitIndex1);

                ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

                IWriteableControllerView ControllerView11 = WriteableControllerView.Create(Controller);
                Debug.Assert(ControllerView11.IsEqual(CompareEqual.New(), ControllerView));

                IWriteableBrowsingExistingBlockNodeIndex SplitIndex2 = (IWriteableBrowsingExistingBlockNodeIndex)ListInner.IndexAt(1, 0);
                Controller.MergeBlocks(ListInner, SplitIndex2);

                ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

                IWriteableControllerView ControllerView12 = WriteableControllerView.Create(Controller);
                Debug.Assert(ControllerView12.IsEqual(CompareEqual.New(), ControllerView));
            }

            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> ListInner2 = (IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>)InnerTable[nameof(IClass.FeatureBlocks)];
            if (ListInner2.BlockStateList.Count > 1)
            {
                Controller.MoveBlock(ListInner2, 0, 1);

                ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));
            }

            Controller.Canonicalize();

            ControllerCheck = WriteableController.Create(new WriteableRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));
        }
        #endregion

        #region Frame
        static void TestFrame(Serializer serializer, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                INode RootNode = serializer.Deserialize(fs) as INode;
                INode ClonedNode = NodeHelper.DeepCloneNode(RootNode);
                Debug.Assert(NodeHelper.NodeHash(RootNode) == NodeHelper.NodeHash(ClonedNode));

                TestFrame(RootNode);
            }
        }

        static void TestFrameGR(IGlobalReplicate rootNode)
        {
            ControllerTools.ResetExpectedName();

            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            Stats Stats = Controller.Stats;
            IFrameController ControllerCheck;

            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            IFrameNodeState RootState = Controller.RootState;
            IFrameInnerReadOnlyDictionary<string> InnerTable = RootState.InnerTable;

            IFrameListInner<IFrameBrowsingListNodeIndex> ListInner2 = (IFrameListInner<IFrameBrowsingListNodeIndex>)InnerTable[nameof(IGlobalReplicate.Patterns)];
            if (ListInner2.StateList.Count > 30)
            {
                IPattern TestNode = ListInner2.StateList[31].Node as IPattern;

                IFrameBrowsingListNodeIndex InsertIndex0 = (IFrameBrowsingListNodeIndex)ListInner2.IndexAt(31);
                Controller.Move(ListInner2, InsertIndex0, -5);

                ControllerCheck = FrameController.Create(new FrameRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));
            }

        }

        static void TestFrame(INode rootNode)
        {
            if (!(rootNode is IClass))
            {
                if (!(rootNode is IGlobalReplicate))
                    return;

                TestFrameGR(rootNode as IGlobalReplicate);
                return;
            }

            ControllerTools.ResetExpectedName();

            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView CustomView = FrameControllerView.Create(Controller, CustomFrameTemplateSet.FrameTemplateSet);
            Stats Stats = Controller.Stats;
            IFrameController ControllerCheck;

            INode RootNodeClone = Controller.RootState.CloneNode();
            ulong h1 = NodeHelper.NodeHash(rootNode);
            ulong h2 = NodeHelper.NodeHash(RootNodeClone);

            byte[] RootData = GetData(rootNode);
            byte[] RootCloneData = GetData(RootNodeClone);

            bool IsEqual = ByteArrayCompare(RootData, RootCloneData);
            Debug.Assert(IsEqual);
            Debug.Assert(h1 == h2);

            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            IFrameNodeState RootState = Controller.RootState;
            IFrameInnerReadOnlyDictionary<string> InnerTable = RootState.InnerTable;
            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> ListInner = (IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)InnerTable[nameof(IClass.ImportBlocks)];

            IPattern PatternNode = NodeHelper.CreateEmptyPattern();
            IIdentifier SourceNode = NodeHelper.CreateEmptyIdentifier();
            IImport FirstNode = NodeHelper.CreateSimpleImport("x", "x", ImportType.Latest);

            FrameInsertionNewBlockNodeIndex InsertIndex0 = new FrameInsertionNewBlockNodeIndex(rootNode, ListInner.PropertyName, FirstNode, 0, PatternNode, SourceNode);
            Controller.Insert(ListInner, InsertIndex0, out IWriteableBrowsingCollectionNodeIndex InsertedIndex0);

            IFrameControllerView ControllerView2 = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
            Debug.Assert(ControllerView2.IsEqual(CompareEqual.New(), ControllerView));

            ControllerCheck = FrameController.Create(new FrameRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IImport SecondNode = NodeHelper.CreateSimpleImport("y", "y", ImportType.Latest);

            FrameInsertionExistingBlockNodeIndex InsertIndex1 = new FrameInsertionExistingBlockNodeIndex(rootNode, ListInner.PropertyName, SecondNode, 0, 1);
            Controller.Insert(ListInner, InsertIndex1, out IWriteableBrowsingCollectionNodeIndex InsertedIndex1);

            ControllerCheck = FrameController.Create(new FrameRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            Debug.Assert(ControllerView.StateViewTable.Count == Controller.Stats.NodeCount);

            ControllerView2 = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
            Debug.Assert(ControllerView2.IsEqual(CompareEqual.New(), ControllerView));

            Controller.ChangeReplication(ListInner, 0, ReplicationStatus.Replicated);

            ControllerCheck = FrameController.Create(new FrameRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IImport ThirdNode = NodeHelper.CreateSimpleImport("z", "z", ImportType.Latest);

            FrameInsertionExistingBlockNodeIndex InsertIndex3 = new FrameInsertionExistingBlockNodeIndex(rootNode, ListInner.PropertyName, ThirdNode, 0, 1);
            Controller.Replace(ListInner, InsertIndex3, out IWriteableBrowsingChildIndex InsertedIndex3);

            ControllerCheck = FrameController.Create(new FrameRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IImport FourthNode = NodeHelper.CreateSimpleImport("a", "a", ImportType.Latest);

            FrameInsertionExistingBlockNodeIndex InsertIndex4 = new FrameInsertionExistingBlockNodeIndex(rootNode, ListInner.PropertyName, FourthNode, 0, 0);
            Controller.Replace(ListInner, InsertIndex4, out IWriteableBrowsingChildIndex InsertedIndex4);

            ControllerCheck = FrameController.Create(new FrameRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IFrameControllerView ControllerView3 = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
            Debug.Assert(ControllerView3.IsEqual(CompareEqual.New(), ControllerView));

            IName FifthNode = NodeHelper.CreateSimpleName("a");

            IFrameSingleInner<IFrameBrowsingChildIndex> ChildInner = (IFrameSingleInner<IFrameBrowsingChildIndex>)InnerTable[nameof(IClass.EntityName)];
            FrameInsertionPlaceholderNodeIndex InsertIndex5 = new FrameInsertionPlaceholderNodeIndex(rootNode, ChildInner.PropertyName, FifthNode);
            Controller.Replace(ChildInner, InsertIndex5, out IWriteableBrowsingChildIndex InsertedIndex5);

            ControllerCheck = FrameController.Create(new FrameRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IFrameControllerView ControllerView4 = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
            Debug.Assert(ControllerView4.IsEqual(CompareEqual.New(), ControllerView));

            IIdentifier SixthNode = NodeHelper.CreateSimpleIdentifier("b");

            IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex> OptionalInner = (IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex>)InnerTable[nameof(IClass.FromIdentifier)];
            FrameInsertionOptionalNodeIndex InsertIndex6 = new FrameInsertionOptionalNodeIndex(rootNode, OptionalInner.PropertyName, SixthNode);
            Controller.Replace(OptionalInner, InsertIndex6, out IWriteableBrowsingChildIndex InsertedIndex6);

            ControllerCheck = FrameController.Create(new FrameRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IFrameControllerView ControllerView5 = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
            Debug.Assert(ControllerView5.IsEqual(CompareEqual.New(), ControllerView));

            bool TestRemove = true;
            if (TestRemove)
            {
                IFrameBrowsingBlockNodeIndex InsertIndex7 = (IFrameBrowsingBlockNodeIndex)ListInner.IndexAt(0, 0);
                Controller.Remove(ListInner, InsertIndex7);

                ControllerCheck = FrameController.Create(new FrameRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

                IFrameControllerView ControllerView7 = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Debug.Assert(ControllerView7.IsEqual(CompareEqual.New(), ControllerView));

                IFrameBrowsingBlockNodeIndex InsertIndex8 = (IFrameBrowsingBlockNodeIndex)ListInner.IndexAt(0, 0);
                Controller.Remove(ListInner, InsertIndex8);

                ControllerCheck = FrameController.Create(new FrameRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

                IFrameControllerView ControllerView8 = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Debug.Assert(ControllerView8.IsEqual(CompareEqual.New(), ControllerView));
            }

            Controller.Unassign(OptionalInner.ChildState.ParentIndex);

            ControllerCheck = FrameController.Create(new FrameRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IFrameControllerView ControllerView9 = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
            Debug.Assert(ControllerView9.IsEqual(CompareEqual.New(), ControllerView));

            Controller.Assign(OptionalInner.ChildState.ParentIndex);

            ControllerCheck = FrameController.Create(new FrameRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IFrameControllerView ControllerView10 = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
            Debug.Assert(ControllerView10.IsEqual(CompareEqual.New(), ControllerView));

            if (ListInner.BlockStateList.Count >= 2)
            {
                IFrameBrowsingExistingBlockNodeIndex SplitIndex1 = (IFrameBrowsingExistingBlockNodeIndex)ListInner.IndexAt(0, 1);
                Controller.SplitBlock(ListInner, SplitIndex1);

                ControllerCheck = FrameController.Create(new FrameRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

                IFrameControllerView ControllerView11 = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Debug.Assert(ControllerView11.IsEqual(CompareEqual.New(), ControllerView));
                
                IFrameBrowsingExistingBlockNodeIndex SplitIndex2 = (IFrameBrowsingExistingBlockNodeIndex)ListInner.IndexAt(1, 0);
                Controller.MergeBlocks(ListInner, SplitIndex2);

                ControllerCheck = FrameController.Create(new FrameRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

                IFrameControllerView ControllerView12 = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Debug.Assert(ControllerView12.IsEqual(CompareEqual.New(), ControllerView));
            }

            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> ListInner2 = (IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)InnerTable[nameof(IClass.FeatureBlocks)];
            if (ListInner.BlockStateList.Count > 1)
            {
                Controller.MoveBlock(ListInner, 0, 1);

                ControllerCheck = FrameController.Create(new FrameRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));
            }

            Controller.Expand(Controller.RootIndex);
            Controller.Reduce(Controller.RootIndex);
            Controller.Expand(Controller.RootIndex);
            Controller.Canonicalize();

            IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            IFrameController NewController = FrameController.Create(NewRootIndex);
            Debug.Assert(NewController.IsEqual(CompareEqual.New(), Controller));
        }
        #endregion

        #region Focus
        static void TestFocus(Serializer serializer, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                INode RootNode = serializer.Deserialize(fs) as INode;
                INode ClonedNode = NodeHelper.DeepCloneNode(RootNode);
                Debug.Assert(NodeHelper.NodeHash(RootNode) == NodeHelper.NodeHash(ClonedNode));

                TestFocus(RootNode);
            }
        }

        static void TestFocusGR(IGlobalReplicate rootNode)
        {
            ControllerTools.ResetExpectedName();

            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            Stats Stats = Controller.Stats;
            IFocusController ControllerCheck;

            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);

            IFocusNodeState RootState = Controller.RootState;
            IFocusInnerReadOnlyDictionary<string> InnerTable = RootState.InnerTable;

            IFocusListInner<IFocusBrowsingListNodeIndex> ListInner2 = (IFocusListInner<IFocusBrowsingListNodeIndex>)InnerTable[nameof(IGlobalReplicate.Patterns)];
            if (ListInner2.StateList.Count > 30)
            {
                IPattern TestNode = ListInner2.StateList[31].Node as IPattern;

                IFocusBrowsingListNodeIndex InsertIndex0 = (IFocusBrowsingListNodeIndex)ListInner2.IndexAt(31);
                Controller.Move(ListInner2, InsertIndex0, -5);

                ControllerCheck = FocusController.Create(new FocusRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));
            }

        }

        static void TestFocus(INode rootNode)
        {
            if (!(rootNode is IClass))
            {
                if (!(rootNode is IGlobalReplicate))
                    return;

                TestFocusGR(rootNode as IGlobalReplicate);
                return;
            }

            ControllerTools.ResetExpectedName();

            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            IFocusControllerView CustomView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);
            Stats Stats = Controller.Stats;
            IFocusController ControllerCheck;

            INode RootNodeClone = Controller.RootState.CloneNode();
            ulong h1 = NodeHelper.NodeHash(rootNode);
            ulong h2 = NodeHelper.NodeHash(RootNodeClone);

            byte[] RootData = GetData(rootNode);
            byte[] RootCloneData = GetData(RootNodeClone);

            bool IsEqual = ByteArrayCompare(RootData, RootCloneData);
            Debug.Assert(IsEqual);
            Debug.Assert(h1 == h2);

            IFocusControllerView ControllerView = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
            //Debug.WriteLine(ControllerView.LastColumnNumber.ToString());

            IFocusVisibleCellViewList CellList = new FocusVisibleCellViewList();
            ControllerView.EnumerateVisibleCellViews(CellList);
            Debug.Assert(CellList.Count > 0);

            IFocusNodeState RootState = Controller.RootState;
            IFocusInnerReadOnlyDictionary<string> InnerTable = RootState.InnerTable;
            IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> ListInner = (IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>)InnerTable[nameof(IClass.ImportBlocks)];

            IPattern PatternNode = NodeHelper.CreateEmptyPattern();
            IIdentifier SourceNode = NodeHelper.CreateEmptyIdentifier();
            IImport FirstNode = NodeHelper.CreateSimpleImport("x", "x", ImportType.Latest);

            FocusInsertionNewBlockNodeIndex InsertIndex0 = new FocusInsertionNewBlockNodeIndex(rootNode, ListInner.PropertyName, FirstNode, 0, PatternNode, SourceNode);
            Controller.Insert(ListInner, InsertIndex0, out IWriteableBrowsingCollectionNodeIndex InsertedIndex0);

            IFocusControllerView ControllerView2 = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
            Debug.Assert(ControllerView2.IsEqual(CompareEqual.New(), ControllerView));

            ControllerCheck = FocusController.Create(new FocusRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IImport SecondNode = NodeHelper.CreateSimpleImport("y", "y", ImportType.Latest);

            FocusInsertionExistingBlockNodeIndex InsertIndex1 = new FocusInsertionExistingBlockNodeIndex(rootNode, ListInner.PropertyName, SecondNode, 0, 1);
            Controller.Insert(ListInner, InsertIndex1, out IWriteableBrowsingCollectionNodeIndex InsertedIndex1);

            ControllerCheck = FocusController.Create(new FocusRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            Debug.Assert(ControllerView.StateViewTable.Count == Controller.Stats.NodeCount);

            ControllerView2 = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
            Debug.Assert(ControllerView2.IsEqual(CompareEqual.New(), ControllerView));

            Controller.ChangeReplication(ListInner, 0, ReplicationStatus.Replicated);

            ControllerCheck = FocusController.Create(new FocusRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IImport ThirdNode = NodeHelper.CreateSimpleImport("z", "z", ImportType.Latest);

            FocusInsertionExistingBlockNodeIndex InsertIndex3 = new FocusInsertionExistingBlockNodeIndex(rootNode, ListInner.PropertyName, ThirdNode, 0, 1);
            Controller.Replace(ListInner, InsertIndex3, out IWriteableBrowsingChildIndex InsertedIndex3);

            ControllerCheck = FocusController.Create(new FocusRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IImport FourthNode = NodeHelper.CreateSimpleImport("a", "a", ImportType.Latest);

            FocusInsertionExistingBlockNodeIndex InsertIndex4 = new FocusInsertionExistingBlockNodeIndex(rootNode, ListInner.PropertyName, FourthNode, 0, 0);
            Controller.Replace(ListInner, InsertIndex4, out IWriteableBrowsingChildIndex InsertedIndex4);

            ControllerCheck = FocusController.Create(new FocusRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IFocusControllerView ControllerView3 = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
            Debug.Assert(ControllerView3.IsEqual(CompareEqual.New(), ControllerView));

            IName FifthNode = NodeHelper.CreateSimpleName("a");

            IFocusSingleInner<IFocusBrowsingChildIndex> ChildInner = (IFocusSingleInner<IFocusBrowsingChildIndex>)InnerTable[nameof(IClass.EntityName)];
            FocusInsertionPlaceholderNodeIndex InsertIndex5 = new FocusInsertionPlaceholderNodeIndex(rootNode, ChildInner.PropertyName, FifthNode);
            Controller.Replace(ChildInner, InsertIndex5, out IWriteableBrowsingChildIndex InsertedIndex5);

            ControllerCheck = FocusController.Create(new FocusRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IFocusControllerView ControllerView4 = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
            Debug.Assert(ControllerView4.IsEqual(CompareEqual.New(), ControllerView));

            IIdentifier SixthNode = NodeHelper.CreateSimpleIdentifier("b");

            IFocusOptionalInner<IFocusBrowsingOptionalNodeIndex> OptionalInner = (IFocusOptionalInner<IFocusBrowsingOptionalNodeIndex>)InnerTable[nameof(IClass.FromIdentifier)];
            FocusInsertionOptionalNodeIndex InsertIndex6 = new FocusInsertionOptionalNodeIndex(rootNode, OptionalInner.PropertyName, SixthNode);
            Controller.Replace(OptionalInner, InsertIndex6, out IWriteableBrowsingChildIndex InsertedIndex6);

            ControllerCheck = FocusController.Create(new FocusRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IFocusControllerView ControllerView5 = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
            Debug.Assert(ControllerView5.IsEqual(CompareEqual.New(), ControllerView));

            bool TestRemove = true;
            if (TestRemove)
            {
                IFocusBrowsingBlockNodeIndex InsertIndex7 = (IFocusBrowsingBlockNodeIndex)ListInner.IndexAt(0, 0);
                Debug.Assert(Controller.IsRemoveable(ListInner, InsertIndex7));
                Controller.Remove(ListInner, InsertIndex7);

                ControllerCheck = FocusController.Create(new FocusRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

                IFocusControllerView ControllerView7 = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Debug.Assert(ControllerView7.IsEqual(CompareEqual.New(), ControllerView));

                IFocusBrowsingBlockNodeIndex InsertIndex8 = (IFocusBrowsingBlockNodeIndex)ListInner.IndexAt(0, 0);
                Debug.Assert(Controller.IsRemoveable(ListInner, InsertIndex8));
                Controller.Remove(ListInner, InsertIndex8);

                ControllerCheck = FocusController.Create(new FocusRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

                IFocusControllerView ControllerView8 = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Debug.Assert(ControllerView8.IsEqual(CompareEqual.New(), ControllerView));
            }

            Controller.Unassign(OptionalInner.ChildState.ParentIndex);

            ControllerCheck = FocusController.Create(new FocusRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IFocusControllerView ControllerView9 = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
            Debug.Assert(ControllerView9.IsEqual(CompareEqual.New(), ControllerView));

            Controller.Assign(OptionalInner.ChildState.ParentIndex);

            ControllerCheck = FocusController.Create(new FocusRootNodeIndex(rootNode));
            Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

            IFocusControllerView ControllerView10 = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
            Debug.Assert(ControllerView10.IsEqual(CompareEqual.New(), ControllerView));

            if (ListInner.BlockStateList.Count >= 2)
            {
                IFocusBrowsingExistingBlockNodeIndex SplitIndex1 = (IFocusBrowsingExistingBlockNodeIndex)ListInner.IndexAt(0, 1);
                Controller.SplitBlock(ListInner, SplitIndex1);

                ControllerCheck = FocusController.Create(new FocusRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

                IFocusControllerView ControllerView11 = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Debug.Assert(ControllerView11.IsEqual(CompareEqual.New(), ControllerView));

                IFocusBrowsingExistingBlockNodeIndex SplitIndex2 = (IFocusBrowsingExistingBlockNodeIndex)ListInner.IndexAt(1, 0);
                Controller.MergeBlocks(ListInner, SplitIndex2);

                ControllerCheck = FocusController.Create(new FocusRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));

                IFocusControllerView ControllerView12 = FocusControllerView.Create(Controller, FocusTemplateSet.Default);
                Debug.Assert(ControllerView12.IsEqual(CompareEqual.New(), ControllerView));
            }

            IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> ListInner2 = (IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>)InnerTable[nameof(IClass.FeatureBlocks)];
            if (ListInner.BlockStateList.Count > 1)
            {
                Controller.MoveBlock(ListInner, 0, 1);

                ControllerCheck = FocusController.Create(new FocusRootNodeIndex(rootNode));
                Debug.Assert(ControllerCheck.IsEqual(CompareEqual.New(), Controller));
            }

            Controller.Expand(Controller.RootIndex);
            Controller.Reduce(Controller.RootIndex);
            Controller.Expand(Controller.RootIndex);
            Controller.Canonicalize();

            IFocusRootNodeIndex NewRootIndex = new FocusRootNodeIndex(Controller.RootIndex.Node);
            IFocusController NewController = FocusController.Create(NewRootIndex);
            Debug.Assert(NewController.IsEqual(CompareEqual.New(), Controller));
        }
        #endregion
    }
}
