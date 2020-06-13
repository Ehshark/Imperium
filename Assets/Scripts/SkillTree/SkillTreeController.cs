using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SkillTreeController : MonoBehaviour
{
    public Image enlargedIcon;
    public Image centerIcon;
    public TMP_Text abilityDesc;
    public TMP_Text abilityName;

    public Button silenceIcon;
    public Button recycleIcon;
    public Button untapIcon;
    public Button poisonIcon;
    public Button peekshopIcon;
    public Button healIcon;
    public Button buffIcon;
    public Button trashIcon;
    public Button stealthIcon;
    public Button expressbuyIcon;

    public Dictionary<int, string> SkillName;
    public Dictionary<int, string> SkillDefinition;
    public Dictionary<int, EVENT_TYPE> PowerEffects;
    public Dictionary<int, Button> PowerIcons;

    private int selectedPower = 0;

    public int SelectedPower { get => selectedPower; set => selectedPower = value; }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.skillTree = gameObject.transform;

        SkillName = new Dictionary<int, string>
        {
            {2,"Peek Shop"},
            {4,"Express Buy"},
            {5,"Recycle"},
            {6,"Heal Allied minion"},
            {7,"Poison Touch"},
            {8,"Stealth"},
            {11,"Untap Minion"},
            {12,"Silence"},
            {14,"Buff Allied Minion"},
            {17,"Trash"}
        };

        SkillDefinition = new Dictionary<int, string>
        {
            {2,"Select a shop card pile and look at the top 2 cards. Send either/both to the bottom or keep on top."},
            {4,"The next card you buy this turn goes straight to your hand"},
            {5,"Select a card from your discard pile. It goes on top of your deck."},
            {6,"Increase target allied minion's health by 1"},
            {7,"This minion gains poison touch until the end of your turn."},
            {8,"This minion gains stealth until the end of your turn."},
            {11,"Untap target allied minion."},
            {12,"Target enemy minion loses its effect(s) until the end of your opponent's next turn."},
            {14,"Target allied minion's damage increases by 1 until the end of your turn"},
            {17,"Select 1 card from your hand or discard pile. Remove that card from the game."}
        };

        PowerEffects = new Dictionary<int, EVENT_TYPE>
        {
            {2,EVENT_TYPE.POWER_PEEK_SHOP},
            {4,EVENT_TYPE.POWER_EXPRESS_BUY},
            {5,EVENT_TYPE.POWER_RECYCLE},
            {6,EVENT_TYPE.POWER_HEAL_MINION},
            {7,EVENT_TYPE.POWER_POISON_TOUCH},
            {8,EVENT_TYPE.POWER_STEALTH},
            {11,EVENT_TYPE.POWER_UNTAP},
            {12,EVENT_TYPE.POWER_SILENCE},
            {14,EVENT_TYPE.POWER_BUFF_MINION},
            {17,EVENT_TYPE.POWER_TRASH}
        };

        PowerIcons = new Dictionary<int, Button>
        {
            {2,peekshopIcon},
            {4,expressbuyIcon},
            {5,recycleIcon},
            {6,healIcon},
            {7,poisonIcon},
            {8,stealthIcon},
            {11,untapIcon},
            {12,silenceIcon},
            {14,buffIcon},
            {17,trashIcon}
        };

        foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
        {
            if (GameManager.Instance.ActiveHero(true).Powers.Contains(entry.Key))
            {
                PowerIcons.Where(x => x.Key == entry.Key).SingleOrDefault().Value.interactable = false;
            }
        }
    }

    public void UnlockPower()
    {
        if (selectedPower != 0)
        {
            GameObject hero = GameManager.Instance.ActiveHero(true).gameObject;

            hero.AddComponent<HeroPowerListener>();

            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
            {
                if (selectedPower == entry.Key)
                {
                    hero.GetComponent<HeroPowerListener>().PowerEffect = PowerEffects.Where(x => x.Key == entry.Key).SingleOrDefault().Value;
                    hero.GetComponent<HeroPowerListener>().enabled = true;
                    hero.GetComponent<Hero>().PowerCount++;
                    hero.GetComponent<Hero>().Powers.Add(entry.Key);

                    if (hero.GetComponent<Hero>().PowerCount == 1)
                    {
                        hero.GetComponent<Hero>().Ability1.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
                        hero.GetComponent<Hero>().Ability1.gameObject.SetActive(true);
                    }
                    else if (hero.GetComponent<Hero>().PowerCount == 2)
                    {
                        hero.GetComponent<Hero>().Ability2.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
                        hero.GetComponent<Hero>().Ability2.gameObject.SetActive(true);
                    }
                    else if (hero.GetComponent<Hero>().PowerCount == 3)
                    {
                        hero.GetComponent<Hero>().Ability3.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
                        hero.GetComponent<Hero>().Ability3.gameObject.SetActive(true);
                    }
                }
            }

            Destroy(gameObject);

            if (GameManager.Instance.WarriorSetup)
            {
                GameManager.Instance.StartGameManager.GetComponent<StartGameController>().SwitchHeroChoosing();
            }
        }
    }

    public void OnDestroy()
    {
        GameManager.Instance.skillTree = null;
    }
}
