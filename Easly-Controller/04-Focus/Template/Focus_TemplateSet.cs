namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNodeHelper;
    using EaslyController.Frame;

    /// <summary>
    /// Set of templates used to describe all possible nodes in the tree.
    /// </summary>
    public interface IFocusTemplateSet : IFrameTemplateSet
    {
        /// <summary>
        /// Templates for nodes by their type.
        /// </summary>
        new IFocusTemplateReadOnlyDictionary NodeTemplateTable { get; }

        /// <summary>
        /// Templates for blocks of nodes.
        /// </summary>
        new IFocusTemplateReadOnlyDictionary BlockTemplateTable { get; }

        /// <summary>
        /// Gets the frame that creates cells associated to states in the inner.
        /// This overload uses selectors to choose the correct frame.
        /// </summary>
        /// <param name="inner">The inner.</param>
        /// <param name="selectorStack">A list of selectors to choose the correct frame.</param>
        IFocusFrame InnerToFrame(IFocusInner<IFocusBrowsingChildIndex> inner, IList<IFocusFrameSelectorList> selectorStack);

        /// <summary>
        /// Gets the frame that creates cells associated to a property in a state.
        /// This overload uses selectors to choose the correct frame.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="selectorStack">A list of selectors to choose the correct frame.</param>
        IFocusFrame PropertyToFrame(IFocusNodeState state, string propertyName, IList<IFocusFrameSelectorList> selectorStack);

        /// <summary>
        /// Gets the frame that creates cells associated to a comment in a state.
        /// This overload uses selectors to choose the correct frame.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="selectorStack">A list of selectors to choose the correct frame.</param>
        IFocusCommentFrame GetCommentFrame(IFocusNodeState state, IList<IFocusFrameSelectorList> selectorStack);
    }

    /// <summary>
    /// Set of templates used to describe all possible nodes in the tree.
    /// </summary>
    public class FocusTemplateSet : FrameTemplateSet, IFocusTemplateSet
    {
        #region Init
        /// <summary>
        /// Returns a default template set.
        /// </summary>
        public static new IFocusTemplateSet Default
        {
            get
            {
                FocusTemplateSet Temporary = new FocusTemplateSet();
                return Temporary.BuildDefault() as IFocusTemplateSet;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusTemplateSet"/> class.
        /// </summary>
        private protected FocusTemplateSet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusTemplateSet"/> class.
        /// </summary>
        /// <param name="nodeTemplateTable">Templates for nodes by their type.</param>
        /// <param name="blockTemplateTable">Templates for blocks of nodes.</param>
        public FocusTemplateSet(IFocusTemplateReadOnlyDictionary nodeTemplateTable, IFocusTemplateReadOnlyDictionary blockTemplateTable)
            : base(nodeTemplateTable, blockTemplateTable)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Templates for nodes by their type.
        /// </summary>
        public new IFocusTemplateReadOnlyDictionary NodeTemplateTable { get { return (IFocusTemplateReadOnlyDictionary)base.NodeTemplateTable; } }

        /// <summary>
        /// Templates for blocks of nodes.
        /// </summary>
        public new IFocusTemplateReadOnlyDictionary BlockTemplateTable { get { return (IFocusTemplateReadOnlyDictionary)base.BlockTemplateTable; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Gets the frame that creates cells associated to states in the inner.
        /// This overload uses selectors to choose the correct frame.
        /// </summary>
        /// <param name="inner">The inner.</param>
        /// <param name="selectorStack">A list of selectors to choose the correct frame.</param>
        public virtual IFocusFrame InnerToFrame(IFocusInner<IFocusBrowsingChildIndex> inner, IList<IFocusFrameSelectorList> selectorStack)
        {
            IFocusNodeState Owner = inner.Owner;
            Type OwnerType = Owner.Node.GetType();
            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(OwnerType);
            IFocusNodeTemplate Template = (IFocusNodeTemplate)NodeTypeToTemplate(InterfaceType);
            IFocusFrame Frame = Template.PropertyToFrame(inner.PropertyName, selectorStack);

            if (Frame is IFocusBlockListFrame AsBlockListFrame)
            {
                IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> BlockListInner = inner as IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>;
                Debug.Assert(BlockListInner != null);

                Type BlockType = NodeTreeHelperBlockList.BlockListBlockType(Owner.Node, BlockListInner.PropertyName);
                IFocusBlockTemplate BlockTemplate = (IFocusBlockTemplate)BlockTypeToTemplate(BlockType);

                Frame = (IFocusFrame)BlockTemplate.GetPlaceholderFrame();
            }

            return Frame;
        }

        /// <summary>
        /// Gets the frame that creates cells associated to a property in a state.
        /// This overload uses selectors to choose the correct frame.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="selectorStack">A list of selectors to choose the correct frame.</param>
        public virtual IFocusFrame PropertyToFrame(IFocusNodeState state, string propertyName, IList<IFocusFrameSelectorList> selectorStack)
        {
            Type OwnerType = state.Node.GetType();
            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(OwnerType);
            IFocusNodeTemplate Template = (IFocusNodeTemplate)NodeTypeToTemplate(InterfaceType);
            IFocusFrame Frame = Template.PropertyToFrame(propertyName, selectorStack);

            return Frame;
        }

        /// <summary>
        /// Gets the frame that creates cells associated to a comment in a state.
        /// This overload uses selectors to choose the correct frame.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="selectorStack">A list of selectors to choose the correct frame.</param>
        public virtual IFocusCommentFrame GetCommentFrame(IFocusNodeState state, IList<IFocusFrameSelectorList> selectorStack)
        {
            Type OwnerType = state.Node.GetType();
            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(OwnerType);
            IFocusNodeTemplate Template = (IFocusNodeTemplate)NodeTypeToTemplate(InterfaceType);
            IFocusCommentFrame Frame = Template.GetCommentFrame(selectorStack);

            return Frame;
        }
        #endregion

        #region Helper
        /// <summary></summary>
        private protected override IFrameFrame GetRoot()
        {
            return FocusFrame.FocusRoot;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxTemplateDictionary object.
        /// </summary>
        private protected override IFrameTemplateDictionary CreateEmptyTemplateDictionary()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusTemplateDictionary();
        }

        /// <summary>
        /// Creates a IxxxTemplateDictionary object.
        /// </summary>
        private protected override IFrameTemplateDictionary CreateDefaultTemplateDictionary()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusTemplateDictionary(NodeHelper.CreateNodeDictionary<IFocusTemplate>());
        }

        /// <summary>
        /// Creates a IxxxHorizontalPanelFrame object.
        /// </summary>
        private protected override IFrameHorizontalPanelFrame CreateHorizontalPanelFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusHorizontalPanelFrame();
        }

        /// <summary>
        /// Creates a IxxxHorizontalCollectionPlaceholderFrame object.
        /// </summary>
        private protected override IFrameHorizontalCollectionPlaceholderFrame CreateHorizontalCollectionPlaceholderFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusHorizontalCollectionPlaceholderFrame();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderFrame object.
        /// </summary>
        private protected override IFramePlaceholderFrame CreatePlaceholderFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusPlaceholderFrame();
        }

        /// <summary>
        /// Creates a IxxxOptionalFrame object.
        /// </summary>
        private protected override IFrameOptionalFrame CreateOptionalFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusOptionalFrame();
        }

        /// <summary>
        /// Creates a IxxxHorizontalListFrame object.
        /// </summary>
        private protected override IFrameHorizontalListFrame CreateHorizontalListFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusHorizontalListFrame();
        }

        /// <summary>
        /// Creates a IxxxHorizontalBlockListFrame object.
        /// </summary>
        private protected override IFrameHorizontalBlockListFrame CreateHorizontalBlockListFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusHorizontalBlockListFrame();
        }

        /// <summary>
        /// Creates a IxxxDiscreteFrame object.
        /// </summary>
        private protected override IFrameDiscreteFrame CreateDiscreteFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusDiscreteFrame();
        }

        /// <summary>
        /// Creates a IxxxKeywordFrame object.
        /// </summary>
        private protected override IFrameKeywordFrame CreateKeywordFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusKeywordFrame();
        }

        /// <summary>
        /// Creates a IxxxTextValueFrame object.
        /// </summary>
        private protected override IFrameTextValueFrame CreateTextValueFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusTextValueFrame();
        }

        /// <summary>
        /// Creates a IxxxCommentFrame object.
        /// </summary>
        private protected override IFrameCommentFrame CreateCommentFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusCommentFrame();
        }

        /// <summary>
        /// Creates a IxxxTemplate object.
        /// </summary>
        private protected override IFrameNodeTemplate CreateNodeTemplate()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusNodeTemplate();
        }

        /// <summary>
        /// Creates a IxxxTemplate object.
        /// </summary>
        private protected override IFrameBlockTemplate CreateBlockTemplate()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusBlockTemplate();
        }

        /// <summary>
        /// Creates a IxxxTemplateSet object.
        /// </summary>
        private protected override IFrameTemplateSet CreateDefaultTemplateSet(IFrameTemplateReadOnlyDictionary nodeTemplateTable, IFrameTemplateReadOnlyDictionary blockTemplateTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusTemplateSet((IFocusTemplateReadOnlyDictionary)nodeTemplateTable, (IFocusTemplateReadOnlyDictionary)blockTemplateTable);
        }
        #endregion
    }
}
