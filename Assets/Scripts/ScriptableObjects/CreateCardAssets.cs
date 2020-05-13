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

            //    string[] lineArr = new string[14];
            //    lineArr = line.Split(',');

            //    MinionData newCard = ScriptableObject.CreateInstance<MinionData>();
            //    newCard.Color = ToColor(lineArr[0]);
            //    newCard.MinionID = Int32.Parse(lineArr[1]);
            //    newCard.GoldAndManaCost = Int32.Parse(lineArr[2]);
            //    newCard.ConditionID = Int32.Parse(lineArr[3]);
            //    newCard.ConditionText = lineArr[4];
            //    newCard.EffectId1 = Int32.Parse(lineArr[5]);
            //    newCard.EffectText1 = lineArr[6];
            //    newCard.EffectId2 = Int32.Parse(lineArr[7]);
            //    newCard.EffectText2 = lineArr[8].ToString();
            //    newCard.AttackDamage = Int32.Parse(lineArr[9]);
            //    newCard.Health = Int32.Parse(lineArr[10]);
            //    newCard.CardClass = lineArr[11];
            //    newCard.IsPromoted = false;
            //    newCard.IsTapped = false;
            //    newCard.IsSilenced = false;
            //    newCard.AllyClassID = Int32.Parse(lineArr[12]);
            //    newCard.AllyClass = lineArr[13];

            //    AssetDatabase.CreateAsset(newCard, "Assets/Resources/Minions/" + lineArr[1] + ".asset");

            //********************** FOR ALLSTARTERS.CSV ******************************

            //    string[] lineArr = new string[9];
            //    lineArr = line.Split(',');
            //    StarterData newCard = ScriptableObject.CreateInstance<StarterData>();

            //    newCard.Color = ToColor(lineArr[0]);
            //    newCard.StarterID = Int32.Parse(lineArr[1]);
            //    newCard.ManaCost = Int32.Parse(lineArr[2]);
            //    newCard.EffectId1 = Int32.Parse(lineArr[3]);
            //    newCard.EffectText1 = lineArr[4];
            //    newCard.EffectId2 = Int32.Parse(lineArr[5]);
            //    newCard.EffectText2 = lineArr[6].ToString();
            //    newCard.AttackDamage = Int32.Parse(lineArr[7]);
            //    newCard.Health = Int32.Parse(lineArr[8]);
            //    newCard.IsPromoted = false;
            //    newCard.IsTapped = false;
            //    newCard.IsSilenced = false;

            //    AssetDatabase.CreateAsset(newCard, "Assets/Resources/Starters/" + lineArr[1] + ".asset");

            //********************** FOR ALLESSENTIALS.CSV ******************************

        //    string[] lineArr = new string[7];
        //    lineArr = line.Split(',');
        //    EssentialsData newCard = ScriptableObject.CreateInstance<EssentialsData>();

        //    newCard.Color = ToColor(lineArr[0]);
        //    newCard.Id = Int32.Parse(lineArr[1]);
        //    newCard.ManaCost = Int32.Parse(lineArr[2]);
        //    newCard.EffectId1 = Int32.Parse(lineArr[3]);
        //    newCard.EffectText1 = lineArr[4];
        //    newCard.EffectId2 = Int32.Parse(lineArr[5]);
        //    newCard.EffectText2 = lineArr[6].ToString();

        //    AssetDatabase.CreateAsset(newCard, "Assets/Resources/Essentials/" + lineArr[1] + ".asset");
        //}
    }

    public Color ToColor(string color)
    {
        if (color.Equals("Orange"))
            return new Color(1.0f, 0.5f, 0.0f);
        else
            return (Color)typeof(Color).GetProperty(color.ToLowerInvariant()).GetValue(null, null);
    }
}
