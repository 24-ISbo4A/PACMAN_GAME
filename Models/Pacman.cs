using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PACMAN_GAME.Interfaces;
using PACMAN_GAME.Properties;

namespace PACMAN_GAME.Models;

/// <summary>
/// Класс, представляющий игрового персонажа Pacman.
/// Реализует интерфейс <see cref="IMovable"/> для поддержки движения.
/// </summary>
public class Pacman : GameEntity, IMovable
{
    /// <summary>Направление движения вверх</summary>
    private const int DirectionUp = 0;
    /// <summary>Направление движения вправо</summary>
    private const int DirectionRight = 1;
    /// <summary>Направление движения вниз</summary>
    private const int DirectionDown = 2;
    /// <summary>Направление движения влево</summary>
    private const int DirectionLeft = 3;

    /// <summary>
    /// Получает или задает текущую скорость движения Pacman.
    /// </summary>
    public int Speed { get; set; }

    /// <summary>
    /// Получает или задает текущее направление движения Pacman.
    /// </summary>
    public int Direction { get; set; }

    /// <summary>
    /// Получает или задает следующее запрошенное направление движения Pacman.
    /// Используется для плавной смены направления при возможности.
    /// </summary>
    public int NextDirection { get; set; }

    private readonly Form _parent;
    private readonly ISoundManager _soundManager;

    /// <summary>
    /// Инициализирует новый экземпляр класса Pacman.
    /// </summary>
    /// <param name="view">PictureBox для отображения Pacman.</param>
    /// <param name="parent">Родительская форма, содержащая игровое поле.</param>
    /// <param name="soundManager">Менеджер звуков для воспроизведения звуковых эффектов.</param>
    public Pacman(PictureBox view, Form parent, ISoundManager soundManager) : base(view)
    {
        Direction = DirectionRight;
        NextDirection = DirectionRight;
        Speed = 12;
        _parent = parent;
        _soundManager = soundManager;
    }

    /// <summary>
    /// Устанавливает следующее направление движения Pacman.
    /// </summary>
    /// <param name="direction">Новое направление движения (0-вверх, 1-вправо, 2-вниз, 3-влево).</param>
    public void SetDirection(int direction)
    {
        NextDirection = direction;
        UpdateImage();
    }

    /// <summary>
    /// Обновляет изображение Pacman в соответствии с текущим направлением движения.
    /// </summary>
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

    /// <summary>
    /// Осуществляет движение Pacman в текущем направлении.
    /// Обрабатывает столкновения со стенами и выход за границы экрана.
    /// </summary>
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

        // Обработка выхода за границы экрана
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

    /// <summary>
    /// Проверяет возможность смены направления движения.
    /// </summary>
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

    /// <summary>
    /// Проверяет возможность перемещения Pacman в указанные координаты.
    /// </summary>
    /// <param name="newX">Новая X-координата.</param>
    /// <param name="newY">Новая Y-координата.</param>
    /// <param name="obstacles">Список препятствий для проверки коллизий.</param>
    /// <returns>true, если перемещение возможно; иначе false.</returns>
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
