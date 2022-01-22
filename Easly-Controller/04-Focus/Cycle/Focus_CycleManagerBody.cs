﻿namespace EaslyController.Focus
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
            CycleBodyInfo Info = new();

            List<Type> BodyTypeList = new List<Type>() { typeof(EffectiveBody), typeof(DeferredBody), typeof(ExternBody), typeof(PrecursorBody) };
            foreach (IFocusInsertionChildNodeIndex Index in CycleIndexList)
            {
                Body Body = (Body)Index.Node;

                if (BodyTypeList.Contains(Body.GetType()))
                    BodyTypeList.Remove(Body.GetType());

                Info.Update(Body);
            }

            // If the list is full, no need to add more nodes to the cycle.
            if (BodyTypeList.Count > 0)
            {
                Type NodeType = BodyTypeList[0];
                Node NewBody = NodeHelper.CreateInitializedBody(NodeType, Info.Documentation, Info.RequireBlocks, Info.EnsureBlocks, Info.ExceptionIdentifierBlocks, Info.EntityDeclarationBlocks, Info.BodyInstructionBlocks, Info.ExceptionHandlerBlocks, Info.AncestorType);

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
