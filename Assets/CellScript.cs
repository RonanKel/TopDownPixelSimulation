using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] int waterOnClick = 9;
    public int rowPosition;
    public int colPosition;

    private GridScript gridScript;

    public float waterLevel = 0;

    private float blueAmount;

    // Start is called before the first frame update
    void Start()
    {
        gridScript = transform.parent.gameObject.GetComponent<GridScript>();
        // random color
        //sr.color = new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f), 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (waterLevel > 0) {
            sr.color = new Color(.8f-(waterLevel/(63f)), 1f, 1f, 1f);
        } else {
            sr.color = new Color(0f, 0f, 0f, 1f);
        }
        
    }

    void OnMouseDown() {
        //Debug.Log("Clicked! (" + rowPosition + ", " + colPosition + ")");
        waterLevel += waterOnClick;
        if (waterLevel > 1) {
            gridScript.nextCellMaxHeap.Add(gameObject);
        }
        //gridScript.SpreadWater(rowPosition, colPosition);
        
    }

    public void SetPosition(int row, int col) {
        rowPosition = row;
        colPosition = col;
    }

    public float GetWaterLevel() {
        return waterLevel;
    }

    public void RecieveWater() {
        waterLevel++;
    }

    public void CheckToSpread() {
        if (waterLevel > 1) {
            //gridScript.SpreadWater(rowPosition, colPosition);
            Debug.Log(rowPosition + " " + colPosition);
        }
    }


}
