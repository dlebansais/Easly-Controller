﻿namespace Test
{
    using Environment = System.Environment;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;
    using NUnit.Framework;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using PolySerializer;
    using System.Collections.Generic;
    using EaslyController;
    using EaslyController.Writeable;
    using EaslyController.Frame;
    using Easly;
    using EaslyController.Focus;
    using TestDebug;
    using EaslyController.Layout;
    using EaslyEdit;
    using System.Text;
    using NotNullReflection;

    [TestFixture]
    public partial class TestSet
    {
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
                EaslyControllerAssembly = Assembly.Missing;
            }
            Assume.That(EaslyControllerAssembly != Assembly.Missing);

            string StartDirectory = Environment.CurrentDirectory;
            while (Path.GetFileName(StartDirectory) != "Test-Easly-Controller" && StartDirectory.Length > 0)
                StartDirectory = Path.GetDirectoryName(StartDirectory);

            if (StartDirectory.Length == 0)
                StartDirectory = $"{Environment.CurrentDirectory}/Test/Test-Easly-Controller";

            string RootPath;
            RootPath = Path.Combine(StartDirectory, "EaslyExamples");

            TestContext.Progress.WriteLine($"Working on examples from: {StartDirectory}");

            FileNameTable = new List<string>();
            FirstRootNode = null;

//#if ADDEASLYFILES
            AddEaslyFiles(RootPath);
//#endif
        }

        static void AddEaslyFiles(string path)
        {
            foreach (string FileName in Directory.GetFiles(path, "*.easly"))
            {
                FileNameTable.Add(FileName.Replace("\\", "/"));

                using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                {
                    Node RootNode = DeserializeAndFix(fs);

                    if (FirstRootNode == null)
                        FirstRootNode = RootNode;
                }
            }

            foreach (string Folder in Directory.GetDirectories(path))
                AddEaslyFiles(Folder);
        }

        private static Node DeserializeAndFix(FileStream fs)
        {
            Serializer Serializer = new Serializer();
            Node RootNode;

            RootNode = (Node)Serializer.Deserialize(fs);

            WalkCallbacks<string> Callbacks = new WalkCallbacks<string>();
            Callbacks.IsRecursive = true;
            Callbacks.HandlerRoot = OnUpdateRoot;
            Callbacks.HandlerNode = OnUpdateNode;
            NodeTreeWalk.Walk(RootNode, Callbacks, string.Empty);

            return RootNode;
        }

        private static bool OnUpdateRoot(Node node, WalkCallbacks<string> callback, string data)
        {
            return OnUpdate(node, callback, data);
        }

        private static bool OnUpdateNode(Node node, Node parentNode, string propertyName, WalkCallbacks<string> callback, string data)
        {
            return OnUpdate(node, callback, data);
        }

        private static bool OnUpdate(Node node, WalkCallbacks<string> callback, string data)
        {
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(node);

            foreach (string PropertyName in PropertyNames)
            {
                if (NodeTreeHelperOptional.IsOptionalChildNodeProperty(node, PropertyName, out Type ChildNodeType))
                {
                    NodeTreeHelperOptional.GetChildNode(node, PropertyName, out bool IsAssigned, out Node ChildNode);
                    if (!IsAssigned)
                    {
                        if (ChildNode is null)
                        {
                            Node NewChildNode = NodeHelper.CreateDefaultFromType(ChildNodeType);
                            NodeTreeHelperOptional.SetOptionalChildNode(node, PropertyName, NewChildNode);
                            NodeTreeHelperOptional.UnassignChildNode(node, PropertyName);
                        }
                    }
                    else if (ChildNode is null)
                    {
                        Node NewChildNode = NodeHelper.CreateDefaultFromType(ChildNodeType);
                        NodeTreeHelperOptional.SetOptionalChildNode(node, PropertyName, NewChildNode);
                    }
                    else if (NodeHelper.IsDefaultNode(ChildNode))
                    {
                    }
                }
            }

            return true;
        }

        static IEnumerable<int> FileIndexRange()
        {
            for (int i = 0; i < 171; i++)
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
        static Node FirstRootNode;

        static bool TestOff = false;
        //const int TestRepeatCount = 5;
        const int TestRepeatCount = 1;
    }
}
