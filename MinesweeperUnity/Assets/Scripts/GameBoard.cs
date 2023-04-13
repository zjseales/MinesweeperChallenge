using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
        newGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /** Called on every new game.
     */
    public void newGame()
    {
        fitToScreen();
        // initialize grid box states using data-field size and display them.
        initStates(boxStates.GetLength(0), boxStates.GetLength(1));
    }

    public void Reset()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }

    /** Fits the grid on screen. To be called when screen settings are changed midgame.
     */
    public void fitToScreen()
    {
        int w = Screen.width;
        int h = Screen.height;
        boxSize = Mathf.Min(h / boxStates.GetLength(0), w / boxStates.GetLength(1));
        Debug.Log("ScreenSize = " + w + "x" + h);
        Debug.Log("BoxSize = " + boxSize);
        setupGrid(boxStates.GetLength(0), boxStates.GetLength(1));
    }

    // Adjust grid size after screen resolution has been adjusted.
    public IEnumerator waitBeforeAdjust()
    {
        yield return new WaitForSeconds(0.2f);
        fitToScreen();
    }

    /** Set up grid values so that the game fits on screen.
     */
    private void setupGrid(int rows, int cols)
    {
        GridLayoutGroup currentGrid = this.GetComponent<GridLayoutGroup>();
        // set cell size
        currentGrid.cellSize = new Vector2(boxSize, boxSize);
        // set number of rows
        currentGrid.constraintCount = rows;
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
                GameObject temp = GameObject.Instantiate(this.box, new Vector3(i, j, -5f), Quaternion.identity);
                temp.name = "b" + i + "_" + j;
                this.boxStates[i, j] = 1;
                temp.transform.position = this.transform.position;
                temp.GetComponent<RectTransform>().SetParent(this.transform);
            }
        }
    }

}
