using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroManager : MonoBehaviour, IListener
{
    private int maxLevel = 5;
    
    public Dictionary<int, int> Warrior_Powers;
    public Dictionary<int, int> MageRogue_Powers;

    public Dictionary<int, string> WarriorLevelStats;
    public Dictionary<int, string> MageLevelStats;
    public Dictionary<int, string> RogueLevelStats;

    public int MaxLevel { get => maxLevel; set => maxLevel = value;  }

    public static HeroManager Instance { get; private set; } = null;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    public void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.LEVEL_UP, this);

        Warrior_Powers = new Dictionary<int, int> {
            { 2, 1 },
            { 3, 2 },
            { 4, 2 },
            { 5, 3 }
        };

        MageRogue_Powers = new Dictionary<int, int> {
            { 2, 1 },
            { 3, 1 },
            { 4, 2 },
            { 5, 2 }
        };

        WarriorLevelStats = new Dictionary<int, string> {
            { 2, "4/4/1" },
            { 3, "5/5/2" },
            { 4, "5/5/2" },
            { 5, "6/6/3" }
        };

        MageLevelStats = new Dictionary<int, string> {
            { 2, "4/4/1" },
            { 3, "5/5/2" },
            { 4, "5/5/2" },
            { 5, "6/6/3" }
        };

        RogueLevelStats = new Dictionary<int, string> {
            { 2, "4/4/2" },
            { 3, "5/5/3" },
            { 4, "5/5/3" },
            { 5, "6/6/4" }
        };
    }

    public void OnEvent(EVENT_TYPE Event_Type)
    {
        Hero hero = GameManager.Instance.ActiveHero(true);

        if (hero.Level != maxLevel)
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Level Up!"));

            hero.IncreaseLevel(1);
            hero.IncreaseExp(6);

            //Assign Health, Mana, and Damage based on Hero
            string[] resources;

            if (hero.Clan == 'W')
            {
                resources = (WarriorLevelStats.Where(l => l.Key == hero.Level).SingleOrDefault().Value).Split('/');
            }
            else if (hero.Clan == 'R')
            {
                resources = (RogueLevelStats.Where(l => l.Key == hero.Level).SingleOrDefault().Value).Split('/');
            }
            else
            {
                resources = (MageLevelStats.Where(l => l.Key == hero.Level).SingleOrDefault().Value).Split('/');
            }

            hero.ChangeResources(int.Parse(resources[0]), int.Parse(resources[1]), int.Parse(resources[2]));

            //Show Skill tree
            if (hero.Clan == 'W')
            {
                if (hero.PowerCount < Warrior_Powers.Where(p => p.Key == hero.Level).SingleOrDefault().Value)
                {
                    GameObject tree = Instantiate(GameManager.Instance.skillTreePrefab);
                    tree.transform.SetParent(GameManager.Instance.canvas, false);
                }
            }
            else
            {
                if (hero.PowerCount < MageRogue_Powers.Where(p => p.Key == hero.Level).SingleOrDefault().Value)
                {
                    GameObject tree = Instantiate(GameManager.Instance.skillTreePrefab);
                    tree.transform.SetParent(GameManager.Instance.canvas, false);
                }
            }

            //Assigns the Hero's Required exp to be at max. Can't be increased anymore
            if (hero.Level == maxLevel)
            {
                hero.Experience = hero.RequredExp;
            }
        }
    }

    public void tmp()
    {
        GameManager.Instance.ActiveHero(true).GainExp(1);
    }
}
