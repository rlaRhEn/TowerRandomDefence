using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bow : Projectile
{
    public override void Move()
    {
        float yPos = 2;
        float yPosEnd = yPos;
        endPos = (Vector2)target.transform.position + new Vector2(0, 0.25f);

        Vector2 tVec = endPos - (Vector2)transform.position;

        float tDis = tVec.sqrMagnitude;
        if (tDis > 0.1f)
        {
            Vector2 tDirVec = (tVec).normalized;
            Vector3 tWect;
            if (yPos == -1f)
            {
                tWect = (moveSpeed * (Vector3)tDirVec);

            }
            else
            {
                yPos -= yPosEnd * Time.deltaTime;
                tWect = (moveSpeed * (Vector3)tDirVec + new Vector3(0, yPos, 0));
            }
            transform.position += tWect * Time.deltaTime;
            transform.up = tWect;
        }
    }
}
