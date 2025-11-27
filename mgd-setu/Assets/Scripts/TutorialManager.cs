using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("UI")]
    public GameObject tutorialPanel;
    public TMP_Text tutorialText;
    public Button gotItButton;

    [Header("Text")]
    [TextArea]
    public string tutorialMessage =
        "Tap anywhere that’s not UI to flip polarity.\n\n" +
        "Red + Red repel, Red + Blue attract.\n" +
        "Use magnetism to dodge walls and hazards.\n\n" +
        "Survive as long as you can.";

    const string TutorialSeenKey = "IH_TutorialSeen";

    public bool hasSeenTutorial;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        hasSeenTutorial = PlayerPrefs.GetInt(TutorialSeenKey, 0) == 1;

        if (gotItButton != null)
            gotItButton.onClick.AddListener(OnGotIt);
    }

    public void ShowTutorialIfNeeded()
    {
        if (hasSeenTutorial) return;
        if (tutorialPanel == null) return;

        if (tutorialText != null)
            tutorialText.text = tutorialMessage;

        tutorialPanel.SetActive(true);
        Time.timeScale = 0f; // pause while reading
    }

    public void OnGotIt()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;

        PlayerPrefs.SetInt(TutorialSeenKey, 1);
        PlayerPrefs.Save();

        // START THE RUN HERE
        if (GameManager.Instance != null)
            GameManager.Instance.StartRun();
    }

}
