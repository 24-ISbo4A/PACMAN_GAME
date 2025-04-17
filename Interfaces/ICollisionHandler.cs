namespace PACMAN_GAME.Interfaces;

public interface ICollisionHandler
{
    bool CheckCollision(IGameEntity entity1, IGameEntity entity2);
    void HandleCollision(IGameEntity entity1, IGameEntity entity2);
} 