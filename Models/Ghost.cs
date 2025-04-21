using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PACMAN_GAME.Interfaces;
using PACMAN_GAME.Properties;

namespace PACMAN_GAME.Models;

/// <summary>
/// Абстрактный базовый класс для всех призраков в игре PACMAN.
/// Реализует интерфейс <see cref="IMovable"/> для поддержки движения.
/// </summary>
public abstract class Ghost : GameEntity, IMovable
{
    /// <summary>Направление движения вверх</summary>
    protected const int DirectionUp = 0;
    /// <summary>Направление движения вправо</summary>
    protected const int DirectionRight = 1;
    /// <summary>Направление движения вниз</summary>
    protected const int DirectionDown = 2;
    /// <summary>Направление движения влево</summary>
    protected const int DirectionLeft = 3;
    
    /// <summary>
    /// Получает или задает скорость движения призрака.
    /// </summary>
    public int Speed { get; set; }
    
    /// <summary>
    /// Получает или задает текущее направление движения призрака.
    /// </summary>
    public int Direction { get; set; }
    
    /// <summary>Генератор случайных чисел для выбора направления</summary>
    protected readonly Random Random = new();
    
    /// <summary>Родительская форма, содержащая игровое поле</summary>
    protected readonly Form Parent;
    
    /// <summary>Флаг, указывающий находится ли призрак в режиме страха</summary>
    protected bool IsFearMode;
    
    /// <summary>Флаг, указывающий был ли призрак съеден</summary>
    protected bool IsEaten;
    
    /// <summary>
    /// Инициализирует новый экземпляр призрака.
    /// </summary>
    /// <param name="view">PictureBox для отображения призрака.</param>
    /// <param name="parent">Родительская форма, содержащая игровое поле.</param>
    protected Ghost(PictureBox view, Form parent) : base(view)
    {
        Parent = parent;
        Speed = 8;
        Direction = Random.Next(4);
    }
    
    /// <summary>
    /// Осуществляет движение призрака согласно текущему направлению.
    /// Автоматически изменяет направление при столкновении со стенами.
    /// </summary>
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
    
    /// <summary>
    /// Проверяет возможность перемещения призрака в указанные координаты.
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
    
    /// <summary>
    /// Активирует режим страха для призрака.
    /// Изменяет изображение и уменьшает скорость.
    /// </summary>
    public void EnterFearMode()
    {
        IsFearMode = true;
        View.Image = Resources.scared_ghost_anim;
    }
    
    /// <summary>
    /// Деактивирует режим страха для призрака.
    /// Восстанавливает нормальное изображение.
    /// </summary>
    public void ExitFearMode()
    {
        IsFearMode = false;
        IsEaten = false;
        SetNormalImage();
    }
    
    /// <summary>
    /// Помечает призрака как съеденного.
    /// </summary>
    public void SetEaten()
    {
        IsEaten = true;
    }
    
    /// <summary>
    /// Проверяет, был ли призрак съеден.
    /// </summary>
    /// <returns>true, если призрак был съеден; иначе false.</returns>
    public bool GetIsEaten()
    {
        return IsEaten;
    }
    
    /// <summary>
    /// Проверяет, находится ли призрак в режиме страха.
    /// </summary>
    /// <returns>true, если призрак в режиме страха; иначе false.</returns>
    public bool GetIsFearMode()
    {
        return IsFearMode;
    }
    
    /// <summary>
    /// Абстрактный метод для установки нормального изображения призрака.
    /// Должен быть реализован в конкретных классах призраков.
    /// </summary>
    protected abstract void SetNormalImage();
    
    /// <summary>
    /// Возвращает призрака на начальную позицию.
    /// </summary>
    public void Respawn()
    {
        X = 710;
        Y = 420;
    }
}
