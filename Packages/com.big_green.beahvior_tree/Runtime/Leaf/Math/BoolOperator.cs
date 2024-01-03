//----------------------------------------
//author: BigGreen
//date: 2023-12-14 17:03
//----------------------------------------

using System;

namespace BG.BT
{
    [Serializable]
    [BTGroup("Math")]
    public class BoolOperator : Leaf
    {
        public enum Operation
        {
            And,
            Or
        }
     
        [BTNodeDataField]
        public Operation operation;
        [BTNodeDataField]
        public SharedBool bool1;
        [BTNodeDataField]
        public SharedBool bool2;
        [BTNodeDataField]
        public SharedBool storeResult;

        public override void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            base.Init(tree, treeAsset);
            BindTreeVariable(bool1);
            BindTreeVariable(bool2);
            BindTreeVariable(storeResult);
        }

        public override BTNodeState Evaluate()
        {
            switch (operation)
            {
                case Operation.And:
                    storeResult.Value = bool1.Value && bool2.Value;
                    break;
                case Operation.Or:
                    storeResult.Value = bool1.Value || bool2.Value;
                    break;
            }
            State = BTNodeState.Success; 
            return State;
        }
    }
}