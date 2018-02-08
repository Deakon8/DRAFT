using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
//Крепим этот скрипт к MenuManager (*Обязательно)
public class MenuManager : PersistentSingleton<MenuManager> {

    //Структура содержащая уровни, записанные в scrollview
    struct LevelNumb
    {
        public Text Txt;
        public Button Btn;
    };

    //Режимы кнопок PanelButtonMenu
    enum PanelMenu { Hided, ShowButtons, ShowLevels };
    PanelMenu menuStatus;

    public RectTransform panelButtonMenu, scrollViewContent;
    public TextMesh txtLastLevel, txtCurrentLevel;
    public Image imgSound, imgRewardedAd;

    public Color currentLevelColor;  //Цвет текста текущего лвл (Закрытый, пройденый, текущий)
    public int distanceBtwRows;
    public bool IsMenuShown { get; set; }
    public Vector2 PosMenuTopEdge { get; private set; }

    private Text txtRewardedAd;
    private Button btnRewardedAd;
    private Sprite soundOn,soundOff;
    private List<LevelNumb> levelsNumbs = new List<LevelNumb>();
    private Vector2 posInitPanelBM , posHidePanelBM,posLevelsHigh;
    private float posZTxt, timeMenuShowAndHide = 0.5F;
    private int gameCount; //Используеться только если включить рекламу
    public override void Awake()
    {
    }
    void Start ()
    {
        float diff = panelButtonMenu.anchoredPosition.y;
 
        float heightPanelMenu = panelButtonMenu.rect.height;
        posInitPanelBM = panelButtonMenu.anchoredPosition;
        posHidePanelBM = new Vector2(posInitPanelBM.x, -diff*1.2F);
        posLevelsHigh = new Vector2(posInitPanelBM.x, posInitPanelBM.y+ heightPanelMenu*5F);


        panelButtonMenu.anchoredPosition = posHidePanelBM;
        txtRewardedAd = imgRewardedAd.transform.GetChild(0).GetComponent<Text>();
        btnRewardedAd = imgRewardedAd.GetComponent<Button>();
        btnRewardedAd.gameObject.SetActive(false);
        
        soundOn = Resources.Load<Sprite>("Pics/SoundOn");
        soundOff = Resources.Load<Sprite>("Pics/SoundOff");

        posZTxt = txtLastLevel.transform.position.z;

        txtLastLevel.gameObject.SetActive(false);
        txtCurrentLevel.gameObject.SetActive(false);

        //Меню со всеми лвл
        menuStatus = PanelMenu.ShowButtons;
        StartCoroutine(DefinePosMenuTopEdge(0));

        imgSound.sprite = PlayerPrefsX.GetBool(Constants.PrefsIsSoundOn) ? soundOn : soundOff;

        List<RectTransform> rows = new List<RectTransform>();
        //Добавляем строки, пока не закончаться лвл
        foreach (RectTransform row in scrollViewContent)
        {
            rows.Add(row);
        }
        int k = 0;
        for (int i = 0; i < rows.Count; i++)
        {
            if (i >= 1)
            {
                rows[i].anchoredPosition = new Vector2(rows[i].anchoredPosition.x, rows[i - 1].anchoredPosition.y - distanceBtwRows);
            }
            Text[] numbsInRow = rows[i].GetComponentsInChildren<Text>();
            for (int j = 0; j < numbsInRow.Length; j++)
            {
                k++;
                numbsInRow[j].text = "" + k;
                LevelNumb lv = new LevelNumb();
                lv.Txt = numbsInRow[j];
                lv.Btn = numbsInRow[j].GetComponent<Button>();
                levelsNumbs.Add(lv);
            }
        }
    }

    //Меню уезжает
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MenuHide();
        }
    }

    //Показываем меню(И все кнопки в том числе)
    public void MenuShow()
    {
        IsMenuShown = true;
        panelButtonMenu.gameObject.SetActive(true);
        btnRewardedAd.gameObject.SetActive(true);
        menuStatus = PanelMenu.ShowButtons;
        StartCoroutine(Tools.MoveTO(panelButtonMenu, posInitPanelBM, timeMenuShowAndHide));
        imgRewardedAd.ColorNonTransparent();
        txtRewardedAd.ColorNonTransparent();
        btnRewardedAd.enabled = true;
        imgSound.ColorNonTransparent();
        StartCoroutine(DefinePosMenuTopEdge(timeMenuShowAndHide));
    }
    IEnumerator DefinePosMenuTopEdge(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        PosMenuTopEdge = Camera.main.ScreenToWorldPoint(btnRewardedAd.GetComponent<RectTransform>().transform.position);
    }

    //Прячем меню(Все кнопки уходят в нон актив)
    public void MenuHide()
    {
        IsMenuShown = false;
        menuStatus = PanelMenu.Hided;
        StartCoroutine(Tools.MoveTO(panelButtonMenu, posHidePanelBM, timeMenuShowAndHide));
        StartCoroutine(DisappearBtnRewardedAd(timeMenuShowAndHide));
        StartCoroutine(DefinePosMenuTopEdge(timeMenuShowAndHide));
    }

    //Грузим лвл и выключаем меню
    public void LoadLevel(Text txt)
    {
        int levelNumb = Int32.Parse(txt.text);
        GameManager.Instance.LevelLoad(levelNumb - 1);
        MenuHide();
    }

    //ВКЛ или ВЫКЛ звук
    public void BtnSound()
    {
        if (PlayerPrefsX.GetBool(Constants.PrefsIsSoundOn))
        {
            imgSound.sprite = soundOff;
            PlayerPrefsX.SetBool(Constants.PrefsIsSoundOn,false);
        }
        else
        {
            imgSound.sprite = soundOn;
            PlayerPrefsX.SetBool(Constants.PrefsIsSoundOn, true);
        }
    }

    //Номера уровней(Циферки сами)
    public void BtnLevels()
    {
        CtrlLevelNumbers();
        if (menuStatus == PanelMenu.ShowLevels)
        {
            MenuHide();
        }
        if (menuStatus == PanelMenu.ShowButtons)
        {
            StartCoroutine(Tools.MoveTO(panelButtonMenu, posLevelsHigh, timeMenuShowAndHide));
            menuStatus = PanelMenu.ShowLevels;
            StartCoroutine(DefinePosMenuTopEdge(timeMenuShowAndHide));
            return;
        }
    }

    //Кнопка подсказки за рекламу (Вывод рекламы)
    public void BtnRewardedAd()
    {
        if (AdsManager.Instance.enabled)
        {
            AdsManager.Instance.ShowRewardedVideo();
        }
    }

    //Показываем какой уровень сейчас (При переходе или включении другого лвл, цифра на весь экран)
    public void NewLevelAnims(int lastLevel, int nextLevel)
    {
        txtLastLevel.text = ""+lastLevel;
        txtLastLevel.transform.SetPos2D(new Vector2(txtLastLevel.transform.position.x, -0.2F));
        Color colorTxtLastLevel = txtLastLevel.color;
        txtLastLevel.color = new Color(colorTxtLastLevel.r, colorTxtLastLevel.g, colorTxtLastLevel.b, 0);
        txtLastLevel.gameObject.SetActive(true);

        txtCurrentLevel.text = "" + nextLevel;
        txtCurrentLevel.transform.SetPos2D(new Vector2(txtCurrentLevel.transform.position.x, 7));

        Color colorTxtCurrentLevel = txtCurrentLevel.color;
        txtCurrentLevel.color = new Color(colorTxtCurrentLevel.r, colorTxtCurrentLevel.g, colorTxtCurrentLevel.b, 1);
        txtCurrentLevel.gameObject.SetActive(true);


        StartCoroutine(NewLevelActions(nextLevel));
    }

   //Глянул рекламу - получил подсказку
    public void AfterVideoAd()
    {
        Debug.Log("<color=blue>AfterVideoAd</color>");
        GameManager.Instance.ShowHint();
    }

    IEnumerator DisappearBtnRewardedAd(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        float duration = 0.2F;
        StartCoroutine(Tools.MakeTransparent(imgRewardedAd,0, duration));
        StartCoroutine(Tools.MakeTransparent(txtRewardedAd, 0, duration));
    }

    IEnumerator NewLevelActions(int nextLevel)
    {
        float time1 = 0.5F;
        StartCoroutine(Tools.MakeTransparent(txtLastLevel, 1, time1));
        yield return new WaitForSeconds(time1+0.2F);

        Vector3 desirePos1 = new Vector3(txtLastLevel.transform.position.x, -7, posZTxt);
        StartCoroutine(Tools.MoveTO(txtLastLevel.transform, desirePos1, time1));
        yield return new WaitForSeconds(0.2F);
        AudioManager.Instance.PlayOneShot(AudioManager.ride);

        Vector3 desirePos2 = new Vector3(txtCurrentLevel.transform.position.x, -0.2F, posZTxt);
        StartCoroutine(Tools.MoveTO(txtCurrentLevel.transform, desirePos2, time1));
        yield return new WaitForSeconds(time1 + 0.2F);
        StartCoroutine(Tools.MakeTransparent(txtCurrentLevel, 0, time1));
        gameCount++;
        AdsManager.Instance.ShowInterstitial(gameCount);
    }


    //Цвета уровней (Цифр) в списке с уровнями
    void CtrlLevelNumbers()
    {
        int currentLevel = GameManager.Instance.GetCurrentLevelNumb();
        for (int i = 0; i < levelsNumbs.Count; i++)
        {
            if (i == currentLevel)
            {
                levelsNumbs[i].Txt.fontStyle = FontStyle.Bold;
                levelsNumbs[i].Txt.color = currentLevelColor;
            }
            else
            {
                levelsNumbs[i].Txt.fontStyle = FontStyle.Normal;
                if (!PlayerPrefsX.GetBool(Constants.PrefsIsAvailable + i))
                {
                    levelsNumbs[i].Txt.color = Color.gray;
                    levelsNumbs[i].Btn.enabled = false;
                }
                else
                {
                    levelsNumbs[i].Txt.color = Color.black;
                    levelsNumbs[i].Btn.enabled = true;
                }
            }
        }
    }
    //Грузим сцену меню
    public void BtnBack()
    {
        SceneManager.LoadScene("MenuScene");
    }
    //Выходим из игры
    public void BtnExit()
    {
        Application.Quit();
    }
    //Грузим сцену с игрой
    public void BtnMenuStart()
    {
        SceneManager.LoadScene("MainScene");
    }
}
