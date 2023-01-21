using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    [SerializeField] Transform botLeft;
    [SerializeField] Transform topRight;
    Dictionary<Vector2Int,PathNode> dict;
    Vector2 gridSize;

    void InitializeGrid(Vector2Int end) {
        dict = new Dictionary<Vector2Int, PathNode>();
        Vector2 gridSizeFloat = topRight.position - botLeft.position;
        gridSize = new Vector2(Mathf.FloorToInt(gridSizeFloat.x)+1,Mathf.FloorToInt(gridSizeFloat.y)+1);

        for(int x = 0; x < gridSize.x; x++) {
            for (int y = 0; y < gridSize.y; y++) {
                PathNode newNode =  new PathNode(new Vector2Int(x,y), 0, end, null);
                RaycastHit2D hit = Physics2D.Raycast(botLeft.position + new Vector3(x,y,0), -Vector2.up, .4f);
                if (hit.collider == null) {
                    newNode.isWalkable = true;
                } else {
                    if (hit.collider.CompareTag("Wall")) {
                        newNode.isWalkable = false;
                    }
                }
                dict.Add(new Vector2Int(x,y), newNode);
            }
        }
    }

    Vector2Int ConvertPositionToGrid(Vector2 pos) {
        Vector2 newPos = new Vector3(pos.x,pos.y,0) - botLeft.position;
        return (new Vector2Int(Mathf.FloorToInt(newPos.x), Mathf.FloorToInt(newPos.y)));
    }

    PathNode GetLowestFCost(List<PathNode> list) {
        PathNode smallest = list[0];
        for(int i = 1; i < list.Count; i++) {
            if (list[i].f < smallest.f) {
                smallest = list[i];
            }
        }
        return smallest;
    }

    List<string> TracePath(PathNode end) {
        List<string> strList = new List<string>();
        
        PathNode curNode = end;

        while (curNode.cameFrom != null) {
            if (curNode.x < curNode.cameFrom.x) {
                strList.Insert(0, "left");
            } else if (curNode.x > curNode.cameFrom.x) {
                strList.Insert(0, "right");
            } else if (curNode.y < curNode.cameFrom.y) {
                strList.Insert(0, "down");
            } else if (curNode.y > curNode.cameFrom.y) {
                strList.Insert(0, "up");
            }
            curNode = curNode.cameFrom;
        }

        return strList;
    }

    public List<string> FindPath(Vector2 start, Vector2 end) {
        return FindPath(ConvertPositionToGrid(start), ConvertPositionToGrid(end));
    }

    public List<string> FindPath(Vector2Int start, Vector2Int end) {
        InitializeGrid(end);

        List<PathNode> openList = new List<PathNode>();

        dict[start].setG(0, null);
        openList.Add(dict[start]);

        PathNode curPos;
        PathNode endPos = dict[end];
        while (openList.Count > 0) {
            curPos = GetLowestFCost(openList);

            if (curPos == endPos) {
                break;
            }

            if (curPos.x > 0) {
                if (dict[curPos.pos + Vector2Int.left].isWalkable) {
                    PathNode nextPath = dict[curPos.pos + Vector2Int.left];
                    if (nextPath.setG(curPos.g + 1, curPos)) {
                        openList.Add(nextPath);
                    }
                }
            }
            if (curPos.x < gridSize.x-1) {
                if (dict[curPos.pos + Vector2Int.right].isWalkable) {
                    PathNode nextPath = dict[curPos.pos + Vector2Int.right];
                    if (nextPath.setG(curPos.g + 1, curPos)) {
                        openList.Add(nextPath);
                    }
                }
            }
            if (curPos.y > 0) {
                if (dict[curPos.pos + Vector2Int.down].isWalkable) {
                    PathNode nextPath = dict[curPos.pos + Vector2Int.down];
                    if (nextPath.setG(curPos.g + 1, curPos)) {
                        openList.Add(nextPath);
                    }
                }
            }
            if (curPos.y < gridSize.y-1) {
                if (dict[curPos.pos + Vector2Int.up].isWalkable) {
                    PathNode nextPath = dict[curPos.pos + Vector2Int.up];
                    if (nextPath.setG(curPos.g + 1, curPos)) {
                        openList.Add(nextPath);
                    }
                }
            }
            openList.Remove(curPos);
        }

        return TracePath(endPos);
    }
}
