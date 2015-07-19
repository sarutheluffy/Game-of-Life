using UnityEngine;
using System.Collections;

public class CellProperty : MonoBehaviour {

    public int propertyI { get; set; }
    public int propertyJ { get; set; }
    private bool isAlive;

    private ConvoysGameOfLife _gameOfLife;
    void Start()
    {
        isAlive = false;
        _gameOfLife = GameObject.Find("Controller").GetComponent<ConvoysGameOfLife>();
    }
    void OnMouseDown()
    {
        if (ConvoysGameOfLife._state != State.PLAY)
        {
            if (isAlive)
            {
                _gameOfLife.MakeThisCellDead(propertyI, propertyJ);
                isAlive = false;
            }
            else
            {
                _gameOfLife.MakeThisCellAlive(propertyI, propertyJ);
                isAlive = true;
            }
        }   
    }
}
