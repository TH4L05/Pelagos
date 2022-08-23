using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FMOD;
using FMODUnity;

namespace PelagosProject.UI
{
    public class ButtonAudio : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
    {
        #region Fields

        [SerializeField] private EventReference audioClickEvent;
        [SerializeField] private EventReference audioSelectEvent;
        [SerializeField] private EventReference audioDeselectEvent;
       
        #endregion

        #region Handle

        public void OnDeselect(BaseEventData eventData)
        {
            PlayAudioDeselect();
        }

        public  void OnSelect(BaseEventData eventData)
        {
            PlayAudioSelect();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            PlayAudioClick();
        }

        #endregion;

        #region PlayAudio

        public void PlayAudioClick()
        {
            if (!audioClickEvent.IsNull) RuntimeManager.PlayOneShot(audioClickEvent);
        }

        public void PlayAudioSelect()
        {
            if (!audioSelectEvent.IsNull) RuntimeManager.PlayOneShot(audioSelectEvent);
        }

        public void PlayAudioDeselect()
        {
            if (!audioDeselectEvent.IsNull) RuntimeManager.PlayOneShot(audioDeselectEvent);
        }

        #endregion
    }
}

