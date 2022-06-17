using UnityEngine;

namespace com.DU.CE.UI
{
    public class UI_InteractableOptions : MonoBehaviour
    {
        [SerializeField] private Canvas m_Canvas = null;
        [SerializeField] private MeshRenderer m_PinMesh = null;

        public void ToggleUI(bool _toggle)
        {
            m_Canvas.enabled = _toggle;
            m_PinMesh.enabled = _toggle;
        }

        internal void PlaceUI(Vector3 _positionToPlace)
        {
            transform.position = _positionToPlace;
            ToggleUI(true);
        }

        internal void InitializeUI(Camera _eyeCam)
        {
            m_Canvas.worldCamera = _eyeCam;
        }
    }
}