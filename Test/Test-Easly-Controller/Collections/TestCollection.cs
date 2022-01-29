namespace Coverage;

using EaslyController;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using NUnit.Framework;
using System;
using System.Collections.Generic;

delegate ReadOnlyBlockStateReadOnlyList ToReadOnlyDelegate();

[TestFixture]
public class TestCollection
{
    [Test]
    [Category("CollectionCoverage")]
    public static void TestCollectionReadOnly()
    {
        ControllerTools.ResetExpectedName();

        TestList<ReadOnlyBlockStateList, ReadOnlyBlockStateReadOnlyList, IReadOnlyBlockState> TestListBlockState = new((ReadOnlyBlockStateList list) => list.ToReadOnly());
        TestListBlockState.TestToReadOnly();

        Func<ReadOnlyBlockStateViewDictionary, ReadOnlyBlockStateViewReadOnlyDictionary> ToReadOnlyHandler = (ReadOnlyBlockStateViewDictionary dictionary) => dictionary.ToReadOnly();
        IReadOnlyBlockState NeutralKey = ReadOnlyBlockState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.Empty;
        ReadOnlyBlockStateView NeutralValue = ReadOnlyBlockStateView.Empty;
        TestDictionary<ReadOnlyBlockStateViewDictionary, ReadOnlyBlockStateViewReadOnlyDictionary, IReadOnlyBlockState, ReadOnlyBlockStateView> TestDictionaryBlockStateView = new(ToReadOnlyHandler, NeutralKey, NeutralValue);
        ReadOnlyBlockStateViewDictionary BlockStateViewDictionaryFromSource = new(new Dictionary<IReadOnlyBlockState, ReadOnlyBlockStateView>());
        ReadOnlyBlockStateViewDictionary BlockStateViewDictionaryWithCapacity = new(1);
        TestDictionaryBlockStateView.TestToReadOnly();
        TestDictionaryBlockStateView.TestIsEqual();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestCollectionWriteable()
    {
        ControllerTools.ResetExpectedName();

        TestList<WriteableBlockStateList, WriteableBlockStateReadOnlyList, IWriteableBlockState> TestListBlockState = new((WriteableBlockStateList list) => (WriteableBlockStateReadOnlyList)list.ToReadOnly());
        TestListBlockState.TestToReadOnly();

        Func<WriteableBlockStateViewDictionary, WriteableBlockStateViewReadOnlyDictionary> ToReadOnlyHandler = (WriteableBlockStateViewDictionary dictionary) => (WriteableBlockStateViewReadOnlyDictionary)dictionary.ToReadOnly();
        IWriteableBlockState NeutralKey = WriteableBlockState<IWriteableInner<IWriteableBrowsingChildIndex>>.Empty;
        WriteableBlockStateView NeutralValue = WriteableBlockStateView.Empty;
        TestDictionary<WriteableBlockStateViewDictionary, WriteableBlockStateViewReadOnlyDictionary, IWriteableBlockState, WriteableBlockStateView> TestDictionaryBlockStateView = new(ToReadOnlyHandler, NeutralKey, NeutralValue);
        WriteableBlockStateViewDictionary BlockStateViewDictionaryFromSource = new(new Dictionary<IWriteableBlockState, WriteableBlockStateView>());
        WriteableBlockStateViewDictionary BlockStateViewDictionaryWithCapacity = new(1);
        TestDictionaryBlockStateView.TestToReadOnly();
        TestDictionaryBlockStateView.TestIsEqual();
    }
}
