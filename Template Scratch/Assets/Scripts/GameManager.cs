using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool debugTools;
    private void Awake()
    {
        //Application.targetFrameRate = 60;
        print("Started scratch game on Unity " + Application.unityVersion);
    }

    private void Update()
    {
        if (debugTools)
        {
            Debug.LogError("FPS : " + 1 / Time.deltaTime);
            Debug.developerConsoleVisible = true;
        }
    }
}