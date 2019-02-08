namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Easly;

    /// <summary>
    /// Cycle manager for IBody nodes.
    /// </summary>
    public interface IFocusCycleManagerBody : IFocusCycleManager
    {
    }

    /// <summary>
    /// Cycle manager for IBody nodes.
    /// </summary>
    public class FocusCycleManagerBody : FocusCycleManager, IFocusCycleManagerBody
    {
        #region Properties
        /// <summary>
        /// Type of the base interface for all nodes participating to the cycle.
        /// </summary>
        public override Type InterfaceType { get { return typeof(IBody); } }
        #endregion

        #region Implementation
        /// <summary></summary>
        protected override void AddNextNodeToCycle(IFocusCyclableNodeState state)
        {
            IFocusInsertionChildNodeIndexList CycleIndexList = state.CycleIndexList;
            INode ParentNode = state.ParentState.Node;
            IFocusIndex NodeIndex = state.ParentIndex;

            IDocument Documentation = null;
            IBlockList<IAssertion, Assertion> RequireBlocks = null;
            IBlockList<IAssertion, Assertion> EnsureBlocks = null;
            IBlockList<IIdentifier, Identifier> ExceptionIdentifierBlocks = null;
            IBlockList<IEntityDeclaration, EntityDeclaration> EntityDeclarationBlocks = null;
            IBlockList<IInstruction, Instruction> BodyInstructionBlocks = null;
            IBlockList<IExceptionHandler, ExceptionHandler> ExceptionHandlerBlocks = null;
            IOptionalReference<IObjectType> AncestorType = null;

            List<Type> BodyTypeList = new List<Type>() { typeof(EffectiveBody), typeof(DeferredBody), typeof(ExternBody), typeof(PrecursorBody) };
            foreach (IFocusInsertionChildNodeIndex Index in CycleIndexList)
            {
                IBody Body = Index.Node as IBody;
                Debug.Assert(Body != null);

                if (BodyTypeList.Contains(Body.GetType()))
                    BodyTypeList.Remove(Body.GetType());

                switch (Body)
                {
                    case IEffectiveBody AsEffective:
                        Documentation = AsEffective.Documentation;
                        RequireBlocks = AsEffective.RequireBlocks;
                        EnsureBlocks = AsEffective.EnsureBlocks;
                        ExceptionIdentifierBlocks = AsEffective.ExceptionIdentifierBlocks;
                        EntityDeclarationBlocks = AsEffective.EntityDeclarationBlocks;
                        BodyInstructionBlocks = AsEffective.BodyInstructionBlocks;
                        ExceptionHandlerBlocks = AsEffective.ExceptionHandlerBlocks;
                        break;

                    case IDeferredBody AsDeferred:
                        Documentation = AsDeferred.Documentation;
                        RequireBlocks = AsDeferred.RequireBlocks;
                        EnsureBlocks = AsDeferred.EnsureBlocks;
                        ExceptionIdentifierBlocks = AsDeferred.ExceptionIdentifierBlocks;
                        break;

                    case IExternBody AsExtern:
                        Documentation = AsExtern.Documentation;
                        RequireBlocks = AsExtern.RequireBlocks;
                        EnsureBlocks = AsExtern.EnsureBlocks;
                        ExceptionIdentifierBlocks = AsExtern.ExceptionIdentifierBlocks;
                        break;

                    case IPrecursorBody AsPrecursor:
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

                INode NewBody = NodeHelper.CreateInitializedBody(NodeType, Documentation, RequireBlocks, EnsureBlocks, ExceptionIdentifierBlocks, EntityDeclarationBlocks, BodyInstructionBlocks, ExceptionHandlerBlocks, AncestorType);

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