using System.Collections.Generic;
using UnityEngine;

using com.DU.CE.AI;
using System;

namespace com.DU.CE.LVL
{
    [RequireComponent(typeof(LVL_AISpawner))]
    public class LVL_AIActivator : MonoBehaviour
    {
        public const int NoOfAIPerTeam = 8;

        [SerializeField] private SOC_AI m_aiSock = null;

        private LVL_AISpawner m_aiSpawnner = null;

        private int m_currentHomeIndex = 0;
        private int m_currentAwayIndex = 0;

        //private List<Transform> m_activatedHomeTeamList = new List<Transform>();
        //private List<Transform> m_activatedAwayTeamList = new List<Transform>();

        private void Awake()
        {
            m_aiSpawnner = GetComponent<LVL_AISpawner>();
        }

        private void OnEnable()
        {
            m_aiSock.OnActivateAI += ActivateAI;
            m_aiSock.OnActivateAIByNum += ActiveteByNum;
            m_aiSock.OnDisableAIByNum += DisableByNum;
            m_aiSock.OnTeamSizeChanged += ChangeSize;
        }



        private void OnDisable()
        {
            m_aiSock.OnActivateAI -= ActivateAI;
            m_aiSock.OnActivateAIByNum -= ActiveteByNum;
            m_aiSock.OnDisableAIByNum -= DisableByNum;
            m_aiSock.OnTeamSizeChanged -= ChangeSize;
        }
        private void ChangeSize(ETEAM team, int size)
        {
            if (team == ETEAM.HOME)
            {
                m_currentHomeIndex = size;
            }
            else
            {
                m_currentAwayIndex = size;
            }
        }

        private void DisableByNum(ETEAM team, int num)
        {
            AI_Avatar ai;
            if (team.Equals(ETEAM.HOME))
            {
                ai = m_aiSpawnner.HomePool[num].GetComponent<AI_Avatar>();
            }
            else
            {
                ai = m_aiSpawnner.AwayPool[num].GetComponent<AI_Avatar>();
            }

            ai.ChangeNetworkActivation(false);
        }

        internal void SetupForPlayer()
        {
            AI_Avatar[] ais = FindObjectsOfType<AI_Avatar>();

            m_aiSock.ais = ais;

            for (int i = 0; i < ais.Length; i++)
            {
                ais[i].transform.parent = this.transform;
                m_aiSock.NoOfAI++;
            }

            enabled = false;
        }
        private void ActiveteByNum(ETEAM team, int num, Transform location)
        {
            AI_Avatar ai;
            if (team.Equals(ETEAM.HOME))
            {
                ai = m_aiSpawnner.HomePool[num].GetComponent<AI_Avatar>();
            }
            else
            {
                ai = m_aiSpawnner.AwayPool[num].GetComponent<AI_Avatar>();
            }

            ai.ChangeNetworkActivation(true);

            ai.transform.position = location.position;
            ai.transform.rotation = location.rotation;

        }


        private void ActivateAI(ETEAM team, Transform location)
        {
            int currentIndex;
            AI_Avatar ai;

            if (team.Equals(ETEAM.HOME))
            {
                currentIndex = m_currentHomeIndex;

                if (currentIndex > NoOfAIPerTeam - 1)
                    return;

                m_currentHomeIndex++;

                ai = m_aiSpawnner.HomePool[currentIndex].GetComponent<AI_Avatar>();
            }
            else
            {
                currentIndex = m_currentAwayIndex;

                if (currentIndex > NoOfAIPerTeam - 1)
                    return;

                m_currentAwayIndex++;

                ai = m_aiSpawnner.AwayPool[currentIndex].GetComponent<AI_Avatar>();
            }

            ai.ChangeNetworkActivation(true);

            ai.transform.position = location.position;
            ai.transform.rotation = location.rotation;

            //AddToActivatedTeamList(team, ai.transform);
        }

        //public void AddToActivatedTeamList(ETEAM team, Transform ObjectToAdd)
        //{
        //    if (team == ETEAM.HOME)
        //        m_activatedHomeTeamList.Add(ObjectToAdd);
        //    else
        //        m_activatedAwayTeamList.Add(ObjectToAdd);
        //}
    }
}