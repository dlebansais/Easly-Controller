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
        [Test]
        public static void TestInit()
        {
            if (TestOff)
                return;

            ControllerTools.ResetExpectedName();

            IReadOnlyRootNodeIndex RootIndex = new ReadOnlyRootNodeIndex(FirstRootNode);
            ReadOnlyController Controller = ReadOnlyController.Create(RootIndex);

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
    }
}
