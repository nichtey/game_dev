using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlCS : MonoBehaviour {

    private float FadeLerp = 1.0f;
    private float AlphaInitialValue = 1.0f;
    private float AlphaTargetValue = 0.0f;

    public GameObject MainCamera;
    public GameObject FadeBlackGameObject;
    public SpriteRenderer BlackSpriteRenderer;
    public SpriteRenderer LoadingSpriteRenderer;
    public Sprite[] SpritesToLoad;
    public GameObject[] GameObjectsToLoad;
    public GameObject[] BlinkLids = new GameObject[3];
    public Animation Anim;
    public RenderTexture MainMenuRenderTexture;

    public UnityEngine.Video.VideoPlayer MainMenuVideo;
    public UnityEngine.Video.VideoPlayer EndSceneLoseVideo;
    public UnityEngine.Video.VideoPlayer EndSceneWinVideo;
    public UnityEngine.Video.VideoClip[] VideoClips = new UnityEngine.Video.VideoClip[5];
    public GameObject ParticleEffects;

    public ScriptReference Scripts;
    public EndGameRemarks EndRemarks;

    void Start() {

        MainMenuVideo.Prepare();

        for (int i = 0; i < SpritesToLoad.Length; i++) {
            LoadingSpriteRenderer.sprite = SpritesToLoad[i];
        }
        LoadingSpriteRenderer.sprite = null;

        Scripts.MiscellaneousGameManagementScript.PlayMusic(Scripts.MiscellaneousGameManagementScript.MusicClips[0]);

        StartCoroutine(LoadVideo());
    }


    public IEnumerator LoadVideo() {

        UnityEngine.Video.VideoPlayer VideoPrepared = MainMenuVideo;
        while (VideoPrepared.isPrepared == false) {
            yield return null;
        }

        MainMenuVideo.Play();
        Color color = BlackSpriteRenderer.color;
        color.a = 1;
        BlackSpriteRenderer.color = color;
        AlphaTargetValue = 0;
        FadeLerp = 0;

        yield return new WaitForSeconds(2.5f);
        ParticleEffects.SetActive(true);
        //Scripts.ParticleEffectControllerScript.BigExplosion.Play();
        //Scripts.ParticleEffectControllerScript.Destroy3Cards();
    }

    void Update() {

        if (FadeLerp < 1) {
            FadeLerp += Time.deltaTime / 0.8f;
            Color color = BlackSpriteRenderer.color;
            color.a = Mathf.Lerp(AlphaInitialValue, AlphaTargetValue, FadeLerp);
            BlackSpriteRenderer.color = color;

            if (FadeLerp > 1) {
                AlphaInitialValue = AlphaTargetValue;
                if (AlphaTargetValue == 0) BlackSpriteRenderer.enabled = false;
            }
        }
    }

    public IEnumerator FadeThroughBlack(float TimeDelay) {
        BlackSpriteRenderer.enabled = true;
        AlphaTargetValue = 1;
        FadeLerp = 0;
        yield return new WaitForSeconds(1 + TimeDelay);
        AlphaTargetValue = 0;
        FadeLerp = 0;
    }

    public IEnumerator LoseGame(string HeaderText, string BodyText, int LevelNo) {

        Scripts.EndSceneControllerScript.LoseEndScene.SetActive(true);
        EndSceneLoseVideo.Prepare();
        StartCoroutine(FadeThroughBlack(0.25f));
        yield return new WaitForSeconds(0.8f);
        Scripts.MiscellaneousGameManagementScript.PlayMusic(Scripts.MiscellaneousGameManagementScript.MusicClips[2]);
        Vector3 CameraTransform = MainCamera.transform.position;
        CameraTransform.z = 33.1f;
        MainCamera.transform.position = CameraTransform;
        EndSceneLoseVideo.Play();

        if (LevelNo != 0)
        {
            Scripts.EndSceneControllerScript.EndSceneHeader.text = EndRemarks.LoseTitle[LevelNo - 1];
            Scripts.EndSceneControllerScript.EndSceneDescription.text = EndRemarks.LoseDescription[LevelNo - 1];
            Scripts.EndSceneControllerScript.EndSceneDescription.text = Scripts.EndSceneControllerScript.EndSceneDescription.text.Replace("NWL", "\n");
        }

        else
        {
            Scripts.EndSceneControllerScript.EndSceneHeader.text = HeaderText;
            Scripts.EndSceneControllerScript.EndSceneDescription.text = BodyText;
        }

    }

    public IEnumerator WinGame(int LevelNo) {

        Scripts.EndSceneControllerScript.WinEndScene.SetActive(true);
        EndSceneWinVideo.Prepare();
        StartCoroutine(FadeThroughBlack(0.25f));
        yield return new WaitForSeconds(0.8f);
        Scripts.MiscellaneousGameManagementScript.PlayMusic(Scripts.MiscellaneousGameManagementScript.MusicClips[3]);
        Vector3 CameraTransform = MainCamera.transform.position;
        CameraTransform.z = 33.1f;
        MainCamera.transform.position = CameraTransform;
        EndSceneWinVideo.Play();

        Scripts.EndSceneControllerScript.EndSceneHeader.text = EndRemarks.WinTitle[LevelNo-1];
        Scripts.EndSceneControllerScript.EndSceneDescription.text = EndRemarks.WinDescription[LevelNo-1];
        Scripts.EndSceneControllerScript.EndSceneDescription.text = Scripts.EndSceneControllerScript.EndSceneDescription.text.Replace("NWL", "\n");
        Scripts.MainMenuControllerScript.PlayerData.ShowUnlockAnimCheck[LevelNo] = true;
        Scripts.MiscellaneousGameManagementScript.Save();

        if (LevelNo == 2 || LevelNo == 3)
        {
            Scripts.MiscellaneousGameManagementScript.ShowVideoAdvertisement();
        }
    }

    public void Blink() {
        Anim.Play();
    }

    [System.Serializable]
    public class ScriptReference {
        public EventCardControlCS EventCardControlScript;
        public ParticleEffectControllerCS ParticleEffectControllerScript;
        public MiscellaneousGameManagementCS MiscellaneousGameManagementScript;
        public EndSceneControllerCS EndSceneControllerScript;
        public CameraControlCS CameraControlScript;
        public MainMenuControllerCS MainMenuControllerScript;
        public UIControllerCS UIControllerScript;
        public HandCardRotationCS HandCardRotationScript;
        public OptionCardMovementCS[] OptionCardScripts = new OptionCardMovementCS[3];
        public OptionClickControlCS OptionClickControlScript;
        public ShufflingCardAnimationCS ShufflingCardAnimationScript;
        public NatureControllerCS NatureControllerScript;
        public HumanControllerCS HumanControllerScript;
        public MainMenuCampaignDragCS MainMenuCampaignDragScript;
    }

    [System.Serializable]
    public class EndGameRemarks
    {
        public string[] WinTitle = new string[3];
        public string[] WinDescription = new string[3];
        public string[] LoseTitle = new string[3];
        public string[] LoseDescription = new string[3];
    }
}
