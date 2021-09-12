using System.Collections.Generic;
using UnityEngine;

namespace KamranWali.SimpleInterface.Editor.Layouts
{
    public class LayoutManager
    {
        private List<BaseLayout> _data;
        private int _pointerOnGUI;
        private int _pointerUpdate;

        public LayoutManager()
        {
            _data = new List<BaseLayout>();
            _pointerOnGUI = 0;
            _pointerUpdate = 0;
        }

        /// <summary>
        /// This method adds a layout to be used and updated.
        /// </summary>
        /// <param name="layout">The layout to add, of type BaseLayout</param>
        public void AddLayout(BaseLayout layout) => _data.Add(layout);

        /// <summary>
        /// This method calls all the layout OnGUIs
        /// </summary>
        public void OnGUI() { for (_pointerOnGUI = 0; _pointerOnGUI < _data.Count; _pointerOnGUI++) _data[_pointerOnGUI].SetupOnGUI(); } // Loop for setting up all the layout GUIs

        /// <summary>
        /// This method updates the layouts.
        /// </summary>
        /// <param name="currentEvent">The current event, of type Event</param>
        public void Update(Event currentEvent) { for (_pointerUpdate = 0; _pointerUpdate < _data.Count; _pointerUpdate++) _data[_pointerUpdate].Update(currentEvent); }
    }
}