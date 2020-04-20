using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterGenerator : MonoBehaviour
{
    Sprite[] randomCharacterSprites;
    WaypointInfo[] randomWaypointInfos;
    Sprite[] mainPathCharacterSprites;
    WaypointInfo[] mainPathWaypointInfos;
    public TextLiterals textLiterals;

    void Awake()
    {
        this.randomCharacterSprites = Resources.LoadAll<Sprite>("CharacterSprites/Random");
        this.randomWaypointInfos = Resources.LoadAll<WaypointInfo>("Waypoints/RandomText");

        this.mainPathCharacterSprites = Resources.LoadAll<Sprite>("CharacterSprites/MainPath");
        this.mainPathWaypointInfos = Resources.LoadAll<WaypointInfo>("Waypoints/PathText");

        this.GenerateRandomCharacters();
        this.GenerateMainPathCharacters();
    }

    private void GenerateRandomCharacters()
    {
        Debug.LogError("Generating Random Characters");

        for (int i = 0; i < this.randomWaypointInfos.Length; i++)
        {
            this.randomWaypointInfos[i].dialog = textLiterals.randomText[i];
            this.randomWaypointInfos[i].waypointImage = this.randomCharacterSprites[i];
            EditorUtility.SetDirty(this.randomWaypointInfos[i]);
        }

        Debug.LogError("Done!!");
    }

    private void GenerateMainPathCharacters()
    {
        Debug.LogError("Generating main path characters...");

        for (int i = 0; i < this.mainPathWaypointInfos.Length; i++)
        {
            this.mainPathWaypointInfos[i].dialog = textLiterals.randomText[i];
            this.mainPathWaypointInfos[i].waypointImage = this.mainPathCharacterSprites[i];
            EditorUtility.SetDirty(this.mainPathWaypointInfos[i]);
        }

        Debug.LogError("Done!!");
    }
}
