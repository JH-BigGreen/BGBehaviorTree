//----------------------------------------
//author: BigGreen
//date: 2023-12-28 15:21
//----------------------------------------

using System;
using System.Linq;
using System.Reflection;
using BG.BT;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BG.BTEditor
{
    public class SharedVariableField : VisualElement
    {
        private BehaviorTreeAsset m_TreeAsset;
        private BTNode m_BtNode;
        private FieldInfo m_FieldInfo;
        private SharedVariable m_SharedVariable;
        
        private VisualElement m_VariableContainer;
        private DropdownField m_NameDropdown;
        private Toggle m_SharedToggle;

        public SharedVariableField(BehaviorTreeAsset asset, BTNode btNode, FieldInfo fieldInfo, SharedVariable sharedVariable)
        {
            m_TreeAsset = asset;
            m_FieldInfo = fieldInfo;
            m_BtNode = btNode;
            m_SharedVariable = sharedVariable;

            var titleContainer = new VisualElement();
            titleContainer.style.flexDirection = FlexDirection.Row;
            Add(titleContainer);

            m_VariableContainer = new VisualElement();
            m_VariableContainer.style.flexGrow = 1;
            titleContainer.Add(m_VariableContainer);

            m_SharedToggle = new Toggle();
            m_SharedToggle.RegisterValueChangedCallback(OnSharedToggleChanged);
            m_SharedToggle.value = m_SharedVariable.Shared;
            titleContainer.Add(m_SharedToggle);

            var titleLabel = new Label("Shared");
            titleLabel.style.color = Color.black;
            titleContainer.Add(titleLabel);
            
            if (!m_SharedVariable.Shared)
            {
                SetVariable();
            }
            else
            {
                SetSharedVariable();
            }
        }

        private void OnSharedToggleChanged(ChangeEvent<bool> evt)
        {
            Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Modify Inspector Variable Toggle");
            m_SharedVariable.Shared = evt.newValue;
            if (evt.newValue)
            {
                SetSharedVariable();
            }
            else
            {
                SetVariable();
            }
        }

        private void SetVariable()
        {
            m_VariableContainer.Clear();
            
            var valueInfo = m_SharedVariable.GetType().GetProperty("Value",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var resolver = FieldResolverFactory.Instance.GetResolver(m_TreeAsset, valueInfo.PropertyType, m_FieldInfo);
            if (resolver is IListResolver listResolver)
            {
                var value = m_SharedVariable.GetValue();
                if (value == null)
                {
                    m_SharedVariable.SetValue(Activator.CreateInstance(valueInfo.PropertyType));    
                    EditorUtility.SetDirty(m_TreeAsset);
                }
                listResolver.DataSource = m_SharedVariable.GetValue();
                resolver.CreateVisualElement();
                resolver.SetLabel(m_FieldInfo.Name);
            }
            else
            {
                resolver.CreateVisualElement();
                resolver.SetLabel(m_FieldInfo.Name);
                resolver.SetInitValue(m_SharedVariable.GetValue());
                resolver.RegisterValueCallback((obj) =>
                {
                    Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Modify Inspector Variable");
                    m_SharedVariable.SetValue(obj);
                    EditorUtility.SetDirty(m_TreeAsset);
                });
            }

            var element = resolver.GetVisualElement();
            m_VariableContainer.Add(element);
        }

        private void SetSharedVariable()
        {
            m_VariableContainer.Clear();
            
            var nameList = m_TreeAsset.variables
                .Where(x => x.GetType() == m_SharedVariable.GetType())
                .Select(v => v.Name)
                .ToList();

            var index = nameList.IndexOf(m_SharedVariable.Name);
            m_NameDropdown = new DropdownField(m_FieldInfo.Name, nameList, index);
            m_NameDropdown.labelElement.style.color = Color.black;
            m_NameDropdown.RegisterValueChangedCallback(OnDropdownChanged);
            
            m_VariableContainer.Add(m_NameDropdown);
        }

        private void OnDropdownChanged(ChangeEvent<string> evt)
        {
            var newName = evt.newValue;
            Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Modify Inspector Variable Name");
            m_SharedVariable.Name = newName;
            EditorUtility.SetDirty(m_TreeAsset);
        }
    }
}