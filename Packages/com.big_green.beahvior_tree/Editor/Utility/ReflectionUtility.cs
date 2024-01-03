//----------------------------------------
//author: BigGreen
//date: 2023-12-19 16:09
//----------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

namespace BG.BTEditor
{
    public static class ReflectionUtility
    {
        public static List<Type> GetSubClassList(Assembly assembly, Type baseType, bool sort = true)
        {
            var list = new List<Type>();
            var allType = assembly.GetTypes();
            foreach (var t in allType)
            {
                if (t.IsSubclassOf(baseType))
                {
                    list.Add(t);
                }
            }

            if (sort)
            {
                list.Sort((a, b) => { return String.CompareOrdinal(a.Name, b.Name);});
            }
            
            return list;
        }

        public static List<Type> GetTypesByAttribute<T>(Assembly assembly, bool sort = true) where T : Attribute
        {
            var list = new List<Type>();
            var allType = assembly.GetTypes();
            foreach (var t in allType)
            {
                if (t.GetCustomAttribute<T>() != null)
                {
                    list.Add(t);
                }
            }
            
            if (sort)
            {
                list.Sort((a, b) => { return String.CompareOrdinal(a.Name, b.Name);});
            }

            return list;
        }
    }
}