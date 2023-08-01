using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSwitcher : MonoBehaviour
{
    [SerializeField] List<GameObject> m_avatars;

    private void Start()
    {
        //foreach (var avatar in m_avatars)
        //{
        //    avatar.SetActive(false);
        //}
    }

    public void loadAvatar(int index)
    {
        if(index >=  m_avatars.Count || index < 0)
        {
            m_avatars[0].SetActive(true);
        }
        else
        {
            m_avatars[index].SetActive(true);
        }
    }
}
