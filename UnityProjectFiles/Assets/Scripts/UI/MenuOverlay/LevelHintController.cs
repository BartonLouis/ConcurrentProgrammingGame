using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelHintController : MonoBehaviour
{
    public static LevelHintController instance;

    private Animator Anim;
    public TextMeshProUGUI Text;

    private bool open;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        Hide();
    }

    public void Show()
    {
        Anim.SetBool("Open", true);
        open = true;
    }

    public void SetText(TextAsset text)
    {
        Text.text = text.text;
    }

    public void Hide()
    {
        Anim.SetBool("Open", false);
        open = false;
    }

    public void ButtonPressed()
    {
        if (open)
            Hide();
        else
            Show();
    }

}
