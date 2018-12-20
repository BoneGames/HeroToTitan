using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        public static GameManager Instance = null;

        private void Awake()
        {
            Instance = this;
        }
        private void OnDestroy()
        {
            Instance = null;
        }
        #endregion

        public List<Tank> tanks;        // List of all tanks in the game
        public int currentTank;     // Index to current tank that's playing
        void Start()
        {
            // Find all the tanks in the game
            tanks = new List<Tank>(FindObjectsOfType<Tank>());
            // Apply the first tank
            SetTank(currentTank);
        }

        void RemoveTank(Tank tankToRemove)
        {
            // remove the tank from the list
            tanks.Remove(tankToRemove);
            // Update the current tank
            SetTank(currentTank);
        }

        // Apply the current tank
        void SetTank(int current)
        {
            // loop through all tanks
            for (int i = 0; i < tanks.Count; i++)
            {
                Tank tank = tanks[i]; // Get tank at Index i
                tank.IsPlaying = false; // Set the tank inputs to false
                if (i == current)
                {
                    // This tank is playing now
                    tank.IsPlaying = true;
                }
            }
        }
        // Select the next tank
        public void NextTank()
        {
            // Increment currenTank
            currentTank++;
            // If currentTank is outside array
            if (currentTank >= tanks.Count)
            {
                // Reset currentTank
                currentTank = 0;
            }
            // Apply the current tank selection
            SetTank(currentTank);
        }
    }

}