using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Custom
{
    public class ApplyTankPrefabs : MonoBehaviour
    {
        public Tank player1, player2;
        public Transform gun;
        public int[] partIndexPlayer1 = new int[3];
        public int[] partIndexPlayer2 = new int[3];



        private void Awake()
        {
            // get data from the GameObject that we created in Customisation (DontDestroyOnLoad())
            if(GameObject.Find("ButtonHandler") != null)
            {
                CreateCharacters handler = GameObject.Find("ButtonHandler").GetComponent<CreateCharacters>();
                partIndexPlayer1 = handler.partIndexPlayer1;
                partIndexPlayer2 = handler.partIndexPlayer2;
            }
        }

        void Start()
        {
            
            gun = Instantiate(Resources.Load("Tank_Parts/Body_" + partIndexPlayer1[0]) as GameObject, player1.transform).transform;
            gun = gun.GetChild(0);
            player1.gun = gun;
            Instantiate(Resources.Load("Tank_Parts/Turret_" + partIndexPlayer1[1]) as GameObject, gun);
            player1.bulletSpawnPoint = gun.GetChild(0).GetChild(0).transform;
            Instantiate(Resources.Load("Tank_Parts/Track_" + partIndexPlayer1[2]) as GameObject, player1.transform);
           

            gun = Instantiate(Resources.Load("Tank_Parts/Body_" + partIndexPlayer2[0]) as GameObject, player2.transform).transform;
            gun = gun.GetChild(0);
            player2.gun = gun;
            Instantiate(Resources.Load("Tank_Parts/Turret_" + partIndexPlayer2[1]) as GameObject, gun);
            player2.bulletSpawnPoint = gun.GetChild(0).GetChild(0).transform;
            Instantiate(Resources.Load("Tank_Parts/Track_" + partIndexPlayer2[2]) as GameObject, player2.transform);
            
        }
    } 
}
