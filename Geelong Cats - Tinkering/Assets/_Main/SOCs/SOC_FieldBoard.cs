using System;

using UnityEngine;

namespace com.DU.CE.INT
{
    [CreateAssetMenu(fileName = "Sock_FieldBoard", menuName = "Socks/FieldBoard")]
    public class SOC_FieldBoard : ScriptableObject
    {
        private const int m_BOARDLAYER = 13;
        private const int m_FIELDLAYER = 9;

        public Camera EagleEyeCamera = null;

        public GameObject PosPinPrefab = null;
        public GameObject PosMarkerPrefab = null;

        [Space]
        public Material HomeTeamMaterial = null;
        public Material AwayTeamMaterial = null;

        [Space]
        public float OriginOffsetX = 0.00515f;
        public float OriginOffsetY = 0.005f;
        public float OriginOffsetZ = 0.00585f;

        [Space]
        public float MoveMultiplier = 1f;
        public float RotateMultiplier = 1f;


        internal event Action<Transform> OnAIInstantiate;
        internal void AIInstantiateCall(Transform _transform)
        {
            OnAIInstantiate?.Invoke(_transform);
        }

        /// <summary>
        /// Raycasts on to the camera render of field and gets world position of the
        /// hit point on the field.
        /// </summary>
        internal bool GetBoardToFieldPosition(Transform _raycastFrom, out Vector3 _fieldPos)
        {
            RaycastHit _texRendHPoint;

            // Ray which points from the pin's base on to the field render
            Ray _pinToRendRay = new Ray(_raycastFrom.position, -_raycastFrom.up);

            //Debug.DrawRay(_pinToRendRay.origin, _pinToRendRay.direction, Color.cyan);

            // Raycast to get the texture coordinate of the field render
            if (Physics.Raycast(_pinToRendRay, out _texRendHPoint, 1 << m_BOARDLAYER))
            {
                // Get texture coordinate
                Vector3 _renderHit = _texRendHPoint.textureCoord;

                //Debug.Log("#BoardPins#-------------------------Raycast hit board \n" + _renderHit + "\n" + _texRendHPoint.distance);


                Ray _rayFromRendCam = EagleEyeCamera.ViewportPointToRay(
                    new Vector3(_renderHit.x, _renderHit.y, 0));

                //Debug.DrawRay(_rayFromRendCam.origin, _rayFromRendCam.direction * 100, Color.cyan);

                RaycastHit _cameraHit;
                if (Physics.Raycast(_rayFromRendCam, out _cameraHit, 1 << m_FIELDLAYER))
                {
                    _fieldPos = _cameraHit.point;
                    return true;
                }
            }

            _fieldPos = Vector3.zero;
            return false;
        }


        internal event Action<Vector3, Quaternion> OnTeleportRequest;
        public void RequestTeleport(Transform _transform)
        {
            Vector3 fieldPos;
            if (!GetBoardToFieldPosition(_transform, out fieldPos))
            {
                Debug.LogError("#SOC_FieldBoard#--------------RaycastFailed");
                return;
            }

            OnTeleportRequest.Invoke(fieldPos, _transform.localRotation);
        }


        public event Action<Vector3> OnUIMarkerAdd;
        public void AddMarkerOnNetwork(Transform _pinTransform)
        {
            OnUIMarkerAdd?.Invoke(_pinTransform.position);
        }
    }
}