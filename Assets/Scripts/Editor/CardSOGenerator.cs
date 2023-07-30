using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

public class CardSOGenerator : ScriptableWizard
{
    Dictionary<string, int> ranks = new Dictionary<string, int>()
    {
        {"2",2},
        {"3",3},
        {"4",4},
        {"5",5},
        {"6",6},
        {"7",7},
        {"8",8},
        {"9",9},
        {"10",10},
        {"ace",1},
        {"jack",11},
        {"queen",12},
        {"king",13}
    };
    Dictionary<string, string> suits = new Dictionary<string, string>()
    {
        {"clubs","Clubs"},
        {"diamonds","Diamonds"},
        {"hearts","Hearts"},
        {"spades","Spades"}
    };

    public DefaultAsset spritesFolder;
    [MenuItem("pisti-digitoy/CardSOGenerator")]
    private static void MenuEntryCall()
    {
        DisplayWizard<CardSOGenerator>("Title");
    }

    private void OnWizardCreate()
    {
        var files = System.IO.Directory.GetFiles(AssetDatabase.GetAssetPath(spritesFolder));
        foreach (var file in files)
        {
            var sprite = AssetDatabase.LoadAssetAtPath(file, typeof(Sprite)) as Sprite;
            if (sprite == null)
                continue;
            var spriteInfo = sprite.name.Split('_');
            if (spriteInfo.Length != 3)
                continue;
            if (suits.ContainsKey(spriteInfo[2]) == false)
                continue;

            var cardInfo = ScriptableObject.CreateInstance<CardInfo>();
            cardInfo.cardImage = sprite;
            cardInfo.suit = suits[spriteInfo[2]];
            cardInfo.rank = ranks[spriteInfo[0]];
            AssetDatabase.CreateAsset(cardInfo, $"Assets/ScriptableObjects/{cardInfo.suit + cardInfo.rank}.asset");
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
