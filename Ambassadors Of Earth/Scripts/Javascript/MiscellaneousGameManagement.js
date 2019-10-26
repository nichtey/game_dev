
//CHECK SAVING CODE, when game is restarted, shows Desolation instead of saved eventcard

#pragma strict
import System;
import System.Runtime.Serialization.Formatters.Binary;
import System.IO;

var Scripts: MainScripts;
var TestSaveCheck: boolean;
var LoadTestCheck: boolean;
var Components: ComponentReference3;
var SoundEffectsAudioPlayer: AudioSource;
var GlobalSoundEffectsVolume: float;
var SoundEffectClips = new AudioClip[10];
var MusicAudioPlayer: AudioSource;
var GlobalMusicVolume: float;
var MusicClips = new AudioClip[5];
var NoticeSprite: SpriteRenderer;
var NoticeText = new UnityEngine.UI.Text[3];
var NoticeSpriteCollection = new Sprite[5];
var NoticeCard: GameObject;
var NoticeCardAnim: Animation;
var FinalYearParticleEffect: GameObject;

var BackgroundDim: Animation;

function Start () {
    QualitySettings.vSyncCount = 1;
    Physics2D.gravity = new Vector3(0f,0f,0f);
}

function Update () {

    if (Application.platform == RuntimePlatform.Android)
    {
        if (Input.GetKey(KeyCode.Escape))
        {

            Application.Quit();
            // Insert Code Here (I.E. Load Scene, Etc)
            // OR Application.Quit();
 
            return;
        }
    }

    if (TestSaveCheck == true){
        TestSaveCheck = false;
        Save();
    }

    if (LoadTestCheck == true){
        LoadTestCheck = false;
        Load();
    }

}

function NewNotice(NoticeNumber: int, NoticeTitle: String, NoticeDescription: String, NoticeLabel: String){
    NoticeCard.SetActive(true);
    NoticeSprite.sprite = NoticeSpriteCollection[NoticeNumber-1];
    NoticeText[0].text = NoticeTitle;
    NoticeText[1].text = NoticeDescription;
    NoticeText[2].text = NoticeLabel;
    NoticeCardAnim.Play("DrawCard");
    PlaySoundEffect(SoundEffectClips[1]);
    yield WaitForSeconds(1.183f);
    Scripts.HandCardRotationScript.Important.NoClicking = false;
}

function NoticeRemoved(){
    FinalYearParticleEffect.SetActive(true);
    PlayMusic(MusicClips[4]);
    BackgroundDim.Play("DimBackground");
    NoticeCardAnim.Play("RemoveCard");
    yield WaitForSeconds(1.183f);
    Scripts.EventCardControlScript.EventCardManager();
    NoticeCard.SetActive(false);
}

var ex: Exception;

function DeleteData(){
    try
    {
        File.Delete(Application.persistentDataPath + "/playerinfo.dat");
    }
    catch (ex)
    {
        Debug.LogException(ex);
    }
}

function Save(){
    var bf = new BinaryFormatter();
    var file: FileStream;

    file = File.Create(Application.persistentDataPath + "/playerinfo.dat");
    //Debug.Log(Application.persistentDataPath + "/playerinfo.dat"); C:/Users/nicht/AppData/LocalLow/DefaultCompany/Earth Reclaimed V1_0/playerinfo.dat

    var data = new Playerdata();

    data.PositionInShuffle = Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle;
    data.SpecialCardActivated = Scripts.EventCardControlScript.Important.SpecialCardActivated;
    data.NumberOfShuffles =  Scripts.EventCardControlScript.Important.NumberOfShuffles;

    for (var i = 0; i<20; i++){
        data.EventCardOrder[i] = Scripts.EventCardControlScript.Important.EventCardOrder[i];
        if (Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[i].CardPriority != 0){
            data.HumanTech0CardPriority[i] = Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[i].CardPriority;
            data.SpriteDisplayIdentifier2[i] = Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[i].SpriteDisplayIdentifier[0];
            data.SpriteDisplayIdentifier3[i] = Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[i].SpriteDisplayIdentifier[1];
            data.CardIdentifier2[i] = Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[i].CardIdentifier[0];
            data.CardIdentifier3[i] = Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[i].CardIdentifier[1];
        }
    }

    for (i = 0; i<10;i++){
        data.CardsToAdd0[i] = Scripts.EventCardControlScript.Important.CardsToAddIn[i].TechLevel;
        data.CardsToAdd1[i] = Scripts.EventCardControlScript.Important.CardsToAddIn[i].Position;
        data.CardsToDestroy0[i] = Scripts.EventCardControlScript.Important.CardsToDestroy[i].TechLevel;
        data.CardsToDestroy1[i] = Scripts.EventCardControlScript.Important.CardsToDestroy[i].Position;
    }

    for (i = 0; i<30; i++){
        data.CardIdentifier0[i] = Scripts.EventCardControlScript.Important.CardToUse[i].CardIdentifier[0];
        data.CardIdentifier1[i] = Scripts.EventCardControlScript.Important.CardToUse[i].CardIdentifier[1];
        data.SpriteDisplayIdentifier0[i] = Scripts.EventCardControlScript.Important.CardToUse[i].SpriteDisplayIdentifier[0];
        data.SpriteDisplayIdentifier1[i] = Scripts.EventCardControlScript.Important.CardToUse[i].SpriteDisplayIdentifier[1];
    }

    for (i=0; i<5; i++){
        data.ResourceTypeCardCount[i] = Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[i];
    }

    bf.Serialize(file,data);
    file.Close();
}

var InstantiatedCard: GameObject;

function Load(){
    if (File.Exists(Application.persistentDataPath + "/playerinfo.dat")){
        var bf = new BinaryFormatter();
        var file: FileStream;
        file = File.Open(Application.persistentDataPath + "/playerinfo.dat", FileMode.Open);
        var data = bf.Deserialize(file) as Playerdata; 

        Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = data.PositionInShuffle;
        Scripts.EventCardControlScript.Important.SpecialCardActivated = data.SpecialCardActivated;
        Scripts.EventCardControlScript.Important.NumberOfShuffles = data.NumberOfShuffles;

        for (var i = 0; i<20; i++){
            Scripts.EventCardControlScript.Important.EventCardOrder[i] = data.EventCardOrder[i];
            if (data.HumanTech0CardPriority[i]!=0){ 
                Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[i].CardPriority = data.HumanTech0CardPriority[i];
                Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[i].SpriteDisplayIdentifier[0] = data.SpriteDisplayIdentifier2[i];
                Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[i].SpriteDisplayIdentifier[1] = data.SpriteDisplayIdentifier3[i];
                Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[i].CardIdentifier[0] = data.CardIdentifier2[i];
                Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[i].CardIdentifier[1] = data.CardIdentifier3[i];
            }
        }

        for (i = 0; i<10;i++){
            Scripts.EventCardControlScript.Important.CardsToAddIn[i].TechLevel =  data.CardsToAdd0[i];
            Scripts.EventCardControlScript.Important.CardsToAddIn[i].Position = data.CardsToAdd1[i];
            Scripts.EventCardControlScript.Important.CardsToDestroy[i].TechLevel = data.CardsToDestroy0[i];
            Scripts.EventCardControlScript.Important.CardsToDestroy[i].Position = data.CardsToDestroy1[i];
        }

        for (i = 0; i<30; i++){
            Scripts.EventCardControlScript.Important.CardToUse[i].CardIdentifier[0] = data.CardIdentifier0[i];
            Scripts.EventCardControlScript.Important.CardToUse[i].CardIdentifier[1] = data.CardIdentifier1[i];
            Scripts.EventCardControlScript.Important.CardToUse[i].SpriteDisplayIdentifier[0] = data.SpriteDisplayIdentifier0[i];
            Scripts.EventCardControlScript.Important.CardToUse[i].SpriteDisplayIdentifier[1] = data.SpriteDisplayIdentifier1[i];
        }

        for (i=0; i<5; i++){
            Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[i] = data.ResourceTypeCardCount[i];
        }

        var PreviousNumberOfCards = Components.HandCardRotationTransform.childCount -1; 
        for (var Delete = PreviousNumberOfCards-1; Delete > -1; Delete--){
            Destroy(Components.HandCardRotationTransform.GetChild(Delete).gameObject);
        }

        for (var life = 0; life < data.ResourceTypeCardCount[0]; life++){
            InstantiatedCard = Instantiate(Components.ResourceCardPrefabs[0]);
            InstantiatedCard.transform.parent = Components.HandCardRotationTransform;
            InstantiatedCard.transform.SetSiblingIndex (life);
            InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[life];
            InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[life];
        }
        
        for (var Resource = life; Resource < data.ResourceTypeCardCount[1]+life; Resource++){
            InstantiatedCard = Instantiate(Components.ResourceCardPrefabs[1]);
            InstantiatedCard.transform.parent = Components.HandCardRotationTransform;
            InstantiatedCard.transform.SetSiblingIndex (Resource);
            InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Resource];
            InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Resource];
        }

        for (var Faith = Resource; Faith < data.ResourceTypeCardCount[2]+Resource; Faith++){
            InstantiatedCard = Instantiate(Components.ResourceCardPrefabs[2]);
            InstantiatedCard.transform.parent = Components.HandCardRotationTransform;
            InstantiatedCard.transform.SetSiblingIndex (Faith);
            InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Faith];
            InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Faith];
        }

        for (var Human = Faith; Human < data.ResourceTypeCardCount[3]+Faith; Human++){
            InstantiatedCard = Instantiate(Components.ResourceCardPrefabs[3]);
            InstantiatedCard.transform.parent = Components.HandCardRotationTransform;
            InstantiatedCard.transform.SetSiblingIndex (Human);
            InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Human];
            InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Human];
        }

        for (var Chaos = Human; Chaos < data.ResourceTypeCardCount[4]+Human; Chaos++){
            InstantiatedCard = Instantiate(Components.ResourceCardPrefabs[4]);
            InstantiatedCard.transform.parent = Components.HandCardRotationTransform;
            InstantiatedCard.transform.SetSiblingIndex (Chaos);
            InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Chaos];
            InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Chaos];
        }

        Scripts.HandCardRotationScript.Important.NumberOfCards= data.ResourceTypeCardCount[0]+data.ResourceTypeCardCount[1]+data.ResourceTypeCardCount[2]+data.ResourceTypeCardCount[3]+data.ResourceTypeCardCount[4]; 
        Scripts.HandCardRotationScript.LoadSavedCards();

        file.Close();

        Scripts.EventCardControlScript.EventCardManager();
    }
}

var CurrentSoundEffect: AudioClip;

function PlaySoundEffect(SoundClip:AudioClip){
    SoundEffectsAudioPlayer.volume = GlobalSoundEffectsVolume;
    CurrentSoundEffect = SoundClip;
    SoundEffectsAudioPlayer.clip = SoundClip;
    SoundEffectsAudioPlayer.Play();
}

var SoundEffectSlider: UnityEngine.UI.Slider;

function UpdateSoundEffectVolume(){
    GlobalSoundEffectsVolume = SoundEffectSlider.value;
    SoundEffectsAudioPlayer.volume = GlobalSoundEffectsVolume;
    PlaySoundEffect(SoundEffectClips[0]);
}

function PlayMusic (SoundClip: AudioClip){
    MusicAudioPlayer.volume = GlobalMusicVolume;
    MusicAudioPlayer.clip = SoundClip;
    MusicAudioPlayer.Play();
}

var MusicVolumeSlider: UnityEngine.UI.Slider;

function UpdateMusicVolume(){
    GlobalMusicVolume = MusicVolumeSlider.value;
    MusicAudioPlayer.volume = GlobalMusicVolume;
}

@SerializeField
class Playerdata{

    //EventCardControl Shuffle Count
    var PositionInShuffle: int;
    var NumberOfShuffledCards: int;
    
    //Event CardControl Important
    var EventCardOrder = new int[20];
    var SpecialCardActivated: int;
    var NumberOfShuffles:int;

    var CardsToAdd0 = new int[10];
    var CardsToAdd1 = new int[10];
    var CardsToDestroy0 = new int[10];
    var CardsToDestroy1 = new int[10];

    var CardIdentifier0 = new int[30];
    var CardIdentifier1 = new int[30];
    var SpriteDisplayIdentifier0 = new int[30];
    var SpriteDisplayIdentifier1 = new int[30];

    //EventCardControl AllEventCards
    var SpriteDisplayIdentifier2 = new int[20];
    var SpriteDisplayIdentifier3 = new int[20];
    var CardIdentifier2 = new int[20];
    var CardIdentifier3 = new int[20];
    var HumanTech0CardPriority = new int [20];

    //HandCardRotation Components
    var ResourceTypeCardCount = new int[5];

}

class ComponentReference3{
    var HandCardRotationTransform: Transform;
    var ResourceCardPrefabs = new GameObject[5];
}