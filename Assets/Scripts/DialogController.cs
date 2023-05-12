using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [SerializeField] GameObject dialogPanel;
    [SerializeField] TextMeshProUGUI speaker;
    [SerializeField] TextMeshProUGUI dialog;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initDialog(string speakerText, string[] dialogText)
    {
        speaker.text = speakerText;
        dialog.text = dialogText[0];
        dialogPanel.SetActive(true);
    }
}
