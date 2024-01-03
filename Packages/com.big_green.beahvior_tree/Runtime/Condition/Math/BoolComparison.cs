//----------------------------------------
//author: BigGreen
//date: 2023-12-14 15:01
//----------------------------------------

using System;

namespace BG.BT
{
    [Serializable]
    [BTGroup("Math")]
    public class BoolComparison : Condition
    {
        public enum Operation
        {
            EqualTo,
            NotEqualTo,
        }

        [BTNodeDataField]
        public Operation operation;
        [BTNodeDataField]
        public SharedBool bool1;
        [BTNodeDataField]
        public SharedBool bool2;
      
        public override void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            base.Init(tree, treeAsset);
            BindTreeVariable(bool1);
            BindTreeVariable(bool2);
        }

        public override bool IsConditionMet()
        {
            switch (operation)
            {
                case Operation.EqualTo:
                    return bool1.Value == bool2.Value;
                    
                case Operation.NotEqualTo:
                    return bool1.Value != bool2.Value;
            }

            return true;
        }
    }
}