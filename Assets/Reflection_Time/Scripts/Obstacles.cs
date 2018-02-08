using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Contain level properties
/// </summary>
public struct Level
{
    public GameObject GameObj;
    public GameObject Hint;
    public List<Figure> Figures;
    public int DisappearedFiguresQTY;
}

/// <summary>
/// Contain Figure properties and components
/// </summary>
public struct Figure
{
    public SpriteRenderer SpriteRen;
    public Transform Trans;
    public GameObject GameObj;
    public Collider2D Collider;
    public Vector3 StartScale;
};

/// <summary>
/// Control obstacles behaviour
/// </summary>
/// initialized in GameManager script
public class Obstacles  {

    private MonoBehaviour mono;
    private StrokeBehaviour strokeB;

    private List<Level> levels = new List<Level>();
    private List<Figure> figures = new List<Figure>();
    private Level currentLevel;
    private bool isCurrentLevelPassed;
    private int hitsQTY;

    public Obstacles(MonoBehaviour mono)
    {
        this.mono = mono;
        strokeB = new StrokeBehaviour(mono);

        #region
        foreach (Transform level in DefineLevelsParents())
        {
            Level levStruct = new Level();
            levStruct.GameObj = level.gameObject;
            levStruct.Figures = new List<Figure>();

            SpriteRenderer[] spriteRenFigs = level.GetComponentsInChildren<SpriteRenderer>();
            Transform[] objsWithSprite = new Transform[spriteRenFigs.Length];

            for (int i = 0; i < spriteRenFigs.Length; i++)
            {
                objsWithSprite[i] = spriteRenFigs[i].transform;
            }
           

            foreach (Transform objsInLevel in objsWithSprite)
            {
                //Debug.Log("<color=blue>Name</color> " + objsInLevel.name);
                //
                if (!objsInLevel.GetComponent<Collider2D>())
                {
                    if (objsInLevel.name.Contains("Hint"))
                    {
                        levStruct.Hint = objsInLevel.gameObject;
                        levStruct.Hint.gameObject.SetActive(false);
                    }                        
                }

                else
                {
                    if (objsInLevel.tag != Constants.TagNotDisappear)
                        levStruct.DisappearedFiguresQTY++;

                    
                    levStruct.Figures.Add(DefineFigureProperties(objsInLevel));
                }
            }

            levels.Add(levStruct);
        }
        #endregion
        foreach (Level lev in levels)
        {
            if (lev.GameObj.activeInHierarchy)
            {
                currentLevel = lev;
            }
        }
        GameManager.Instance.Levels = levels;


        if(currentLevel.Figures.Count == 0)
        {
            Debug.Log("<color=red>figs count == 0</color>" );
        }

        //string prefsKey = Constants.PrefsIsAvailable + (GameManager.Instance.GetCurrentLevelNumb());
        //PlayerPrefsX.SetBool(prefsKey, true);
    }


    public void HitResponse(GameObject hitedObj)
    {
        #region hit on object, which disappeared
        if (hitedObj.tag != Constants.TagNotDisappear)
        {
            bool isFigsEqual = false;
            foreach (Figure fig in currentLevel.Figures)
            {                
                if (hitedObj.Equals(fig.GameObj))
                {
                    Debug.Log("<color=green>figure Equals</color> ");
                    isFigsEqual = true;
                    hitsQTY++;
                    FigureDisappear(fig);
                    break;
                }
            }

            //if hited gameobject doesnt exist in figures list, figure struct is created 
            if (!isFigsEqual)
            {
                Debug.Log("<color=yellow>NOT FOUND</color> figures in list");
                Figure fig = DefineFigureProperties(hitedObj.transform);
                figures.Add(fig);
                FigureDisappear(fig);
            }

            CheckForLevelComplete();
        }
        #endregion

        //Hit on black figure
        else
        {
            mono.StartCoroutine(Tools.ScalePingPong(hitedObj.transform, hitedObj.transform.localScale * 1.08F,0.1F));
            AudioManager.Instance.PlayOneShot(AudioManager.kick);
        }
    }
  
    /// <summary>
    /// Show "Hint" gameobject 
    /// </summary>
    public void ShowHint()
    {
        if(currentLevel.Hint != null)
        {
            currentLevel.Hint.gameObject.SetActive(true);
            Debug.Log("<color=yellow>Hint make Active</color>");
        }        
        else
        {
            Debug.LogError("Hint is null");
        }
    }

    public void LevelLoad(Level lev)
    {

        if(!lev.Equals(currentLevel))
        {
            currentLevel = lev;
            foreach (Figure fig in currentLevel.Figures)
            {
                //Debug.Log("<color=red>Fatal error:</color> AssetBundle not found");
                fig.SpriteRen.ColorTransparent();
                mono.StartCoroutine(Tools.MakeTransparent(fig.SpriteRen, 1, 0.5F));
                //fig.Collider.enabled = true;
                fig.Collider.enabled = true;
                fig.Collider.isTrigger = false;
            }

            if (lev.Hint != null)
                lev.Hint.SetActive(false);

            isCurrentLevelPassed = false;
            strokeB.Reset();
        }
    }

    /// <summary>
    /// Reset all properties in figures
    /// </summary>
    public void Reset()
    {
        if(!isCurrentLevelPassed)
        {
            hitsQTY = 0;
            foreach (Figure fig in currentLevel.Figures)
            {
                if (fig.Collider.isTrigger || fig.SpriteRen.color.a != 1)
                {
                    mono.StartCoroutine(Tools.MakeTransparent(fig.SpriteRen, 1, 0.4F));
                    fig.Collider.isTrigger = false;
                }
                if(fig.Trans.tag == Constants.TagNotDisappear)
                {
                    mono.StartCoroutine(Tools.ChangeScale(fig.Trans,fig.StartScale,0.4F));
                }
            }
            strokeB.Reset();
        }
    }


    /// <summary>
    /// Define properties of figure struct from transform object
    /// </summary>
    /// <returns>figure struct</returns>
    Figure DefineFigureProperties(Transform trans)
    {
        Figure fig = new Figure();
        fig.Trans = trans.transform;
        fig.GameObj = trans.gameObject;
        fig.Collider = trans.GetComponent<Collider2D>();
        fig.SpriteRen = trans.GetComponent<SpriteRenderer>();
        fig.StartScale = trans.localScale;
        return fig;
    }

    void FigureDisappear(Figure hitedFig)
    {
        AudioManager.Instance.LadderPlay(hitsQTY-1);
        hitedFig.Collider.isTrigger = true;
        mono.StartCoroutine(Tools.MakeTransparent(hitedFig.SpriteRen, 0, 0.1F));
        strokeB.StrokeReact(hitedFig);
    }


    void CheckForLevelComplete()
    {
        int k = 0;

        foreach (Figure fig in currentLevel.Figures)
        {
            if (fig.Collider.isTrigger)
                k++;
        }

        if (k == currentLevel.DisappearedFiguresQTY)
        {
            Debug.Log("<color=green>new Level</color>");
            isCurrentLevelPassed = true;

            string prefsKey = Constants.PrefsIsAvailable  + (GameManager.Instance.GetCurrentLevelNumb()+1);
            PlayerPrefsX.SetBool(prefsKey, true);

            mono.StartCoroutine(NewLevel(1F));
        }
    }

    /// <summary>
    /// Wait and load new Level
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator NewLevel(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        float timeFigToFade = 0.4F;
        foreach (Figure fig in currentLevel.Figures)
        {
            if (fig.Trans.tag == Constants.TagNotDisappear)
            {
                mono.StartCoroutine(Tools.MakeTransparent(fig.SpriteRen, 0, timeFigToFade));
            }
        }
        //if current level doesn't contain not disappeared figures
        if ((currentLevel.Figures.Count - currentLevel.DisappearedFiguresQTY) == 0)
        {
            timeFigToFade = 0;
        }

        yield return new WaitForSeconds(timeFigToFade);
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].GameObj.activeInHierarchy)
            {
                levels[i].GameObj.SetActive(false);
                if (i != levels.Count - 1)
                {
                    //Show level number in textMesh
                    MenuManager.Instance.NewLevelAnims(i+1, (i + 2));

                    yield return new WaitForSeconds(1.8F);
                    GameManager.Instance.LevelLoad(i + 1);
                    break;
                }
                else
                {
                    //Level is Last
                    Debug.Log("<color=red>Level is last</color>");
                }
            }
        }
    }


    //Define all levels parent
    List<Transform> DefineLevelsParents()
    {
        //Find all gameobjecs with tag "Level"
        Transform[] levelsArray = GameObject.Find("Levels").transform.GetComponentsInChildren<Transform>(true);
        List<Transform> newLevelList = new List<Transform>();
        for (int i = 0; i < levelsArray.Length; i++)
        {
            if (levelsArray[i].tag == Constants.TagLevel)
                newLevelList.Add(levelsArray[i]);
        }
        return newLevelList;
    }
}
