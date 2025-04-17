using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PACMAN_GAME.Interfaces;
using PACMAN_GAME.Properties;

namespace PACMAN_GAME.Models;

public class Pacman : GameEntity, IMovable
{
    private const int DirectionUp = 0;
    private const int DirectionRight = 1;
    private const int DirectionDown = 2;
    private const int DirectionLeft = 3;

    public int Speed { get; set; }
    public int Direction { get; set; }
    public int NextDirection { get; set; }
    private readonly Form _parent;
    private readonly ISoundManager _soundManager;

    public Pacman(PictureBox view, Form parent, ISoundManager soundManager) : base(view)
    {
        Direction = DirectionRight;
        NextDirection = DirectionRight;
        Speed = 12;
        _parent = parent;
        _soundManager = soundManager;
    }

    public void SetDirection(int direction)
    {
        NextDirection = direction;
        UpdateImage();
    }

    private void UpdateImage()
    {
        switch (NextDirection)
        {
            case DirectionUp:
                View.Image = Resources.up;
                break;
            case DirectionDown:
                View.Image = Resources.down;
                break;
            case DirectionLeft:
                View.Image = Resources.left;
                break;
            case DirectionRight:
                View.Image = Resources.right;
                break;
        }
    }

    public void Move()
    {
        CheckDirectionChange();
        
        int newX = X;
        int newY = Y;

        switch (Direction)
        {
            case DirectionUp:
                newY -= Speed;
                break;
            case DirectionRight:
                newX += Speed;
                break;
            case DirectionDown:
                newY += Speed;
                break;
            case DirectionLeft:
                newX -= Speed;
                break;
        }

        // Handle wraparound
        if (newX < -10) newX = _parent.ClientSize.Width - 10;
        if (newX > _parent.ClientSize.Width - 10) newX = -10;
        if (newY < -10) newY = _parent.ClientSize.Height - 10;
        if (newY > _parent.ClientSize.Height - 10) newY = -10;

        bool moved = false;
        List<IGameEntity> walls = _parent.Controls
            .OfType<PictureBox>()
            .Where(p => (string)p.Tag == "wall")
            .Select(p => new Wall(p))
            .Cast<IGameEntity>()
            .ToList();

        if (CanMove(newX, newY, walls))
        {
            X = newX;
            Y = newY;
            moved = true;
        }

        if (moved)
        {
            if (!_soundManager.IsPlaying("pacman_move") && !_soundManager.IsPlaying("game_start"))
            {
                _soundManager.PlaySound("pacman_move");
            }
        }
        else
        {
            _soundManager.StopSound("pacman_move");
        }
    }

    private void CheckDirectionChange()
    {
        if (NextDirection == Direction) return;

        int checkX = X;
        int checkY = Y;

        switch (NextDirection)
        {
            case DirectionUp:
                checkY -= 40;
                break;
            case DirectionRight:
                checkX += 40;
                break;
            case DirectionDown:
                checkY += 40;
                break;
            case DirectionLeft:
                checkX -= 40;
                break;
        }

        List<IGameEntity> walls = _parent.Controls
            .OfType<PictureBox>()
            .Where(p => (string)p.Tag == "wall")
            .Select(p => new Wall(p))
            .Cast<IGameEntity>()
            .ToList();

        if (CanMove(checkX, checkY, walls))
        {
            Direction = NextDirection;
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
} 