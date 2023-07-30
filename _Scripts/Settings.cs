using UnityEngine;

public class Settings : MonoBehaviour
{
    //Allows for sensitivity setting to persist between states 
    public static Settings Instance;
    public float cameraSensitivity;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        cameraSensitivity = 15f;
        DontDestroyOnLoad(gameObject);
    }

    public void setCameraSensitivity(float sensitivity)
    {
        Settings.Instance.cameraSensitivity = sensitivity;
    }
}
