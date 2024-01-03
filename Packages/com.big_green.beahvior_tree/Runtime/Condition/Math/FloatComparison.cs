//----------------------------------------
//author: BigGreen
//date: 2023-12-14 15:14
//----------------------------------------

using System;

namespace BG.BT
{
    [Serializable]
    [BTGroup("Math")]
    public class FloatComparison : Condition
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
        public SharedFloat float1;
        [BTNodeDataField]
        public SharedFloat float2;
        
        public override void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            base.Init(tree, treeAsset);
            BindTreeVariable(float1);
            BindTreeVariable(float2);
        }

        public override bool IsConditionMet()
        {
            switch (operation)
            {
                case Operation.LessThan:
                    return float1.Value < float2.Value;
                case Operation.LessThanOrEqualTo:
                    return float1.Value <= float2.Value;
                case Operation.EqualTo:
                    return float1.Value == float2.Value;
                case Operation.NotEqualTo:
                    return float1.Value != float2.Value;
                case Operation.GreaterThan:
                    return float1.Value > float2.Value;
                case Operation.GreaterThanOrEqualTo:
                    return float1.Value >= float2.Value;
           }
            return true;
        }
    }
}