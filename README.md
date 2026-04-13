# Лабораторная работа №29

---

## Выполненные изменения

### 1. Модели данных (Model)
- Реализован интерфейс `INotifyPropertyChanged` для всех классов моделей
- Добавлено событие `PropertyChanged` и метод `OnPropertyChanged()`
- Все свойства генерируют уведомления при изменении значений
- Добавлены методы копирования объектов (`ShallowCopy`, `CopyFromStudent`)

### 2. Слой ViewModel
- **GroupViewModel**: управление коллекцией групп, команды CRUD, свойство `SelectedGroup`
- **StudentViewModel**: управление коллекциями студентов, синхронизация данных, свойство `SelectedStudentDPO`

### 3. Команды (Commands)
- Создан класс `RelayCommand`, реализующий `ICommand`
- Реализованы команды: `Add`, `Edit`, `Delete` для групп и студентов
- Команды редактирования/удаления активны только при выбранном элементе
- Логика кнопок перенесена из View в ViewModel

### 4. Очистка View
- Удалены все обработчики событий `Click` из code-behind
- Убрана ручная работа с `ItemsSource` и элементами управления
- В конструкторах окон осталась только инициализация и установка `DataContext`

### 5. Привязка данных (XAML)
- Кнопки: `Command="{Binding AddGroup}"` вместо `Click="..."`
- Списки: `ItemsSource="{Binding ListGroup}"`, `SelectedItem="{Binding SelectedGroup}"`
- Элементы ввода: двусторонняя привязка с `UpdateSourceTrigger=PropertyChanged`

### 6. Стили и ресурсы
- Создан `Dictionary1.xaml` со стилем `ButtonMenu` для кнопок
- Словарь подключён в `App.xaml` через `MergedDictionaries`
- Единое оформление всех кнопок управления

---

## 📁 Структура проекта
```
WpfApp2/
├── Helper/
│   ├── RelayCommand.cs      # Реализация ICommand
│   └── FindGroup.cs         # Вспомогательный класс
├── Model/
│   ├── Group.cs             # Модель группы
│   ├── Student.cs           # Модель студента
│   └── StudentDPO.cs        # DTO для отображения
├── ViewModel/
│   ├── GroupViewModel.cs    # Логика работы с группами
│   └── StudentViewModel.cs  # Логика работы со студентами
├── View/                    # Окна приложения (XAML + минимальный CS)
├── Dictionary1.xaml         # Словарь стилей
└── App.xaml                 # Подключение ресурсов
```
