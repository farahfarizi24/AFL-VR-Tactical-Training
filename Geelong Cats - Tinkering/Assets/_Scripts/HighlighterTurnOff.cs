using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.DU.CE.AI
{
    public class HighlighterTurnOff : MonoBehaviour
    {
        private GameObject[] HomePlayers;
        private GameObject[] AwayPlayers;

        private AI_Avatar avatar_manager;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            findPlayers();
        }

        public void findPlayers()
        {
            HomePlayers = GameObject.FindGameObjectsWithTag("Home");
            AwayPlayers = GameObject.FindGameObjectsWithTag("Away");
            foreach (GameObject player in HomePlayers)
            {
            avatar_manager =  player.GetComponent<AI_Avatar>();
                if (avatar_manager.BallReceiver && avatar_manager.OutlineScript.enabled)
                {
                    avatar_manager.OutlineScript.enabled = false;

                }
            }

            foreach (GameObject player in AwayPlayers)
            {
                avatar_manager = player.GetComponent<AI_Avatar>();
                if (avatar_manager.BallReceiver && avatar_manager.OutlineScript.enabled)
                {
                    avatar_manager.OutlineScript.enabled = false;

                }
            }


        }
    }
}

