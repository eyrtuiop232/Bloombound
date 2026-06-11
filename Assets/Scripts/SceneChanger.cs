using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public Image FadeImage;
    public string TargetScene;
    public float FadeDuration = 0.5f;

    public void Load()
    {
        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        FadeImage.gameObject.SetActive(true);

        Color c = FadeImage.color;
        c.a = 0f;
        FadeImage.color = c;

        float elapsed = 0f;
        while (elapsed < FadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.SmoothStep(0f, 1f, elapsed / FadeDuration);
            FadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        FadeImage.color = c;

        SceneManager.LoadScene(TargetScene);
    }
}
