using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace KamranWali.SimpleInterface.Editor.Layouts
{
    public class PrefabPathSearch
    {
        private string[] _pathsAll;
        private List<string> _paths;
        private List<string> _pathNames;
        private string[] _pathObjects;
        private int _index;

        public PrefabPathSearch()
        {
            _paths = new List<string>();
            _pathNames = new List<string>();
        }

        /// <summary>
        /// This method searches for all paths that contains at least one prefab.
        /// </summary>
        /// <param name="path">The starting path to search from, of type string</param>
        public void SearchAllPrefabPaths(string path)
        {
            if (!Directory.Exists(path)) return;
            
            _paths.Clear();
            _pathNames.Clear();
            _pathsAll = Directory.GetDirectories(path, ".", SearchOption.AllDirectories);

            for (_index = 0; _index < _pathsAll.Length; _index++) // Loop for finding the prefab paths
            {
                _pathsAll[_index] = _pathsAll[_index].Replace("\\", "/"); // Fixing path location
                _pathObjects = Directory.GetFiles(_pathsAll[_index], "*.prefab"); // Adding all prefab in the folder

                if (_pathObjects.Length != 0) // Condition to add path and path name
                {
                    _paths.Add(_pathsAll[_index]); // Adding path
                    _pathNames.Add(_pathsAll[_index].Substring(_pathsAll[_index].LastIndexOf("/") + 1)); // Adding name
                }
            }
        }

        /// <summary>
        /// This method gets the prefab paths.
        /// </summary>
        /// <returns>The prefab paths, of type string[]</returns>
        public string[] GetPrefabPaths() => _paths.ToArray();

        /// <summary>
        /// This method gets the path names.
        /// </summary>
        /// <returns>The path names, of type string[]</returns>
        public string[] GetPathNames() => _pathNames.ToArray();
    }
}