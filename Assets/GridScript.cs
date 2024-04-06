using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maxheap;

public class GridScript : MonoBehaviour
{
    // Length of one side of the matrix in game units
    [SerializeField] float matrixLength = 5;
    // The numbers of cells on one side of the matrix, total matrix is cellsInLength x cellsInLength
    [SerializeField] int numRows = 5;
    // The Object of each cell
    [SerializeField] GameObject cell;

    [SerializeField] GameObject brush;


    public List<GameObject> waterCells = new List<GameObject>();
    public List<GameObject> nextWaterCells = new List<GameObject>();



    public List<List<GameObject>> matrix = new List<List<GameObject>>();

    // The current cell
    private GameObject currentCell;
    // The position of the current cell
    private Vector2 currentCellPos;

    //The position of the first cell [0][0]
    private Vector2 firstCellPos;

    // The length of a cell
    private float cellLength;

    CellScript adjacentCellScript;

    CellScript currentCellScript;

    public MaxHeap cellMaxHeap = new MaxHeap();
    public MaxHeap nextCellMaxHeap = new MaxHeap();

    private string mouseMode = "";



    // Start is called before the first frame update
    void Awake()
    {
        BuildMatrix();
    }

    void Start() 
    {
        InvokeRepeating("Flow", 2f, .15f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BuildMatrix() {
        int i;
        int j;
        
        
        cellLength = matrixLength / numRows;
        // Initializes the matrix
        for (i = 0; i < numRows; i++) {
            matrix.Add(new List<GameObject>());
        }
        

        firstCellPos = new Vector2((-matrixLength/2)+(cellLength/2), (matrixLength/2)-(cellLength/2));

        CellScript cellScript;
        

        // Sets up the matrix with cells
        for (i = 0; i < numRows; i++) {
            for (j = 0; j < numRows; j++) {
                
                currentCellPos = firstCellPos + new Vector2(j * cellLength, (i * cellLength) * -1);
                currentCell = Instantiate(cell, currentCellPos, transform.rotation, transform);
                currentCell.transform.localScale = new Vector3(cellLength, cellLength, cellLength);
                cellScript = currentCell.GetComponent<CellScript>();
                cellScript.SetPosition(i, j);
                matrix[i].Add(currentCell);
            }
        }
    }

    

    public void SpreadWater(GameObject cell) {
        int i;
        int j;

        

        Debug.Log("Spreading water!");

        currentCellScript = cell.GetComponent<CellScript>();

        int rowPosition = currentCellScript.rowPosition;
        int colPosition = currentCellScript.colPosition;

        int startI = (int) Mathf.Max((rowPosition-1), 0f);
        int endI = (int) Mathf.Min(rowPosition+2, numRows);
        int startJ = (int) Mathf.Max(colPosition-1, 0f);
        int endJ = (int) Mathf.Min(colPosition+2, numRows);

        float waterTotal = 0f;
        int squaresWithLessWater = 0;
        float averageWaterFlow;
        List<CellScript> cellScripts = new List<CellScript>();

        //int waterLevel = currentCellScript.waterLevel;

    
        // see how much water to pass to adjacent squares
        for (i = startI; i < endI; i++) {
            for (j = startJ; j < endJ; j++) {
                adjacentCellScript = matrix[i][j].GetComponent<CellScript>();

                if (adjacentCellScript.waterLevel <= currentCellScript.waterLevel) {
                    squaresWithLessWater++;
                    waterTotal += adjacentCellScript.GetWaterLevel();
                    cellScripts.Add(adjacentCellScript);
                }
            }   
        }

        
        averageWaterFlow = waterTotal / squaresWithLessWater;
        //Debug.Log(averageWaterFlow);

        foreach (CellScript cellScript in cellScripts) {
            if (cellScript.waterLevel <= 0f) {
                nextWaterCells.Add(cellScript.gameObject);
            }
            cellScript.waterLevel = averageWaterFlow;
            

        }


        // Find the cell

        // For each adjacent cell if it has a lower water level then spread one water there on a delay
    }

    [ContextMenu("Flow")]
    public void Flow() {
        int i;

        //waterCells = nextWaterCells;
        //nextWaterCells = new List<GameObject>();
        
        waterCells.AddRange(nextWaterCells);

        /*foreach (GameObject cell in nextWaterCells) {
            waterCells.Add(cell);
        }*/

        nextWaterCells.Clear();

        // Sort this list in decreasing order
        BubbleSort(waterCells, waterCells.Count);

        for (i = 0; i < waterCells.Count; i++){
            if (waterCells[i].GetComponent<CellScript>().waterLevel >= 1f) {
                SpreadWater(waterCells[i]);
            }
        }
    }

    private List<GameObject> BubbleSort(List<GameObject> cells, int len) {
        int i;
        int j;
        bool swapped;
        

        for (i = 0; i < len - 1; i++) {
            swapped = false;
            for (j = 0; j < len - i - 1; j++) {
                currentCellScript = cells[j].GetComponent<CellScript>();
                adjacentCellScript = cells[j+1].GetComponent<CellScript>();
                if (currentCellScript.waterLevel > adjacentCellScript.waterLevel) {
                    currentCell = cells[j];
                    cells[j] = cells[j+1];
                    cells[j+1] = currentCell;
                }
            }
            if (!swapped) {
                break;
            }
        }

        return cells;

    }

    public void AddWaterClickButton() {
        mouseMode = "water";
        brush.SetActive(false);
    }

    public string CheckMouseType() {
        return mouseMode;
    }



    public void ChangeElevationBrushButton() {
        // change the mouse type
        mouseMode = "elevation";

        //enable the brush
        brush.SetActive(true);
    }

}
