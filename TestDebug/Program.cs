using BaseNode;
using BaseNodeHelper;
using EaslyController;
using EaslyController.Frame;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using PolySerializer;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Markup;

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

            CustomNodeTemplates = LoadTemplate(FrameTemplateListString);
            CustomBlockTemplates = LoadTemplate(FrameBlockTemplateString);
            Test(Serializer, CurrentDirectory);
        }

        static IFrameTemplateReadOnlyDictionary CustomNodeTemplates;
        static IFrameTemplateReadOnlyDictionary CustomBlockTemplates;

        static void Test(Serializer Serializer, string Folder)
        {
            foreach (string Subfolder in Directory.GetDirectories(Folder))
                Test(Serializer, Subfolder);

            foreach (string FileName in Directory.GetFiles(Folder, "*.easly"))
            {
                Console.WriteLine(FileName);

                //TestReadOnly(Serializer, FileName);
                //TestWriteable(Serializer, FileName);
                TestFrame(Serializer, FileName);
            }
        }

        static IFrameTemplateReadOnlyDictionary LoadTemplate(string s)
        {
            byte[] ByteArray = Encoding.UTF8.GetBytes(s);
            using (MemoryStream ms = new MemoryStream(ByteArray))
            {
                IFrameTemplateList Templates = XamlReader.Parse(s) as IFrameTemplateList;

                FrameTemplateDictionary TemplateDictionary = new FrameTemplateDictionary();
                foreach (IFrameTemplate Item in Templates)
                {
                    Item.Root.UpdateParent(Item, FrameFrame.Root);
                    TemplateDictionary.Add(Item.NodeType, Item);
                }

                IFrameTemplateReadOnlyDictionary Result = new FrameTemplateReadOnlyDictionary(TemplateDictionary);
                return Result;
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
                Debug.Assert(NodeHelper.NodeHash(RootNode) == NodeHelper.NodeHash(ClonedNode));

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
            IFrameTemplateSet CustomTemplateSet = new FrameTemplateSet(CustomNodeTemplates, CustomBlockTemplates);
            IFrameControllerView CustomView = FrameControllerView.Create(Controller, CustomTemplateSet);
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

        #region Templates
        static string FrameTemplateListString =
@"<FrameTemplateList
    xmlns=""clr-namespace:EaslyController.Frame;assembly=Easly-Controller""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:xaml=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller""
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <FrameNodeTemplate NodeType=""{xaml:Type IAssertion}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameOptionalFrame PropertyName=""Tag"" />
                <FrameKeywordFrame>:</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""BooleanExpression"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAttachment}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame Text=""else"">
                </FrameKeywordFrame>
                <FrameKeywordFrame>as</FrameKeywordFrame>
                <FrameHorizontalBlockListFrame PropertyName=""AttachTypeBlocks"" />
                <FrameInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IClass}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""CopySpecification"">
                    <FrameKeywordFrame>any</FrameKeywordFrame>
                    <FrameKeywordFrame>reference</FrameKeywordFrame>
                    <FrameKeywordFrame>value</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameDiscreteFrame PropertyName=""Cloneable"">
                    <FrameKeywordFrame>cloneable</FrameKeywordFrame>
                    <FrameKeywordFrame>single</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameDiscreteFrame PropertyName=""Comparable"">
                    <FrameKeywordFrame>comparable</FrameKeywordFrame>
                    <FrameKeywordFrame>incomparable</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameDiscreteFrame PropertyName=""IsAbstract"">
                    <FrameKeywordFrame>instanceable</FrameKeywordFrame>
                    <FrameKeywordFrame>abstract</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameKeywordFrame>class</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""EntityName""/>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>from</FrameKeywordFrame>
                    <FrameOptionalFrame PropertyName=""FromIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>import</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ImportBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ImportBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>generic</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""GenericBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""GenericBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ExportBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ExportBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>typedef</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""TypedefBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""TypedefBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>inheritance</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""InheritanceBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""InheritanceBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>discrete</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""DiscreteBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""DiscreteBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>replicate</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ClassReplicateBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ClassReplicateBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>feature</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""FeatureBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""FeatureBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>conversion</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ConversionBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ConversionBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>invariant</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""InvariantBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""InvariantBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame Text=""end"">
            </FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IClassReplicate}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ReplicateName"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalBlockListFrame PropertyName=""PatternBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ICommandOverload}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>parameter</FrameKeywordFrame>
                    <FrameDiscreteFrame PropertyName=""ParameterEnd"">
                        <FrameKeywordFrame>closed</FrameKeywordFrame>
                        <FrameKeywordFrame>open</FrameKeywordFrame>
                    </FrameDiscreteFrame>
                    <FrameInsertFrame CollectionName=""ParameterBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
            </FrameVerticalPanelFrame>
            <FramePlaceholderFrame PropertyName=""CommandBody"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ICommandOverloadType}"">
        <FrameHorizontalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>parameter</FrameKeywordFrame>
                        <FrameDiscreteFrame PropertyName=""ParameterEnd"">
                            <FrameKeywordFrame>closed</FrameKeywordFrame>
                            <FrameKeywordFrame>open</FrameKeywordFrame>
                        </FrameDiscreteFrame>
                        <FrameInsertFrame CollectionName=""ParameterBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>require</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""RequireBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>ensure</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""EnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>exception</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameKeywordFrame>end</FrameKeywordFrame>
            </FrameVerticalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IConditional}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame Text=""else"">
                </FrameKeywordFrame>
                <FrameKeywordFrame>if</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""BooleanExpression""/>
                <FrameKeywordFrame>then</FrameKeywordFrame>
                <FrameInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IConstraint}"">
        <FrameVerticalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ParentType"" />
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>rename</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""RenameBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""RenameBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IContinuation}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>execute</FrameKeywordFrame>
                <FrameInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FramePlaceholderFrame PropertyName=""Instructions"" />
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>cleanup</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""CleanupBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""CleanupBlocks"" />
                </FrameVerticalPanelFrame>
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IDiscrete}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>=</FrameKeywordFrame>
                <FrameOptionalFrame PropertyName=""NumericValue"" />
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IEntityDeclaration}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameKeywordFrame>:</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""EntityType"" />
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>=</FrameKeywordFrame>
                <FrameOptionalFrame PropertyName=""DefaultValue"" />
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IExceptionHandler}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>catch</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ExceptionIdentifier"" />
                <FrameInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IExport}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalBlockListFrame PropertyName=""ClassIdentifierBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IExportChange}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalBlockListFrame PropertyName=""IdentifierBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IGeneric}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>=</FrameKeywordFrame>
                    <FrameOptionalFrame PropertyName=""DefaultValue"" />
                </FrameHorizontalPanelFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>conform to</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ConstraintBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ConstraintBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame Text=""end"">
            </FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IGlobalReplicate}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ReplicateName"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FrameHorizontalListFrame PropertyName=""Patterns"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IImport}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Type"">
                    <FrameKeywordFrame>latest</FrameKeywordFrame>
                    <FrameKeywordFrame>strict</FrameKeywordFrame>
                    <FrameKeywordFrame>stable</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""LibraryIdentifier"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>from</FrameKeywordFrame>
                    <FrameOptionalFrame PropertyName=""FromIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>rename</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""RenameBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""RenameBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameKeywordFrame>end</FrameKeywordFrame>
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IInheritance}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Conformance"">
                    <FrameKeywordFrame>conformant</FrameKeywordFrame>
                    <FrameKeywordFrame>non-conformant</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""ParentType"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>rename</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""RenameBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""RenameBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>forget</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ForgetBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""ForgetBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>keep</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""KeepBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""KeepBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>discontinue</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""DiscontinueBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""DiscontinueBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>export</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ExportChangeBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""ExportChangeBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameDiscreteFrame PropertyName=""ForgetIndexer"">
                    <FrameKeywordFrame>ignore indexer</FrameKeywordFrame>
                    <FrameKeywordFrame>forget indexer</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameDiscreteFrame PropertyName=""KeepIndexer"">
                    <FrameKeywordFrame>ignore indexer</FrameKeywordFrame>
                    <FrameKeywordFrame>keep indexer</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameDiscreteFrame PropertyName=""DiscontinueIndexer"">
                    <FrameKeywordFrame>ignore indexer</FrameKeywordFrame>
                    <FrameKeywordFrame>discontinue indexer</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameKeywordFrame Text=""end"">
                </FrameKeywordFrame>
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ILibrary}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>library</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""EntityName""/>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>from</FrameKeywordFrame>
                    <FrameOptionalFrame PropertyName=""FromIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>import</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ImportBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ImportBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>class</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ClassIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ClassIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame Text=""end"">
            </FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IName}"">
        <FrameTextValueFrame PropertyName=""Text""/>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPattern}"">
        <FrameTextValueFrame PropertyName=""Text""/>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IQualifiedName}"">
        <FrameHorizontalListFrame PropertyName=""Path"" />
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IQueryOverload}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>parameter</FrameKeywordFrame>
                    <FrameDiscreteFrame PropertyName=""ParameterEnd"">
                        <FrameKeywordFrame>closed</FrameKeywordFrame>
                        <FrameKeywordFrame>open</FrameKeywordFrame>
                    </FrameDiscreteFrame>
                    <FrameInsertFrame CollectionName=""ParameterBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>result</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ResultBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ResultBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>modified</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ModifiedQueryBlocks"" />
            </FrameVerticalPanelFrame>
            <FramePlaceholderFrame PropertyName=""QueryBody"" />
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>variant</FrameKeywordFrame>
                <FrameOptionalFrame PropertyName=""Variant"" />
            </FrameHorizontalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IQueryOverloadType}"">
        <FrameHorizontalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>parameter</FrameKeywordFrame>
                        <FrameDiscreteFrame PropertyName=""ParameterEnd"">
                            <FrameKeywordFrame>closed</FrameKeywordFrame>
                            <FrameKeywordFrame>open</FrameKeywordFrame>
                        </FrameDiscreteFrame>
                        <FrameInsertFrame CollectionName=""ParameterBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>return</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ResultBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""ResultBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>require</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""RequireBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>ensure</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""EnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>exception</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameKeywordFrame>end</FrameKeywordFrame>
            </FrameVerticalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IRange}"">
        <FrameHorizontalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"">
            </FrameSymbolFrame>
            <FramePlaceholderFrame PropertyName=""LeftExpression"" />
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>to</FrameKeywordFrame>
                <FrameOptionalFrame PropertyName=""RightExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IRename}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""SourceIdentifier"" />
            <FrameKeywordFrame>to</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""DestinationIdentifier"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IRoot}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>libraries</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""LibraryBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""LibraryBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>classes</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ClassBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ClassBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>replicates</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""Replicates"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalListFrame PropertyName=""Replicates"" />
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IScope}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>local</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>do</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""InstructionBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""InstructionBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ITypedef}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameKeywordFrame>is</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""DefinedType"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAssignmentArgument}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalBlockListFrame PropertyName=""ParameterBlocks""/>
            <FramePlaceholderFrame PropertyName=""Source""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IWith}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>case</FrameKeywordFrame>
                <FrameHorizontalBlockListFrame PropertyName=""RangeBlocks""/>
                <FrameInsertFrame CollectionName=""RangeBlocks""/>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions""/>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IDeferredBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>require</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>throw</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>getter</FrameKeywordFrame>
                    <FrameKeywordFrame>deferred</FrameKeywordFrame>
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>ensure</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPositionalArgument}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Source""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IEffectiveBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>require</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>throw</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>local</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>getter</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""BodyInstructionBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""BodyInstructionBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>exception</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ExceptionHandlerBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ExceptionHandlerBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>ensure</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IExternBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>require</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>throw</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>getter</FrameKeywordFrame>
                    <FrameKeywordFrame>extern</FrameKeywordFrame>
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>ensure</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPrecursorBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>require</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>throw</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>getter</FrameKeywordFrame>
                    <FrameKeywordFrame>precursor</FrameKeywordFrame>
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>ensure</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAgentExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>agent</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FrameOptionalFrame PropertyName=""BaseType"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Delegated"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAssertionTagExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>tag</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""TagIdentifier"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IBinaryConditionalExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""LeftExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
            <FrameDiscreteFrame PropertyName=""Conditional"">
                <FrameKeywordFrame>and</FrameKeywordFrame>
                <FrameKeywordFrame>or</FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IBinaryOperatorExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""LeftExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Operator"" />
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IClassConstantExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.Dot}""/>
            <FramePlaceholderFrame PropertyName=""ConstantIdentifier"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ICloneOfExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameDiscreteFrame PropertyName=""Type"">
                <FrameKeywordFrame>shallow</FrameKeywordFrame>
                <FrameKeywordFrame>deep</FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FrameKeywordFrame>clone of</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""Source"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IEntityExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>entity</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""Query""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IEqualityExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""LeftExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
            <FrameDiscreteFrame PropertyName=""Comparison"">
                <FrameKeywordFrame>=</FrameKeywordFrame>
                <FrameKeywordFrame></FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FrameDiscreteFrame PropertyName=""Equality"">
                <FrameKeywordFrame>phys</FrameKeywordFrame>
                <FrameKeywordFrame>deep</FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FrameKeywordFrame Text="" ""/>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IIndexQueryExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""IndexedExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IInitializedObjectExpression}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameVerticalBlockListFrame PropertyName=""AssignmentBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FrameInsertFrame CollectionName=""AssignmentBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IKeywordExpression}"">
        <FrameDiscreteFrame PropertyName=""Value"">
            <FrameKeywordFrame>True</FrameKeywordFrame>
            <FrameKeywordFrame>False</FrameKeywordFrame>
            <FrameKeywordFrame>Current</FrameKeywordFrame>
            <FrameKeywordFrame>Value</FrameKeywordFrame>
            <FrameKeywordFrame>Result</FrameKeywordFrame>
            <FrameKeywordFrame>Retry</FrameKeywordFrame>
            <FrameKeywordFrame>Exception</FrameKeywordFrame>
        </FrameDiscreteFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IManifestCharacterExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>'</FrameKeywordFrame>
            <FrameCharacterFrame PropertyName=""Text""/>
            <FrameKeywordFrame>'</FrameKeywordFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IManifestNumberExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameNumberFrame PropertyName=""Text""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IManifestStringExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>""</FrameKeywordFrame>
            <FrameTextValueFrame PropertyName=""Text""/>
            <FrameKeywordFrame>""</FrameKeywordFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type INewExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>new</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""Object"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IOldExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>old</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""Query"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPrecursorExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>precursor</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FrameOptionalFrame PropertyName=""AncestorType"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPrecursorIndexExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>precursor</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FrameOptionalFrame PropertyName=""AncestorType"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
                <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPreprocessorExpression}"">
        <FrameDiscreteFrame PropertyName=""Value"">
            <FrameKeywordFrame>DateAndTime</FrameKeywordFrame>
            <FrameKeywordFrame>CompilationDiscreteIdentifier</FrameKeywordFrame>
            <FrameKeywordFrame>ClassPath</FrameKeywordFrame>
            <FrameKeywordFrame>CompilerVersion</FrameKeywordFrame>
            <FrameKeywordFrame>ConformanceToStandard</FrameKeywordFrame>
            <FrameKeywordFrame>DiscreteClassIdentifier</FrameKeywordFrame>
            <FrameKeywordFrame>Counter</FrameKeywordFrame>
            <FrameKeywordFrame>Debugging</FrameKeywordFrame>
            <FrameKeywordFrame>RandomInteger</FrameKeywordFrame>
        </FrameDiscreteFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IQueryExpression}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Query"" />
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IResultOfExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>result of</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""Source"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IUnaryNotExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>not</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IUnaryOperatorExpression}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Operator"" />
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAttributeFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Export"">
                    <FrameKeywordFrame>exported</FrameKeywordFrame>
                    <FrameKeywordFrame>private</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameKeywordFrame>:</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""EntityType""/>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>ensure</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""EnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export to</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame Text=""end"">
            </FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IConstantFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Export"">
                    <FrameKeywordFrame>exported</FrameKeywordFrame>
                    <FrameKeywordFrame>private</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameKeywordFrame>:</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""EntityType""/>
                <FrameKeywordFrame>=</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ConstantValue""/>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export to</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame Text=""end"">
            </FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ICreationFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Export"">
                    <FrameKeywordFrame>exported</FrameKeywordFrame>
                    <FrameKeywordFrame>private</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameKeywordFrame>creation</FrameKeywordFrame>
                <FrameInsertFrame CollectionName=""OverloadBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""OverloadBlocks"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export to</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IFunctionFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Export"">
                    <FrameKeywordFrame>exported</FrameKeywordFrame>
                    <FrameKeywordFrame>private</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>once per</FrameKeywordFrame>
                    <FrameDiscreteFrame PropertyName=""Once"">
                        <FrameKeywordFrame>normal</FrameKeywordFrame>
                        <FrameKeywordFrame>object</FrameKeywordFrame>
                        <FrameKeywordFrame>processor</FrameKeywordFrame>
                        <FrameKeywordFrame>process</FrameKeywordFrame>
                    </FrameDiscreteFrame>
                </FrameHorizontalPanelFrame>
                <FrameKeywordFrame>function</FrameKeywordFrame>
                <FrameInsertFrame CollectionName=""OverloadBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""OverloadBlocks"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export to</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IIndexerFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Export"">
                    <FrameKeywordFrame>exported</FrameKeywordFrame>
                    <FrameKeywordFrame>private</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameKeywordFrame>indexer</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""EntityType""/>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameDiscreteFrame PropertyName=""ParameterEnd"">
                            <FrameKeywordFrame>closed</FrameKeywordFrame>
                            <FrameKeywordFrame>open</FrameKeywordFrame>
                        </FrameDiscreteFrame>
                        <FrameKeywordFrame>parameter</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""IndexParameterBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""IndexParameterBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>modify</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalPanelFrame>
                        <FrameHorizontalBlockListFrame PropertyName=""ModifiedQueryBlocks"" />
                    </FrameVerticalPanelFrame>
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameOptionalFrame PropertyName=""GetterBody"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameOptionalFrame PropertyName=""SetterBody"" />
                </FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export to</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IProcedureFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Export"">
                    <FrameKeywordFrame>exported</FrameKeywordFrame>
                    <FrameKeywordFrame>private</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameKeywordFrame>procedure</FrameKeywordFrame>
                <FrameInsertFrame CollectionName=""OverloadBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""OverloadBlocks"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export to</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPropertyFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""Export"">
                    <FrameKeywordFrame>exported</FrameKeywordFrame>
                    <FrameKeywordFrame>private</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameKeywordFrame>is</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""EntityType""/>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>modify</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalPanelFrame>
                        <FrameHorizontalBlockListFrame PropertyName=""ModifiedQueryBlocks"" />
                    </FrameVerticalPanelFrame>
                </FrameVerticalPanelFrame>
                <FrameOptionalFrame PropertyName=""GetterBody""  />
                <FrameOptionalFrame PropertyName=""SetterBody"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>export to</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame Text=""end"">
            </FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IIdentifier}"">
        <FrameTextValueFrame PropertyName=""Text""/>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAsLongAsInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>as long as</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ContinueCondition""/>
                <FrameInsertFrame CollectionName=""ContinuationBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""ContinuationBlocks"" />
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>else</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameOptionalFrame PropertyName=""ElseInstructions"" />
                </FrameVerticalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalBlockListFrame PropertyName=""DestinationBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAttachmentInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>attach</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""Source"" />
                <FrameKeywordFrame>to</FrameKeywordFrame>
                <FrameHorizontalBlockListFrame PropertyName=""EntityNameBlocks"" />
                <FrameInsertFrame CollectionName=""AttachmentBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""AttachmentBlocks"" />
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>else</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameOptionalFrame PropertyName=""ElseInstructions"" />
                </FrameVerticalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ICheckInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>check</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""BooleanExpression"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ICommandInstruction}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Command"" />
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FrameSymbolFrame>
                <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FrameSymbolFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ICreateInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>create</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""EntityIdentifier"" />
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>with</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""CreationRoutineIdentifier"" />
                <FrameHorizontalPanelFrame>
                    <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}""/>
                    <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                    <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}""/>
                </FrameHorizontalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>same processor as</FrameKeywordFrame>
                    <FrameOptionalFrame PropertyName=""Processor"" />
                </FrameHorizontalPanelFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IDebugInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>debug</FrameKeywordFrame>
                <FrameInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions"" />
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>end</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IForLoopInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameKeywordFrame>loop</FrameKeywordFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>local</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>init</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""InitInstructionBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""InitInstructionBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>while</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""WhileCondition""/>
                    <FrameInsertFrame CollectionName=""LoopInstructionBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""LoopInstructionBlocks"" />
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>iterate</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""IterationInstructionBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""IterationInstructionBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>invariant</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""InvariantBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""InvariantBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>variant</FrameKeywordFrame>
                    <FrameOptionalFrame PropertyName=""Variant"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IIfThenElseInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalBlockListFrame PropertyName=""ConditionalBlocks"" />
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>else</FrameKeywordFrame>
                    <FrameInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                </FrameHorizontalPanelFrame>
                <FrameOptionalFrame PropertyName=""ElseInstructions"" />
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IIndexAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Destination"" />
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
                <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            </FrameHorizontalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IInspectInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>inspect</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""Source"" />
                <FrameInsertFrame CollectionName=""WithBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameVerticalBlockListFrame PropertyName=""WithBlocks"" />
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>else</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameOptionalFrame PropertyName=""ElseInstructions"" />
                </FrameVerticalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IKeywordAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameDiscreteFrame PropertyName=""Destination"">
                <FrameKeywordFrame>True</FrameKeywordFrame>
                <FrameKeywordFrame>False</FrameKeywordFrame>
                <FrameKeywordFrame>Current</FrameKeywordFrame>
                <FrameKeywordFrame>Value</FrameKeywordFrame>
                <FrameKeywordFrame>Result</FrameKeywordFrame>
                <FrameKeywordFrame>Retry</FrameKeywordFrame>
                <FrameKeywordFrame>Exception</FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IOverLoopInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>over</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""OverList"" />
                <FrameKeywordFrame>for each</FrameKeywordFrame>
                <FrameHorizontalBlockListFrame PropertyName=""IndexerBlocks"" />
                <FrameDiscreteFrame PropertyName=""Iteration"">
                    <FrameKeywordFrame>Single</FrameKeywordFrame>
                    <FrameKeywordFrame>Nested</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameInsertFrame CollectionName=""LoopInstructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FramePlaceholderFrame PropertyName=""LoopInstructions"" />
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>exit if</FrameKeywordFrame>
                    <FrameOptionalFrame PropertyName=""ExitEntityName"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>invariant</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""InvariantBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""InvariantBlocks"" />
                </FrameVerticalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameKeywordFrame>end</FrameKeywordFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPrecursorIndexAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>precursor</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>from</FrameKeywordFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
                <FrameOptionalFrame PropertyName=""AncestorType"" />
            </FrameHorizontalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPrecursorInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>precursor</FrameKeywordFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>from</FrameKeywordFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FrameOptionalFrame PropertyName=""AncestorType"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}""/>
                <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}""/>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IRaiseEventInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>raise</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""QueryIdentifier"" />
            <FrameDiscreteFrame PropertyName=""Event"">
                <FrameKeywordFrame>once</FrameKeywordFrame>
                <FrameKeywordFrame>forever</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IReleaseInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>release</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""EntityName""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IThrowInstruction}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>throw</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""ExceptionType"" />
            <FrameKeywordFrame>with</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""CreationRoutine"" />
            <FrameHorizontalPanelFrame>
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}""/>
                <FrameHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}""/>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAnchoredType}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>like</FrameKeywordFrame>
            <FrameDiscreteFrame PropertyName=""AnchorKind"">
                <FrameKeywordFrame>declaration</FrameKeywordFrame>
                <FrameKeywordFrame>creation</FrameKeywordFrame>
            </FrameDiscreteFrame>
            <FramePlaceholderFrame PropertyName=""AnchoredName"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IFunctionType}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>function</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""BaseType"" />
            <FrameHorizontalBlockListFrame PropertyName=""OverloadBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IGenericType}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameHorizontalBlockListFrame PropertyName=""TypeArgumentBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FrameDiscreteFrame PropertyName=""Sharing"">
                <FrameKeywordFrame>not shared</FrameKeywordFrame>
                <FrameKeywordFrame>readwrite</FrameKeywordFrame>
                <FrameKeywordFrame>read-only</FrameKeywordFrame>
                <FrameKeywordFrame>write-only</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IIndexerType}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""BaseType"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>indexer</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""EntityType""/>
                </FrameHorizontalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>parameter</FrameKeywordFrame>
                        <FrameDiscreteFrame PropertyName=""ParameterEnd"">
                            <FrameKeywordFrame>closed</FrameKeywordFrame>
                            <FrameKeywordFrame>open</FrameKeywordFrame>
                        </FrameDiscreteFrame>
                        <FrameInsertFrame CollectionName=""IndexParameterBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""IndexParameterBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameDiscreteFrame PropertyName=""IndexerKind"">
                    <FrameKeywordFrame>read-only</FrameKeywordFrame>
                    <FrameKeywordFrame>write-only</FrameKeywordFrame>
                    <FrameKeywordFrame>readwrite</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>getter require</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""GetRequireBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""GetRequireBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>getter ensure</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""GetEnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""GetEnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>getter exception</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""GetExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""GetExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>setter require</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""SetRequireBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""SetRequireBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>setter ensure</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""SetEnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""SetEnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>setter exception</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""SetExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""SetExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameKeywordFrame>end</FrameKeywordFrame>
            </FrameVerticalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IKeywordAnchoredType}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>like</FrameKeywordFrame>
            <FrameDiscreteFrame PropertyName=""Anchor"">
                <FrameKeywordFrame>True</FrameKeywordFrame>
                <FrameKeywordFrame>False</FrameKeywordFrame>
                <FrameKeywordFrame>Current</FrameKeywordFrame>
                <FrameKeywordFrame>Value</FrameKeywordFrame>
                <FrameKeywordFrame>Result</FrameKeywordFrame>
                <FrameKeywordFrame>Retry</FrameKeywordFrame>
                <FrameKeywordFrame>Exception</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IProcedureType}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>procedure</FrameKeywordFrame>
            <FramePlaceholderFrame PropertyName=""BaseType"" />
            <FrameHorizontalBlockListFrame PropertyName=""OverloadBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPropertyType}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""BaseType"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <FrameKeywordFrame>is</FrameKeywordFrame>
                    <FramePlaceholderFrame PropertyName=""EntityType""/>
                </FrameHorizontalPanelFrame>
                <FrameDiscreteFrame PropertyName=""PropertyKind"">
                    <FrameKeywordFrame>read-only</FrameKeywordFrame>
                    <FrameKeywordFrame>write-only</FrameKeywordFrame>
                    <FrameKeywordFrame>readwrite</FrameKeywordFrame>
                </FrameDiscreteFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>getter ensure</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""GetEnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""GetEnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>getter exception</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""GetExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""GetExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>setter require</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""SetRequireBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""SetRequireBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <FrameKeywordFrame>setter exception</FrameKeywordFrame>
                        <FrameInsertFrame CollectionName=""SetExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalBlockListFrame PropertyName=""SetExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameKeywordFrame>end</FrameKeywordFrame>
            </FrameVerticalPanelFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ISimpleType}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" />
            <FrameDiscreteFrame PropertyName=""Sharing"">
                <FrameKeywordFrame>not shared</FrameKeywordFrame>
                <FrameKeywordFrame>readwrite</FrameKeywordFrame>
                <FrameKeywordFrame>read-only</FrameKeywordFrame>
                <FrameKeywordFrame>write-only</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type ITupleType}"">
        <FrameHorizontalPanelFrame>
            <FrameKeywordFrame>tuple</FrameKeywordFrame>
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FrameVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FrameDiscreteFrame PropertyName=""Sharing"">
                <FrameKeywordFrame>not shared</FrameKeywordFrame>
                <FrameKeywordFrame>readwrite</FrameKeywordFrame>
                <FrameKeywordFrame>read-only</FrameKeywordFrame>
                <FrameKeywordFrame>write-only</FrameKeywordFrame>
            </FrameDiscreteFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IAssignmentTypeArgument}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ParameterIdentifier"" />
            <FrameSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{xaml:Type IPositionalTypeArgument}"">
        <FramePlaceholderFrame PropertyName=""Source"" />
    </FrameNodeTemplate>
</FrameTemplateList>";

        static string FrameBlockTemplateString =
@"<FrameTemplateList
    xmlns=""clr-namespace:EaslyController.Frame;assembly=Easly-Controller""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:xaml=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller""
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IArgument,Argument}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IAssertion,Assertion}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IAssignmentArgument,AssignmentArgument}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IAttachment,Attachment}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IClass,Class}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IClassReplicate,ClassReplicate}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,ICommandOverload,CommandOverload}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,ICommandOverloadType,CommandOverloadType}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IConditional,Conditional}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IConstraint,Constraint}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IContinuation,Continuation}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IDiscrete,Discrete}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IEntityDeclaration,EntityDeclaration}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IExceptionHandler,ExceptionHandler}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IExport,Export}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IExportChange,ExportChange}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IFeature,Feature}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IGeneric,Generic}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IIdentifier,Identifier}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IImport,Import}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IInheritance,Inheritance}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IInstruction,Instruction}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,ILibrary,Library}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IName,Name}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IObjectType,ObjectType}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IPattern,Pattern}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IQualifiedName,QualifiedName}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IQueryOverload,QueryOverload}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IQueryOverloadType,QueryOverloadType}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IRange,Range}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IRename,Rename}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,ITypeArgument,TypeArgument}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,ITypedef,Typedef}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
    <FrameBlockTemplate NodeType=""{xaml:Type IBlock,IWith,With}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame>Replicate</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FrameKeywordFrame>From</FrameKeywordFrame>
                <FramePlaceholderFrame PropertyName=""SourceIdentifier""/>
                <FrameKeywordFrame>All</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalCollectionPlaceholderFrame/>
            <FrameKeywordFrame Text=""end""/>
        </FrameHorizontalPanelFrame>
    </FrameBlockTemplate>
</FrameTemplateList>
";
        #endregion
    }
}
