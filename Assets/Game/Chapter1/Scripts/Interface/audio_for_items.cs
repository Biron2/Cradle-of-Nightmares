using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio_for_items : MonoBehaviour
{
    [SerializeField] public AudioSource[] determineAudio;

    [SerializeField] public AudioClip[] determineClips;


    [SerializeField] private AudioSource putItBackSnd;
    public void PlaySounds(string str)
    {
        switch (str)
        {
            case "Postcard_2":
                putItBackSnd.PlayOneShot(determineClips[13]);
                break;
            case "postCardZoom_2":
                putItBackSnd.PlayOneShot(determineClips[13]);
                break;
            case "Postcard_1":
                putItBackSnd.PlayOneShot(determineClips[13]);
                break;
            case "postCardZoom_1":
                putItBackSnd.PlayOneShot(determineClips[13]);
                break;
            case "growGame":
                putItBackSnd.PlayOneShot(determineClips[0]);
                break;
            case "growGame2":
                putItBackSnd.PlayOneShot(determineClips[0]);
                break;
            case "growGame3horror":
                putItBackSnd.PlayOneShot(determineClips[0]);
                break;
            case "growGame4empty":
                putItBackSnd.PlayOneShot(determineClips[0]);
                break;
            case "growGame5empty":
                putItBackSnd.PlayOneShot(determineClips[0]);
                break;
            case "safeInside":
                putItBackSnd.PlayOneShot(determineClips[1]);
                break;
            case "pictureOpen":
                putItBackSnd.PlayOneShot(determineClips[6]);
                break;
            case "pictureClose":
                putItBackSnd.PlayOneShot(determineClips[7]);
                break;
            case "grow":
                putItBackSnd.PlayOneShot(determineClips[3]);
                break;
            case "flower":
                putItBackSnd.PlayOneShot(determineClips[3]);
                break;
            case "flower2":
                putItBackSnd.PlayOneShot(determineClips[3]);
                break;
            case "flowerHorror":
                putItBackSnd.PlayOneShot(determineClips[3]);
                break;
            case "flower3":
                putItBackSnd.PlayOneShot(determineClips[3]);
                break;
            case "safeClosed":
                putItBackSnd.PlayOneShot(determineClips[2]);
                break;
            case "closetOpened11":
                putItBackSnd.PlayOneShot(determineClips[4]);
                break;
            case "closetOpened2":
                putItBackSnd.PlayOneShot(determineClips[4]);
                break;
            case "InsideCloset":
                putItBackSnd.PlayOneShot(determineClips[5]);
                break;
            case "InsideCloset2":
                putItBackSnd.PlayOneShot(determineClips[5]);
                break;
            case "Piano":
                putItBackSnd.PlayOneShot(determineClips[6]);
                break;
            case "PianoGame":
                putItBackSnd.PlayOneShot(determineClips[7]);
                break;
            case "RatOpen":
                putItBackSnd.PlayOneShot(determineClips[6]);
                break;
            case "RatClose":
                putItBackSnd.PlayOneShot(determineClips[7]);
                break;
            case "clown":
                putItBackSnd.PlayOneShot(determineClips[6]);
                break;
            case "clownMiniGame":
                putItBackSnd.PlayOneShot(determineClips[7]);
                break;
            case "SwitchPicture":
                putItBackSnd.PlayOneShot(determineClips[6]);
                break;
            case "insideSwitchPicture":
                putItBackSnd.PlayOneShot(determineClips[7]);
                break;
            case "lockedSpecial":
                putItBackSnd.PlayOneShot(determineClips[6]);
                break;
            case "SpecialKeyGame":
                putItBackSnd.PlayOneShot(determineClips[7]);
                break;
            case "note1":
                putItBackSnd.PlayOneShot(determineClips[8]);
                break;
            case "note2":
                putItBackSnd.PlayOneShot(determineClips[8]);
                break;
            case "note3":
                putItBackSnd.PlayOneShot(determineClips[8]);
                break;
            case "insideNote1":
                putItBackSnd.PlayOneShot(determineClips[9]);
                break;
            case "insideNote2":
                putItBackSnd.PlayOneShot(determineClips[9]);
                break;
            case "insideNote3":
                putItBackSnd.PlayOneShot(determineClips[9]);
                break;
            case "safeCodePaperOpen":
                putItBackSnd.PlayOneShot(determineClips[8]);
                break;
            case "safeCode":
                putItBackSnd.PlayOneShot(determineClips[9]);
                break;
            case "Shkatulochka":
                putItBackSnd.PlayOneShot(determineClips[6]);
                break;
            case "NooberGame":
                putItBackSnd.PlayOneShot(determineClips[7]);
                break;
            case "OpenTip1":
                putItBackSnd.PlayOneShot(determineClips[10]);
                break;
            case "OpenTip2":
                putItBackSnd.PlayOneShot(determineClips[10]);
                break;
            case "OpenTip3":
                putItBackSnd.PlayOneShot(determineClips[10]);
                break;
            case "OpenTip4":
                putItBackSnd.PlayOneShot(determineClips[10]);
                break;
            case "OpenTip5":
                putItBackSnd.PlayOneShot(determineClips[10]);
                break;
            case "OpenTip6":
                putItBackSnd.PlayOneShot(determineClips[10]);
                break;
            case "OpenTip7":
                putItBackSnd.PlayOneShot(determineClips[10]);
                break;
            case "OpenTip8":
                putItBackSnd.PlayOneShot(determineClips[10]);
                break;
            case "TipFile1":
                putItBackSnd.PlayOneShot(determineClips[11]);
                break;
            case "TipFile2":
                putItBackSnd.PlayOneShot(determineClips[11]);
                break;
            case "TipFile3":
                putItBackSnd.PlayOneShot(determineClips[11]);
                break;
            case "TipFile4":
                putItBackSnd.PlayOneShot(determineClips[11]);
                break;
            case "TipFile5":
                putItBackSnd.PlayOneShot(determineClips[11]);
                break;
            case "TipFile6":
                putItBackSnd.PlayOneShot(determineClips[11]);
                break;
            case "TipFile7":
                putItBackSnd.PlayOneShot(determineClips[11]);
                break;
            case "TipFile8":
                putItBackSnd.PlayOneShot(determineClips[11]);
                break;
            case "nightstand_closed":
                putItBackSnd.PlayOneShot(determineClips[4]);
                break;
            case "bucket":
                putItBackSnd.PlayOneShot(determineClips[6]);
                break;
            case "carpet":
                putItBackSnd.PlayOneShot(determineClips[6]);
                break;
            case "clown_balloon":
                putItBackSnd.PlayOneShot(determineClips[6]);
                break;
            case "clownBalloon":
                putItBackSnd.PlayOneShot(determineClips[6]);
                break;
            case "picture4":
                putItBackSnd.PlayOneShot(determineClips[6]);
                break;
            case "Plants":
                putItBackSnd.PlayOneShot(determineClips[6]);
                break;
            case "table_with_photos":
                putItBackSnd.PlayOneShot(determineClips[6]);
                break;
            case "bucketInside":
                putItBackSnd.PlayOneShot(determineClips[7]);
                break;
            case "clownBalloonsGame":
                putItBackSnd.PlayOneShot(determineClips[7]);
                break;
            case "flowersGame":
                putItBackSnd.PlayOneShot(determineClips[7]);
                break;
            case "nightstand_opened":
                putItBackSnd.PlayOneShot(determineClips[6]);
                break;
            case "InsideClosetMini":
                putItBackSnd.PlayOneShot(determineClips[7]);
                break;
            case "sakuraPctGame":
                putItBackSnd.PlayOneShot(determineClips[7]);
                break;
            case "shoesGame":
                putItBackSnd.PlayOneShot(determineClips[7]);
                break;
            case "slidingPuzzleGame":
                putItBackSnd.PlayOneShot(determineClips[7]);
                break;
            case "tonerGame":
                putItBackSnd.PlayOneShot(determineClips[7]);
                break;
            case "ZoomIn":
                putItBackSnd.PlayOneShot(determineClips[12]);
                break;
            case "zoomOut":
                putItBackSnd.PlayOneShot(determineClips[12]);
                break;
        }
    }
    public void PlaySoundsItem(string str)
    {
        switch (str)
        {
            case "gold_key":
            case "red_key":
            case "siniy_klyuch":
            case "full_spec_key":
            case "portal_key":
            case "key_for_the_car":
                determineAudio[0].Play();
                break;

            case "spec_key1":
            case "spec_key2":
            case "liquorice":
                determineAudio[1].Play();
                break;

            case "kanistra":
                determineAudio[9].Play();
                break;

            case "lozhka":
                determineAudio[12].Play();
                break;

            case "banka_metal":
                determineAudio[2].Play();
                break;

            case "detali":
            case "needles_item":
                determineAudio[7].Play();
                break;

            case "picture2":
            case "picture3":
            case "book_purp1_item":
            case "book_purp4_item":
            case "nameplate2_item":
            case "nameplate5_item":
            case "noColorPhoto":
            case "finished_photo":
                determineAudio[11].Play();
                break;

            case "statue_hand":
            case "statue_head":
                determineAudio[3].Play();
                break;

            case "banka_pechen":
            case "banka_serdtse":
                determineAudio[4].Play();
                break;

            case "hand_item":
                determineAudio[5].Play();
                break;
            case "pianoThingsItem":
                determineAudio[10].Play();
                break;

            case "paper1":
            case "paper_stuck_item":
            case "safe_code_item":
                determineAudio[6].Play();
                break;

            case "saw_item":
                determineAudio[8].Play();
                break;

            case "Shkatulochka":
            case "chest_item":
                determineAudio[13].Play();
                break;

            case "casset":
            case "casset_red_item":
            case "casset_yellow_item":
                determineAudio[14].Play();
                break;

            case "cookie":
                determineAudio[15].Play();
                break;

            case "duck_item":
                determineAudio[16].Play();
                break;

            case "bubble_purpule_item":
            case "bubble_transparent_item":
            case "bubble_white_item":
            case "bubble_yellow_item":
            case "toner_item":
                determineAudio[17].Play();
                break;

            case "cartrige_item":
            case "plata_item":
                determineAudio[18].Play();
                break;

            case "knife_for_tree":
                determineAudio[19].Play();
                break;

            case "hammer_item":
                determineAudio[20].Play();
                break;

            case "robot_eye":
            case "robot_hand":
            case "robot_head":
                determineAudio[21].Play();
                break;

            default:
                determineAudio[1].Play();
                break;
        }
    }
}
