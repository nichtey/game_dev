using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Advertisements;

public class MiscellaneousGameManagementCS : MonoBehaviour {

    public bool TestSaveCheck;
    public bool LoadTestCheck;
    public CameraControlCS.ScriptReference Scripts;
    public ComponentReference3 Components;
    public AudioSource SoundEffectsAudioPlayer;
    public float GlobalSoundEffectsVolume;
    public AudioClip[] SoundEffectClips = new AudioClip[10];
    public AudioSource MusicAudioPlayer;
    public float GlobalMusicVolume;
    public AudioClip[] MusicClips = new AudioClip[5];
    public SpriteRenderer NoticeSprite;
    public UnityEngine.UI.Text[] NoticeText = new UnityEngine.UI.Text[3];
    public Sprite[] NoticeSpriteCollection = new Sprite[5];
    public GameObject NoticeCard;
    public Animation NoticeCardAnim;
    public GameObject FinalYearParticleEffect;
    public Animation BackgroundDim;
    public AudioClip CurrentSoundEffect;
    public UnityEngine.UI.Slider SoundEffectSlider;
    public UnityEngine.UI.Slider MusicVolumeSlider;

    void Start () {
        QualitySettings.vSyncCount = 1;
        Physics2D.gravity = new Vector3(0f, 0f, 0f);
        Load();
    }
	
	void Update () {

        if (Application.platform == RuntimePlatform.Android) {
            if (Input.GetKey(KeyCode.Escape)) {

                if (Camera.main.transform.position.z == -23.8f)
                {
                    Scripts.MainMenuControllerScript.ClickQuit();
                    return;
                }

                else if (Camera.main.transform.position.z == 1.84f)
                {
                    Scripts.UIControllerScript.PauseClicked();
                }
            }
        }

        if (TestSaveCheck == true) {
            TestSaveCheck = false;
            Save();
        }

        if (LoadTestCheck == true) {
            LoadTestCheck = false;
            Load();
        }
    }

    public void ShowVideoAdvertisement()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    public IEnumerator NewNotice(int NoticeNumber, string NoticeTitle, string NoticeDescription, string NoticeLabel) {
        NoticeCard.SetActive(true);
        NoticeSprite.sprite = NoticeSpriteCollection[NoticeNumber - 1];
        NoticeText[0].text = NoticeTitle;
        NoticeText[1].text = NoticeDescription;
        NoticeText[2].text = NoticeLabel;
        NoticeCardAnim.Play("DrawCard");
        PlaySoundEffect(SoundEffectClips[1]);
        yield return new WaitForSeconds(1.183f);
        Scripts.HandCardRotationScript.Important.NoClicking = false;
    }

    public IEnumerator NoticeRemoved() {

        if (NoticeText[0].text == "Final Year")
        {
            FinalYearParticleEffect.SetActive(true);
            PlayMusic(MusicClips[4]);
            BackgroundDim.Play("DimBackground");
        }

        NoticeCardAnim.Play("RemoveCard");
        yield return new WaitForSeconds(1.183f);
        StartCoroutine(Scripts.EventCardControlScript.EventCardManager());
        NoticeCard.SetActive(false);
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(Application.persistentDataPath + "/playerinfo.dat", FileMode.Create);
        DataToSave data = new DataToSave();

        for (int i = 0; i < 3; i++)
        {
            data.ClearedLevelCheck = Scripts.MainMenuControllerScript.PlayerData.ClearedLevelCheck;
            data.ShowUnlockAnimCheck = Scripts.MainMenuControllerScript.PlayerData.ShowUnlockAnimCheck;
        }

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerinfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = new FileStream(Application.persistentDataPath + "/playerinfo.dat", FileMode.Open);
            DataToSave data = bf.Deserialize(file) as DataToSave;

            for (int i = 0; i < 3; i++)
            {
                Scripts.MainMenuControllerScript.PlayerData.ClearedLevelCheck = data.ClearedLevelCheck;
                Scripts.MainMenuControllerScript.PlayerData.ShowUnlockAnimCheck = data.ShowUnlockAnimCheck;
            }

            file.Close();
        }
    }

    public void DeleteData() {
        try {
            File.Delete(Application.persistentDataPath + "/playerinfo.dat");
        }
        catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    public void PlayMusic(AudioClip SoundClip) {
        MusicAudioPlayer.volume = GlobalMusicVolume;
        MusicAudioPlayer.clip = SoundClip;
        MusicAudioPlayer.Play();
    }

    public void UpdateMusicVolume(){
        GlobalMusicVolume = MusicVolumeSlider.value;
        MusicAudioPlayer.volume = GlobalMusicVolume;
    }

    public void PlaySoundEffect(AudioClip SoundClip) {
        SoundEffectsAudioPlayer.volume = GlobalSoundEffectsVolume;
        CurrentSoundEffect = SoundClip;
        SoundEffectsAudioPlayer.clip = SoundClip;
        SoundEffectsAudioPlayer.Play();
    }

    public void UpdateSoundEffectVolume()
    {
        GlobalSoundEffectsVolume = SoundEffectSlider.value;
        SoundEffectsAudioPlayer.volume = GlobalSoundEffectsVolume;
        PlaySoundEffect(SoundEffectClips[0]);
    }

    [Serializable]
    public class ComponentReference3 {
        public Transform HandCardRotationTransform;
        public GameObject[] ResourceCardPrefabs = new GameObject[5];
    }

    [Serializable]
    public class DataToSave
    {
        public bool[] ClearedLevelCheck = new bool[3];
        public bool[] ShowUnlockAnimCheck = new bool[3];
    }
}
