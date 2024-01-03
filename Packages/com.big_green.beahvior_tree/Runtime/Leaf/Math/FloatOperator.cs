//----------------------------------------
//author: BigGreen
//date: 2023-12-14 17:05
//----------------------------------------

using System;
using UnityEngine;

namespace BG.BT
{
    [Serializable]
    [BTGroup("Math")]
    public class FloatOperator : Leaf
    {
        public enum Operation
        {
            Add,
            Subtract,
            Multiply,
            Divide,
            Modulo,
            Min,
            Max,
        }
        
        [BTNodeDataField]
        public Operation operation;
        [BTNodeDataField]
        public SharedFloat float1;
        [BTNodeDataField]
        public SharedFloat float2;
        [BTNodeDataField]
        public SharedFloat storeResult;
        
        public override void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            base.Init(tree, treeAsset);
            BindTreeVariable(float1);
            BindTreeVariable(float2);
            BindTreeVariable(storeResult);
        }

        public override BTNodeState Evaluate()
        {
            switch (operation)
            {
                case Operation.Add:
                    storeResult.Value = float1.Value + float2.Value;
                    break;
                case Operation.Subtract:
                    storeResult.Value = float1.Value - float2.Value;
                    break;
                case Operation.Multiply:
                    storeResult.Value = float1.Value * float2.Value;
                    break;
                case Operation.Divide:
                    storeResult.Value = float1.Value / float2.Value;
                    break;
                case Operation.Modulo:
                    storeResult.Value = float1.Value % float2.Value;
                    break;
                case Operation.Min:
                    storeResult.Value = Mathf.Min(float1.Value, float2.Value);
                    break;
                case Operation.Max:
                    storeResult.Value = Mathf.Max(float1.Value, float2.Value);
                    break;
            }
            State = BTNodeState.Success; 
            return State;
        }
    }
}