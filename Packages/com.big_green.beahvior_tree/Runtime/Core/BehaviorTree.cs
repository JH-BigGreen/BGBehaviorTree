//----------------------------------------
//author: BigGreen
//date: 2023-10-21 20:30
//----------------------------------------

using System;
using UnityEngine;

namespace BG.BT
{
    public class BehaviorTree : MonoBehaviour
    {
        public BehaviorTreeAsset asset;

        private bool m_Enable;

        private void Start()
        {
            EnableTree();
        }

        public void EnableTree()
        {
            m_Enable = true;
            
            if (asset != null)
            {
                asset.root.Init(this, asset);
            }
        }

        private void Update()
        {
            if (!m_Enable)
            {
                return;
            }
            
            if (asset != null)
            {
                asset.root.Evaluate();
            }
        }
    }
}