# Лабораторная работа №29
---

##  Ключевые изменения по сравнению с предыдущей лабораторной работой

### Модели данных (Model)
| Было | Стало |
|------|-------|
| Обычные POCO-классы |  Классы реализуют `INotifyPropertyChanged` |
| Нет уведомления об изменениях |  Свойства вызывают `OnPropertyChanged()` в сеттерах |
| Методы копирования возвращали новый объект |  Методы изменяют текущий объект (`void`) |
--- 
1)POCO классы содержат только данные (свойства) и не зависят от внешних библиотек, классов
2) INotifyPropertyChanged уведомляет об изменении значения свойства
###  ViewModel
| Было | Стало |
|------|-------|
| Только коллекции данных |  + Свойства `SelectedGroup`/`SelectedStudentDPO` |
| Нет команд |  Команды `Add/Edit/Delete` типа `RelayCommand` |
| Логика в View | Вся бизнес-логика перенесена в ViewModel |
| Нет валидации доступности кнопок |  `CanExecute` автоматически управляет состоянием кнопок |

### View (Code-Behind)
| Было | Стало |
|------|-------|
| Обработчики `btnAdd_Click`, `btnEdit_Click` | Удалены — логика в командах ViewModel |
| Ручная установка `ItemsSource` | Привязка через `ItemsSource="{Binding ...}"` |
| Прямая работа с элементами UI |  Синхронизация через `SelectedItem="{Binding ...}"` |
| Конструктор с логикой |  Только `InitializeComponent()` + `DataContext = new ViewModel()` |

### XAML-разметка
| Было | Стало |
|------|-------|
| `<Button Click="..." />` |  `<Button Command="{Binding AddGroup}" />` |
| ListView без привязок |  `ItemsSource` и `SelectedItem` через Binding |
| Стили в каждой кнопке |  Единый стиль `{StaticResource ButtonMenu}` из словаря ресурсов |

### Работа с данными
| Было | Стало |
|------|-------|
| Конвертация `Student ↔ StudentDPO` в View |  Конвертация в ViewModel |
| Ручное обновление `ItemsSource` | Автоматическое через `ObservableCollection + INotifyPropertyChanged` |
| Выбор группы в ComboBox через code-behind |  Привязка `SelectedValue="{Binding GroupId}"` |

### RelayCommand (Helper)
| Было | Стало |
|------|-------|
| Базовая реализация `ICommand` |  Добавлен метод `RaiseCanExecuteChanged()` |
| Нет принудительного обновления |  `CommandManager.InvalidateRequerySuggested()` для обновления состояния кнопок |

1) RaiseCanExecuteChanged - метод интерфейса Icommand, уведомляющий UI о необходимости перепроверить состояние команды. Когма меняются условия - взываем метод, чтобы кнопка/меню обновили своё состояние.

---

##  Структура проекта после изменений

```
WpfApp2/
├── Helper/
│   ├── RelayCommand.cs      # Универсальная реализация ICommand
│   └── FindGroup.cs         # Вспомогательный класс для поиска
├── Model/
│   ├── Group.cs             # Модель группы (с INotifyPropertyChanged)
│   ├── Student.cs           # Модель студента (с INotifyPropertyChanged)
│   └── StudentDPO.cs        # DTO для отображения данных
├── ViewModel/
│   ├── GroupViewModel.cs    # Логика работы с группами + команды
│   └── StudentViewModel.cs  # Логика работы со студентами + команды
├── View/
│   ├── WindowGroup.xaml(.cs)      # Окно списка групп
│   ├── WindowNewGroup.xaml(.cs)   # Окно редактирования группы
│   ├── WindowStudent.xaml(.cs)    # Окно списка студентов
│   └── WindowNewStudent.xaml(.cs) # Окно редактирования студента
├── Dictionary1.xaml         # Словарь ресурсов со стилями
└── App.xaml                 # Подключение словаря ресурсов
```

---

## Реализованный функционал

- **CRUD для групп**: добавление, редактирование, удаление с валидацией
- **CRUD для студентов**: добавление с выбором группы, редактирование, удаление
- **Автоматическая блокировка кнопок**: "Редактировать" и "Удалить" активны только при выбранном элементе
- **Единый стиль интерфейса**: стиль `ButtonMenu` через словарь ресурсов
- **Двусторонняя синхронизация данных**: изменения в ViewModel автоматически отображаются в UI

---
# Лабораторная работа №30

## Разработка приложений, взаимодействующих с данными в формате JSON

### Сравнение с лабораторной работой №29

| Аспект | Лаба №29 | Лаба №30 |
|--------|----------|----------|
| **Хранение данных** | Данные в оперативной памяти (in-memory) | Данные в JSON-файлах на диске |
| **Сохранность** | Данные теряются при закрытии приложения | Данные сохраняются между запусками |
| **Тип Birthday** | `DateTime` | `string` (формат "dd.MM.yyyy") |
| **Зависимости** | Стандартные библиотеки WPF | + **Newtonsoft.Json** (NuGet) |
| **Методы ViewModel** | Только CRUD операции | + `Load...()`, `SaveChanges()` |

### 🔧 Основные изменения

#### 1. **Структура проекта**
```
WpfApp2/
├── DataModels/              ← НОВАЯ папка
│   ├── GroupData.json       ← Новый файл
│   └── StudentData.json     ← Новый файл
├── Model/
├── View/
└── ViewModel/
    ├── GroupViewModel.cs    ← Модифицирован
    └── StudentViewModel.cs  ← Модифицирован
```

#### 2. **Изменения в моделях данных**
```csharp
// Было (Лаба 29)
public DateTime Birthday { get; set; }

// Стало (Лаба 30)
public string Birthday { get; set; }
```

#### 3. **Новые поля в ViewModel**
```csharp
readonly string path = @"...\DataModels\GroupData.json";
string _jsonGroups = String.Empty;
public string Error { get; set; }
```

### Основные функции

#### **GroupViewModel.cs**

| Метод | Описание |
|-------|----------|
| `LoadGroup()` | Загружает JSON файл и десериализует в коллекцию `ListGroup` |
| `SaveChanges(ListGroup)` | Сериализует коллекцию в JSON и записывает в файл |
| `MaxId()` | Находит максимальный ID для генерации нового |
| `AddGroup` | Добавляет группу + **сохраняет в JSON** |
| `EditGroup` | Редактирует группу + **сохраняет в JSON** |
| `DeleteGroup` | Удаляет группу + **сохраняет в JSON** |

#### **StudentViewModel.cs**

| Метод | Описание |
|-------|----------|
| `LoadStudent()` | Загружает данные студентов из JSON файла |
| `GetListStudentDPO()` | Преобразует `Student` → `StudentDPO` для отображения |
| `SaveChanges(ListStudent)` | Сохраняет изменения в `StudentData.json` |
| `MaxId()` | Определяет следующий свободный ID |
| `AddStudent` | Добавляет студента + **сохраняет в JSON** |
| `EditStudent` | Редактирует студента + **сохраняет в JSON** |
| `DeleteStudent` | Удаляет студента + **сохраняет в JSON** |

### Ключевые технологии

- **Newtonsoft.Json** - сериализация/десериализация
- **File.ReadAllText()** - чтение JSON файла
- **File.CreateText()** + **StreamWriter** - запись в файл
- **JsonConvert.DeserializeObject<T>()** - преобразование JSON → объект
- **JsonConvert.SerializeObject()** - преобразование объект → JSON

### 📝 Формат JSON файлов

**GroupData.json:**
```json
[
  { "Id": 1, "Name": "ИВТ-11", "Specialty": "...", "Course": 1 }
]
```

**StudentData.json:**
```json
[
  { "Id": 1, "GroupId": 1, "FirstName": "Иван", "LastName": "Иванов", "Birthday": "15.01.2000" }
]
```

### Требования

- .NET Framework / .NET Core
- **Newtonsoft.Json** (установить через NuGet)
- Visual Studio 2019+

### Быстрый старт

1. Установите пакет: `Install-Package Newtonsoft.Json`
2. Создайте папку `DataModels` с JSON файлами
3. Измените `path` в ViewModel на актуальный путь
4. Соберите и запустите проект

---

**Результат:** Приложение сохраняет все изменения (добавление, редактирование, удаление) в JSON-файлы, данные сохраняются между запусками программы.

