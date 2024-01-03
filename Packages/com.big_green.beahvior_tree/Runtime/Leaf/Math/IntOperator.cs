//----------------------------------------
//author: BigGreen
//date: 2023-12-14 17:07
//----------------------------------------

using System;
using UnityEngine;

namespace BG.BT
{
    [Serializable]
    [BTGroup("Math")]
    public class IntOperator : Leaf
    {
        private enum Operation
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
        private Operation operation;
        [BTNodeDataField]
        private SharedInt int1;
        [BTNodeDataField]
        private SharedInt int2;
        [BTNodeDataField]
        private SharedInt storeResult;
      
        public override void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            base.Init(tree, treeAsset);
            BindTreeVariable(int1);
            BindTreeVariable(int2);
            BindTreeVariable(storeResult);
        }

        public override BTNodeState Evaluate()
        {
            switch (operation)
            {
                case Operation.Add:
                    storeResult.Value = int1.Value + int2.Value;
                    break;
                case Operation.Subtract:
                    storeResult.Value = int1.Value - int2.Value;
                    break;
                case Operation.Multiply:
                    storeResult.Value = int1.Value * int2.Value;
                    break;
                case Operation.Divide:
                    storeResult.Value = int1.Value / int2.Value;
                    break;
                case Operation.Modulo:
                    storeResult.Value = int1.Value % int2.Value;
                    break;
                case Operation.Min:
                    storeResult.Value = Mathf.Min(int1.Value, int2.Value);
                    break;
                case Operation.Max:
                    storeResult.Value = Mathf.Max(int1.Value, int2.Value);
                    break;
            }
            State = BTNodeState.Success; 
            return State;
        }
    }
}