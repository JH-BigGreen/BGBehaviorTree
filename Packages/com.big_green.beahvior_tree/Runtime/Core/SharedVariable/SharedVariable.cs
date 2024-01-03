//----------------------------------------
//author: BigGreen
//date: 2023-10-21 19:57
//----------------------------------------

using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BG.BT
{
    [Serializable]
    public abstract class SharedVariable
    {
        [SerializeField]
        private string m_Name;
        [SerializeField]
        private bool m_Shared;
        
        public string Name
        {
            get => m_Name;
            set => m_Name = value;
        }

        public bool Shared
        {
            get => m_Shared;
            set => m_Shared = value;
        }

        public abstract object GetValue();

        public abstract void SetValue(object value);

        public abstract void Bind(object other);
    }

    [Serializable]
    public class SharedVariable<T> : SharedVariable
    {
       
        [SerializeField]
        protected T m_Value;
        
        private Func<T> Getter;
        private Action<T> Setter;
        
        public T Value
        {
            get
            {
                if (Getter == null)
                {
                    return m_Value;
                }

                return Getter();
            }
            set
            {
                if (Setter == null)
                {
                    m_Value = value;
                }
                else
                {
                    Setter(value);
                }
            }
        }

        public override object GetValue()
        {
            return Value;
        }

        public override void SetValue(object value)
        {
            Value = (T) value;
        }

        public override void Bind(object source)
        {
            var other = source as SharedVariable<T>;
            Assert.IsNotNull(other, "other must be sharedVariable<> !!!");
            Getter = () => other.Value;
            Setter = (value) => other.Value = value;
        }
    }
}