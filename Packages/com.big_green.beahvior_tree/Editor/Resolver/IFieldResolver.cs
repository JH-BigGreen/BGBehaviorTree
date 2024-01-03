//----------------------------------------
//author: BigGreen
//date: 2023-12-25 16:28
//----------------------------------------

using System;
using UnityEngine.UIElements;

namespace BG.BTEditor
{
    public interface IFieldResolver
    {
        public void CreateVisualElement();

        public VisualElement GetVisualElement();

        public VisualElement NewVisualElement();

        public void SetLabel(string label);

        public void SetInitValue(object value);
        
        public void RegisterValueCallback(Action<object> callback);
    }
}