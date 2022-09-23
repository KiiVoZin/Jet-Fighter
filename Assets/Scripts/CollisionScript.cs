using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class CollisionScript : MonoBehaviour
{
    private static List<GameObject> objects = new();

    public static void Add(GameObject gameObject)
    {
        objects.Add(gameObject);
    }

    public static GameObject Collide(GameObject mainObject)
    {
        Vector2 position = mainObject.transform.position;
        Vector2 scale = mainObject.transform.localScale;

        Vector2 positionTemp;
        Vector2 scaleTemp;

        float topY = position.y + scale.y / 2;
        float bottomY = position.y - scale.y / 2;
        float leftX = position.x - scale.x / 2;
        float rightX = position.x + scale.x / 2;

        float topYTemp;
        float bottomYTemp;
        float leftXTemp;
        float rightXTemp;

        foreach (var obj in objects)
        {
            positionTemp = obj.transform.position;
            scaleTemp = obj.transform.localScale;

            topYTemp = positionTemp.y + scaleTemp.y / 2;
            bottomYTemp = positionTemp.y - scaleTemp.y / 2;
            leftXTemp = positionTemp.x - scaleTemp.x / 2;
            rightXTemp = positionTemp.x + scaleTemp.x / 2;
            if (rightX >= leftXTemp && rightXTemp >= leftX && topY >= bottomYTemp && topYTemp >= bottomY) return obj;
        }

        return null;
    }

    public static GameObject CollideWithIntersect(GameObject mainObject, Vector2 move)
    {
        Vector2 position = mainObject.transform.position;
        Vector2 scale = mainObject.transform.localScale;

        Vector2 positionTemp;
        Vector2 scaleTemp;

        float topY = position.y + scale.y / 2;
        float bottomY = position.y - scale.y / 2;
        float leftX = position.x - scale.x / 2;
        float rightX = position.x + scale.x / 2;

        float topYTemp;
        float bottomYTemp;
        float leftXTemp;
        float rightXTemp;

        foreach (var obj in objects)
        {
            positionTemp = obj.transform.position;
            scaleTemp = obj.transform.localScale;

            topYTemp = positionTemp.y + scaleTemp.y / 2;
            bottomYTemp = positionTemp.y - scaleTemp.y / 2;
            leftXTemp = positionTemp.x - scaleTemp.x / 2;
            rightXTemp = positionTemp.x + scaleTemp.x / 2;
            if (rightX >= leftXTemp && rightXTemp >= leftX && topY >= bottomYTemp && topYTemp >= bottomY) return obj;
        }

        Vector2 positionNext = move - position;

        foreach (var obj in objects)
        {
            positionTemp = obj.transform.position;
            scaleTemp = obj.transform.localScale;

            topYTemp = positionTemp.y + scaleTemp.y / 2;
            bottomYTemp = positionTemp.y - scaleTemp.y / 2;
            leftXTemp = positionTemp.x - scaleTemp.x / 2;
            rightXTemp = positionTemp.x + scaleTemp.x / 2;
            List<Vector> edges = new List<Vector>();
            edges.Add(new Vector(new Vector2(leftXTemp, topYTemp), new Vector2(rightXTemp, topYTemp)));
            edges.Add(new Vector(new Vector2(rightXTemp, topYTemp), new Vector2(rightXTemp, bottomYTemp)));
            edges.Add(new Vector(new Vector2(rightXTemp, bottomYTemp), new Vector2(leftXTemp, bottomYTemp)));
            edges.Add(new Vector(new Vector2(leftXTemp, bottomYTemp), new Vector2(leftXTemp, topYTemp)));

            foreach (var edge in edges)
            {
                Vector2 edgeP1 = edge.p1;
                Vector2 edgeP2 = edge.p2;
                float denominator = (edgeP2.y - edgeP1.y) * (positionNext.x - position.x) - (edgeP2.x - edgeP1.x) * (positionNext.y - position.y);

                if (denominator == 0) continue;
                float u_a = ((edgeP2.x - edgeP1.x) * (position.y - edgeP1.y) - (edgeP2.y - edgeP1.y) * (position.x - edgeP1.x)) / denominator;
                float u_b = ((positionNext.x - position.x) * (position.y - edgeP1.y) - (positionNext.y - position.y) * (position.x - edgeP1.x)) / denominator;

                if ((u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)) return obj;
            }
        }
        return null;
    }

}
