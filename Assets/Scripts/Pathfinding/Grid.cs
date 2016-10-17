using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Direction {
    Right,
    Left,
    Top,
    Bottom,
    BottomLeft,
    BottomRight,
    TopLeft,
    TopRight,
}

public class Grid : MonoBehaviour {

    public Vector2 Offset;

    public int Width;
    public int Height;

    public Node[,] Nodes;

    public int Left { get { return 0; } }
    public int Right { get { return Width; } }
    public int Bottom { get { return 0; } }
    public int Top { get { return Height; } }

    public const float UnitSize = 0.5f;

    private LineRenderer LineRenderer;
    GameObject Player;

    void Awake() {
        if (Nodes == null)
            Setup();
    }
    public void Setup() {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        LineRenderer = transform.GetComponent<LineRenderer>();

        //Get grid dimensions
        Offset = this.transform.position;

        Width = ((int)this.transform.localScale.x) * 2 + 2;
        Height = ((int)this.transform.localScale.y) * 2 + 2;

        Nodes = new Node[Width, Height];

        //Initialize the grid nodes - 1 grid unit between each node
        //We render the grid in a diamond pattern
        for (int x = 0; x < Width / 2; x++) {
            for (int y = 0; y < Height; y++) {
                float ptx = x;
                float pty = -(y / 2) + (UnitSize / 2f);
                int offsetx = 0;

                if (y % 2 == 0) {
                    ptx = x + (UnitSize / 2f);
                    offsetx = 1;
                } else {
                    pty = -(y / 2);
                }

                Vector2 pos = new Vector2(ptx, pty) + Offset;
                Node node = new Node(x * 2 + offsetx, y, pos, this);
                Nodes[x * 2 + offsetx, y] = node;
            }
        }

        //Create connections between each node
        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                if (Nodes[x, y] == null) continue;
                Nodes[x, y].InitializeConnections(this);
            }
        }

        //Pass 1, we removed the bad nodes, based on valid connections
        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                if (Nodes[x, y] == null)
                    continue;

                Nodes[x, y].CheckConnectionsPass1(this);
            }
        }

        //Pass 2, remove bad connections based on bad nodes
        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                if (Nodes[x, y] == null)
                    continue;

                Nodes[x, y].CheckConnectionsPass2();
                //Nodes[x, y].DrawConnections ();	//debug
            }
        }
    }

    public Point WorldToGrid(Vector2 worldPosition) {
        Node closestNode = null;
        float distance = 1000000f;

        foreach (Node cur in Nodes) {
            if (cur != null) {
                if (cur.BadNode) continue;
                float curDistance = Vector2.Distance(cur.Position, worldPosition);

                if (curDistance < distance) {
                    closestNode = cur;
                    distance = curDistance;
                }
            }
        }

        if (closestNode == null)
            return null;

        return new Point(closestNode.X, closestNode.Y);
    }

    public Vector2 GridToWorld(Point gridPosition) {
        return Nodes[gridPosition.X, gridPosition.Y].Position;
    }

    public bool ConnectionIsValid(Point point1, Point point2) {
        //comparing same point, return false
        if (point1.X == point2.X && point1.Y == point2.Y)
            return false;

        if (Nodes[point1.X, point1.Y] == null)
            return false;

        //determine direction from point1 to point2
        Direction direction = Direction.Bottom;

        if (point1.X == point2.X) {
            if (point1.Y < point2.Y)
                direction = Direction.Bottom;
            else if (point1.Y > point2.Y)
                direction = Direction.Top;
        } else if (point1.Y == point2.Y) {
            if (point1.X < point2.X)
                direction = Direction.Right;
            else if (point1.X > point2.X)
                direction = Direction.Left;
        } else if (point1.X < point2.X) {
            if (point1.Y > point2.Y)
                direction = Direction.TopRight;
            else if (point1.Y < point2.Y)
                direction = Direction.BottomRight;
        } else if (point1.X > point2.X) {
            if (point1.Y > point2.Y)
                direction = Direction.TopLeft;
            else if (point1.Y < point2.Y)
                direction = Direction.BottomLeft;
        }

        //check connection
        switch (direction) {
            case Direction.Bottom:
                if (Nodes[point1.X, point1.Y].Bottom != null)
                    return Nodes[point1.X, point1.Y].Bottom.Valid;
                else
                    return false;

            case Direction.Top:
                if (Nodes[point1.X, point1.Y].Top != null)
                    return Nodes[point1.X, point1.Y].Top.Valid;
                else
                    return false;

            case Direction.Right:
                if (Nodes[point1.X, point1.Y].Right != null)
                    return Nodes[point1.X, point1.Y].Right.Valid;
                else
                    return false;

            case Direction.Left:
                if (Nodes[point1.X, point1.Y].Left != null)
                    return Nodes[point1.X, point1.Y].Left.Valid;
                else
                    return false;

            case Direction.BottomLeft:
                if (Nodes[point1.X, point1.Y].BottomLeft != null)
                    return Nodes[point1.X, point1.Y].BottomLeft.Valid;
                else
                    return false;

            case Direction.BottomRight:
                if (Nodes[point1.X, point1.Y].BottomRight != null)
                    return Nodes[point1.X, point1.Y].BottomRight.Valid;
                else
                    return false;

            case Direction.TopLeft:
                if (Nodes[point1.X, point1.Y].TopLeft != null)
                    return Nodes[point1.X, point1.Y].TopLeft.Valid;
                else
                    return false;

            case Direction.TopRight:
                if (Nodes[point1.X, point1.Y].TopRight != null)
                    return Nodes[point1.X, point1.Y].TopRight.Valid;
                else
                    return false;

            default:
                return false;
        }
    }
}



