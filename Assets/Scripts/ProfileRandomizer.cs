using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ProfileRandomizer : MonoBehaviour
{
    [SerializeField] Sprite[] m_avatars;
    [SerializeField] float m_animationTime;
    [SerializeField] Image m_TargetImage;
    [SerializeField] int m_selectedIndex;
    [SerializeField] TMP_InputField m_PlayerName;
    float nextChangeStep;
    float internalTimer;
    bool animate;
    int iteration;
    public int SelectedIndex { get { return m_selectedIndex; } }
    public string PlayerName { get { return m_PlayerName.text; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (animate)
        {
            if (Math.Round(internalTimer,3) <= Math.Round(nextChangeStep,3) && animate)
            {
                iteration++;
                nextChangeStep = Time.fixedDeltaTime * iteration;
                assignRandomImage();
            }
            if (internalTimer > m_animationTime)
            {
                animate = false;
                assignRandomImage();
            }
            internalTimer += Time.fixedDeltaTime;
        }
    }
    public void RandomizeImages()
    {
        animate = true;
    }

    void assignRandomImage()
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        m_selectedIndex = UnityEngine.Random.Range(0, m_avatars.Length);
        m_TargetImage.sprite = m_avatars[m_selectedIndex];
    }

    IEnumerator BeginRandomization()
    {
        while(internalTimer < m_animationTime)
        {
            internalTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if(nextChangeStep < internalTimer)
            {
                iteration++;
                nextChangeStep += iteration * 0.5f;
                
            }
        }
    }
}
