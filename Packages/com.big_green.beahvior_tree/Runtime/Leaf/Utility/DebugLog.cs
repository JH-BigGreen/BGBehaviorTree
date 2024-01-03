//----------------------------------------
//author: BigGreen
//date: 2023-12-14 16:28
//----------------------------------------

using System;
using UnityEngine;

namespace BG.BT
{
    [Serializable]
    [BTGroup("Utility")]
    public class DebugLog : Leaf
    {
        [BTNodeDataField]
        public SharedString log;

        public override void Init(BehaviorTree tree, BehaviorTreeAsset treeAsset)
        {
            base.Init(tree, treeAsset);
            BindTreeVariable(log);
        }

        public override BTNodeState Evaluate()
        {
            Debug.Log(log.Value);
            State = BTNodeState.Success; 
            return State;
        }
    }
}