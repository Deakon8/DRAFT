using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class, that contains all of extension methods
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Make object non transparent
    /// </summary>
    public static void ColorNonTransparent(this Image img)
    {
        Color color = img.color;
        img.color = new Color(color.r, color.g, color.b, 1);
    }

    /// <summary>
    /// Make object non transparent
    /// </summary>
    public static void ColorNonTransparent(this Text txt)
    {
        Color color = txt.color;
        txt.color = new Color(color.r, color.g, color.b, 1);
    }

    /// <summary>
    /// Make object non transparent
    /// </summary>
    public static void ColorNonTransparent(this SpriteRenderer spriteRen)
    {
        Color color = spriteRen.color;
        spriteRen.color = new Color(color.r, color.g, color.b, 1);
    }

    /// <summary>
    /// Set position based on input x and y coordinated, but with own z
    /// </summary>
    public static void SetPos2D(this Transform trans,Vector3 posNew)
    {
        trans.position = new Vector3(posNew.x,posNew.y,trans.position.z);
    }


    /// <summary>
    /// Make object transparent
    /// </summary>
    public static void ColorTransparent(this SpriteRenderer spriteRen)
    {
        Color color = spriteRen.color;
        spriteRen.color = new Color(color.r, color.g, color.b, 0);
    }

    /// <summary>
    /// Make object transparent
    /// </summary>
    public static void ColorTransparent(this Image img)
    {
        Color color = img.color;
        img.color = new Color(color.r, color.g, color.b, 0);
    }

    /// <summary>
    /// Make object transparent
    /// </summary>
    public static void ColorTransparent(this Text txt)
    {
        Color color = txt.color;
        txt.color = new Color(color.r, color.g, color.b, 0);
    }
}