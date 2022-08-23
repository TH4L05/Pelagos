using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace PelagosProject.UI.Menu
{
    public class ButtonEvents : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public UnityEvent onSelect;
        public UnityEvent onDeselect;

        public void OnDeselect(BaseEventData eventData)
        {
            onDeselect?.Invoke();
        }

        public void OnSelect(BaseEventData eventData)
        {  
            onSelect?.Invoke();
        }
    }
}


