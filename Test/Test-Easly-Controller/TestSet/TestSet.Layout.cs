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
#if LAYOUT
        [Test]
        [TestCaseSource(nameof(FileIndexRange))]
        public static void Layout(int index)
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

            TestLayout(index, FileName, RootNode);
        }
#endif

        public static void TestLayout(int index, string name, Node rootNode)
        {
            ControllerTools.ResetExpectedName();

            TestLayoutStats(index, name, rootNode, out Stats Stats);

            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);

            LayoutControllerView ControllerView;

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

        public static void TestLayoutCellViewList(LayoutControllerView controllerView, string name)
        {
            LayoutVisibleCellViewList CellViewList = new LayoutVisibleCellViewList();
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
                Node ChildNode = CellView.StateView.State.Node;
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

        public static void TestLayoutStats(int index, string name, Node rootNode, out Stats stats)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);

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

        public static void TestLayoutInsert(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => InsertAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool InsertAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            LayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    Node NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
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

                    Node NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    if (RandNext(2) == 0)
                    {
                        int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        ILayoutBlockState BlockState = (ILayoutBlockState)AsBlockListInner.BlockStateList[BlockIndex];
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

                        Pattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                        Identifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
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
                LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                LayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutReplace(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => ReplaceAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool ReplaceAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            LayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutPlaceholderInner AsPlaceholderInner)
            {
                Node NewNode = NodeHelper.DeepCloneNode(AsPlaceholderInner.ChildState.Node, cloneCommentGuid: false);
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
                Node NewNode = NodeHelper.CreateDefaultFromType(NodeInterfaceType);
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
                    Node NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
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

                    Node NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    ILayoutBlockState BlockState = (ILayoutBlockState)AsBlockListInner.BlockStateList[BlockIndex];
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
                LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                LayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutRemove(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => RemoveAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool RemoveAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            LayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutListInner AsListInner)
            {
                if (AsListInner.StateList.Count > 0)
                {
                    int Index = RandNext(AsListInner.StateList.Count);
                    ILayoutNodeState ChildState = (ILayoutNodeState)AsListInner.StateList[Index];
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
                    ILayoutBlockState BlockState = (ILayoutBlockState)AsBlockListInner.BlockStateList[BlockIndex];
                    int Index = RandNext(BlockState.StateList.Count);
                    ILayoutNodeState ChildState = (ILayoutNodeState)BlockState.StateList[Index];
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
                LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                LayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutRemoveBlockRange(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => RemoveBlockRangeAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool RemoveBlockRangeAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            LayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

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
                LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                LayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutReplaceBlockRange(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => ReplaceBlockRangeAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool ReplaceBlockRangeAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            LayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
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
                LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                LayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutInsertBlockRange(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => InsertBlockRangeAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool InsertBlockRangeAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            LayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int InsertedIndex = RandNext(AsBlockListInner.BlockStateList.Count);

                    Node NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsBlockListInner.InterfaceType}");

                    Pattern ReplicationPattern = NodeHelper.CreateSimplePattern("x");
                    Identifier SourceIdentifier = NodeHelper.CreateSimpleIdentifier("y");
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
                LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                LayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutRemoveNodeRange(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => RemoveNodeRangeAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool RemoveNodeRangeAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            LayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

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
                LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                LayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutReplaceNodeRange(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => ReplaceNodeRangeAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool ReplaceNodeRangeAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            LayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
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

                    Node NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
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
                LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                LayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutInsertNodeRange(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => InsertNodeRangeAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool InsertNodeRangeAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            LayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    int InsertedNodeIndex = RandNext(AsBlockListInner.BlockStateList[BlockIndex].StateList.Count);

                    Node NewNode = NodeHelper.DeepCloneNode(AsBlockListInner.BlockStateList[0].StateList[0].Node, cloneCommentGuid: false);
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

                    Node NewNode = NodeHelper.DeepCloneNode(AsListInner.StateList[0].Node, cloneCommentGuid: false);
                    Assert.That(NewNode != null, $"Type: {AsListInner.InterfaceType}");

                    ILayoutInsertionListNodeIndex ExistingNodeIndex = new LayoutInsertionListNodeIndex(AsListInner.Owner.Node, AsListInner.PropertyName, NewNode, InsertedNodeIndex);
                    List<IWriteableInsertionCollectionNodeIndex> IndexList = new List<IWriteableInsertionCollectionNodeIndex>() { ExistingNodeIndex };

                    Controller.InsertNodeRange(AsListInner, BlockIndex, InsertedNodeIndex, IndexList);
                    IsModified = true;
                }
            }

            if (IsModified)
            {
                LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                LayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutAssign(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => AssignAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool AssignAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            LayoutController Controller = controllerView.Controller;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutOptionalInner AsOptionalInner)
            {
                ILayoutOptionalNodeState ChildState = AsOptionalInner.ChildState;
                Assert.That(ChildState != null);

                ILayoutBrowsingOptionalNodeIndex OptionalIndex = ChildState.ParentIndex;
                Assert.That(Controller.Contains(OptionalIndex));

                IOptionalReference Optional = OptionalIndex.Optional;
                Assert.That(Optional != null);

                Controller.Assign(OptionalIndex, out bool IsChanged);
                Assert.That(Optional.IsAssigned);
                Assert.That(AsOptionalInner.IsAssigned);
                Assert.That(Optional.Item == ChildState.Node);

                LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                LayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                if (IsChanged)
                {
                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestLayoutUnassign(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => UnassignAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool UnassignAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            LayoutController Controller = controllerView.Controller;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

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

                LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                LayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                if (IsChanged)
                {
                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestLayoutChangeReplication(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => ChangeReplicationAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool ChangeReplicationAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            LayoutController Controller = controllerView.Controller;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int BlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    ILayoutBlockState BlockState = (ILayoutBlockState)AsBlockListInner.BlockStateList[BlockIndex];

                    ReplicationStatus Replication = (ReplicationStatus)RandNext(2);
                    Controller.ChangeReplication(AsBlockListInner, BlockIndex, Replication);

                    LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                    LayoutController NewController = LayoutController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestLayoutSplit(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => SplitAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool SplitAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            LayoutController Controller = controllerView.Controller;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 0)
                {
                    Assert.That(AsBlockListInner.BlockStateList[0].StateList.Count > 0);

                    int SplitBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                    ILayoutBlockState BlockState = (ILayoutBlockState)AsBlockListInner.BlockStateList[SplitBlockIndex];
                    if (BlockState.StateList.Count > 1)
                    {
                        int SplitIndex = 1 + RandNext(BlockState.StateList.Count - 1);
                        ILayoutBrowsingExistingBlockNodeIndex NodeIndex = (ILayoutBrowsingExistingBlockNodeIndex)AsBlockListInner.IndexAt(SplitBlockIndex, SplitIndex);
                        Controller.SplitBlock(AsBlockListInner, NodeIndex);

                        LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                        Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                        ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                        LayoutController NewController = LayoutController.Create(NewRootIndex);
                        Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                        Controller.Undo();

                        Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                        LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                        Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));

                        Assert.That(AsBlockListInner.BlockStateList.Count > 0);
                        int OldBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        int NewBlockIndex = RandNext(AsBlockListInner.BlockStateList.Count);
                        int Direction = NewBlockIndex - OldBlockIndex;

                        Assert.That(Controller.IsBlockMoveable(AsBlockListInner, OldBlockIndex, Direction));

                        Controller.MoveBlock(AsBlockListInner, OldBlockIndex, Direction);

                        LayoutControllerView NewViewAfterMove = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                        Assert.That(NewViewAfterMove.IsEqual(CompareEqual.New(), controllerView));

                        ILayoutRootNodeIndex NewRootIndexAfterMove = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                        LayoutController NewControllerAfterMove = LayoutController.Create(NewRootIndexAfterMove);
                        Assert.That(NewControllerAfterMove.IsEqual(CompareEqual.New(), Controller));
                    }
                }
            }

            return false;
        }

        public static void TestLayoutMerge(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => MergeAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool MergeAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            LayoutController Controller = controllerView.Controller;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutBlockListInner AsBlockListInner)
            {
                if (AsBlockListInner.BlockStateList.Count > 1)
                {
                    int MergeBlockIndex = 1 + RandNext(AsBlockListInner.BlockStateList.Count - 1);
                    ILayoutBlockState BlockState = (ILayoutBlockState)AsBlockListInner.BlockStateList[MergeBlockIndex];

                    ILayoutBrowsingExistingBlockNodeIndex NodeIndex = (ILayoutBrowsingExistingBlockNodeIndex)AsBlockListInner.IndexAt(MergeBlockIndex, 0);
                    Controller.MergeBlocks(AsBlockListInner, NodeIndex);

                    LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                    Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                    ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                    LayoutController NewController = LayoutController.Create(NewRootIndex);
                    Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                    Controller.Undo();

                    Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                    LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                    Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
                }
            }

            return false;
        }

        public static void TestLayoutMove(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => MoveAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool MoveAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            LayoutController Controller = controllerView.Controller;
            bool IsModified = false;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

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
                    ILayoutBlockState BlockState = (ILayoutBlockState)AsBlockListInner.BlockStateList[BlockIndex];

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
                LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

                ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
                LayoutController NewController = LayoutController.Create(NewRootIndex);
                Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), controllerView));
            }

            return false;
        }

        public static void TestLayoutExpand(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => ExpandAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool ExpandAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            LayoutController Controller = controllerView.Controller;
            ILayoutNodeIndex NodeIndex;
            ILayoutPlaceholderNodeState State;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutPlaceholderInner AsPlaceholderInner)
            {
                NodeIndex = AsPlaceholderInner.ChildState.ParentIndex as ILayoutNodeIndex;
                Assert.That(NodeIndex != null);

                State = Controller.IndexToState(NodeIndex) as ILayoutPlaceholderNodeState;
                Assert.That(State != null);

                NodeTreeHelper.GetArgumentBlocks(State.Node, out IDictionary<string, IBlockList<Argument>> ArgumentBlocksTable);
                if (ArgumentBlocksTable.Count == 0)
                    return true;
            }
            else
                return true;

            Controller.Expand(NodeIndex, out bool IsChanged);

            LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
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

        public static void TestLayoutReduce(int index, Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            LayoutTestCount = 0;
            LayoutBrowseNode(Controller, RootIndex, (ILayoutInner inner) => ReduceAndCompare(ControllerView, RandNext(LayoutMaxTestCount), inner));
        }

        static bool ReduceAndCompare(LayoutControllerView controllerView, int TestIndex, ILayoutInner inner)
        {
            if (LayoutTestCount++ < TestIndex)
                return true;

            MoveLayoutRandomly(controllerView);

            LayoutController Controller = controllerView.Controller;
            ILayoutNodeIndex NodeIndex;
            ILayoutPlaceholderNodeState State;

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

            if (inner is ILayoutPlaceholderInner AsPlaceholderInner)
            {
                NodeIndex = AsPlaceholderInner.ChildState.ParentIndex as ILayoutNodeIndex;
                Assert.That(NodeIndex != null);

                State = Controller.IndexToState(NodeIndex) as ILayoutPlaceholderNodeState;
                Assert.That(State != null);

                NodeTreeHelper.GetArgumentBlocks(State.Node, out IDictionary<string, IBlockList<Argument>> ArgumentBlocksTable);
                if (ArgumentBlocksTable.Count == 0)
                    return true;
            }
            else
                return true;

            Controller.Reduce(NodeIndex, out bool IsChanged);

            LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), controllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"Inner: {inner.PropertyName}, Owner: {inner.Owner.Node}");
                LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
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

        public static void TestLayoutCanonicalize(Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);

            ILayoutRootNodeIndex OldRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController OldController = LayoutController.Create(OldRootIndex);

            Controller.Canonicalize(out bool IsChanged);

            LayoutControllerView NewView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));

            if (IsChanged)
            {
                Controller.Undo();

                Assert.That(OldController.IsEqual(CompareEqual.New(), Controller), $"RootNode: {rootNode}");
                LayoutControllerView OldView = LayoutControllerView.Create(Controller, LayoutTemplateSet.Default, LayoutDrawPrintContext.Default);
                Assert.That(OldView.IsEqual(CompareEqual.New(), ControllerView));
            }
        }

        public static void LayoutTestNewItemInsertable(Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

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

            LayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestItemRemoveable(Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemRemoveable(out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index))
                    Controller.Remove(inner, index);
            }

            LayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestItemMoveable(Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

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

            LayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestBlockMoveable(Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

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

            LayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestItemSplittable(Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemSplittable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index))
                    Controller.SplitBlock(inner, index);
            }

            LayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestItemMergeable(Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemMergeable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index))
                    Controller.MergeBlocks(inner, index);
            }

            LayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestItemCyclable(Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            for (int i = 0; i < 20; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsItemCyclableThrough(out IFocusCyclableNodeState state, out int cyclePosition))
                    Controller.Replace(state.ParentInner, state.CycleIndexList, cyclePosition, out IFocusBrowsingChildIndex nodeIndex);
            }

            LayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestItemSimplifiable(Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

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
            LayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));*/
        }
        static int total = 0;

        public static void LayoutTestItemComplexifiable(Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

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

        public static void LayoutTestIdentifierSplittable(Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            for (int i = 0; i < 200; i++)
            {
                int Min = ControllerView.MinFocusMove;
                int Max = ControllerView.MaxFocusMove;
                int Direction = RandNext(Max - Min + 1) + Min;

                ControllerView.MoveFocus(Direction, true, out bool IsMoved);

                if (ControllerView.IsIdentifierSplittable(out IFocusListInner Inner, out IFocusInsertionListNodeIndex ReplaceIndex, out IFocusInsertionListNodeIndex InsertIndex))
                    Controller.SplitIdentifier(Inner, ReplaceIndex, InsertIndex, out IWriteableBrowsingListNodeIndex FirstIndex, out IWriteableBrowsingListNodeIndex SecondIndex);
            }

            LayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);
            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        public static void LayoutTestReplicationModifiable(Node rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            LayoutController Controller = LayoutController.Create(RootIndex);
            LayoutControllerView ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

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

            LayoutControllerView NewView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, LayoutDrawPrintContext.Default);

            CompareEqual comparer = CompareEqual.New();
            bool IsEq = NewView.IsEqual(comparer, ControllerView);
            System.Diagnostics.Debug.Assert(IsEq);

            Assert.That(NewView.IsEqual(CompareEqual.New(), ControllerView));

            ILayoutRootNodeIndex NewRootIndex = new LayoutRootNodeIndex(Controller.RootIndex.Node);
            LayoutController NewController = LayoutController.Create(NewRootIndex);
            Assert.That(NewController.IsEqual(CompareEqual.New(), Controller));
        }

        static bool LayoutBrowseNode(LayoutController controller, ILayoutIndex index, Func<ILayoutInner, bool> test)
        {
            Assert.That(index != null, "Layout #0");
            Assert.That(controller.Contains(index), "Layout #1");
            ILayoutNodeState State = (ILayoutNodeState)controller.IndexToState(index);
            Assert.That(State != null, "Layout #2");
            Assert.That(State.ParentIndex == index, "Layout #4");

            Node Node;

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
                    NodeTreeHelperOptional.GetChildNode(Node, PropertyName, out bool IsAssigned, out Node ChildNode);
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
                        ILayoutPlaceholderNodeState ChildState = (ILayoutPlaceholderNodeState)Inner.StateList[i];
                        ILayoutIndex ChildIndex = ChildState.ParentIndex;
                        if (!LayoutBrowseNode(controller, ChildIndex, test))
                            return false;
                    }
                }

                else if (NodeTreeHelperBlockList.IsBlockListProperty(Node, PropertyName, /*out Type ChildInterfaceType,*/ out ChildNodeType))
                {
                    ILayoutBlockListInner Inner = (ILayoutBlockListInner)State.PropertyToInner(PropertyName);
                    if (!test(Inner))
                        return false;

                    for (int BlockIndex = 0; BlockIndex < Inner.BlockStateList.Count; BlockIndex++)
                    {
                        ILayoutBlockState BlockState = (ILayoutBlockState)Inner.BlockStateList[BlockIndex];
                        if (!LayoutBrowseNode(controller, BlockState.PatternIndex, test))
                            return false;
                        if (!LayoutBrowseNode(controller, BlockState.SourceIndex, test))
                            return false;

                        for (int i = 0; i < BlockState.StateList.Count; i++)
                        {
                            ILayoutPlaceholderNodeState ChildState = (ILayoutPlaceholderNodeState)BlockState.StateList[i];
                            ILayoutIndex ChildIndex = ChildState.ParentIndex;
                            if (!LayoutBrowseNode(controller, ChildIndex, test))
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        static void MoveLayoutRandomly(LayoutControllerView controllerView)
        {
            int MinFocusMove = controllerView.MinFocusMove;
            int MaxFocusMove = controllerView.MaxFocusMove;

            int Direction = MinFocusMove + RandNext(MaxFocusMove - MinFocusMove + 1);
            controllerView.MoveFocus(Direction, true, out bool IsMoved);
        }
    }
}
