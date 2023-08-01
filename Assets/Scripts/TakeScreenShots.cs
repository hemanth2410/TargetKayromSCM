using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenShots : MonoBehaviour
{
    [SerializeField] List<GameObject> children;
    Camera cam;
    int internalPointer;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        StartCoroutine(TakePicture());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TakePicture()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetActive(true);
            RenderTexture tempRT = new RenderTexture(800, 800, 24, RenderTextureFormat.ARGB32)
            {
                antiAliasing = 4
            };
            cam.targetTexture = tempRT;
            RenderTexture.active = tempRT;
            cam.Render();
            Texture2D imageData = new Texture2D(800, 800, TextureFormat.RGBA32, false, true);
            imageData.ReadPixels(new Rect(0, 0, imageData.width, imageData.height), 0, 0);
            imageData.Apply();
            RenderTexture.active = null;
            byte[] bytes = imageData.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "\\ProfilePics\\" + i.ToString() + ".png", bytes);
            children[i].SetActive(false);
        }
        cam.targetTexture = null;
    }
}
