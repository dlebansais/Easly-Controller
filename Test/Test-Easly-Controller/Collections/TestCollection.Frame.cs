namespace Coverage;

using EaslyController;
using EaslyController.Frame;
using NUnit.Framework;
using System;
using System.Collections.Generic;

[TestFixture]
public class TestCollectionFrame
{
    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameBlockStateList()
    {
        ControllerTools.ResetExpectedName();

        Func<FrameBlockStateList, FrameBlockStateReadOnlyList> ListToReadOnlyHandler = (FrameBlockStateList list) => (FrameBlockStateReadOnlyList)list.ToReadOnly();
        IFrameBlockState NeutralItem = FrameBlockState<IFrameInner<IFrameBrowsingChildIndex>>.Empty;
        TestList<FrameBlockStateList, FrameBlockStateReadOnlyList, IFrameBlockState> TestListBlockState = new(ListToReadOnlyHandler, NeutralItem);
        TestListBlockState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameBlockStateViewDictionary()
    {
        ControllerTools.ResetExpectedName();

        Func<FrameBlockStateViewDictionary, FrameBlockStateViewReadOnlyDictionary> DictionaryToReadOnlyHandler = (FrameBlockStateViewDictionary dictionary) => (FrameBlockStateViewReadOnlyDictionary)dictionary.ToReadOnly();
        IFrameBlockState NeutralKey = FrameBlockState<IFrameInner<IFrameBrowsingChildIndex>>.Empty;
        FrameBlockStateView NeutralValue = FrameBlockStateView.Empty;
        TestDictionary<FrameBlockStateViewDictionary, FrameBlockStateViewReadOnlyDictionary, IFrameBlockState, FrameBlockStateView> TestDictionaryBlockStateView = new(DictionaryToReadOnlyHandler, NeutralKey, NeutralValue);
        FrameBlockStateViewDictionary BlockStateViewDictionaryFromSource = new(new Dictionary<IFrameBlockState, FrameBlockStateView>());
        FrameBlockStateViewDictionary BlockStateViewDictionaryWithCapacity = new(1);
        TestDictionaryBlockStateView.Test();
    }
}
