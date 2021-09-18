using System;
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
        private int _index2;
        private string _tempName;
        private string _tempPath;

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
            AddPrefabPath(path);
            for (_index = 0; _index < _pathsAll.Length; _index++) AddPrefabPath(_pathsAll[_index]); // Loop for finding the prefab paths
            if (_paths.Count > 1) SortPath(true);
            for (_index = 0; _index < _pathNames.Count; _index++) _pathNames[_index] = _pathNames[_index].Insert(0, $"{(_index + 1).ToString()}. "); // Loop for adding prefix to path names
        }

        /// <summary>
        /// This method sorts the path with the path name.
        /// </summary>
        /// <param name="isAscending">Flag to sort in ascending order, false means descending order, of type bool</param>
        private void SortPath(bool isAscending)
        {
            for(_index = 0; _index < _pathNames.Count; _index++) // Loop for going through all elements
            {
                for(_index2 = _index + 1; _index2 < _pathNames.Count; _index2++) // Loop for going through all unsorted element
                {
                    // Condition to check if ascending order or descending order
                    if(isAscending ? _pathNames[_index].CompareTo(_pathNames[_index2]) > 0 : _pathNames[_index].CompareTo(_pathNames[_index2]) < 0)
                    {
                        // Sort Swapping
                        _tempName = _pathNames[_index];
                        _tempPath = _paths[_index];
                        _pathNames[_index] = _pathNames[_index2];
                        _paths[_index] = _paths[_index2];
                        _pathNames[_index2] = _tempName;
                        _paths[_index2] = _tempPath;
                    }
                }
            }
        }

        /// <summary>
        /// This method adds a prefab path to the list if it contains prefabs.
        /// </summary>
        /// <param name="path">The path to add if it contains prefabs, of type string</param>
        private void AddPrefabPath(string path)
        {
            path = path.Replace("\\", "/"); // Fixing path location
            _pathObjects = Directory.GetFiles(path, "*.prefab"); // Adding all prefab in the folder

            if (_pathObjects.Length != 0) // Condition to add path and path name
            {
                _paths.Add(path); // Adding path
                _pathNames.Add(path.Substring(path.LastIndexOf("/") + 1)); // Adding name
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