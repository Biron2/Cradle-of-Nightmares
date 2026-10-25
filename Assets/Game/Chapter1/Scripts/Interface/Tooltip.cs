using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Используйте TMPro, если вы работаете с TextMeshPro

public class Tooltip : MonoBehaviour
{
    public GameObject tooltipPrefab; // Ссылка на префаб подсказки
    public float fadeDuration = 0.5f; // Время для плавного появления/исчезновения
    public Vector3 offset = new Vector3(0, 20, 0); // Смещение по оси Y
    private GameObject tooltipInstance; // Экземпляр созданного префаба
    private CanvasGroup canvasGroup; // Для управления прозрачностью текста
    private Text mText; // Ссылка на текст
    public int language;
    [SerializeField] private Transform tip_parent;
    private Image localImage;
    [SerializeField] private VariableDeterminator _VariableDeterminator;
    private Coroutine hoverCoroutine;
    public void langSwitch()
    {
        language = PlayerPrefs.GetInt("language", language);
    }

    private void Start()
    {
        localImage = GetComponent<Image>();
        langSwitch();
    }

    private void OnMouseEnter()
    {
        if (!VariableDeterminator.isDraging)
        {
            if (hoverCoroutine == null)
            {
                hoverCoroutine = StartCoroutine(ShowTooltipWithDelay(1f)); // Задержка в 1 секунду
            }
        }
    }

    private void OnMouseExit()
    {
        StopTipShowing();
    }
    public void StopTipShowing()
    {
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
            hoverCoroutine = null;
        }

        if (tooltipInstance != null)
        {
            StopAllCoroutines();
            _VariableDeterminator.CallFadeTooltip(0, canvasGroup, tooltipInstance, fadeDuration);
        }
    }
    private IEnumerator ShowTooltipWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (tooltipInstance == null)
        {
            // Создать экземпляр префаба
            tooltipInstance = Instantiate(tooltipPrefab, transform.position + offset, Quaternion.identity, tip_parent);
            canvasGroup = tooltipInstance.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup = tooltipInstance.AddComponent<CanvasGroup>();
            }

            mText = tooltipInstance.GetComponentInChildren<Text>();
            canvasGroup.alpha = 0; // Установить начальную прозрачность в 0
        }

        mText.text = UpdateTooltipText();
        StopAllCoroutines();

        _VariableDeterminator.CallFadeTooltip(1, canvasGroup, tooltipInstance, fadeDuration);
    }
    private string UpdateTooltipText()
    {
        if (localImage.sprite == null)
        {
            return language == 1 ? "Хммм..." : "Hmmm...";
        }
        else
        {
            switch (localImage.sprite.name)
            {
                case "cookie":
                    return language == 1 ? "Вкусная печенька" : "Tasty cookie";
                case "picture2":
                    return language == 1 ? "Фрагмент картины" : "Fragment of a picture";

                case "picture3":
                    return language == 1 ? "Фрагмент картины" : "Fragment of a picture";

                case "banka_metal":
                    return language == 1 ? "Консервная банка" : "A tincan";

                case "statue_head":
                    return language == 1 ? "Фрагмент статуи" : "Fragment of a statue";

                case "statue_hand":
                    return language == 1 ? "Фрагмент статуи" : "Fragment of a statue";

                case "spec_key2":
                    return language == 1 ? "Нужно соединить с другой\nчастью и получится ключ" : "You need to connect it with\nanother part and you will get a key";

                case "spec_key1":
                    return language == 1 ? "Нужно соединить с другой\nчастью и получится ключ" : "You need to connect it with\nanother part and you will get a key";

                case "siniy_klyuch":
                    return language == 1 ? "Серебристый ключ" : "Silver key";

                case "Shkatulochka":
                    return language == 1 ? "Шкатулка, внутри что-то есть" : "It's a casket, there's something inside";

                case "portal_key":
                    return language == 1 ? "Ключ-рука, достаточно массивный" : "Key-hand, quite massive";

                case "pianoThingsItem":
                    return language == 1 ? "Клавиши с алыми символами" : "Keys with scarlet symbols";

                case "m3Without":
                    return language == 1 ? "Голова взрослой женщины..." : "A grown woman's head...";

                case "lozhka":
                    return language == 1 ? "Ложку можно использовать\nне только для поедания пищи" : "The spoon can be used\nfor more than just eating food";

                case "liver":
                    return language == 1 ? "Человеческая печень" : "A human liver";

                case "kanistra":
                    if (VariableDeterminator.Isfullkanistra)
                    {
                        return language == 1 ? "Канистра, тяжеловатая" : "The canister, it's a bit heavy";
                    }
                    else
                    {
                        return language == 1 ? "Старая канистра, пустая" : "An old canister, empty";
                    }

                case "heart":
                    return language == 1 ? "Человеческое сердце" : "A human heart";

                case "hand_item":
                    return language == 1 ? "Женская рука,\nвлажная и холодная..." : "A woman's hand,\ndamp and cold...";

                case "gold_key":
                    return language == 1 ? "Золотистый ключ" : "Golden key";

                case "full_spec_key":
                    return language == 1 ? "Ключ-череп" : "Key-skull";

                case "detali":
                    return language == 1 ? "Провода, шестеренки,\nболты и гайки" : "Wires, gears,\nbolts and nuts";

                case "casset":
                    return language == 1 ? "Старая кассета с записью" : "Old cassette tape\nwith recording";

                case "banka_serdtse":
                    return language == 1 ? "Это же...человеческие органы" : "These are... human organs";

                case "banka_pechen":
                    return language == 1 ? "Это же...человеческие органы" : "These are... human organs";

                case "saw_item":
                    return language == 1 ? "Старая ржавеющая пила" : "Old rusty saw";

                case "book_purp1_item":
                    return language == 1 ? "Массивная книжица. На ней\nизображён японский символ" : "A massive book. A Japanese\nsymbol is depicted on it";

                case "book_purp4_item":
                    return language == 1 ? "Маленькая книжица. На ней\nизображён японский символ" : "A small book. A Japanese\nsymbol is depicted on it";

                case "cartrige_item":
                    return language == 1 ? "Катридж. На вид новый" : "A cartridge. Looks new";

                case "casset_red_item":
                    return language == 1 ? "Кассета с красной полоской" : "A cassette with a red stripe";

                case "casset_yellow_item":
                    return language == 1 ? "Кассета с желтой полоской" : "A cassette with a yellow stripe";

                case "chest_item":
                    return language == 1 ? "Объёмный сундучок,\nвнутри что-то есть" : "A large chest with\nsomething inside";

                case "duck_item":
                    return language == 1 ? "Эта утка мне что-то напоминает..." : "This duck reminds\nme of something...";

                case "hammer_item":
                    return language == 1 ? "Молоток, таким можно\nбы что-то разломать" : "A hammer, you could\nsmash something with it";

                case "key_for_the_car":
                    return language == 1 ? "Ключ от машины" : "Car key";

                case "knife_for_tree":
                    return language == 1 ? "Кухонный нож" : "Kitchen knife";

                case "liquorice":
                    return language == 1 ? "У этой штуки очень\nрезкий запах" : "This thing has\na very strong smell";

                case "nameplate2_item":
                    return language == 1 ? "Табличка с изображением" : "A nameplate with an image";

                case "nameplate5_item":
                    return language == 1 ? "Табличка с изображением" : "A nameplate with an image";

                case "nameplate6_item":
                    return language == 1 ? "Табличка с изображением" : "A nameplate with an image";

                case "paper_stuck_item":
                    return language == 1 ? "Пачка белой бумаги" : "A stack of white paper";

                case "plata_item":
                    return language == 1 ? "Плата микроконтроллер" : "Microcontroller board";

                case "robot_eye":
                    return language == 1 ? "Пластмассовый квадрат,\nбудто от какой-то игрушки" : "A plastic square,\nlike from a toy";

                case "robot_hand":
                    return language == 1 ? "Пластмассовая рука" : "A plastic hand";

                case "robot_head":
                    return language == 1 ? "Пластмассовая голова" : "A plastic head";

                case "safe_code_item":
                    return language == 1 ? "Лист с иероглифами" : "A sheet with hieroglyphs";

                case "toner_item":
                    return language == 1 ? "Тонер. Еще не использованный." : "Toner. Unused.";

                case "bubble_purpule_item":
                    return language == 1 ? "Банка с жидкостью" : "A jar with liquid";

                case "bubble_transparent_item":
                    return language == 1 ? "Банка с жидкостью" : "A jar with liquid";

                case "bubble_white_item":
                    return language == 1 ? "Банка с жидкостью" : "A jar with liquid";

                case "bubble_yellow_item":
                    return language == 1 ? "Банка с жидкостью" : "A jar with liquid";

                case "finished_photo":
                    return language == 1 ? "Фото с изображением девочки" : "A photo of a girl";

                case "noColorPhoto":
                    return language == 1 ? "Непроявленное фото" : "An undeveloped photo";

                case "purpulePhoto":
                    return language == 1 ? "Не до конца проявленное фото" : "A partially developed photo";

                case "wetPhoto":
                    return language == 1 ? "Не до конца проявленное фото" : "A partially developed photo";

                case "whitePhoto":
                    return language == 1 ? "Не до конца проявленное фото" : "A partially developed photo";

                case "yellowPhoto":
                    return language == 1 ? "Не до конца проявленное фото" : "A partially developed photo";

                case "needles_item":
                    return language == 1 ? "Набор иголок для шитья" : "Set of sewing needles";

                default:
                    return language == 1 ? "Хммм..." : "Hmmm...";

            }
        }
    }
}