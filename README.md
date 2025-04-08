----------------------------------------------------------------------------------------------
# 🟡 PACMAN GAME (Windows Forms)

> Аркадная игра Pac-Man, созданная на Windows Forms. Управляй Пакманом, избегай призраков и собирай все точки на карте!

---

## 🎮 Как запустить игру (для пользователя)

### 📥 1. Скачать и запустить

- Перейди в папку `Release` или `bin/Debug` после сборки проекта
- Запусти файл `PACMAN_GAME.exe`

> ⚠️ Убедись, что у тебя установлен [.NET Framework 4.x](https://dotnet.microsoft.com/en-us/download/dotnet-framework) или [Runtime .NET 6/7](https://dotnet.microsoft.com/en-us/download)

---

## 🛠️ Как вести разработку (для разработчика)

### 🔧 Требования

- Visual Studio 2022 или новее
- .NET Framework 4.x или .NET 6+
- Знание Windows Forms

### 🗃️ Структура веток

- `master` — стабильная версия
- `dev` — активная разработка
- `feature/*` — новые фичи
- `task/*` — мелкие задачи

### 🚀 Начало работы

```bash
git clone https://github.com/24-ISbo4A/PACMAN_GAME/
cd PACMAN_GAME
git checkout -b dev origin/dev
```

## 🖥️ Работа с GitHub Desktop

### 📦 Клонирование проекта

1. Установи [GitHub Desktop](https://desktop.github.com/)
2. Открой его и нажми **"File → Clone repository"**
3. Введи URL проекта или выбери из списка своих репозиториев
4. Укажи папку для клонирования и нажми **Clone**

### 🌿 Работа с ветками

- Перейди в выпадающий список веток (вверху слева)
- Выбери нужную ветку (например, `dev`) или нажми **"New branch"** для создания новой от текущей
- Назови ветку, например: `feature/menu`, и нажми **Create Branch**

### 📝 Коммиты и пуш

- Внеси изменения в проект
- Перейди в GitHub Desktop, введи сообщение коммита и нажми **"Commit to..."**
- Нажми **"Push origin"**, чтобы отправить ветку на GitHub

### 🔁 Создание Pull Request

После пуша в верхней части окна появится кнопка **"Create Pull Request"** — нажми и откроется браузер с формой PR. Выбери, чтобы влить изменения в ветку `dev`.

---

▶️ Запуск и отладка
Запусти PACMAN_GAME.sln через Visual Studio
Выбери конфигурацию Debug
Нажми F5 для запуска

Работа с ветками:
git checkout dev
git pull
git checkout -b feature/название_фичи
# внёс изменения
git add .
git commit -m "Добавил фичу: ..."
git push origin feature/название_фичи

