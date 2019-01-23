using BaseNode;
using BaseNodeHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Set of templates used to describe all possible nodes in the tree.
    /// </summary>
    public interface IFrameTemplateSet
    {
        /// <summary>
        /// Templates for nodes by their type.
        /// </summary>
        IFrameTemplateReadOnlyDictionary NodeTemplateTable { get; }

        /// <summary>
        /// Templates for blocks of nodes.
        /// </summary>
        IFrameTemplateReadOnlyDictionary BlockTemplateTable { get; }

        /// <summary>
        /// Checks that templates are valid for nodes.
        /// </summary>
        /// <param name="nodeTemplateTable">Table of templates.</param>
        bool IsValid(IFrameTemplateReadOnlyDictionary nodeTemplateTable);

        /// <summary>
        /// Checks that templates are valid for blocks.
        /// </summary>
        /// <param name="blockTemplateTable">Table of templates.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        bool IsBlockValid(IFrameTemplateReadOnlyDictionary blockTemplateTable, IFrameTemplateReadOnlyDictionary nodeTemplateTable);

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
        public static IFrameTemplateSet Default { get { return (new FrameTemplateSet()).BuildDefault() as IFrameTemplateSet; } }
        private static IFrameTemplateSet _Default;

        /// <summary></summary>
        protected FrameTemplateSet()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FrameTemplateSet"/>.
        /// </summary>
        /// <param name="nodeTemplateTable">Templates for nodes by their type.</param>
        /// <param name="blockTemplateTable">Templates for blocks of nodes.</param>
        public FrameTemplateSet(IFrameTemplateReadOnlyDictionary nodeTemplateTable, IFrameTemplateReadOnlyDictionary blockTemplateTable)
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
        public IFrameTemplateReadOnlyDictionary NodeTemplateTable { get; }

        /// <summary>
        /// Templates for blocks of nodes.
        /// </summary>
        public IFrameTemplateReadOnlyDictionary BlockTemplateTable { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that templates are valid for nodes.
        /// </summary>
        /// <param name="nodeTemplateTable">Table of templates.</param>
        public virtual bool IsValid(IFrameTemplateReadOnlyDictionary nodeTemplateTable)
        {
            Debug.Assert(nodeTemplateTable != null);

            IFrameTemplateDictionary DefaultDictionary = CreateDefaultTemplateDictionary();

            foreach (KeyValuePair<Type, IFrameTemplate> Entry in DefaultDictionary)
                if (!nodeTemplateTable.ContainsKey(Entry.Key))
                    return false;

            if (nodeTemplateTable.Count != DefaultDictionary.Count)
                return false;

            foreach (KeyValuePair<Type, IFrameTemplate> Entry in nodeTemplateTable)
            {
                Type NodeType = Entry.Key;
                IFrameTemplate Template = Entry.Value;

                if (!Template.IsValid)
                    return false;

                if (!IsValidNodeType(NodeType, Template.NodeType))
                    return false;

                if (!Template.Root.IsValid(NodeType, nodeTemplateTable))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks that a node type is a compatible interface to another.
        /// </summary>
        /// <param name="expectedType">The type a template must support.</param>
        /// <param name="providedType">The type a template has declared.</param>
        public virtual bool IsValidNodeType(Type expectedType, Type providedType)
        {
            if (expectedType == providedType)
                return true;

            if (expectedType.IsAssignableFrom(providedType))
                return true;

            if (expectedType.IsGenericType)
            {
                Type GenericTypeDefinition = expectedType.GetGenericTypeDefinition();
                string GenericTypeDefinitionName = GenericTypeDefinition.Name;
                int GenericCharIndex = GenericTypeDefinitionName.IndexOf("`");
                if (GenericCharIndex > 0)
                    GenericTypeDefinitionName = GenericTypeDefinitionName.Substring(0, GenericCharIndex);

                if (GenericTypeDefinitionName == providedType.Name)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks that templates are valid for blocks.
        /// </summary>
        /// <param name="blockTemplateTable">Table of templates.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        public virtual bool IsBlockValid(IFrameTemplateReadOnlyDictionary blockTemplateTable, IFrameTemplateReadOnlyDictionary nodeTemplateTable)
        {
            Debug.Assert(blockTemplateTable != null);

            List<Type> BlockKeys = new List<Type>(NodeHelper.CreateNodeDictionary<object>().Keys);

            IFrameTemplateDictionary DefaultDictionary = CreateEmptyTemplateDictionary();
            foreach (Type Key in BlockKeys)
                AddBlockNodeTypes(DefaultDictionary, Key);

            foreach (KeyValuePair<Type, IFrameTemplate> Entry in DefaultDictionary)
                if (!blockTemplateTable.ContainsKey(Entry.Key))
                    return false;

            if (blockTemplateTable.Count != DefaultDictionary.Count)
                return false;

            foreach (KeyValuePair<Type, IFrameTemplate> Entry in blockTemplateTable)
            {
                Type NodeType = Entry.Key;
                IFrameTemplate Template = Entry.Value;

                if (!NodeTreeHelper.IsBlockType(NodeType))
                    return false;

                if (!Template.IsValid)
                    return false;

                if (!Template.Root.IsValid(NodeType, nodeTemplateTable))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Template that will be used to describe the given node.
        /// </summary>
        /// <param name="nodeType">Type of the node for which a template is requested.</param>
        public virtual IFrameNodeTemplate NodeTypeToTemplate(Type nodeType)
        {
            Debug.Assert(nodeType != null);
            Debug.Assert(NodeTemplateTable.ContainsKey(nodeType));

            return NodeTemplateTable[nodeType] as IFrameNodeTemplate;
        }

        /// <summary>
        /// Template that will be used to describe the given block.
        /// </summary>
        /// <param name="blockType">Type of the block for which a template is requested.</param>
        public virtual IFrameBlockTemplate BlockTypeToTemplate(Type blockType)
        {
            Debug.Assert(blockType != null);
            Debug.Assert(BlockTemplateTable.ContainsKey(blockType));

            return BlockTemplateTable[blockType] as IFrameBlockTemplate;
        }
        #endregion

        #region Helper
        /// <summary></summary>
        protected virtual IFrameTemplateSet BuildDefault()
        {
            if (_Default != null && _Default.GetType() == GetType()) // Recreate the default if the layer has changed.
                return _Default;

            IFrameTemplateReadOnlyDictionary DefaultNodeTemplateTable = BuildDefaultNodeTemplateTable();
            IFrameTemplateReadOnlyDictionary DefaultBlockTemplateTable = BuildDefaultBlockListTemplate();

            Debug.Assert(IsValid(DefaultNodeTemplateTable));
            Debug.Assert(IsBlockValid(DefaultBlockTemplateTable, DefaultNodeTemplateTable));

            _Default = CreateDefaultTemplateSet(DefaultNodeTemplateTable, DefaultBlockTemplateTable);

            return _Default;
        }

        /// <summary></summary>
        protected virtual IFrameTemplateReadOnlyDictionary BuildDefaultNodeTemplateTable()
        {
            IFrameTemplateDictionary DefaultDictionary = CreateDefaultTemplateDictionary();

            List<Type> Keys = new List<Type>(DefaultDictionary.Keys);
            foreach (Type Key in Keys)
                SetNodeTypeToDefault(DefaultDictionary, Key);

            return CreateTemplateReadOnlyDictionary(DefaultDictionary);
        }

        /// <summary></summary>
        protected virtual void SetNodeTypeToDefault(IFrameTemplateDictionary dictionary, Type nodeType)
        {
            Debug.Assert(dictionary.ContainsKey(nodeType));

            if (dictionary[nodeType] != null)
                return;

            IFrameHorizontalPanelFrame RootFrame = CreateHorizontalPanelFrame();
            IFrameNodeTemplate RootTemplate = CreateNodeTemplate();
            RootTemplate.NodeType = nodeType;
            RootTemplate.Root = RootFrame;

            // Set the template, even if empty, in case the node recursively refers to itself (ex: expressions).
            dictionary[nodeType] = RootTemplate;

            Type ChildNodeType;
            IList<string> Properties = NodeTreeHelper.EnumChildNodeProperties(nodeType);
            foreach (string PropertyName in Properties)
                if (NodeTreeHelperChild.IsChildNodeProperty(nodeType, PropertyName, out ChildNodeType))
                {
                    IFramePlaceholderFrame NewFrame = CreatePlaceholderFrame();
                    NewFrame.PropertyName = PropertyName;
                    RootFrame.Items.Add(NewFrame);
                }
                else if (NodeTreeHelperOptional.IsOptionalChildNodeProperty(nodeType, PropertyName, out ChildNodeType))
                {
                    IFrameOptionalFrame NewFrame = CreateOptionalFrame();
                    NewFrame.PropertyName = PropertyName;
                    RootFrame.Items.Add(NewFrame);
                }
                else if (NodeTreeHelperList.IsNodeListProperty(nodeType, PropertyName, out Type ListNodeType))
                {
                    IFrameHorizontalListFrame NewFrame = CreateHorizontalListFrame();
                    NewFrame.PropertyName = PropertyName;
                    RootFrame.Items.Add(NewFrame);
                }
                else if (NodeTreeHelperBlockList.IsBlockListProperty(nodeType, PropertyName, out Type ChildInterfaceType, out Type ChildItemType))
                {
                    IFrameHorizontalBlockListFrame NewFrame = CreateHorizontalBlockListFrame();
                    NewFrame.PropertyName = PropertyName;
                    RootFrame.Items.Add(NewFrame);
                }
                else if (NodeTreeHelper.IsBooleanProperty(nodeType, PropertyName))
                {
                    IFrameDiscreteFrame NewDiscreteFrame = CreateDiscreteFrame();
                    NewDiscreteFrame.PropertyName = PropertyName;

                    IFrameKeywordFrame KeywordFalseFrame = CreateKeywordFrame();
                    KeywordFalseFrame.Text = $"{PropertyName}=False";
                    NewDiscreteFrame.Items.Add(KeywordFalseFrame);

                    IFrameKeywordFrame KeywordTrueFrame = CreateKeywordFrame();
                    KeywordTrueFrame.Text = $"{PropertyName}=True";
                    NewDiscreteFrame.Items.Add(KeywordTrueFrame);

                    RootFrame.Items.Add(NewDiscreteFrame);
                }
                else if (NodeTreeHelper.IsEnumProperty(nodeType, PropertyName))
                {
                    NodeTreeHelper.GetEnumRange(nodeType, PropertyName, out int Min, out int Max);

                    IFrameDiscreteFrame NewDiscreteFrame = CreateDiscreteFrame();
                    NewDiscreteFrame.PropertyName = PropertyName;

                    for (int i = Min; i <= Max; i++)
                    {
                        IFrameKeywordFrame KeywordFrame = CreateKeywordFrame();
                        KeywordFrame.Text = $"{PropertyName}={i}";
                        NewDiscreteFrame.Items.Add(KeywordFrame);
                    }

                    RootFrame.Items.Add(NewDiscreteFrame);
                }
                else if (NodeTreeHelper.IsStringProperty(nodeType, PropertyName))
                {
                    IFrameTextValueFrame NewDiscreteFrame = CreateTextValueFrame();
                    NewDiscreteFrame.PropertyName = PropertyName;
                    RootFrame.Items.Add(NewDiscreteFrame);
                }
                else if (NodeTreeHelper.IsGuidProperty(nodeType, PropertyName))
                { }
                else if (NodeTreeHelper.IsDocumentProperty(nodeType, PropertyName))
                { }
                else
                    throw new ArgumentOutOfRangeException(nameof(PropertyName));

            RootFrame.UpdateParent(RootTemplate, GetRoot());
        }

        /// <summary></summary>
        protected virtual IFrameFrame GetRoot()
        {
            return FrameFrame.FrameRoot;
        }

        /// <summary></summary>
        protected virtual IFrameTemplateReadOnlyDictionary BuildDefaultBlockListTemplate()
        {
            List<Type> Keys = new List<Type>(NodeHelper.CreateNodeDictionary<object>().Keys);

            IFrameTemplateDictionary DefaultDictionary = CreateEmptyTemplateDictionary();
            foreach (Type Key in Keys)
                AddBlockNodeTypes(DefaultDictionary, Key);

            IFramePlaceholderFrame PatternFrame = CreatePlaceholderFrame();
            PatternFrame.PropertyName = nameof(IBlock.ReplicationPattern);

            IFramePlaceholderFrame SourceFrame = CreatePlaceholderFrame();
            SourceFrame.PropertyName = nameof(IBlock.SourceIdentifier);

            IFrameHorizontalCollectionPlaceholderFrame CollectionPlaceholderFrame = CreateHorizontalCollectionPlaceholderFrame();

            IFrameHorizontalPanelFrame RootFrame = CreateHorizontalPanelFrame();
            RootFrame.Items.Add(PatternFrame);
            RootFrame.Items.Add(SourceFrame);
            RootFrame.Items.Add(CollectionPlaceholderFrame);

            IFrameBlockTemplate RootTemplate = CreateBlockTemplate();
            RootTemplate.NodeType = typeof(IBlock);
            RootTemplate.Root = RootFrame;

            RootFrame.UpdateParent(RootTemplate, GetRoot());

            List<Type> BlockKeys = new List<Type>(DefaultDictionary.Keys);
            foreach (Type Key in BlockKeys)
                DefaultDictionary[Key] = RootTemplate;

            return CreateTemplateReadOnlyDictionary(DefaultDictionary);
        }

        /// <summary></summary>
        protected virtual void AddBlockNodeTypes(IFrameTemplateDictionary dictionary, Type nodeType)
        {
            IList<string> Properties = NodeTreeHelper.EnumChildNodeProperties(nodeType);
            foreach (string PropertyName in Properties)
                if (NodeTreeHelperBlockList.IsBlockListProperty(nodeType, PropertyName, out Type ChildInterfaceType, out Type ChildItemType))
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
        protected virtual IFrameTemplateDictionary CreateEmptyTemplateDictionary()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameTemplateDictionary();
        }

        /// <summary>
        /// Creates a IxxxTemplateDictionary object.
        /// </summary>
        protected virtual IFrameTemplateDictionary CreateDefaultTemplateDictionary()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameTemplateDictionary(NodeHelper.CreateNodeDictionary<IFrameTemplate>());
        }

        /// <summary>
        /// Creates a IxxxTemplateReadOnlyDictionary object.
        /// </summary>
        protected virtual IFrameTemplateReadOnlyDictionary CreateTemplateReadOnlyDictionary(IFrameTemplateDictionary dictionary)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameTemplateReadOnlyDictionary(dictionary);
        }

        /// <summary>
        /// Creates a IxxxHorizontalPanelFrame object.
        /// </summary>
        protected virtual IFrameHorizontalPanelFrame CreateHorizontalPanelFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameHorizontalPanelFrame();
        }

        /// <summary>
        /// Creates a IxxxHorizontalCollectionPlaceholderFrame object.
        /// </summary>
        protected virtual IFrameHorizontalCollectionPlaceholderFrame CreateHorizontalCollectionPlaceholderFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameHorizontalCollectionPlaceholderFrame();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderFrame object.
        /// </summary>
        protected virtual IFramePlaceholderFrame CreatePlaceholderFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FramePlaceholderFrame();
        }

        /// <summary>
        /// Creates a IxxxOptionalFrame object.
        /// </summary>
        protected virtual IFrameOptionalFrame CreateOptionalFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameOptionalFrame();
        }

        /// <summary>
        /// Creates a IxxxHorizontalListFrame object.
        /// </summary>
        protected virtual IFrameHorizontalListFrame CreateHorizontalListFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameHorizontalListFrame();
        }

        /// <summary>
        /// Creates a IxxxHorizontalBlockListFrame object.
        /// </summary>
        protected virtual IFrameHorizontalBlockListFrame CreateHorizontalBlockListFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameHorizontalBlockListFrame();
        }

        /// <summary>
        /// Creates a IxxxDiscreteFrame object.
        /// </summary>
        protected virtual IFrameDiscreteFrame CreateDiscreteFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameDiscreteFrame();
        }

        /// <summary>
        /// Creates a IxxxKeywordFrame object.
        /// </summary>
        protected virtual IFrameKeywordFrame CreateKeywordFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameKeywordFrame();
        }

        /// <summary>
        /// Creates a IxxxTextValueFrame object.
        /// </summary>
        protected virtual IFrameTextValueFrame CreateTextValueFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameTextValueFrame();
        }

        /// <summary>
        /// Creates a IxxxTemplate object.
        /// </summary>
        protected virtual IFrameNodeTemplate CreateNodeTemplate()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameNodeTemplate();
        }

        /// <summary>
        /// Creates a IxxxTemplate object.
        /// </summary>
        protected virtual IFrameBlockTemplate CreateBlockTemplate()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameBlockTemplate();
        }

        /// <summary>
        /// Creates a IxxxTemplateSet object.
        /// </summary>
        protected virtual IFrameTemplateSet CreateDefaultTemplateSet(IFrameTemplateReadOnlyDictionary nodeTemplateTable, IFrameTemplateReadOnlyDictionary blockTemplateTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameTemplateSet(nodeTemplateTable, blockTemplateTable);
        }
        #endregion
    }
}
