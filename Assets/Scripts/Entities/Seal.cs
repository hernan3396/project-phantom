using UnityEngine;

public class Seal : MonoBehaviour
{
    // this script blocks (setactive = false) something (target)
    public GameObject target;

    private void Start()
    {
        target.SetActive(false);
    }

    private void OnDisable()
    {
        if (target != null)
        {
            target.SetActive(true);
        }
    }
}
