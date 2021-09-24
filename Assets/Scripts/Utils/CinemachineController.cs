using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    public CinemachineVirtualCamera myCinemachine;
    public void ChangeFollow(Transform objective)
    {
        myCinemachine.m_Follow = objective;
    }
}
