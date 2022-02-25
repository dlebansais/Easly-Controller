namespace Coverage;

using EaslyController;
using EaslyController.Layout;
using NUnit.Framework;
using System.Collections.Generic;
using NotNullReflection;

[TestFixture]
public class TestCollectionLayout
{
    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutAssignableCellViewDictionary()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutAssignableCellViewDictionary<string>, LayoutAssignableCellViewReadOnlyDictionary<string>> DictionaryToReadOnlyHandler = (LayoutAssignableCellViewDictionary<string> dictionary) => (LayoutAssignableCellViewReadOnlyDictionary<string>)dictionary.ToReadOnly();
        string NeutralKey = string.Empty;
        ILayoutAssignableCellView NeutralValue = LayoutCellViewCollection.Empty;
        TestDictionary<LayoutAssignableCellViewDictionary<string>, LayoutAssignableCellViewReadOnlyDictionary<string>, string, ILayoutAssignableCellView> TestDictionaryAssignableCellView = new(DictionaryToReadOnlyHandler, NeutralKey, NeutralValue);
        LayoutAssignableCellViewDictionary<string> AssignableCellViewDictionaryFromSource = new(new Dictionary<string, ILayoutAssignableCellView>());
        LayoutAssignableCellViewDictionary<string> AssignableCellViewDictionaryWithCapacity = new(1);
        TestDictionaryAssignableCellView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutBlockStateList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutBlockStateList, LayoutBlockStateReadOnlyList> ListToReadOnlyHandler = (LayoutBlockStateList list) => (LayoutBlockStateReadOnlyList)list.ToReadOnly();
        ILayoutBlockState NeutralItem = LayoutBlockState<ILayoutInner<ILayoutBrowsingChildIndex>>.Empty;
        TestList<LayoutBlockStateList, LayoutBlockStateReadOnlyList, ILayoutBlockState> TestListBlockState = new(ListToReadOnlyHandler, NeutralItem);
        TestListBlockState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutBlockStateViewDictionary()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutBlockStateViewDictionary, LayoutBlockStateViewReadOnlyDictionary> DictionaryToReadOnlyHandler = (LayoutBlockStateViewDictionary dictionary) => (LayoutBlockStateViewReadOnlyDictionary)dictionary.ToReadOnly();
        ILayoutBlockState NeutralKey = LayoutBlockState<ILayoutInner<ILayoutBrowsingChildIndex>>.Empty;
        LayoutBlockStateView NeutralValue = LayoutBlockStateView.Empty;
        TestDictionary<LayoutBlockStateViewDictionary, LayoutBlockStateViewReadOnlyDictionary, ILayoutBlockState, LayoutBlockStateView> TestDictionaryBlockStateView = new(DictionaryToReadOnlyHandler, NeutralKey, NeutralValue);
        LayoutBlockStateViewDictionary BlockStateViewDictionaryFromSource = new(new Dictionary<ILayoutBlockState, LayoutBlockStateView>());
        LayoutBlockStateViewDictionary BlockStateViewDictionaryWithCapacity = new(1);
        TestDictionaryBlockStateView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutBrowsingBlockNodeIndexList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutBrowsingBlockNodeIndexList, LayoutBrowsingBlockNodeIndexReadOnlyList> ListToReadOnlyHandler = (LayoutBrowsingBlockNodeIndexList list) => (LayoutBrowsingBlockNodeIndexReadOnlyList)list.ToReadOnly();
        ILayoutBrowsingBlockNodeIndex NeutralItem = LayoutBrowsingNewBlockNodeIndex.Empty;
        TestList<LayoutBrowsingBlockNodeIndexList, LayoutBrowsingBlockNodeIndexReadOnlyList, ILayoutBrowsingBlockNodeIndex> TestListBrowsingBlockNodeIndex = new(ListToReadOnlyHandler, NeutralItem);
        TestListBrowsingBlockNodeIndex.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutBrowsingListNodeIndexList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutBrowsingListNodeIndexList, LayoutBrowsingListNodeIndexReadOnlyList> ListToReadOnlyHandler = (LayoutBrowsingListNodeIndexList list) => (LayoutBrowsingListNodeIndexReadOnlyList)list.ToReadOnly();
        ILayoutBrowsingListNodeIndex NeutralItem = LayoutBrowsingListNodeIndex.Empty;
        TestList<LayoutBrowsingListNodeIndexList, LayoutBrowsingListNodeIndexReadOnlyList, ILayoutBrowsingListNodeIndex> TestListBrowsingListNodeIndex = new(ListToReadOnlyHandler, NeutralItem);
        TestListBrowsingListNodeIndex.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutCellViewList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutCellViewList, LayoutCellViewReadOnlyList> ListToReadOnlyHandler = (LayoutCellViewList list) => (LayoutCellViewReadOnlyList)list.ToReadOnly();
        ILayoutCellView NeutralItem = LayoutCellViewCollection.Empty;
        TestList<LayoutCellViewList, LayoutCellViewReadOnlyList, ILayoutCellView> TestListCellView = new(ListToReadOnlyHandler, NeutralItem);
        TestListCellView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutCycleManagerList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutCycleManagerList, LayoutCycleManagerReadOnlyList> ListToReadOnlyHandler = (LayoutCycleManagerList list) => (LayoutCycleManagerReadOnlyList)list.ToReadOnly();
        ILayoutCycleManager NeutralItem = LayoutCycleManager.Empty;
        TestList<LayoutCycleManagerList, LayoutCycleManagerReadOnlyList, ILayoutCycleManager> TestListCycleManager = new(ListToReadOnlyHandler, NeutralItem);
        TestListCycleManager.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutFocusList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutFocusList, LayoutFocusReadOnlyList> ListToReadOnlyHandler = (LayoutFocusList list) => (LayoutFocusReadOnlyList)list.ToReadOnly();
        ILayoutFocus NeutralItem = LayoutFocus.Empty;
        TestList<LayoutFocusList, LayoutFocusReadOnlyList, ILayoutFocus> TestListLayout = new(ListToReadOnlyHandler, NeutralItem);
        TestListLayout.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutFrameList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutFrameList, LayoutFrameReadOnlyList> ListToReadOnlyHandler = (LayoutFrameList list) => (LayoutFrameReadOnlyList)list.ToReadOnly();
        ILayoutFrame NeutralItem = LayoutFrame.LayoutRoot;
        TestList<LayoutFrameList, LayoutFrameReadOnlyList, ILayoutFrame> TestListLayout = new(ListToReadOnlyHandler, NeutralItem);
        TestListLayout.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutFrameSelectorList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutFrameSelectorList, LayoutFrameSelectorReadOnlyList> ListToReadOnlyHandler = (LayoutFrameSelectorList list) => (LayoutFrameSelectorReadOnlyList)list.ToReadOnly();
        ILayoutFrameSelector NeutralItem = LayoutFrameSelector.Empty;
        TestList<LayoutFrameSelectorList, LayoutFrameSelectorReadOnlyList, ILayoutFrameSelector> TestListLayout = new(ListToReadOnlyHandler, NeutralItem);
        TestListLayout.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutIndexCollectionList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutIndexCollectionList, LayoutIndexCollectionReadOnlyList> ListToReadOnlyHandler = (LayoutIndexCollectionList list) => (LayoutIndexCollectionReadOnlyList)list.ToReadOnly();
        ILayoutIndexCollection NeutralItem = LayoutIndexCollection<ILayoutBrowsingChildIndex>.Empty;
        TestList<LayoutIndexCollectionList, LayoutIndexCollectionReadOnlyList, ILayoutIndexCollection> TestListIndexCollection = new(ListToReadOnlyHandler, NeutralItem);
        TestListIndexCollection.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutInnerDictionary()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutInnerDictionary<string>, LayoutInnerReadOnlyDictionary<string>> DictionaryToLayoutHandler = (LayoutInnerDictionary<string> dictionary) => (LayoutInnerReadOnlyDictionary<string>)dictionary.ToReadOnly();
        string NeutralKey = string.Empty;
        ILayoutInner NeutralValue = LayoutInner<ILayoutBrowsingChildIndex>.Empty;
        TestDictionary<LayoutInnerDictionary<string>, LayoutInnerReadOnlyDictionary<string>, string, ILayoutInner> TestDictionaryInner = new(DictionaryToLayoutHandler, NeutralKey, NeutralValue);
        LayoutInnerDictionary<string> InnerDictionaryFromSource = new(new Dictionary<string, ILayoutInner>());
        LayoutInnerDictionary<string> InnerDictionaryWithCapacity = new(1);
        TestDictionaryInner.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutInsertionChildNodeIndexList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutInsertionChildNodeIndexList, LayoutInsertionChildNodeIndexReadOnlyList> ListToReadOnlyHandler = (LayoutInsertionChildNodeIndexList list) => (LayoutInsertionChildNodeIndexReadOnlyList)list.ToReadOnly();
        ILayoutInsertionChildNodeIndex NeutralItem = LayoutInsertionEmptyNodeIndex.Empty;
        TestList<LayoutInsertionChildNodeIndexList, LayoutInsertionChildNodeIndexReadOnlyList, ILayoutInsertionChildNodeIndex> TestListInsertionChildNodeIndex = new(ListToReadOnlyHandler, NeutralItem);
        TestListInsertionChildNodeIndex.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutKeywordFrameList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutKeywordFrameList, LayoutKeywordFrameReadOnlyList> ListToReadOnlyHandler = (LayoutKeywordFrameList list) => (LayoutKeywordFrameReadOnlyList)list.ToReadOnly();
        ILayoutKeywordFrame NeutralItem = LayoutKeywordFrame.Empty;
        TestList<LayoutKeywordFrameList, LayoutKeywordFrameReadOnlyList, ILayoutKeywordFrame> TestListKeywordFrame = new(ListToReadOnlyHandler, NeutralItem);
        TestListKeywordFrame.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutNodeFrameVisibilityList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutNodeFrameVisibilityList, LayoutNodeFrameVisibilityReadOnlyList> ListToReadOnlyHandler = (LayoutNodeFrameVisibilityList list) => (LayoutNodeFrameVisibilityReadOnlyList)list.ToReadOnly();
        ILayoutNodeFrameVisibility NeutralItem = LayoutComplexFrameVisibility.Empty;
        TestList<LayoutNodeFrameVisibilityList, LayoutNodeFrameVisibilityReadOnlyList, ILayoutNodeFrameVisibility> TestListNodeFrameVisibility = new(ListToReadOnlyHandler, NeutralItem);
        TestListNodeFrameVisibility.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutNodeStateDictionary()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutNodeStateDictionary, LayoutNodeStateReadOnlyDictionary> DictionaryToLayoutHandler = (LayoutNodeStateDictionary dictionary) => (LayoutNodeStateReadOnlyDictionary)dictionary.ToReadOnly();
        ILayoutIndex NeutralKey = LayoutRootNodeIndex.Empty;
        ILayoutNodeState NeutralValue = LayoutNodeState<ILayoutInner<ILayoutBrowsingChildIndex>>.Empty;
        TestDictionary<LayoutNodeStateDictionary, LayoutNodeStateReadOnlyDictionary, ILayoutIndex, ILayoutNodeState> TestDictionaryNodeState = new(DictionaryToLayoutHandler, NeutralKey, NeutralValue);
        LayoutNodeStateDictionary NodeStateDictionaryFromSource = new(new Dictionary<ILayoutIndex, ILayoutNodeState>());
        LayoutNodeStateDictionary NodeStateDictionaryWithCapacity = new(1);
        TestDictionaryNodeState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutNodeStateList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutNodeStateList, LayoutNodeStateReadOnlyList> ListToReadOnlyHandler = (LayoutNodeStateList list) => (LayoutNodeStateReadOnlyList)list.ToReadOnly();
        ILayoutNodeState NeutralItem = LayoutNodeState<ILayoutInner<ILayoutBrowsingChildIndex>>.Empty;
        TestList<LayoutNodeStateList, LayoutNodeStateReadOnlyList, ILayoutNodeState> TestListNodeState = new(ListToReadOnlyHandler, NeutralItem);
        TestListNodeState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutNodeStateViewDictionary()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutNodeStateViewDictionary, LayoutNodeStateViewReadOnlyDictionary> DictionaryToLayoutHandler = (LayoutNodeStateViewDictionary dictionary) => (LayoutNodeStateViewReadOnlyDictionary)dictionary.ToReadOnly();
        ILayoutNodeState NeutralKey = LayoutNodeState<ILayoutInner<ILayoutBrowsingChildIndex>>.Empty;
        ILayoutNodeStateView NeutralValue = LayoutNodeStateView.Empty;
        TestDictionary<LayoutNodeStateViewDictionary, LayoutNodeStateViewReadOnlyDictionary, ILayoutNodeState, ILayoutNodeStateView> TestDictionaryNodeStateView = new(DictionaryToLayoutHandler, NeutralKey, NeutralValue);
        LayoutNodeStateViewDictionary NodeStateViewDictionaryFromSource = new(new Dictionary<ILayoutNodeState, ILayoutNodeStateView>());
        LayoutNodeStateViewDictionary NodeStateViewDictionaryWithCapacity = new(1);
        TestDictionaryNodeStateView.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutOperationGroupList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutOperationGroupList, LayoutOperationGroupReadOnlyList> ListToReadOnlyHandler = (LayoutOperationGroupList list) => (LayoutOperationGroupReadOnlyList)list.ToReadOnly();
        LayoutOperationGroup NeutralItem = LayoutOperationGroup.Empty;
        TestList<LayoutOperationGroupList, LayoutOperationGroupReadOnlyList, LayoutOperationGroup> TestListOperationGroup = new(ListToReadOnlyHandler, NeutralItem);
        TestListOperationGroup.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutOperationList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutOperationList, LayoutOperationReadOnlyList> ListToReadOnlyHandler = (LayoutOperationList list) => (LayoutOperationReadOnlyList)list.ToReadOnly();
        ILayoutOperation NeutralItem = LayoutOperation.Empty;
        TestList<LayoutOperationList, LayoutOperationReadOnlyList, ILayoutOperation> TestListOperation = new(ListToReadOnlyHandler, NeutralItem);
        TestListOperation.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutPlaceholderNodeStateList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutPlaceholderNodeStateList, LayoutPlaceholderNodeStateReadOnlyList> ListToReadOnlyHandler = (LayoutPlaceholderNodeStateList list) => (LayoutPlaceholderNodeStateReadOnlyList)list.ToReadOnly();
        ILayoutPlaceholderNodeState NeutralItem = LayoutPlaceholderNodeState<ILayoutInner<ILayoutBrowsingChildIndex>>.Empty;
        TestList<LayoutPlaceholderNodeStateList, LayoutPlaceholderNodeStateReadOnlyList, ILayoutPlaceholderNodeState> TestListPlaceholderNodeState = new(ListToReadOnlyHandler, NeutralItem);
        TestListPlaceholderNodeState.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutSelectableFrameList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutSelectableFrameList, LayoutSelectableFrameReadOnlyList> ListToReadOnlyHandler = (LayoutSelectableFrameList list) => (LayoutSelectableFrameReadOnlyList)list.ToReadOnly();
        ILayoutSelectableFrame NeutralItem = LayoutSelectableFrame.Empty;
        TestList<LayoutSelectableFrameList, LayoutSelectableFrameReadOnlyList, ILayoutSelectableFrame> TestListSelectableFrame = new(ListToReadOnlyHandler, NeutralItem);
        TestListSelectableFrame.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutTemplateDictionary()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutTemplateDictionary, LayoutTemplateReadOnlyDictionary> DictionaryToLayoutHandler = (LayoutTemplateDictionary dictionary) => (LayoutTemplateReadOnlyDictionary)dictionary.ToReadOnly();
        Type NeutralKey = Type.FromTypeof<object>();
        ILayoutTemplate NeutralValue = LayoutTemplate.Empty;
        TestDictionary<LayoutTemplateDictionary, LayoutTemplateReadOnlyDictionary, Type, ILayoutTemplate> TestDictionaryTemplate = new(DictionaryToLayoutHandler, NeutralKey, NeutralValue);
        LayoutTemplateDictionary TemplateDictionaryFromSource = new(new Dictionary<Type, ILayoutTemplate>());
        LayoutTemplateDictionary TemplateDictionaryWithCapacity = new(1);
        TestDictionaryTemplate.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutTemplateList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutTemplateList, LayoutTemplateReadOnlyList> ListToReadOnlyHandler = (LayoutTemplateList list) => (LayoutTemplateReadOnlyList)list.ToReadOnly();
        ILayoutTemplate NeutralItem = LayoutTemplate.Empty;
        TestList<LayoutTemplateList, LayoutTemplateReadOnlyList, ILayoutTemplate> TestListTemplate = new(ListToReadOnlyHandler, NeutralItem);
        TestListTemplate.Test();
    }

    [Test]
    [Category("CollectionCoverage")]
    public static void TestLayoutVisibleCellViewList()
    {
        ControllerTools.ResetExpectedName();

        System.Func<LayoutVisibleCellViewList, LayoutVisibleCellViewReadOnlyList> ListToReadOnlyHandler = (LayoutVisibleCellViewList list) => (LayoutVisibleCellViewReadOnlyList)list.ToReadOnly();
        ILayoutVisibleCellView NeutralItem = LayoutVisibleCellView.Empty;
        TestList<LayoutVisibleCellViewList, LayoutVisibleCellViewReadOnlyList, ILayoutVisibleCellView> TestListVisibleCellView = new(ListToReadOnlyHandler, NeutralItem);
        TestListVisibleCellView.Test();
    }
}
