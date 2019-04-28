using UnityEngine;

namespace Mortal
{
    public class SelectableCell : MonoBehaviour
    {
        public GameObject activateOnActive;
        public bool isSelected = false;

        private void OnEnable()
        {
            ToggleAttachedGO();
        }

        void ToggleAttachedGO()
        {
            activateOnActive?.SetActive(isSelected);
        }

        private void OnMouseDown()
        {
            isSelected = !isSelected;
            ToggleAttachedGO();
        }
    }
}
