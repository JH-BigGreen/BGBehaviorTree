//----------------------------------------
//author: BigGreen
//date: 2023-12-20 12:50
//----------------------------------------

using System;
using BG.BT;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BG.BTEditor
{
    public class TreeNode : Node
    {
        private BTNode m_BTNode;

        private Label m_DescLabel;
        private Port m_InputPort;
        private Port m_OutputPort;

        public BTNode Node => m_BTNode;
        public Port InputPort => m_InputPort;
        public Port OutputPort => m_OutputPort;

        private Action<TreeNode> m_OnSelectNode;
        private Action<TreeNode> m_OnUnSelectNode;

        public TreeNode(BTNode btNode) : base(AssetDatabase.GetAssetPath(Resources.Load<VisualTreeAsset>("TreeNode")))
        {
            m_BTNode = btNode;

            title = m_BTNode.GetType().Name;
            m_DescLabel = this.Q<Label>("description-label");
            SetDesc(m_BTNode.comment);
            
            style.left = m_BTNode.nodePosition.x;
            style.top = m_BTNode.nodePosition.y;

            CreateInputPort();
            CreateOutPort();
            SetupClasses();
        }

        private void CreateInputPort()
        {
            if (m_BTNode is Root)
            {
                return;
            }
            m_InputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(BTNode));
            m_InputPort.portName = "";
            m_InputPort.name = "input-port";
            inputContainer.Add(m_InputPort);
        }

        private void CreateOutPort()
        {
            if (m_BTNode is Root)
            {
                m_OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(BTNode));
            }
            else if (m_BTNode is Leaf)
            {
                m_OutputPort = null;
            }
            else if (m_BTNode is Composite)
            {
                m_OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(BTNode));
            }
            else if (m_BTNode is Condition)
            {
                m_OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(BTNode));
            }
            else if (m_BTNode is Decorator)
            {
                m_OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(BTNode));
            }

            if (m_OutputPort == null)
            {
                return;
            }
            
            m_OutputPort.portName = "";
            m_OutputPort.name = "output-port";
            outputContainer.Add(m_OutputPort);
        }

        private void SetupClasses()
        {
            if (m_BTNode is Root)
            {
                AddToClassList("root");
            }
            else if (m_BTNode is Leaf)
            {
                AddToClassList("leaf");
            }
            else if (m_BTNode is Composite)
            {
                AddToClassList("composite");
            }
            else if (m_BTNode is Condition)
            {
                AddToClassList("condition");
            }
            else if (m_BTNode is Decorator)
            {
                AddToClassList("decorator");
            }
        }
        
        public override void OnSelected()
        {
            base.OnSelected();
            m_OnSelectNode?.Invoke(this);
        }

        public override void OnUnselected()
        {
            m_OnUnSelectNode?.Invoke(this);
        }
        
        public void SetSelectCallback(Action<TreeNode> selectCallback)
        {
            m_OnSelectNode = selectCallback;
            //m_OnUnSelectNode = unSelectCallback;
        }

        public void SetDesc(string comment)
        {
            if (string.IsNullOrEmpty(comment))
            {
                m_DescLabel.style.display = DisplayStyle.None;
            }
            else
            {
                m_DescLabel.style.display = DisplayStyle.Flex;
                m_DescLabel.text = comment;
            }          
        }

        public void UpdateState()
        {
            RemoveFromClassList("running");
            RemoveFromClassList("success");
            RemoveFromClassList("failure");

            if (!Application.isPlaying)
            {
                return;
            }

            switch (Node.State)
            {
                case BTNodeState.Success:
                    AddToClassList("success");
                    break;
                case BTNodeState.Failure:
                    AddToClassList("failure");
                    break;
                case BTNodeState.Running:
                    AddToClassList("running");
                    break;
                default:
                    break;
            }
        }
    }
}