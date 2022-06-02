using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Grid : MonoBehaviour
{
    private float cameraSizeConstant = 0.6f;
    private int score;
    private int n;
    private bool[,] matrix;
    private Node[,] nodesMatrix;

    [SerializeField] private GameObject node;
    [SerializeField] private Transform cameraPosition;

    [SerializeField] private GameObject entryPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text scoreText;

    private List<sub> list;

    public class sub
    {
        public KeyValuePair<int, int> v1;
        public KeyValuePair<int, int> v2;
        public KeyValuePair<int, int> v3;

        public sub(KeyValuePair<int, int> v1, KeyValuePair<int, int> v2, KeyValuePair<int, int> v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }

        public bool checkReverseEqual(KeyValuePair<int, int> v1, KeyValuePair<int, int> v2, KeyValuePair<int, int> v3)
        {
            if ((this.v1.Equals(v3) && this.v2.Equals(v2) && this.v3.Equals(v1)) || (this.v1.Equals(v1) && this.v2.Equals(v2) && this.v3.Equals(v3))
               || (this.v1.Equals(v2) && this.v2.Equals(v3) && this.v3.Equals(v1)) || (this.v1.Equals(v3) && this.v2.Equals(v1) && this.v3.Equals(v2))
               || (this.v1.Equals(v1) && this.v2.Equals(v3) && this.v3.Equals(v2)) || (this.v1.Equals(v2) && this.v2.Equals(v1) && this.v3.Equals(v3)))
            {
                return true;
            }
            return false;
        }

    }

    private void Start()
    {
        list = new List<sub>();  
        score = 0;
        scoreText.text = "SCORE: " + score;
    }

    public void startGameplay()
    {
        if (inputField.text.Length > 0 && int.TryParse(inputField.text, out n))
        {
            int.TryParse(inputField.text, out n);
            nodesMatrix = new Node[n, n];
            CreateGrid();
            gameplayPanel.SetActive(true);
            entryPanel.SetActive(false);
        }
    }

    private bool checkList(KeyValuePair<int, int> v1, KeyValuePair<int, int> v2, KeyValuePair<int, int> v3)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].checkReverseEqual(v1, v2, v3))
            {
                return true;
            }
        }

        return false;
    }

    public void pointNode(int x, int y)
    {
        if (matrix[x, y] == false)
        {
            matrix[x, y] = true;
            checkAll();
        }
    }

    private void CreateGrid()
    {
        matrix = new bool[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matrix[i, j] = false;

                GameObject spawnedNode = Instantiate(node, new Vector3(i, j), Quaternion.identity);
                Node currentNode = spawnedNode.GetComponent<Node>();             
                currentNode.SetValues(i, j);
                nodesMatrix[i,j] = currentNode;
            }
        }

        Camera.main.orthographicSize = n * cameraSizeConstant;
        cameraPosition.transform.position = new Vector3((float)n / 2 - 0.5f, (float)n / 2 - 0.5f, -10);
    }

    //

    void checkAll()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (matrix[i, j])
                {
                    checkOne(i, j);
                }
            }
        }

        for(int i = 0; i <list.Count; i++)
        {
            if(matrix[list[i].v1.Key, list[i].v1.Value])
            {
                matrix[list[i].v1.Key, list[i].v1.Value] = false;
                nodesMatrix[list[i].v1.Key, list[i].v1.Value].setEmptySprite();
            }

            if (matrix[list[i].v2.Key, list[i].v2.Value])
            {
                matrix[list[i].v2.Key, list[i].v2.Value] = false;
                nodesMatrix[list[i].v2.Key, list[i].v2.Value].setEmptySprite();
            }

            if (matrix[list[i].v3.Key, list[i].v3.Value])
            {
                matrix[list[i].v3.Key, list[i].v3.Value] = false;
                nodesMatrix[list[i].v3.Key, list[i].v3.Value].setEmptySprite();
            }
        }

        scoreText.text = "SCORE: " + score;
        list.Clear();

        if(score >= 1)
        {
            
        }
    }

    private void checkOne(int x, int y)
    {
        if (x + 2 < n && matrix[x, y] && matrix[x + 1, y] && matrix[x + 2, y]
            && !checkList(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x + 1, y), new KeyValuePair<int, int>(x + 2, y)))
        {
            score++;
            sub s = new sub(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x + 1, y), new KeyValuePair<int, int>(x + 2, y));
            list.Add(s);

        }

        if (x - 2 >= 0 && matrix[x, y] && matrix[x - 1, y] && matrix[x - 2, y]
            && !checkList(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x - 1, y), new KeyValuePair<int, int>(x - 2, y)))
        {
            score++;
            sub s = new sub(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x - 1, y), new KeyValuePair<int, int>(x - 2, y));
            list.Add(s);
        }

        if (y + 2 < n && matrix[x, y] && matrix[x, y + 1] && matrix[x, y + 2]
            && !checkList(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x, y + 1), new KeyValuePair<int, int>(x, y + 2)))
        {
            score++;
            sub s = new sub(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x, y + 1), new KeyValuePair<int, int>(x, y + 2));
            list.Add(s);
        }

        if (y - 2 >= 0 && matrix[x, y] && matrix[x, y - 1] && matrix[x, y - 2]
            && !checkList(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x, y - 1), new KeyValuePair<int, int>(x, y - 2)))
        {
            score++;
            sub s = new sub(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x, y - 1), new KeyValuePair<int, int>(x, y - 2));
            list.Add(s);
        }

        if (x + 1 < n && y + 1 < n && matrix[x, y] && matrix[x + 1, y] && matrix[x + 1, y + 1]
            && !checkList(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x + 1, y), new KeyValuePair<int, int>(x + 1, y + 1)))
        {
            score++;
            sub s = new sub(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x + 1, y), new KeyValuePair<int, int>(x + 1, y + 1));
            list.Add(s);
        }

        if (x + 1 < n && y + 1 < n && matrix[x, y] && matrix[x, y + 1] && matrix[x + 1, y + 1]      //
           && !checkList(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x, y + 1), new KeyValuePair<int, int>(x + 1, y + 1)))
        {
            score++;
            sub s = new sub(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x, y + 1), new KeyValuePair<int, int>(x + 1, y + 1));
            list.Add(s);
        }

        if (x + 1 < n && y - 1 >= 0 && matrix[x, y] && matrix[x + 1, y] && matrix[x + 1, y - 1]
            && !checkList(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x + 1, y), new KeyValuePair<int, int>(x + 1, y - 1)))
        {
            score++;
            sub s = new sub(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x + 1, y), new KeyValuePair<int, int>(x + 1, y - 1));
            list.Add(s);
        }

        if (x + 1 < n && y - 1 >= 0 && matrix[x, y] && matrix[x, y - 1] && matrix[x + 1, y - 1]    //
            && !checkList(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x, y - 1), new KeyValuePair<int, int>(x + 1, y - 1)))
        {
            score++;
            sub s = new sub(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x, y - 1), new KeyValuePair<int, int>(x + 1, y - 1));
            list.Add(s);
        }

        if (x - 1 >= 0 && y - 1 >= 0 && matrix[x, y] && matrix[x - 1, y] && matrix[x - 1, y - 1]
            && !checkList(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x - 1, y), new KeyValuePair<int, int>(x - 1, y - 1)))
        {
            score++;
            sub s = new sub(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x - 1, y), new KeyValuePair<int, int>(x - 1, y - 1));
            list.Add(s);
        }

        if (x - 1 >= 0 && y - 1 >= 0 && matrix[x, y] && matrix[x, y - 1] && matrix[x - 1, y - 1]    //
            && !checkList(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x, y - 1), new KeyValuePair<int, int>(x - 1, y - 1)))
        {
            score++;
            sub s = new sub(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x, y - 1), new KeyValuePair<int, int>(x - 1, y - 1));
            list.Add(s);
        }

        if (x - 1 >= 0 && y + 1 < n && matrix[x, y] && matrix[x - 1, y] && matrix[x - 1, y + 1]
            && !checkList(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x - 1, y), new KeyValuePair<int, int>(x - 1, y + 1)))
        {
            score++;
            sub s = new sub(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x - 1, y), new KeyValuePair<int, int>(x - 1, y + 1));
            list.Add(s);
        }

        if (x - 1 >= 0 && y + 1 < n && matrix[x, y] && matrix[x, y + 1] && matrix[x - 1, y + 1]     //
            && !checkList(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x, y + 1), new KeyValuePair<int, int>(x - 1, y + 1)))
        {
            score++;
            sub s = new sub(new KeyValuePair<int, int>(x, y), new KeyValuePair<int, int>(x, y + 1), new KeyValuePair<int, int>(x - 1, y + 1));
            list.Add(s);
        }

        

    }

    public void exit()
    {
        Application.Quit();
    }

    //
}
