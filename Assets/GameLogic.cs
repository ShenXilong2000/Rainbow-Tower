using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum MoveDir
{
    FrontBack,
    RightLeft,
}



public class GameLogic : MonoBehaviour
{

    public Transform topPlate;
    Transform movingPlate;

    MoveDir moveDir = MoveDir.FrontBack;

    public float speed = 1;

    bool inverseMove = false;
    bool ifGameOver = false;

    float[] rgb = { 1, 1, 1 };
    int[] rgbIndex = { 0, 1, 2 };


    int[] RandomIndex()
    {
        List<int> list1 = new List<int>();
        for (int i = 0; i < rgbIndex.Length; i++)
        {
            list1.Add(rgbIndex[i]);
        }
        List<int> list2 = new List<int>();
        for (int i = list1.Count; i > 0; i--)
        {
            int index;
            index = Random.Range(0, i);
            list2.Add(list1[index]);
            list1.Remove(list1[index]);
        }
        return list2.ToArray();
    }

    bool reduce = true;

    void ColorGradualChange()
    {
        if (reduce)
        {
            if (rgb[rgbIndex[0]] > 0)
                rgb[rgbIndex[0]] -= 0.1f;
            else if (rgb[rgbIndex[1]] > 0)
                rgb[rgbIndex[1]] -= 0.1f;
            else if (rgb[rgbIndex[2]] > 0)
                rgb[rgbIndex[2]] -= 0.1f;
            else
            {
                reduce = !reduce;
                rgbIndex = RandomIndex();
            }
        }
        else
        {
            if (rgb[rgbIndex[0]] < 1)
                rgb[rgbIndex[0]] += 0.1f;
            else if (rgb[rgbIndex[1]] < 1)
                rgb[rgbIndex[1]] += 0.1f;
            else if (rgb[rgbIndex[2]] < 1)
                rgb[rgbIndex[2]] += 0.1f;
            else
            {
                reduce = !reduce;
                rgbIndex = RandomIndex();
            }
        }
    }

    void Start()
    {

    }


    void movePalate()
    {
        Vector3 move;

        if (moveDir == MoveDir.FrontBack)
        {
            move = new Vector3(0, 0, speed * 10);
            if (inverseMove)
            {
                if (movingPlate.position.z < -10)
                {
                    inverseMove = !inverseMove;
                }
            }
            else
            {
                if (movingPlate.position.z > 10)
                {
                    inverseMove = !inverseMove;
                }
            }
        }
        else
        {
            move = new Vector3(speed * 10, 0, 0);
            if (inverseMove)
            {
                if (movingPlate.position.x < -10)
                {
                    inverseMove = !inverseMove;
                }
            }
            else
            {
                if (movingPlate.position.x > 10)
                    inverseMove = !inverseMove;
            }
        }

        if (inverseMove)
        {
            move = -move;

        }

        movingPlate.Translate(move * Time.deltaTime);
    }


    void GenerateNewPlate()
    {
        if (moveDir == MoveDir.FrontBack)
            moveDir = MoveDir.RightLeft;
        else
            moveDir = MoveDir.FrontBack;

        while (GameObject.Find("topPlate"))
        {
            Transform temp = GameObject.Find("topPlate").transform;
            temp.name = "basePlate";
        }
        movingPlate = Instantiate(topPlate);
        topPlate.name = "topPlate";
        movingPlate.name = "Plate";
        if (moveDir == MoveDir.FrontBack)
        {
            System.Random r = new System.Random();
            int flag = r.Next(1, 3);
            if (flag == 1)
                movingPlate.position = new Vector3(topPlate.position.x, topPlate.position.y + 0.5f, 10);
            else
                movingPlate.position = new Vector3(topPlate.position.x, topPlate.position.y + 0.5f, -10);
        }
        else
        {
            System.Random r = new System.Random();
            int flag = r.Next(1, 3);
            if (flag == 1)
                movingPlate.position = new Vector3(10, topPlate.position.y + 0.5f, topPlate.position.z);
            else
                movingPlate.position = new Vector3(-10, topPlate.position.y + 0.5f, topPlate.position.z);
        }

        ColorGradualChange();
        Color c = new Color(rgb[0], rgb[1], rgb[2]);
        movingPlate.gameObject.GetComponent<Renderer>().material.color = c;
        addScore(c);
    }
    void checkGameOver()
    {
        float moveFront = movingPlate.position.z + movingPlate.localScale.z / 2;
        float moveBack = movingPlate.position.z - movingPlate.localScale.z / 2;
        float moveRight = movingPlate.position.x + movingPlate.localScale.x / 2;
        float moveLeft = movingPlate.position.x - movingPlate.localScale.x / 2;

        float topFornt = topPlate.position.z + topPlate.localScale.z / 2;
        float topBack = topPlate.position.z - topPlate.localScale.z / 2;
        float topRight = topPlate.position.x + topPlate.localScale.x / 2;
        float topLeft = topPlate.position.x - topPlate.localScale.x / 2;


        if (moveDir == MoveDir.FrontBack)
        {
            if (moveBack > topFornt)
            {
                ifGameOver = true;
                gameOver();
            }
            if (moveFront < topBack)
            {
                ifGameOver = true;
                gameOver();
            }
        }
        else
        {
            if (moveRight < topLeft)
            {
                ifGameOver = true;
                gameOver();
            }
            if (moveLeft > topRight)
            {
                ifGameOver = true;
                gameOver();
            }
        }
    }

    void stopPlate()
    {
        checkGameOver();
        if (ifGameOver) gameOver();

        float moveFront = movingPlate.position.z + movingPlate.localScale.z / 2;
        float moveBack = movingPlate.position.z - movingPlate.localScale.z / 2;
        float moveRight = movingPlate.position.x + movingPlate.localScale.x / 2;
        float moveLeft = movingPlate.position.x - movingPlate.localScale.x / 2;

        float topFornt = topPlate.position.z + topPlate.localScale.z / 2;
        float topBack = topPlate.position.z - topPlate.localScale.z / 2;
        float topRight = topPlate.position.x + topPlate.localScale.x / 2;
        float topLeft = topPlate.position.x - topPlate.localScale.x / 2;

        if (moveDir == MoveDir.FrontBack)
        {
            float cutFront, cutBack, stayFront, stayBack;

            if (movingPlate.position.z > topPlate.position.z)
            {
                cutFront = moveFront;
                cutBack = topFornt;
                stayFront = topFornt;
                stayBack = moveBack;
            }
            else
            {
                cutFront = topBack;
                cutBack = moveBack;
                stayFront = moveFront;
                stayBack = topBack;
            }

            Destroy(movingPlate.gameObject);
            Transform cutPlate = Instantiate(movingPlate);
            cutPlate.position = new Vector3(cutPlate.position.x,
                cutPlate.position.y,
                (cutFront + cutBack) / 2);
            cutPlate.localScale = new Vector3(cutPlate.localScale.x,
                cutPlate.localScale.y,
                cutFront - cutBack);
            cutPlate.gameObject.AddComponent<Rigidbody>();


            Transform stayPlate = Instantiate(movingPlate);
            stayPlate.position = new Vector3(stayPlate.position.x, stayPlate.position.y, (stayFront + stayBack) / 2);
            stayPlate.localScale = new Vector3(stayPlate.localScale.x, stayPlate.localScale.y, stayFront - stayBack);
            topPlate = stayPlate;

        }
        else
        {
            float cutRight, cutLeft, stayRight, stayLeft;

            if (movingPlate.position.x > topPlate.position.x)
            {
                cutRight = moveRight;
                cutLeft = topRight;
                stayRight = topRight;
                stayLeft = moveLeft;
            }
            else
            {
                cutRight = topLeft;
                cutLeft = moveLeft;
                stayRight = moveRight;
                stayLeft = topLeft;
            }

            Destroy(movingPlate.gameObject);
            Transform cutPlate = Instantiate(movingPlate);
            cutPlate.position = new Vector3((cutRight + cutLeft) / 2,
                cutPlate.position.y,
                cutPlate.position.z);
            cutPlate.localScale = new Vector3(cutRight - cutLeft,
                cutPlate.localScale.y,
                cutPlate.localScale.z);
            cutPlate.gameObject.AddComponent<Rigidbody>();


            Transform stayPlate = Instantiate(movingPlate);
            stayPlate.position = new Vector3((stayRight + stayLeft) / 2,
                stayPlate.position.y,
                stayPlate.position.z);

            stayPlate.localScale = new Vector3(stayRight - stayLeft,
                stayPlate.localScale.y,
                stayPlate.localScale.z);
            topPlate = stayPlate;
        }

        movingPlate = null;
    }
    void addScore(Color cubeColor)
    {
        Text score = GameObject.Find("Score").GetComponent<Text>();
        score.text = (System.Convert.ToInt32(score.text) + 1).ToString();
        score.color = cubeColor;
    }

    void Update()
    {
        if (ifGameOver) gameOver();
        if (movingPlate == null)
        {
            GenerateNewPlate();
        }

        movePalate();

        if (Input.GetButtonDown("Fire1"))
        {
            stopPlate();
        }

    }

    void Transformation(Vector3 pos, Vector3 local, Vector3 euler, Color color)
    {
        transform.position = Vector3.Lerp(transform.position, pos, 0.1f);
        transform.localScale = Vector3.Lerp(transform.localScale, local, 0.1f);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, euler, 0.1f);
        GetComponent<MeshRenderer>().material.color = Color.Lerp(GetComponent<MeshRenderer>().material.color,color, 0.1f);
        if((transform.position - pos).magnitude<0.1f)
        {
            transform.position = pos;
            transform.localScale = local;
            transform.eulerAngles = euler;
            GetComponent<MeshRenderer>().material.color = color;
        }

    }

    void gameOver()
    {
        return;
        //Transform camera = GameObject.Find("basePlate").transform;
        //while(GameObject.Find("basePlate").transform)
        //{
        //    GameObject.Find("basePlate").transform.gameObject.AddComponent<Rigidbody>();
        //}
        //GameObject.Find("Plate").transform.gameObject.AddComponent<Rigidbody>();
        //movingPlate = null;
        //topPlate.position = new Vector3(0, 0, 0);
        //topPlate.localScale = new Vector3(5f, 0.5f, 5f);
        //ifGameOver = false;


        //Transform replayCude = Instantiate(movingPlate);
        //replayCude.localScale = new Vector3(2f, 0.1f, 1f);
        //Transform camera = GameObject.Find("Main Camera").transform;
        //Vector3 local = new Vector3();
        //local = camera.position;
        //replayCude.localEulerAngles = camera.localEulerAngles;
        //replayCude.rotation = camera.rotation;
        //Color c = new Color(rgb[0], rgb[1], rgb[2]);


        //Transformation(replayCude.position,replayCude.localScale,replayCude.localEulerAngles,c);

    }
}
