using System;
using UnityEngine;

/// <summary>
/// Interface class for the board pins which contain their necessary 
/// methods
/// </summary>
namespace com.DU.CE.INT
{
    public interface INT_IBaseBoardPin
    {
        void SwitchPin(bool val);

        // Methods to used when Pin is held by the XRController
        void UpdatePosWhenHeld();
        void UpdateRotWhenHeld();
    }


    public interface INT_IBoardLinkedPin : INT_IBaseBoardPin
    {
        void LinkObject(Transform linkedObject);
        void SetupPin(ETEAM team, int playerNo);

        void SetObjectStatus(bool activatedStatus);
        void UpdatePinPosition();
    }


    public interface INT_ILinkedPinObject
    {
        void LinkPin(INT_IBoardLinkedPin pin);

        Transform GetRelativeTransform();

        void SetNavAgentDestination(Vector3 _destinationInXY);
        void SetRelativeYRotation(float rotY);
        float GetRelativeYRotation();
    }
}