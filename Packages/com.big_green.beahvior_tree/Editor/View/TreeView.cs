//----------------------------------------
//author: BigGreen
//date: 2023-12-17 17:28
//----------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BG.BT;
using OdinSerializer;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using SerializationUtility = OdinSerializer.SerializationUtility;

namespace BG.BTEditor
{
    public class TreeView : GraphView
    {
        private BehaviorTreeAsset m_TreeAsset;
        public BTTreeWindow treeWindow;

        public Action<TreeNode> OnSelectNode;
        //public Action<TreeNode> OnUnSelectNode;

        public new class UxmlFactory : UxmlFactory<TreeView, UxmlTraits>
        {
        }

        public TreeView()
        {
            style.flexGrow = 1;
            Insert(0, new GridBackground() {name = "grid_background"});
            AddManipulators();
            AddProvider();
            RegisterCallback();
        }

        private void AddManipulators()
        {
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        private void AddProvider()
        {
            var provider = ScriptableObject.CreateInstance<NodeSearchWindowProvider>();
            provider.Init(this);
            nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), provider);
            };
        }

        private void RegisterCallback()
        {
            graphViewChanged += OnGraphViewChanged;
        }
        
        public void SetTreeAsset( BehaviorTreeAsset treeAsset, BTTreeWindow treeWindowEditor)
        {
            m_TreeAsset = treeAsset;
            treeWindow = treeWindowEditor;
        }

        public void Refresh()
        {
            ClearView();
            if (m_TreeAsset == null)
            {
                return;
            }
            SetTreeNodes();
            SetUnusedTreeNodes();
        }

        private void ClearView()
        {
            var list = new List<GraphElement>();
            foreach (var node in nodes)
            {
                list.Add(node);
            }

            foreach (var edge in edges)
            {
                list.Add(edge);
            }

            for (int i = list.Count - 1; i >= 0; i--)
            {
                RemoveElement(list[i]);
            }
        }

        private void SetTreeNodes()
        {
            var root = m_TreeAsset.root;
            var rootTreeNode = new TreeNode(root);
            rootTreeNode.capabilities &= ~Capabilities.Deletable;
            rootTreeNode.capabilities &= ~Capabilities.Copiable;
            rootTreeNode.SetSelectCallback(OnSelectNode);
            AddElement(rootTreeNode);

            SetChildrenAndEdge(root, rootTreeNode);
        }

        private void SetUnusedTreeNodes()
        {
            var unusedList = m_TreeAsset.unusedNodes;
            foreach (var unusedNode in unusedList)
            {
                var treeNode = new TreeNode(unusedNode);
                treeNode.SetSelectCallback(OnSelectNode);
                AddElement(treeNode);

                SetChildrenAndEdge(unusedNode, treeNode);
            }
        }

        private void SetChildrenAndEdge(BTNode parentNode, TreeNode parentTreeNode)
        {
            var children = parentNode.GetChildren();
            if (children == null)
            {
                return;
            }

            foreach (var child in children)
            {
                var childTreeNode = new TreeNode(child);
                childTreeNode.SetSelectCallback(OnSelectNode);
                AddElement(childTreeNode);

                var edge = parentTreeNode.OutputPort.ConnectTo(childTreeNode.InputPort);
                AddElement(edge);

                SetChildrenAndEdge(child, childTreeNode);
            }
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                foreach (var element in graphViewChange.elementsToRemove)
                {
                    switch (element)
                    {
                        case TreeNode treeNode:
                            DeleteNode(treeNode);
                            break;
                        case Edge edge:
                            DeleteEdge(edge);
                            break;
                        default:
                            //todo 与group有关
                            throw new Exception("先定位一下！");
                    }
                }
            }

            if (graphViewChange.edgesToCreate != null)
            {
                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    AddEdge(edge);
                }
            }

            if (graphViewChange.movedElements != null)
            {
                foreach (var element in graphViewChange.movedElements)
                {
                    switch (element)
                    {
                        case TreeNode treeNode:
                            MoveNode(treeNode);
                            break;
                        default:
                            break;
                    }
                }
            }

            return graphViewChange;
        }

        public TreeNode CreateNode(BTNode btNode, Vector2 position, bool setGuid = true)
        {
            if (setGuid)
            {
                btNode.guid = Guid.NewGuid().ToString();    
            }            
            btNode.nodePosition = position;
            Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Add Node");
            m_TreeAsset.unusedNodes.Add(btNode);
            EditorUtility.SetDirty(m_TreeAsset);

            var treeNode = new TreeNode(btNode);
            treeNode.SetSelectCallback(OnSelectNode);
            AddElement(treeNode);

            return treeNode;
        }

        public void DeleteNode(TreeNode treeNode)
        {
            var node = treeNode.Node;
            if (!m_TreeAsset.unusedNodes.Contains(node))
            {
                throw new Exception("一定在没有使用的节点中！");
            }

            if (node.GetChildren() != null)
            {
                throw new Exception("一定没有子节点！");
            }

            Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Remove Node");
            m_TreeAsset.unusedNodes.Remove(node);
            EditorUtility.SetDirty(m_TreeAsset);
        }

        public void AddEdge(Edge edge)
        {
            var parentNode = ((TreeNode) edge.output.node).Node;
            var childNode = ((TreeNode) edge.input.node).Node;

            if (!m_TreeAsset.unusedNodes.Contains(childNode))
            {
                throw new Exception("一定在没有使用的节点中！");
            }

            Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Add Edge");
            m_TreeAsset.unusedNodes.Remove(childNode);
            parentNode.AddChild(childNode);
            EditorUtility.SetDirty(m_TreeAsset);
        }

        private void DeleteEdge(Edge edge)
        {
            var parentNode = ((TreeNode) edge.output.node).Node;
            var childNode = ((TreeNode) edge.input.node).Node;
            Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Remove Edge");
            parentNode.RemoveChild(childNode);
            m_TreeAsset.unusedNodes.Add(childNode);
            EditorUtility.SetDirty(m_TreeAsset);
        }

        private void MoveNode(TreeNode treeNode)
        {
            Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Move Node");
            treeNode.Node.nodePosition = treeNode.GetPosition().position;
            if (treeNode.InputPort != null)
            {
                var es = treeNode.InputPort.connections;
                if (es != null && es.Count() == 1)
                {
                    foreach (var e in es)
                    {
                        var parentNode = (TreeNode)e.output.node;
                        parentNode.Node.SortChildren();
                        break;
                    }
                }
            }
            
            EditorUtility.SetDirty(m_TreeAsset);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var list = new List<Port>();

            foreach (var port in ports)
            {
                if (port.node != startPort.node && port.direction != startPort.direction &&
                    port.portType == startPort.portType)
                {
                    list.Add(port);
                }
            }

            return list;
        }

        public void UpdateNodeState()
        {
            foreach (var node in nodes)
            {
                var btNode = (TreeNode)node;
                btNode.UpdateState();
            }
        }

        #region 指令相关

        /// <summary>
        /// 去除默认的剪切
        /// </summary>
        protected override bool canCutSelection => false;

        /// <summary>
        /// 去除默认的复制
        /// </summary>
        protected override bool canCopySelection => false;
        
        /// <summary>
        /// 去除默认的复制
        /// </summary>
        protected override bool canDuplicateSelection => false;
        
        /// <summary>
        /// 去除默认的粘贴
        /// </summary>
        protected override bool canPaste => false;

        /// <summary>
        /// 重写复制
        /// </summary>
        private bool CanDuplicate => selection.All<ISelectable>((Func<ISelectable, bool>) (s =>
        {
            switch (s)
            {
                case Node n:
                    if (((TreeNode)n).Node is Root)
                    {
                        return false;
                    }

                    return true;
                case Group _:
                case Placemat _:
                case Edge _:
                    return true;
                default:
                    return s is StickyNote;
            }
        }));
        
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (evt.target is UnityEditor.Experimental.GraphView.GraphView && this.nodeCreationRequest != null)
            {
                evt.menu.AppendAction("Create Node", OnCreateNode,
                    new Func<DropdownMenuAction, DropdownMenuAction.Status>(DropdownMenuAction.AlwaysEnabled));
                evt.menu.AppendSeparator();
            }
            
            if (evt.target is UnityEditor.Experimental.GraphView.GraphView || evt.target is Node ||
                evt.target is Group || evt.target is Edge)
            {
                evt.menu.AppendSeparator();
                evt.menu.AppendAction("Delete",
                    (Action<DropdownMenuAction>) (a =>
                        DeleteSelectionCallback(UnityEditor.Experimental.GraphView.GraphView.AskUser.DontAskUser)),
                    (Func<DropdownMenuAction, DropdownMenuAction.Status>) (a =>
                        canDeleteSelection
                            ? DropdownMenuAction.Status.Normal
                            : DropdownMenuAction.Status.Disabled));
            }

            if (!(evt.target is UnityEditor.Experimental.GraphView.GraphView) && !(evt.target is Node) &&
                !(evt.target is Group))
                return;
            evt.menu.AppendSeparator();
            evt.menu.AppendAction("Duplicate", OnDuplicate,
                (Func<DropdownMenuAction, DropdownMenuAction.Status>) (a =>
                    CanDuplicate
                        ? DropdownMenuAction.Status.Normal
                        : DropdownMenuAction.Status.Disabled));
            evt.menu.AppendSeparator();
        }

        private void OnCreateNode(DropdownMenuAction a)
        {
            //没有好办法  反射一下 
            var type = GetType();
            var method = type.BaseType.GetMethod("OnContextMenuNodeCreate", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method == null)
            {
                return;
            }

            method.Invoke(this, new object[]{a});
        }

        private void OnDuplicate(DropdownMenuAction a)
        {
            var nodeList = new List<BTNode>();
            var edgeList = new List<Edge>();
            
            foreach (var item in selection)
            {
                if (item is TreeNode treeNode)
                {
                    var node = treeNode.Node;
                    nodeList.Add(node);
                }

                if (item is Edge edge)
                {
                    edgeList.Add(edge);
                }
            }

           
            var newNodeList = new List<TreeNode>();
            foreach (var node in nodeList)
            {
                var bytes = SerializationUtility.SerializeValue(node, DataFormat.Binary);
                var copyNode = SerializationUtility.DeserializeValue<BTNode>(bytes, DataFormat.Binary);
                copyNode.SetChildren(null);
                var newNode = CreateNode(copyNode, copyNode.nodePosition + new Vector2(20, 20), false);
                
                newNodeList.Add(newNode);
            }

            foreach (var edge in edgeList)
            {
                var outputNode = (TreeNode)edge.output.node;                
                if (outputNode.Node is Root)
                {
                    continue;
                }
                var inputNode = (TreeNode)edge.input.node;
                TreeNode newOutputNode = null;
                TreeNode newInputNode = null;
                foreach (var newNode in newNodeList)
                {
                    if (newNode.Node.guid == outputNode.Node.guid)
                    {
                        newOutputNode = newNode;
                    }

                    else if (newNode.Node.guid == inputNode.Node.guid)
                    {
                        newInputNode = newNode;
                    }
                }

                if (newOutputNode != null && newInputNode != null)
                {
                    var newEdge = new Edge();
                    newEdge.output = newOutputNode.OutputPort;
                    newEdge.input = newInputNode.InputPort;
                    AddElement(newEdge);
                    AddEdge(newEdge);
                }
            }

            foreach (var newNode in newNodeList)
            {
                newNode.Node.guid = Guid.NewGuid().ToString();
            }

            EditorUtility.SetDirty(m_TreeAsset);
        }

        #endregion

        public void NodeDescChanged(string comment, string guid)
        {
            foreach (var node in nodes)
            {
                var treeNode = (TreeNode) node;
                if (treeNode.Node.guid == guid)
                {
                    treeNode.SetDesc(comment);
                    break;
                }
            }
        }
    }
}