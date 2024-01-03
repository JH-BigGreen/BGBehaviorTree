//----------------------------------------
//author: BigGreen
//date: 2023-12-22 15:30
//----------------------------------------

using System;
using System.Collections.Generic;
using BG.BT;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Reflection;
using UnityEngine;

namespace BG.BTEditor
{
    public class BlackBoardPanel : VisualElement
    {
        private BehaviorTreeAsset m_TreeAsset;
        private ToolbarButton m_AddBtn;
        private VisualElement m_ElementContainer;

        public BlackBoardPanel(BehaviorTreeAsset asset)
        {
            m_TreeAsset = asset;

            var toolBar = new Toolbar();
            toolBar.style.justifyContent = Justify.FlexEnd;
            toolBar.style.height = new StyleLength(StyleKeyword.Auto);
            Add(toolBar);
            m_AddBtn = new ToolbarButton();
            m_AddBtn.text = "[+]";
            m_AddBtn.style.fontSize = 21;
            m_AddBtn.clicked += OnAddBtnClick;
            toolBar.Add(m_AddBtn);

            m_ElementContainer = new VisualElement();
            Add(m_ElementContainer);

            Refresh();
        }

        private void Refresh()
        {
            m_ElementContainer.Clear();
            foreach (var variable in m_TreeAsset.variables)
            {
                SetVariableElement(variable);
            }
        }

        private void SetVariableElement(SharedVariable variable)
        {
            var container = new Box();
            container.style.marginBottom = 5;
            m_ElementContainer.Add(container);

            var title = new VisualElement();
            title.style.flexDirection = FlexDirection.Row;
            container.Add(title);

            var text = new TextField("key");
            text.value = variable.Name;
            text.labelElement.style.color = Color.black;
            text.style.flexGrow = 1;

            text.RegisterValueChangedCallback((evt) =>
            {
                Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Modify Blackboard Variable Key");
                variable.Name = evt.newValue;
                EditorUtility.SetDirty(m_TreeAsset);
                ChangeAllVariableName(evt.previousValue, evt.newValue);
            });
            title.Add(text);
            var removeBtn = new Button();
            removeBtn.text = "-";
            removeBtn.clicked += () =>
            {
                var removeIndex = m_TreeAsset.variables.IndexOf(variable);
                Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Remove Blackboard Variable");
                m_TreeAsset.variables.RemoveAt(removeIndex);
                EditorUtility.SetDirty(m_TreeAsset);
                m_ElementContainer.RemoveAt(removeIndex);
            };
            title.Add(removeBtn);

            var valueInfo = variable.GetType().GetProperty("Value",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var resolver = FieldResolverFactory.Instance.GetResolver(m_TreeAsset, valueInfo.PropertyType, valueInfo);
            if (resolver is IListResolver listResolver)
            {
                var initValue = variable.GetValue();
                if (initValue == null)
                {
                    variable.SetValue(Activator.CreateInstance(valueInfo.PropertyType));
                    EditorUtility.SetDirty(m_TreeAsset);
                }

                listResolver.DataSource = variable.GetValue();
                resolver.CreateVisualElement();
                resolver.SetLabel("value");
            }
            else
            {
                resolver.CreateVisualElement();
                resolver.SetLabel("value");
                resolver.SetInitValue(variable.GetValue());
                resolver.RegisterValueCallback((obj) =>
                {
                    Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Modify Blackboard Variable");
                    valueInfo.SetValue(variable, obj);
                    EditorUtility.SetDirty(m_TreeAsset);
                });
            }

            var element = resolver.GetVisualElement();
            container.Add(element);
        }

        private void OnAddBtnClick()
        {
            var assembly = typeof(SharedVariable).Assembly;
            var types = ReflectionUtility.GetTypesByAttribute<ShareVariableMenuAttribute>(assembly);
            GenericMenu menu = new GenericMenu();
            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<ShareVariableMenuAttribute>();
                menu.AddItem(new GUIContent(attr.menuName), false, OnAddShareVariable, type);
            }

            menu.ShowAsContext();
        }

        private void OnAddShareVariable(object userData)
        {
            var type = (Type) userData;
            var variable = (SharedVariable) Activator.CreateInstance(type);
            variable.Name = SetNewVariableName(variable);
            Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Add Blackboard Variable");
            m_TreeAsset.variables.Add(variable);
            EditorUtility.SetDirty(m_TreeAsset);

            SetVariableElement(variable);
        }

        private string SetNewVariableName(SharedVariable variable)
        {
            var str = variable.GetType().Name;
            var index = 0;
            while (true)
            {
                index++;
                var same = false;
                foreach (var item in m_TreeAsset.variables)
                {
                    if (item.Name == str)
                    {
                        str = variable.GetType().Name + index;
                        same = true;
                        break;
                    }
                }

                if (!same)
                {
                    break;
                }
            }

            return str;
        }

        private void ChangeAllVariableName(string preName, string newName)
        {
            var allNodes = m_TreeAsset.GetAllNodes();
            while (allNodes.Count > 0)
            {
                var node = allNodes.Dequeue();
                var allField = node.GetType().GetFields(BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic);
                var fieldList = new List<FieldInfo>();
                foreach (var fieldInfo in allField)
                {
                    if (fieldInfo.FieldType.IsSubclassOf(typeof(SharedVariable)))
                    {
                        fieldList.Add(fieldInfo);
                    }
                }

                foreach (var fieldInfo in fieldList)
                {
                    var variable = (SharedVariable)fieldInfo.GetValue(node);
                    if (variable.Name == preName)
                    {
                        variable.Name = newName;
                        EditorUtility.SetDirty(m_TreeAsset);
                    }
                }
            }
        }
    }
}