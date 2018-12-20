using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuHandler : MonoBehaviour
    {
        public void LoadScene(int menuID)
        {
            SceneManager.LoadScene(menuID);
        }
        public void Quitgame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    } 
}
