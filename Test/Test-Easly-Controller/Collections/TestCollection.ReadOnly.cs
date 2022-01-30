namespace Coverage;

using EaslyController;
using EaslyController.ReadOnly;
using NUnit.Framework;
using System;
using System.Collections.Generic;

[TestFixture]
public class TestCollectionReadOnly
{
    [Test]
    [Category("CollectionCoverage")]
    public static void TestReadOnlyBlockStateList()
    {
        ControllerTools.ResetExpectedName();

        Func<ReadOnlyBlockStateList, ReadOnlyBlockStateReadOnlyList> ListToReadOnlyHandler = (ReadOnlyBlockStateList list) => list.ToReadOnly();
        IReadOnlyBlockState NeutralItem = ReadOnlyBlockState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.Empty;
        TestList<ReadOnlyBlockStateList, ReadOnlyBlockStateReadOnlyList, IReadOnlyBlockState> TestListBlockState = new(ListToReadOnlyHandler, NeutralItem);
        TestListBlockState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestReadOnlyBlockStateViewDictionary()
    {
        ControllerTools.ResetExpectedName();

        Func<ReadOnlyBlockStateViewDictionary, ReadOnlyBlockStateViewReadOnlyDictionary> DictionaryToReadOnlyHandler = (ReadOnlyBlockStateViewDictionary dictionary) => dictionary.ToReadOnly();
        IReadOnlyBlockState NeutralKey = ReadOnlyBlockState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.Empty;
        ReadOnlyBlockStateView NeutralValue = ReadOnlyBlockStateView.Empty;
        TestDictionary<ReadOnlyBlockStateViewDictionary, ReadOnlyBlockStateViewReadOnlyDictionary, IReadOnlyBlockState, ReadOnlyBlockStateView> TestDictionaryBlockStateView = new(DictionaryToReadOnlyHandler, NeutralKey, NeutralValue);
        ReadOnlyBlockStateViewDictionary BlockStateViewDictionaryFromSource = new(new Dictionary<IReadOnlyBlockState, ReadOnlyBlockStateView>());
        ReadOnlyBlockStateViewDictionary BlockStateViewDictionaryWithCapacity = new(1);
        TestDictionaryBlockStateView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestReadOnlyBrowsingBlockNodeIndexList()
    {
        ControllerTools.ResetExpectedName();

        Func<ReadOnlyBrowsingBlockNodeIndexList, ReadOnlyBrowsingBlockNodeIndexReadOnlyList> ListToReadOnlyHandler = (ReadOnlyBrowsingBlockNodeIndexList list) => list.ToReadOnly();
        IReadOnlyBrowsingBlockNodeIndex NeutralItem = ReadOnlyBrowsingNewBlockNodeIndex.Empty;
        TestList<ReadOnlyBrowsingBlockNodeIndexList, ReadOnlyBrowsingBlockNodeIndexReadOnlyList, IReadOnlyBrowsingBlockNodeIndex> TestListBrowsingBlockNodeIndex = new(ListToReadOnlyHandler, NeutralItem);
        TestListBrowsingBlockNodeIndex.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestReadOnlyBrowsingListNodeIndexList()
    {
        ControllerTools.ResetExpectedName();

        Func<ReadOnlyBrowsingListNodeIndexList, ReadOnlyBrowsingListNodeIndexReadOnlyList> ListToReadOnlyHandler = (ReadOnlyBrowsingListNodeIndexList list) => list.ToReadOnly();
        IReadOnlyBrowsingListNodeIndex NeutralItem = ReadOnlyBrowsingListNodeIndex.Empty;
        TestList<ReadOnlyBrowsingListNodeIndexList, ReadOnlyBrowsingListNodeIndexReadOnlyList, IReadOnlyBrowsingListNodeIndex> TestListBrowsingListNodeIndex = new(ListToReadOnlyHandler, NeutralItem);
        TestListBrowsingListNodeIndex.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestReadOnlyIndexCollectionList()
    {
        ControllerTools.ResetExpectedName();

        Func<ReadOnlyIndexCollectionList, ReadOnlyIndexCollectionReadOnlyList> ListToReadOnlyHandler = (ReadOnlyIndexCollectionList list) => list.ToReadOnly();
        IReadOnlyIndexCollection NeutralItem = ReadOnlyIndexCollection<IReadOnlyBrowsingChildIndex>.Empty;
        TestList<ReadOnlyIndexCollectionList, ReadOnlyIndexCollectionReadOnlyList, IReadOnlyIndexCollection> TestListIndexCollection = new(ListToReadOnlyHandler, NeutralItem);
        TestListIndexCollection.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestReadOnlyInnerDictionary()
    {
        ControllerTools.ResetExpectedName();

        Func<ReadOnlyInnerDictionary<string>, ReadOnlyInnerReadOnlyDictionary<string>> DictionaryToReadOnlyHandler = (ReadOnlyInnerDictionary<string> dictionary) => dictionary.ToReadOnly();
        string NeutralKey = string.Empty;
        IReadOnlyInner NeutralValue = ReadOnlyInner<IReadOnlyBrowsingChildIndex>.Empty;
        TestDictionary<ReadOnlyInnerDictionary<string>, ReadOnlyInnerReadOnlyDictionary<string>, string, IReadOnlyInner> TestDictionaryInner = new(DictionaryToReadOnlyHandler, NeutralKey, NeutralValue);
        ReadOnlyInnerDictionary<string> InnerDictionaryFromSource = new(new Dictionary<string, IReadOnlyInner>());
        ReadOnlyInnerDictionary<string> InnerDictionaryWithCapacity = new(1);
        TestDictionaryInner.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestReadOnlyNodeStateDictionary()
    {
        ControllerTools.ResetExpectedName();

        Func<ReadOnlyNodeStateDictionary, ReadOnlyNodeStateReadOnlyDictionary> DictionaryToReadOnlyHandler = (ReadOnlyNodeStateDictionary dictionary) => dictionary.ToReadOnly();
        IReadOnlyIndex NeutralKey = ReadOnlyRootNodeIndex.Empty;
        IReadOnlyNodeState NeutralValue = ReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.Empty;
        TestDictionary<ReadOnlyNodeStateDictionary, ReadOnlyNodeStateReadOnlyDictionary, IReadOnlyIndex, IReadOnlyNodeState> TestDictionaryNodeState = new(DictionaryToReadOnlyHandler, NeutralKey, NeutralValue);
        ReadOnlyNodeStateDictionary NodeStateDictionaryFromSource = new(new Dictionary<IReadOnlyIndex, IReadOnlyNodeState>());
        ReadOnlyNodeStateDictionary NodeStateDictionaryWithCapacity = new(1);
        TestDictionaryNodeState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestReadOnlyNodeStateList()
    {
        ControllerTools.ResetExpectedName();

        Func<ReadOnlyNodeStateList, ReadOnlyNodeStateReadOnlyList> ListToReadOnlyHandler = (ReadOnlyNodeStateList list) => list.ToReadOnly();
        IReadOnlyNodeState NeutralItem = ReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.Empty;
        TestList<ReadOnlyNodeStateList, ReadOnlyNodeStateReadOnlyList, IReadOnlyNodeState> TestListNodeState = new(ListToReadOnlyHandler, NeutralItem);
        TestListNodeState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestReadOnlyNodeStateViewDictionary()
    {
        ControllerTools.ResetExpectedName();

        Func<ReadOnlyNodeStateViewDictionary, ReadOnlyNodeStateViewReadOnlyDictionary> DictionaryToReadOnlyHandler = (ReadOnlyNodeStateViewDictionary dictionary) => dictionary.ToReadOnly();
        IReadOnlyNodeState NeutralKey = ReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.Empty;
        IReadOnlyNodeStateView NeutralValue = ReadOnlyNodeStateView.Empty;
        TestDictionary<ReadOnlyNodeStateViewDictionary, ReadOnlyNodeStateViewReadOnlyDictionary, IReadOnlyNodeState, IReadOnlyNodeStateView> TestDictionaryNodeStateView = new(DictionaryToReadOnlyHandler, NeutralKey, NeutralValue);
        ReadOnlyNodeStateViewDictionary NodeStateViewDictionaryFromSource = new(new Dictionary<IReadOnlyNodeState, IReadOnlyNodeStateView>());
        ReadOnlyNodeStateViewDictionary NodeStateViewDictionaryWithCapacity = new(1);
        TestDictionaryNodeStateView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestReadOnlyPlaceholderNodeStateList()
    {
        ControllerTools.ResetExpectedName();

        Func<ReadOnlyPlaceholderNodeStateList, ReadOnlyPlaceholderNodeStateReadOnlyList> ListToReadOnlyHandler = (ReadOnlyPlaceholderNodeStateList list) => list.ToReadOnly();
        IReadOnlyPlaceholderNodeState NeutralItem = ReadOnlyPlaceholderNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.Empty;
        TestList<ReadOnlyPlaceholderNodeStateList, ReadOnlyPlaceholderNodeStateReadOnlyList, IReadOnlyPlaceholderNodeState> TestListPlaceholderNodeState = new(ListToReadOnlyHandler, NeutralItem);
        TestListPlaceholderNodeState.Test();
    }
}
