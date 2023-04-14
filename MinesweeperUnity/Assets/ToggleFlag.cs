using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ToggleFlag : MonoBehaviour, IPointerEnterHandler
{
    // Turn off flag on instantiation
    void Start()
    {
        this.transform.GetChild(1).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    /** Determines the name of the object being hovered
     */
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Debug.Log("Currently hovering " + name);
        GameObject flag = GameObject.Find(name).transform.GetChild(1).gameObject;
        Vector2 pos = getPosition(name);

        //toggle flag on right click
        if (Input.GetMouseButtonDown(1))
        {
            toggleFlag((int)pos.x, (int)pos.y, this.transform.GetChild(1).gameObject);
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

    private void toggleFlag(int x, int y, GameObject b)
    {
        b.SetActive(true);
    }
}
