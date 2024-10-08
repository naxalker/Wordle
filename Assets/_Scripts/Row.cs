using UnityEngine;

public class Row : MonoBehaviour
{
    public Tile[] Tiles { get; private set; }

    public string Word
    {
        get
        {
            string word = "";

            for (int i = 0; i < Tiles.Length; i++)
            {
                word += Tiles[i].Letter;
            }

            return word.ToLower();
        }
    }

    private void Awake()
    {
        Tiles = GetComponentsInChildren<Tile>();
    }
}
