using System.Collections.Generic;

namespace PACMAN_GAME.Interfaces;

public interface IMovable : IGameEntity
{
    int Speed { get; set; }
    int Direction { get; set; }
    void Move();
    bool CanMove(int newX, int newY, List<IGameEntity> obstacles);
} 