using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Крепим этот скрипт к ClassKeeper (*Обязательно)
public class GameManager : PersistentSingleton<GameManager> {
    public Color colorStroke;

    public bool isTestMode; //Тестмодом можно загрузить любой активный уровень 
    public List<Level> Levels { get;  set; }
    //public string LevelName { get; set; } //Используеться только если уровни записаны буквами.
    public int LevelNumber { get; set; }

    Obstacles obstacles;

    public override void Awake()
    {
        QualitySettings.antiAliasing = 4;

        //Фишка с удалением данных игры. Если допустим обновили уровни.
        //PlayerPrefs.DeleteAll();

        //Первый раз в игре - звук включен
        if (PlayerPrefs.GetInt(Constants.PrefsSessionCount) == 0)
        {
            PlayerPrefs.SetInt(Constants.PrefsSessionCount, PlayerPrefs.GetInt(Constants.PrefsSessionCount) + 1);
            PlayerPrefsX.SetBool(Constants.PrefsIsSoundOn, true);
        }


        //Настройка слоя
        GameObject[] figurePrefabs = Resources.LoadAll<GameObject>("Prefabs/Figures");
        for (int i = 0; i < figurePrefabs.Length; i++)
        {
            figurePrefabs[i].layer = LayerMask.NameToLayer(Constants.LayerHited);
            if(figurePrefabs[i].name.Contains("NotDisp"))
            {
                figurePrefabs[i].tag = Constants.TagNotDisappear;
            }
        }

        //Крепим слой bounds к объектам которые вне камеры
        Transform cam = Camera.main.transform;
        Transform[] trans = cam.GetComponentsInChildren<Transform>();
        for (int i = 0; i < trans.Length; i++)
        {
            if (trans[i].GetComponent<Collider2D>() && trans[i].GetComponent<SpriteRenderer>()== null)
            {
                trans[i].gameObject.layer = LayerMask.NameToLayer(Constants.LayerBounds);
            }
        }
    }

    void Start ()
    {
        obstacles = new Obstacles(this);
        //Тестмод. Включаем тот лвл, который активен
        if (!isTestMode)
        {
            PlayerPrefsX.SetBool(Constants.PrefsIsAvailable + 0, true);
            int maxAvailableLevel = 0;
            for (int i = 0; i < Levels.Count; i++)
            {
                if (PlayerPrefsX.GetBool(Constants.PrefsIsAvailable + i))
                    maxAvailableLevel = i;
            }

            for (int i = 0; i < Levels.Count; i++)
            {
                if (i == maxAvailableLevel)
                {
                    Levels[i].GameObj.SetActive(true);
                    obstacles.LevelLoad(Levels[i]);
                }
                else
                {
                    Levels[i].GameObj.SetActive(false);
                }
            }
        }
    }

    public void HitResponse(GameObject hitedObj) //Попадание в объект
    {
        obstacles.HitResponse(hitedObj);
    }
    public void ResetLevel() //Рестарт лвл
    {
        obstacles.Reset();
    }
    public void ShowHint() //Показать подсказку
    {
        obstacles.ShowHint();
    }
    public void LevelLoad(int levelNumb) //Грузим лвл
    {
        LevelNumber = levelNumb;
        if (levelNumb < Levels.Count)
        {
            for (int i = 0; i < Levels.Count; i++)
            {
                if(i == levelNumb)
                {
                    Levels[i].GameObj.SetActive(true);

                    obstacles.LevelLoad(Levels[i]);
                }  
                else
                {
                    Levels[i].GameObj.SetActive(false);
                }          
            }
        }
    }

    public int GetCurrentLevelNumb() //Вывод текущего лвл
    {
        int levelNumb = 0;
        for (int i = 0; i < Levels.Count; i++)
        {
            if (Levels[i].GameObj.activeInHierarchy)
            {
                levelNumb = i;
                break;
            }
        }
        return levelNumb;          
    }
    public void Escape() //Выход
    {
        Application.Quit();
    }
}
