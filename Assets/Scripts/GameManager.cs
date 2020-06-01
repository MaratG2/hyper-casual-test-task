using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject buttonRestart;
    [SerializeField] private Image progressBar;
    [SerializeField] private GameObject cubeObject;
    [SerializeField] private GameObject cubeParticles;
    [SerializeField] private Animator BlackScreenAnim;

    private CubeController cubeControllerScript;
    private bool win;   
    private float progressRaw = 0f; //Progress in degrees
    private float progress = 0f; //Progress in quantity of rotations
    private int partclesState = 0; //State machine, could be called oldProgress, to detect the moment of their difference
    private bool inRestartCR;

    void Start()
    {
        cubeControllerScript = cubeObject.GetComponent<CubeController>();
    }

    void LateUpdate()
    {
        progressRaw += cubeControllerScript.deltaRotationY;
        progress = progressRaw / 360f;
        progressBar.fillAmount = progress / 5;
        if (progress >= 5 && !win)
            Victory();
        if(partclesState < UnityEngine.Mathf.FloorToInt(progress))
        {
            partclesState++;
            cubeObject.transform.localScale = new Vector3(1f - partclesState/5f, 1f - partclesState/5f, 1f - partclesState/5f);
            cubeParticles.SetActive(true);
            cubeParticles.GetComponent<ParticleSystem>().Play();
        }
    }

    void Victory()
    {
        win = true;
        cubeObject.GetComponent<CubeController>().win = true;  
        GameEnd();
    }

    private void GameEnd()
    {
        text.enabled = true;
        text.gameObject.GetComponent<Animator>().SetTrigger("Appear");
        buttonRestart.GetComponent<Animator>().SetTrigger("In");
        buttonRestart.GetComponent<Image>().raycastTarget = true; //Enable interaction with button
    }

    public void RestartClick()
    {
        if (!inRestartCR) //Make sure to start coroutine only once
            StartCoroutine(RestartClickCR());
    }

    private IEnumerator RestartClickCR()
    {
        inRestartCR = true;
        BlackScreenAnim.SetTrigger("FadeIn");
        text.gameObject.GetComponent<Animator>().SetTrigger("Disappear");
        buttonRestart.GetComponent<Animator>().SetTrigger("Out");
        yield return new WaitForSecondsRealtime(1f); //Pause for animations' end
        SceneManager.LoadScene(0);
    }
}
