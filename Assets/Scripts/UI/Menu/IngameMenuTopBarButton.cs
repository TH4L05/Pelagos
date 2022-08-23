/// <author>Thoams Krahl</author>

using UnityEngine;
using UnityEngine.UI;

namespace PelagosProject.UI.Menu.Ingame
{
    public class IngameMenuTopBarButton : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite selectedSprite;

        public void ActivatePanel()
        {
            if (panel) panel.SetActive(true);
            if (buttonImage && selectedSprite) buttonImage.sprite = selectedSprite;
            transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }

        public void DisablePanel()
        {
            if (panel) panel.SetActive(false);
            if (buttonImage && normalSprite) buttonImage.sprite = normalSprite;
            transform.localScale = Vector3.one;
        }
    }
}

