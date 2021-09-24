using UnityEngine;
using System.Collections;


public class LevelManager : MonoBehaviour
{
    #region Variables
    public CameraManager cameraManager;
    public GameObject[] levelList;
    #endregion

    #region Methods
    private void Start()
    {
        int initialLevel = 0;

        if (PlayerPrefs.HasKey("activeLevel"))
        {
            initialLevel = PlayerPrefs.GetInt("activeLevel");
            cameraManager.SetCamera(levelList[initialLevel].transform); // move camera to level pos
        }

        LoadLevel(initialLevel);
    }

    public void LoadLevel(int nextLevel)
    {
        // loads level
        levelList[nextLevel].SetActive(true);
    }

    public void UnloadLevel()
    {
        // unloads previous level
        // esto funciona mas generico, si una puerta secreta
        // te hace avanzar al final del juego podrias usarlo
        // a diferencia de como lo teniamos antes que
        // solo descargaba el activeLevel - 1
        int previousLevel = PlayerPrefs.GetInt("activeLevel");
        StartCoroutine(UnloadingLevel(previousLevel));
    }

    IEnumerator UnloadingLevel(int previousLevel)
    {
        // waits a while before unloading previous level
        yield return new WaitForSeconds(3f);
        levelList[previousLevel].SetActive(false);
    }
    #endregion
}
