namespace Coverage;

using EaslyController;
using EaslyController.Frame;
using NUnit.Framework;
using System.Collections.Generic;
using NotNullReflection;

[TestFixture]
public class TestCollectionFrame
{
    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameAssignableCellViewDictionary()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameAssignableCellViewDictionary<string>, FrameAssignableCellViewReadOnlyDictionary<string>> DictionaryToReadOnlyHandler = (FrameAssignableCellViewDictionary<string> dictionary) => dictionary.ToReadOnly();
        string NeutralKey = string.Empty;
        IFrameAssignableCellView NeutralValue = FrameCellViewCollection.Empty;
        TestDictionary<FrameAssignableCellViewDictionary<string>, FrameAssignableCellViewReadOnlyDictionary<string>, string, IFrameAssignableCellView> TestDictionaryAssignableCellView = new(DictionaryToReadOnlyHandler, NeutralKey, NeutralValue);
        FrameAssignableCellViewDictionary<string> AssignableCellViewDictionaryFromSource = new(new Dictionary<string, IFrameAssignableCellView>());
        FrameAssignableCellViewDictionary<string> AssignableCellViewDictionaryWithCapacity = new(1);
        TestDictionaryAssignableCellView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameBlockStateList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameBlockStateList, FrameBlockStateReadOnlyList> ListToReadOnlyHandler = (FrameBlockStateList list) => (FrameBlockStateReadOnlyList)list.ToReadOnly();
        IFrameBlockState NeutralItem = FrameBlockState<IFrameInner<IFrameBrowsingChildIndex>>.Empty;
        TestList<FrameBlockStateList, FrameBlockStateReadOnlyList, IFrameBlockState> TestListBlockState = new(ListToReadOnlyHandler, NeutralItem);
        TestListBlockState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameBlockStateViewDictionary()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameBlockStateViewDictionary, FrameBlockStateViewReadOnlyDictionary> DictionaryToReadOnlyHandler = (FrameBlockStateViewDictionary dictionary) => (FrameBlockStateViewReadOnlyDictionary)dictionary.ToReadOnly();
        IFrameBlockState NeutralKey = FrameBlockState<IFrameInner<IFrameBrowsingChildIndex>>.Empty;
        FrameBlockStateView NeutralValue = FrameBlockStateView.Empty;
        TestDictionary<FrameBlockStateViewDictionary, FrameBlockStateViewReadOnlyDictionary, IFrameBlockState, FrameBlockStateView> TestDictionaryBlockStateView = new(DictionaryToReadOnlyHandler, NeutralKey, NeutralValue);
        FrameBlockStateViewDictionary BlockStateViewDictionaryFromSource = new(new Dictionary<IFrameBlockState, FrameBlockStateView>());
        FrameBlockStateViewDictionary BlockStateViewDictionaryWithCapacity = new(1);
        TestDictionaryBlockStateView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameBrowsingBlockNodeIndexList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameBrowsingBlockNodeIndexList, FrameBrowsingBlockNodeIndexReadOnlyList> ListToReadOnlyHandler = (FrameBrowsingBlockNodeIndexList list) => (FrameBrowsingBlockNodeIndexReadOnlyList)list.ToReadOnly();
        IFrameBrowsingBlockNodeIndex NeutralItem = FrameBrowsingNewBlockNodeIndex.Empty;
        TestList<FrameBrowsingBlockNodeIndexList, FrameBrowsingBlockNodeIndexReadOnlyList, IFrameBrowsingBlockNodeIndex> TestListBrowsingBlockNodeIndex = new(ListToReadOnlyHandler, NeutralItem);
        TestListBrowsingBlockNodeIndex.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameBrowsingListNodeIndexList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameBrowsingListNodeIndexList, FrameBrowsingListNodeIndexReadOnlyList> ListToReadOnlyHandler = (FrameBrowsingListNodeIndexList list) => (FrameBrowsingListNodeIndexReadOnlyList)list.ToReadOnly();
        IFrameBrowsingListNodeIndex NeutralItem = FrameBrowsingListNodeIndex.Empty;
        TestList<FrameBrowsingListNodeIndexList, FrameBrowsingListNodeIndexReadOnlyList, IFrameBrowsingListNodeIndex> TestListBrowsingListNodeIndex = new(ListToReadOnlyHandler, NeutralItem);
        TestListBrowsingListNodeIndex.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameCellViewList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameCellViewList, FrameCellViewReadOnlyList> ListToReadOnlyHandler = (FrameCellViewList list) => list.ToReadOnly();
        IFrameCellView NeutralItem = FrameCellViewCollection.Empty;
        TestList<FrameCellViewList, FrameCellViewReadOnlyList, IFrameCellView> TestListCellView = new(ListToReadOnlyHandler, NeutralItem);
        TestListCellView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameFrameList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameFrameList, FrameFrameReadOnlyList> ListToReadOnlyHandler = (FrameFrameList list) => list.ToReadOnly();
        IFrameFrame NeutralItem = FrameFrame.FrameRoot;
        TestList<FrameFrameList, FrameFrameReadOnlyList, IFrameFrame> TestListFrame = new(ListToReadOnlyHandler, NeutralItem);
        TestListFrame.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameIndexCollectionList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameIndexCollectionList, FrameIndexCollectionReadOnlyList> ListToReadOnlyHandler = (FrameIndexCollectionList list) => (FrameIndexCollectionReadOnlyList)list.ToReadOnly();
        IFrameIndexCollection NeutralItem = FrameIndexCollection<IFrameBrowsingChildIndex>.Empty;
        TestList<FrameIndexCollectionList, FrameIndexCollectionReadOnlyList, IFrameIndexCollection> TestListIndexCollection = new(ListToReadOnlyHandler, NeutralItem);
        TestListIndexCollection.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameInnerDictionary()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameInnerDictionary<string>, FrameInnerReadOnlyDictionary<string>> DictionaryToFrameHandler = (FrameInnerDictionary<string> dictionary) => (FrameInnerReadOnlyDictionary<string>)dictionary.ToReadOnly();
        string NeutralKey = string.Empty;
        IFrameInner NeutralValue = FrameInner<IFrameBrowsingChildIndex>.Empty;
        TestDictionary<FrameInnerDictionary<string>, FrameInnerReadOnlyDictionary<string>, string, IFrameInner> TestDictionaryInner = new(DictionaryToFrameHandler, NeutralKey, NeutralValue);
        FrameInnerDictionary<string> InnerDictionaryFromSource = new(new Dictionary<string, IFrameInner>());
        FrameInnerDictionary<string> InnerDictionaryWithCapacity = new(1);
        TestDictionaryInner.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameKeywordFrameList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameKeywordFrameList, FrameKeywordFrameReadOnlyList> ListToReadOnlyHandler = (FrameKeywordFrameList list) => list.ToReadOnly();
        IFrameKeywordFrame NeutralItem = FrameKeywordFrame.Empty;
        TestList<FrameKeywordFrameList, FrameKeywordFrameReadOnlyList, IFrameKeywordFrame> TestListKeywordFrame = new(ListToReadOnlyHandler, NeutralItem);
        TestListKeywordFrame.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameNodeStateDictionary()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameNodeStateDictionary, FrameNodeStateReadOnlyDictionary> DictionaryToFrameHandler = (FrameNodeStateDictionary dictionary) => (FrameNodeStateReadOnlyDictionary)dictionary.ToReadOnly();
        IFrameIndex NeutralKey = FrameRootNodeIndex.Empty;
        IFrameNodeState NeutralValue = FrameNodeState<IFrameInner<IFrameBrowsingChildIndex>>.Empty;
        TestDictionary<FrameNodeStateDictionary, FrameNodeStateReadOnlyDictionary, IFrameIndex, IFrameNodeState> TestDictionaryNodeState = new(DictionaryToFrameHandler, NeutralKey, NeutralValue);
        FrameNodeStateDictionary NodeStateDictionaryFromSource = new(new Dictionary<IFrameIndex, IFrameNodeState>());
        FrameNodeStateDictionary NodeStateDictionaryWithCapacity = new(1);
        TestDictionaryNodeState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameNodeStateList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameNodeStateList, FrameNodeStateReadOnlyList> ListToReadOnlyHandler = (FrameNodeStateList list) => (FrameNodeStateReadOnlyList)list.ToReadOnly();
        IFrameNodeState NeutralItem = FrameNodeState<IFrameInner<IFrameBrowsingChildIndex>>.Empty;
        TestList<FrameNodeStateList, FrameNodeStateReadOnlyList, IFrameNodeState> TestListNodeState = new(ListToReadOnlyHandler, NeutralItem);
        TestListNodeState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameNodeStateViewDictionary()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameNodeStateViewDictionary, FrameNodeStateViewReadOnlyDictionary> DictionaryToFrameHandler = (FrameNodeStateViewDictionary dictionary) => (FrameNodeStateViewReadOnlyDictionary)dictionary.ToReadOnly();
        IFrameNodeState NeutralKey = FrameNodeState<IFrameInner<IFrameBrowsingChildIndex>>.Empty;
        IFrameNodeStateView NeutralValue = FrameNodeStateView.Empty;
        TestDictionary<FrameNodeStateViewDictionary, FrameNodeStateViewReadOnlyDictionary, IFrameNodeState, IFrameNodeStateView> TestDictionaryNodeStateView = new(DictionaryToFrameHandler, NeutralKey, NeutralValue);
        FrameNodeStateViewDictionary NodeStateViewDictionaryFromSource = new(new Dictionary<IFrameNodeState, IFrameNodeStateView>());
        FrameNodeStateViewDictionary NodeStateViewDictionaryWithCapacity = new(1);
        TestDictionaryNodeStateView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameOperationGroupList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameOperationGroupList, FrameOperationGroupReadOnlyList> ListToReadOnlyHandler = (FrameOperationGroupList list) => (FrameOperationGroupReadOnlyList)list.ToReadOnly();
        FrameOperationGroup NeutralItem = FrameOperationGroup.Empty;
        TestList<FrameOperationGroupList, FrameOperationGroupReadOnlyList, FrameOperationGroup> TestListOperationGroup = new(ListToReadOnlyHandler, NeutralItem);
        TestListOperationGroup.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameOperationList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameOperationList, FrameOperationReadOnlyList> ListToReadOnlyHandler = (FrameOperationList list) => (FrameOperationReadOnlyList)list.ToReadOnly();
        IFrameOperation NeutralItem = FrameOperation.Empty;
        TestList<FrameOperationList, FrameOperationReadOnlyList, IFrameOperation> TestListOperation = new(ListToReadOnlyHandler, NeutralItem);
        TestListOperation.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFramePlaceholderNodeStateList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FramePlaceholderNodeStateList, FramePlaceholderNodeStateReadOnlyList> ListToReadOnlyHandler = (FramePlaceholderNodeStateList list) => (FramePlaceholderNodeStateReadOnlyList)list.ToReadOnly();
        IFramePlaceholderNodeState NeutralItem = FramePlaceholderNodeState<IFrameInner<IFrameBrowsingChildIndex>>.Empty;
        TestList<FramePlaceholderNodeStateList, FramePlaceholderNodeStateReadOnlyList, IFramePlaceholderNodeState> TestListPlaceholderNodeState = new(ListToReadOnlyHandler, NeutralItem);
        TestListPlaceholderNodeState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameTemplateDictionary()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameTemplateDictionary, FrameTemplateReadOnlyDictionary> DictionaryToFrameHandler = (FrameTemplateDictionary dictionary) => dictionary.ToReadOnly();
        Type NeutralKey = Type.FromTypeof<object>();
        IFrameTemplate NeutralValue = FrameTemplate.Empty;
        TestDictionary<FrameTemplateDictionary, FrameTemplateReadOnlyDictionary, Type, IFrameTemplate> TestDictionaryTemplate = new(DictionaryToFrameHandler, NeutralKey, NeutralValue);
        FrameTemplateDictionary TemplateDictionaryFromSource = new(new Dictionary<Type, IFrameTemplate>());
        FrameTemplateDictionary TemplateDictionaryWithCapacity = new(1);
        TestDictionaryTemplate.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameTemplateList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameTemplateList, FrameTemplateReadOnlyList> ListToReadOnlyHandler = (FrameTemplateList list) => list.ToReadOnly();
        IFrameTemplate NeutralItem = FrameTemplate.Empty;
        TestList<FrameTemplateList, FrameTemplateReadOnlyList, IFrameTemplate> TestListTemplate = new(ListToReadOnlyHandler, NeutralItem);
        TestListTemplate.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFrameVisibleCellViewList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<FrameVisibleCellViewList, FrameVisibleCellViewReadOnlyList> ListToReadOnlyHandler = (FrameVisibleCellViewList list) => list.ToReadOnly();
        IFrameVisibleCellView NeutralItem = FrameVisibleCellView.Empty;
        TestList<FrameVisibleCellViewList, FrameVisibleCellViewReadOnlyList, IFrameVisibleCellView> TestListVisibleCellView = new(ListToReadOnlyHandler, NeutralItem);
        TestListVisibleCellView.Test();
    }
}
