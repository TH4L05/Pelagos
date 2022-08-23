/// <author>Thoams Krahl</author>

using UnityEngine;

public class OpenUrl : MonoBehaviour
{
    [SerializeField] private bool isEnabled;
    [SerializeField] private string url;

    public void OpenAnUrl()
    {
        if (!isEnabled) return;
        Application.OpenURL(url);
    }
}
