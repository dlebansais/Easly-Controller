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
}
