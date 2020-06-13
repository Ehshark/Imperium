using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowEffect : MonoBehaviour
{
   public void ShowIcon(int id)
    {
        Image icon = GameManager.Instance.skillTree.GetComponent<SkillTreeController>().enlargedIcon;
        Image centericon = GameManager.Instance.skillTree.GetComponent<SkillTreeController>().centerIcon;
        TMP_Text abilityName = GameManager.Instance.skillTree.GetComponent<SkillTreeController>().abilityName;
        TMP_Text abilityDesc = GameManager.Instance.skillTree.GetComponent<SkillTreeController>().abilityDesc;

        foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
        {
            if (id == entry.Key)
            {
                icon.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
                centericon.sprite = icon.sprite;

                abilityName.text = GameManager.Instance.skillTree.GetComponent<SkillTreeController>().SkillName
                                                                                                     .Where(x => x.Key == id).SingleOrDefault().Value;
                abilityDesc.text = GameManager.Instance.skillTree.GetComponent<SkillTreeController>().SkillDefinition
                                                                                                     .Where(x => x.Key == id).SingleOrDefault().Value;
                GameManager.Instance.skillTree.GetComponent<SkillTreeController>().SelectedPower = id;
            }
        }
    }
}
