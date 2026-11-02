using System.Collections;
using UnityEngine;

public class Shell : MonoBehaviour
{
    private float lifeTime = 5f;
    private Material mat;
    private Color originalColor;
    private float fadePercent;
    private float deathTime;
    private bool fading;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        originalColor = mat.color;
        deathTime = Time.time + lifeTime;
        StartCoroutine("Fade");
    }

    IEnumerator Fade()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            if (fading)
            {
                fadePercent += Time.deltaTime;
                mat.color = Color.Lerp(originalColor,Color.clear, fadePercent);

                if (fadePercent>= 1f)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (Time.time > deathTime)
                {
                    fading = true;
                }
            }
        }


    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Ground")
        {
            GetComponent<Rigidbody>().Sleep();
        }

    }

}
