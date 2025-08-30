# Project 4 B-Side - ToDo Management System

A comprehensive, console-based ToDo task management system with filtering, sorting, multi-format data persistence, and multi-channel notifications (Console, Email, Telegram).

## 📁 Project Structure

```
Project_4_B_Side/
├── C# Filters and Sorters/    # Library for data filtering and sorting
├── C# Info/                   # Core data models (User, Statistics)
├── MenuLibrary/               # Console menu system and application logic
├── Messages/                  # Notification system (Console, Email, Telegram)
├── Parsers/                   # Data parsers for CSV and JSON formats
├── Project_4_B_Side/          # Main application project
├── ToDoLib/                   # Core ToDo task models and collection management
└── WorkingFiles/              # Data storage directory
    ├── ToDo.csv
    └── ToDo.json
```

---

## 🧩 Modules Overview

### 1. ToDoLib (Core Task Library)
**Purpose:** Provides the fundamental data structures and logic for representing and managing ToDo tasks.
**Key Components:**
*   **`ToDo`**: The main class representing a task with properties like `Id`, `Name`, `Description`, `Category`, `Created`, `DeadlineDate`, `Status`, and `Priority`.
*   **`ToDoCollection`**: A managed collection of `ToDo` items with methods to add, remove, find, and display tasks in a formatted table (using Spectre.Console).
*   **`ToDoStatus` & `ToDoPriority`**: Enums defining possible task states (`Active`, `Completed`, `Postponed`) and importance levels (`High`, `Medium`, `Low`).
*   **`ToDoCategories`**: A static class managing a list of available task categories (e.g., "Work", "Personal", "Study").
*   **`MyEnumExtensions`**: Provides formatted, color-coded string output for `ToDoPriority` and `ToDoStatus` enums.

---

### 2. C# Filters and Sorters
**Purpose:** A reusable library for applying filter and sort operations to any collection of data, providing a pipeline pattern for sequential processing.
**Key Components:**
*   **`Filter<T>`**: Applies a predicate function to a collection to select items that meet a specific condition.
*   **`Sorter<T>`**: Sorts a collection based on a specified key and supports ascending/descending order.
*   **`Pipeline<T>`**: Chains multiple `Filter<T>` and `Sorter<T>` operations together to be applied sequentially to a collection.
*   **`IApplyable<T>`**: The interface that all operations implement, ensuring flexibility and extensibility.

---

### 3. Parsers (Data Persistence)
**Purpose:** Handles the import and export of `ToDoCollection` data from and to CSV and JSON files.
**Key Components:**
*   **`CsvParser`**: A static class for CSV file operations.
    *   `CsvToCollection`: Reads a CSV file (using `;` as a delimiter, ignoring the header) and creates a `ToDoCollection`.
    *   `WriteToCsv`: Writes a `ToDoCollection` to a CSV file.
*   **`JsonParser`**: A static class for JSON file operations.
    *   `JsonToCollection`: Deserializes a JSON file into a `ToDoCollection`.
    *   `WriteToJson`: Serializes a `ToDoCollection` into a well-formatted JSON file.
*   **Robustness**: Both parsers include exception handling to manage corrupt or incorrectly formatted files.

---

### 4. Messages (Notification System)
**Purpose:** Manages sending notifications to the user through various channels when a task deadline is approaching.
**Key Components:**
*   **`INotifiable`**: The common interface implemented by all notification types.
*   **`ConsoleNotification`**: Sends a notification to the application console.
*   **`EmailNotification`**: Sends a notification via email (requires configuration).
*   **`TelegramNotification`**: Sends a notification to a user via a Telegram bot.
*   **`NotificationInfo` & `ToDoInfo`**: Data classes containing all necessary information for constructing and sending a notification.
*   **`Report`**: A class for generating reports on task completion.

---

### 5. C# Info (Data & User Management)
**Purpose:** Defines the data models for user information, authentication, and statistics tracking.
**Key Components:**
*   **`User`**: Stores user data such as name, email, Telegram username, and password.
*   **`UserDataManager`**: A central class that manages all user data and provides information about the current user.
*   **`Statistics`**: Tracks and stores user productivity statistics over a specific period.
*   **`Months`**: A helper class/enum for managing months of the year.

---

### 6. MenuLibrary (Application Interface)
**Purpose:** The core of the console application, providing a hierarchical menu system for user interaction and orchestrating all other modules.
**Key Components:**
*   **`Menu`**: The abstract base class for all menu types.
*   **`Auth`**: Handles user registration and login, validating input like email format and non-empty passwords.
*   **`MainManagerMenu`**: The main entry point menu, offering options to view tasks, edit data, set filters, analyze productivity, and exit.
*   **Specialized Manager Menus**:
    *   **`ManagerMenuData`**: For adding, editing, and deleting tasks.
    *   **`ManagerMenuFilter`**: For applying and resetting filters based on status and priority.
    *   **`ManagerMenuPrint`**: For displaying tasks as a table, calendar, or statistical charts.
    *   **`ManagerMenuProductivity`**: For analyzing productivity over yearly, monthly, and weekly periods.
    *   **`ManagerMenuExit`**: Handles the graceful shutdown of the app, saving data and stopping the Telegram bot.
    *   **`TableManagerMenu`**: A powerful menu for displaying tasks with real-time filtering and sorting capabilities.

---

## 🚀 How It All Works Together

1.  **Startup**: The `Project_4_B_Side` application starts, initializing the `MainManagerMenu`.
2.  **Authentication**: The `Auth` class prompts the user to log in or register.
3.  **Data Loading**: Upon successful login, the `MainManagerMenu` uses the `Parsers` module to load the user's tasks from a CSV or JSON file in the `WorkingFiles` directory into a `ToDoCollection`.
4.  **User Interaction**: The user navigates through the menus:
    *   The `MenuLibrary` renders options and captures user input.
    *   The `C# Filters and Sorters` library is used by `ManagerMenuFilter` and `TableManagerMenu` to process the `ToDoCollection`.
    *   Any changes made (add, edit, delete) are performed on the `ToDoCollection` object.
5.  **Notifications**: The `Messages` system checks for upcoming deadlines and sends notifications via the configured channels (Console, Email, Telegram).
6.  **Persistence**: Upon exiting via `ManagerMenuExit`, the updated `ToDoCollection` is saved back to the data file using the `Parsers` module.

---

## ✅ Correct Input Data Formats

*   **User Registration/Login**:
    *   **Name**: Non-empty.
    *   **Telegram Nickname**: Must contain an `@` symbol.
    *   **Email**: Must be a valid format (e.g., `example@example.com`).
    *   **Password**: Non-empty.
*   **Tasks**:
    *   **Deadline/Notification Date**: Must be in format `dd/MM/yyyy HH:mm:ss` and cannot be a past date.
    *   **Status**: Must be `Active`, `Completed`, or `Postponed`.
    *   **Priority**: Must be `High`, `Medium`, or `Low`.
*   **Files**: Supported formats are `.txt`, `.csv`, `.json`.

---

## 📋 Example Usage

```csharp
// The application is run by starting the main menu.
var mainMenu = new MainManagerMenu();
mainMenu.Run(); // Launches the interactive console interface.
```

## 🔄 Example Data Formats

**CSV (ToDo.csv):**
```csv
Id;Name;Description;Category;Created;DeadLineDate;Status;Priority
1;Task 1;Description 1;Work;2023-10-01;2023-10-10;Active;High
```

**JSON (ToDo.json):**
```json
[
    {
        "Id": 1,
        "Name": "Task 1",
        "Description": "Description 1",
        "Category": "Work",
        "Created": "2023-10-01",
        "DeadlineDate": "2023-10-10",
        "Status": "Active",
        "Priority": "High"
    }
]
```
Appropriate data is presented in a working files
