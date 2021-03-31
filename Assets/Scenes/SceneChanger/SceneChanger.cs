using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Animator animator;

    string seceneToLoad = "";

    #region Singleton

    public static SceneChanger instance;

    void Awake()
    {

        if (instance != null)
        {
            Debug.LogWarning("More than one of instance of SceneChanger found!");
            return;
        }
        instance = this;
    }

    #endregion

    public void FadeToMainScene()
    {
        seceneToLoad = "MainScene";
        animator.SetTrigger("FadeOut");
    }

    public void FadeToEazyScene()
    {
        seceneToLoad = "EazyScene";
        animator.SetTrigger("FadeOut");
    }

    public void FadeToMenu()
    {
        seceneToLoad = "MainMenu";
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(seceneToLoad);
    }
}
