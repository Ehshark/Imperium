﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEditor;

public class SkillTreeController : MonoBehaviour
{
    public Image enlargedIcon;
    public Image centerIcon;
    public TMP_Text abilityDesc;
    public TMP_Text abilityName;

    public Button silenceIcon;
    public Button recycleIcon;
    public Button lifeStealIcon;
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

    //Multiplayer
    const byte POWER_SELECTED_EVENT = 20;

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
            {10,"Lifesteal"},
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
            {10,"This minion gains lifesteal until the end of your turn."},
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
            {10,EVENT_TYPE.POWER_LIFESTEAL},
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
            {10,lifeStealIcon},
            {12,silenceIcon},
            {14,buffIcon},
            {17,trashIcon}
        };

        if (!StartGameController.Instance.tutorial)
        {
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
            {
                if (GameManager.Instance.ActiveHero(true).Powers.Contains(entry.Key))
                {
                    PowerIcons.Where(x => x.Key == entry.Key).SingleOrDefault().Value.interactable = false;
                }
            }
        }
        else
        {
            foreach (KeyValuePair<int, Button> entry in PowerIcons)
            {
                entry.Value.interactable = false;
            }

            int parentCnt = gameObject.transform.parent.childCount;
            gameObject.transform.SetSiblingIndex(parentCnt - 2);

            //StartGameController.Instance.TutorialObject.GetComponent<TutorialTextController>().ShowUI();
        }
    }

    public void UnlockPower()
    {
        if (selectedPower != 0)
        {
            GameObject hero;

            if (StartGameController.Instance.StartingPowerSelected)
                hero = GameManager.Instance.ActiveHero(true).gameObject;
            else
            {
                hero = GameManager.Instance.bottomHero.gameObject;
            }

            Hero heroScript = hero.GetComponent<Hero>();
            HeroPowerListener hpl = hero.AddComponent<HeroPowerListener>();

            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
            {
                if (selectedPower == entry.Key)
                {
                    hpl.PowerEffect = PowerEffects.Where(x => x.Key == entry.Key).SingleOrDefault().Value;
                    hpl.enabled = true;
                    heroScript.PowerCount++;
                    heroScript.Powers.Add(entry.Key);

                    if (heroScript.PowerCount == 1)
                    {
                        heroScript.Ability1.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
                        heroScript.Ability1.gameObject.SetActive(true);
                    }
                    else if (heroScript.PowerCount == 2)
                    {
                        heroScript.Ability2.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
                        heroScript.Ability2.gameObject.SetActive(true);
                    }
                    else if (hero.GetComponent<Hero>().PowerCount == 3)
                    {
                        heroScript.Ability3.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
                        heroScript.Ability3.gameObject.SetActive(true);
                    }
                }
            }

            object[] data = new object[] { hero.GetComponent<Hero>().PowerCount, selectedPower };
            PhotonNetwork.RaiseEvent(POWER_SELECTED_EVENT, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                SendOptions.SendReliable);
            StartGameController.Instance.StartingPowerSelected = true;

            Destroy(gameObject);

            if (StartGameController.Instance.tutorial)
            {
                StartGameController.Instance.TutorialObject.GetComponent<TutorialTextController>().ShowUI();
            }
        }
    }

    public void OnDestroy()
    {
        GameManager.Instance.skillTree = null;
    }
}
