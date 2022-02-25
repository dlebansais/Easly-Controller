namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Contracts;
    using NotNullReflection;

    /// <summary>
    /// Set of templates used to describe all possible nodes in the tree.
    /// </summary>
    public interface IFrameTemplateSet
    {
        /// <summary>
        /// Templates for nodes by their type.
        /// </summary>
        FrameTemplateReadOnlyDictionary NodeTemplateTable { get; }

        /// <summary>
        /// Templates for blocks of nodes.
        /// </summary>
        FrameTemplateReadOnlyDictionary BlockTemplateTable { get; }

        /// <summary>
        /// Checks that templates are valid for nodes.
        /// </summary>
        /// <param name="nodeTemplateTable">Table of templates.</param>
        bool IsValid(FrameTemplateReadOnlyDictionary nodeTemplateTable);

        /// <summary>
        /// Checks that templates are valid for blocks.
        /// </summary>
        /// <param name="blockTemplateTable">Table of templates.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        bool IsBlockValid(FrameTemplateReadOnlyDictionary blockTemplateTable, FrameTemplateReadOnlyDictionary nodeTemplateTable);

        /// <summary>
        /// Template that will be used to describe the given node.
        /// </summary>
        /// <param name="nodeType">Type of the node for which a template is requested.</param>
        IFrameNodeTemplate NodeTypeToTemplate(Type nodeType);

        /// <summary>
        /// Template that will be used to describe the given block.
        /// </summary>
        /// <param name="blockType">Type of the block for which a template is requested.</param>
        IFrameBlockTemplate BlockTypeToTemplate(Type blockType);

        /// <summary>
        /// Gets the frame that creates cells associated to states in the inner.
        /// </summary>
        /// <param name="inner">The inner.</param>
        IFrameFrame InnerToFrame(IFrameInner<IFrameBrowsingChildIndex> inner);

        /// <summary>
        /// Gets the frame that creates cells associated to a property in a state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="propertyName">The property name.</param>
        IFrameFrame PropertyToFrame(IFrameNodeState state, string propertyName);

        /// <summary>
        /// Gets the frame that creates cells associated to a comment in a state.
        /// </summary>
        /// <param name="state">The state.</param>
        IFrameCommentFrame GetCommentFrame(IFrameNodeState state);
    }

    /// <summary>
    /// Set of templates used to describe all possible nodes in the tree.
    /// </summary>
    public class FrameTemplateSet : IFrameTemplateSet
    {
        #region Init
        /// <summary>
        /// Returns a default template set.
        /// </summary>
        public static IFrameTemplateSet Default
        {
            get
            {
                FrameTemplateSet Temporary = new FrameTemplateSet();
                return Temporary.BuildDefault() as IFrameTemplateSet;
            }
        }
        private static IFrameTemplateSet _Default;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameTemplateSet"/> class.
        /// </summary>
        private protected FrameTemplateSet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameTemplateSet"/> class.
        /// </summary>
        /// <param name="nodeTemplateTable">Templates for nodes by their type.</param>
        /// <param name="blockTemplateTable">Templates for blocks of nodes.</param>
        public FrameTemplateSet(FrameTemplateReadOnlyDictionary nodeTemplateTable, FrameTemplateReadOnlyDictionary blockTemplateTable)
        {
            Debug.Assert(IsValid(nodeTemplateTable));
            Debug.Assert(IsBlockValid(blockTemplateTable, nodeTemplateTable));

            NodeTemplateTable = nodeTemplateTable;
            BlockTemplateTable = blockTemplateTable;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Templates for nodes by their type.
        /// </summary>
        public FrameTemplateReadOnlyDictionary NodeTemplateTable { get; }

        /// <summary>
        /// Templates for blocks of nodes.
        /// </summary>
        public FrameTemplateReadOnlyDictionary BlockTemplateTable { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that templates are valid for nodes.
        /// </summary>
        /// <param name="nodeTemplateTable">Table of templates.</param>
        public virtual bool IsValid(FrameTemplateReadOnlyDictionary nodeTemplateTable)
        {
            Contract.RequireNotNull(nodeTemplateTable, out FrameTemplateReadOnlyDictionary NodeTemplateTable);

            FrameTemplateDictionary DefaultDictionary = CreateDefaultTemplateDictionary();

            bool IsValid = true;

            foreach (KeyValuePair<Type, IFrameTemplate> Entry in DefaultDictionary)
                IsValid &= NodeTemplateTable.ContainsKey(Entry.Key);

            foreach (KeyValuePair<Type, IFrameTemplate> Entry in NodeTemplateTable)
            {
                Type NodeType = Entry.Key;
                IFrameTemplate Template = Entry.Value;
                int CommentFrameCount = 0;

                IsValid &= Template.IsValid;
                IsValid &= IsValidNodeType(NodeType, Template.NodeType);
                IsValid &= Template.Root.IsValid(NodeType, NodeTemplateTable, ref CommentFrameCount);
                IsValid &= CommentFrameCount == 1;

                Debug.Assert(IsValid);
            }

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary>
        /// Checks that a node type is a compatible interface to another.
        /// </summary>
        /// <param name="expectedType">The type a template must support.</param>
        /// <param name="providedType">The type a template has declared.</param>
        public virtual bool IsValidNodeType(Type expectedType, Type providedType)
        {
            bool IsValid = true;

            IsValid &= (expectedType == providedType) || expectedType.IsAssignableFrom(providedType);

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary>
        /// Checks that templates are valid for blocks.
        /// </summary>
        /// <param name="blockTemplateTable">Table of templates.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        public virtual bool IsBlockValid(FrameTemplateReadOnlyDictionary blockTemplateTable, FrameTemplateReadOnlyDictionary nodeTemplateTable)
        {
            Contract.RequireNotNull(blockTemplateTable, out FrameTemplateReadOnlyDictionary BlockTemplateTable);
            Contract.RequireNotNull(nodeTemplateTable, out FrameTemplateReadOnlyDictionary NodeTemplateTable);

            IList<Type> BlockKeys = NodeHelper.GetNodeKeys();

            FrameTemplateDictionary DefaultDictionary = CreateEmptyTemplateDictionary();
            foreach (Type Key in BlockKeys)
                AddBlockNodeTypes(DefaultDictionary, Key);

            bool IsValid = true;

            foreach (KeyValuePair<Type, IFrameTemplate> Entry in DefaultDictionary)
                IsValid &= BlockTemplateTable.ContainsKey(Entry.Key);

            foreach (KeyValuePair<Type, IFrameTemplate> Entry in BlockTemplateTable)
            {
                Type NodeType = Entry.Key;
                IFrameTemplate Template = Entry.Value;
                int CommentFrameCount = 0;

                IsValid &= NodeTreeHelper.IsBlockInterfaceType(NodeType);
                IsValid &= Template.IsValid;
                IsValid &= Template.Root.IsValid(NodeType, NodeTemplateTable, ref CommentFrameCount);
            }

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary>
        /// Template that will be used to describe the given node.
        /// </summary>
        /// <param name="nodeType">Type of the node for which a template is requested.</param>
        public virtual IFrameNodeTemplate NodeTypeToTemplate(Type nodeType)
        {
            Contract.RequireNotNull(nodeType, out Type NodeType);
            Debug.Assert(NodeTemplateTable.ContainsKey(NodeType));

            return NodeTemplateTable[NodeType] as IFrameNodeTemplate;
        }

        /// <summary>
        /// Template that will be used to describe the given block.
        /// </summary>
        /// <param name="blockType">Type of the block for which a template is requested.</param>
        public virtual IFrameBlockTemplate BlockTypeToTemplate(Type blockType)
        {
            Contract.RequireNotNull(blockType, out Type BlockType);
            Debug.Assert(BlockTemplateTable.ContainsKey(BlockType));

            return BlockTemplateTable[BlockType] as IFrameBlockTemplate;
        }

        /// <summary>
        /// Gets the frame that creates cells associated to states in the inner.
        /// </summary>
        /// <param name="inner">The inner.</param>
        public virtual IFrameFrame InnerToFrame(IFrameInner<IFrameBrowsingChildIndex> inner)
        {
            // Call overloads of this method if they exist.
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());

            IFrameNodeState Owner = inner.Owner;
            Type OwnerType = Type.FromGetType(Owner.Node);

            //Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(OwnerType);
            //IFrameNodeTemplate Template = NodeTypeToTemplate(InterfaceType);
            IFrameNodeTemplate Template = NodeTypeToTemplate(OwnerType);

            IFrameFrame Frame = Template.PropertyToFrame(inner.PropertyName);

            if (Frame is IFrameBlockListFrame AsBlockListFrame)
            {
                IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> BlockListInner = inner as IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>;
                Debug.Assert(BlockListInner != null);

                Type BlockType = NodeTreeHelperBlockList.BlockListBlockType(Owner.Node, BlockListInner.PropertyName);
                IFrameBlockTemplate BlockTemplate = BlockTypeToTemplate(BlockType);

                Frame = BlockTemplate.GetPlaceholderFrame();
            }

            return Frame;
        }

        /// <summary>
        /// Gets the frame that creates cells associated to a property in a state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="propertyName">The property name.</param>
        public virtual IFrameFrame PropertyToFrame(IFrameNodeState state, string propertyName)
        {
            // Call overloads of this method if they exist.
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());

            Type OwnerType = Type.FromGetType(state.Node);

            //Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(OwnerType);
            //IFrameNodeTemplate Template = NodeTypeToTemplate(InterfaceType);
            IFrameNodeTemplate Template = NodeTypeToTemplate(OwnerType);

            IFrameFrame Frame = Template.PropertyToFrame(propertyName);

            return Frame;
        }

        /// <summary>
        /// Gets the frame that creates cells associated to a comment in a state.
        /// </summary>
        /// <param name="state">The state.</param>
        public virtual IFrameCommentFrame GetCommentFrame(IFrameNodeState state)
        {
            // Call overloads of this method if they exist.
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());

            Type OwnerType = Type.FromGetType(state.Node);

            //Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(OwnerType);
            //IFrameNodeTemplate Template = NodeTypeToTemplate(InterfaceType);
            IFrameNodeTemplate Template = NodeTypeToTemplate(OwnerType);

            IFrameCommentFrame Frame = Template.GetCommentFrame();

            return Frame;
        }
        #endregion

        #region Helper
        private protected virtual IFrameTemplateSet BuildDefault()
        {
            if (_Default != null && _Default.GetType() == GetType()) // Recreate the default if the layer has changed.
                return _Default;

            FrameTemplateReadOnlyDictionary DefaultNodeTemplateTable = BuildDefaultNodeTemplateTable();
            FrameTemplateReadOnlyDictionary DefaultBlockTemplateTable = BuildDefaultBlockListTemplate();

            Debug.Assert(IsValid(DefaultNodeTemplateTable));
            Debug.Assert(IsBlockValid(DefaultBlockTemplateTable, DefaultNodeTemplateTable));

            _Default = CreateDefaultTemplateSet(DefaultNodeTemplateTable, DefaultBlockTemplateTable);

            return _Default;
        }

        private protected virtual FrameTemplateReadOnlyDictionary BuildDefaultNodeTemplateTable()
        {
            FrameTemplateDictionary DefaultDictionary = CreateDefaultTemplateDictionary();

            List<Type> Keys = new List<Type>(DefaultDictionary.Keys);
            foreach (Type Key in Keys)
                SetNodeTypeToDefault(DefaultDictionary, Key);

            return DefaultDictionary.ToReadOnly();
        }

        private protected virtual void SetNodeTypeToDefault(FrameTemplateDictionary dictionary, Type nodeType)
        {
            Debug.Assert(dictionary.ContainsKey(nodeType));
            Debug.Assert(dictionary[nodeType] == null);

            FrameHorizontalPanelFrame RootFrame = (FrameHorizontalPanelFrame)CreateHorizontalPanelFrame();
            FrameNodeTemplate RootTemplate = (FrameNodeTemplate)CreateNodeTemplate();
            RootTemplate.NodeType = nodeType;
            RootTemplate.Root = RootFrame;

            // Set the template, even if empty, in case the node recursively refers to itself (ex: expressions).
            dictionary[nodeType] = RootTemplate;

            RootFrame.Items.Add(CreateCommentFrame());

            Type ChildNodeType;
            IList<string> Properties = NodeTreeHelper.EnumChildNodeProperties(nodeType);
            foreach (string PropertyName in Properties)
            {
                bool IsHandled = false;

                if (NodeTreeHelperChild.IsChildNodeProperty(nodeType, PropertyName, out ChildNodeType))
                {
                    FramePlaceholderFrame NewFrame = (FramePlaceholderFrame)CreatePlaceholderFrame();
                    NewFrame.PropertyName = PropertyName;
                    RootFrame.Items.Add(NewFrame);
                    IsHandled = true;
                }
                else if (NodeTreeHelperOptional.IsOptionalChildNodeProperty(nodeType, PropertyName, out ChildNodeType))
                {
                    FrameOptionalFrame NewFrame = (FrameOptionalFrame)CreateOptionalFrame();
                    NewFrame.PropertyName = PropertyName;
                    RootFrame.Items.Add(NewFrame);
                    IsHandled = true;
                }
                else if (NodeTreeHelperList.IsNodeListProperty(nodeType, PropertyName, out Type ListNodeType))
                {
                    FrameHorizontalListFrame NewFrame = (FrameHorizontalListFrame)CreateHorizontalListFrame();
                    NewFrame.PropertyName = PropertyName;
                    RootFrame.Items.Add(NewFrame);
                    IsHandled = true;
                }
                else if (NodeTreeHelperBlockList.IsBlockListProperty(nodeType, PropertyName, /*out Type ChildInterfaceType,*/ out Type ChildItemType))
                {
                    FrameHorizontalBlockListFrame NewFrame = (FrameHorizontalBlockListFrame)CreateHorizontalBlockListFrame();
                    NewFrame.PropertyName = PropertyName;
                    RootFrame.Items.Add(NewFrame);
                    IsHandled = true;
                }
                else if (NodeTreeHelper.IsBooleanProperty(nodeType, PropertyName))
                {
                    FrameDiscreteFrame NewDiscreteFrame = (FrameDiscreteFrame)CreateDiscreteFrame();
                    NewDiscreteFrame.PropertyName = PropertyName;

                    FrameKeywordFrame KeywordFalseFrame = (FrameKeywordFrame)CreateKeywordFrame();
                    KeywordFalseFrame.Text = $"{PropertyName}=False";
                    NewDiscreteFrame.Items.Add(KeywordFalseFrame);

                    FrameKeywordFrame KeywordTrueFrame = (FrameKeywordFrame)CreateKeywordFrame();
                    KeywordTrueFrame.Text = $"{PropertyName}=True";
                    NewDiscreteFrame.Items.Add(KeywordTrueFrame);

                    RootFrame.Items.Add(NewDiscreteFrame);
                    IsHandled = true;
                }
                else if (NodeTreeHelper.IsEnumProperty(nodeType, PropertyName))
                {
                    NodeTreeHelper.GetEnumRange(nodeType, PropertyName, out int Min, out int Max);

                    FrameDiscreteFrame NewDiscreteFrame = (FrameDiscreteFrame)CreateDiscreteFrame();
                    NewDiscreteFrame.PropertyName = PropertyName;

                    for (int i = Min; i <= Max; i++)
                    {
                        FrameKeywordFrame KeywordFrame = (FrameKeywordFrame)CreateKeywordFrame();
                        KeywordFrame.Text = $"{PropertyName}={i}";
                        NewDiscreteFrame.Items.Add(KeywordFrame);
                    }

                    RootFrame.Items.Add(NewDiscreteFrame);
                    IsHandled = true;
                }
                else if (NodeTreeHelper.IsStringProperty(nodeType, PropertyName))
                {
                    FrameTextValueFrame NewDiscreteFrame = (FrameTextValueFrame)CreateTextValueFrame();
                    NewDiscreteFrame.PropertyName = PropertyName;
                    RootFrame.Items.Add(NewDiscreteFrame);
                    IsHandled = true;
                }
                else if (NodeTreeHelper.IsGuidProperty(nodeType, PropertyName))
                {
                    IsHandled = true;
                }
                else if (NodeTreeHelper.IsDocumentProperty(nodeType, PropertyName))
                {
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }

            RootFrame.UpdateParent(RootTemplate, GetRoot());
        }

        private protected virtual IFrameFrame GetRoot()
        {
            return FrameFrame.FrameRoot;
        }

        private protected virtual FrameTemplateReadOnlyDictionary BuildDefaultBlockListTemplate()
        {
            IList<Type> NodeKeys = NodeHelper.GetNodeKeys();

            FrameTemplateDictionary DefaultDictionary = CreateEmptyTemplateDictionary();
            foreach (Type Key in NodeKeys)
                AddBlockNodeTypes(DefaultDictionary, Key);

            FramePlaceholderFrame PatternFrame = (FramePlaceholderFrame)CreatePlaceholderFrame();
            PatternFrame.PropertyName = nameof(IBlock.ReplicationPattern);

            FramePlaceholderFrame SourceFrame = (FramePlaceholderFrame)CreatePlaceholderFrame();
            SourceFrame.PropertyName = nameof(IBlock.SourceIdentifier);

            IFrameHorizontalCollectionPlaceholderFrame CollectionPlaceholderFrame = CreateHorizontalCollectionPlaceholderFrame();

            IFrameHorizontalPanelFrame RootFrame = CreateHorizontalPanelFrame();
            RootFrame.Items.Add(PatternFrame);
            RootFrame.Items.Add(SourceFrame);
            RootFrame.Items.Add(CollectionPlaceholderFrame);

            FrameBlockTemplate RootTemplate = (FrameBlockTemplate)CreateBlockTemplate();
            RootTemplate.NodeType = Type.FromTypeof<IBlock>();
            RootTemplate.Root = RootFrame;

            RootFrame.UpdateParent(RootTemplate, GetRoot());

            List<Type> BlockKeys = new List<Type>(DefaultDictionary.Keys);
            foreach (Type Key in BlockKeys)
                DefaultDictionary[Key] = RootTemplate;

            return DefaultDictionary.ToReadOnly();
        }

        private protected virtual void AddBlockNodeTypes(FrameTemplateDictionary dictionary, Type nodeType)
        {
            IList<string> Properties = NodeTreeHelper.EnumChildNodeProperties(nodeType);
            foreach (string PropertyName in Properties)
                if (NodeTreeHelperBlockList.IsBlockListProperty(nodeType, PropertyName, /*out Type ChildInterfaceType,*/ out Type ChildItemType))
                {
                    Type BlockListType = NodeTreeHelperBlockList.BlockListBlockType(nodeType, PropertyName);

                    if (!dictionary.ContainsKey(BlockListType))
                        dictionary.Add(BlockListType, null);
                }
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxTemplateDictionary object.
        /// </summary>
        private protected virtual FrameTemplateDictionary CreateEmptyTemplateDictionary()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());
            return new FrameTemplateDictionary();
        }

        /// <summary>
        /// Creates a IxxxTemplateDictionary object.
        /// </summary>
        private protected virtual FrameTemplateDictionary CreateDefaultTemplateDictionary()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());

            IList<Type> NodeKeys = NodeHelper.GetNodeKeys();
            IDictionary<Type, IFrameTemplate> Dictionary = new Dictionary<Type, IFrameTemplate>();
            foreach (Type Key in NodeKeys)
                Dictionary.Add(Key, default(IFrameTemplate));

            return new FrameTemplateDictionary(Dictionary);
        }

        /// <summary>
        /// Creates a IxxxHorizontalPanelFrame object.
        /// </summary>
        private protected virtual IFrameHorizontalPanelFrame CreateHorizontalPanelFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());
            return new FrameHorizontalPanelFrame();
        }

        /// <summary>
        /// Creates a IxxxHorizontalCollectionPlaceholderFrame object.
        /// </summary>
        private protected virtual IFrameHorizontalCollectionPlaceholderFrame CreateHorizontalCollectionPlaceholderFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());
            return new FrameHorizontalCollectionPlaceholderFrame();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderFrame object.
        /// </summary>
        private protected virtual IFramePlaceholderFrame CreatePlaceholderFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());
            return new FramePlaceholderFrame();
        }

        /// <summary>
        /// Creates a IxxxOptionalFrame object.
        /// </summary>
        private protected virtual IFrameOptionalFrame CreateOptionalFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());
            return new FrameOptionalFrame();
        }

        /// <summary>
        /// Creates a IxxxHorizontalListFrame object.
        /// </summary>
        private protected virtual IFrameHorizontalListFrame CreateHorizontalListFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());
            return new FrameHorizontalListFrame();
        }

        /// <summary>
        /// Creates a IxxxHorizontalBlockListFrame object.
        /// </summary>
        private protected virtual IFrameHorizontalBlockListFrame CreateHorizontalBlockListFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());
            return new FrameHorizontalBlockListFrame();
        }

        /// <summary>
        /// Creates a IxxxDiscreteFrame object.
        /// </summary>
        private protected virtual IFrameDiscreteFrame CreateDiscreteFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());
            return new FrameDiscreteFrame();
        }

        /// <summary>
        /// Creates a IxxxKeywordFrame object.
        /// </summary>
        private protected virtual IFrameKeywordFrame CreateKeywordFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());
            return new FrameKeywordFrame();
        }

        /// <summary>
        /// Creates a IxxxTextValueFrame object.
        /// </summary>
        private protected virtual IFrameTextValueFrame CreateTextValueFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());
            return new FrameTextValueFrame();
        }

        /// <summary>
        /// Creates a IxxxCommentFrame object.
        /// </summary>
        private protected virtual IFrameCommentFrame CreateCommentFrame()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());
            return new FrameCommentFrame();
        }

        /// <summary>
        /// Creates a IxxxTemplate object.
        /// </summary>
        private protected virtual IFrameNodeTemplate CreateNodeTemplate()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());
            return new FrameNodeTemplate();
        }

        /// <summary>
        /// Creates a IxxxTemplate object.
        /// </summary>
        private protected virtual IFrameBlockTemplate CreateBlockTemplate()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());
            return new FrameBlockTemplate();
        }

        /// <summary>
        /// Creates a IxxxTemplateSet object.
        /// </summary>
        private protected virtual IFrameTemplateSet CreateDefaultTemplateSet(FrameTemplateReadOnlyDictionary nodeTemplateTable, FrameTemplateReadOnlyDictionary blockTemplateTable)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameTemplateSet>());
            return new FrameTemplateSet(nodeTemplateTable, blockTemplateTable);
        }
        #endregion
    }
}
