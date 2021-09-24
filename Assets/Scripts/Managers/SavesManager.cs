using UnityEngine;

public class SavesManager : MonoBehaviour
{
    public void SetLevel(int activeLevel)
    {
        // saves level to prefs
        PlayerPrefs.SetInt("activeLevel", activeLevel);
        Debug.Log("El nivel activo es: " + activeLevel);
    }

    public void SetCheckpoint(Transform pos)
    {
        // if player has this prefs and is different from current checkpoint
        // it saves pos as a new checkpoint
        // if player hasnt (esta bien asi? lol) prefs it saves pos as new checkpoint
        if (PlayerPrefs.HasKey("checkpointX") && PlayerPrefs.HasKey("checkpointY"))
        {
            Vector3 checkpoint = new Vector3(PlayerPrefs.GetFloat("checkpointX"), PlayerPrefs.GetFloat("checkpointY"), 0);

            // checks if it is a new checkpoint
            if (checkpoint != pos.position)
            {
                SetCoordinates(pos.position);
            }
        }
        else
        {
            SetCoordinates(pos.position);
        }
    }

    public void SetScene(string activeScene)
    {
        DeleteSaves(); // deletes saves for next scene
        PlayerPrefs.SetString("scene", activeScene);
    }

    public void DeleteSaves()
    {
        // deletes prefs
        PlayerPrefs.DeleteKey("activeLevel");
        PlayerPrefs.DeleteKey("scene");
        PlayerPrefs.DeleteKey("checkpointX");
        PlayerPrefs.DeleteKey("checkpointY");
        Debug.Log("PlayerPrefs deleted");
    }

    private void SetCoordinates(Vector3 checkpoint)
    {
        // this actually saves the prefs (checkpoint)
        PlayerPrefs.SetFloat("checkpointX", checkpoint.x);
        PlayerPrefs.SetFloat("checkpointY", checkpoint.y);
        PlayerPrefs.Save();
        Debug.Log("New checkpoint in: " + checkpoint);
    }
}
