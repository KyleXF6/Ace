using UnityEngine;
using System.Collections.Generic;
using AceConsole;

[System.Serializable]
public class LocationManager : MonoBehaviour
{

    public List<Location> locationList;
    public Dictionary<string, Sprite> IDToSpriteMap;

    

    public Sprite GetLocationSprite(string id)
    {
        return IDToSpriteMap.ContainsKey(id) ? IDToSpriteMap[id] : null;
    }

    public void SetLocations(List<Location> locations)
    {
        IDToSpriteMap = new Dictionary<string, Sprite>();
        foreach (Location entry in locationList)
        {
            IDToSpriteMap[entry.ID] = entry.Image;
        }
    }

}


