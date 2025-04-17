using PACMAN_GAME.Interfaces;

namespace PACMAN_GAME.Managers;

public class CollisionHandler : ICollisionHandler
{
    private readonly ISoundManager _soundManager;
    
    public CollisionHandler(ISoundManager soundManager)
    {
        _soundManager = soundManager;
    }
    
    public bool CheckCollision(IGameEntity entity1, IGameEntity entity2)
    {
        return entity1.Bounds.IntersectsWith(entity2.Bounds);
    }
    
    public void HandleCollision(IGameEntity entity1, IGameEntity entity2)
    {
        // This method is empty because the specific collision handling is done in GameManager
    }
} 