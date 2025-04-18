using System.Windows.Forms;
using PACMAN_GAME.Interfaces;
using PACMAN_GAME.Properties;

namespace PACMAN_GAME.Models;

public class GameEntityFactory
{
    private readonly Form _parent;
    private readonly ISoundManager _soundManager;

    public GameEntityFactory(Form parent, ISoundManager soundManager)
    {
        _parent = parent;
        _soundManager = soundManager;
    }

    public IGameEntity CreateGameEntity(string entityType, PictureBox view, int value = 1)
    {
        return entityType switch
        {
            "pacman" => CreatePacman(view),
            "redGhost" => CreateRedGhost(view),
            "yellowGhost" => CreateYellowGhost(view),
            "pinkGhost" => CreatePinkGhost(view),
            "coin" => CreateCoin(view, value),
            "wall" => CreateWall(view),
            _ => throw new ArgumentException($"Unknown entity type: {entityType}")
        };
    }

    private Pacman CreatePacman(PictureBox view)
    {
        return new Pacman(view, _parent, _soundManager);
    }

    private RedGhost CreateRedGhost(PictureBox view)
    {
        return new RedGhost(view, _parent);
    }

    private YellowGhost CreateYellowGhost(PictureBox view)
    {
        return new YellowGhost(view, _parent);
    }

    private PinkGhost CreatePinkGhost(PictureBox view)
    {
        return new PinkGhost(view, _parent);
    }

    private Coin CreateCoin(PictureBox view, int value)
    {
        return new Coin(view, value);
    }

    private Wall CreateWall(PictureBox view)
    {
        return new Wall(view);
    }
} 