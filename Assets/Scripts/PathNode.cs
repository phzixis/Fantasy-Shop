using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int g;
    public float h;
    public float f {
        get {return g + h;}
    }

    public Vector2Int pos;

    public int x {
        get { return pos.x; }
    }
    public int y {
        get { return pos.y; }
    }

    public bool isWalkable = true;
    public bool used = false;
    public PathNode cameFrom;

    public PathNode(Vector2Int _pos, int _g, float _h, PathNode _c) {
        pos = _pos;
        g = _g;
        h = _h;
        cameFrom = _c;
    }

    public PathNode(Vector2Int _pos, int _g, Vector2Int _end, PathNode _c) {
        pos = _pos;
        g = _g;
        h = Vector2Int.Distance(_pos, _end);
        cameFrom = _c;
    }

    public bool setG(int _g, PathNode _c) {
        if (used) return false;
        g = _g;
        cameFrom = _c;
        used = true;
        return true;
    }

}
