//----------------------------------------
//author: BigGreen
//date: 2023-12-19 16:18
//----------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BG.BTEditor
{
    public static class Utility
    {
        public static void DicListAddValue<T1, T2>(Dictionary<T1, List<T2>> dic, T1 key, T2 value)
        {
            if (dic.ContainsKey(key))
            {
                var list = dic[key];
                list.Add(value);
            }
            else
            {
                var list = new List<T2>();
                list.Add(value);
                dic.Add(key, list);
            }
        }

        public static T DeepCopy<T>(T source)
        {
            object copy;
            using (MemoryStream ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, source);
                ms.Seek(0, SeekOrigin.Begin);
                copy = bf.Deserialize(ms);
            }

            return (T) copy;
        }
    }
}