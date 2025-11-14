using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SimpleTodoApp
{
    public static class TodoManager
    {
        private const string DataFileName = "todos.dat";
        private const int MaxTodos = 100;
        private const int MaxTitleLength = 100;
        private const int MaxDescLength = 500;

        public static string GetAppDataPath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appDataPath, "TodoApp");
        }

        public static void InitAppData()
        {
            string appDataPath = GetAppDataPath();
            if (!Directory.Exists(appDataPath))
            {
                try
                {
                    Directory.CreateDirectory(appDataPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to create directory: {appDataPath}\nError: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                }
            }
        }

        public static TodoList LoadTodos()
        {
            InitAppData();
            string filePath = Path.Combine(GetAppDataPath(), DataFileName);

            if (!File.Exists(filePath))
            {
                return new TodoList();
            }

            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read)))
                {
                    TodoList list = new TodoList();
                    
                    // Read all todos (100 todos in the array)
                    for (int i = 0; i < MaxTodos; i++)
                    {
                        Todo todo = new Todo();
                        
                        // Read id (int - 4 bytes)
                        todo.Id = reader.ReadInt32();
                        
                        // Read title (char[100] - 100 bytes, null-terminated)
                        byte[] titleBytes = reader.ReadBytes(MaxTitleLength);
                        todo.Title = Encoding.Default.GetString(titleBytes).TrimEnd('\0');
                        
                        // Read description (char[500] - 500 bytes, null-terminated)
                        byte[] descBytes = reader.ReadBytes(MaxDescLength);
                        todo.Description = Encoding.Default.GetString(descBytes).TrimEnd('\0');
                        
                        // Read priority (int - 4 bytes)
                        todo.Priority = reader.ReadInt32();
                        
                        // Read completed (int - 4 bytes)
                        todo.Completed = reader.ReadInt32();
                        
                        // Read created_at (time_t - 8 bytes as long)
                        todo.CreatedAt = reader.ReadInt64();
                        
                        // Read completed_at (time_t - 8 bytes as long)
                        todo.CompletedAt = reader.ReadInt64();
                        
                        list.Todos[i] = todo;
                    }
                    
                    // Read count (int - 4 bytes)
                    list.Count = reader.ReadInt32();
                    
                    // Validate count
                    if (list.Count < 0 || list.Count > MaxTodos)
                    {
                        list.Count = 0;
                    }
                    
                    return list;
                }
            }
            catch
            {
                // If file format doesn't match, backup and return empty list
                string backupPath = filePath + ".corrupted." + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Copy(filePath, backupPath, true);
                        File.Delete(filePath);
                        MessageBox.Show($"The todos file is corrupted or in an invalid format.\n" +
                            $"A backup has been saved to:\n{backupPath}\n\n" +
                            $"Starting with an empty todo list.", 
                            "File Format Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch
                {
                    // If backup fails, try to just delete
                    try { File.Delete(filePath); } catch { }
                }
                return new TodoList();
            }
        }

        public static void SaveTodos(TodoList list)
        {
            if (list == null)
            {
                return;
            }

            InitAppData();
            string filePath = Path.Combine(GetAppDataPath(), DataFileName);

            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create, FileAccess.Write)))
                {
                    // Write all todos (100 todos in the array)
                    for (int i = 0; i < MaxTodos; i++)
                    {
                        Todo todo = i < list.Count ? list.Todos[i] : new Todo();
                        
                        // Write id (int - 4 bytes)
                        writer.Write(todo.Id);
                        
                        // Write title (char[100] - 100 bytes, null-padded)
                        byte[] titleBytes = new byte[MaxTitleLength];
                        if (!string.IsNullOrEmpty(todo.Title))
                        {
                            byte[] temp = Encoding.Default.GetBytes(todo.Title);
                            int copyLen = Math.Min(temp.Length, MaxTitleLength - 1);
                            Array.Copy(temp, 0, titleBytes, 0, copyLen);
                        }
                        writer.Write(titleBytes);
                        
                        // Write description (char[500] - 500 bytes, null-padded)
                        byte[] descBytes = new byte[MaxDescLength];
                        if (!string.IsNullOrEmpty(todo.Description))
                        {
                            byte[] temp = Encoding.Default.GetBytes(todo.Description);
                            int copyLen = Math.Min(temp.Length, MaxDescLength - 1);
                            Array.Copy(temp, 0, descBytes, 0, copyLen);
                        }
                        writer.Write(descBytes);
                        
                        // Write priority (int - 4 bytes)
                        writer.Write(todo.Priority);
                        
                        // Write completed (int - 4 bytes)
                        writer.Write(todo.Completed);
                        
                        // Write created_at (time_t - 8 bytes as long)
                        writer.Write(todo.CreatedAt);
                        
                        // Write completed_at (time_t - 8 bytes as long)
                        writer.Write(todo.CompletedAt);
                    }
                    
                    // Write count (int - 4 bytes)
                    writer.Write(list.Count);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save todos: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool CanAddTodo(TodoList list)
        {
            return list.Count < MaxTodos;
        }
    }
}

