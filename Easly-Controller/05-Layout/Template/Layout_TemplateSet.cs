namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using BaseNodeHelper;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using NotNullReflection;

    /// <summary>
    /// Set of templates used to describe all possible nodes in the tree.
    /// </summary>
    public interface ILayoutTemplateSet : IFocusTemplateSet
    {
        /// <summary>
        /// Templates for nodes by their type.
        /// </summary>
        new LayoutTemplateReadOnlyDictionary NodeTemplateTable { get; }

        /// <summary>
        /// Templates for blocks of nodes.
        /// </summary>
        new LayoutTemplateReadOnlyDictionary BlockTemplateTable { get; }
    }

    /// <summary>
    /// Set of templates used to describe all possible nodes in the tree.
    /// </summary>
    public class LayoutTemplateSet : FocusTemplateSet, ILayoutTemplateSet
    {
        #region Init
        /// <summary>
        /// Returns a default template set.
        /// </summary>
        public static new ILayoutTemplateSet Default
        {
            get
            {
                LayoutTemplateSet Temporary = new LayoutTemplateSet();
                return Temporary.BuildDefault() as ILayoutTemplateSet;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutTemplateSet"/> class.
        /// </summary>
        private protected LayoutTemplateSet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutTemplateSet"/> class.
        /// </summary>
        /// <param name="nodeTemplateTable">Templates for nodes by their type.</param>
        /// <param name="blockTemplateTable">Templates for blocks of nodes.</param>
        public LayoutTemplateSet(LayoutTemplateReadOnlyDictionary nodeTemplateTable, LayoutTemplateReadOnlyDictionary blockTemplateTable)
            : base(nodeTemplateTable, blockTemplateTable)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Templates for nodes by their type.
        /// </summary>
        public new LayoutTemplateReadOnlyDictionary NodeTemplateTable { get { return (LayoutTemplateReadOnlyDictionary)base.NodeTemplateTable; } }

        /// <summary>
        /// Templates for blocks of nodes.
        /// </summary>
        public new LayoutTemplateReadOnlyDictionary BlockTemplateTable { get { return (LayoutTemplateReadOnlyDictionary)base.BlockTemplateTable; } }
        #endregion

        #region Helper
        private protected override IFrameFrame GetRoot()
        {
            return LayoutFrame.LayoutRoot;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxTemplateDictionary object.
        /// </summary>
        private protected override FrameTemplateDictionary CreateEmptyTemplateDictionary()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutTemplateSet>());
            return new LayoutTemplateDictionary();
        }

        /// <summary>
        /// Creates a IxxxTemplateDictionary object.
        /// </summary>
        private protected override FrameTemplateDictionary CreateDefaultTemplateDictionary()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutTemplateSet>());

            IList<Type> NodeKeys = NodeHelper.GetNodeKeys();
            IDictionary<Type, ILayoutTemplate> Dictionary = new Dictionary<Type, ILayoutTemplate>();
            foreach (Type Key in NodeKeys)
                Dictionary.Add(Key, default(ILayoutTemplate));

            return new LayoutTemplateDictionary(Dictionary);
        }

        /// <summary>
        /// Creates a IxxxHorizontalPanelFrame object.
        /// </summary>
        private protected override IFrameHorizontalPanelFrame CreateHorizontalPanelFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutTemplateSet>());
            return new LayoutHorizontalPanelFrame();
        }

        /// <summary>
        /// Creates a IxxxHorizontalCollectionPlaceholderFrame object.
        /// </summary>
        private protected override IFrameHorizontalCollectionPlaceholderFrame CreateHorizontalCollectionPlaceholderFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutTemplateSet>());
            return new LayoutHorizontalCollectionPlaceholderFrame();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderFrame object.
        /// </summary>
        private protected override IFramePlaceholderFrame CreatePlaceholderFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutTemplateSet>());
            return new LayoutPlaceholderFrame();
        }

        /// <summary>
        /// Creates a IxxxOptionalFrame object.
        /// </summary>
        private protected override IFrameOptionalFrame CreateOptionalFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutTemplateSet>());
            return new LayoutOptionalFrame();
        }

        /// <summary>
        /// Creates a IxxxHorizontalListFrame object.
        /// </summary>
        private protected override IFrameHorizontalListFrame CreateHorizontalListFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutTemplateSet>());
            return new LayoutHorizontalListFrame();
        }

        /// <summary>
        /// Creates a IxxxHorizontalBlockListFrame object.
        /// </summary>
        private protected override IFrameHorizontalBlockListFrame CreateHorizontalBlockListFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutTemplateSet>());
            return new LayoutHorizontalBlockListFrame();
        }

        /// <summary>
        /// Creates a IxxxDiscreteFrame object.
        /// </summary>
        private protected override IFrameDiscreteFrame CreateDiscreteFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutTemplateSet>());
            return new LayoutDiscreteFrame();
        }

        /// <summary>
        /// Creates a IxxxKeywordFrame object.
        /// </summary>
        private protected override IFrameKeywordFrame CreateKeywordFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutTemplateSet>());
            return new LayoutKeywordFrame();
        }

        /// <summary>
        /// Creates a IxxxTextValueFrame object.
        /// </summary>
        private protected override IFrameTextValueFrame CreateTextValueFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutTemplateSet>());
            return new LayoutTextValueFrame();
        }

        /// <summary>
        /// Creates a IxxxCommentFrame object.
        /// </summary>
        private protected override IFrameCommentFrame CreateCommentFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutTemplateSet>());
            return new LayoutCommentFrame();
        }

        /// <summary>
        /// Creates a IxxxTemplate object.
        /// </summary>
        private protected override IFrameNodeTemplate CreateNodeTemplate()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutTemplateSet>());
            return new LayoutNodeTemplate();
        }

        /// <summary>
        /// Creates a IxxxTemplate object.
        /// </summary>
        private protected override IFrameBlockTemplate CreateBlockTemplate()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutTemplateSet>());
            return new LayoutBlockTemplate();
        }

        /// <summary>
        /// Creates a IxxxTemplateSet object.
        /// </summary>
        private protected override IFrameTemplateSet CreateDefaultTemplateSet(FrameTemplateReadOnlyDictionary nodeTemplateTable, FrameTemplateReadOnlyDictionary blockTemplateTable)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutTemplateSet>());
            return new LayoutTemplateSet((LayoutTemplateReadOnlyDictionary)nodeTemplateTable, (LayoutTemplateReadOnlyDictionary)blockTemplateTable);
        }
        #endregion
    }
}
