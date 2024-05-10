using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapController : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] GameObject[] tileMapObjects;
    [SerializeField]GameObject currentTile;
    [SerializeField] Vector2 pos;
    bool left;
    bool right;
    bool up;
    bool down;
    bool leftDown;
    bool rightDown;
    bool leftUp;
    bool rightUp;
    [SerializeField]GameObject rTile = null;
    [SerializeField]GameObject lTile = null;
    [SerializeField]GameObject uTile = null;
    [SerializeField]GameObject dTile = null;
    [SerializeField]GameObject luTile = null;
    [SerializeField]GameObject ruTile = null;
    [SerializeField]GameObject ldTile = null;
    [SerializeField]GameObject rdTile = null;

    enum directions {left, right, up, down, upLeft, upRight, downLeft, downRight}

    void Start()
    {
        playerTransform = FindAnyObjectByType<PlayerController>().transform;    
        currentTile = tileMapObjects[0];
    }

    // Update is called once per frame
    void Update()
    {
        TransitionToNewTile();
        pos = playerTransform.position;
        Vector2 tilePos = currentTile.transform.position;
        if (pos.x > tilePos.x + 15) //Right
        {
            if (!right)
            {
                right = true;
                rTile = GetDisabledTile();
                ChangeTile(tilePos.x + 100, tilePos.y, rTile);
            }        
        }
        else if(right)
        {
            rTile.SetActive(false);
            rTile = null;
            right = false;
        }
        // END
        if (pos.x < tilePos.x - 15) //Left
        {
            if(!left)
            {   
                left = true;
                lTile = GetDisabledTile();
                ChangeTile(tilePos.x - 100, tilePos.y, lTile);
            }    
        }
        else if (left)
        {
            lTile.SetActive(false);
            lTile = null;
            left = false;
        }
        // END
        if (pos.y > tilePos.y + 25)  //Up
        {
            if(!up)
            {   
                up = true;
                uTile = GetDisabledTile();
                ChangeTile(tilePos.x, tilePos.y +100, uTile);
            }
        }
        else if (up)
        {
            uTile.SetActive(false);
            uTile = null;
            up = false;
        }
        // END
        if (pos.y < tilePos.y - 25) //Down
        {
            if(!down)
            {   
                down = true;
                dTile = GetDisabledTile();
                ChangeTile(tilePos.x, tilePos.y -100, dTile);
            }
        }
        else if (down)
        {
            dTile.SetActive(false);
            dTile = null;
            down = false;
        }
        //END

        //DIAGANOL UPS
        if (left && up) 
        {
            if (!leftUp)
            {   
                leftUp = true;
                luTile = GetDisabledTile();
                ChangeTile(tilePos.x - 100, tilePos.y + 100, luTile);
            }

        }
        else if(leftUp)
        {
            luTile.SetActive(false);
            luTile = null;
            leftUp = false;
        }
        //END
        if(right && up)
        {
            if(!rightUp)
            {   
                rightUp = true;
                ruTile = GetDisabledTile();
                ChangeTile(tilePos.x + 100, tilePos.y + 100, ruTile);
            }

        }
        else if (rightUp)
        {
            ruTile.SetActive(false);
            ruTile = null;
            rightUp = false;
        }

        // DIAGANOL DOWNS
        if (left && down)
        {
            if(!leftDown)
            {   
                leftDown = true;
                ldTile = GetDisabledTile();
                ChangeTile(tilePos.x - 100, tilePos.y - 100, ldTile);
            }

        }
        else if (leftDown)
        {
            ldTile.SetActive(false);
            ldTile = null;
            leftDown = false;
        }
        //END
        if (right && down)
        {
            if (!rightDown)
            {   
                rightDown = true;
                rdTile = GetDisabledTile();
                ChangeTile(tilePos.x + 100, tilePos.y - 100, rdTile);
            }

        }
        else if (rightDown)
        {
            rdTile.SetActive(false);
            rdTile = null;
            rightDown = false;
        }
        //END
    }
    void ChangeTile(float x, float y, GameObject obj)
    {
        obj.transform.position = new Vector3(x, y, 0);
        obj.SetActive(true);
    }
    GameObject GetDisabledTile()
    {
        foreach (GameObject obj in tileMapObjects)
        {
            if (obj.activeSelf == false)
            {
                return obj.transform.gameObject;
            }
        }
        return null;
    }

    void TransitionToNewTile()
    {
        //if (Mathf.Abs(pos.x) < Mathf.Abs(currentTile.transform.position.x) + 50 && Mathf.Abs(pos.y) < Mathf.Abs(currentTile.transform.position.y) + 50)
        //    return;
        //    print("adasdaw");

        float distance = 55; //55
        GameObject temp = currentTile;
        if (pos.x > currentTile.transform.position.x + distance)
        {
            currentTile = rTile;
            rTile = temp;
            ResetPositions();
            return;
        }
        if(pos.x < currentTile.transform.position.x - distance)
        {
            currentTile = lTile;
            lTile = temp;
            ResetPositions();
            return;
        }
        if (pos.y > currentTile.transform.position.y + distance)
        {
            currentTile = uTile;
            uTile = temp;
            ResetPositions();
            return;
        }
        if (pos.y < currentTile.transform.position.y - distance)
        {
            currentTile = dTile;

            dTile = temp;
            ResetPositions();
            return;
        }
    }
    void ResetPositions()
    {
        left = false; right = false; up = false; down = false; leftUp = false; rightUp = false; leftDown = false; rightDown = false;
        if(lTile)lTile.SetActive(false);
        if(ldTile)ldTile.SetActive(false);
        if(luTile)luTile.SetActive(false);
        if(uTile)uTile.SetActive(false);
        if(dTile)dTile.SetActive(false);
        if(rTile)rTile.SetActive(false);
        if(rdTile)rdTile.SetActive(false);
        if(ruTile)ruTile.SetActive(false);
        lTile = null; ldTile = null; luTile = null; uTile = null; dTile = null; rTile = null; rdTile = null; ruTile = null;
    }

    //void ChangeTile(directions direction)
    //{
    //    switch (direction) 
    //    {
    //        case directions.down:
    //            break;
    //        case directions.up:
    //            break;
    //        case directions.left:
    //            break;
    //        case directions.right:
    //            break;
    //        case directions.upLeft:
    //            break;
    //        case directions.upRight:
    //            break;
    //        case directions.downLeft:
    //            break;
    //        case directions.downRight:
    //            break;
                
    //    }
    //}
}
