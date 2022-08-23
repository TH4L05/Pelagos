/// <author>Thoams Krahl</author>

using UnityEngine;

namespace PelagosProject.UI.Menu.Ingame
{
    public class ScannableLibaryUIScrollArea : MonoBehaviour
    {
        private int scrollIndex = 0;
        [SerializeField] private GameObject[] segments;


        private void OnEnable()
        {
            if (segments.Length != 0) ActivateSection(0);
        }

        private void OnDisable()
        {
            DisableAllSections();
        }

        public void NextSection()
        {
            if (segments.Length == 0) return;

            scrollIndex++;
            if (scrollIndex > segments.Length -1)
            {
                scrollIndex = segments.Length -1;
                return;
            }
            DisableAllSections();
            ActivateSection(scrollIndex);
            
        }

        public void PreviousSection()
        {
            if (segments.Length == 0) return;

            scrollIndex--;
            if (scrollIndex < 0)
            {
                scrollIndex = 0;
                return;
            }
            DisableAllSections();
            ActivateSection(scrollIndex);          
        }

        private void ActivateSection(int index)
        {
            segments[index].SetActive(true);
        }

        private void DisableAllSections()
        {
            if (segments.Length == 0) return;
            foreach (var section in segments)
            {
                section.SetActive(false);
            }
        }
    }
}

