using System.Collections;

using UnityEngine;

namespace com.DU.CE.LVL
{
    public class LVL_BallSpawner : LVL_ASpawner
    {
        [SerializeField] private GameObject m_rugbyBallPrefab;
        public Transform spawnPoint;
        private Transform m_rugbyBall = null;
        public bool m_isSpawned;

        protected override IEnumerator OnCoachSetup()
        {
            Initialize();

            yield return null;
        }

        protected override IEnumerator OnPlayerSetup()
        {
            m_rugbyBall = null;

            yield return null;

            gameObject.SetActive(false);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        public void Initialize()
        {
            if (m_isSpawned)
                return;

            m_rugbyBall = p_NetSpawnerSock.InstantiateNetObject(m_rugbyBallPrefab.name, true, false).transform;
            m_rugbyBall.transform.position = spawnPoint.position;
            //m_rugbyBall.gameObject.SetActive(false);
            m_isSpawned = true;
        }
    }
}