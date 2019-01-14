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
    public interface IFocusNodeSemantic
    {
        /// <summary>
        /// Semantics for properties of the node.
        /// </summary>
        IFocusPropertySemanticList Items { get; }

        /// <summary>
        /// Checks that the property of a node is compatible with the semantic.
        /// </summary>
        /// <param name="node">The node to check.</param>
        /// <param name="propertyName">The property to check.</param>
        bool IsCompatible(INode node, string propertyName);

        /// <summary>
        /// Checks that the property of a node is a collection that must never be empty.
        /// </summary>
        /// <param name="propertyName">The property to check.</param>
        bool IsNeverEmpty(string propertyName);
    }

    /// <summary>
    /// Semantic describing all specifics of a node that are not captured in its structure.
    /// </summary>
    public class FocusNodeSemantic : IFocusNodeSemantic
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FocusNodeSemantic"/>.
        /// </summary>
        public FocusNodeSemantic()
        {
            Items = CreatePropertySemanticList();
        }

        /// <summary>
        /// Initializes an instance of <see cref="FocusNodeSemantic"/>.
        /// </summary>
        /// <param name="items">Initial list of property semantics.</param>
        public FocusNodeSemantic(IFocusPropertySemanticList items)
        {
            Items = items;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Semantics for properties of the node.
        /// </summary>
        public IFocusPropertySemanticList Items { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that the property of a node is compatible with the semantic.
        /// </summary>
        /// <param name="node">The node to check.</param>
        /// <param name="propertyName">The property to check.</param>
        public virtual bool IsCompatible(INode node, string propertyName)
        {
            Type ChildInterfaceType, ChildNodeType;

            foreach (IFocusPropertySemantic PropertySemantic in Items)
            {
                if (PropertySemantic.PropertyName == propertyName)
                {
                    if (PropertySemantic.IsNeverEmpty)
                    {
                        if (NodeTreeHelperList.IsNodeListProperty(node, propertyName, out ChildNodeType))
                        {
                            NodeTreeHelperList.GetChildNodeList(node, propertyName, out IReadOnlyList<INode> ChildNodeList);
                            if (ChildNodeList.Count == 0)
                                return false;
                        }

                        else if (NodeTreeHelperBlockList.IsBlockListProperty(node, propertyName, out ChildInterfaceType, out ChildNodeType))
                        {
                            NodeTreeHelperBlockList.GetChildBlockList(node, propertyName, out IReadOnlyList<INodeTreeBlock> ChildBlockList);
                            if (ChildBlockList.Count == 0)
                                return false;

                            Debug.Assert(ChildBlockList[0].NodeList.Count > 0);
                        }

                        else
                            Debug.Assert(false);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Checks that the property of a node is a collection that must never be empty.
        /// </summary>
        /// <param name="propertyName">The property to check.</param>
        public virtual bool IsNeverEmpty(string propertyName)
        {
            foreach (IFocusPropertySemantic PropertySemantic in Items)
                if (PropertySemantic.PropertyName == propertyName)
                    if (PropertySemantic.IsNeverEmpty)
                        return true;

            return false;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPropertySemanticList object.
        /// </summary>
        protected virtual IFocusPropertySemanticList CreatePropertySemanticList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusNodeSemantic));
            return new FocusPropertySemanticList();
        }
        #endregion
    }
}
