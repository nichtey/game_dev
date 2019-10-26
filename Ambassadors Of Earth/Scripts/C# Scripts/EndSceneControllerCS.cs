using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneControllerCS : MonoBehaviour {

    public CameraControlCS.ScriptReference Scripts;
    public UnityEngine.Video.VideoPlayer EndSceneVideoPlayer;
    public UnityEngine.Video.VideoPlayer EndSceneWinVideo;
    public UnityEngine.UI.Text EndSceneHeader;
    public UnityEngine.UI.Text EndSceneDescription;
    public GameObject WinEndScene;
    public GameObject LoseEndScene;

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void ReturnToMainMenuLose()
    {
        Scripts.MiscellaneousGameManagementScript.PlayMusic(Scripts.MiscellaneousGameManagementScript.MusicClips[0]);

        if (Scripts.ParticleEffectControllerScript.WeatherInPlay != 0)
        {
            Scripts.ParticleEffectControllerScript.Weather[Scripts.ParticleEffectControllerScript.WeatherInPlay - 1].Stop();
            Scripts.ParticleEffectControllerScript.WeatherInPlay = 0;
        }

        StartCoroutine(Scripts.CameraControlScript.FadeThroughBlack(0.25f));
        Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[0].SetActive(true);
        Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[1].SetActive(true);
        Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[2].SetActive(true);
        Scripts.CameraControlScript.MainMenuVideo.clip = Scripts.CameraControlScript.VideoClips[1];
        Scripts.CameraControlScript.MainMenuVideo.frame = 0;
        StartCoroutine(MoveCameraClickMainMenu());
        StartCoroutine(ResetEndScene());
    }

    public void ReturnToMainMenuWon()
    {
        Scripts.MiscellaneousGameManagementScript.PlayMusic(Scripts.MiscellaneousGameManagementScript.MusicClips[0]);
        if (Scripts.ParticleEffectControllerScript.WeatherInPlay != 0)
        {
            Scripts.ParticleEffectControllerScript.Weather[Scripts.ParticleEffectControllerScript.WeatherInPlay - 1].Stop();
            Scripts.ParticleEffectControllerScript.WeatherInPlay = 0;
        }

        StartCoroutine(Scripts.CameraControlScript.FadeThroughBlack(0.25f));
        Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[0].SetActive(true);
        Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[1].SetActive(true);
        Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[2].SetActive(true);
        Scripts.CameraControlScript.MainMenuVideo.clip = Scripts.CameraControlScript.VideoClips[1];
        Scripts.CameraControlScript.MainMenuVideo.frame = 0;
        StartCoroutine(MoveCameraClickMainMenu());
        StartCoroutine(ResetEndScene());
    }

    public void ReturnToMainMenuLevelSelectionLose()
    {
        Scripts.MiscellaneousGameManagementScript.PlayMusic(Scripts.MiscellaneousGameManagementScript.MusicClips[0]);
        if (Scripts.ParticleEffectControllerScript.WeatherInPlay != 0)
        {
            Scripts.ParticleEffectControllerScript.Weather[Scripts.ParticleEffectControllerScript.WeatherInPlay - 1].Stop();
            Scripts.ParticleEffectControllerScript.WeatherInPlay = 0;
        }

        StartCoroutine(Scripts.CameraControlScript.FadeThroughBlack(0.25f));
        Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[0].SetActive(true);
        Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[1].SetActive(true);
        Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[2].SetActive(true);
        Scripts.CameraControlScript.MainMenuVideo.clip = Scripts.CameraControlScript.VideoClips[1];
        Scripts.CameraControlScript.MainMenuVideo.frame = 0;
        StartCoroutine(MoveCameraClickMainMenu());
        StartCoroutine(ResetEndScene());

        Scripts.MainMenuControllerScript.Layers[0].SetActive(false);    //Level Selection Page
        Scripts.MainMenuControllerScript.Layers[1].SetActive(true);
        Scripts.MainMenuControllerScript.StartingImages.SetActive(false);
        Scripts.MainMenuControllerScript.MainMenuCanvas[0].SetActive(false);
        Scripts.MainMenuControllerScript.MainMenuCanvas[1].SetActive(true);
        Scripts.MainMenuControllerScript.CurrentPage = 3;
    }

    public void ReturnToMainMenuLevelSelectionWin()
    {
        Scripts.MiscellaneousGameManagementScript.PlayMusic(Scripts.MiscellaneousGameManagementScript.MusicClips[0]);
        if (Scripts.ParticleEffectControllerScript.WeatherInPlay != 0)
        {
            Scripts.ParticleEffectControllerScript.Weather[Scripts.ParticleEffectControllerScript.WeatherInPlay - 1].Stop();
            Scripts.ParticleEffectControllerScript.WeatherInPlay = 0;
        }

        StartCoroutine(Scripts.CameraControlScript.FadeThroughBlack(0.25f));
        Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[0].SetActive(true);
        Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[1].SetActive(true);
        Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[2].SetActive(true);
        Scripts.CameraControlScript.MainMenuVideo.clip = Scripts.CameraControlScript.VideoClips[1];
        Scripts.CameraControlScript.MainMenuVideo.frame = 0;
        StartCoroutine(MoveCameraClickMainMenu());
        StartCoroutine(ResetEndScene());

        Scripts.MainMenuControllerScript.Layers[0].SetActive(false);    //Level Selection Page
        Scripts.MainMenuControllerScript.Layers[1].SetActive(true);
        Scripts.MainMenuControllerScript.StartingImages.SetActive(false);
        Scripts.MainMenuControllerScript.MainMenuCanvas[0].SetActive(false);
        Scripts.MainMenuControllerScript.MainMenuCanvas[1].SetActive(true);
        Scripts.MainMenuControllerScript.CurrentPage = 3;
        StartCoroutine(DelayedDisplay());
    }


    public IEnumerator DelayedDisplay()
    {
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < Scripts.MainMenuControllerScript.PlayerData.ShowUnlockAnimCheck.Length; i++)
        {
            if (Scripts.MainMenuControllerScript.PlayerData.ShowUnlockAnimCheck[i] == true && Scripts.MainMenuControllerScript.PlayerData.ClearedLevelCheck[i] == false)
            {
                Scripts.MainMenuCampaignDragScript.JumpToSelected(i);
                StartCoroutine(Scripts.MainMenuControllerScript.ShowUnlockLevelAnimation(i));
                break;
            }
        }
    }

    public IEnumerator ResetEndScene()
    {
        yield return new WaitForSeconds(0.8f);
        EndSceneWinVideo.Stop();
        WinEndScene.SetActive(false);
        LoseEndScene.SetActive(false);
    }

    public IEnumerator MoveCameraClickMainMenu()
    {
        yield return new WaitForSeconds(0.8f);
        Vector3 Pos = new Vector3(-0.6f, 52.8f, -23.8f);
        Camera.main.transform.position = Pos;
        Scripts.CameraControlScript.MainMenuVideo.Play();
    }
}
