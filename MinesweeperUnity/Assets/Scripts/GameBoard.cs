using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** GameBoard.cs
 *  Minesweeper Unity - Personal Challenge 2023
 *  
 *  Script representing a game of minesweeper.
 *  @author Zac Seales
 */
public class GameBoard : MonoBehaviour
{
    /** The state of each box in the grid.
     *  Either: 
     *      -1 (Flagged)
     *      0  (Revealed)
     *      1  (Unrevealed and unflagged). */
    public int[,] boxStates;
    /** The number of neighbouring mines for each box
     *  Either: (0-8), or 9 if the box contains a mine. */
    public int[,] boxValues;
    /* The number of mines. */
    public int numMines;

    /* The interactable box prefab object. */
    public GameObject box;
    /* The size of each box in the grid. */
    private int boxSize;

    // Start is called before the first frame update
    void Start()
    {
        int w = Screen.width;
        int h = Screen.height;
        boxSize = w / boxStates.GetLength(1);
        Debug.Log("ScreenSize = " + w + "x" + h);
        Debug.Log("BoxSize = " + boxSize);
        // initialize grid box states using data-field size and display them.
        initStates(boxStates.GetLength(0), boxStates.GetLength(1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /** Initialize all box states as unrevealed and unflagged.
     *  And display them in a grid layout on screen.
     */
    private void initStates(int rows, int cols)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                GameObject temp = GameObject.Instantiate(this.box, new Vector3(i * (boxSize/100) - (Screen.width / 141), j * (boxSize/100) - (Screen.height / 148), -5f), Quaternion.identity);
                this.boxStates[i, j] = 1;
            }
        }
    }

}
