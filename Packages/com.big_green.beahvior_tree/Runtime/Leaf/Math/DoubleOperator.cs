//----------------------------------------
//author: BigGreen
//date: 2023-12-14 17:10
//----------------------------------------

using System;

namespace BG.BT
{
    [Serializable]
    [BTGroup("Math")]
    public class DoubleOperator : Leaf
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
        private SharedDouble double1;
        [BTNodeDataField]
        private SharedDouble double2;
        [BTNodeDataField]
        private SharedDouble storeResult;
       
        public override void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            base.Init(tree, treeAsset);
            BindTreeVariable(double1);
            BindTreeVariable(double2);
            BindTreeVariable(storeResult);
        }

        public override BTNodeState Evaluate()
        {
            switch (operation)
            {
                case Operation.Add:
                    storeResult.Value = double1.Value + double2.Value;
                    break;
                case Operation.Subtract:
                    storeResult.Value = double1.Value - double2.Value;
                    break;
                case Operation.Multiply:
                    storeResult.Value = double1.Value * double2.Value;
                    break;
                case Operation.Divide:
                    storeResult.Value = double1.Value / double2.Value;
                    break;
                case Operation.Modulo:
                    storeResult.Value = double1.Value % double2.Value;
                    break;
                case Operation.Min:
                    storeResult.Value = Math.Min(double1.Value, double2.Value);
                    break;
                case Operation.Max:
                    storeResult.Value = Math.Max(double1.Value, double2.Value);
                    break;
            }
            State = BTNodeState.Success; 
            return State;
        }
    }
}