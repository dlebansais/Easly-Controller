namespace Coverage;

using EaslyController;
using EaslyController.Writeable;
using NUnit.Framework;
using System;
using System.Collections.Generic;

[TestFixture]
public class TestCollectionWriteable
{
    [Test]
    [Category("CollectionCoverage")]
    public static void TestWriteableBlockStateList()
    {
        ControllerTools.ResetExpectedName();

        Func<WriteableBlockStateList, WriteableBlockStateReadOnlyList> ListToReadOnlyHandler = (WriteableBlockStateList list) => (WriteableBlockStateReadOnlyList)list.ToReadOnly();
        IWriteableBlockState NeutralItem = WriteableBlockState<IWriteableInner<IWriteableBrowsingChildIndex>>.Empty;
        TestList<WriteableBlockStateList, WriteableBlockStateReadOnlyList, IWriteableBlockState> TestListBlockState = new(ListToReadOnlyHandler, NeutralItem);
        TestListBlockState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestWriteableBlockStateViewDictionary()
    {
        ControllerTools.ResetExpectedName();

        Func<WriteableBlockStateViewDictionary, WriteableBlockStateViewReadOnlyDictionary> DictionaryToReadOnlyHandler = (WriteableBlockStateViewDictionary dictionary) => (WriteableBlockStateViewReadOnlyDictionary)dictionary.ToReadOnly();
        IWriteableBlockState NeutralKey = WriteableBlockState<IWriteableInner<IWriteableBrowsingChildIndex>>.Empty;
        WriteableBlockStateView NeutralValue = WriteableBlockStateView.Empty;
        TestDictionary<WriteableBlockStateViewDictionary, WriteableBlockStateViewReadOnlyDictionary, IWriteableBlockState, WriteableBlockStateView> TestDictionaryBlockStateView = new(DictionaryToReadOnlyHandler, NeutralKey, NeutralValue);
        WriteableBlockStateViewDictionary BlockStateViewDictionaryFromSource = new(new Dictionary<IWriteableBlockState, WriteableBlockStateView>());
        WriteableBlockStateViewDictionary BlockStateViewDictionaryWithCapacity = new(1);
        TestDictionaryBlockStateView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestWriteableBrowsingBlockNodeIndexList()
    {
        ControllerTools.ResetExpectedName();

        Func<WriteableBrowsingBlockNodeIndexList, WriteableBrowsingBlockNodeIndexReadOnlyList> ListToReadOnlyHandler = (WriteableBrowsingBlockNodeIndexList list) => (WriteableBrowsingBlockNodeIndexReadOnlyList)list.ToReadOnly();
        IWriteableBrowsingBlockNodeIndex NeutralItem = WriteableBrowsingNewBlockNodeIndex.Empty;
        TestList<WriteableBrowsingBlockNodeIndexList, WriteableBrowsingBlockNodeIndexReadOnlyList, IWriteableBrowsingBlockNodeIndex> TestListBrowsingBlockNodeIndex = new(ListToReadOnlyHandler, NeutralItem);
        TestListBrowsingBlockNodeIndex.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestWriteableBrowsingListNodeIndexList()
    {
        ControllerTools.ResetExpectedName();

        Func<WriteableBrowsingListNodeIndexList, WriteableBrowsingListNodeIndexReadOnlyList> ListToReadOnlyHandler = (WriteableBrowsingListNodeIndexList list) => (WriteableBrowsingListNodeIndexReadOnlyList)list.ToReadOnly();
        IWriteableBrowsingListNodeIndex NeutralItem = WriteableBrowsingListNodeIndex.Empty;
        TestList<WriteableBrowsingListNodeIndexList, WriteableBrowsingListNodeIndexReadOnlyList, IWriteableBrowsingListNodeIndex> TestListBrowsingListNodeIndex = new(ListToReadOnlyHandler, NeutralItem);
        TestListBrowsingListNodeIndex.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestWriteableIndexCollectionList()
    {
        ControllerTools.ResetExpectedName();

        Func<WriteableIndexCollectionList, WriteableIndexCollectionReadOnlyList> ListToReadOnlyHandler = (WriteableIndexCollectionList list) => (WriteableIndexCollectionReadOnlyList)list.ToReadOnly();
        IWriteableIndexCollection NeutralItem = WriteableIndexCollection<IWriteableBrowsingChildIndex>.Empty;
        TestList<WriteableIndexCollectionList, WriteableIndexCollectionReadOnlyList, IWriteableIndexCollection> TestListIndexCollection = new(ListToReadOnlyHandler, NeutralItem);
        TestListIndexCollection.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestWriteableInnerDictionary()
    {
        ControllerTools.ResetExpectedName();

        Func<WriteableInnerDictionary<string>, WriteableInnerReadOnlyDictionary<string>> DictionaryToWriteableHandler = (WriteableInnerDictionary<string> dictionary) => (WriteableInnerReadOnlyDictionary<string>)dictionary.ToReadOnly();
        string NeutralKey = string.Empty;
        IWriteableInner NeutralValue = WriteableInner<IWriteableBrowsingChildIndex>.Empty;
        TestDictionary<WriteableInnerDictionary<string>, WriteableInnerReadOnlyDictionary<string>, string, IWriteableInner> TestDictionaryInner = new(DictionaryToWriteableHandler, NeutralKey, NeutralValue);
        WriteableInnerDictionary<string> InnerDictionaryFromSource = new(new Dictionary<string, IWriteableInner>());
        WriteableInnerDictionary<string> InnerDictionaryWithCapacity = new(1);
        TestDictionaryInner.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestWriteableNodeStateDictionary()
    {
        ControllerTools.ResetExpectedName();

        Func<WriteableNodeStateDictionary, WriteableNodeStateReadOnlyDictionary> DictionaryToWriteableHandler = (WriteableNodeStateDictionary dictionary) => (WriteableNodeStateReadOnlyDictionary)dictionary.ToReadOnly();
        IWriteableIndex NeutralKey = WriteableRootNodeIndex.Empty;
        IWriteableNodeState NeutralValue = WriteableNodeState<IWriteableInner<IWriteableBrowsingChildIndex>>.Empty;
        TestDictionary<WriteableNodeStateDictionary, WriteableNodeStateReadOnlyDictionary, IWriteableIndex, IWriteableNodeState> TestDictionaryNodeState = new(DictionaryToWriteableHandler, NeutralKey, NeutralValue);
        WriteableNodeStateDictionary NodeStateDictionaryFromSource = new(new Dictionary<IWriteableIndex, IWriteableNodeState>());
        WriteableNodeStateDictionary NodeStateDictionaryWithCapacity = new(1);
        TestDictionaryNodeState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestWriteableNodeStateList()
    {
        ControllerTools.ResetExpectedName();

        Func<WriteableNodeStateList, WriteableNodeStateReadOnlyList> ListToReadOnlyHandler = (WriteableNodeStateList list) => (WriteableNodeStateReadOnlyList)list.ToReadOnly();
        IWriteableNodeState NeutralItem = WriteableNodeState<IWriteableInner<IWriteableBrowsingChildIndex>>.Empty;
        TestList<WriteableNodeStateList, WriteableNodeStateReadOnlyList, IWriteableNodeState> TestListNodeState = new(ListToReadOnlyHandler, NeutralItem);
        TestListNodeState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestWriteableNodeStateViewDictionary()
    {
        ControllerTools.ResetExpectedName();

        Func<WriteableNodeStateViewDictionary, WriteableNodeStateViewReadOnlyDictionary> DictionaryToWriteableHandler = (WriteableNodeStateViewDictionary dictionary) => (WriteableNodeStateViewReadOnlyDictionary)dictionary.ToReadOnly();
        IWriteableNodeState NeutralKey = WriteableNodeState<IWriteableInner<IWriteableBrowsingChildIndex>>.Empty;
        IWriteableNodeStateView NeutralValue = WriteableNodeStateView.Empty;
        TestDictionary<WriteableNodeStateViewDictionary, WriteableNodeStateViewReadOnlyDictionary, IWriteableNodeState, IWriteableNodeStateView> TestDictionaryNodeStateView = new(DictionaryToWriteableHandler, NeutralKey, NeutralValue);
        WriteableNodeStateViewDictionary NodeStateViewDictionaryFromSource = new(new Dictionary<IWriteableNodeState, IWriteableNodeStateView>());
        WriteableNodeStateViewDictionary NodeStateViewDictionaryWithCapacity = new(1);
        TestDictionaryNodeStateView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestWriteableOperationGroupList()
    {
        ControllerTools.ResetExpectedName();

        Func<WriteableOperationGroupList, WriteableOperationGroupReadOnlyList> ListToReadOnlyHandler = (WriteableOperationGroupList list) => list.ToReadOnly();
        WriteableOperationGroup NeutralItem = WriteableOperationGroup.Empty;
        TestList<WriteableOperationGroupList, WriteableOperationGroupReadOnlyList, WriteableOperationGroup> TestListOperationGroup = new(ListToReadOnlyHandler, NeutralItem);
        TestListOperationGroup.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestWriteableOperationList()
    {
        ControllerTools.ResetExpectedName();

        Func<WriteableOperationList, WriteableOperationReadOnlyList> ListToReadOnlyHandler = (WriteableOperationList list) => list.ToReadOnly();
        IWriteableOperation NeutralItem = WriteableOperation.Empty;
        TestList<WriteableOperationList, WriteableOperationReadOnlyList, IWriteableOperation> TestListOperation = new(ListToReadOnlyHandler, NeutralItem);
        TestListOperation.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestWriteablePlaceholderNodeStateList()
    {
        ControllerTools.ResetExpectedName();

        Func<WriteablePlaceholderNodeStateList, WriteablePlaceholderNodeStateReadOnlyList> ListToReadOnlyHandler = (WriteablePlaceholderNodeStateList list) => (WriteablePlaceholderNodeStateReadOnlyList)list.ToReadOnly();
        IWriteablePlaceholderNodeState NeutralItem = WriteablePlaceholderNodeState<IWriteableInner<IWriteableBrowsingChildIndex>>.Empty;
        TestList<WriteablePlaceholderNodeStateList, WriteablePlaceholderNodeStateReadOnlyList, IWriteablePlaceholderNodeState> TestListPlaceholderNodeState = new(ListToReadOnlyHandler, NeutralItem);
        TestListPlaceholderNodeState.Test();
    }
}
