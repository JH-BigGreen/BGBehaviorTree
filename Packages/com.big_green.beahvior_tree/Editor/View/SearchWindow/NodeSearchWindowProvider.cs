//----------------------------------------
//author: BigGreen
//date: 2023-12-19 15:47
//----------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using BG.BT;
using UnityEngine.UIElements;

namespace BG.BTEditor
{
    public class NodeSearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private TreeView m_TreeView;
        
        public void Init(TreeView treeView)
        {
            m_TreeView = treeView;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0)
            };

            Type[] ts = new Type[] {typeof(Leaf), typeof(Composite), typeof(Condition), typeof(Decorator)};
            foreach (var t in ts)
            {
                entries.Add(new SearchTreeGroupEntry(new GUIContent(t.Name), 1));

                var assembly = t.Assembly;
                var subClassList = ReflectionUtility.GetSubClassList(assembly, t);
                var dic = new Dictionary<string, List<Type>>();
                foreach (var subType in subClassList)
                {
                    var attr = subType.GetCustomAttribute<BTGroupAttribute>();
                    if (attr == null)
                    {
                        Utility.DicListAddValue(dic, "", subType);
                    }
                    else
                    {
                        Utility.DicListAddValue(dic, attr.group, subType);
                    }
                }

                var groupList = dic.Keys.ToList();
                groupList.Sort();
                foreach (var group in groupList)
                {
                    var subTypeList = dic[group];
                    if (!string.IsNullOrEmpty(group))
                    {
                        entries.Add(new SearchTreeGroupEntry(new GUIContent(group), 2));
                        subTypeList.Sort((a, b) => { return String.CompareOrdinal(a.Name, b.Name);});
                        foreach (var subType in subTypeList)
                        {
                            entries.Add(new SearchTreeEntry(new GUIContent($"{group} : {subType.Name}")){level = 3, userData = subType});
                        }
                    }
                    else
                    {
                        foreach (var subType in subTypeList)
                        {
                            entries.Add(new SearchTreeEntry(new GUIContent(subType.Name)){level = 2, userData = subType});
                        }
                    }
                }
            }

            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {   
            var worldMousePosition = m_TreeView.treeWindow.rootVisualElement.ChangeCoordinatesTo(m_TreeView.treeWindow.rootVisualElement, context.screenMousePosition - m_TreeView.treeWindow.position.position);
            var localMousePosition = m_TreeView.contentViewContainer.WorldToLocal(worldMousePosition);
                                
            var userData = SearchTreeEntry.userData;
            if (((Type)userData).IsSubclassOf(typeof(BTNode)))
            { 
                var btNode = (BTNode)Activator.CreateInstance((Type) userData);
                m_TreeView.CreateNode(btNode, localMousePosition);
            }
          
            return true;
        }
    }
}