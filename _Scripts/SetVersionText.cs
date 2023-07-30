using TMPro;
using UnityEngine;

public class SetVersionText : MonoBehaviour
{
    public TextMeshProUGUI VersiontextBox;
    public TextMeshProUGUI TitletextBox;
    //Sets title & version no on Title Screen
    void Start()
    {
        VersiontextBox.SetText("Version " + Application.version);
        TitletextBox.SetText(Application.productName);
    }
}
