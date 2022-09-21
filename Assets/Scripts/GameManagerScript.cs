using JetBrains.Annotations;
using JetFighter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
//using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject Player1, Player2;
    [SerializeField] float MovementSpeed, RotationSpeed;
    [SerializeField] GameObject Bullet;
    [SerializeField] float BulletSpeed, BulletLifetime, BulletFireCooldown;
    [SerializeField] TextMeshProUGUI p1text;
    [SerializeField] TextMeshProUGUI p2text;
    List<GameObject> Bullets;
    List<Vector2> oldPositions;
    private int p1score;
    private int p2score;
    private float p1BulletCD;
    private float p2BulletCD;


    // Start is called before the first frame update
    void Start()
    {
        Player1.GetComponent<PlayerScript>().playerNum = Player.Player1;
        Player2.GetComponent<PlayerScript>().playerNum = Player.Player2;
        CollisionScript.Add(Player1);
        CollisionScript.Add(Player2);
        Bullets = new List<GameObject>();
        oldPositions = new List<Vector2>();
        p1score = 0;
        p2score = 0;
        p1BulletCD = BulletFireCooldown;
        p2BulletCD = BulletFireCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        p1BulletCD -= Time.deltaTime;
        p2BulletCD -= Time.deltaTime;
        //for (int i = 0; i < P1Bullets.Count; i++)
        //{
        //    if (P1Bullets[i] == null)
        //    {
        //        Destroy(P1Bullets[i]);
        //        P1Bullets.Remove(P1Bullets[i]);
        //        continue;
        //    }
        //        Vector3 pos = P1Bullets[i].transform.position;
        //        Vector3 move = BulletSpeed * Time.deltaTime * P1Bullets[i].transform.up;
        //        if (CollisionScript.CollideWithIntersect(P1Bullets[i], move).name == Player2str)
        //        {
        //            p1scored();
        //            Destroy(P1Bullets[i]);
        //            P1Bullets.Remove(P1Bullets[i]);
        //            continue;
        //        }
        //        P1Bullets[i].transform.position += move;
        //        Vector2 temp = cam.WorldToViewportPoint(pos + move);
        //        temp = new Vector2(Modulo(temp.x, 1), Modulo(temp.y, 1));
        //        temp = cam.ViewportToWorldPoint(temp);
        //        P1Bullets[i].transform.position = temp;
        //        Vector2 debug = new Vector2(temp.x + P1Bullets[i].transform.up.x, temp.y + P1Bullets[i].transform.up.y);
        //        Debug.DrawLine(temp, debug);

        //}
        //for (int i = 0; i < P2Bullets.Count; i++)
        //{
        //    if (P2Bullets[i] == null)
        //    {
        //        Destroy(P2Bullets[i]);
        //        P2Bullets.Remove(P2Bullets[i]);
        //        continue;
        //    }
        //        Vector3 pos = P2Bullets[i].transform.position;
        //        Vector3 move = BulletSpeed * Time.deltaTime * P2Bullets[i].transform.up;
        //        if (CollisionScript.CollideWithIntersect(P2Bullets[i], move).name == Player1str)
        //        {
        //            p2scored();
        //            Destroy(P2Bullets[i]);
        //            P2Bullets.Remove(P2Bullets[i]);
        //            continue;
        //        }
        //        P2Bullets[i].transform.position += move;
        //        Vector2 temp = cam.WorldToViewportPoint(pos + move);
        //        temp = new Vector2(Modulo(temp.x, 1), Modulo(temp.y, 1));
        //        temp = cam.ViewportToWorldPoint(temp);
        //        P2Bullets[i].transform.position = temp;

        //}
        MoveBullet(Bullets);
        Debug.Assert(Bullets.Count == oldPositions.Count,"Not equal");
        //Debug.Log("BULLET SIZE: " + Bullets.Count);
        //Debug.Log("POSITIONS SIZE: " + oldPositions.Count);
        MovePlayer();
        CollisionBullets(Bullets, oldPositions);
    }

    void CollisionBullets(List<GameObject> bulletList, List<Vector2> oldPositions)
    {
        for (int i = 0; i < bulletList.Count; i++)
        {
            var currentBulletNum = bulletList[i].GetComponent<BulletScript>().playerNum;

            if (bulletList[i] == null)
            {
                //Destroy(bulletList[i]);
                bulletList.Remove(bulletList[i]);
                oldPositions.Remove(oldPositions[i]);
                continue;
            }
            Vector3 pos = bulletList[i].transform.position;
            var intersectedPlayer = CollisionScript.CollideWithIntersect(bulletList[i], oldPositions[i]);

            if (intersectedPlayer == null) continue;
            var intersectedPlayerNum = intersectedPlayer.GetComponent<PlayerScript>().playerNum;

            if ((int)intersectedPlayerNum != (int)currentBulletNum)
            {
                if ((int)intersectedPlayerNum == 0) p2scored();
                else p1scored();

                Destroy(bulletList[i]);
                bulletList.Remove(bulletList[i]);
                oldPositions.Remove(oldPositions[i]);
                continue;
            }
        }
    }

    void MoveBullet(List<GameObject> bulletList)
    {
        for (int i = 0; i < bulletList.Count; i++)
        {
            if (bulletList[i] == null)
            {
                //Destroy(bulletList[i]);
                bulletList.Remove(bulletList[i]);
                oldPositions.Remove(oldPositions[i]);
                continue;
            }
            Vector3 pos = bulletList[i].transform.position;
            oldPositions[i] = pos;
            Vector3 move = BulletSpeed * Time.deltaTime * bulletList[i].transform.up;
            //var intersectedPlayerName = CollisionScript.CollideWithIntersect(bulletList[i], move).name;
            //if (intersectedPlayerName == targetPlayer)
            //{
            //    p1scored();
            //    Destroy(bulletList[i]);
            //    bulletList.Remove(bulletList[i]);
            //    continue;
            //}
            bulletList[i].transform.position += move;
            Vector2 temp = cam.WorldToViewportPoint(pos + move);
            temp = new Vector2(Modulo(temp.x, 1), Modulo(temp.y, 1));
            temp = cam.ViewportToWorldPoint(temp);
            bulletList[i].transform.position = temp;
            Vector2 debug = new Vector2(temp.x + bulletList[i].transform.up.x, temp.y + bulletList[i].transform.up.y);
            Debug.DrawLine(temp, debug);

        }
    }

    void MovePlayer()
    {
        float angleP1 = 0f;
        float angleP2 = 0f;
        Vector3 p1Up = Player1.transform.up;
        Vector3 p2Up = Player2.transform.up;
        Vector3 p1Pos = Player1.transform.position;
        Vector3 p2Pos = Player2.transform.position;
        if (Input.GetKey(KeyCode.D))
        {
            angleP1 = -RotationSpeed * Mathf.Deg2Rad * Time.deltaTime;
            float x = p1Up.x * Mathf.Cos(angleP1) - p1Up.y * Mathf.Sin(angleP1);
            float y = p1Up.x * Mathf.Sin(angleP1) + p1Up.y * Mathf.Cos(angleP1);
            Vector2 direction = new Vector2(x, y);
            p1Up = direction;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            angleP1 = RotationSpeed * Mathf.Deg2Rad * Time.deltaTime;
            float x = p1Up.x * Mathf.Cos(angleP1) - p1Up.y * Mathf.Sin(angleP1);
            float y = p1Up.x * Mathf.Sin(angleP1) + p1Up.y * Mathf.Cos(angleP1);
            Vector2 direction = new Vector2(x, y);
            p1Up = direction;
        }
        if (Input.GetKey(KeyCode.W))
        {
            p1Pos += p1Up * MovementSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            p1Pos -= p1Up * MovementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            angleP2 = -RotationSpeed * Mathf.Deg2Rad * Time.deltaTime;
            float x = p2Up.x * Mathf.Cos(angleP2) - p2Up.y * Mathf.Sin(angleP2);
            float y = p2Up.x * Mathf.Sin(angleP2) + p2Up.y * Mathf.Cos(angleP2);
            Vector2 direction = new Vector2(x, y);
            p2Up = direction;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            angleP2 = RotationSpeed * Mathf.Deg2Rad * Time.deltaTime;
            float x = p2Up.x * Mathf.Cos(angleP2) - p2Up.y * Mathf.Sin(angleP2);
            float y = p2Up.x * Mathf.Sin(angleP2) + p2Up.y * Mathf.Cos(angleP2);
            Vector2 direction = new Vector2(x, y);
            p2Up = direction;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            p2Pos += p2Up * MovementSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            p2Pos -= MovementSpeed * Time.deltaTime * p2Up;
        }
        Player1.transform.up = p1Up;
        Player2.transform.up = p2Up;

        p1Pos = cam.WorldToViewportPoint(p1Pos);
        p1Pos = new Vector3(Modulo(p1Pos.x, 1), Modulo(p1Pos.y, 1));
        p1Pos = cam.ViewportToWorldPoint(p1Pos);
        p1Pos.z = 0;
        Player1.transform.position = p1Pos;
        p2Pos = cam.WorldToViewportPoint(p2Pos);
        p2Pos = new Vector3(Modulo(p2Pos.x, 1), Modulo(p2Pos.y, 1));
        p2Pos = cam.ViewportToWorldPoint(p2Pos);
        p2Pos.z = 0;
        Player2.transform.position = p2Pos;

        if (Input.GetKeyDown(KeyCode.F) && p1BulletCD <= 0)
        {
            p1BulletCD = BulletFireCooldown;
            GameObject bullet = Instantiate(Bullet);
            bullet.GetComponent<BulletScript>().playerNum = Player.Player1;
            bullet.GetComponent<DestroyOnTimeScript>().duration = BulletLifetime;
            bullet.transform.position = p1Pos + p1Up * Player1.transform.localScale.y / 2;
            bullet.transform.up = p1Up;
            bullet.GetComponent<Renderer>().material.color = new Color(255, 119, 119, 255);
            Bullets.Add(bullet);
            oldPositions.Add(bullet.transform.position);
        }
        if (Input.GetKeyDown(KeyCode.RightControl) && p2BulletCD <= 0)
        {
            p2BulletCD = BulletFireCooldown;
            GameObject bullet = Instantiate(Bullet);
            bullet.GetComponent<BulletScript>().playerNum = Player.Player2;
            bullet.GetComponent<DestroyOnTimeScript>().duration = BulletLifetime;
            bullet.transform.position = p2Pos + p2Up * Player2.transform.localScale.y / 2;
            bullet.transform.up = p2Up;
            Bullets.Add(bullet);
            oldPositions.Add(bullet.transform.position);
        }
    }

    float Modulo(float value, float modulo)
    {
        while (value < 0) value += modulo;
        return value % modulo;
    }

    public void p1scored()
    {
        p1score++;
        p1text.text = "" + p1score;
    }
    public void p2scored()
    {
        p2score++;
        p2text.text = "" + p2score;
    }
}
