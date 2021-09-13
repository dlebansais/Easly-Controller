namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Easly;

    /// <summary>
    /// Cycle manager for Feature nodes.
    /// </summary>
    public class FocusCycleManagerFeature : FocusCycleManager
    {
        #region Properties
        /// <summary>
        /// Type of the base interface for all nodes participating to the cycle.
        /// </summary>
        public override Type InterfaceType { get { return typeof(Feature); } }
        #endregion

        #region Implementation
        /// <summary></summary>
        protected override void AddNextNodeToCycle(IFocusCyclableNodeState state)
        {
            FocusInsertionChildNodeIndexList CycleIndexList = state.CycleIndexList;
            Node ParentNode = state.ParentState.Node;
            IFocusNodeIndex NodeIndex = state.ParentIndex as IFocusNodeIndex;

            Document Documentation = null;
            Identifier ExportIdentifier = null;
            ExportStatus Export = ExportStatus.Exported;
            Name EntityName = null;
            ObjectType EntityType = null;
            BlockList<Assertion> EnsureBlocks = null;
            Expression ConstantValue = null;
            BlockList<CommandOverload> CommandOverloadBlocks = null;
            OnceChoice Once = OnceChoice.Normal;
            BlockList<QueryOverload> QueryOverloadBlocks = null;
            UtilityType PropertyKind = UtilityType.ReadWrite;
            BlockList<Identifier> ModifiedQueryBlocks = null;
            OptionalReference<Body> GetterBody = null;
            OptionalReference<Body> SetterBody = null;
            BlockList<EntityDeclaration> IndexParameterBlocks = null;
            ParameterEndStatus ParameterEnd = ParameterEndStatus.Closed;

            List<Type> FeatureTypeList = new List<Type>() { typeof(AttributeFeature), typeof(ConstantFeature), typeof(CreationFeature), typeof(FunctionFeature), typeof(ProcedureFeature), typeof(PropertyFeature), typeof(IndexerFeature) };
            foreach (IFocusInsertionChildNodeIndex Index in CycleIndexList)
            {
                Feature Feature = Index.Node as Feature;
                Debug.Assert(Feature != null);

                if (FeatureTypeList.Contains(Feature.GetType()))
                    FeatureTypeList.Remove(Feature.GetType());

                switch (Feature)
                {
                    case AttributeFeature AsAttribute:
                        Documentation = AsAttribute.Documentation;
                        ExportIdentifier = AsAttribute.ExportIdentifier;
                        Export = AsAttribute.Export;
                        EntityName = AsAttribute.EntityName;
                        EntityType = AsAttribute.EntityType;
                        EnsureBlocks = AsAttribute.EnsureBlocks;
                        break;

                    case ConstantFeature AsConstant:
                        Documentation = AsConstant.Documentation;
                        ExportIdentifier = AsConstant.ExportIdentifier;
                        Export = AsConstant.Export;
                        EntityName = AsConstant.EntityName;
                        EntityType = AsConstant.EntityType;
                        ConstantValue = AsConstant.ConstantValue;
                        break;

                    case CreationFeature AsCreation:
                        Documentation = AsCreation.Documentation;
                        ExportIdentifier = AsCreation.ExportIdentifier;
                        Export = AsCreation.Export;
                        EntityName = AsCreation.EntityName;
                        CommandOverloadBlocks = AsCreation.OverloadBlocks;
                        Debug.Assert(CommandOverloadBlocks != null);
                        break;

                    case FunctionFeature AsFunction:
                        Documentation = AsFunction.Documentation;
                        ExportIdentifier = AsFunction.ExportIdentifier;
                        Export = AsFunction.Export;
                        EntityName = AsFunction.EntityName;
                        Once = AsFunction.Once;
                        QueryOverloadBlocks = AsFunction.OverloadBlocks;
                        Debug.Assert(QueryOverloadBlocks != null);
                        break;

                    case ProcedureFeature AsProcedure:
                        Documentation = AsProcedure.Documentation;
                        ExportIdentifier = AsProcedure.ExportIdentifier;
                        Export = AsProcedure.Export;
                        EntityName = AsProcedure.EntityName;
                        CommandOverloadBlocks = AsProcedure.OverloadBlocks;
                        Debug.Assert(CommandOverloadBlocks != null);
                        break;

                    case PropertyFeature AsProperty:
                        Documentation = AsProperty.Documentation;
                        ExportIdentifier = AsProperty.ExportIdentifier;
                        Export = AsProperty.Export;
                        EntityName = AsProperty.EntityName;
                        EntityType = AsProperty.EntityType;
                        PropertyKind = AsProperty.PropertyKind;
                        ModifiedQueryBlocks = AsProperty.ModifiedQueryBlocks;
                        GetterBody = AsProperty.GetterBody;
                        SetterBody = AsProperty.SetterBody;
                        break;

                    case IndexerFeature AsIndexer:
                        Documentation = AsIndexer.Documentation;
                        ExportIdentifier = AsIndexer.ExportIdentifier;
                        Export = AsIndexer.Export;
                        EntityType = AsIndexer.EntityType;
                        IndexParameterBlocks = AsIndexer.IndexParameterBlocks;
                        ParameterEnd = AsIndexer.ParameterEnd;
                        ModifiedQueryBlocks = AsIndexer.ModifiedQueryBlocks;
                        GetterBody = AsIndexer.GetterBody;
                        SetterBody = AsIndexer.SetterBody;
                        Debug.Assert(IndexParameterBlocks != null);
                        break;
                }

                Debug.Assert(CommandOverloadBlocks == null || CommandOverloadBlocks.NodeBlockList.Count > 0);
                Debug.Assert(QueryOverloadBlocks == null || QueryOverloadBlocks.NodeBlockList.Count > 0);
                Debug.Assert(IndexParameterBlocks == null || IndexParameterBlocks.NodeBlockList.Count > 0);
            }

            // If the list is full, no need to add more nodes to the cycle.
            if (FeatureTypeList.Count > 0)
            {
                Type NodeType = FeatureTypeList[0];

                /*if (NodeType == typeof(CreationFeature))
                    Debug.Assert(CommandOverloadBlocks != null);*/

                Node NewFeature = NodeHelper.CreateInitializedFeature(NodeType, Documentation, ExportIdentifier, Export, EntityName, EntityType, EnsureBlocks, ConstantValue, CommandOverloadBlocks, Once, QueryOverloadBlocks, PropertyKind, ModifiedQueryBlocks, GetterBody, SetterBody, IndexParameterBlocks, ParameterEnd);

                IFocusInsertionChildNodeIndex InsertionIndex = (IFocusInsertionChildNodeIndex)((IFocusBrowsingInsertableIndex)NodeIndex).ToInsertionIndex(ParentNode, NewFeature);
                CycleIndexList.Add(InsertionIndex);
            }
        }
        #endregion
    }
}