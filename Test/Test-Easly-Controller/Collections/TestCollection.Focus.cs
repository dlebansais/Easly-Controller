namespace Coverage;

using EaslyController;
using EaslyController.Focus;
using NUnit.Framework;
using System;
using System.Collections.Generic;

[TestFixture]
public class TestCollectionFocus
{
    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusAssignableCellViewDictionary()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusAssignableCellViewDictionary<string>, FocusAssignableCellViewReadOnlyDictionary<string>> DictionaryToReadOnlyHandler = (FocusAssignableCellViewDictionary<string> dictionary) => (FocusAssignableCellViewReadOnlyDictionary<string>)dictionary.ToReadOnly();
        string NeutralKey = string.Empty;
        IFocusAssignableCellView NeutralValue = FocusCellViewCollection.Empty;
        TestDictionary<FocusAssignableCellViewDictionary<string>, FocusAssignableCellViewReadOnlyDictionary<string>, string, IFocusAssignableCellView> TestDictionaryAssignableCellView = new(DictionaryToReadOnlyHandler, NeutralKey, NeutralValue);
        FocusAssignableCellViewDictionary<string> AssignableCellViewDictionaryFromSource = new(new Dictionary<string, IFocusAssignableCellView>());
        FocusAssignableCellViewDictionary<string> AssignableCellViewDictionaryWithCapacity = new(1);
        TestDictionaryAssignableCellView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusBlockStateList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusBlockStateList, FocusBlockStateReadOnlyList> ListToReadOnlyHandler = (FocusBlockStateList list) => (FocusBlockStateReadOnlyList)list.ToReadOnly();
        IFocusBlockState NeutralItem = FocusBlockState<IFocusInner<IFocusBrowsingChildIndex>>.Empty;
        TestList<FocusBlockStateList, FocusBlockStateReadOnlyList, IFocusBlockState> TestListBlockState = new(ListToReadOnlyHandler, NeutralItem);
        TestListBlockState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusBlockStateViewDictionary()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusBlockStateViewDictionary, FocusBlockStateViewReadOnlyDictionary> DictionaryToReadOnlyHandler = (FocusBlockStateViewDictionary dictionary) => (FocusBlockStateViewReadOnlyDictionary)dictionary.ToReadOnly();
        IFocusBlockState NeutralKey = FocusBlockState<IFocusInner<IFocusBrowsingChildIndex>>.Empty;
        FocusBlockStateView NeutralValue = FocusBlockStateView.Empty;
        TestDictionary<FocusBlockStateViewDictionary, FocusBlockStateViewReadOnlyDictionary, IFocusBlockState, FocusBlockStateView> TestDictionaryBlockStateView = new(DictionaryToReadOnlyHandler, NeutralKey, NeutralValue);
        FocusBlockStateViewDictionary BlockStateViewDictionaryFromSource = new(new Dictionary<IFocusBlockState, FocusBlockStateView>());
        FocusBlockStateViewDictionary BlockStateViewDictionaryWithCapacity = new(1);
        TestDictionaryBlockStateView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusBrowsingBlockNodeIndexList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusBrowsingBlockNodeIndexList, FocusBrowsingBlockNodeIndexReadOnlyList> ListToReadOnlyHandler = (FocusBrowsingBlockNodeIndexList list) => (FocusBrowsingBlockNodeIndexReadOnlyList)list.ToReadOnly();
        IFocusBrowsingBlockNodeIndex NeutralItem = FocusBrowsingNewBlockNodeIndex.Empty;
        TestList<FocusBrowsingBlockNodeIndexList, FocusBrowsingBlockNodeIndexReadOnlyList, IFocusBrowsingBlockNodeIndex> TestListBrowsingBlockNodeIndex = new(ListToReadOnlyHandler, NeutralItem);
        TestListBrowsingBlockNodeIndex.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusBrowsingListNodeIndexList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusBrowsingListNodeIndexList, FocusBrowsingListNodeIndexReadOnlyList> ListToReadOnlyHandler = (FocusBrowsingListNodeIndexList list) => (FocusBrowsingListNodeIndexReadOnlyList)list.ToReadOnly();
        IFocusBrowsingListNodeIndex NeutralItem = FocusBrowsingListNodeIndex.Empty;
        TestList<FocusBrowsingListNodeIndexList, FocusBrowsingListNodeIndexReadOnlyList, IFocusBrowsingListNodeIndex> TestListBrowsingListNodeIndex = new(ListToReadOnlyHandler, NeutralItem);
        TestListBrowsingListNodeIndex.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusCellViewList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusCellViewList, FocusCellViewReadOnlyList> ListToReadOnlyHandler = (FocusCellViewList list) => (FocusCellViewReadOnlyList)list.ToReadOnly();
        IFocusCellView NeutralItem = FocusCellViewCollection.Empty;
        TestList<FocusCellViewList, FocusCellViewReadOnlyList, IFocusCellView> TestListCellView = new(ListToReadOnlyHandler, NeutralItem);
        TestListCellView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusCycleManagerList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusCycleManagerList, FocusCycleManagerReadOnlyList> ListToReadOnlyHandler = (FocusCycleManagerList list) => list.ToReadOnly();
        IFocusCycleManager NeutralItem = FocusCycleManager.Empty;
        TestList<FocusCycleManagerList, FocusCycleManagerReadOnlyList, IFocusCycleManager> TestListCycleManager = new(ListToReadOnlyHandler, NeutralItem);
        TestListCycleManager.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusFocusList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusFocusList, FocusFocusReadOnlyList> ListToReadOnlyHandler = (FocusFocusList list) => (FocusFocusReadOnlyList)list.ToReadOnly();
        IFocusFocus NeutralItem = FocusFocus.Empty;
        TestList<FocusFocusList, FocusFocusReadOnlyList, IFocusFocus> TestListFocus = new(ListToReadOnlyHandler, NeutralItem);
        TestListFocus.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusFrameList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusFrameList, FocusFrameReadOnlyList> ListToReadOnlyHandler = (FocusFrameList list) => (FocusFrameReadOnlyList)list.ToReadOnly();
        IFocusFrame NeutralItem = FocusFrame.FocusRoot;
        TestList<FocusFrameList, FocusFrameReadOnlyList, IFocusFrame> TestListFocus = new(ListToReadOnlyHandler, NeutralItem);
        TestListFocus.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusFrameSelectorList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusFrameSelectorList, FocusFrameSelectorReadOnlyList> ListToReadOnlyHandler = (FocusFrameSelectorList list) => (FocusFrameSelectorReadOnlyList)list.ToReadOnly();
        IFocusFrameSelector NeutralItem = FocusFrameSelector.Empty;
        TestList<FocusFrameSelectorList, FocusFrameSelectorReadOnlyList, IFocusFrameSelector> TestListFocus = new(ListToReadOnlyHandler, NeutralItem);
        TestListFocus.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusIndexCollectionList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusIndexCollectionList, FocusIndexCollectionReadOnlyList> ListToReadOnlyHandler = (FocusIndexCollectionList list) => (FocusIndexCollectionReadOnlyList)list.ToReadOnly();
        IFocusIndexCollection NeutralItem = FocusIndexCollection<IFocusBrowsingChildIndex>.Empty;
        TestList<FocusIndexCollectionList, FocusIndexCollectionReadOnlyList, IFocusIndexCollection> TestListIndexCollection = new(ListToReadOnlyHandler, NeutralItem);
        TestListIndexCollection.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusInnerDictionary()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusInnerDictionary<string>, FocusInnerReadOnlyDictionary<string>> DictionaryToFocusHandler = (FocusInnerDictionary<string> dictionary) => (FocusInnerReadOnlyDictionary<string>)dictionary.ToReadOnly();
        string NeutralKey = string.Empty;
        IFocusInner NeutralValue = FocusInner<IFocusBrowsingChildIndex>.Empty;
        TestDictionary<FocusInnerDictionary<string>, FocusInnerReadOnlyDictionary<string>, string, IFocusInner> TestDictionaryInner = new(DictionaryToFocusHandler, NeutralKey, NeutralValue);
        FocusInnerDictionary<string> InnerDictionaryFromSource = new(new Dictionary<string, IFocusInner>());
        FocusInnerDictionary<string> InnerDictionaryWithCapacity = new(1);
        TestDictionaryInner.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusInsertionChildNodeIndexList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusInsertionChildNodeIndexList, FocusInsertionChildNodeIndexReadOnlyList> ListToReadOnlyHandler = (FocusInsertionChildNodeIndexList list) => list.ToReadOnly();
        IFocusInsertionChildNodeIndex NeutralItem = FocusInsertionEmptyNodeIndex.Empty;
        TestList<FocusInsertionChildNodeIndexList, FocusInsertionChildNodeIndexReadOnlyList, IFocusInsertionChildNodeIndex> TestListInsertionChildNodeIndex = new(ListToReadOnlyHandler, NeutralItem);
        TestListInsertionChildNodeIndex.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusKeywordFrameList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusKeywordFrameList, FocusKeywordFrameReadOnlyList> ListToReadOnlyHandler = (FocusKeywordFrameList list) => (FocusKeywordFrameReadOnlyList)list.ToReadOnly();
        IFocusKeywordFrame NeutralItem = FocusKeywordFrame.Empty;
        TestList<FocusKeywordFrameList, FocusKeywordFrameReadOnlyList, IFocusKeywordFrame> TestListKeywordFrame = new(ListToReadOnlyHandler, NeutralItem);
        TestListKeywordFrame.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusNodeFrameVisibilityList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusNodeFrameVisibilityList, FocusNodeFrameVisibilityReadOnlyList> ListToReadOnlyHandler = (FocusNodeFrameVisibilityList list) => list.ToReadOnly();
        IFocusNodeFrameVisibility NeutralItem = FocusComplexFrameVisibility.Empty;
        TestList<FocusNodeFrameVisibilityList, FocusNodeFrameVisibilityReadOnlyList, IFocusNodeFrameVisibility> TestListNodeFrameVisibility = new(ListToReadOnlyHandler, NeutralItem);
        TestListNodeFrameVisibility.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusNodeStateDictionary()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusNodeStateDictionary, FocusNodeStateReadOnlyDictionary> DictionaryToFocusHandler = (FocusNodeStateDictionary dictionary) => (FocusNodeStateReadOnlyDictionary)dictionary.ToReadOnly();
        IFocusIndex NeutralKey = FocusRootNodeIndex.Empty;
        IFocusNodeState NeutralValue = FocusNodeState<IFocusInner<IFocusBrowsingChildIndex>>.Empty;
        TestDictionary<FocusNodeStateDictionary, FocusNodeStateReadOnlyDictionary, IFocusIndex, IFocusNodeState> TestDictionaryNodeState = new(DictionaryToFocusHandler, NeutralKey, NeutralValue);
        FocusNodeStateDictionary NodeStateDictionaryFromSource = new(new Dictionary<IFocusIndex, IFocusNodeState>());
        FocusNodeStateDictionary NodeStateDictionaryWithCapacity = new(1);
        TestDictionaryNodeState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusNodeStateList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusNodeStateList, FocusNodeStateReadOnlyList> ListToReadOnlyHandler = (FocusNodeStateList list) => (FocusNodeStateReadOnlyList)list.ToReadOnly();
        IFocusNodeState NeutralItem = FocusNodeState<IFocusInner<IFocusBrowsingChildIndex>>.Empty;
        TestList<FocusNodeStateList, FocusNodeStateReadOnlyList, IFocusNodeState> TestListNodeState = new(ListToReadOnlyHandler, NeutralItem);
        TestListNodeState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusNodeStateViewDictionary()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusNodeStateViewDictionary, FocusNodeStateViewReadOnlyDictionary> DictionaryToFocusHandler = (FocusNodeStateViewDictionary dictionary) => (FocusNodeStateViewReadOnlyDictionary)dictionary.ToReadOnly();
        IFocusNodeState NeutralKey = FocusNodeState<IFocusInner<IFocusBrowsingChildIndex>>.Empty;
        IFocusNodeStateView NeutralValue = FocusNodeStateView.Empty;
        TestDictionary<FocusNodeStateViewDictionary, FocusNodeStateViewReadOnlyDictionary, IFocusNodeState, IFocusNodeStateView> TestDictionaryNodeStateView = new(DictionaryToFocusHandler, NeutralKey, NeutralValue);
        FocusNodeStateViewDictionary NodeStateViewDictionaryFromSource = new(new Dictionary<IFocusNodeState, IFocusNodeStateView>());
        FocusNodeStateViewDictionary NodeStateViewDictionaryWithCapacity = new(1);
        TestDictionaryNodeStateView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusOperationGroupList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusOperationGroupList, FocusOperationGroupReadOnlyList> ListToReadOnlyHandler = (FocusOperationGroupList list) => (FocusOperationGroupReadOnlyList)list.ToReadOnly();
        FocusOperationGroup NeutralItem = FocusOperationGroup.Empty;
        TestList<FocusOperationGroupList, FocusOperationGroupReadOnlyList, FocusOperationGroup> TestListOperationGroup = new(ListToReadOnlyHandler, NeutralItem);
        TestListOperationGroup.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusOperationList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusOperationList, FocusOperationReadOnlyList> ListToReadOnlyHandler = (FocusOperationList list) => (FocusOperationReadOnlyList)list.ToReadOnly();
        IFocusOperation NeutralItem = FocusOperation.Empty;
        TestList<FocusOperationList, FocusOperationReadOnlyList, IFocusOperation> TestListOperation = new(ListToReadOnlyHandler, NeutralItem);
        TestListOperation.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusPlaceholderNodeStateList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusPlaceholderNodeStateList, FocusPlaceholderNodeStateReadOnlyList> ListToReadOnlyHandler = (FocusPlaceholderNodeStateList list) => (FocusPlaceholderNodeStateReadOnlyList)list.ToReadOnly();
        IFocusPlaceholderNodeState NeutralItem = FocusPlaceholderNodeState<IFocusInner<IFocusBrowsingChildIndex>>.Empty;
        TestList<FocusPlaceholderNodeStateList, FocusPlaceholderNodeStateReadOnlyList, IFocusPlaceholderNodeState> TestListPlaceholderNodeState = new(ListToReadOnlyHandler, NeutralItem);
        TestListPlaceholderNodeState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusSelectableFrameList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusSelectableFrameList, FocusSelectableFrameReadOnlyList> ListToReadOnlyHandler = (FocusSelectableFrameList list) => list.ToReadOnly();
        IFocusSelectableFrame NeutralItem = FocusSelectableFrame.Empty;
        TestList<FocusSelectableFrameList, FocusSelectableFrameReadOnlyList, IFocusSelectableFrame> TestListSelectableFrame = new(ListToReadOnlyHandler, NeutralItem);
        TestListSelectableFrame.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusTemplateDictionary()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusTemplateDictionary, FocusTemplateReadOnlyDictionary> DictionaryToFocusHandler = (FocusTemplateDictionary dictionary) => (FocusTemplateReadOnlyDictionary)dictionary.ToReadOnly();
        Type NeutralKey = typeof(object);
        IFocusTemplate NeutralValue = FocusTemplate.Empty;
        TestDictionary<FocusTemplateDictionary, FocusTemplateReadOnlyDictionary, Type, IFocusTemplate> TestDictionaryTemplate = new(DictionaryToFocusHandler, NeutralKey, NeutralValue);
        FocusTemplateDictionary TemplateDictionaryFromSource = new(new Dictionary<Type, IFocusTemplate>());
        FocusTemplateDictionary TemplateDictionaryWithCapacity = new(1);
        TestDictionaryTemplate.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusTemplateList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusTemplateList, FocusTemplateReadOnlyList> ListToReadOnlyHandler = (FocusTemplateList list) => (FocusTemplateReadOnlyList)list.ToReadOnly();
        IFocusTemplate NeutralItem = FocusTemplate.Empty;
        TestList<FocusTemplateList, FocusTemplateReadOnlyList, IFocusTemplate> TestListTemplate = new(ListToReadOnlyHandler, NeutralItem);
        TestListTemplate.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestFocusVisibleCellViewList()
    {
        ControllerTools.ResetExpectedName();

        Func<FocusVisibleCellViewList, FocusVisibleCellViewReadOnlyList> ListToReadOnlyHandler = (FocusVisibleCellViewList list) => (FocusVisibleCellViewReadOnlyList)list.ToReadOnly();
        IFocusVisibleCellView NeutralItem = FocusVisibleCellView.Empty;
        TestList<FocusVisibleCellViewList, FocusVisibleCellViewReadOnlyList, IFocusVisibleCellView> TestListVisibleCellView = new(ListToReadOnlyHandler, NeutralItem);
        TestListVisibleCellView.Test();
    }
}
