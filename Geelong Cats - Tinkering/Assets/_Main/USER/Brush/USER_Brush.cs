using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

using com.DU.CE.NET.NCM;


namespace com.DU.CE.USER
{
    public class USER_Brush : MonoBehaviour
    {
        [SerializeField] private SOC_NetSpawner m_NetSpawnerSock = null;
        [Space]
        [SerializeField] private EBRUSH BrushType;
        [SerializeField] private GameObject _brushStrokePrefab = null;
        [SerializeField] private InputActionProperty m_DrawInput;
        [SerializeField] private Transform m_brushTip = null;

        private bool m_isBrushDown = false;


        // Used to keep track of the current brush tip position and the actively drawing brush stroke

        private NET_BrushStroke m_activeBrushStroke;

        private void OnEnable()
        {
            m_DrawInput.action.performed += OnBrushTipDown;
            m_DrawInput.action.canceled += OnBrushTipUp;
        }

        private void OnDisable()
        {
            m_DrawInput.action.performed -= OnBrushTipDown;
            m_DrawInput.action.canceled -= OnBrushTipUp;
        }


        private void OnBrushTipDown(InputAction.CallbackContext obj)
        {
            Debug.Log("#INT_Pen--------------------------PenDown");

            m_isBrushDown = true;

            // If we haven't created a new brush stroke to draw, create one!
            if (m_activeBrushStroke == null)
            {
                // Instantiate a copy of the Brush Stroke prefab.
                GameObject brushStrokeGameObject = m_NetSpawnerSock.InstantiateNetObject(_brushStrokePrefab.name, true, true);

                // Grab the BrushStroke component from it
                m_activeBrushStroke = brushStrokeGameObject.GetComponent<NET_BrushStroke>();

                // Tell the BrushStroke to begin drawing at the current brush position
                m_activeBrushStroke.BeginBrushStrokeWithBrushTipPoint(m_brushTip.position, m_brushTip.rotation);
            }

            StartCoroutine(Draw());
        }


        private void OnBrushTipUp(InputAction.CallbackContext obj)
        {
            Debug.Log("#INT_Pen--------------------------PenUp");

            m_isBrushDown = false;
            StopCoroutine(Draw());

            // If the button is no longer pressed, and we still have an active brush stroke, mark it as finished and clear it.
            if (m_activeBrushStroke != null)
            {
                m_activeBrushStroke.EndBrushStrokeWithBrushTipPoint(m_brushTip.position, m_brushTip.rotation);
                m_activeBrushStroke = null;
            }
        }


        IEnumerator Draw()
        {
            while (m_isBrushDown)
            {
                // Move the brush stroke to the new brush tip position
                m_activeBrushStroke.MoveBrushTipToPoint(m_brushTip.position, m_brushTip.rotation);

                yield return new WaitForEndOfFrame();
            }

            yield return null;
        }
    }
}