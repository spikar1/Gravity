using UnityEngine;
using System.Collections;

public class TilePrefabSelector : MonoBehaviour {

    public Tile[] tiles;
    

    [System.Serializable]
    public class Tile
    {
        public string name = "Name me";     
        public Color color;                 
        public GameObject gameObject;       
        public string groupName = "Misc";   
    } 
	
}
