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
        void SetPinColour(Color32 color);
        void setBasePinColour(Color32 color);

        void SetPinRing();
        void UnsetPinRing();
        void SetObjectStatus(bool activatedStatus);
        void UpdatePinPosition();
        void UpdateForLoad(Vector3 _destinationpPosition, Vector3 destinationRotation);
    }


    public interface INT_ILinkedPinObject
    {
        void LinkPin(INT_IBoardLinkedPin pin);

        Transform GetRelativeTransform();

        void SetScenarioTransform(Vector3 _initPosi, Vector3 _initRot, Vector3 _finalPosi, Vector3 _finalRot);
        void SetInitPosition();
        void SetNavAgentDestination(Vector3 _destinationInXY);
        void SetRelativeYRotation(float rotY);
        float GetRelativeYRotation();
        void SetBallReceiver(bool status);
        void SetHighlight();
        void ResetHighlight();

        void SetFinalPosition();
        void SetPlayerReference(bool status);
    }
}