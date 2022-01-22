namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Easly;

    /// <summary>
    /// Cycle manager for Body nodes.
    /// </summary>
    public class FocusCycleManagerBody : FocusCycleManager
    {
        #region Properties
        /// <summary>
        /// Type of the base interface for all nodes participating to the cycle.
        /// </summary>
        public override Type InterfaceType { get { return typeof(Body); } }
        #endregion

        #region Implementation
        /// <summary></summary>
        protected override void AddNextNodeToCycle(IFocusCyclableNodeState state)
        {
            FocusInsertionChildNodeIndexList CycleIndexList = state.CycleIndexList;
            Node ParentNode = state.ParentState.Node;
            IFocusIndex NodeIndex = state.ParentIndex;

            Document Documentation = null;
            IBlockList<Assertion> RequireBlocks = null;
            IBlockList<Assertion> EnsureBlocks = null;
            IBlockList<Identifier> ExceptionIdentifierBlocks = null;
            IBlockList<EntityDeclaration> EntityDeclarationBlocks = null;
            IBlockList<Instruction> BodyInstructionBlocks = null;
            IBlockList<ExceptionHandler> ExceptionHandlerBlocks = null;
            IOptionalReference<ObjectType> AncestorType = null;

            List<Type> BodyTypeList = new List<Type>() { typeof(EffectiveBody), typeof(DeferredBody), typeof(ExternBody), typeof(PrecursorBody) };
            foreach (IFocusInsertionChildNodeIndex Index in CycleIndexList)
            {
                Body Body = Index.Node as Body;
                Debug.Assert(Body != null);

                if (BodyTypeList.Contains(Body.GetType()))
                    BodyTypeList.Remove(Body.GetType());

                switch (Body)
                {
                    case EffectiveBody AsEffective:
                        Documentation = AsEffective.Documentation;
                        RequireBlocks = AsEffective.RequireBlocks;
                        EnsureBlocks = AsEffective.EnsureBlocks;
                        ExceptionIdentifierBlocks = AsEffective.ExceptionIdentifierBlocks;
                        EntityDeclarationBlocks = AsEffective.EntityDeclarationBlocks;
                        BodyInstructionBlocks = AsEffective.BodyInstructionBlocks;
                        ExceptionHandlerBlocks = AsEffective.ExceptionHandlerBlocks;
                        break;

                    case DeferredBody AsDeferred:
                        Documentation = AsDeferred.Documentation;
                        RequireBlocks = AsDeferred.RequireBlocks;
                        EnsureBlocks = AsDeferred.EnsureBlocks;
                        ExceptionIdentifierBlocks = AsDeferred.ExceptionIdentifierBlocks;
                        break;

                    case ExternBody AsExtern:
                        Documentation = AsExtern.Documentation;
                        RequireBlocks = AsExtern.RequireBlocks;
                        EnsureBlocks = AsExtern.EnsureBlocks;
                        ExceptionIdentifierBlocks = AsExtern.ExceptionIdentifierBlocks;
                        break;

                    case PrecursorBody AsPrecursor:
                        Documentation = AsPrecursor.Documentation;
                        RequireBlocks = AsPrecursor.RequireBlocks;
                        EnsureBlocks = AsPrecursor.EnsureBlocks;
                        ExceptionIdentifierBlocks = AsPrecursor.ExceptionIdentifierBlocks;
                        AncestorType = AsPrecursor.AncestorType;
                        break;
                }
            }

            // If the list is full, no need to add more nodes to the cycle.
            if (BodyTypeList.Count > 0)
            {
                Type NodeType = BodyTypeList[0];

                if (RequireBlocks == null)
                    RequireBlocks = BlockListHelper.CreateEmptyBlockList<Assertion>();
                if (EnsureBlocks == null)
                    EnsureBlocks = BlockListHelper.CreateEmptyBlockList<Assertion>();
                if (ExceptionIdentifierBlocks == null)
                    ExceptionIdentifierBlocks = BlockListHelper.CreateEmptyBlockList<Identifier>();
                if (EntityDeclarationBlocks == null)
                    EntityDeclarationBlocks = BlockListHelper.CreateEmptyBlockList<EntityDeclaration>();
                if (BodyInstructionBlocks == null)
                    BodyInstructionBlocks = BlockListHelper.CreateEmptyBlockList<Instruction>();
                if (ExceptionHandlerBlocks == null)
                    ExceptionHandlerBlocks = BlockListHelper.CreateEmptyBlockList<ExceptionHandler>();
                if (AncestorType == null)
                    AncestorType = OptionalReferenceHelper.CreateReference(NodeHelper.CreateDefaultObjectType());

                Node NewBody = NodeHelper.CreateInitializedBody(NodeType, Documentation, RequireBlocks, EnsureBlocks, ExceptionIdentifierBlocks, EntityDeclarationBlocks, BodyInstructionBlocks, ExceptionHandlerBlocks, AncestorType);

                IFocusBrowsingInsertableIndex InsertableNodeIndex = NodeIndex as IFocusBrowsingInsertableIndex;
                Debug.Assert(InsertableNodeIndex != null);
                IFocusInsertionChildNodeIndex InsertionIndex = InsertableNodeIndex.ToInsertionIndex(ParentNode, NewBody) as IFocusInsertionChildNodeIndex;
                Debug.Assert(InsertionIndex != null);

                CycleIndexList.Add(InsertionIndex);
            }
        }
        #endregion
    }
}