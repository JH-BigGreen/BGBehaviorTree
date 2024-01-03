//----------------------------------------
//author: BigGreen
//date: 2023-12-14 15:16
//----------------------------------------

using System;

namespace BG.BT
{
    [Serializable]
    [BTGroup("Math")]
    public class DoubleComparison : Condition
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
        public SharedDouble double1;
        [BTNodeDataField]
        public SharedDouble double2;
       

        public override void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            base.Init(tree, treeAsset);
            BindTreeVariable(double1);
            BindTreeVariable(double2);
        }

        public override bool IsConditionMet()
        {
            switch (operation)
            {
                case Operation.LessThan:
                    return double1.Value < double2.Value;
                case Operation.LessThanOrEqualTo:
                    return double1.Value <= double2.Value;
                case Operation.EqualTo:
                    return double1.Value == double2.Value;
                case Operation.NotEqualTo:
                    return double1.Value != double2.Value;
                case Operation.GreaterThan:
                    return double1.Value > double2.Value;
                case Operation.GreaterThanOrEqualTo:
                    return double1.Value >= double2.Value;
            }
            return true;
        }
    }
}