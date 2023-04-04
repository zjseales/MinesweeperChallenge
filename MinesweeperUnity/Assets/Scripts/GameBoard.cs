using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** GameBoard.cs
 *  Minesweeper Unity - Personal Challenge 2023
 *  
 *  Script representing a game of minesweeper.
 *  @author Zac Seales
 */
public class GameBoard
{
    /** The state of each box in the grid.
     *  Either: 
     *      -1 (Flagged)
     *      0  (Revealed)
     *      1  (Unrevealed and unflagged)
     */
    private int[,] boxStates;
    /** The number of neighbouring mines for each box
     *  Either: (0-8) or 9 if the box contains a mine.
     */
    private int[,] boxValues;

    /** Constructor - Initializes data-fields using the argument values.
     */
    public GameBoard(int rows, int cols, int mines)
    {
        this.boxStates = new int[rows, cols];
        this.boxValues = new int[rows, cols];
        //initialize all box states as unrevealed and unflagged
        for(int i = 0; i < rows; i++) 
        {
            for (int j = 0; j < cols; j++)
            {
                this.boxStates[i, j] = 1;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Create game");
        for (int i = 0; i < boxStates.GetLength(0); i++)
        {
            for (int j = 0; j < boxStates.GetLength(1); j++)
            {
                Debug.Log("" + boxStates[i, j] + ", ");
            }
            Debug.Log("\n");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
