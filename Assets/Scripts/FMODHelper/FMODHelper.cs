using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODHelper : MonoBehaviour
{
    public FMOD.Studio.PARAMETER_ID GetParameterID(FMOD.Studio.EventInstance eventInstance, string parameterName)
    {
        FMOD.Studio.EventDescription eventDescription;
        eventInstance.getDescription(out eventDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION parameterDescription;
        eventDescription.getParameterDescriptionByName(parameterName, out parameterDescription);
        return parameterDescription.id;
    }

    public float GetParameterValueByID(FMOD.Studio.EventInstance eventInstance, FMOD.Studio.PARAMETER_ID parameterID)
    {
        float value = 0;
        eventInstance.getParameterByID(parameterID, out value);
        return value;
    }
}
