//----------------------------------------
//author: BigGreen
//date: 2023-12-14 15:06
//----------------------------------------

using System;

namespace BG.BT
{
    [Serializable]
    [BTGroup("Math")]
    public class IntComparison : Condition
    {
        public enum Operation
        {
            LessThan,
            LessThanOrEqualTo,
            EqualTo,
            NotEqualTo,
            GreaterThan,
            GreaterThanOrEqualTo,
        }

        [BTNodeDataField]
        public Operation operation;
        [BTNodeDataField]
        public SharedInt int1;
        [BTNodeDataField]
        public SharedInt int2;
        
        public override void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            base.Init(tree, treeAsset);
            BindTreeVariable(int1);
            BindTreeVariable(int2);
        }

        public override bool IsConditionMet()
        {
            switch (operation)
            {
                case Operation.LessThan:
                    return int1.Value < int2.Value;
                case Operation.LessThanOrEqualTo:
                    return int1.Value <= int2.Value;
                case Operation.EqualTo:
                    return int1.Value == int2.Value;
                case Operation.NotEqualTo:
                    return int1.Value != int2.Value;
                case Operation.GreaterThan:
                    return int1.Value > int2.Value;
                case Operation.GreaterThanOrEqualTo:
                    return int1.Value >= int2.Value;
            }
            return true;
        }
    }
}