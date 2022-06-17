using System.Collections;
using UnityEngine;

namespace com.DU.CE.LVL
{
    public class LVL_FieldLinesSpawner : LVL_ASpawner
    {
        [SerializeField] private GameObject m_StadiumPropertiesPrefab = null;
        [SerializeField] private GameObject m_FieldLinesPrefab = null;

        protected override IEnumerator OnCoachSetup()
        {
            p_NetSpawnerSock.InstantiateNetObject(m_StadiumPropertiesPrefab.name, true, true);

            InstantiateFieldLines();

            yield return null;
        }

        protected override IEnumerator OnPlayerSetup()
        {
            InstantiateFieldLines();

            yield return null;
        }

        private void InstantiateFieldLines()
        {
            Instantiate(m_FieldLinesPrefab,
                        Vector3.zero,
                        m_FieldLinesPrefab.transform.rotation,
                        this.transform);
        } 
    }
}