using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SurrenderBtn : MonoBehaviour
{
    [SerializeField] Sprite actirve;
    [SerializeField] Sprite inActive;
    [SerializeField] TMP_Text btnText;
    public Button surrenderButton;
    // Start is called before the first frame update
    void Start()
    {
        Setup(true);

        surrenderButton = GetComponent<Button>();

        surrenderButton.onClick.AddListener(delegate {
            GameManager.instance.GameResult(false);
        });
    }
    public void Setup(bool isActive)
    {
        GetComponent<Image>().sprite = isActive ? actirve : inActive;
        GetComponent<Button>().interactable = isActive;
        btnText.color = isActive ? Color.black : Color.yellow;
    }
}
