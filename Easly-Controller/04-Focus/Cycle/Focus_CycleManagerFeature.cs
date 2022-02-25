namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using NotNullReflection;

    /// <summary>
    /// Cycle manager for Feature nodes.
    /// </summary>
    public class FocusCycleManagerFeature : FocusCycleManager
    {
        #region Properties
        /// <summary>
        /// Type of the base interface for all nodes participating to the cycle.
        /// </summary>
        public override Type InterfaceType { get { return Type.FromTypeof<Feature>(); } }
        #endregion

        #region Implementation
        /// <summary></summary>
        protected override void AddNextNodeToCycle(IFocusCyclableNodeState state)
        {
            FocusInsertionChildNodeIndexList CycleIndexList = state.CycleIndexList;
            Node ParentNode = state.ParentState.Node;
            IFocusNodeIndex NodeIndex = state.ParentIndex as IFocusNodeIndex;
            CycleFeatureInfo Info = new();

            List<Type> FeatureTypeList = new List<Type>() { Type.FromTypeof<AttributeFeature>(), Type.FromTypeof<ConstantFeature>(), Type.FromTypeof<CreationFeature>(), Type.FromTypeof<FunctionFeature>(), Type.FromTypeof<ProcedureFeature>(), Type.FromTypeof<PropertyFeature>(), Type.FromTypeof<IndexerFeature>() };
            foreach (IFocusInsertionChildNodeIndex Index in CycleIndexList)
            {
                Feature Feature = Index.Node as Feature;
                Debug.Assert(Feature != null);

                if (FeatureTypeList.Contains(Type.FromGetType(Feature)))
                    FeatureTypeList.Remove(Type.FromGetType(Feature));

                Info.Update(Feature);
            }

            // If the list is full, no need to add more nodes to the cycle.
            if (FeatureTypeList.Count > 0)
            {
                Type NodeType = FeatureTypeList[0];

                Node NewFeature = NodeHelper.CreateInitializedFeature(NodeType, Info.Documentation, Info.ExportIdentifier, Info.Export, Info.EntityName, Info.EntityType, Info.EnsureBlocks, Info.ConstantValue, Info.CommandOverloadBlocks, Info.Once, Info.QueryOverloadBlocks, Info.PropertyKind, Info.ModifiedQueryBlocks, Info.GetterBody, Info.SetterBody, Info.IndexParameterBlocks, Info.ParameterEnd);

                IFocusInsertionChildNodeIndex InsertionIndex = (IFocusInsertionChildNodeIndex)((IFocusBrowsingInsertableIndex)NodeIndex).ToInsertionIndex(ParentNode, NewFeature);
                CycleIndexList.Add(InsertionIndex);
            }
        }
        #endregion
    }
}