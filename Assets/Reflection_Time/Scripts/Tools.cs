using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class contains static methods,that used in game
/// </summary>
public class Tools {

    /// <summary>
    /// Define true size of object
    /// </summary>
    public static Vector2 TrueSize(Transform obj)
    {
        SpriteRenderer spriteRen = obj.GetComponent<SpriteRenderer>();
        float width = spriteRen.sprite.bounds.size.x * obj.transform.localScale.x;
        float height = spriteRen.sprite.bounds.size.y * obj.transform.localScale.y;
        return new Vector2(width, height);
    }

    /// <summary>
    /// Make object transparent in animation
    /// </summary>
    public static IEnumerator MakeTransparent(SpriteRenderer target, float alpha, float duration)
    {
        Color color = target.color;
        if (target == null)
            yield break;

        float t = 0f;
        while (t < 1.0f)
        {
            if (target == null)
                yield break;

            Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(color.a, alpha, t));
            target.color = newColor;

            t += Time.deltaTime / duration;

            yield return null;

        }
        Color finalColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(color.a, alpha, t));
        target.color = finalColor;
    }

    public static IEnumerator MakeTransparent(GameObject transObj, float alpha, float duration)
    {
        SpriteRenderer target = transObj.GetComponent<SpriteRenderer>();
        Color color = target.color;

        if (target == null)
            yield break;

        float t = 0f;
        while (t < 1.0f)
        {
            if (target == null)
                yield break;

            Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(color.a, alpha, t));
            target.color = newColor;

            t += Time.deltaTime / duration;

            yield return null;

        }
        Color finalColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(color.a, alpha, t));
        target.color = finalColor;
    }

    public static IEnumerator MakeTransparent(TextMesh txtMesh, float alpha, float duration)
    {
        Color color = txtMesh.color;

        if (txtMesh == null)
            yield break;

        float t = 0f;
        while (t < 1.0f)
        {
            if (txtMesh == null)
                yield break;

            Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(color.a, alpha, t));
            txtMesh.color = newColor;

            t += Time.deltaTime / duration;

            yield return null;

        }
        Color finalColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(color.a, alpha, t));
        txtMesh.color = finalColor;
    }

    public static IEnumerator MakeTransparent(Image image, float alpha, float duration)
    {
        Color color = image.color;

        if (image == null)
            yield break;

        float t = 0f;
        while (t < 1.0f)
        {
            if (image == null)
                yield break;

            Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(color.a, alpha, t));
            image.color = newColor;

            t += Time.deltaTime / duration;

            yield return null;

        }
        Color finalColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(color.a, alpha, t));
        image.color = finalColor;
    }

    public static IEnumerator MakeTransparent(Text textUI, float alpha, float duration)
    {
        Color color = textUI.color;

        if (textUI == null)
            yield break;

        float t = 0f;
        while (t < 1.0f)
        {
            if (textUI == null)
                yield break;

            Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(color.a, alpha, t));
            textUI.color = newColor;

            t += Time.deltaTime / duration;

            yield return null;

        }
        Color finalColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(color.a, alpha, t));
        textUI.color = finalColor;
    }


    /// <summary>
    /// Change scale in animation
    /// </summary>
    public static IEnumerator ChangeScale(Transform transObj, Vector2 desireScale, float duration)
    {
        Vector2 startScale = transObj.localScale;

        float timeElapsed = 0f;
        
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transObj.localScale = Vector2.Lerp(startScale, desireScale, timeElapsed / duration);
            yield return null;
        }
    }

    /// <summary>
    /// Change scale in ping-pong style
    /// </summary>
    public static IEnumerator ScalePingPong(Transform transObj, Vector2 maxScale, float duration)
    {
        Vector2 startScale = transObj.localScale;
        Vector2 miniMaxScale = startScale + (maxScale - startScale) / 4;
        float timeElapsed = 0f;


        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transObj.localScale = Vector2.Lerp(startScale, maxScale, timeElapsed / duration);
            yield return null;
        }

        timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transObj.localScale = Vector2.Lerp(maxScale, startScale, timeElapsed / duration);
            yield return null;
        }

        timeElapsed = 0f;
        duration /= 1.5F;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transObj.localScale = Vector2.Lerp(startScale, miniMaxScale, timeElapsed / duration);
            yield return null;
        }
        timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transObj.localScale = Vector2.Lerp(miniMaxScale, startScale, timeElapsed / duration);
            yield return null;
        }
        transObj.localScale = startScale;
    }

    /// <summary>
    /// Move object to defined position
    /// </summary>
    public static IEnumerator MoveTO(RectTransform transObj, Vector3 desirePos, float duration)
    {
        Vector3 startPos = transObj.anchoredPosition;
        float timeElapsed = 0f;
        Vector3 velocity = Vector3.zero;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transObj.anchoredPosition = Vector3.Lerp(startPos, desirePos, timeElapsed / duration);
            yield return new WaitForEndOfFrame();
        }
    }

    public static IEnumerator MoveTO(Transform transObj, Vector3 desirePos, float duration)
    {
        Vector3 startPos = transObj.position;
        float timeElapsed = 0f;
        Vector3 velocity = Vector3.zero;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transObj.position = Vector3.Lerp(startPos, desirePos, timeElapsed / duration);
            yield return new WaitForEndOfFrame();
        }
    }
}
