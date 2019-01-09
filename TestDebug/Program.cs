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

            Test(Serializer, CurrentDirectory);
        }

        static void Test(Serializer Serializer, string Folder)
        {
            LoadTemplates();

            foreach (string Subfolder in Directory.GetDirectories(Folder))
                Test(Serializer, Subfolder);

            foreach (string FileName in Directory.GetFiles(Folder, "*.easly"))
            {
                Console.WriteLine(FileName);

                TestReadOnly(Serializer, FileName);
                TestWriteable(Serializer, FileName);
                TestFrame(Serializer, FileName);
            }
        }

        static void LoadTemplates()
        {
            object t = LoadTemplate(
@"<FrameTemplateList xmlns=""clr-namespace:EaslyController.Frame;assembly=Easly-Controller"" xmlns:x=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller"">
    <FrameNodeTemplate NodeType=""{x:Type IAssertion}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FramePlaceholderFrame PropertyName=""Tag""/>
                <FrameKeywordFrame>:</FrameKeywordFrame>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""BooleanExpression""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IAttachment}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FrameKeywordFrame Text=""else"">
                </FrameKeywordFrame>
                <FrameKeywordFrame>as</FrameKeywordFrame>
                <FrameHorizontalBlockListFrame PropertyName=""AttachTypeBlocks""/>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions""/>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
</FrameTemplateList>
"
                );
        }

        static object LoadTemplate(string s)
        {
            s = TemplateList;

            byte[] ByteArray = Encoding.UTF8.GetBytes(s);
            using (MemoryStream ms = new MemoryStream(ByteArray))
            {
                object t = XamlReader.Parse(s);
                return t;
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
        static string TemplateList =
@"<FrameTemplateList xmlns=""clr-namespace:EaslyController.Frame;assembly=Easly-Controller"" xmlns:x=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller"">
    <FrameNodeTemplate NodeType=""{x:Type IAssertion}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FramePlaceholderFrame PropertyName=""Tag"" />
                <KeywordDecoration RightMargin=""Whitespace"">:</KeywordDecoration>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""BooleanExpression"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IAttachment}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <KeywordDecoration RightMargin=""Whitespace"" Text=""else"">
                </KeywordDecoration>
                <KeywordDecoration RightMargin=""Whitespace"">as</KeywordDecoration>
                <HorizontalCollectionDecoration PropertyName=""AttachTypeBlocks"" />
                <InsertDecoration CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IClass}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <DiscreteDecoration PropertyName=""CopySpecification"" RightMargin=""Whitespace"">
                    <KeywordDecoration>any</KeywordDecoration>
                    <KeywordDecoration>reference</KeywordDecoration>
                    <KeywordDecoration>value</KeywordDecoration>
                </DiscreteDecoration>
                <DiscreteDecoration PropertyName=""Cloneable"" RightMargin=""Whitespace"">
                    <KeywordDecoration>cloneable</KeywordDecoration>
                    <KeywordDecoration>single</KeywordDecoration>
                </DiscreteDecoration>
                <DiscreteDecoration PropertyName=""Comparable"" RightMargin=""Whitespace"">
                    <KeywordDecoration>comparable</KeywordDecoration>
                    <KeywordDecoration>incomparable</KeywordDecoration>
                </DiscreteDecoration>
                <DiscreteDecoration PropertyName=""IsAbstract"" RightMargin=""Whitespace"">
                    <KeywordDecoration>instanceable</KeywordDecoration>
                    <KeywordDecoration>abstract</KeywordDecoration>
                </DiscreteDecoration>
                <KeywordDecoration>class</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""EntityName"" LeftMargin=""Whitespace"" />
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""Whitespace"">from</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""FromIdentifier"" IdentifierType=""Source"" />
                </FrameHorizontalPanelFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>import</KeywordDecoration>
                    <InsertDecoration CollectionName=""ImportBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration HasTabulationMargin=""True"" PropertyName=""ImportBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>generic</KeywordDecoration>
                    <InsertDecoration CollectionName=""GenericBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration HasTabulationMargin=""True"" PropertyName=""GenericBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>export</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExportBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration HasTabulationMargin=""True"" PropertyName=""ExportBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>typedef</KeywordDecoration>
                    <InsertDecoration CollectionName=""TypedefBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration HasTabulationMargin=""True"" PropertyName=""TypedefBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>inheritance</KeywordDecoration>
                    <InsertDecoration CollectionName=""InheritanceBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration HasTabulationMargin=""True"" PropertyName=""InheritanceBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>discrete</KeywordDecoration>
                    <InsertDecoration CollectionName=""DiscreteBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration HasTabulationMargin=""True"" PropertyName=""DiscreteBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>replicate</KeywordDecoration>
                    <InsertDecoration CollectionName=""ClassReplicateBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration HasTabulationMargin=""True"" PropertyName=""ClassReplicateBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>feature</KeywordDecoration>
                    <InsertDecoration CollectionName=""FeatureBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration HasTabulationMargin=""True"" PropertyName=""FeatureBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>conversion</KeywordDecoration>
                    <InsertDecoration CollectionName=""ConversionBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration HasTabulationMargin=""True"" PropertyName=""ConversionBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>invariant</KeywordDecoration>
                    <InsertDecoration CollectionName=""InvariantBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration HasTabulationMargin=""True"" PropertyName=""InvariantBlocks"" />
            </FrameVerticalPanelFrame>
            <KeywordDecoration Text=""end"">
            </KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IClassReplicate}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ReplicateName"" />
            <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</KeywordDecoration>
            <HorizontalCollectionDecoration PropertyName=""PatternBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type ICommandOverload}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>parameter</KeywordDecoration>
                    <DiscreteDecoration PropertyName=""ParameterEnd"" LeftMargin=""Whitespace"">
                        <KeywordDecoration>closed</KeywordDecoration>
                        <KeywordDecoration>open</KeywordDecoration>
                    </DiscreteDecoration>
                    <InsertDecoration CollectionName=""ParameterBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""ParameterBlocks"" />
            </FrameVerticalPanelFrame>
            <FramePlaceholderFrame PropertyName=""CommandBody"" BodyType=""Overload"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type ICommandOverloadType}"">
        <FrameHorizontalPanelFrame>
            <SymbolDecoration Symbol=""LeftBracket"" RightMargin=""ThinSpace""/>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>parameter</KeywordDecoration>
                        <DiscreteDecoration PropertyName=""ParameterEnd"" LeftMargin=""Whitespace"">
                            <KeywordDecoration>closed</KeywordDecoration>
                            <KeywordDecoration>open</KeywordDecoration>
                        </DiscreteDecoration>
                        <InsertDecoration CollectionName=""ParameterBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""ParameterBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>require</KeywordDecoration>
                        <InsertDecoration CollectionName=""RequireBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""RequireBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>ensure</KeywordDecoration>
                        <InsertDecoration CollectionName=""EnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""EnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>exception</KeywordDecoration>
                        <InsertDecoration CollectionName=""ExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""ExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <KeywordDecoration>end</KeywordDecoration>
            </FrameVerticalPanelFrame>
            <SymbolDecoration Symbol=""RightBracket"" LeftMargin=""ThinSpace""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IConditional}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <KeywordDecoration RightMargin=""Whitespace"" Text=""else"">
                </KeywordDecoration>
                <KeywordDecoration>if</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""BooleanExpression"" LeftMargin=""Whitespace"" RightMargin=""Whitespace"" />
                <KeywordDecoration>then</KeywordDecoration>
                <InsertDecoration CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IConstraint}"">
        <FrameVerticalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ParentType"" />
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>rename</KeywordDecoration>
                    <InsertDecoration CollectionName=""RenameBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""RenameBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IContinuation}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <KeywordDecoration>execute</KeywordDecoration>
                <InsertDecoration CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FramePlaceholderFrame PropertyName=""Instructions"" />
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>cleanup</KeywordDecoration>
                        <InsertDecoration CollectionName=""CleanupBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""CleanupBlocks"" />
                </FrameVerticalPanelFrame>
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IDiscrete}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <FrameHorizontalPanelFrame>
                <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""Whitespace"">=</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""NumericValue"" />
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IEntityDeclaration}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <KeywordDecoration RightMargin=""Whitespace"">:</KeywordDecoration>
            <FramePlaceholderFrame PropertyName=""EntityType"" />
            <FrameHorizontalPanelFrame>
                <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""Whitespace"">=</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""DefaultValue"" />
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IExceptionHandler}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <KeywordDecoration>catch</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""ExceptionIdentifier"" IdentifierType=""Type"" LeftMargin=""Whitespace"" />
                <InsertDecoration CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IExport}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</KeywordDecoration>
            <HorizontalCollectionDecoration PropertyName=""ClassIdentifierBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IExportChange}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ExportIdentifier"" IdentifierType=""Export"" />
            <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</KeywordDecoration>
            <HorizontalCollectionDecoration PropertyName=""IdentifierBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IGeneric}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""Whitespace"">=</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""DefaultValue"" />
                </FrameHorizontalPanelFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>conform to</KeywordDecoration>
                    <InsertDecoration CollectionName=""ConstraintBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""ConstraintBlocks"" />
            </FrameVerticalPanelFrame>
            <KeywordDecoration Text=""end"">
            </KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IGlobalReplicate}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ReplicateName"" />
            <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</KeywordDecoration>
            <HorizontalCollectionDecoration PropertyName=""Patterns"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IImport}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <DiscreteDecoration PropertyName=""Type"">
                    <KeywordDecoration>latest</KeywordDecoration>
                    <KeywordDecoration>strict</KeywordDecoration>
                    <KeywordDecoration>stable</KeywordDecoration>
                </DiscreteDecoration>
                <FramePlaceholderFrame PropertyName=""LibraryIdentifier"" IdentifierType=""Library"" LeftMargin=""Whitespace"" />
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""Whitespace"">from</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""FromIdentifier"" IdentifierType=""Source"" />
                </FrameHorizontalPanelFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>rename</KeywordDecoration>
                    <InsertDecoration CollectionName=""RenameBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration HasTabulationMargin=""True"" PropertyName=""RenameBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <KeywordDecoration>end</KeywordDecoration>
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IInheritance}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <DiscreteDecoration PropertyName=""Conformance"" RightMargin=""Whitespace"">
                    <KeywordDecoration>conformant</KeywordDecoration>
                    <KeywordDecoration>non-conformant</KeywordDecoration>
                </DiscreteDecoration>
                <FramePlaceholderFrame PropertyName=""ParentType"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>rename</KeywordDecoration>
                        <InsertDecoration CollectionName=""RenameBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""RenameBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>forget</KeywordDecoration>
                        <InsertDecoration CollectionName=""ForgetBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""ForgetBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>keep</KeywordDecoration>
                        <InsertDecoration CollectionName=""KeepBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""KeepBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>discontinue</KeywordDecoration>
                        <InsertDecoration CollectionName=""DiscontinueBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""DiscontinueBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>export</KeywordDecoration>
                        <InsertDecoration CollectionName=""ExportChangeBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""ExportChangeBlocks"" />
                </FrameVerticalPanelFrame>
                <DiscreteDecoration PropertyName=""ForgetIndexer"" RightMargin=""Whitespace"">
                    <KeywordDecoration>ignore indexer</KeywordDecoration>
                    <KeywordDecoration>forget indexer</KeywordDecoration>
                </DiscreteDecoration>
                <DiscreteDecoration PropertyName=""KeepIndexer"" RightMargin=""Whitespace"">
                    <KeywordDecoration>ignore indexer</KeywordDecoration>
                    <KeywordDecoration>keep indexer</KeywordDecoration>
                </DiscreteDecoration>
                <DiscreteDecoration PropertyName=""DiscontinueIndexer"" RightMargin=""Whitespace"">
                    <KeywordDecoration>ignore indexer</KeywordDecoration>
                    <KeywordDecoration>discontinue indexer</KeywordDecoration>
                </DiscreteDecoration>
                <KeywordDecoration Text=""end"">
                </KeywordDecoration>
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type ILibrary}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <KeywordDecoration>library</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""EntityName"" LeftMargin=""Whitespace"" />
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""Whitespace"">from</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""FromIdentifier"" IdentifierType=""Source"" />
                </FrameHorizontalPanelFrame>
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>import</KeywordDecoration>
                    <InsertDecoration CollectionName=""ImportBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""ImportBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>class</KeywordDecoration>
                    <InsertDecoration CollectionName=""ClassIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""ClassIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <KeywordDecoration Text=""end"">
            </KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IName}"">
        <IdentifierDecoration PropertyName=""Text"" Type=""Name""/>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IPattern}"">
        <IdentifierDecoration PropertyName=""Text"" Type=""Pattern""/>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IQualifiedName}"">
        <HorizontalCollectionDecoration PropertyName=""Path"" />
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IQueryOverload}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>parameter</KeywordDecoration>
                    <DiscreteDecoration PropertyName=""ParameterEnd"" LeftMargin=""Whitespace"">
                        <KeywordDecoration>closed</KeywordDecoration>
                        <KeywordDecoration>open</KeywordDecoration>
                    </DiscreteDecoration>
                    <InsertDecoration CollectionName=""ParameterBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""ParameterBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>result</KeywordDecoration>
                    <InsertDecoration CollectionName=""ResultBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""ResultBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>modified</KeywordDecoration>
                    <InsertDecoration CollectionName=""ModifiedQueryBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""ModifiedQueryBlocks"" />
            </FrameVerticalPanelFrame>
            <FramePlaceholderFrame PropertyName=""QueryBody"" BodyType=""Overload"" />
            <FrameHorizontalPanelFrame>
                <KeywordDecoration RightMargin=""Whitespace"">variant</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""Variant"" />
            </FrameHorizontalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IQueryOverloadType}"">
        <FrameHorizontalPanelFrame>
            <SymbolDecoration Symbol=""LeftBracket"" RightMargin=""ThinSpace""/>
            <FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>parameter</KeywordDecoration>
                        <DiscreteDecoration PropertyName=""ParameterEnd"" LeftMargin=""Whitespace"">
                            <KeywordDecoration>closed</KeywordDecoration>
                            <KeywordDecoration>open</KeywordDecoration>
                        </DiscreteDecoration>
                        <InsertDecoration CollectionName=""ParameterBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""ParameterBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>return</KeywordDecoration>
                        <InsertDecoration CollectionName=""ResultBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""ResultBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>require</KeywordDecoration>
                        <InsertDecoration CollectionName=""RequireBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""RequireBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>ensure</KeywordDecoration>
                        <InsertDecoration CollectionName=""EnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""EnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>exception</KeywordDecoration>
                        <InsertDecoration CollectionName=""ExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""ExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <KeywordDecoration>end</KeywordDecoration>
            </FrameVerticalPanelFrame>
            <SymbolDecoration Symbol=""RightBracket"" LeftMargin=""ThinSpace""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IRange}"">
        <FrameHorizontalPanelFrame>
            <SymbolDecoration Symbol=""LeftBracket"" RightMargin=""ThinSpace"">
            </SymbolDecoration>
            <FramePlaceholderFrame PropertyName=""LeftExpression"" />
            <FrameHorizontalPanelFrame>
                <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <SymbolDecoration Symbol=""RightBracket"" LeftMargin=""ThinSpace""/>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IRename}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""SourceIdentifier"" IdentifierType=""Feature"" />
            <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</KeywordDecoration>
            <FramePlaceholderFrame PropertyName=""DestinationIdentifier"" IdentifierType=""Feature"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IRoot}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>libraries</KeywordDecoration>
                    <InsertDecoration CollectionName=""LibraryBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""LibraryBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>classes</KeywordDecoration>
                    <InsertDecoration CollectionName=""ClassBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""ClassBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>replicates</KeywordDecoration>
                    <InsertDecoration CollectionName=""Replicates"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""Replicates"" />
            </FrameVerticalPanelFrame>
            <KeywordDecoration>end</KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IScope}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>local</KeywordDecoration>
                    <InsertDecoration CollectionName=""EntityDeclarationBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EntityDeclarationBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>do</KeywordDecoration>
                    <InsertDecoration CollectionName=""InstructionBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""InstructionBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type ITypedef}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""EntityName"" />
            <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""Whitespace"">is</KeywordDecoration>
            <FramePlaceholderFrame PropertyName=""DefinedType"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IWith}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <KeywordDecoration RightMargin=""Whitespace"">case</KeywordDecoration>
                <HorizontalCollectionDecoration PropertyName=""RangeBlocks"" />
                <InsertDecoration CollectionName=""RangeBlocks"" />
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions"" />
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>

    <FrameNodeTemplate NodeType=""{x:Type IAssignmentArgument}"">
        <FrameHorizontalPanelFrame>
            <HorizontalCollectionDecoration PropertyName=""ParameterBlocks"" />
            <SymbolDecoration Symbol=""LeftArrow"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IPositionalArgument}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>

    <FrameNodeTemplate NodeType=""{x:Type IDeferredBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>require</KeywordDecoration>
                    <InsertDecoration CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>throw</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <HorizontalCollectionDecoration PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>getter</KeywordDecoration>
                    <KeywordDecoration IsFocusable=""True"" LeftMargin=""Whitespace"">deferred</KeywordDecoration>
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>ensure</KeywordDecoration>
                    <InsertDecoration CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IDeferredBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>require</KeywordDecoration>
                    <InsertDecoration CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>throw</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <HorizontalCollectionDecoration PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration IsFocusable=""True"">deferred</KeywordDecoration>
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>ensure</KeywordDecoration>
                    <InsertDecoration CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IDeferredBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>require</KeywordDecoration>
                    <InsertDecoration CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>throw</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <HorizontalCollectionDecoration PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>setter</KeywordDecoration>
                    <KeywordDecoration IsFocusable=""True"" LeftMargin=""Whitespace"">deferred</KeywordDecoration>
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>ensure</KeywordDecoration>
                    <InsertDecoration CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>

    <FrameNodeTemplate NodeType=""{x:Type IEffectiveBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>require</KeywordDecoration>
                    <InsertDecoration CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>throw</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <HorizontalCollectionDecoration PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>local</KeywordDecoration>
                    <InsertDecoration CollectionName=""EntityDeclarationBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EntityDeclarationBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration IsFocusable=""True"">getter</KeywordDecoration>
                    <InsertDecoration CollectionName=""BodyInstructionBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""BodyInstructionBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>exception</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExceptionHandlerBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""ExceptionHandlerBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>ensure</KeywordDecoration>
                    <InsertDecoration CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IEffectiveBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>require</KeywordDecoration>
                    <InsertDecoration CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>throw</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <HorizontalCollectionDecoration PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>local</KeywordDecoration>
                    <InsertDecoration CollectionName=""EntityDeclarationBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EntityDeclarationBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration IsFocusable=""True"">do</KeywordDecoration>
                    <InsertDecoration CollectionName=""BodyInstructionBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""BodyInstructionBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>exception</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExceptionHandlerBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""ExceptionHandlerBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>ensure</KeywordDecoration>
                    <InsertDecoration CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IEffectiveBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>require</KeywordDecoration>
                    <InsertDecoration CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>throw</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <HorizontalCollectionDecoration PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>local</KeywordDecoration>
                    <InsertDecoration CollectionName=""EntityDeclarationBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EntityDeclarationBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration IsFocusable=""True"">setter</KeywordDecoration>
                    <InsertDecoration CollectionName=""BodyInstructionBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""BodyInstructionBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>exception</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExceptionHandlerBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""ExceptionHandlerBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>ensure</KeywordDecoration>
                    <InsertDecoration CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>

    <FrameNodeTemplate NodeType=""{x:Type IExternBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>require</KeywordDecoration>
                    <InsertDecoration CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>throw</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <HorizontalCollectionDecoration PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>getter</KeywordDecoration>
                    <KeywordDecoration IsFocusable=""True"" LeftMargin=""Whitespace"">extern</KeywordDecoration>
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>ensure</KeywordDecoration>
                    <InsertDecoration CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IExternBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>require</KeywordDecoration>
                    <InsertDecoration CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>throw</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <HorizontalCollectionDecoration PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration IsFocusable=""True"">extern</KeywordDecoration>
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>ensure</KeywordDecoration>
                    <InsertDecoration CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IExternBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>require</KeywordDecoration>
                    <InsertDecoration CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>throw</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <HorizontalCollectionDecoration PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>setter</KeywordDecoration>
                    <KeywordDecoration IsFocusable=""True"" LeftMargin=""Whitespace"">extern</KeywordDecoration>
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>ensure</KeywordDecoration>
                    <InsertDecoration CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>

    <FrameNodeTemplate NodeType=""{x:Type IPrecursorBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>require</KeywordDecoration>
                    <InsertDecoration CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>throw</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <HorizontalCollectionDecoration PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>getter</KeywordDecoration>
                    <KeywordDecoration IsFocusable=""True"" LeftMargin=""Whitespace"">precursor</KeywordDecoration>
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>ensure</KeywordDecoration>
                    <InsertDecoration CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IPrecursorBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>require</KeywordDecoration>
                    <InsertDecoration CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>throw</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <HorizontalCollectionDecoration PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration IsFocusable=""True"">precursor</KeywordDecoration>
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>ensure</KeywordDecoration>
                    <InsertDecoration CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IPrecursorBody}"">
        <FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>require</KeywordDecoration>
                    <InsertDecoration CollectionName=""RequireBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""RequireBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>throw</KeywordDecoration>
                    <InsertDecoration CollectionName=""ExceptionIdentifierBlocks"" />
                </FrameHorizontalPanelFrame>
                <HorizontalCollectionDecoration PropertyName=""ExceptionIdentifierBlocks"" />
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>setter</KeywordDecoration>
                    <KeywordDecoration IsFocusable=""True"" LeftMargin=""Whitespace"">precursor</KeywordDecoration>
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>ensure</KeywordDecoration>
                    <InsertDecoration CollectionName=""EnsureBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""EnsureBlocks"" />
            </FrameVerticalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>

    <FrameNodeTemplate NodeType=""{x:Type IAgentExpression}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration>agent</KeywordDecoration>
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftCurlyBracket"" LeftMargin=""ThinSpace""/>
                <FramePlaceholderFrame PropertyName=""BaseType"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace"" />
                <SymbolDecoration Symbol=""RightCurlyBracket""/>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Delegated"" IdentifierType=""Feature"" LeftMargin=""Whitespace"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IAssertionTagExpression}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration>tag</KeywordDecoration>
            <FramePlaceholderFrame PropertyName=""TagIdentifier"" IdentifierType=""Feature"" LeftMargin=""Whitespace"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IBinaryConditionalExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" RightMargin=""ThinSpace"">
                </SymbolDecoration>
                <FramePlaceholderFrame PropertyName=""LeftExpression"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace"">
                </SymbolDecoration>
            </FrameHorizontalPanelFrame>
            <DiscreteDecoration PropertyName=""Conditional"" LeftMargin=""Whitespace"" RightMargin=""Whitespace"">
                <KeywordDecoration>and</KeywordDecoration>
                <KeywordDecoration>or</KeywordDecoration>
            </DiscreteDecoration>
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" RightMargin=""ThinSpace"">
                </SymbolDecoration>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace"">
                </SymbolDecoration>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IBinaryOperatorExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" RightMargin=""ThinSpace"">
                </SymbolDecoration>
                <FramePlaceholderFrame PropertyName=""LeftExpression"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace"">
                </SymbolDecoration>
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Operator"" IdentifierType=""Feature"" LeftMargin=""Whitespace"" RightMargin=""Whitespace"" />
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" RightMargin=""ThinSpace"">
                </SymbolDecoration>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace"">
                </SymbolDecoration>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IClassConstantExpression}"">
        <FrameHorizontalPanelFrame>
            <SymbolDecoration Symbol=""LeftCurlyBracket""/>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" IdentifierType=""Class"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace"" />
            <SymbolDecoration Symbol=""RightCurlyBracket""/>
            <SymbolDecoration Symbol=""Dot""/>
            <FramePlaceholderFrame PropertyName=""ConstantIdentifier"" IdentifierType=""Feature"" LeftMargin=""ThinSpace"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type ICloneOfExpression}"">
        <FrameHorizontalPanelFrame>
            <DiscreteDecoration PropertyName=""Type"" RightMargin=""Whitespace"">
                <KeywordDecoration>shallow</KeywordDecoration>
                <KeywordDecoration>deep</KeywordDecoration>
            </DiscreteDecoration>
            <KeywordDecoration RightMargin=""Whitespace"" IsFocusable=""True"">clone of</KeywordDecoration>
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" RightMargin=""ThinSpace"">
                </SymbolDecoration>
                <FramePlaceholderFrame PropertyName=""Source"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace"">
                </SymbolDecoration>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IEntityExpression}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration>entity</KeywordDecoration>
            <FramePlaceholderFrame PropertyName=""Query"" LeftMargin=""Whitespace"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IEqualityExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" RightMargin=""ThinSpace"">
                </SymbolDecoration>
                <FramePlaceholderFrame PropertyName=""LeftExpression"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace"">
                </SymbolDecoration>
            </FrameHorizontalPanelFrame>
            <DiscreteDecoration PropertyName=""Comparison"" LeftMargin=""Whitespace"">
                <KeywordDecoration>=</KeywordDecoration>
                <KeywordDecoration>≠</KeywordDecoration>
            </DiscreteDecoration>
            <DiscreteDecoration PropertyName=""Equality"">
                <KeywordDecoration>phys</KeywordDecoration>
                <KeywordDecoration>deep</KeywordDecoration>
            </DiscreteDecoration>
            <KeywordDecoration Text="" ""/>
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" RightMargin=""ThinSpace"">
                </SymbolDecoration>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace"">
                </SymbolDecoration>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IIndexQueryExpression}"">
        <FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" RightMargin=""ThinSpace"">
                </SymbolDecoration>
                <FramePlaceholderFrame PropertyName=""IndexedExpression"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace"">
                </SymbolDecoration>
            </FrameHorizontalPanelFrame>
            <SymbolDecoration Symbol=""LeftBracket"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
            <HorizontalCollectionDecoration PropertyName=""ArgumentBlocks"" />
            <SymbolDecoration Symbol=""RightBracket"" LeftMargin=""ThinSpace""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IInitializedObjectExpression}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" IdentifierType=""Type"" />
            <SymbolDecoration Symbol=""LeftBracket"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
            <VerticalCollectionDecoration PropertyName=""AssignmentBlocks"" />
            <SymbolDecoration Symbol=""RightBracket"" LeftMargin=""ThinSpace""/>
            <InsertDecoration CollectionName=""AssignmentBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IKeywordExpression}"">
        <DiscreteDecoration PropertyName=""Value"">
            <KeywordDecoration>True</KeywordDecoration>
            <KeywordDecoration>False</KeywordDecoration>
            <KeywordDecoration>Current</KeywordDecoration>
            <KeywordDecoration>Value</KeywordDecoration>
            <KeywordDecoration>Result</KeywordDecoration>
            <KeywordDecoration>Retry</KeywordDecoration>
            <KeywordDecoration>Exception</KeywordDecoration>
        </DiscreteDecoration>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IManifestCharacterExpression}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration>'</KeywordDecoration>
            <CharacterDecoration PropertyName=""Text""/>
            <KeywordDecoration>'</KeywordDecoration>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IManifestNumberExpression}"">
        <FrameHorizontalPanelFrame>
            <NumberValueDecoration PropertyName=""Text""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IManifestStringExpression}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration>""</KeywordDecoration>
            <StringDecoration PropertyName=""Text""/>
            <KeywordDecoration>""</KeywordDecoration>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type INewExpression}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration RightMargin=""Whitespace"">new</KeywordDecoration>
            <FramePlaceholderFrame PropertyName=""Object"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IOldExpression}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration RightMargin=""Whitespace"">old</KeywordDecoration>
            <FramePlaceholderFrame PropertyName=""Query"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IPrecursorExpression}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration IsFocusable=""True"">precursor</KeywordDecoration>
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftCurlyBracket"" LeftMargin=""ThinSpace""/>
                <FramePlaceholderFrame PropertyName=""AncestorType"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace"" />
                <SymbolDecoration Symbol=""RightCurlyBracket""/>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" LeftMargin=""Whitespace"" RightMargin=""ThinSpace"">
                </SymbolDecoration>
                <HorizontalCollectionDecoration PropertyName=""ArgumentBlocks"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace"">
                </SymbolDecoration>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IPrecursorIndexExpression}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration IsFocusable=""True"">precursor</KeywordDecoration>
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftCurlyBracket"" LeftMargin=""ThinSpace""/>
                <FramePlaceholderFrame PropertyName=""AncestorType"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace"" />
                <SymbolDecoration Symbol=""RightCurlyBracket""/>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftBracket"" LeftMargin=""Whitespace"" RightMargin=""ThinSpace""/>
                <HorizontalCollectionDecoration PropertyName=""ArgumentBlocks"" />
                <SymbolDecoration Symbol=""RightBracket"" LeftMargin=""ThinSpace""/>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IPreprocessorExpression}"">
        <DiscreteDecoration PropertyName=""Value"">
            <KeywordDecoration>DateAndTime</KeywordDecoration>
            <KeywordDecoration>CompilationDiscreteIdentifier</KeywordDecoration>
            <KeywordDecoration>ClassPath</KeywordDecoration>
            <KeywordDecoration>CompilerVersion</KeywordDecoration>
            <KeywordDecoration>ConformanceToStandard</KeywordDecoration>
            <KeywordDecoration>DiscreteClassIdentifier</KeywordDecoration>
            <KeywordDecoration>Counter</KeywordDecoration>
            <KeywordDecoration>Debugging</KeywordDecoration>
            <KeywordDecoration>RandomInteger</KeywordDecoration>
        </DiscreteDecoration>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IQueryExpression}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Query"" />
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace"">
                </SymbolDecoration>
                <HorizontalCollectionDecoration PropertyName=""ArgumentBlocks"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace"">
                </SymbolDecoration>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IResultOfExpression}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration RightMargin=""Whitespace"">result of</KeywordDecoration>
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" RightMargin=""ThinSpace"">
                </SymbolDecoration>
                <FramePlaceholderFrame PropertyName=""Source"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace"">
                </SymbolDecoration>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IUnaryNotExpression}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration RightMargin=""Whitespace"">not</KeywordDecoration>
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" RightMargin=""ThinSpace"">
                </SymbolDecoration>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace"">
                </SymbolDecoration>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IUnaryOperatorExpression}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Operator"" IdentifierType=""Feature"" RightMargin=""Whitespace"" />
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" RightMargin=""ThinSpace"">
                </SymbolDecoration>
                <FramePlaceholderFrame PropertyName=""RightExpression"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace"">
                </SymbolDecoration>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>

    <FrameNodeTemplate NodeType=""{x:Type IAttributeFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <DiscreteDecoration PropertyName=""Export"" RightMargin=""Whitespace"">
                    <KeywordDecoration>exported</KeywordDecoration>
                    <KeywordDecoration>private</KeywordDecoration>
                </DiscreteDecoration>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <KeywordDecoration>:</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""EntityType"" LeftMargin=""Whitespace"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>ensure</KeywordDecoration>
                        <InsertDecoration CollectionName=""EnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""EnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>export to</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" IdentifierType=""Export"" LeftMargin=""Whitespace"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <KeywordDecoration Text=""end"">
            </KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IConstantFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <DiscreteDecoration PropertyName=""Export"" RightMargin=""Whitespace"">
                    <KeywordDecoration>exported</KeywordDecoration>
                    <KeywordDecoration>private</KeywordDecoration>
                </DiscreteDecoration>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <KeywordDecoration>:</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""EntityType"" LeftMargin=""Whitespace"" RightMargin=""Whitespace"" />
                <KeywordDecoration>=</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""ConstantValue"" LeftMargin=""Whitespace"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>export to</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" IdentifierType=""Export"" LeftMargin=""Whitespace"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <KeywordDecoration Text=""end"">
            </KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type ICreationFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <DiscreteDecoration PropertyName=""Export"" RightMargin=""Whitespace"">
                    <KeywordDecoration>exported</KeywordDecoration>
                    <KeywordDecoration>private</KeywordDecoration>
                </DiscreteDecoration>
                <FramePlaceholderFrame PropertyName=""EntityName"" RightMargin=""Whitespace"" />
                <KeywordDecoration>creation</KeywordDecoration>
                <InsertDecoration CollectionName=""OverloadBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <VerticalCollectionDecoration PropertyName=""OverloadBlocks"" />
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>export to</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" IdentifierType=""Export"" LeftMargin=""Whitespace"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <KeywordDecoration>end</KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IFunctionFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <DiscreteDecoration PropertyName=""Export"" RightMargin=""Whitespace"">
                    <KeywordDecoration>exported</KeywordDecoration>
                    <KeywordDecoration>private</KeywordDecoration>
                </DiscreteDecoration>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration LeftMargin=""Whitespace"">once per</KeywordDecoration>
                    <DiscreteDecoration PropertyName=""Once"" LeftMargin=""Whitespace"">
                        <KeywordDecoration>normal</KeywordDecoration>
                        <KeywordDecoration>object</KeywordDecoration>
                        <KeywordDecoration>processor</KeywordDecoration>
                        <KeywordDecoration>process</KeywordDecoration>
                    </DiscreteDecoration>
                </FrameHorizontalPanelFrame>
                <KeywordDecoration LeftMargin=""Whitespace"">function</KeywordDecoration>
                <InsertDecoration CollectionName=""OverloadBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <VerticalCollectionDecoration PropertyName=""OverloadBlocks"" />
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>export to</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" IdentifierType=""Export"" LeftMargin=""Whitespace"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <KeywordDecoration>end</KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IIndexerFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <DiscreteDecoration PropertyName=""Export"" RightMargin=""Whitespace"">
                    <KeywordDecoration>exported</KeywordDecoration>
                    <KeywordDecoration>private</KeywordDecoration>
                </DiscreteDecoration>
                <KeywordDecoration IsFocusable=""True"">indexer</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""EntityType"" LeftMargin=""Whitespace"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <DiscreteDecoration PropertyName=""ParameterEnd"" RightMargin=""Whitespace"">
                            <KeywordDecoration>closed</KeywordDecoration>
                            <KeywordDecoration>open</KeywordDecoration>
                        </DiscreteDecoration>
                        <KeywordDecoration>parameter</KeywordDecoration>
                        <InsertDecoration CollectionName=""IndexParameterBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""IndexParameterBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>modify</KeywordDecoration>
                        <InsertDecoration CollectionName=""ModifiedQueryBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                        <HorizontalCollectionDecoration PropertyName=""ModifiedQueryBlocks"" />
                    </FrameVerticalPanelFrame>
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FramePlaceholderFrame PropertyName=""GetterBody"" BodyType=""Getter"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FramePlaceholderFrame PropertyName=""SetterBody"" BodyType=""Setter"" />
                </FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>export to</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" IdentifierType=""Export"" LeftMargin=""Whitespace"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <KeywordDecoration>end</KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IProcedureFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <DiscreteDecoration PropertyName=""Export"" RightMargin=""Whitespace"">
                    <KeywordDecoration>exported</KeywordDecoration>
                    <KeywordDecoration>private</KeywordDecoration>
                </DiscreteDecoration>
                <FramePlaceholderFrame PropertyName=""EntityName"" />
                <KeywordDecoration LeftMargin=""Whitespace"">procedure</KeywordDecoration>
                <InsertDecoration CollectionName=""OverloadBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <VerticalCollectionDecoration PropertyName=""OverloadBlocks"" />
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>export to</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" IdentifierType=""Export"" LeftMargin=""Whitespace"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <KeywordDecoration>end</KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IPropertyFeature}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <DiscreteDecoration PropertyName=""Export"" RightMargin=""Whitespace"">
                    <KeywordDecoration>exported</KeywordDecoration>
                    <KeywordDecoration>private</KeywordDecoration>
                </DiscreteDecoration>
                <FramePlaceholderFrame PropertyName=""EntityName"" RightMargin=""Whitespace"" />
                <KeywordDecoration>is</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""EntityType"" LeftMargin=""Whitespace"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>modify</KeywordDecoration>
                        <InsertDecoration CollectionName=""ModifiedQueryBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                        <HorizontalCollectionDecoration PropertyName=""ModifiedQueryBlocks"" />
                    </FrameVerticalPanelFrame>
                </FrameVerticalPanelFrame>
                <FramePlaceholderFrame PropertyName=""GetterBody"" BodyType=""Getter"" >
                </FramePlaceholderFrame>
                <FramePlaceholderFrame PropertyName=""SetterBody"" BodyType=""Setter"" >
                </FramePlaceholderFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>export to</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""ExportIdentifier"" IdentifierType=""Export"" LeftMargin=""Whitespace"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <KeywordDecoration Text=""end"">
            </KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>

    <FrameNodeTemplate NodeType=""{x:Type IIdentifier}"">
        <IdentifierDecoration PropertyName=""Text"" Type=""Type""/>
    </FrameNodeTemplate>

    <FrameNodeTemplate NodeType=""{x:Type IAsLongAsInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <KeywordDecoration>as long as</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""ContinueCondition"" LeftMargin=""Whitespace"" />
                <InsertDecoration CollectionName=""ContinuationBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <VerticalCollectionDecoration PropertyName=""ContinuationBlocks"" />
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>else</KeywordDecoration>
                        <InsertDecoration CollectionName=""ElseInstructions.InstructionBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FramePlaceholderFrame PropertyName=""ElseInstructions"" />
                </FrameVerticalPanelFrame>
            </FrameVerticalPanelFrame>
            <KeywordDecoration>end</KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <HorizontalCollectionDecoration PropertyName=""DestinationBlocks"" />
            <SymbolDecoration Symbol=""LeftArrow"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IAttachmentInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <KeywordDecoration RightMargin=""Whitespace"">attach</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""Source"" RightMargin=""Whitespace"" />
                <KeywordDecoration RightMargin=""Whitespace"">to</KeywordDecoration>
                <HorizontalCollectionDecoration PropertyName=""EntityNameBlocks"" IsNeverEmpty=""True"" Separator=""Comma"" />
                <InsertDecoration CollectionName=""AttachmentBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <VerticalCollectionDecoration PropertyName=""AttachmentBlocks"" IsNeverEmpty=""True"" />
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>else</KeywordDecoration>
                        <InsertDecoration CollectionName=""ElseInstructions.InstructionBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FramePlaceholderFrame PropertyName=""ElseInstructions"" />
                </FrameVerticalPanelFrame>
            </FrameVerticalPanelFrame>
            <KeywordDecoration>end</KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type ICheckInstruction}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration RightMargin=""Whitespace"">check</KeywordDecoration>
            <FramePlaceholderFrame PropertyName=""BooleanExpression"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type ICommandInstruction}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Command"" />
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace"">
                </SymbolDecoration>
                <HorizontalCollectionDecoration PropertyName=""ArgumentBlocks"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace"">
                </SymbolDecoration>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type ICreateInstruction}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration>create</KeywordDecoration>
            <FramePlaceholderFrame PropertyName=""EntityIdentifier"" IdentifierType=""Feature"" LeftMargin=""Whitespace"" />
            <FrameHorizontalPanelFrame>
                <KeywordDecoration LeftMargin=""Whitespace"">with</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""CreationRoutineIdentifier"" IdentifierType=""Feature"" LeftMargin=""Whitespace"" />
                <FrameHorizontalPanelFrame>
                    <SymbolDecoration Symbol=""LeftParenthesis"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
                    <HorizontalCollectionDecoration PropertyName=""ArgumentBlocks"" />
                    <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace""/>
                </FrameHorizontalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""Whitespace"">same processor as</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""Processor"" />
                </FrameHorizontalPanelFrame>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IDebugInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <KeywordDecoration>debug</KeywordDecoration>
                <InsertDecoration CollectionName=""Instructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Instructions"" />
            <FrameHorizontalPanelFrame>
                <KeywordDecoration>end</KeywordDecoration>
            </FrameHorizontalPanelFrame>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IForLoopInstruction}"">
        <FrameVerticalPanelFrame>
            <KeywordDecoration>loop</KeywordDecoration>
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>local</KeywordDecoration>
                        <InsertDecoration CollectionName=""EntityDeclarationBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""EntityDeclarationBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>init</KeywordDecoration>
                        <InsertDecoration CollectionName=""InitInstructionBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""InitInstructionBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>while</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""WhileCondition"" LeftMargin=""Whitespace"" />
                    <InsertDecoration CollectionName=""LoopInstructionBlocks"" />
                </FrameHorizontalPanelFrame>
                <VerticalCollectionDecoration PropertyName=""LoopInstructionBlocks"" />
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>iterate</KeywordDecoration>
                        <InsertDecoration CollectionName=""IterationInstructionBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""IterationInstructionBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>invariant</KeywordDecoration>
                        <InsertDecoration CollectionName=""InvariantBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""InvariantBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration RightMargin=""Whitespace"">variant</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""Variant"" />
                </FrameHorizontalPanelFrame>
            </FrameVerticalPanelFrame>
            <KeywordDecoration>end</KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IIfThenElseInstruction}"">
        <FrameVerticalPanelFrame>
            <VerticalCollectionDecoration PropertyName=""ConditionalBlocks"" />
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>else</KeywordDecoration>
                    <InsertDecoration CollectionName=""ElseInstructions.InstructionBlocks"" />
                </FrameHorizontalPanelFrame>
                <FramePlaceholderFrame PropertyName=""ElseInstructions"" />
            </FrameVerticalPanelFrame>
            <KeywordDecoration>end</KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IIndexAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""Destination"" RightMargin=""ThinSpace"" />
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftBracket"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
                <HorizontalCollectionDecoration PropertyName=""ArgumentBlocks"" />
                <SymbolDecoration Symbol=""RightBracket"" LeftMargin=""ThinSpace""/>
            </FrameHorizontalPanelFrame>
            <SymbolDecoration Symbol=""LeftArrow"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IInspectInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <KeywordDecoration RightMargin=""Whitespace"">inspect</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""Source"" />
                <InsertDecoration CollectionName=""WithBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <VerticalCollectionDecoration PropertyName=""WithBlocks"" />
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>else</KeywordDecoration>
                        <InsertDecoration CollectionName=""ElseInstructions.InstructionBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <FramePlaceholderFrame PropertyName=""ElseInstructions"" />
                </FrameVerticalPanelFrame>
            </FrameVerticalPanelFrame>
            <KeywordDecoration>end</KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type IKeywordAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <DiscreteDecoration PropertyName=""Destination"">
                <KeywordDecoration>True</KeywordDecoration>
                <KeywordDecoration>False</KeywordDecoration>
                <KeywordDecoration>Current</KeywordDecoration>
                <KeywordDecoration>Value</KeywordDecoration>
                <KeywordDecoration>Result</KeywordDecoration>
                <KeywordDecoration>Retry</KeywordDecoration>
                <KeywordDecoration>Exception</KeywordDecoration>
            </DiscreteDecoration>
            <SymbolDecoration Symbol=""LeftArrow"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IOverLoopInstruction}"">
        <FrameVerticalPanelFrame>
            <FrameHorizontalPanelFrame>
                <KeywordDecoration RightMargin=""Whitespace"">over</KeywordDecoration>
                <FramePlaceholderFrame PropertyName=""OverList"" RightMargin=""Whitespace"" />
                <KeywordDecoration RightMargin=""Whitespace"">for each</KeywordDecoration>
                <HorizontalCollectionDecoration PropertyName=""IndexerBlocks"" />
                <DiscreteDecoration PropertyName=""Iteration"" LeftMargin=""Whitespace"">
                    <KeywordDecoration>Single</KeywordDecoration>
                    <KeywordDecoration>Nested</KeywordDecoration>
                </DiscreteDecoration>
                <InsertDecoration CollectionName=""LoopInstructions.InstructionBlocks"" />
            </FrameHorizontalPanelFrame>
            <FrameVerticalPanelFrame HasTabulationMargin=""True"">
                <FramePlaceholderFrame PropertyName=""LoopInstructions"" />
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>exit if</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""ExitEntityName"" IdentifierType=""Feature"" LeftMargin=""Whitespace"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>invariant</KeywordDecoration>
                        <InsertDecoration CollectionName=""InvariantBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""InvariantBlocks"" />
                </FrameVerticalPanelFrame>
            </FrameVerticalPanelFrame>
            <KeywordDecoration>end</KeywordDecoration>
        </FrameVerticalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IPrecursorIndexAssignmentInstruction}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration>precursor</KeywordDecoration>
            <FrameHorizontalPanelFrame>
                <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""ThinSpace"">from</KeywordDecoration>
                <SymbolDecoration Symbol=""LeftCurlyBracket"" RightMargin=""ThinSpace""/>
                <SymbolDecoration Symbol=""RightBracket"" RightMargin=""Whitespace""/>
                <FramePlaceholderFrame PropertyName=""AncestorType"" />
            </FrameHorizontalPanelFrame>
            <SymbolDecoration Symbol=""LeftBracket"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
            <HorizontalCollectionDecoration PropertyName=""ArgumentBlocks"" />
            <SymbolDecoration Symbol=""RightBracket"" LeftMargin=""ThinSpace""/>
            <SymbolDecoration Symbol=""LeftArrow"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IPrecursorInstruction}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration IsFocusable=""True"">precursor</KeywordDecoration>
            <FrameHorizontalPanelFrame>
                <KeywordDecoration LeftMargin=""Whitespace"" RightMargin=""ThinSpace"">from</KeywordDecoration>
                <SymbolDecoration Symbol=""LeftCurlyBracket"" RightMargin=""ThinSpace""/>
                <FramePlaceholderFrame PropertyName=""AncestorType"" />
                <SymbolDecoration Symbol=""RightCurlyBracket"" RightMargin=""Whitespace""/>
            </FrameHorizontalPanelFrame>
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
                <HorizontalCollectionDecoration PropertyName=""ArgumentBlocks"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace""/>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IRaiseEventInstruction}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration RightMargin=""Whitespace"">raise</KeywordDecoration>
            <FramePlaceholderFrame PropertyName=""QueryIdentifier"" IdentifierType=""Feature"" />
            <DiscreteDecoration PropertyName=""Event"" LeftMargin=""Whitespace"">
                <KeywordDecoration>once</KeywordDecoration>
                <KeywordDecoration>forever</KeywordDecoration>
            </DiscreteDecoration>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IReleaseInstruction}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration>release</KeywordDecoration>
            <FramePlaceholderFrame PropertyName=""EntityName"" LeftMargin=""Whitespace"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IThrowInstruction}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration RightMargin=""Whitespace"">throw</KeywordDecoration>
            <FramePlaceholderFrame PropertyName=""ExceptionType"" RightMargin=""Whitespace"" />
            <KeywordDecoration RightMargin=""Whitespace"">with</KeywordDecoration>
            <FramePlaceholderFrame PropertyName=""CreationRoutine"" IdentifierType=""Feature"" />
            <FrameHorizontalPanelFrame>
                <SymbolDecoration Symbol=""LeftParenthesis"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
                <HorizontalCollectionDecoration PropertyName=""ArgumentBlocks"" />
                <SymbolDecoration Symbol=""RightParenthesis"" LeftMargin=""ThinSpace""/>
            </FrameHorizontalPanelFrame>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>

    <FrameNodeTemplate NodeType=""{x:Type IAnchoredType}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration RightMargin=""Whitespace"">like</KeywordDecoration>
            <DiscreteDecoration PropertyName=""AnchorKind"" RightMargin=""Whitespace"">
                <KeywordDecoration>declaration</KeywordDecoration>
                <KeywordDecoration>creation</KeywordDecoration>
            </DiscreteDecoration>
            <FramePlaceholderFrame PropertyName=""AnchoredName"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IFunctionType}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration RightMargin=""Whitespace"">function</KeywordDecoration>
            <FramePlaceholderFrame PropertyName=""BaseType"" RightMargin=""Whitespace"" />
            <HorizontalCollectionDecoration PropertyName=""OverloadBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IGenericType}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" IdentifierType=""Class"" />
            <SymbolDecoration Symbol=""LeftBracket"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
            <HorizontalCollectionDecoration PropertyName=""TypeArgumentBlocks"" />
            <SymbolDecoration Symbol=""RightBracket"" LeftMargin=""ThinSpace""/>
            <DiscreteDecoration PropertyName=""Sharing"" LeftMargin=""Whitespace"">
                <KeywordDecoration>not shared</KeywordDecoration>
                <KeywordDecoration>readwrite</KeywordDecoration>
                <KeywordDecoration>read-only</KeywordDecoration>
                <KeywordDecoration>write-only</KeywordDecoration>
            </DiscreteDecoration>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IIndexerType}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""BaseType"" />
            <SymbolDecoration Symbol=""LeftBracket"" LeftMargin=""Whitespace"" RightMargin=""ThinSpace""/>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>indexer</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""EntityType"" LeftMargin=""Whitespace"" />
                </FrameHorizontalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>parameter</KeywordDecoration>
                        <DiscreteDecoration PropertyName=""ParameterEnd"" LeftMargin=""Whitespace"">
                            <KeywordDecoration>closed</KeywordDecoration>
                            <KeywordDecoration>open</KeywordDecoration>
                        </DiscreteDecoration>
                        <InsertDecoration CollectionName=""IndexParameterBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""IndexParameterBlocks"" />
                </FrameVerticalPanelFrame>
                <DiscreteDecoration PropertyName=""IndexerKind"">
                    <KeywordDecoration>read-only</KeywordDecoration>
                    <KeywordDecoration>write-only</KeywordDecoration>
                    <KeywordDecoration>readwrite</KeywordDecoration>
                </DiscreteDecoration>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>getter require</KeywordDecoration>
                        <InsertDecoration CollectionName=""GetRequireBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""GetRequireBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>getter ensure</KeywordDecoration>
                        <InsertDecoration CollectionName=""GetEnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""GetEnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>getter exception</KeywordDecoration>
                        <InsertDecoration CollectionName=""GetExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""GetExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>setter require</KeywordDecoration>
                        <InsertDecoration CollectionName=""SetRequireBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""SetRequireBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>setter ensure</KeywordDecoration>
                        <InsertDecoration CollectionName=""SetEnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""SetEnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>setter exception</KeywordDecoration>
                        <InsertDecoration CollectionName=""SetExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""SetExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <KeywordDecoration>end</KeywordDecoration>
            </FrameVerticalPanelFrame>
            <SymbolDecoration Symbol=""RightBracket"" LeftMargin=""ThinSpace""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IKeywordAnchoredType}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration RightMargin=""Whitespace"">like</KeywordDecoration>
            <DiscreteDecoration PropertyName=""Anchor"">
                <KeywordDecoration>True</KeywordDecoration>
                <KeywordDecoration>False</KeywordDecoration>
                <KeywordDecoration>Current</KeywordDecoration>
                <KeywordDecoration>Value</KeywordDecoration>
                <KeywordDecoration>Result</KeywordDecoration>
                <KeywordDecoration>Retry</KeywordDecoration>
                <KeywordDecoration>Exception</KeywordDecoration>
            </DiscreteDecoration>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IProcedureType}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration RightMargin=""Whitespace"">procedure</KeywordDecoration>
            <FramePlaceholderFrame PropertyName=""BaseType"" RightMargin=""Whitespace"" />
            <HorizontalCollectionDecoration PropertyName=""OverloadBlocks"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IPropertyType}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""BaseType"" />
            <SymbolDecoration Symbol=""LeftBracket"" LeftMargin=""Whitespace"" RightMargin=""ThinSpace""/>
            <FrameVerticalPanelFrame>
                <FrameHorizontalPanelFrame>
                    <KeywordDecoration>is</KeywordDecoration>
                    <FramePlaceholderFrame PropertyName=""EntityType"" LeftMargin=""Whitespace"" />
                </FrameHorizontalPanelFrame>
                <DiscreteDecoration PropertyName=""PropertyKind"">
                    <KeywordDecoration>read-only</KeywordDecoration>
                    <KeywordDecoration>write-only</KeywordDecoration>
                    <KeywordDecoration>readwrite</KeywordDecoration>
                </DiscreteDecoration>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>getter ensure</KeywordDecoration>
                        <InsertDecoration CollectionName=""GetEnsureBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""GetEnsureBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>getter exception</KeywordDecoration>
                        <InsertDecoration CollectionName=""GetExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""GetExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>setter require</KeywordDecoration>
                        <InsertDecoration CollectionName=""SetRequireBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""SetRequireBlocks"" />
                </FrameVerticalPanelFrame>
                <FrameVerticalPanelFrame>
                    <FrameHorizontalPanelFrame>
                        <KeywordDecoration>setter exception</KeywordDecoration>
                        <InsertDecoration CollectionName=""SetExceptionIdentifierBlocks"" />
                    </FrameHorizontalPanelFrame>
                    <VerticalCollectionDecoration PropertyName=""SetExceptionIdentifierBlocks"" />
                </FrameVerticalPanelFrame>
                <KeywordDecoration>end</KeywordDecoration>
            </FrameVerticalPanelFrame>
            <SymbolDecoration Symbol=""RightBracket"" LeftMargin=""ThinSpace""/>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    ﻿<FrameNodeTemplate NodeType=""{x:Type ISimpleType}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ClassIdentifier"" IdentifierType=""Type"" />
            <DiscreteDecoration PropertyName=""Sharing"" LeftMargin=""Whitespace"">
                <KeywordDecoration>not shared</KeywordDecoration>
                <KeywordDecoration>readwrite</KeywordDecoration>
                <KeywordDecoration>read-only</KeywordDecoration>
                <KeywordDecoration>write-only</KeywordDecoration>
            </DiscreteDecoration>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type ITupleType}"">
        <FrameHorizontalPanelFrame>
            <KeywordDecoration>tuple</KeywordDecoration>
            <SymbolDecoration Symbol=""LeftBracket"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
            <VerticalCollectionDecoration PropertyName=""EntityDeclarationBlocks"" />
            <SymbolDecoration Symbol=""RightBracket"" LeftMargin=""ThinSpace""/>
            <DiscreteDecoration PropertyName=""Sharing"" LeftMargin=""Whitespace"">
                <KeywordDecoration>not shared</KeywordDecoration>
                <KeywordDecoration>readwrite</KeywordDecoration>
                <KeywordDecoration>read-only</KeywordDecoration>
                <KeywordDecoration>write-only</KeywordDecoration>
            </DiscreteDecoration>
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>

    <FrameNodeTemplate NodeType=""{x:Type IAssignmentTypeArgument}"">
        <FrameHorizontalPanelFrame>
            <FramePlaceholderFrame PropertyName=""ParameterIdentifier"" IdentifierType=""Feature"" />
            <SymbolDecoration Symbol=""LeftArrow"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
            <FramePlaceholderFrame PropertyName=""Source"" />
        </FrameHorizontalPanelFrame>
    </FrameNodeTemplate>
    <FrameNodeTemplate NodeType=""{x:Type IPositionalTypeArgument}"">
        <FramePlaceholderFrame PropertyName=""Source"" />
    </FrameNodeTemplate>
</FrameTemplateList>";
        #endregion
    }
}
