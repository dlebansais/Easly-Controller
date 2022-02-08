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
#if FRAME
        [Test]
        [TestCaseSource(nameof(FileIndexRange))]
        public static void Frame(int index)
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

            TestFrame(index, FileName, RootNode);
        }
#endif

        public static void TestFrame(int index, string name, Node rootNode)
        {
            ControllerTools.ResetExpectedName();

            TestFrameStats(index, name, rootNode, out Stats Stats);

            Random rand = new Random(0x123456);

            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);

            FrameControllerView ControllerView;

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

        public static void TestFrameCellViewList(FrameControllerView controllerView, string name)
        {
            FrameVisibleCellViewList CellViewList = new FrameVisibleCellViewList();
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
                Node ChildNode = CellView.StateView.State.Node;
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

        public static void TestFrameCanonicalize(Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            Controller.Canonicalize(out bool IsChanged);

            FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController NewController = FrameController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        static int FrameTestCount = 0;
        static int FrameMaxTestCount = 0;

        public static bool JustCount(IFrameInner inner)
        {
            FrameTestCount++;
            return true;
        }

        public static void TestFrameStats(int index, string name, Node rootNode, out Stats stats)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);

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

        public static void TestFrameInsert(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => InsertAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool InsertAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    Node NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
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

                    Node NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    if (RandNext(2) == 0)
                    {
                        int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        IFrameBlockState BlockState = (IFrameBlockState)AsBlockListInner.BlockStateList[BlockIndex];
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

                        Pattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                        Identifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
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
                FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                FrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameReplace(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => ReplaceAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool ReplaceAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFramePlaceholderInner AsPlaceholderInner)
            {
                Node NewNode = NodeHelper.DeepCloneNode(AsPlaceholderInner.ChildState.Node, cloneCommentGuid: false);
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
                Node NewNode = NodeHelper.CreateDefaultFromType(NodeInterfaceType);
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
                    Node NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
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

                    Node NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IFrameBlockState BlockState = (IFrameBlockState)AsBlockListInner.BlockStateList[BlockIndex];
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
                FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                FrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameRemove(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => RemoveAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool RemoveAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int Index = RandNext(AsListInner.StateList.Count);
                    IFrameNodeState ChildState = (IFrameNodeState)AsListInner.StateList[Index];
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
                    IFrameBlockState BlockState = (IFrameBlockState)AsBlockListInner.BlockStateList[BlockIndex];
                    int Index = RandNext(BlockState.StateList.Count);
                    IFrameNodeState ChildState = (IFrameNodeState)BlockState.StateList[Index];
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
                FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                FrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameRemoveBlockRange(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => RemoveBlockRangeAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool RemoveBlockRangeAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

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
                FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                FrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameReplaceBlockRange(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => ReplaceBlockRangeAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool ReplaceBlockRangeAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
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
                FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                FrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameInsertBlockRange(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => InsertBlockRangeAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool InsertBlockRangeAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int InsertedIndex = RandNext(AsBlockListInner.BlockStateList.Count);

                    Node NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    Pattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                    Identifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
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
                FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                FrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameRemoveNodeRange(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => RemoveNodeRangeAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool RemoveNodeRangeAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

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
                FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                FrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameReplaceNodeRange(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => ReplaceNodeRangeAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool ReplaceNodeRangeAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
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

                    Node NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
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
                FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                FrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameInsertNodeRange(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => InsertNodeRangeAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool InsertNodeRangeAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int InsertedNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count);

                    Node NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
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

                    Node NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    IFrameInsertionListNodeIndex ExistingNodeIndex = new FrameInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, InsertedNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    Controller.InsertNodeRange(AsListInner, BlockIndex, InsertedNodeIndex, IndexList);
                    IsModified = true;
                }
            }

            if (IsModified)
            {
                FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                FrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameAssign(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => AssignAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool AssignAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameOptionalInner AsOptionalInner)
            {
                IFrameOptionalNodeState ChildState = AsOptionalInner.ChildState;
                Assert.That(ChildState != null);

                IFrameBrowsingOptionalNodeIndex OptionalIndex = ChildState.ParentIndex;
                Assert.That(Controller.Contains(OptionalIndex));

                IOptionalReference Optional = OptionalIndex.Optional;
                Assert.That(Optional != null);

                Controller.Assign(OptionalIndex, out bool IsChanged);
                Assert.That(Optional.IsAssigned);
                Assert.That(AsOptionalInner.IsAssigned);
                Assert.That(Optional.Item == ChildState.Node);

                FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                FrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                if (IsChanged)
                {
                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestFrameUnassign(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => UnassignAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool UnassignAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

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

                FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                FrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                if (IsChanged)
                {
                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestFrameChangeReplication(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => ChangeReplicationAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool ChangeReplicationAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IFrameBlockState BlockState = (IFrameBlockState)AsBlockListInner.BlockStateList[BlockIndex];

                    ReplicationStatus Replication = (ReplicationStatus)RandNext(2);
                    Controller.ChangeReplication(AsBlockListInner, BlockIndex, Replication);

                    FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                    FrameController NewController = FrameController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestFrameSplit(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => SplitAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool SplitAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int SplitBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    IFrameBlockState BlockState = (IFrameBlockState)AsBlockListInner.BlockStateList[SplitBlockIndex];
                    if (BlockState.StateList.Count > 1)
                    {
                        int SplitIndex = 1 + RandNext(BlockState.StateList.Count - 1);
                        IFrameBrowsingExistingBlockNodeIndex NodeIndex = (IFrameBrowsingExistingBlockNodeIndex)AsBlockListInner.IndexAt(SplitBlockIndex, SplitIndex);
                        Controller.SplitBlock(AsBlockListInner, NodeIndex);

                        FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                        Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                        IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                        FrameController NewController = FrameController.Create(NewRootIndex);
                        Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                        Controller.Undo();

                        Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                        FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                        Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                        Controller.Redo();

                        Assert.That(AsBlockListInner.BlockStateList.Count > 0);
                        int OldBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        int NewBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        int Direction = NewBlockIndex - OldBlockIndex;

                        Assert.That(Controller.IsBlockMoveable(AsBlockListInner, OldBlockIndex, Direction));

                        Controller.MoveBlock(AsBlockListInner, OldBlockIndex, Direction);

                        FrameControllerView NewViewAfterMove = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                        Assert.That(NewViewAfterMove.IsEqual(CompareEqual.New(), controllerView));

                        IFrameRootNodeIndex NewRootIndexAfterMove = new FrameRootNodeIndex(Controller.RootIndex.Node);
                        FrameController NewControllerAfterMove = FrameController.Create(NewRootIndexAfterMove);
                        Assert.That(NewControllerAfterMove.IsEqual(CompareEqual.New(), Controller));
                    }
                }
            }

            return false;
        }

        public static void TestFrameMerge(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => MergeAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool MergeAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFrameBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 1)
                {
                    int MergeBlockIndex = 1 + RandNext(AsBlockListInner.BlockStateList.Count - 1);
                    IFrameBlockState BlockState = (IFrameBlockState)AsBlockListInner.BlockStateList[MergeBlockIndex];

                    IFrameBrowsingExistingBlockNodeIndex NodeIndex = (IFrameBrowsingExistingBlockNodeIndex)AsBlockListInner.IndexAt(MergeBlockIndex, 0);
                    Controller.MergeBlocks(AsBlockListInner, NodeIndex);

                    FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                    FrameController NewController = FrameController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestFrameMove(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => MoveAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool MoveAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;
            bool IsModified = false;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

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
                    IFrameBlockState BlockState = (IFrameBlockState)AsBlockListInner.BlockStateList[BlockIndex];

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
                FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
                FrameController NewController = FrameController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestFrameExpand(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => ExpandAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool ExpandAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;
            IFrameNodeIndex NodeIndex;
            IFramePlaceholderNodeState State;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFramePlaceholderInner AsPlaceholderInner)
            {
                NodeIndex = AsPlaceholderInner.ChildState.ParentIndex as IFrameNodeIndex;
                Assert.That(NodeIndex != null);

                State = Controller.IndexToState(NodeIndex) as IFramePlaceholderNodeState;
                Assert.That(State != null);

                NodeTreeHelper.GetArgumentBlocks(State.Node, out IDictionary<string, IBlockList<Argument>> ArgumentBlocksTable);
                if (ArgumentBlocksTable.Count == 0)
                    return true;
            }
            else
                return true;

            Controller.Expand(NodeIndex, out bool IsChanged);

            FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController NewController = FrameController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
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

        public static void TestFrameReduce(int index, Node rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            FrameController Controller = FrameController.Create(RootIndex);
            FrameControllerView ControllerView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);

            FrameTestCount = 0;
            FrameBrowseNode(Controller, RootIndex, (IFrameInner inner) => ReduceAndCompare(ControllerView, RandNext(FrameMaxTestCount), inner));
        }

        static bool ReduceAndCompare(FrameControllerView controllerView, int TestIndex, IFrameInner inner)
        {
            if (FrameTestCount++ < TestIndex)
                return true;

            FrameController Controller = controllerView.Controller;
            IFrameNodeIndex NodeIndex;
            IFramePlaceholderNodeState State;

            IFrameRootNodeIndex OldRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController OldController = FrameController.Create(OldRootIndex);

            if (inner is IFramePlaceholderInner AsPlaceholderInner)
            {
                NodeIndex = AsPlaceholderInner.ChildState.ParentIndex as IFrameNodeIndex;
                Assert.That(NodeIndex != null);

                State = Controller.IndexToState(NodeIndex) as IFramePlaceholderNodeState;
                Assert.That(State != null);

                NodeTreeHelper.GetArgumentBlocks(State.Node, out IDictionary<string, IBlockList<Argument>> ArgumentBlocksTable);
                if (ArgumentBlocksTable.Count == 0)
                    return true;
            }
            else
                return true;

            Controller.Reduce(NodeIndex, out bool IsChanged);

            FrameControllerView NewView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            IFrameRootNodeIndex NewRootIndex = new FrameRootNodeIndex(Controller.RootIndex.Node);
            FrameController NewController = FrameController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                FrameControllerView OldView = FrameControllerView.Create(Controller, FrameTemplateSet.Default);
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

        static bool FrameBrowseNode(FrameController controller, IFrameIndex index, Func<IFrameInner, bool> test)
        {
            Assert.That(index != null, "Frame #0");
            Assert.That(controller.Contains(index), "Frame #1");
            IFrameNodeState State = (IFrameNodeState)controller.IndexToState(index);
            Assert.That(State != null, "Frame #2");
            Assert.That(State.ParentIndex == index, "Frame #4");

            Node Node;

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
                    NodeTreeHelperOptional.GetChildNode(Node, PropertyName, out bool IsAssigned, out Node ChildNode);
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
                        IFramePlaceholderNodeState ChildState = (IFramePlaceholderNodeState)Inner.StateList[i];
                        IFrameIndex ChildIndex = ChildState.ParentIndex;
                        if (!FrameBrowseNode(controller, ChildIndex, test))
                            return false;
                    }
                }

                else if (NodeTreeHelperBlockList.IsBlockListProperty(Node, PropertyName, /*out Type ChildInterfaceType,*/ out ChildNodeType))
                {
                    IFrameBlockListInner Inner = (IFrameBlockListInner)State.PropertyToInner(PropertyName);
                    if (!test(Inner))
                        return false;

                    for (int BlockIndex = 0; BlockIndex < Inner.BlockStateList.Count; BlockIndex++)
                    {
                        IFrameBlockState BlockState = (IFrameBlockState)Inner.BlockStateList[BlockIndex];
                        if (!FrameBrowseNode(controller, BlockState.PatternIndex, test))
                            return false;
                        if (!FrameBrowseNode(controller, BlockState.SourceIndex, test))
                            return false;

                        for (int i = 0; i < BlockState.StateList.Count; i++)
                        {
                            IFramePlaceholderNodeState ChildState = (IFramePlaceholderNodeState)BlockState.StateList[i];
                            IFrameIndex ChildIndex = ChildState.ParentIndex;
                            if (!FrameBrowseNode(controller, ChildIndex, test))
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        private static bool ListCellViews(IFrameVisibleCellView cellview, FrameVisibleCellViewList cellViewList)
        {
            cellViewList.Add(cellview);
            return false;
        }
    }
}
