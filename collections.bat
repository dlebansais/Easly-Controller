@echo off
setlocal

set GEN=C:\Projects\CollectionGenerator\CollectionGenerator\bin\x64\Debug\net5.0\CollectionGenerator.exe

cd C:\Projects\Easly-Controller\Easly-Controller\01-ReadOnly\Collections

%GEN% List EaslyController.ReadOnly IReadOnlyBlockState
%GEN% ReadOnlyList EaslyController.ReadOnly IReadOnlyBlockState
%GEN% Dictionary EaslyController.ReadOnly IEqualComparable IReadOnlyBlockState ReadOnlyBlockStateView
%GEN% ReadOnlyDictionary EaslyController.ReadOnly IEqualComparable IReadOnlyBlockState ReadOnlyBlockStateView
%GEN% List EaslyController.ReadOnly IReadOnlyBrowsingBlockNodeIndex
%GEN% ReadOnlyList EaslyController.ReadOnly IReadOnlyBrowsingBlockNodeIndex
%GEN% List EaslyController.ReadOnly IReadOnlyBrowsingListNodeIndex
%GEN% ReadOnlyList EaslyController.ReadOnly IReadOnlyBrowsingListNodeIndex
%GEN% List EaslyController.ReadOnly IReadOnlyIndexCollection
%GEN% ReadOnlyList EaslyController.ReadOnly IReadOnlyIndexCollection
%GEN% Dictionary EaslyController.ReadOnly IReadOnlyIndex IReadOnlyNodeState
%GEN% ReadOnlyDictionary EaslyController.ReadOnly IReadOnlyIndex IReadOnlyNodeState
%GEN% DictionaryTKey EaslyController.ReadOnly IEqualComparable IReadOnlyInner
%GEN% ReadOnlyDictionaryTKey EaslyController.ReadOnly IEqualComparable IReadOnlyInner
%GEN% List EaslyController.ReadOnly IReadOnlyNodeState
%GEN% ReadOnlyList EaslyController.ReadOnly IReadOnlyNodeState
%GEN% List EaslyController.ReadOnly IEqualComparable IReadOnlyPlaceholderNodeState
%GEN% ReadOnlyList EaslyController.ReadOnly IEqualComparable IReadOnlyPlaceholderNodeState
%GEN% Dictionary EaslyController.ReadOnly IEqualComparable IReadOnlyNodeState IReadOnlyNodeStateView
%GEN% ReadOnlyDictionary EaslyController.ReadOnly IEqualComparable IReadOnlyNodeState IReadOnlyNodeStateView

cd C:\Projects\Easly-Controller\Easly-Controller\02-Writeable\Collections

%GEN% List EaslyController.Writeable IWriteableBlockState EaslyController.ReadOnly.IReadOnlyBlockState
%GEN% ReadOnlyList EaslyController.Writeable IWriteableBlockState EaslyController.ReadOnly.IReadOnlyBlockState
%GEN% Dictionary EaslyController.Writeable IEqualComparable IWriteableBlockState WriteableBlockStateView IReadOnlyBlockState EaslyController.ReadOnly.ReadOnlyBlockStateView
%GEN% ReadOnlyDictionary EaslyController.Writeable IEqualComparable IWriteableBlockState WriteableBlockStateView IReadOnlyBlockState EaslyController.ReadOnly.ReadOnlyBlockStateView
%GEN% List EaslyController.Writeable IWriteableBrowsingBlockNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingBlockNodeIndex
%GEN% ReadOnlyList EaslyController.Writeable IWriteableBrowsingBlockNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingBlockNodeIndex
%GEN% List EaslyController.Writeable IWriteableBrowsingListNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingListNodeIndex
%GEN% ReadOnlyList EaslyController.Writeable IWriteableBrowsingListNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingListNodeIndex
%GEN% List EaslyController.Writeable IWriteableIndexCollection EaslyController.ReadOnly.IReadOnlyIndexCollection
%GEN% ReadOnlyList EaslyController.Writeable IWriteableIndexCollection EaslyController.ReadOnly.IReadOnlyIndexCollection
%GEN% Dictionary EaslyController.Writeable IWriteableIndex IWriteableNodeState IReadOnlyIndex EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% ReadOnlyDictionary EaslyController.Writeable IWriteableIndex IWriteableNodeState IReadOnlyIndex EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% DictionaryTKey EaslyController.Writeable IEqualComparable IWriteableInner EaslyController.ReadOnly.IReadOnlyInner
%GEN% ReadOnlyDictionaryTKey EaslyController.Writeable IEqualComparable IWriteableInner EaslyController.ReadOnly.IReadOnlyInner
%GEN% List EaslyController.Writeable IWriteableNodeState EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% ReadOnlyList EaslyController.Writeable IWriteableNodeState EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% List EaslyController.Writeable IEqualComparable IWriteablePlaceholderNodeState EaslyController.ReadOnly.IReadOnlyPlaceholderNodeState
%GEN% ReadOnlyList EaslyController.Writeable IEqualComparable IWriteablePlaceholderNodeState EaslyController.ReadOnly.IReadOnlyPlaceholderNodeState
%GEN% Dictionary EaslyController.Writeable IEqualComparable IWriteableNodeState IWriteableNodeStateView IReadOnlyNodeState EaslyController.ReadOnly.IReadOnlyNodeStateView
%GEN% ReadOnlyDictionary EaslyController.Writeable IEqualComparable IWriteableNodeState IWriteableNodeStateView IReadOnlyNodeState EaslyController.ReadOnly.IReadOnlyNodeStateView
%GEN% List EaslyController.Writeable IWriteableOperation
%GEN% ReadOnlyList EaslyController.Writeable IWriteableOperation
%GEN% List EaslyController.Writeable WriteableOperationGroup
%GEN% ReadOnlyList EaslyController.Writeable WriteableOperationGroup

cd C:\Projects\Easly-Controller\Easly-Controller\03-Frame\Collections

%GEN% List EaslyController.Frame IFrameBlockState EaslyController.Writeable.IWriteableBlockState EaslyController.ReadOnly.IReadOnlyBlockState
%GEN% ReadOnlyList EaslyController.Frame IFrameBlockState EaslyController.Writeable.IWriteableBlockState EaslyController.ReadOnly.IReadOnlyBlockState
%GEN% Dictionary EaslyController.Frame IEqualComparable IFrameBlockState FrameBlockStateView IWriteableBlockState EaslyController.Writeable.WriteableBlockStateView IReadOnlyBlockState EaslyController.ReadOnly.ReadOnlyBlockStateView
%GEN% ReadOnlyDictionary EaslyController.Frame IEqualComparable IFrameBlockState FrameBlockStateView IWriteableBlockState EaslyController.Writeable.WriteableBlockStateView IReadOnlyBlockState EaslyController.ReadOnly.ReadOnlyBlockStateView
%GEN% List EaslyController.Frame IFrameBrowsingBlockNodeIndex EaslyController.Writeable.IWriteableBrowsingBlockNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingBlockNodeIndex
%GEN% ReadOnlyList EaslyController.Frame IFrameBrowsingBlockNodeIndex EaslyController.Writeable.IWriteableBrowsingBlockNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingBlockNodeIndex
%GEN% List EaslyController.Frame IFrameBrowsingListNodeIndex EaslyController.Writeable.IWriteableBrowsingListNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingListNodeIndex
%GEN% ReadOnlyList EaslyController.Frame IFrameBrowsingListNodeIndex EaslyController.Writeable.IWriteableBrowsingListNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingListNodeIndex
%GEN% List EaslyController.Frame IFrameIndexCollection EaslyController.Writeable.IWriteableIndexCollection EaslyController.ReadOnly.IReadOnlyIndexCollection
%GEN% ReadOnlyList EaslyController.Frame IFrameIndexCollection EaslyController.Writeable.IWriteableIndexCollection EaslyController.ReadOnly.IReadOnlyIndexCollection
%GEN% Dictionary EaslyController.Frame IFrameIndex IFrameNodeState IWriteableIndex EaslyController.Writeable.IWriteableNodeState IReadOnlyIndex EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% ReadOnlyDictionary EaslyController.Frame IFrameIndex IFrameNodeState IWriteableIndex EaslyController.Writeable.IWriteableNodeState IReadOnlyIndex EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% DictionaryTKey EaslyController.Frame IEqualComparable IFrameInner EaslyController.Writeable.IWriteableInner EaslyController.ReadOnly.IReadOnlyInner
%GEN% ReadOnlyDictionaryTKey EaslyController.Frame IEqualComparable IFrameInner EaslyController.Writeable.IWriteableInner EaslyController.ReadOnly.IReadOnlyInner
%GEN% List EaslyController.Frame IFrameNodeState EaslyController.Writeable.IWriteableNodeState EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% ReadOnlyList EaslyController.Frame IFrameNodeState EaslyController.Writeable.IWriteableNodeState EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% List EaslyController.Frame IEqualComparable IFramePlaceholderNodeState EaslyController.Writeable.IWriteablePlaceholderNodeState EaslyController.ReadOnly.IReadOnlyPlaceholderNodeState
%GEN% ReadOnlyList EaslyController.Frame IEqualComparable IFramePlaceholderNodeState EaslyController.Writeable.IWriteablePlaceholderNodeState EaslyController.ReadOnly.IReadOnlyPlaceholderNodeState
%GEN% Dictionary EaslyController.Frame IEqualComparable IFrameNodeState IFrameNodeStateView IWriteableNodeState EaslyController.Writeable.IWriteableNodeStateView IReadOnlyNodeState EaslyController.ReadOnly.IReadOnlyNodeStateView
%GEN% ReadOnlyDictionary EaslyController.Frame IEqualComparable IFrameNodeState IFrameNodeStateView IWriteableNodeState EaslyController.Writeable.IWriteableNodeStateView IReadOnlyNodeState EaslyController.ReadOnly.IReadOnlyNodeStateView
%GEN% List EaslyController.Frame IFrameOperation EaslyController.Writeable.IWriteableOperation
%GEN% ReadOnlyList EaslyController.Frame IFrameOperation EaslyController.Writeable.IWriteableOperation
%GEN% List EaslyController.Frame FrameOperationGroup EaslyController.Writeable.WriteableOperationGroup
%GEN% ReadOnlyList EaslyController.Frame FrameOperationGroup EaslyController.Writeable.WriteableOperationGroup
%GEN% DictionaryTKey EaslyController.Frame IEqualComparable IFrameAssignableCellView
%GEN% ReadOnlyDictionaryTKey EaslyController.Frame IEqualComparable IFrameAssignableCellView
%GEN% List EaslyController.Frame IEqualComparable IFrameCellView
%GEN% ReadOnlyList EaslyController.Frame IEqualComparable IFrameCellView
%GEN% List EaslyController.Frame IFrameFrame
%GEN% ReadOnlyList EaslyController.Frame IFrameFrame
%GEN% List EaslyController.Frame IFrameKeywordFrame
%GEN% ReadOnlyList EaslyController.Frame IFrameKeywordFrame
%GEN% Dictionary EaslyController.Frame System.Type IFrameTemplate
%GEN% ReadOnlyDictionary EaslyController.Frame System.Type IFrameTemplate
%GEN% List EaslyController.Frame IFrameTemplate
%GEN% ReadOnlyList EaslyController.Frame IFrameTemplate
%GEN% List EaslyController.Frame IFrameVisibleCellView
%GEN% ReadOnlyList EaslyController.Frame IFrameVisibleCellView

cd C:\Projects\Easly-Controller\Easly-Controller\04-Focus\Collections

%GEN% List EaslyController.Focus IFocusBlockState EaslyController.Frame.IFrameBlockState EaslyController.ReadOnly.IReadOnlyBlockState
%GEN% ReadOnlyList EaslyController.Focus IFocusBlockState EaslyController.Frame.IFrameBlockState EaslyController.ReadOnly.IReadOnlyBlockState
%GEN% Dictionary EaslyController.Focus IEqualComparable IFocusBlockState FocusBlockStateView IFrameBlockState EaslyController.Frame.FrameBlockStateView IReadOnlyBlockState EaslyController.ReadOnly.ReadOnlyBlockStateView
%GEN% ReadOnlyDictionary EaslyController.Focus IEqualComparable IFocusBlockState FocusBlockStateView IFrameBlockState EaslyController.Frame.FrameBlockStateView IReadOnlyBlockState EaslyController.ReadOnly.ReadOnlyBlockStateView
%GEN% List EaslyController.Focus IFocusBrowsingBlockNodeIndex EaslyController.Frame.IFrameBrowsingBlockNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingBlockNodeIndex
%GEN% ReadOnlyList EaslyController.Focus IFocusBrowsingBlockNodeIndex EaslyController.Frame.IFrameBrowsingBlockNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingBlockNodeIndex
%GEN% List EaslyController.Focus IFocusBrowsingListNodeIndex EaslyController.Frame.IFrameBrowsingListNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingListNodeIndex
%GEN% ReadOnlyList EaslyController.Focus IFocusBrowsingListNodeIndex EaslyController.Frame.IFrameBrowsingListNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingListNodeIndex
%GEN% List EaslyController.Focus IFocusIndexCollection EaslyController.Frame.IFrameIndexCollection EaslyController.ReadOnly.IReadOnlyIndexCollection
%GEN% ReadOnlyList EaslyController.Focus IFocusIndexCollection EaslyController.Frame.IFrameIndexCollection EaslyController.ReadOnly.IReadOnlyIndexCollection
%GEN% Dictionary EaslyController.Focus IFocusIndex IFocusNodeState IFrameIndex EaslyController.Frame.IFrameNodeState IReadOnlyIndex EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% ReadOnlyDictionary EaslyController.Focus IFocusIndex IFocusNodeState IFrameIndex EaslyController.Frame.IFrameNodeState IReadOnlyIndex EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% DictionaryTKey EaslyController.Focus IEqualComparable IFocusInner EaslyController.Frame.IFrameInner EaslyController.ReadOnly.IReadOnlyInner
%GEN% ReadOnlyDictionaryTKey EaslyController.Focus IEqualComparable IFocusInner EaslyController.Frame.IFrameInner EaslyController.ReadOnly.IReadOnlyInner
%GEN% List EaslyController.Focus IFocusNodeState EaslyController.Frame.IFrameNodeState EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% ReadOnlyList EaslyController.Focus IFocusNodeState EaslyController.Frame.IFrameNodeState EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% List EaslyController.Focus IEqualComparable IFocusPlaceholderNodeState EaslyController.Frame.IFramePlaceholderNodeState EaslyController.ReadOnly.IReadOnlyPlaceholderNodeState
%GEN% ReadOnlyList EaslyController.Focus IEqualComparable IFocusPlaceholderNodeState EaslyController.Frame.IFramePlaceholderNodeState EaslyController.ReadOnly.IReadOnlyPlaceholderNodeState
%GEN% Dictionary EaslyController.Focus IEqualComparable IFocusNodeState IFocusNodeStateView IFrameNodeState EaslyController.Frame.IFrameNodeStateView IReadOnlyNodeState EaslyController.ReadOnly.IReadOnlyNodeStateView
%GEN% ReadOnlyDictionary EaslyController.Focus IEqualComparable IFocusNodeState IFocusNodeStateView IFrameNodeState EaslyController.Frame.IFrameNodeStateView IReadOnlyNodeState EaslyController.ReadOnly.IReadOnlyNodeStateView
%GEN% List EaslyController.Focus IFocusOperation EaslyController.Frame.IFrameOperation EaslyController.Writeable.IWriteableOperation
%GEN% ReadOnlyList EaslyController.Focus IFocusOperation EaslyController.Frame.IFrameOperation EaslyController.Writeable.IWriteableOperation
%GEN% List EaslyController.Focus FocusOperationGroup EaslyController.Frame.FrameOperationGroup EaslyController.Writeable.WriteableOperationGroup
%GEN% ReadOnlyList EaslyController.Focus FocusOperationGroup EaslyController.Frame.FrameOperationGroup EaslyController.Writeable.WriteableOperationGroup
%GEN% DictionaryTKey EaslyController.Focus IEqualComparable IFocusAssignableCellView EaslyController.Frame.IFrameAssignableCellView
%GEN% ReadOnlyDictionaryTKey EaslyController.Focus IEqualComparable IFocusAssignableCellView EaslyController.Frame.IFrameAssignableCellView
%GEN% List EaslyController.Focus IEqualComparable IFocusCellView EaslyController.Frame.IFrameCellView
%GEN% ReadOnlyList EaslyController.Focus IEqualComparable IFocusCellView EaslyController.Frame.IFrameCellView
%GEN% List EaslyController.Focus IFocusFrame EaslyController.Frame.IFrameFrame
%GEN% ReadOnlyList EaslyController.Focus IFocusFrame EaslyController.Frame.IFrameFrame
%GEN% List EaslyController.Focus IFocusKeywordFrame EaslyController.Frame.IFrameKeywordFrame
%GEN% ReadOnlyList EaslyController.Focus IFocusKeywordFrame EaslyController.Frame.IFrameKeywordFrame
%GEN% Dictionary EaslyController.Focus System.Type IFocusTemplate System.Type EaslyController.Frame.IFrameTemplate
%GEN% ReadOnlyDictionary EaslyController.Focus System.Type IFocusTemplate System.Type EaslyController.Frame.IFrameTemplate
%GEN% List EaslyController.Focus IFocusTemplate EaslyController.Frame.IFrameTemplate
%GEN% ReadOnlyList EaslyController.Focus IFocusTemplate EaslyController.Frame.IFrameTemplate
%GEN% List EaslyController.Focus IFocusVisibleCellView EaslyController.Frame.IFrameVisibleCellView
%GEN% ReadOnlyList EaslyController.Focus IFocusVisibleCellView EaslyController.Frame.IFrameVisibleCellView
%GEN% List EaslyController.Focus IFocusCycleManager
%GEN% ReadOnlyList EaslyController.Focus IFocusCycleManager
%GEN% List EaslyController.Focus IFocusFocus
%GEN% ReadOnlyList EaslyController.Focus IFocusFocus
%GEN% List EaslyController.Focus IEqualComparable IFocusFrameSelector
%GEN% ReadOnlyList EaslyController.Focus IEqualComparable IFocusFrameSelector
%GEN% List EaslyController.Focus IFocusInsertionChildNodeIndex
%GEN% ReadOnlyList EaslyController.Focus IFocusInsertionChildNodeIndex
%GEN% List EaslyController.Focus IFocusNodeFrameVisibility
%GEN% ReadOnlyList EaslyController.Focus IFocusNodeFrameVisibility
%GEN% List EaslyController.Focus IFocusSelectableFrame
%GEN% ReadOnlyList EaslyController.Focus IFocusSelectableFrame

cd C:\Projects\Easly-Controller\Easly-Controller\05-Layout\Collections

%GEN% List EaslyController.Layout ILayoutBlockState EaslyController.Focus.IFocusBlockState EaslyController.ReadOnly.IReadOnlyBlockState
%GEN% ReadOnlyList EaslyController.Layout ILayoutBlockState EaslyController.Focus.IFocusBlockState EaslyController.ReadOnly.IReadOnlyBlockState
%GEN% Dictionary EaslyController.Layout IEqualComparable ILayoutBlockState LayoutBlockStateView IFocusBlockState EaslyController.Focus.FocusBlockStateView IReadOnlyBlockState EaslyController.ReadOnly.ReadOnlyBlockStateView
%GEN% ReadOnlyDictionary EaslyController.Layout IEqualComparable ILayoutBlockState LayoutBlockStateView IFocusBlockState EaslyController.Focus.FocusBlockStateView IReadOnlyBlockState EaslyController.ReadOnly.ReadOnlyBlockStateView
%GEN% List EaslyController.Layout ILayoutBrowsingBlockNodeIndex EaslyController.Focus.IFocusBrowsingBlockNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingBlockNodeIndex
%GEN% ReadOnlyList EaslyController.Layout ILayoutBrowsingBlockNodeIndex EaslyController.Focus.IFocusBrowsingBlockNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingBlockNodeIndex
%GEN% List EaslyController.Layout ILayoutBrowsingListNodeIndex EaslyController.Focus.IFocusBrowsingListNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingListNodeIndex
%GEN% ReadOnlyList EaslyController.Layout ILayoutBrowsingListNodeIndex EaslyController.Focus.IFocusBrowsingListNodeIndex EaslyController.ReadOnly.IReadOnlyBrowsingListNodeIndex
%GEN% List EaslyController.Layout ILayoutIndexCollection EaslyController.Focus.IFocusIndexCollection EaslyController.ReadOnly.IReadOnlyIndexCollection
%GEN% ReadOnlyList EaslyController.Layout ILayoutIndexCollection EaslyController.Focus.IFocusIndexCollection EaslyController.ReadOnly.IReadOnlyIndexCollection
%GEN% Dictionary EaslyController.Layout ILayoutIndex ILayoutNodeState IFocusIndex EaslyController.Focus.IFocusNodeState IReadOnlyIndex EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% ReadOnlyDictionary EaslyController.Layout ILayoutIndex ILayoutNodeState IFocusIndex EaslyController.Focus.IFocusNodeState IReadOnlyIndex EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% DictionaryTKey EaslyController.Layout IEqualComparable ILayoutInner EaslyController.Focus.IFocusInner EaslyController.ReadOnly.IReadOnlyInner
%GEN% ReadOnlyDictionaryTKey EaslyController.Layout IEqualComparable ILayoutInner EaslyController.Focus.IFocusInner EaslyController.ReadOnly.IReadOnlyInner
%GEN% List EaslyController.Layout ILayoutNodeState EaslyController.Focus.IFocusNodeState EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% ReadOnlyList EaslyController.Layout ILayoutNodeState EaslyController.Focus.IFocusNodeState EaslyController.ReadOnly.IReadOnlyNodeState
%GEN% List EaslyController.Layout IEqualComparable ILayoutPlaceholderNodeState EaslyController.Focus.IFocusPlaceholderNodeState EaslyController.ReadOnly.IReadOnlyPlaceholderNodeState
%GEN% ReadOnlyList EaslyController.Layout IEqualComparable ILayoutPlaceholderNodeState EaslyController.Focus.IFocusPlaceholderNodeState EaslyController.ReadOnly.IReadOnlyPlaceholderNodeState
%GEN% Dictionary EaslyController.Layout IEqualComparable ILayoutNodeState ILayoutNodeStateView IFocusNodeState EaslyController.Focus.IFocusNodeStateView IReadOnlyNodeState EaslyController.ReadOnly.IReadOnlyNodeStateView
%GEN% ReadOnlyDictionary EaslyController.Layout IEqualComparable ILayoutNodeState ILayoutNodeStateView IFocusNodeState EaslyController.Focus.IFocusNodeStateView IReadOnlyNodeState EaslyController.ReadOnly.IReadOnlyNodeStateView
%GEN% List EaslyController.Layout ILayoutOperation EaslyController.Focus.IFocusOperation EaslyController.Writeable.IWriteableOperation
%GEN% ReadOnlyList EaslyController.Layout ILayoutOperation EaslyController.Focus.IFocusOperation EaslyController.Writeable.IWriteableOperation
%GEN% List EaslyController.Layout LayoutOperationGroup EaslyController.Focus.FocusOperationGroup EaslyController.Writeable.WriteableOperationGroup
%GEN% ReadOnlyList EaslyController.Layout LayoutOperationGroup EaslyController.Focus.FocusOperationGroup EaslyController.Writeable.WriteableOperationGroup
%GEN% DictionaryTKey EaslyController.Layout IEqualComparable ILayoutAssignableCellView EaslyController.Focus.IFocusAssignableCellView EaslyController.Frame.IFrameAssignableCellView
%GEN% ReadOnlyDictionaryTKey EaslyController.Layout IEqualComparable ILayoutAssignableCellView EaslyController.Focus.IFocusAssignableCellView EaslyController.Frame.IFrameAssignableCellView
%GEN% List EaslyController.Layout IEqualComparable ILayoutCellView EaslyController.Focus.IFocusCellView EaslyController.Frame.IFrameCellView
%GEN% ReadOnlyList EaslyController.Layout IEqualComparable ILayoutCellView EaslyController.Focus.IFocusCellView EaslyController.Frame.IFrameCellView
%GEN% List EaslyController.Layout ILayoutFrame EaslyController.Focus.IFocusFrame EaslyController.Frame.IFrameFrame
%GEN% ReadOnlyList EaslyController.Layout ILayoutFrame EaslyController.Focus.IFocusFrame EaslyController.Frame.IFrameFrame
%GEN% List EaslyController.Layout ILayoutKeywordFrame EaslyController.Focus.IFocusKeywordFrame EaslyController.Frame.IFrameKeywordFrame
%GEN% ReadOnlyList EaslyController.Layout ILayoutKeywordFrame EaslyController.Focus.IFocusKeywordFrame EaslyController.Frame.IFrameKeywordFrame
%GEN% Dictionary EaslyController.Layout System.Type ILayoutTemplate System.Type EaslyController.Focus.IFocusTemplate System.Type EaslyController.Frame.IFrameTemplate
%GEN% ReadOnlyDictionary EaslyController.Layout System.Type ILayoutTemplate System.Type EaslyController.Focus.IFocusTemplate System.Type EaslyController.Frame.IFrameTemplate
%GEN% List EaslyController.Layout ILayoutTemplate EaslyController.Focus.IFocusTemplate EaslyController.Frame.IFrameTemplate
%GEN% ReadOnlyList EaslyController.Layout ILayoutTemplate EaslyController.Focus.IFocusTemplate EaslyController.Frame.IFrameTemplate
%GEN% List EaslyController.Layout ILayoutVisibleCellView EaslyController.Focus.IFocusVisibleCellView EaslyController.Frame.IFrameVisibleCellView
%GEN% ReadOnlyList EaslyController.Layout ILayoutVisibleCellView EaslyController.Focus.IFocusVisibleCellView EaslyController.Frame.IFrameVisibleCellView
%GEN% List EaslyController.Layout ILayoutCycleManager EaslyController.Focus.IFocusCycleManager
%GEN% ReadOnlyList EaslyController.Layout ILayoutCycleManager EaslyController.Focus.IFocusCycleManager
%GEN% List EaslyController.Layout ILayoutFocus EaslyController.Focus.IFocusFocus
%GEN% ReadOnlyList EaslyController.Layout ILayoutFocus EaslyController.Focus.IFocusFocus
%GEN% List EaslyController.Layout IEqualComparable ILayoutFrameSelector EaslyController.Focus.IFocusFrameSelector
%GEN% ReadOnlyList EaslyController.Layout IEqualComparable ILayoutFrameSelector EaslyController.Focus.IFocusFrameSelector
%GEN% List EaslyController.Layout ILayoutInsertionChildNodeIndex EaslyController.Focus.IFocusInsertionChildNodeIndex
%GEN% ReadOnlyList EaslyController.Layout ILayoutInsertionChildNodeIndex EaslyController.Focus.IFocusInsertionChildNodeIndex
%GEN% List EaslyController.Layout ILayoutNodeFrameVisibility EaslyController.Focus.IFocusNodeFrameVisibility
%GEN% ReadOnlyList EaslyController.Layout ILayoutNodeFrameVisibility EaslyController.Focus.IFocusNodeFrameVisibility
%GEN% List EaslyController.Layout ILayoutSelectableFrame EaslyController.Focus.IFocusSelectableFrame
%GEN% ReadOnlyList EaslyController.Layout ILayoutSelectableFrame EaslyController.Focus.IFocusSelectableFrame

cd C:\Projects\Easly-Controller
