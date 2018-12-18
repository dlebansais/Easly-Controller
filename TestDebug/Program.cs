﻿using BaseNode;
using BaseNodeHelper;
using EaslyController;
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
            IClass RootNode;
            Serializer Serializer = new Serializer();

            string CurrentDirectory = Environment.CurrentDirectory;
            if (CurrentDirectory.Contains("Debug") || CurrentDirectory.Contains("Release"))
                CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);
            if (CurrentDirectory.Contains("x64"))
                CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);
            if (CurrentDirectory.Contains("bin"))
                CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);

            CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);

            using (FileStream fs = new FileStream(Path.Combine(CurrentDirectory, "Test/test.easly"), FileMode.Open, FileAccess.Read))
            {
                RootNode = Serializer.Deserialize(fs) as IClass;
            }

            TestReadOnly(RootNode);
            TestWriteable(RootNode);
        }
        #endregion

        #region Tools
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

                        //Debug.WriteLine(s);
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

        static void TestReadOnly(INode rootNode)
        {
            ControllerTools.ResetExpectedName();

            IReadOnlyRootNodeIndex RootIndex = new ReadOnlyRootNodeIndex(rootNode);
            IReadOnlyController Controller = ReadOnlyController.Create(RootIndex);
            Stats Stats = Controller.Stats;

            INode RootNodeClone = Controller.RootState.CloneNode();
            ulong h1 = NodeHelper.NodeHash(rootNode);
            ulong h2 = NodeHelper.NodeHash(RootNodeClone);

            byte[] RootData = GetData(rootNode);
            byte[] RootCloneData = GetData(RootNodeClone);

            bool IsEqual = ByteArrayCompare(RootData, RootCloneData);
            Debug.Assert(IsEqual);
            Debug.Assert(h1 == h2);

            IReadOnlyControllerView ControllerView = ReadOnlyControllerView.Create(Controller);
        }

        static void TestWriteable(IClass rootNode)
        {
            ControllerTools.ResetExpectedName();

            IWriteableRootNodeIndex RootIndex = new WriteableRootNodeIndex(rootNode);
            IWriteableController Controller = WriteableController.Create(RootIndex);
            Stats Stats = Controller.Stats;

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
            IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner = (IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex>)InnerTable[nameof(IClass.ConversionBlocks)];

            IPattern PatternNode = NodeHelper.CreateEmptyPattern();
            IIdentifier SourceNode = NodeHelper.CreateEmptyIdentifier();
            IIdentifier FirstNode = NodeHelper.CreateEmptyIdentifier();

            WriteableInsertionNewBlockNodeIndex InsertIndex0 = new WriteableInsertionNewBlockNodeIndex(rootNode, FirstNode, nameof(IClass.ConversionBlocks), 0, PatternNode, SourceNode);
            Controller.Insert(Inner, InsertIndex0);

            IIdentifier SecondNode = NodeHelper.CreateEmptyIdentifier();

            WriteableInsertionExistingBlockNodeIndex InsertIndex1 = new WriteableInsertionExistingBlockNodeIndex(rootNode, SecondNode, nameof(IClass.ConversionBlocks), 0, 1);
            Controller.Insert(Inner, InsertIndex1);

            Debug.Assert(ControllerView.StateViewTable.Count == Controller.Stats.NodeCount);
            
        }
    }
}
