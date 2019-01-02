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
        bool IsBlockValid(IFrameTemplateReadOnlyDictionary blockTemplateTable);

        /// <summary>
        /// Template that will be used to describe the given node.
        /// </summary>
        /// <param name="nodeType">Type of the node for which a template is requested.</param>
        IFrameTemplate NodeTypeToTemplate(Type nodeType);

        /// <summary>
        /// Template that will be used to describe the given block.
        /// </summary>
        /// <param name="blockType">Type of the block for which a template is requested.</param>
        IFrameTemplate BlockTypeToTemplate(Type blockType);
    }

    /// <summary>
    /// Set of templates used to describe all possible nodes in the tree.
    /// </summary>
    public class FrameTemplateSet : IFrameTemplateSet
    {
        #region Init
        /// <summary>
        /// Return a default template set.
        /// </summary>
        public static IFrameTemplateSet Default { get { return (new FrameTemplateSet()).BuildDefault() as IFrameTemplateSet; } }
        protected static IFrameTemplateSet _Default;

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
            Debug.Assert(nodeTemplateTable != null && IsValid(nodeTemplateTable));
            Debug.Assert(blockTemplateTable != null && IsBlockValid(blockTemplateTable));

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

            foreach (KeyValuePair<Type, IFrameTemplate> Entry in nodeTemplateTable)
            {
                Type NodeType = Entry.Key;
                IFrameTemplate Template = Entry.Value;

                if (!Template.IsValid)
                    return false;

                if (!Template.Root.IsValid(NodeType))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks that templates are valid for blocks.
        /// </summary>
        /// <param name="nodeTemplateTable">Table of templates.</param>
        public virtual bool IsBlockValid(IFrameTemplateReadOnlyDictionary blockTemplateTable)
        {
            Debug.Assert(blockTemplateTable != null);

            foreach (KeyValuePair<Type, IFrameTemplate> Entry in blockTemplateTable)
            {
                Type NodeType = Entry.Key;
                IFrameTemplate Template = Entry.Value;

                if (!NodeTreeHelper.IsBlockType(NodeType))
                    return false;

                if (!Template.IsValid)
                    return false;

                if (!Template.Root.IsValid(NodeType))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Template that will be used to describe the given node.
        /// </summary>
        /// <param name="nodeType">Type of the node for which a template is requested.</param>
        public virtual IFrameTemplate NodeTypeToTemplate(Type nodeType)
        {
            Debug.Assert(nodeType != null);
            Debug.Assert(NodeTemplateTable.ContainsKey(nodeType));

            return NodeTemplateTable[nodeType];
        }

        /// <summary>
        /// Template that will be used to describe the given block.
        /// </summary>
        /// <param name="blockType">Type of the block for which a template is requested.</param>
        public virtual IFrameTemplate BlockTypeToTemplate(Type blockType)
        {
            Debug.Assert(blockType != null);
            Debug.Assert(BlockTemplateTable.ContainsKey(blockType));

            return BlockTemplateTable[blockType];
        }
        #endregion

        #region Helper
        private IFrameTemplateSet BuildDefault()
        {
            if (_Default != null)
                return _Default;

            IFrameTemplateReadOnlyDictionary DefaultNodeTemplateTable = BuildDefaultNodeTemplateTable();
            IFrameTemplateReadOnlyDictionary DefaultBlockTemplateTable = BuildDefaultBlockListTemplate();
            _Default = CreateDefaultTemplateSet(DefaultNodeTemplateTable, DefaultBlockTemplateTable);

            return _Default;
        }

        protected virtual IFrameTemplateReadOnlyDictionary BuildDefaultNodeTemplateTable()
        {
            IFrameTemplateDictionary DefaultDictionary = CreateTemplateDictionary(NodeHelper.CreateNodeDictionary<IFrameTemplate>());

            List<Type> Keys = new List<Type>(DefaultDictionary.Keys);
            foreach (Type Key in Keys)
                SetNodeTypeToDefault(DefaultDictionary, Key);

            return CreateTemplateReadOnlyDictionary(DefaultDictionary);
        }

        protected virtual void SetNodeTypeToDefault(IFrameTemplateDictionary dictionary, Type nodeType)
        {
            Debug.Assert(dictionary.ContainsKey(nodeType));

            if (dictionary[nodeType] != null)
                return;

            IFrameHorizontalPanelFrame RootFrame = CreateHorizontalPanelFrame();
            IFrameTemplate RootTemplate = CreateTemplate();
            RootTemplate.NodeName = nodeType.Name;
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

                else if (NodeTreeHelper.IsDocumentProperty(nodeType, PropertyName))
                { }

                else
                {
                    IFrameValueFrame NewFrame = CreateValueFrame();
                    NewFrame.PropertyName = PropertyName;
                    RootFrame.Items.Add(NewFrame);
                }

            RootFrame.UpdateParentFrame(FrameFrame.Root);
        }

        protected virtual IFrameTemplateReadOnlyDictionary BuildDefaultBlockListTemplate()
        {
            List<Type> Keys = new List<Type>(NodeHelper.CreateNodeDictionary<object>().Keys);

            IFrameTemplateDictionary DefaultDictionary = CreateTemplateDictionary(new Dictionary<Type, IFrameTemplate>());
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

            RootFrame.UpdateParentFrame(FrameFrame.Root);

            IFrameTemplate DefaultBlockListTemplate = CreateTemplate();
            DefaultBlockListTemplate.NodeName = typeof(IBlockList).Name;
            DefaultBlockListTemplate.Root = RootFrame;

            List<Type> BlockKeys = new List<Type>(DefaultDictionary.Keys);
            foreach (Type Key in BlockKeys)
                DefaultDictionary[Key] = DefaultBlockListTemplate;

            return CreateTemplateReadOnlyDictionary(DefaultDictionary);
        }

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
        protected virtual IFrameTemplateDictionary CreateTemplateDictionary(IDictionary<Type, IFrameTemplate> dictionary)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameTemplateDictionary(dictionary);
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
        /// Creates a IxxxValueFrame object.
        /// </summary>
        protected virtual IFrameValueFrame CreateValueFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameValueFrame();
        }

        /// <summary>
        /// Creates a IxxxTemplate object.
        /// </summary>
        protected virtual IFrameTemplate CreateTemplate()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameTemplate();
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
