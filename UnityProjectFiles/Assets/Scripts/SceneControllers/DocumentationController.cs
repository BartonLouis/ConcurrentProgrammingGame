using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DocumentationController : MonoBehaviour
{

    public static DocumentationController instance;

    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image image;
    public Transform parent;
    [Space(10)]

    public TutorialElement[] tutorialElements;
    public GameObject buttonPrefab;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        AudioManager.instance.PlayMusic("MainMenu");
        int index = 0;
        foreach(TutorialElement e in tutorialElements)
        {
            GameObject button = Instantiate(buttonPrefab, parent);
            button.GetComponent<TopicButton>().Setup(index, e.name);
            index++;
        }
    }

    public void ButtonClicked(int index)
    {
        TutorialElement element = tutorialElements[index];
        title.text = element.name;
        description.text = element.text.text;
        image.sprite = element.image;
    }

    public void MainMenu()
    {
        AudioManager.instance.Play("Menu2");
        SceneManager.LoadScene("MainMenu");
    }

}
