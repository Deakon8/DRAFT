using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Control stoke behavior 
/// </summary>
///initialized in Obstacles script
public class StrokeBehaviour  {


    /// <summary>
    /// Contain component reference to stroke gameobject
    /// </summary>
    struct Stroke
    {
        public SpriteRenderer SpriteRen;
        public Transform Trans;
        public GameObject GameObj;
    };

    MonoBehaviour mono;
    List<Stroke> strokes = new List<Stroke>();
    public StrokeBehaviour(MonoBehaviour mono)
    {
        this.mono = mono;
        Transform strokesParent = new GameObject().transform;
        strokesParent.name = "Strokes";

        GameObject[] strokePrefabs = Resources.LoadAll<GameObject>("Prefabs/Strokes");
        for (int i = 0; i < strokePrefabs.Length; i++)
        {
            StrokesInit(strokePrefabs[i], strokesParent, strokePrefabs[i].name);
        }
    }


    /// <summary>
    /// Stroke reaction from hit
    /// </summary>
    /// <param name="hitedFig"></param>
    public void StrokeReact(Figure hitedFig)
    {
        foreach (Stroke stroke in strokes)
        {
            if (!stroke.GameObj.activeInHierarchy && stroke.SpriteRen.sprite.name.StartsWith(hitedFig.SpriteRen.sprite.name))
            {
                stroke.Trans.position = hitedFig.Trans.position;
                stroke.Trans.localRotation = hitedFig.Trans.localRotation;
                stroke.Trans.localScale = hitedFig.Trans.localScale;
                stroke.GameObj.SetActive(true);
                float time = 0.5F;
                mono.StartCoroutine(Tools.MakeTransparent(stroke.SpriteRen, 0, time));
                mono.StartCoroutine(Tools.ChangeScale(stroke.Trans, hitedFig.Trans.localScale * 1.5F, time));
                break;
            }
        }
    }

    /// <summary>
    /// Reset of all strokes
    /// </summary>
    public void Reset()
    {
        foreach (Stroke s in strokes)
        {
            s.GameObj.SetActive(false);
            s.SpriteRen.ColorNonTransparent();
        }
    }

    /// <summary>
    /// Spawn strokes
    /// </summary>
    /// <param name="sample">from which will be spawing</param>
    /// <param name="parent">parent gameobject of future gameobject</param>
    /// <param name="name">name of future gameobject</param>
    void StrokesInit(GameObject sample, Transform parent, string name)
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject newObj = (GameObject)GameObject.Instantiate(sample, Vector3.zero, Quaternion.identity);
            newObj.name = name + i;
            newObj.transform.parent = parent;
            newObj.SetActive(false);

            Stroke newStroke = new Stroke();
            newStroke.SpriteRen = newObj.GetComponent<SpriteRenderer>();
            newStroke.Trans = newObj.transform;
            newStroke.GameObj = newObj;

            newStroke.SpriteRen.color = GameManager.Instance.colorStroke;
            strokes.Add(newStroke);
        }
    }
}
