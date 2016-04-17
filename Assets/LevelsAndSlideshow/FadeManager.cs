using UnityEngine;
using System.Collections;

public class FadeManager: MonoBehaviour
{
    public static FadeManager instance;

    public float speed = 1f;
    float alpha = 0;

    public static bool Fading;
    public static bool Unfading;

    public delegate void act();
    SpriteRenderer sr;

    public static void Fade(act postFade)
    {
        if (instance == null)
        {
            postFade();
            return;
        }
        
        Fading = true;
        Unfading = false;

        instance.alpha = 0f;
        instance.StartCoroutine(instance.WaitForFade(postFade));
    }

    public IEnumerator WaitForFade(act postFade)
    {
        while (Fading)
            yield return new WaitForEndOfFrame();

        postFade();
    }

    public static void Unfade(act postUnfade)
    {
        if (instance == null)
        {
            postUnfade();
            return;
        }

        Fading = false;
        Unfading = true;

        instance.alpha = 1f;
        instance.StartCoroutine(instance.WaitForUnfade(postUnfade));
    }

    public IEnumerator WaitForUnfade(act postUnfade)
    {
        while (Unfading)
            yield return new WaitForEndOfFrame();

        if (postUnfade != null)
            postUnfade();
    }

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            sr = GetComponent<SpriteRenderer>();
            DontDestroyOnLoad(this);
            instance = this;
            Unfade(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Unfading)
        {
            alpha = alpha - speed * Time.deltaTime;
            if (alpha > 0)
                sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, alpha);
            else
            {
                sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, 0f);
                Unfading = false;
            }
        }
        if (Fading)
        {
            alpha = alpha + speed * Time.deltaTime;
            if (alpha < 1)
                sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, alpha);
            else
            {
                sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, 1f);
                Fading = false;
            }
        }
    }
}