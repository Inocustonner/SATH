using System;
using Code.Data.Enums;
using Code.Infrastructure.GameLoop;
using Core.Infrastructure.Utils;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Code.Audio.AudioEvents
{
    [Serializable]
    public class AudioParameterController
    {
        [SerializeField] private AudioParamType _paramName;
        private PARAMETER_DESCRIPTION _parameterDescription;
        private PARAMETER_ID _parameterID;


        public void InitParam()
        {
            RuntimeManager.StudioSystem.getParameterDescriptionByName(_paramName.ToString(), out _parameterDescription);
            _parameterID = _parameterDescription.id;
        }

        public void ChangeParam(float value)
        {
            RuntimeManager.StudioSystem.setParameterByID(_parameterID, value);
          RuntimeManager.StudioSystem.getParameterByID(_parameterID,out  var currentvalue);
          this.Log(currentvalue);
        }
    }
}