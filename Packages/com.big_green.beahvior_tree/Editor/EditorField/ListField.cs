//----------------------------------------
//author: BigGreen
//date: 2023-12-25 15:21
//----------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using BG.BT;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace BG.BTEditor
{
    public class ListField<T> : BaseField<List<T>>
    {
        private BehaviorTreeAsset m_TreeAsset;
        private IFieldResolver m_SubResolver;
        private List<T> m_DataSource;
        private VisualElement m_ElementsContainer;
        
        public ListField(string label, VisualElement visualInput, BehaviorTreeAsset asset, List<T> dataSource, IFieldResolver subResolver) : base(label, visualInput)
        {
            m_TreeAsset = asset;
            m_SubResolver = subResolver;
            m_DataSource = dataSource;
            
            style.flexDirection = FlexDirection.Column;
            m_ElementsContainer = new VisualElement();
            Add(m_ElementsContainer);
            var btnContainer = new VisualElement();
            Add(btnContainer);

            RefreshContent();
            
            var addBtn = new Button();
            addBtn.text = "+";
            addBtn.clicked += () =>
            {
                Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Add Tree List Property");
                m_DataSource.Add(default(T));
                AddElement(m_DataSource.Count - 1);
            };
            btnContainer.Add(addBtn);
        }

        private void RefreshContent()
        {
            m_ElementsContainer.Clear();
            for (int i = 0; i < m_DataSource.Count; i++)
            {
                AddElement(i);
            }
        }

        private void AddElement(int index)
        {
            var ele = new VisualElement();
            ele.style.flexDirection = FlexDirection.Row;
            var data = m_DataSource[index];
            var element = m_SubResolver.NewVisualElement();
            element.style.flexGrow = 1;
            if (typeof(T).IsSubclassOf(typeof(Object)))
            {
                var baseField = element as ObjectField;
                baseField.value =  data as Object;
                baseField.RegisterValueChangedCallback((evt) =>
                {
                    Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Modify Tree List Property");
                    m_DataSource[index] = (T)(evt.newValue as object);
                    EditorUtility.SetDirty(m_TreeAsset);
                });
            }
            else
            {
                var baseField =  element as BaseField<T>;
                baseField.value = data;
                baseField.RegisterValueChangedCallback((evt) =>
                {
                    Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Modify Tree List Property");
                    m_DataSource[index] = evt.newValue;
                    EditorUtility.SetDirty(m_TreeAsset);
                });
            }
            ele.Add(element);

            var removeBtn = new Button();
            removeBtn.text = "-";
            removeBtn.clicked += () =>
            {
                Undo.RegisterCompleteObjectUndo(m_TreeAsset, "Remove Tree List Property");
                m_DataSource.RemoveAt(index);
                RefreshContent();
            };
            ele.Add(removeBtn);
                
            m_ElementsContainer.Add(ele);
        }
    }
}