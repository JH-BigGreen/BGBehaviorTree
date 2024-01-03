//----------------------------------------
//author: BigGreen
//date: 2023-12-27 16:54
//----------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using BG.BT;
using UnityEditor;
using UnityEngine.UIElements;

namespace BG.BTEditor
{
    public class InspectorPanel : VisualElement
    {
        private BTTreeWindow m_Window;
        private BehaviorTreeAsset m_TreeAsset;
        private string m_TreeNodeGuid;
        
        public InspectorPanel(BTTreeWindow window, BehaviorTreeAsset asset, string treeNodeGuid)
        {
            m_Window = window;
            m_TreeAsset = asset;
            m_TreeNodeGuid = treeNodeGuid;
            if (m_TreeNodeGuid == null)
            {
                Clear();
                return;
            }

            RefreshPanel();

        }

        private void RefreshPanel()
        {
            var nodeData = m_TreeAsset.GetBtNode(m_TreeNodeGuid);
            var allField = nodeData.GetType().GetFields(BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic);
            var fieldList = new List<FieldInfo>();
            foreach (var field in allField)
            {
                if (field.GetCustomAttribute<BTNodeDataFieldAttribute>() != null)
                {
                    fieldList.Add(field);
                }
            }

            foreach (var field in fieldList)
            {
                if (field.FieldType.IsSubclassOf(typeof(SharedVariable)))
                {
                    var sharedVariable = (SharedVariable)field.GetValue(nodeData);
                    var element = new SharedVariableField(m_TreeAsset, nodeData, field, sharedVariable);
                    Add(element);
                }
                else
                {
                    var resolver = FieldResolverFactory.Instance.GetResolver(m_TreeAsset, field.FieldType, field);
                    if (resolver is IListResolver listResolver)
                    {
                        var initValue = field.GetValue(nodeData);
                        if (initValue == null)
                        {
                            field.SetValue(nodeData, Activator.CreateInstance(field.FieldType));
                            EditorUtility.SetDirty(m_TreeAsset);
                        }

                        listResolver.DataSource = field.GetValue(nodeData);
                        resolver.CreateVisualElement();
                        resolver.SetLabel(field.Name);
                    }
                    else
                    {
                        resolver.CreateVisualElement();
                        resolver.SetLabel(field.Name);
                        resolver.SetInitValue(field.GetValue(nodeData));
                        resolver.RegisterValueCallback((obj) =>
                        {
                            Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Modify Tree Inspector");
                            field.SetValue(nodeData, obj);
                            EditorUtility.SetDirty(m_TreeAsset);
                        });
                    }

                    if (field.Name == "comment")
                    {
                        resolver.RegisterValueCallback((obj) =>
                        {
                            m_Window.NodeDescChanged((string)obj, nodeData.guid);
                        });
                    }
                    
                    var element = resolver.GetVisualElement();
                    Add(element);
                }
            }
        }
    }
}