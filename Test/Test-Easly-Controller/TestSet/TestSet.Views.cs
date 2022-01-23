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
#if VIEWS
        [Test]
        [TestCaseSource(nameof(FileIndexRange))]
        public static void StateViews(int index)
        {
            if (TestOff)
                return;

            string Name = null;
            Node RootNode = null;
            int n = index;
            foreach (string FileName in FileNameTable)
            {
                if (n == 0)
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    {
                        Name = FileName;
                        Serializer Serializer = new Serializer();
                        RootNode = Serializer.Deserialize(fs) as Node;
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

        public static void TestStateView(int index, Node rootNode)
        {
            ControllerTools.ResetExpectedName();

            IReadOnlyRootNodeIndex RootIndex = new ReadOnlyRootNodeIndex(rootNode);
            ReadOnlyController Controller = ReadOnlyController.Create(RootIndex);
            ReadOnlyControllerView ControllerView = ReadOnlyControllerView.Create(Controller);

            Assert.That(ControllerView.StateViewTable.ContainsKey(Controller.RootState), $"Views #0");
            Assert.That(ControllerView.StateViewTable.Count == Controller.Stats.NodeCount, $"Views #1");

            foreach (KeyValuePair<IReadOnlyNodeState, IReadOnlyNodeStateView> Entry in ControllerView.StateViewTable)
            {
                IReadOnlyNodeState State = Entry.Key;
                Assert.That(ControllerView.StateViewTable.ContainsKey(Controller.RootState), $"Views #2, state={State}");

                IReadOnlyNodeStateView View = Entry.Value;
                Assert.That(View.State == State, $"Views #3");
            }

            ReadOnlyControllerView ControllerView2 = ReadOnlyControllerView.Create(Controller);
            Assert.That(ControllerView2.IsEqual(CompareEqual.New(), ControllerView), $"Views #4");
        }
    }
}
