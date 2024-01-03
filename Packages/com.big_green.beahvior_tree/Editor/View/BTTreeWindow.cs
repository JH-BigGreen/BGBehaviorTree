//----------------------------------------
//author: BigGreen
//date: 2023-12-17 17:25
//----------------------------------------

using BG.BT;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace BG.BTEditor
{
    public class BTTreeWindow : EditorWindow
    {
        [SerializeField]
        private BehaviorTreeAsset m_TreeAsset;

        private Label m_TitleLabel;
        private TreeView m_TreeView;
        private Toggle m_BlackBoardToggle;
        private Toggle m_InspectorToggle;
        private Button m_SaveBtn;
        private VisualElement m_LeftContent;

        private string m_SelectNodeGuid;
        
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (Selection.activeObject is not BehaviorTreeAsset) return false;
            OpenTreeEditor((BehaviorTreeAsset)Selection.activeObject);
            return true;
        }
        
        public static void OpenTreeEditor(BehaviorTreeAsset treeAsset)
        {
            var window = GetWindow<BTTreeWindow>("Behavior Tree Editor");
            window.m_SelectNodeGuid = null;
            window.SetTreeAsset(treeAsset);
        }

        private void OnSelectionChange()
        {
            var activeObject = Selection.activeObject;
            if (activeObject is BehaviorTreeAsset asset)
            {
                if (asset == m_TreeAsset)
                {
                    return;
                }
                
                OpenTreeEditor(asset);
            }

            var obj = activeObject as GameObject;
            if (obj!= null && obj.GetComponent<BehaviorTree>() != null)
            {
                var tree = obj.GetComponent<BehaviorTree>();
                OpenTreeEditor(tree.asset);
            }
        }

        public void SetTreeAsset(BehaviorTreeAsset treeAsset)
        {
            m_TreeAsset = treeAsset;
            m_TreeView.SetTreeAsset(m_TreeAsset, this);
            RefreshWindow();
            Repaint();
        }

        private void CreateGUI()
        {
            var vt = Resources.Load<VisualTreeAsset>("BTTreeWindow");
            vt.CloneTree(rootVisualElement);
            m_TitleLabel = rootVisualElement.Q<Label>("asset-title");
            m_TreeView = rootVisualElement.Q<TreeView>("tree-view");
          
            m_BlackBoardToggle = rootVisualElement.Q<Toggle>("blackBoard-toggle");
            m_BlackBoardToggle.labelElement.style.color = Color.black;
            m_InspectorToggle = rootVisualElement.Q<Toggle>("inspector-toggle");
            m_InspectorToggle.labelElement.style.color = Color.black;
            m_LeftContent = rootVisualElement.Q<VisualElement>("left-content");
            m_SaveBtn =  rootVisualElement.Q<Button>("save-btn");
            RegisterCallback();
            
            if (m_TreeAsset != null)
            {
                m_TreeView.SetTreeAsset(m_TreeAsset, this);
                RefreshWindow();
            }
            
            Undo.undoRedoPerformed += UndoRedoPerformed;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= UndoRedoPerformed;
            m_SaveBtn.clicked -= OnSaveBtnClick;
        }

        private void RegisterCallback()
        {
            m_BlackBoardToggle.RegisterValueChangedCallback(OnBlackBoardToggleChanged);
            m_InspectorToggle.RegisterValueChangedCallback(OnInspectorToggleChanged);
            m_SaveBtn.clicked += OnSaveBtnClick;
            m_TreeView.OnSelectNode = OnSelectNode;
            //m_TreeView.OnUnSelectNode = OnUnSelectNode;
        }

        private void OnBlackBoardToggleChanged(ChangeEvent<bool> result)
        {
            m_InspectorToggle.value = !result.newValue;
            RefreshLeft();
        }
        
        private void OnInspectorToggleChanged(ChangeEvent<bool> result)
        {
            m_BlackBoardToggle.value = !result.newValue;
            RefreshLeft();
        }

        private void OnSaveBtnClick()
        {
            EditorUtility.SetDirty(m_TreeAsset);
            AssetDatabase.SaveAssets();
        }

        private void OnSelectNode(TreeNode node)
        {
            m_SelectNodeGuid = node.Node.guid;
            
            if (m_BlackBoardToggle.value)
            {
                return;
            }
            
            RefreshLeft();
        }

        // private void OnUnSelectNode(TreeNode node)
        // {
        //     if (m_SelectNodeGuid == node.Node.guid)
        //     {
        //         m_SelectNodeGuid = null;
        //     }
        //     
        //     if (m_BlackBoardToggle.value)
        //     {
        //         return;
        //     }
        //     
        //     RefreshLeft();
        // }

        private void UndoRedoPerformed()
        { 
            m_TreeView.Refresh();
            RefreshLeft();
            Repaint();
        }

        private void RefreshWindow()
        {
            RefreshLeft();
            RefreshRight();
        }

        private void RefreshLeft()
        {
            m_LeftContent.Clear();
            if (m_TreeAsset == null)
            {
                return;
            }
            if (m_BlackBoardToggle.value)
            {
                var panel = new BlackBoardPanel(m_TreeAsset);
                m_LeftContent.Add(panel);
            }
            else if (m_InspectorToggle.value)
            {
                var panel = new InspectorPanel(this, m_TreeAsset, m_SelectNodeGuid);
                m_LeftContent.Add(panel);
            }
        }

        private void RefreshRight()
        {
            RefreshTitle();
            RefreshGraphView();
        }

        private void RefreshTitle()
        {
            if (m_TreeAsset == null)
            {
                m_TitleLabel.text = "";
                return;
            }
            m_TitleLabel.text = m_TreeAsset.name;
        }

        private void RefreshGraphView()
        {
            m_TreeView.Refresh();
        }

        public void NodeDescChanged(string comment, string guid)
        {
            m_TreeView.NodeDescChanged(comment, guid);
        }
        
        private void Update()
        {
            m_TreeView.UpdateNodeState();
        }
    }
}