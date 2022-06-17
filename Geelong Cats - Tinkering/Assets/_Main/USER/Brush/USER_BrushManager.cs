using System.Collections.Generic;
using UnityEngine;

using com.DU.CE.NET.NCM;

namespace com.DU.CE.USER
{
    public class USER_BrushManager : MonoBehaviour
    {
        [SerializeField] private SOC_NetSpawner m_netSpawnerSock = null;

        private Stack<NET_BrushStroke> m_boardBrushStrokes = new Stack<NET_BrushStroke>();

        private void OnEnable()
        {
            m_netSpawnerSock.OnBrushStrokeAdded += AddBoardBrushStroke;
            m_netSpawnerSock.OnUndoBoardBrushStroke += DeletePreviousBoardBrushStroke;
            m_netSpawnerSock.OnClearBoardBrushStrokes += ClearAllBoardBrushStrokes;
        }

        private void OnDisable()
        {
            m_netSpawnerSock.OnBrushStrokeAdded -= AddBoardBrushStroke;
            m_netSpawnerSock.OnUndoBoardBrushStroke -= DeletePreviousBoardBrushStroke;
            m_netSpawnerSock.OnClearBoardBrushStrokes -= ClearAllBoardBrushStrokes;
        }


        #region ----------------------------------------------------------------- Brush Strokes 


        internal void AddBoardBrushStroke(NET_BrushStroke _brushStrokeToAdd)
        {
            m_boardBrushStrokes.Push(_brushStrokeToAdd);
            _brushStrokeToAdd.transform.parent = this.transform;
        }


        private void DeletePreviousBoardBrushStroke()
        {
            if (m_boardBrushStrokes.Count < 1)
                return;

            NET_BrushStroke toBeDeleted = m_boardBrushStrokes.Peek();

            m_boardBrushStrokes.Pop();

            m_netSpawnerSock.DestroyNetObject(toBeDeleted.gameObject);
        }


        private void ClearAllBoardBrushStrokes()
        {
            if (m_boardBrushStrokes.Count < 1)
                return;

            NET_BrushStroke[] _brushStrokesToBeDeleted = m_boardBrushStrokes.ToArray();

            m_boardBrushStrokes.Clear();

            for (int i = 0; i < _brushStrokesToBeDeleted.Length; i++)
            {
                m_netSpawnerSock.DestroyNetObject(_brushStrokesToBeDeleted[i].gameObject);
            }
        }


        private void OnDestroy()
        {
            m_boardBrushStrokes.Clear();
        }

        #endregion -----------------------------------------------------------------
    }
}