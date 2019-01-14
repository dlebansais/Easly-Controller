using BaseNode;
using BaseNodeHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Semantic describing all specifics of a node that are not captured in its structure.
    /// </summary>
    public interface IFocusSemanticSet
    {
        /// <summary>
        /// Semantics for all nodes in the source code.
        /// </summary>
        IFocusNodeSemanticDictionary<Type> NodeSemanticTable { get; }

        /// <summary>
        /// Checks that semantics are valid for nodes.
        /// </summary>
        /// <param name="nodeTemplateTable">Table of semantics.</param>
        bool IsValid(IFocusNodeSemanticDictionary<Type> nodeSemanticTable);

        /// <summary>
        /// Checks that a semantic associated to a node type is valid.
        /// </summary>
        /// <param name="nodeType">The type for which semantic is provided.</param>
        /// <param name="nodeSemantic">The semantic to check.</param>
        bool IsValidNodeSemantic(Type nodeType, IFocusNodeSemantic nodeSemantic);

        /// <summary>
        /// Gets the semantic that will be used to analyze the given node.
        /// </summary>
        /// <param name="nodeType">Type of the node for which a semantic is requested.</param>
        IFocusNodeSemantic NodeTypeToSemantic(Type nodeType);

        /// <summary>
        /// Checks that a node tree is compatible with the semantic.
        /// </summary>
        bool IsCompatible(INode node);
    }

    /// <summary>
    /// Semantic describing all specifics of a node that are not captured in its structure.
    /// </summary>
    public class FocusSemanticSet : IFocusSemanticSet
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FocusSemanticSet"/>.
        /// </summary>
        /// <param name="nodeSemanticTable">Semantics for all nodes in the source code.</param>
        public FocusSemanticSet(IFocusNodeSemanticDictionary<Type> nodeSemanticTable)
        {
            Debug.Assert(IsValid(nodeSemanticTable));

            NodeSemanticTable = nodeSemanticTable;
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that semantics are valid for nodes.
        /// </summary>
        /// <param name="nodeTemplateTable">Table of semantics.</param>
        public virtual bool IsValid(IFocusNodeSemanticDictionary<Type> nodeSemanticTable)
        {
            Debug.Assert(nodeSemanticTable != null);

            IFocusNodeSemanticDictionary<Type> DefaultDictionary = CreateDefaultNodeSemanticDictionary();

            foreach (KeyValuePair<Type, IFocusNodeSemantic> Entry in DefaultDictionary)
                if (!nodeSemanticTable.ContainsKey(Entry.Key))
                    return false;

            if (nodeSemanticTable.Count != DefaultDictionary.Count)
                return false;

            foreach (KeyValuePair<Type, IFocusNodeSemantic> Entry in nodeSemanticTable)
            {
                Type NodeType = Entry.Key;
                IFocusNodeSemantic NodeSemantic = Entry.Value;

                if (!IsValidNodeSemantic(NodeType, NodeSemantic))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks that a semantic associated to a node type is valid.
        /// </summary>
        /// <param name="nodeType">The type for which semantic is provided.</param>
        /// <param name="nodeSemantic">The semantic to check.</param>
        public virtual bool IsValidNodeSemantic(Type nodeType, IFocusNodeSemantic nodeSemantic)
        {
            foreach (IFocusPropertySemantic PropertySemantic in nodeSemantic.Items)
            {
                string PropertyName = PropertySemantic.PropertyName;
                Type InterfaceType, NodeType;

                if (string.IsNullOrEmpty(PropertyName))
                    return false;

                if (NodeTreeHelper.GetPropertyOf(nodeType, PropertyName) == null)
                    return false;

                if (PropertySemantic.IsNeverEmpty)
                    if (!NodeTreeHelperBlockList.IsBlockListProperty(nodeType, PropertyName, out InterfaceType, out NodeType) && !NodeTreeHelperList.IsNodeListProperty(nodeType, PropertyName, out InterfaceType))
                        return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the semantic that will be used to analyze the given node.
        /// </summary>
        /// <param name="nodeType">Type of the node for which a semantic is requested.</param>
        public virtual IFocusNodeSemantic NodeTypeToSemantic(Type nodeType)
        {
            Debug.Assert(nodeType != null);
            Debug.Assert(NodeSemanticTable.ContainsKey(nodeType));

            return NodeSemanticTable[nodeType];
        }
        #endregion

        #region Properties
        /// <summary>
        /// Semantics for all nodes in the source code.
        /// </summary>
        public IFocusNodeSemanticDictionary<Type> NodeSemanticTable { get; }
        #endregion

        #region Helper
        /// <summary>
        /// Checks that a node tree is compatible with the semantic.
        /// </summary>
        public virtual bool IsCompatible(INode node)
        {
            IFocusNodeSemantic NodeSemantic = NodeTypeToSemantic(NodeTreeHelper.NodeTypeToInterfaceType(node.GetType()));

            Type ChildNodeType;
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(node);

            foreach (string PropertyName in PropertyNames)
            {
                if (NodeTreeHelperChild.IsChildNodeProperty(node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelperChild.GetChildNode(node, PropertyName, out INode ChildNode);
                    if (!IsCompatible(ChildNode))
                        return false;
                }

                else if (NodeTreeHelperOptional.IsOptionalChildNodeProperty(node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelperOptional.GetChildNode(node, PropertyName, out bool IsAssigned, out INode ChildNode);
                    if (IsAssigned)
                        if (!IsCompatible(ChildNode))
                            return false;
                }

                else if (NodeTreeHelperList.IsNodeListProperty(node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelperList.GetChildNodeList(node, PropertyName, out IReadOnlyList<INode> ChildNodeList);

                    for (int Index = 0; Index < ChildNodeList.Count; Index++)
                    {
                        INode ChildNode = ChildNodeList[Index];
                        if (!IsCompatible(ChildNode))
                            return false;
                    }
                }

                else if (NodeTreeHelperBlockList.IsBlockListProperty(node, PropertyName, out Type ChildInterfaceType, out ChildNodeType))
                {
                    NodeTreeHelperBlockList.GetChildBlockList(node, PropertyName, out IReadOnlyList<INodeTreeBlock> ChildBlockList);

                    for (int BlockIndex = 0; BlockIndex < ChildBlockList.Count; BlockIndex++)
                    {
                        INodeTreeBlock Block = ChildBlockList[BlockIndex];
                        for (int Index = 0; Index < Block.NodeList.Count; Index++)
                        {
                            INode ChildNode = Block.NodeList[Index];
                            if (!IsCompatible(ChildNode))
                                return false;
                        }
                    }
                }

                if (!NodeSemantic.IsCompatible(node, PropertyName))
                    return false;
            }

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeSemanticDictionary object.
        /// </summary>
        protected virtual IFocusNodeSemanticDictionary<Type> CreateDefaultNodeSemanticDictionary()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSemanticSet));
            return new FocusNodeSemanticDictionary<Type>(NodeHelper.CreateNodeDictionary<IFocusNodeSemantic>());
        }
        #endregion
    }
}
