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
}
