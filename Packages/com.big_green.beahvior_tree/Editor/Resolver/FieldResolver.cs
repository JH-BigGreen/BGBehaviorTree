//----------------------------------------
//author: BigGreen
//date: 2023-12-25 14:19
//----------------------------------------

using System;
using System.Reflection;
using BG.BT;
using UnityEngine;
using UnityEngine.UIElements;

namespace BG.BTEditor
{
    public abstract class FieldResolver<T, K> : IFieldResolver where T : BaseField<K> 
    {
        protected BehaviorTreeAsset m_TreeAsset;
        protected Type m_FieldType;
        protected MemberInfo m_MemberInfo;
        protected string m_NodeGuid;
        
        protected T m_EditorField;

        public FieldResolver(BehaviorTreeAsset asset, Type fieldType, MemberInfo info)
        {
            m_TreeAsset = asset;
            m_FieldType = fieldType;
            m_MemberInfo = info;
        }

        public abstract T CreateEditorField();
        
        public virtual T GetEditorField()
        {
            return m_EditorField;
        }

        public void CreateVisualElement()
        {
            m_EditorField = CreateEditorField();
        }
        
        public VisualElement GetVisualElement()
        {
            return m_EditorField;
        }

        public VisualElement NewVisualElement()
        {
            return CreateEditorField();
        }

        public virtual void SetLabel(string label)
        {
            m_EditorField.label = label;
            m_EditorField.labelElement.style.color = Color.black;
        }

        public virtual void SetInitValue(object value)
        {
            m_EditorField.value = (K)value;
        }

        public virtual void RegisterValueCallback(Action<object> callback)
        {
            m_EditorField.RegisterValueChangedCallback((evt) =>
            {
                callback?.Invoke(evt.newValue);
            });
        }
    }
}