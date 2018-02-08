using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public GameObject levelChanger;
    public GameObject MenuBack;
    public GameObject exitPanel;
    public GameObject optionsPanel;
    public GameObject customPanel;
    public GameObject ExitButton;
    public GameObject BackButton;

    void Update()
    {
        if (levelChanger.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            levelChanger.SetActive(false);
        }
        if (MenuBack.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            MenuBack.SetActive(false);
        }
        if (exitPanel.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            exitPanel.SetActive(false);
        }
        if (optionsPanel.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            optionsPanel.SetActive(false);
        }
        if (customPanel.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            customPanel.SetActive(false);
        }
        if (ExitButton.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitButton.SetActive(false);
        }
        if (BackButton.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            BackButton.SetActive(false);
        }
    }

    public void OnClickStart()
    {
        levelChanger.SetActive(true);
        BackButton.SetActive(true);
        MenuBack.SetActive(false);
        ExitButton.SetActive(false);
    }
    public void OnClickBack()
    {
        MenuBack.SetActive(true);
        ExitButton.SetActive(true);
        levelChanger.SetActive(false);
        optionsPanel.SetActive(false);
        customPanel.SetActive(false);
        BackButton.SetActive(false);
    }
    public void OnClickExit()
    {
        exitPanel.SetActive(true);
        MenuBack.SetActive(false);
        ExitButton.SetActive(false);
        BackButton.SetActive(false);
    }
    public void OnClickExitTrue()
    {
        Application.Quit();
    }
    public void OnClickExitFalse()
    {
        exitPanel.SetActive(false);
        MenuBack.SetActive(true);
        ExitButton.SetActive(true);
        BackButton.SetActive(false);
    }
    public void levelBttns(int level)
    {
        SceneManager.LoadScene(level);
    }
    public void rate()
    {
        Application.OpenURL("https://play.google.com/");
    }
    public void OnClickOptions()
    {
        optionsPanel.SetActive(true);
        BackButton.SetActive(true);
        MenuBack.SetActive(false);
        ExitButton.SetActive(false);
    }
    public void OnClickCustomize()
    {
        customPanel.SetActive(true);
        BackButton.SetActive(true);
        MenuBack.SetActive(false);
        ExitButton.SetActive(false);
    }
}