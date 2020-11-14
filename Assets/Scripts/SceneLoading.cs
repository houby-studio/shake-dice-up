using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoading : MonoBehaviour
{
    public GameObject progressBar;
    public float rotateSpeed;

    private RectTransform rectComponent;

    // Start is called before the first frame update
    void Start()
    {
        rectComponent = progressBar.GetComponent<RectTransform>();
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        // Start loading game
        SceneManager.LoadSceneAsync(1);
        // Spin the spinner baby
        while (true)
        {
            rectComponent.Rotate(0.0f, 0.0f, rotateSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
