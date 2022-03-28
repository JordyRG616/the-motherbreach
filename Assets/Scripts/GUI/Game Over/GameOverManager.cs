using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer endgameTransitionPanel;
    [SerializeField] private Animator animator;
    [SerializeField] private string endgameMusicName;
    [SerializeField] private Volume globalVolume;
    private Material endgameMaterial;
    private WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.01f);
    private AudioManager audioManager;

    void Start()
    {
        endgameMaterial = new Material(endgameTransitionPanel.material);
        endgameTransitionPanel.material = endgameMaterial;

        endgameMaterial.SetTexture("_ScreenTex", GameManager.Main.endgamePic);
        audioManager = AudioManager.Main;

        StartCoroutine(Transition());
    }

    private IEnumerator Transition()
    {
        float step = 0;

        animator.enabled = true;
        
        audioManager.StopAllAudio();
        audioManager.RequestMusic(endgameMusicName);
        audioManager.SwitchMusicTracks("Special");

        while(step <= 1)
        {
            var _weight = Mathf.Lerp(0, 1, step);

            globalVolume.weight = _weight;

            var _step = Mathf.Lerp(-1, 1, step);
            endgameMaterial.SetFloat("_Transition", _step);

            step += 0.01f;
            yield return wait;
        }
    }

    public void Restart()
    {
        GameManager.Main.StartGameLoop();
    }

    public void Title()
    {
        Destroy(GameManager.Main.gameObject);
        Destroy(AudioManager.Main.gameObject);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
