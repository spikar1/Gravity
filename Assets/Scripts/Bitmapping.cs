using UnityEngine;
using System.Collections;

public class Bitmapping : MonoBehaviour {

    public Sprite[] sprites;
    public Block[] blocks;

    int[] indices = new int[]{2, 8, 10, 11, 16, 18, 22, 24, 26, 27, 30, 31, 64, 66, 72, 74, 75, 80, 82, 86, 88, 90, 91, 94, 95, 104, 106, 107, 120, 122, 123, 126, 127, 208, 210, 214, 216, 218, 219, 222, 223, 248, 250, 251, 254, 255, 0};

	// Use this for initialization
	void Awake () {
        sprites = Resources.LoadAll<Sprite>("Tilemaps/LevelSpriteSheet_Example 1");
        blocks = transform.Find("Walls").GetComponentsInChildren<Block>();

        

        foreach(var block in blocks)
        {
            int sum = block.GetBit();
            int spriteIndex = GetSpriteIndex(sum);

            block.GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
        }
	    
	}
	
	// Update is called once per frame
	int GetSpriteIndex(int sum)
    {
        for (int i = 0; i < indices.Length; i++)
        {
            if (indices[i] == sum)
                return i+1;
        }
        return 0;
    }
}
