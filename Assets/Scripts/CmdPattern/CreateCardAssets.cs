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
    //private readonly string path = "D:\\UnityProjects\\Unity-Imperium\\Imperium\\Assets\\allMinions.csv";

    // Start is called before the first frame update
    void Start()
    {
        //FileStream fileStream = File.OpenRead(path);
        //StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize);
        //string line;
        //line = streamReader.ReadLine();
        //while ((line = streamReader.ReadLine()) != null)
        //{
        //    string[] lineArr = new string[12];
        //    lineArr = line.Split(',');

        //    CardData newCard = ScriptableObject.CreateInstance<CardData>();
        //    newCard.Color = ToColor(lineArr[0]);
        //    newCard.MinionID = Int32.Parse(lineArr[1]);
        //    newCard.GoldAndManaCost = Int32.Parse(lineArr[2]);
        //    newCard.ConditionText = lineArr[3];
        //    newCard.EffectText1 = lineArr[4];
        //    newCard.EffectText2 = lineArr[5].ToString();
        //    newCard.AttackDamage = Int32.Parse(lineArr[6]);
        //    newCard.Health = Int32.Parse(lineArr[7]);
        //    newCard.CardClass = lineArr[8];
        //    newCard.IsPromoted = false;
        //    newCard.IsTapped = false;
        //    newCard.IsSilenced = false;
        //    newCard.AllyClass = lineArr[9];

        //    AssetDatabase.CreateAsset(newCard, "Assets/Cards/" + lineArr[1] + ".asset");
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
