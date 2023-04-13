using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    /* The background image for a revealed square. */
    public Sprite revealedBox;
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
        if (Input.GetMouseButtonDown(0)) 
        {
            reveal();
        }
    }

    /** Reveals the grid square and displays it's value, or a mine.
     */
    private void reveal()
    {
        // get selected object
        GameObject selectedBox = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        // do nothing if no object selected
        if (selectedBox == null) 
        {
            return;
        }
        // get indices of selected square
        Vector2 pos = getPosition(selectedBox.name);
        // ensure selected object was a square
        if (pos.x == -1)
        {
            return;
        }
        // switch unrevealed box to revealed (if no mine)
        if (boxStates[(int)pos.x, (int)pos.y] == 1 && boxValues[(int)pos.x, (int)pos.y] != 9)
        {
            selectedBox.GetComponent<Image>().sprite = revealedBox;
            selectedBox.transform.GetChild(0).GetComponent<Text>().text = boxValues[(int)pos.x, (int)pos.y].ToString();
        } 
        else if (boxValues[(int)pos.x, (int)pos.y] == 9)
        {
            selectedBox.transform.GetChild(0).GetComponent<Text>().text = "" + 9;
        }
    }

    /** Returns the vector position of the grid square using the name parameter.
     */
    private Vector2 getPosition(string name)
    {
        string[] vals = Regex.Split(name, @"_");
        // should be two values with a number in each
        if (vals.Length != 2)
        {
            return new Vector2(-1, -1);
        }
        // retrieve int values
        int x = int.Parse(Regex.Match(vals[0], @"\d+").Value);
        int y = int.Parse(vals[1]);
        return new Vector2(x, y);
    }

    /** Called on every new game.
     */
    public void newGame()
    {
        int rows = boxStates.GetLength(0);
        int cols = boxStates.GetLength(1);
        fitToScreen();
        // initialize grid box states using data-field size and display them.
        initStates(rows, cols);
        initMines(rows, cols);
    }

    /** Initialise all mines in the new game.
     */
    private void initMines(int rows, int cols)
    {
        float chance = numMines / (rows * cols);
        int minesToPlace = placeMines(numMines, chance, rows, cols);
        // if still mines to place
        while (minesToPlace > 0)
        {
            // chance increases to avoid long wait time
            chance += 0.01f;
            minesToPlace = placeMines(minesToPlace, chance, rows, cols);
        }
    }

    /** Place mines iteration.
     */
    private int placeMines(int minesToPlace, float chance, int rows, int cols)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // place mines
                if (boxValues[i, j] != 9 && Random.value <= chance)
                {
                    minesToPlace--;
                    boxValues[i, j] = 9;
                    if (minesToPlace == 0)
                    {
                        return 0;
                    }
                }
            }
        }
        return minesToPlace;
    }

    /** Destroy all grid objects to get ready for new game setup.
     * */
    public void Reset()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }

    /** Fits the grid on screen. To be called on new game or when screen settings changed.
     */
    public void fitToScreen()
    {
        int w = Screen.width;
        int h = Screen.height;
        boxSize = Mathf.Min(h / boxStates.GetLength(0), w / boxStates.GetLength(1));
        setupGrid(boxStates.GetLength(0), boxStates.GetLength(1));
    }

    /** Adjust grid size after screen resolution has been adjusted.
     */
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
