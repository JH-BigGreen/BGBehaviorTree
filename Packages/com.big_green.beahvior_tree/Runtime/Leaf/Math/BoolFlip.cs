//----------------------------------------
//author: BigGreen
//date: 2023-12-14 17:02
//----------------------------------------

using System;

namespace BG.BT
{
    [Serializable]
    [BTGroup("Math")]
    public class BoolFlip : Leaf
    {
        [BTNodeDataField]
        public SharedBool boolToFlip;

        public override void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            base.Init(tree, treeAsset);
            BindTreeVariable(boolToFlip);
        }

        public override BTNodeState Evaluate()
        {
            boolToFlip.Value = !boolToFlip.Value;
            State = BTNodeState.Success; 
            return State;
        }
    }
}