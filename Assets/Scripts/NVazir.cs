using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NVazir : MonoBehaviour
{
    public GameObject[] queens;
    public GameObject[,] position;
    public bool[,] empty;
   public List<GameObject> sol;
  public  List<GameObject> pos;
   public  List<bool> placepos;
    public float show_speed=1f;
    // Start is called before the first frame update
    void Start()
    {
        sol = new List<GameObject>();
        empty = new bool[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                empty[i, j] = true;
            }
        }
        position = new GameObject[8, 8];
        int temp = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                position[i, j] = queens[temp];
                temp++;
            }
        }
        if (BackTracking_Search(sol, 0))
        {
            Debug.Log("Happy");
            StartCoroutine(Show());
        }
        else
        {
            Debug.Log("Sad");
        }
    }
 
    public bool Check(int row, int column)
    {
        for (int i = 1; i < 9; i++)
        {
            if ((row + i >= 8) || (column + i >= 8))
                break;
            else
            {
                if (!empty[row + i, column + i])
                {
                    return false;
                }
            }
          

        }
        for (int i = 1; i < 9; i++)
        {
            if (row - i < 0 || column - i < 0)
                break;
            if (!empty[row - i, column - i])
            {
                return false;
            }

        }
        return true;
    }
    public bool CheckReverse(int row, int column)
    {
        for (int i = 1; i < 9; i++)
        {
            if ((row - i <0) || (column + i >= 8))
                break;
            else
            {
                if (!empty[row - i, column + i])
                {
                    return false;
                }
            }


        }
        for (int i = 1; i < 9; i++)
        {
            if (row + i >= 8 || column - i < 0)
                break;
            if (!empty[row + i, column - i])
            {
                return false;
            }

        }
        return true;
    }
    public bool CheckRow(int row, int column)
    {
        for (int i = 0; i < 8; i++)
        {
            if (i == column)
                continue;
            if (!empty[row, i])
            {
                return false;
            }
        }
        return true;
    }
    public bool CheckColumn(int row, int column)
    {
        for (int i = 0; i < 8; i++)
        {
            if (i == row)
                continue;
            if (!empty[i, column])
            {
                return false;
            }
        }
        return true;
    }

    public IEnumerator PlaceQueen(int row , int column)
    {
        
        position[row, column].GetComponent<Queen>().x = row;
        position[row, column].GetComponent<Queen>().y = column;
        position[row, column].SetActive(true);
        pos.Add(position[row, column]);
        placepos.Add(true);
        yield return new WaitForSeconds(.2f);
    }
    public IEnumerator RemoveQueen(int row, int column)
    {
        pos.Add(position[row, column]);
        placepos.Add(false);
        position[row, column].GetComponent<Queen>().x = row;
        position[row, column].GetComponent<Queen>().y = column;
        position[row, column].SetActive(false);
        yield return new WaitForSeconds(.2f);
    }
    public bool BackTracking_Search(List<GameObject> sol, int column)
    {
        if (sol.Count == 8)
        {
            return true;
        }

        bool found = false;
        for (int j = 0; j < 8; j++)
        {
            if (empty[j, column])
            {

                StartCoroutine(PlaceQueen(j,column));
             
                if (Check(j, column) && CheckColumn(j, column) && CheckRow(j, column)&&CheckReverse(j,column))
                {
                    sol.Add(position[j, column]);
                    empty[j, column] = false;
                    found = true;
                    if (column <= 7)
                    {
                        if (BackTracking_Search(sol, column + 1))
                        {
                            found = true;
                        
                            return true;
                        }
                        else
                        {
                        Debug.Log(column + "<<<" + j);
                        empty[j, column] = true;
                        found = false;
                            sol.Remove(position[j, column]);
                            StartCoroutine(RemoveQueen(j, column));
                          
                        }
                    }
                    else
                    {
                        return true;
                    }


                }
                else
                {
                    found = false;
                    empty[j, column] = true;
                    StartCoroutine(RemoveQueen(j,column));
                   

                }
               
            }

        }
        if (!found)
        {
            return false;
        }
        else
            return true;

    }
  
    public IEnumerator Show()
    {
        int count = 0;
        foreach(bool item in placepos)
        {
            if(item)
            {
                pos[count].SetActive(true);
                pos[count].GetComponent<Renderer>().enabled = true;
                yield return new WaitForSeconds(show_speed);
                count++;
            }
            else
            {
                pos[count].SetActive(false);
                pos[count].GetComponent<Renderer>().enabled = false;
                yield return new WaitForSeconds(show_speed);
                count++;
            }
        }
    }
  
}
