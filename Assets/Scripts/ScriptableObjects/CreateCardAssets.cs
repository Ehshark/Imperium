//USE THIS SCRIPT TO GENERATE SCRIPTABLE OBJECTS INSIDE THE RESOURCES FOLDER

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CreateCardAssets : MonoBehaviour
{
    //const Int32 BufferSize = 128;
    //private readonly string path = "D:\\UnityProjects\\Unity-Imperium\\Imperium\\Assets\\ExcelFiles\\allEssentials.csv";

    void Start()
    {
        //FileStream fileStream = File.OpenRead(path);
        //StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize);
        //string line;
        //line = streamReader.ReadLine();
        //while ((line = streamReader.ReadLine()) != null)
        //{

        //********************** FOR ALLMINIONS.CSV ******************************

        //    string[] lineArr = new string[13];
        //    lineArr = line.Split(',');

        //    MinionData newCard = ScriptableObject.CreateInstance<MinionData>();
        //    newCard.MinionID = Int32.Parse(lineArr[0]);
        //    newCard.GoldAndManaCost = Int32.Parse(lineArr[1]);
        //    newCard.ConditionID = Int32.Parse(lineArr[2]);
        //    newCard.ConditionText = lineArr[3];
        //    newCard.EffectId1 = Int32.Parse(lineArr[4]);
        //    newCard.EffectText1 = lineArr[5];
        //    newCard.EffectId2 = Int32.Parse(lineArr[6]);
        //    newCard.EffectText2 = lineArr[7].ToString();
        //    newCard.AttackDamage = Int32.Parse(lineArr[8]);
        //    newCard.Health = Int32.Parse(lineArr[9]);
        //    newCard.CardClass = lineArr[10];
        //    newCard.IsPromoted = false;
        //    newCard.IsTapped = false;
        //    newCard.IsSilenced = false;
        //    newCard.AllyClassID = Int32.Parse(lineArr[11]);
        //    newCard.AllyClass = lineArr[12];

        //    AssetDatabase.CreateAsset(newCard, "Assets/Resources/Minions/" + lineArr[0] + ".asset");

        //********************** FOR ALLSTARTERS.CSV ******************************

        //    string[] lineArr = new string[8];
        //    lineArr = line.Split(',');
        //    StarterData newCard = ScriptableObject.CreateInstance<StarterData>();

        //    newCard.StarterID = Int32.Parse(lineArr[0]);
        //    newCard.ManaCost = Int32.Parse(lineArr[1]);
        //    newCard.EffectId1 = Int32.Parse(lineArr[2]);
        //    newCard.EffectText1 = lineArr[3];
        //    newCard.EffectId2 = Int32.Parse(lineArr[4]);
        //    newCard.EffectText2 = lineArr[5].ToString();
        //    newCard.AttackDamage = Int32.Parse(lineArr[6]);
        //    newCard.Health = Int32.Parse(lineArr[7]);
        //    newCard.IsPromoted = false;
        //    newCard.IsTapped = false;
        //    newCard.IsSilenced = false;

        //    AssetDatabase.CreateAsset(newCard, "Assets/Resources/Starters/" + lineArr[0] + ".asset");

        //********************** FOR ALLESSENTIALS.CSV ******************************

        //        string[] lineArr = new string[6];
        //    lineArr = line.Split(',');
        //    EssentialsData newCard = ScriptableObject.CreateInstance<EssentialsData>();

        //    newCard.Id = Int32.Parse(lineArr[0]);
        //    newCard.GoldCost = Int32.Parse(lineArr[1]);
        //    newCard.ManaCost = Int32.Parse(lineArr[2]);
        //    newCard.EffectId1 = Int32.Parse(lineArr[3]);
        //    newCard.EffectText1 = lineArr[4];
        //    newCard.EffectId2 = Int32.Parse(lineArr[5]);
        //    newCard.EffectText2 = lineArr[6].ToString();

        //    AssetDatabase.CreateAsset(newCard, "Assets/Resources/Essentials/" + lineArr[0] + ".asset");
        //}
    }
}
