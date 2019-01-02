using BaseNode;
using BaseNodeHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Frame
{
    public interface IFrameTemplateSet
    {
        IFrameTemplateReadOnlyDictionary TemplateTable { get; }
        IFrameTemplateReadOnlyDictionary BlockListTemplateTable { get; }
        IFrameTemplate IndexToTemplate(IFrameNodeIndex nodeIndex);
    }

    public class FrameTemplateSet : IFrameTemplateSet
    {
        #region Init
        public static IFrameTemplateSet Default { get { return (new FrameTemplateSet()).BuildDefault() as IFrameTemplateSet; } }
        protected static IFrameTemplateSet _Default;

        protected FrameTemplateSet()
        {
        }

        public FrameTemplateSet(IFrameTemplateReadOnlyDictionary templateTable, IFrameTemplateReadOnlyDictionary blockListTemplateTable)
        {
            Debug.Assert(templateTable != null && IsValid(templateTable));
            Debug.Assert(blockListTemplateTable != null && IsValid(blockListTemplateTable));

            TemplateTable = templateTable;
            BlockListTemplateTable = blockListTemplateTable;
        }
        #endregion

        #region Properties
        public IFrameTemplateReadOnlyDictionary TemplateTable { get; }
        public IFrameTemplateReadOnlyDictionary BlockListTemplateTable { get; }
        #endregion

        #region Client Interface
        public virtual bool IsValid(IFrameTemplateReadOnlyDictionary templateTable)
        {
            Debug.Assert(templateTable != null);

            foreach (KeyValuePair<Type, IFrameTemplate> Entry in templateTable)
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

        public virtual IFrameTemplate IndexToTemplate(IFrameNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);

            Type NodeType = nodeIndex.Node.GetType();
            Debug.Assert(TemplateTable.ContainsKey(NodeType));

            return TemplateTable[NodeType];
        }
        #endregion

        #region Helper
        public virtual IFrameTemplateSet BuildDefault()
        {
            if (_Default != null)
                return _Default;

            IFrameTemplateReadOnlyDictionary DefaultTemplateTable = BuildDefaultTemplateTable();
            IFrameTemplateReadOnlyDictionary DefaultBlockListTemplateTable = BuildDefaultBlockListTemplate();
            _Default = CreateDefaultTemplateSet(DefaultTemplateTable, DefaultBlockListTemplateTable);

            return _Default;
        }

        public virtual IFrameTemplateReadOnlyDictionary BuildDefaultTemplateTable()
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
        }

        public virtual IFrameTemplateReadOnlyDictionary BuildDefaultBlockListTemplate()
        {
            List<Type> Keys = new List<Type>(NodeHelper.CreateNodeDictionary<object>().Keys);

            IFrameTemplateDictionary DefaultBlockDictionary = CreateTemplateDictionary(new Dictionary<Type, IFrameTemplate>());
            foreach (Type Key in Keys)
                AddBlockNodeTypes(DefaultBlockDictionary, Key);

            IFramePlaceholderFrame PatternFrame = CreatePlaceholderFrame();
            PatternFrame.PropertyName = nameof(IBlock.ReplicationPattern);

            IFramePlaceholderFrame SourceFrame = CreatePlaceholderFrame();
            SourceFrame.PropertyName = nameof(IBlock.SourceIdentifier);

            IFrameHorizontalCollectionPlaceholderFrame CollectionPlaceholderFrame = CreateHorizontalCollectionPlaceholderFrame();

            IFrameHorizontalPanelFrame RootFrame = CreateHorizontalPanelFrame();
            RootFrame.Items.Add(PatternFrame);
            RootFrame.Items.Add(SourceFrame);
            RootFrame.Items.Add(CollectionPlaceholderFrame);

            IFrameTemplate DefaultBlockListTemplate = CreateTemplate();
            DefaultBlockListTemplate.NodeName = typeof(IBlockList).Name;
            DefaultBlockListTemplate.Root = RootFrame;

            List<Type> BlockKeys = new List<Type>(DefaultBlockDictionary.Keys);
            foreach (Type Key in BlockKeys)
                DefaultBlockDictionary[Key] = DefaultBlockListTemplate;

            return CreateTemplateReadOnlyDictionary(DefaultBlockDictionary);
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
        protected virtual IFrameTemplateDictionary CreateTemplateDictionary(IDictionary<Type, IFrameTemplate> dictionary)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameTemplateDictionary(dictionary);
        }

        protected virtual IFrameTemplateReadOnlyDictionary CreateTemplateReadOnlyDictionary(IFrameTemplateDictionary dictionary)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameTemplateReadOnlyDictionary(dictionary);
        }

        protected virtual IFrameHorizontalPanelFrame CreateHorizontalPanelFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameHorizontalPanelFrame();
        }

        protected virtual IFrameVerticalPanelFrame CreateVerticalPanelFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameVerticalPanelFrame();
        }

        protected virtual IFrameHorizontalCollectionPlaceholderFrame CreateHorizontalCollectionPlaceholderFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameHorizontalCollectionPlaceholderFrame();
        }

        protected virtual IFramePlaceholderFrame CreatePlaceholderFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FramePlaceholderFrame();
        }

        protected virtual IFrameOptionalFrame CreateOptionalFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameOptionalFrame();
        }

        protected virtual IFrameHorizontalListFrame CreateHorizontalListFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameHorizontalListFrame();
        }

        protected virtual IFrameHorizontalBlockListFrame CreateHorizontalBlockListFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameHorizontalBlockListFrame();
        }

        protected virtual IFrameValueFrame CreateValueFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameValueFrame();
        }

        protected virtual IFrameTemplate CreateTemplate()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameTemplate();
        }

        protected virtual IFrameTemplateSet CreateDefaultTemplateSet(IFrameTemplateReadOnlyDictionary templateTable, IFrameTemplateReadOnlyDictionary blockListTemplateTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTemplateSet));
            return new FrameTemplateSet(templateTable, blockListTemplateTable);
        }
        #endregion
    }
}
