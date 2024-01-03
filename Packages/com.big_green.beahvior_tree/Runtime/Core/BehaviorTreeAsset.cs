//----------------------------------------
//author: BigGreen
//date: 2023-10-21 20:27
//----------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace BG.BT
{
    [CreateAssetMenu(menuName = "BehaviourTree/Asset", fileName = "BehaviorTreeAsset")]
    public class BehaviorTreeAsset : ScriptableObject
    {
        public Root root;
        [SerializeReference]
        public List<SharedVariable> variables;
        [SerializeReference]
        public List<BTNode> unusedNodes;

        public BTNode GetBtNode(string guid)
        {
            var result = GetBtNodeInternal(root, guid);
            if (result != null)
            {
                return result;
            }

            foreach (var node in unusedNodes)
            {
                result = GetBtNodeInternal(node, guid);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private BTNode GetBtNodeInternal(BTNode parent, string guid)
        {
            var queue = new Queue<BTNode>(); 
            queue.Enqueue(parent);
            
            var queue2 = new Queue<BTNode>(); 
            while (queue.Count > 0)
            {
                var btNode = queue.Dequeue();
                if (btNode.guid == guid)
                {
                    return btNode;
                }
                queue2.Enqueue(btNode);
                var children = btNode.GetChildren();
                if (children != null)
                {
                    foreach (var child in children)
                    {
                        queue.Enqueue(child);
                    }
                }
            }

            return null;
        }

        public Queue<BTNode> GetAllNodes()
        {
            var queue = GetAllNodes(root);
            foreach (var node in unusedNodes)
            {
                var q = GetAllNodes(node);
                while (q.Count > 0)
                {
                    var n = q.Dequeue();
                    queue.Enqueue(n);
                }
            }

            return queue;
        }

        public Queue<BTNode> GetAllNodes(BTNode parent)
        {
            var queue = new Queue<BTNode>(); 
            queue.Enqueue(parent);
            
            var queue2 = new Queue<BTNode>(); 
            while (queue.Count > 0)
            {
                var btNode = queue.Dequeue();
                queue2.Enqueue(btNode);
                var children = btNode.GetChildren();
                if (children != null)
                {
                    foreach (var child in children)
                    {
                        queue.Enqueue(child);
                    }
                }
            }

            return queue2;
        }
    }
}