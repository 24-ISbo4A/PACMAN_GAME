using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PACMAN_GAME.Interfaces;
using PACMAN_GAME.Properties;

namespace PACMAN_GAME.Models;

public abstract class Ghost : GameEntity, IMovable
{
    protected const int DirectionUp = 0;
    protected const int DirectionRight = 1;
    protected const int DirectionDown = 2;
    protected const int DirectionLeft = 3;
    
    public int Speed { get; set; }
    public int Direction { get; set; }
    protected readonly Random Random = new();
    protected readonly Form Parent;
    protected bool IsFearMode;
    protected bool IsEaten;
    
    protected Ghost(PictureBox view, Form parent) : base(view)
    {
        Parent = parent;
        Speed = 8;
        Direction = Random.Next(4);
    }
    
    public virtual void Move()
    {
        int actualSpeed = IsFearMode ? 4 : Speed;
        
        int newX = X;
        int newY = Y;
        
        switch (Direction)
        {
            case DirectionUp:
                newY -= actualSpeed;
                break;
            case DirectionRight:
                newX += actualSpeed;
                break;
            case DirectionDown:
                newY += actualSpeed;
                break;
            case DirectionLeft:
                newX -= actualSpeed;
                break;
        }
        
        List<IGameEntity> walls = Parent.Controls
            .OfType<PictureBox>()
            .Where(p => (string)p.Tag == "wall")
            .Select(p => new Wall(p))
            .Cast<IGameEntity>()
            .ToList();
        
        if (CanMove(newX, newY, walls))
        {
            X = newX;
            Y = newY;
        }
        else
        {
            Direction = Random.Next(4);
        }
    }
    
    public bool CanMove(int newX, int newY, List<IGameEntity> obstacles)
    {
        Rectangle newBounds = new Rectangle(newX, newY, Width, Height);
        
        foreach (IGameEntity obstacle in obstacles)
        {
            if (newBounds.IntersectsWith(obstacle.Bounds))
            {
                return false;
            }
        }
        
        return true;
    }
    
    public void EnterFearMode()
    {
        IsFearMode = true;
        View.Image = Resources.scared_ghost_anim;
    }
    
    public void ExitFearMode()
    {
        IsFearMode = false;
        IsEaten = false;
        SetNormalImage();
    }
    
    public void SetEaten()
    {
        IsEaten = true;
    }
    
    public bool GetIsEaten()
    {
        return IsEaten;
    }
    
    public bool GetIsFearMode()
    {
        return IsFearMode;
    }
    
    protected abstract void SetNormalImage();
    
    public void Respawn()
    {
        X = 710;
        Y = 420;
    }
} 