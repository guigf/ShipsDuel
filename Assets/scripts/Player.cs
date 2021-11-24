using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Player
{
    float posX = 0;
    float posY = 0;
    float speed = 0.05f;
    Color color;
    bool shooting = false;
    float shootSpeed = 0.35f;
    float shootDamage = 1;
    float life = 5;
    float maxLife = 5;
    int score = 0;
    KeyCode keyUp;
    KeyCode keyDown;
    KeyCode keyShoot;
    public List<Projectile> projectilesList;
    int shootDirection = 1;
    float shootTimer = 0;
    string name;

    public float Life { get => life; set => life = value; }
    public float PosX { get => posX; set => posX = value; }
    public float PosY { get => posY; set => posY = value; }
    public int Score { get => score; set => score = value; }
    public string Name { get => name; set => name = value; }

    public Player(Color color, float posX, int shootDir, KeyCode keyUp, KeyCode keyDown, KeyCode keyShoot, string name)
    {
        this.color = color;
        this.PosX = posX;
        this.PosY = 0;
        this.shootDirection = shootDir;
        this.keyUp = keyUp;
        this.keyDown = keyDown;
        this.keyShoot = keyShoot;
        this.projectilesList = new List<Projectile>();
        this.Name = name;
    }

    public void Update(Vector2 screenBoard)
    {
        if (Input.GetKey(keyUp))
        {
            if (PosY + speed + 1 <= screenBoard.y)
                PosY += speed;
        }

        if (Input.GetKey(keyDown))
        {
            if (PosY - speed > screenBoard.y * (-1))
                PosY -= speed;
        }

        if (Input.GetKey(keyShoot))
        {
            if (shootTimer <= 0)
            {
                Shoot();
                shootTimer = shootSpeed;
            }
        }

        if (shootTimer > 0)
            shootTimer -= Time.deltaTime;

        List<int> projectilesRemove = new List<int>();
        projectilesList.ForEach(delegate (Projectile item)
        {
            item.Update(screenBoard);
            if ((item.PosX * shootDirection) > screenBoard.x + 1)
                projectilesRemove.Add(projectilesList.IndexOf(item));
        });

        projectilesRemove.ForEach(delegate (int item)
        {
            projectilesList.RemoveAt(item);
        });
    }

    public void Render(Material mat)
    {
        GL.PushMatrix();
        mat.SetPass(0);
        GL.Begin(GL.QUADS);
        GL.Color(color);

        GL.Vertex3(PosX, PosY, 0);
        GL.Vertex3(PosX, PosY + 1, 0);
        GL.Vertex3(PosX + 1, PosY + 1, 0);
        GL.Vertex3(PosX + 1, PosY, 0);

        GL.End();
        GL.PopMatrix();

        projectilesList.ForEach(delegate (Projectile item)
        {
            item.Render(mat);
        });
    }

    public void Shoot()
    {
        float posX;
        if (shootDirection > 0)
            posX = PosX + 1;
        else
            posX = PosX -0.5f;

        projectilesList.Add(new Projectile(color, shootDirection, posX, PosY+0.5f));
    }

    public void CheckProjectileHit(Player enemyPlayer)
    {
        Projectile hit = null;

        projectilesList.ForEach(delegate (Projectile item)
        {
            if (HasCollided(enemyPlayer, item))
            {
                hit = item;
                return;
            }
        });

        if (hit != null)
        {
            enemyPlayer.Life -= hit.Damage;
            projectilesList.Remove(hit);
        }
    }

    private bool HasCollided(Player enemyPlayer, Projectile projectile)
    {
        if ((enemyPlayer.PosY + 1 > projectile.PosY && enemyPlayer.PosY < projectile.PosY))
        {
            if (shootDirection == -1)
            {
                if ((enemyPlayer.PosX + 1f >= projectile.PosX))
                {
                    return true;
                }
            } else if (shootDirection == 1)
            {
                if ((enemyPlayer.PosX - 0.5f <= projectile.PosX))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void ResetRound()
    {
        projectilesList = new List<Projectile>();
        life = maxLife;
    }
}
