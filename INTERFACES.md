# Документация по интерфейсам

## ISoundManager
```csharp
public interface ISoundManager
```
Интерфейс для управления звуковыми эффектами в игре.

### Методы
- `void PlaySound(string soundName)` - Воспроизведение звука
- `void StopSound(string soundName)` - Остановка звука
- `void StopAllSounds()` - Остановка всех звуков
- `void InitializeSounds()` - Инициализация звуковых эффектов
- `bool IsPlaying(string soundName)` - Проверка воспроизведения звука

### Свойства
- `Dictionary<string, MediaPlayer> _sounds_pub` - Публичный словарь звуковых плееров

## IGameEntity
```csharp
public interface IGameEntity
```
Базовый интерфейс для всех игровых объектов.

### Свойства
- `PictureBox View` - Визуальное представление объекта
- `bool IsVisible` - Видимость объекта
- `int X` - X-координата
- `int Y` - Y-координата
- `int Width` - Ширина
- `int Height` - Высота
- `Rectangle Bounds` - Границы объекта

## IMovable
```csharp
public interface IMovable : IGameEntity
```
Интерфейс для перемещающихся объектов.

### Свойства
- `int Speed` - Скорость движения
- `int Direction` - Направление движения

### Методы
- `void Move()` - Перемещение объекта
- `bool CanMove(int newX, int newY, List<IGameEntity> obstacles)` - Проверка возможности перемещения

## ICollisionHandler
```csharp
public interface ICollisionHandler
```
Интерфейс для обработки столкновений.

### Методы
- `bool CheckCollision(IGameEntity entity1, IGameEntity entity2)` - Проверка столкновения
- `void HandleCollision(IGameEntity entity1, IGameEntity entity2)` - Обработка столкновения

## IGameManager
```csharp
public interface IGameManager
```
Интерфейс для управления игровым процессом.

### Методы
- `void StartGame()` - Начало игры
- `void ResetGame()` - Сброс игры
- `void UpdateGame()` - Обновление состояния игры
- `void GameOver(string message)` - Окончание игры
- `void NextLevel()` - Переход на следующий уровень

## IInputHandler
```csharp
public interface IInputHandler
```
Интерфейс для обработки пользовательского ввода.

### Методы
- `void HandleKeyDown(KeyEventArgs e)` - Обработка нажатия клавиши
- `void HandleKeyUp(KeyEventArgs e)` - Обработка отпускания клавиши

## IUIManager
```csharp
public interface IUIManager
```
Интерфейс для управления пользовательским интерфейсом.

### Методы
- `void ShowMainMenu()` - Показ главного меню
- `void HideMainMenu()` - Скрытие главного меню
- `void ShowGameOverScreen(string message)` - Показ экрана окончания игры
- `void UpdateScore(int score)` - Обновление счета
- `void UpdateArrowPosition(int selectedOption)` - Обновление позиции стрелки в меню
- `void HideGameOverScreen()` - Скрытие экрана окончания игры 