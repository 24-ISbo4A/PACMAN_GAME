# Документация по классам

## GameManager
```csharp
public class GameManager : IGameManager
```
Центральный класс, управляющий игровым процессом.

### Основные свойства
- `bool IsInMenu` - Состояние меню
- `bool IsGameOver` - Состояние окончания игры

### Основные методы
- `void StartGame()` - Запуск игры
- `void ResetGame()` - Сброс состояния игры
- `void UpdateGame()` - Обновление игрового состояния
- `void GameOver(string message)` - Обработка окончания игры
- `void NextLevel()` - Переход на следующий уровень
- `void SelectMenuOption(int direction)` - Выбор опции в меню
- `void ExecuteSelectedMenuOption()` - Выполнение выбранной опции

## SoundManager
```csharp
public class SoundManager : ISoundManager
```
Класс для управления звуковыми эффектами.

### Основные методы
- `void InitializeSounds()` - Инициализация звуковых файлов
- `void PlaySound(string soundName)` - Воспроизведение звука
- `void StopSound(string soundName)` - Остановка звука
- `void StopAllSounds()` - Остановка всех звуков
- `bool IsPlaying(string soundName)` - Проверка воспроизведения

## Pacman
```csharp
public class Pacman : GameEntity, IMovable
```
Класс, представляющий главного героя игры.

### Константы
- `DirectionUp`, `DirectionRight`, `DirectionDown`, `DirectionLeft` - Направления движения

### Основные свойства
- `int Speed` - Скорость движения
- `int Direction` - Текущее направление
- `int NextDirection` - Следующее направление

### Основные методы
- `void SetDirection(int direction)` - Установка направления
- `void Move()` - Перемещение
- `bool CanMove(int newX, int newY, List<IGameEntity> obstacles)` - Проверка возможности движения

## Ghost
```csharp
public abstract class Ghost : GameEntity, IMovable
```
Абстрактный класс для призраков.

### Константы
- `DirectionUp`, `DirectionRight`, `DirectionDown`, `DirectionLeft` - Направления движения

### Основные свойства
- `bool IsFearMode` - Режим страха
- `bool IsEaten` - Состояние съеденности

### Основные методы
- `void EnterFearMode()` - Вход в режим страха
- `void ExitFearMode()` - Выход из режима страха
- `void SetEaten()` - Установка состояния съеденности
- `void Respawn()` - Возрождение призрака

## RedGhost, YellowGhost, PinkGhost
```csharp
public class RedGhost : Ghost
public class YellowGhost : Ghost
public class PinkGhost : Ghost
```
Конкретные реализации призраков разных цветов.

### Основные методы
- `protected override void SetNormalImage()` - Установка нормального изображения

## Coin
```csharp
public class Coin : GameEntity
```
Класс для монеток.

### Основные свойства
- `int Value` - Значение монетки

## Wall
```csharp
public class Wall : GameEntity
```
Класс для стен.

## CollisionHandler
```csharp
public class CollisionHandler : ICollisionHandler
```
Класс для обработки столкновений.

### Основные методы
- `bool CheckCollision(IGameEntity entity1, IGameEntity entity2)` - Проверка столкновения
- `void HandleCollision(IGameEntity entity1, IGameEntity entity2)` - Обработка столкновения

## UIManager
```csharp
public class UIManager : IUIManager
```
Класс для управления пользовательским интерфейсом.

### Основные методы
- `void ShowMainMenu()` - Показ главного меню
- `void HideMainMenu()` - Скрытие главного меню
- `void ShowGameOverScreen(string message)` - Показ экрана окончания игры
- `void UpdateScore(int score)` - Обновление счета
- `void UpdateArrowPosition(int selectedOption)` - Обновление позиции стрелки

## InputHandler
```csharp
public class InputHandler : IInputHandler
```
Класс для обработки пользовательского ввода.

### Основные методы
- `void HandleKeyDown(KeyEventArgs e)` - Обработка нажатия клавиши
- `void HandleKeyUp(KeyEventArgs e)` - Обработка отпускания клавиши 