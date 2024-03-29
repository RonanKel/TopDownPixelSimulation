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

    int i;
    int j;
    int n;

    // Start is called before the first frame update
    void Awake()
    {
        BuildMatrix();
    }

    void Start() 
    {
        //InvokeRepeating("Flow", 2f, .15f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BuildMatrix() {
        
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
        //Called by cell of water

        currentCellScript = cell.GetComponent<CellScript>();
        int rowPosition = currentCellScript.rowPosition;
        int colPosition = currentCellScript.colPosition;
        float waterTotal = 0;
        int squaresWithLessWater = 0;
        float averageWaterFlow;
        List<GameObject> cells = new List<GameObject>();
        //int waterLevel = currentCellScript.waterLevel;

    
        // see how much water to pass to adjacent squares
        for (i = (int) Mathf.Max((rowPosition-1), 0f); i < Mathf.Min(rowPosition+2, numRows); i++) {
            for (j = (int) Mathf.Max(colPosition-1, 0); j < Mathf.Min(colPosition+2, numRows); j++) {
                adjacentCellScript = matrix[i][j].GetComponent<CellScript>();

                if (adjacentCellScript.GetWaterLevel() <= currentCellScript.waterLevel) {
                    squaresWithLessWater++;
                    waterTotal += adjacentCellScript.GetWaterLevel();
                    cells.Add(matrix[i][j]);
                }
            }
        }
        averageWaterFlow = waterTotal / squaresWithLessWater;

        for (i = 0; i < cells.Count; i++) {
            cells[i].GetComponent<CellScript>().waterLevel = averageWaterFlow;
            if (averageWaterFlow > 1) {
                nextCellMaxHeap.Add(cells[i]);
            }

        }


        // Find the cell

        // For each adjacent cell if it has a lower water level then spread one water there on a delay


    }

    [ContextMenu("Flow")]
    public void Flow() {

        /*Debug.Log("Flowing!");
        for (i = 0; i < numRows; i++) {
            for (j = 0; j < numRows; j++) {
                
                currentCellScript = matrix[i][j].GetComponent<CellScript>();
                if (currentCellScript.waterLevel > 1) {
                    Debug.Log(j);
                    //SpreadWater(i, j);
                    //Debug.Log("SpreadWater: " + i + " " + j);
                    cellMaxHeap.Add(matrix[i][j]);
                }
            }
        }*/
        while (cellMaxHeap.Count > 0) {
            GameObject cell = cellMaxHeap.ExtractMax();
            SpreadWater(cell);
        }
        while (nextCellMaxHeap.Count > 0) {
            cellMaxHeap.Add(nextCellMaxHeap.ExtractMax());
        }
    }

}
