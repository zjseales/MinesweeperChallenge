using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

/** ToggleFlag.cs
 *  MinesweeperUnity project - Personal challenge 2023
 *  
 *  Class that detects right click on buttons and toggles flags on/off.
 *  @author Zac Seales
 */

public class ToggleFlag : MonoBehaviour, IPointerClickHandler
{
    /* The current active game state. */
    private GameObject activeGameState;

    /** Turn off flag and mine images on instantiation.
     */
    void Start()
    {
        this.transform.GetChild(1).gameObject.SetActive(false);
        this.transform.GetChild(2).gameObject.SetActive(false);
        activeGameState = GameObject.Find("Game");
    }

    /** Detects right click and toggles the flag on the grid square that was selected.
     * <param name="eventData"> The click event that occurs. </param>
     */
    public void OnPointerClick(PointerEventData eventData)
    {
        // Detects right click and toggles a flag, if able.
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Vector2 pos = getPosition(name);
            int[,] states = activeGameState.GetComponent<GameBoard>().boxStates;
            if (states[(int)pos.x, (int)pos.y] != 0)
            {
                toggleFlag((int)pos.x, (int)pos.y, this.transform.GetChild(1).gameObject, states);
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

    /** Toggles flag on/off.
     * <param name="x"> The x coordinate of the square having its flag toggled. </param>
     * <param name="y"> The y coordinate of the square being toggled. </param>
     * <param name="flag"> The flag child object being toggled. </param>
     * <param name="states"> The grid states to be changed. </param>
     */
    private void toggleFlag(int x, int y, GameObject flag, int[,] states)
    {
        states[x, y] *= -1;
        if (states[x, y] == 1)
        {
            flag.SetActive(false);
        }
        else
        {
            flag.SetActive(true);
        }
        // update main record of game state
        activeGameState.GetComponent<GameBoard>().boxStates = states;
    }
}
