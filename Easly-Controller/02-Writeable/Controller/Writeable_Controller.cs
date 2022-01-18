namespace EaslyController.Writeable
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports operations to modify the tree.
    /// </summary>
    public partial class WriteableController : ReadOnlyController
    {
        private protected override void CheckInvariant()
        {
            base.CheckInvariant();

            bool IsValid = IsNodeTreeValid(RootState.Node);
            Debug.Assert(IsValid);
        }

        private protected virtual bool IsNodeTreeValid(Node node)
        {
            Type ChildNodeType;
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(node);
            bool IsValid = true;

            foreach (string PropertyName in PropertyNames)
            {
                if (NodeTreeHelperChild.IsChildNodeProperty(node, PropertyName, out ChildNodeType))
                {
                    IsValid &= InvariantFailed(IsNodeTreeChildNodeValid(node, PropertyName));
                }
                else if (NodeTreeHelperOptional.IsOptionalChildNodeProperty(node, PropertyName, out ChildNodeType))
                {
                    IsValid &= InvariantFailed(IsNodeTreeOptionalNodeValid(node, PropertyName));
                }
                else if (NodeTreeHelperList.IsNodeListProperty(node, PropertyName, out ChildNodeType))
                {
                    IsValid &= InvariantFailed(IsNodeTreeListValid(node, PropertyName));
                }
                else if (NodeTreeHelperBlockList.IsBlockListProperty(node, PropertyName, /*out Type ChildInterfaceType,*/ out ChildNodeType))
                {
                    IsValid &= InvariantFailed(IsNodeTreeBlockListValid(node, PropertyName));
                }
            }

            return IsValid;
        }

        private protected virtual bool IsNodeTreeChildNodeValid(Node node, string propertyName)
        {
            NodeTreeHelperChild.GetChildNode(node, propertyName, out Node ChildNode);
            Debug.Assert(ChildNode != null);

            bool IsValid = InvariantFailed(IsNodeTreeValid(ChildNode));

            return IsValid;
        }

        private protected virtual bool IsNodeTreeOptionalNodeValid(Node node, string propertyName)
        {
            bool IsValid = true;

            NodeTreeHelperOptional.GetChildNode(node, propertyName, out bool IsAssigned, out _, out Node ChildNode);
            if (IsAssigned)
            {
                IsValid &= InvariantFailed(IsNodeTreeValid(ChildNode));
            }

            return IsValid;
        }

        private protected virtual bool IsNodeTreeListValid(Node node, string propertyName)
        {
            NodeTreeHelperList.GetChildNodeList(node, propertyName, out IReadOnlyList<Node> ChildNodeList);
            Debug.Assert(ChildNodeList != null);

            bool IsValid = true;

            if (ChildNodeList.Count == 0)
            {
                IsValid &= InvariantFailed(IsEmptyListValid(node, propertyName));
            }

            for (int Index = 0; Index < ChildNodeList.Count; Index++)
            {
                Node ChildNode = ChildNodeList[Index];
                Debug.Assert(ChildNode != null);

                IsValid &= InvariantFailed(IsNodeTreeValid(ChildNode));
            }

            return IsValid;
        }

        private protected virtual bool IsNodeTreeBlockListValid(Node node, string propertyName)
        {
            NodeTreeHelperBlockList.GetChildBlockList(node, propertyName, out IList<NodeTreeBlock> ChildBlockList);
            Debug.Assert(ChildBlockList != null);

            bool IsValid = true;

            if (ChildBlockList.Count == 0)
            {
                IsValid &= InvariantFailed(IsEmptyBlockListValid(node, propertyName));
            }

            for (int BlockIndex = 0; BlockIndex < ChildBlockList.Count; BlockIndex++)
            {
                NodeTreeBlock Block = ChildBlockList[BlockIndex];
                Debug.Assert(Block.NodeList.Count > 0);

                for (int Index = 0; Index < Block.NodeList.Count; Index++)
                {
                    Node ChildNode = Block.NodeList[Index];
                    Debug.Assert(ChildNode != null);

                    IsValid &= InvariantFailed(IsNodeTreeValid(ChildNode));
                }
            }

            return IsValid;
        }

        private protected virtual bool IsEmptyListValid(Node node, string propertyName)
        {
            Type NodeType = node.GetType();
            Debug.Assert(NodeTreeHelperList.IsNodeListProperty(NodeType, propertyName, out Type ChildNodeType));

            bool IsValid = true;

            //Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);
            Type InterfaceType = NodeType;

            IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
            if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
            {
                foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                {
                    IsValid &= InvariantFailed(Item != propertyName);
                }
            }

            return IsValid;
        }

        private protected virtual bool IsEmptyBlockListValid(Node node, string propertyName)
        {
            Type NodeType = node.GetType();
            Debug.Assert(NodeTreeHelperBlockList.IsBlockListProperty(NodeType, propertyName, /*out Type ChildInterfaceType,*/ out Type ChildNodeType));

            bool IsValid = true;

            //Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);
            Type InterfaceType = NodeType;

            IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
            if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
            {
                foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                {
                    IsValid &= InvariantFailed(Item != propertyName);
                }
            }

            return IsValid;
        }

        private protected bool InvariantFailed(bool condition)
        {
            Debug.Assert(condition, "Invariant");
            return condition;
        }
    }
}
