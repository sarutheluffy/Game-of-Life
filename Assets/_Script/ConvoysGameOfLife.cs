using UnityEngine;
using System.Collections;

public enum State
{ 
    INPUT,PLAY
}

public class ConvoysGameOfLife : MonoBehaviour
{
    public GameObject _prefabCell;
	 public static State _state;
     public float _timeGap;//time between the update interval;
     private float startTime;
    /// <summary>
    /// Needed for the size of Grid i.e MxN size of Grid 
    /// </summary>
    public const int width = 45;
    public const int height = 23;
    public GameObject[,] cells;

    /// <summary>
    /// white will show the dead cell in the grid while the yellow color will 
    /// show the cells is live 
    /// </summary>
    public static Color deadColor = Color.white;
    public static Color liveColor = Color.yellow;

    //Need to set and get the color of the cells whether they are dead or live
    private Renderer[,] renderer;
    private bool[,] isLive;
    bool[,] board;




    void Start()
    {
        board = new bool[height, width];
        startTime = 0;
		_state = State.INPUT;
        cells = new GameObject[height, width];
        renderer = new Renderer[height, width];
        isLive = new bool[height, width];
        GiveMeTheGrid(height, width);

    }


    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (_state == State.PLAY)
            {
                _state = State.INPUT;
            }
            else
            {
                _state = State.PLAY;

            }
        }
        if (_state != State.INPUT && _timeGap<=Time.time-startTime)
        {
            startTime = Time.time;
            UpdateGrid();
            DrawGrid();
        }
    }

    private void GiveMeTheGrid(int height, int width)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject obj = Instantiate(_prefabCell, new Vector3(transform.position.x + j, transform.position.y - i, transform.position.z), Quaternion.identity) as GameObject;
				CellProperty property = obj.GetComponent<CellProperty>();
                property.propertyI = i;
                property.propertyJ = j;
				cells[i, j] = obj;
                renderer[i, j] = obj.GetComponent<Renderer>();
                isLive[i, j] = false;

            }
        }

    }
    
    private void DrawGrid()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                CopyBoardArrayToIsLive(i,j);
                renderer[i, j].material.color = isLive[i, j] ? ConvoysGameOfLife.liveColor : ConvoysGameOfLife.deadColor;
            }
        }

    }

    private void CopyBoardArrayToIsLive(int i, int j)
    {

        isLive[i, j] = board[i, j];
    }

	public void MakeThisCellDead(int x, int y)
    {
        isLive[x, y] = false;
        renderer[x, y].material.color = ConvoysGameOfLife.deadColor;
    }
    public void MakeThisCellAlive(int x, int y)
    {
        isLive[x, y] = true;
        renderer[x, y].material.color = ConvoysGameOfLife.liveColor;
    }

	
    private void UpdateGrid()
    {
        int numberOfNeighbor=0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                numberOfNeighbor=GetNumberOfLiveNeighbour(i,j);
               // Debug.Log("the number of neighbor at i = " + i + " and j = " + j + "is = " + numberOfNeighbor);
                if (numberOfNeighbor < 2 || numberOfNeighbor > 3)
                {
                    board[i, j] = false;
                }
                else if (numberOfNeighbor == 3)
                {
                    board[i, j] = true;
                }
                else
                {
                    board[i, j] = isLive[i, j];
                }
            }
        }

       // CopyArray();

    }

    //private void CopyArray()
    //{
    //    for (int i = 0; i < height; i++)
    //    {
    //        for (int j = 0; j < width; j++)
    //        {
    //            isLive[i, j] = board[i, j];
    //        }
    //    }
    //}


    private int GetNumberOfLiveNeighbour(int x, int y)
    {
        int value = 0;
        switch (x)
        {
            case 0:
                if (y == 0)
                {
                    //upper left corner
                    value = GetNeighbors(x, y, false, true, false, true);
                }
                else if (y == width - 1)
                {
                    //upper right corner
                    value = GetNeighbors(x, y, false, true, true, false);
                }
                else
                {
                    //upper bound
                    value = GetNeighbors(x, y, false, true, true, true);
                }
                break;
            case height - 1:
                if (y == 0)
                {
                    //lower left corner
                    value = GetNeighbors(x, y, true, false, false, true);
                }
                else if (y == width - 1)
                {
                    //lower right corner
                    value = GetNeighbors(x, y, true, false, true, false);
                }
                else
                {
                    //lower bound
                    value = GetNeighbors(x, y, true, false, true, true);
                }
                break;
            default:
                switch (y)
                {
                    case 0:
                        //left bound
                        value = GetNeighbors(x, y, true, true, false, true);
                        break;
                    case width - 1:
                        //right bound
                        value = GetNeighbors(x, y, true, true, true, false);
                        break;
                    default:
                        //middle 
                        value = GetNeighbors(x, y, true, true, true, true);
                        break;
                }
                break;
        }
        return value;
    }

    private int GetNeighbors(int x, int y, bool isAbove, bool isBelow, bool isLeft, bool isRight)
    {

        int value = 0;
        if (isLeft)
        {
            if (isRight)
            {
                value += CheckLeftMiddleRight(x, y);
                if (isAbove)
                {
                    value += CheckLeftMiddleRight(x - 1, y);
                }
                if (isBelow)
                {
                    value += CheckLeftMiddleRight(x + 1, y);
                }
            }
            else if (isAbove)
            {
                value += CheckLeftMiddle(x - 1, y);
                if (isBelow)
                {
                    value += CheckLeftMiddle(x + 1, y);
                }
            }
            else if (isBelow)
            {
                value += CheckLeftMiddle(x + 1, y);
            }

        }
        else if (isRight)
        {
            if (isAbove)
            {
                value += CheckRightMiddle(x - 1, y);
                if (isBelow)
                {
                    value += CheckRightMiddle(x + 1, y);
                }
            }
            else if (isBelow)
            {
                value += CheckRightMiddle(x + 1, y);
            }

        }


        return (value-(isLive[x,y]?1:0));
    }
    private int CheckLeftMiddleRight(int x, int y)
    {
        int value = 0;
        for (int i = -1; i <= 1; i++)
        {
            value += isLive[x, y + i] ? 1 : 0;
        }
        return value;
    }
    private int CheckLeftMiddle(int x, int y)
    {
        int value = 0;
        value += isLive[x, y] ? 1 : 0;
        value += isLive[x, y-1] ? 1 : 0;
        return value;
    }
    private int CheckRightMiddle(int x, int y)
    {
        int value = 0;
        value += isLive[x, y] ? 1 : 0;
        value += isLive[x, y+1] ? 1 : 0;
        return value;
    }

}
