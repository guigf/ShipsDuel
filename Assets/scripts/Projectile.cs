using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile
{
    float posX = 0;
    float posY = 0;
    float speed = 0.05f;
    Color color;
    float damage = 1;
    int shootDirection = 1;

    public Projectile()
    { }

    public Projectile(Color color, int shootDir, float posX, float posY)
    {
        this.color = color;
        this.shootDirection = shootDir;
        this.PosX = posX;
        this.PosY = posY;
    }

    public float Damage { get => damage; set => damage = value; }
    public float PosX { get => posX; set => posX = value; }
    public float PosY { get => posY; set => posY = value; }

    public void Render(Material mat)
    {
        GL.PushMatrix();
        mat.SetPass(0);
        GL.Begin(GL.QUADS);
        GL.Color(color);

        GL.Vertex3(PosX, PosY, 0);
        GL.Vertex3(PosX, PosY + 0.25f, 0);
        GL.Vertex3(PosX + 0.5f, PosY + 0.25f, 0);
        GL.Vertex3(PosX + 0.5f, PosY, 0);

        GL.End();
        GL.PopMatrix();
    }

    public void Update(Vector2 screenBoard)
    {
        PosX += speed * shootDirection;
    }
}
