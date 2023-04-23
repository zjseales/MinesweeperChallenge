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
    /* The background image for a mined square. */
    public Sprite mineBox;
    /* The interactable box prefab object. */
    public GameObject box;

    /* Lose condition */
    private bool lost;
    /* Win condition */
    private bool won;
    /* The size of each box in the grid. */
    private int boxSize;
    /* All revealed boxes */
    private int numRevealed;

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
        // check if game is over.
        if (lost || won)
        {
            // reset or exit options
            Debug.Log("Game is over, can not alter state");
            return;
        }
        // retrieve clicked box
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
        switchToRevealed((int)pos.x, (int)pos.y, selectedBox);
    }

    /** Switches unrevealed and unrevealed boxes to a revealed state.
     *<param name="x"> The x index of the box being revealed </param>
     *<param name="y"> The y index of the box being revealed </param>
     *<param name="boxToReveal"> The grid box object having it's state altered. </param>
     */
    private void switchToRevealed(int x, int y, GameObject boxToReveal)
    {
        // do not reveal if flagged, or if already revealed.
        if (boxStates[x, y] == -1 || boxStates[x, y] == 0)
        {
            return;
        }
        //set as revealed
        boxStates[x, y] = 0;
        // switch unrevealed box to revealed (if no mine)
        if (!isMine(new Vector2(x, y)))
        {
            boxToReveal.GetComponent<Image>().sprite = revealedBox;
            numRevealed++;
            if (boxValues[x, y] != 0)
            {
                boxToReveal.transform.GetChild(0).GetComponent<Text>().color = setColor(boxValues[x, y]);
                boxToReveal.transform.GetChild(0).GetComponent<Text>().text = boxValues[x, y].ToString();
            }
            else
            {
                chainReveal(x, y);
            }
            if (checkWin())
            {
                // Show win screen
                Debug.Log("Game is won");
            }
        }
        else
        {
            // reveals mine and sets loss condition to true.
            lost = true;
            boxToReveal.GetComponent<Image>().sprite = mineBox;
            boxToReveal.transform.GetChild(2).gameObject.SetActive(true);
            Debug.Log("Game is lost");
        }
    }

    /** Determines the color of text given the input value.
     *<param> The integer value of the revealed box. </param>
     *<returns> The Color associated with the integer. </returns>
     */
    private Color setColor(int v)
    {
        Color c = Color.black;
        switch(v.ToString())
        {
            case "1":
                c = Color.blue;
                break;
            case "2":
                c = new Color(0, 0.4f, 0, 1);
                break;
            case "3":
                c = Color.red;
                break;
            case "4":
                c = new Color(0.3f, 0, 0.6f, 1);
                break;
            case "5":
                c = new Color(0.4f, 0, 0, 1);
                break;
            case "6":
                c = new Color(0, 0.6f, 0.6f, 1);
                break;
            case "7":
                c = Color.black;
                break;
            case "8":
                c = Color.grey;
                break;
        }
        return c;
    }

    /** Determines whether the game has been won.
     *<returns> A boolean defining whether the game has been won or not. </returns>
     */
    private bool checkWin()
    {
        if ((numRevealed + numMines) == (boxStates.GetLength(0) * boxStates.GetLength(1)))
        {
            won = true;
            return won;
        }
        return false;
    }

    /** Reveals all neighbouring boxes that have no neighbouring mines.
     *<param name="x"> The x index of the square having it's neighbours revealed. </param>
     *<param name="y"> The y index of the square having it's neighbours revealed. </param>
     */
    private void chainReveal(int x, int y)
    {
        int nextX;
        int nextY;
        for (int i = -1; i < 2; i++)
        {
            nextX = x + i;
            for (int j = -1; j < 2; j++)
            {
                nextY = y + j;
                //ensure in bounds
                if (nextY < boxStates.GetLength(1) && nextY >= 0 && nextX >= 0 && nextX < boxStates.GetLength(0))
                {
                    GameObject nextBox = GameObject.Find("b" + nextX + "_" + nextY);
                    switchToRevealed(nextX, nextY, nextBox);
                }
            }
        }
    }

    /** Returns the vector position of the grid square using the name parameter.
     *<param name="name"> The name of the grid square having it's position returned. </param>
     *<returns> The position of the input parameter in the grid space. </returns>
     */
    public Vector2 getPosition(string name)
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
     *  Initialises all global fields.
     */
    public void newGame()
    {
        int rows = boxStates.GetLength(0);
        int cols = boxStates.GetLength(1);
        won = false;
        lost = false;
        numRevealed = 0;
        fitToScreen();
        // initialize grid box states and values
        initMines(rows, cols);
        initStates(rows, cols);
    }

    /** Initialise all mines in the new game.
     *<param name="rows"> The number of rows in the grid space. </param>
     *<param name="cols"> The number of columns in the grid space. </param>
     */
    private void initMines(int rows, int cols)
    {
        float chance = numMines / (rows * cols);
        int minesToPlace = placeMines(numMines, chance, rows, cols);
        // if still mines to place, iterate til none left
        while (minesToPlace > 0)
        {
            // chance increases on each iteration
            // to avoid long wait times
            chance += 0.01f;
            minesToPlace = placeMines(minesToPlace, chance, rows, cols);
        }
    }

    /** Returns a boolean defining whether the input vector position contains a mine.
     *<param name="check"> The 2D-Vector index of the neighbouring square being checked for a mine. </param>
     *<returns> A boolean defining whether a mine is stored in the Vector position </returns>
     */
    private bool isMine(Vector2 check) 
    {
        if (boxValues[(int)check.x, (int)check.y] == 9)
        {
            return true;
        }
        return false;
    }

    /** Place mines iteration.
     *<param name="minesToPlace"> The number of mines still needed to place in the grid. </param>
     *<param name="chance"> The probability that an grid square will contain a mine. </param>
     *<param name="rows"> The number of rows in the grid space. </param>
     *<param name="cols"> The number of columns in the grid space. </param>
     *<returns> The number of mines still to place after iteration. </returns>
     */
    private int placeMines(int minesToPlace, float chance, int rows, int cols)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // place mines
                if (!isMine(new Vector2(i, j)) && Random.value <= chance)
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
     *<param name="rows"> The number of rows in the grid space. </param>
     *<param name="cols"> The number of columns in the grid space. </param>
     */
    private void initStates(int rows, int cols)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                //initialise states
                GameObject temp = GameObject.Instantiate(this.box, new Vector3(i, j, -5f), Quaternion.identity);
                temp.name = "b" + i + "_" + j;
                boxStates[i, j] = 1;
                temp.transform.position = this.transform.position;
                temp.GetComponent<RectTransform>().SetParent(this.transform);

                // initialise unseen values (representing neighbouring mines)
                if (boxValues[i, j] != 9)
                {
                    boxValues[i, j] = countNeighbours(i, j);
                }
            }
        }
    }

    /** Counts all neighbouring mines to the psotion defined by the arguments
     *  and returns the resulting value.
     *<param name="x"> The x index of the grid square being analyzed. </param>
     *<param name="y"> The y index of the grid square being analyzed. </param>
     *<returns> The number of neighbouring mines. </returns>
     */
    private int countNeighbours(int x, int y)
    {
        int n = 0;
        for (int i = x-1; i < x+2; i++)
        {
            for (int j = y - 1; j < y + 2; j++)
            {
                //ensure in bounds
                if (j < boxStates.GetLength(1) && j >= 0 && i >= 0 && i < boxStates.GetLength(0))
                {
                    if (isMine(new Vector2(i, j)))
                    {
                        n++;
                    }
                }
            }
        }
        return n;
    }

}
