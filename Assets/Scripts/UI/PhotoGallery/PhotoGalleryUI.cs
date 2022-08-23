/// <author>Thoams Krahl</author>

using System;
using UnityEngine;

namespace PelagosProject.UI
{
    public class PhotoGalleryUI : MonoBehaviour
    {
        public static Action<bool> PhotoGalleryUIStatusChanged;

        private void OnEnable()
        {
            PhotoGalleryUIStatusChanged?.Invoke(true);
        }

        private void OnDisable()
        {
            PhotoGalleryUIStatusChanged?.Invoke(false);
        }
    }
}
