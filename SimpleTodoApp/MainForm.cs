using Microsoft.Win32;

namespace SimpleTodoApp
{
    public partial class MainForm : Form
    {
        private ListView listView;
        private TextBox searchEdit;
        private TextBox titleEdit;
        private TextBox descEdit;
        private ComboBox priorityCombo;
        private Button addButton;
        private Button editButton;
        private Button saveButton;
        private Button cancelButton;
        private Button deleteButton;
        private Button completeButton;
        private Label titleLabel;
        private Label descLabel;
        private Label priorityLabel;

        private TodoList todoList;
        private int editingIndex = -1;
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;
        private bool isSearching = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeListViewColumns();
            InitializeTrayIcon();
            LoadTodos();
            RefreshListView();
        }

        private void InitializeListViewColumns()
        {
            if (listView.Columns.Count == 0)
            {
                listView.Columns.Add("ID", 40);
                listView.Columns.Add("Title", 150);
                listView.Columns.Add("Description", 200);
                listView.Columns.Add("Priority", 80);
                listView.Columns.Add("Status", 100);
                listView.Columns.Add("Created", 150);
            }
            listView.MultiSelect = true;
        }

        private void InitializeTrayIcon()
        {
            trayIcon = new NotifyIcon
            {
                Icon = new Icon("img\\todo.ico"),
                Text = "Todo Application",
                Visible = true
            };
            trayIcon.MouseClick += TrayIcon_MouseClick;
            trayIcon.MouseDoubleClick += TrayIcon_MouseDoubleClick;

            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Show", null, (s, e) => { this.Show(); this.WindowState = FormWindowState.Normal; });
            trayMenu.Items.Add("Hide", null, (s, e) => this.Hide());
            trayMenu.Items.Add(new ToolStripSeparator());
            trayMenu.Items.Add("Start with Windows", null, TrayMenu_AutoStart_Click).Name = "AutoStart";
            trayMenu.Items.Add(new ToolStripSeparator());
            trayMenu.Items.Add("Exit", null, (s, e) => { trayIcon.Visible = false; Application.Exit(); });

            UpdateAutoStartMenuItem();
        }

        private void UpdateAutoStartMenuItem()
        {
            var item = trayMenu.Items.Find("AutoStart", false).FirstOrDefault();
            if (item is ToolStripMenuItem menuItem)
            {
                menuItem.Checked = IsAutoStartEnabled();
            }
        }

        private void TrayMenu_AutoStart_Click(object? sender, EventArgs e)
        {
            bool current = IsAutoStartEnabled();
            SetAutoStart(!current);
            UpdateAutoStartMenuItem();
        }

        private void TrayIcon_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                trayMenu.Show(Cursor.Position);
            }
        }

        private void TrayIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || !this.Visible)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.Hide();
            }
        }

        private void LoadTodos()
        {
            todoList = TodoManager.LoadTodos();
            if (todoList == null)
            {
                todoList = new TodoList();
            }
        }

        private void RefreshListView()
        {
            listView.Items.Clear();

            string searchText = searchEdit.Text;
            if (string.IsNullOrEmpty(searchText) || searchText == "Search todos...")
            {
                isSearching = false;
                for (int i = 0; i < todoList.Count; i++)
                {
                    AddTodoToListView(todoList.Todos[i]);
                }
            }
            else
            {
                isSearching = true;
                var filtered = SearchHelper.FilterTodosBySearch(todoList, searchText);
                foreach (var todo in filtered)
                {
                    AddTodoToListView(todo);
                }
            }
        }

        private void AddTodoToListView(Todo todo)
        {
            ListViewItem item = new ListViewItem(todo.Id.ToString());
            item.Tag = todo.Id;
            item.SubItems.Add(todo.Title);
            item.SubItems.Add(todo.Description);
            item.SubItems.Add(todo.Priority.ToString());
            item.SubItems.Add(todo.IsCompleted ? "Completed" : "Pending");
            item.SubItems.Add(todo.CreatedAtDateTime.ToString("yyyy-MM-dd HH:mm"));
            listView.Items.Add(item);
        }

        private void AddButton_Click(object? sender, EventArgs e)
        {
            if (!TodoManager.CanAddTodo(todoList))
            {
                MessageBox.Show("Maximum number of todos reached!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(titleEdit.Text))
            {
                MessageBox.Show("Please enter a title!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Todo todo = new Todo
            {
                Id = todoList.Count + 1,
                Title = titleEdit.Text,
                Description = descEdit.Text,
                Priority = priorityCombo.SelectedIndex + 1,
                Completed = 0,
                CreatedAtDateTime = DateTime.Now
            };

            todoList.Todos[todoList.Count] = todo;
            todoList.Count++;
            TodoManager.SaveTodos(todoList);
            RefreshListView();

            titleEdit.Clear();
            descEdit.Clear();
            priorityCombo.SelectedIndex = 0;
        }

        private void EditButton_Click(object? sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a todo to edit!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (editingIndex != -1)
            {
                MessageBox.Show("Please finish editing the current todo first!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int todoId = (int)listView.SelectedItems[0].Tag;
            Todo? todo = FindTodoById(todoId);
            if (todo == null)
            {
                MessageBox.Show("Selected todo not found!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            editingIndex = todoId;
            titleEdit.Text = todo.Title;
            descEdit.Text = todo.Description;
            priorityCombo.SelectedIndex = todo.Priority - 1;

            saveButton.Visible = true;
            cancelButton.Visible = true;
            titleEdit.Focus();
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            if (editingIndex == -1)
            {
                return;
            }

            Todo? todo = FindTodoById(editingIndex);
            if (todo == null)
            {
                MessageBox.Show("Todo not found!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(titleEdit.Text))
            {
                MessageBox.Show("Title cannot be empty!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            todo.Title = titleEdit.Text;
            todo.Description = descEdit.Text;
            todo.Priority = priorityCombo.SelectedIndex + 1;

            TodoManager.SaveTodos(todoList);
            RefreshListView();

            titleEdit.Clear();
            descEdit.Clear();
            priorityCombo.SelectedIndex = 0;

            saveButton.Visible = false;
            cancelButton.Visible = false;
            editingIndex = -1;
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            if (editingIndex == -1)
            {
                return;
            }

            titleEdit.Clear();
            descEdit.Clear();
            priorityCombo.SelectedIndex = 0;

            saveButton.Visible = false;
            cancelButton.Visible = false;
            editingIndex = -1;
        }

        private void DeleteButton_Click(object? sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a todo to delete!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedItems = listView.SelectedItems.Cast<ListViewItem>().ToList();
            foreach (var item in selectedItems)
            {
                int todoId = (int)item.Tag;
                for (int i = 0; i < todoList.Count; i++)
                {
                    if (todoList.Todos[i].Id == todoId)
                    {
                        for (int j = i; j < todoList.Count - 1; j++)
                        {
                            todoList.Todos[j] = todoList.Todos[j + 1];
                        }
                        todoList.Count--;
                        break;
                    }
                }
            }

            TodoManager.SaveTodos(todoList);
            RefreshListView();
        }

        private void CompleteButton_Click(object? sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a todo to complete!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedItems = listView.SelectedItems.Cast<ListViewItem>().ToList();
            foreach (var item in selectedItems)
            {
                int todoId = (int)item.Tag;
                Todo? todo = FindTodoById(todoId);
                if (todo != null)
                {
                    todo.Completed = 1;
                    todo.CompletedAtDateTime = DateTime.Now;
                }
            }

            TodoManager.SaveTodos(todoList);
            RefreshListView();
        }

        private Todo? FindTodoById(int id)
        {
            for (int i = 0; i < todoList.Count; i++)
            {
                if (todoList.Todos[i].Id == id)
                {
                    return todoList.Todos[i];
                }
            }
            return null;
        }

        private void ListView_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0 && !isSearching)
            {
                int todoId = (int)listView.SelectedItems[0].Tag;
                Todo? todo = FindTodoById(todoId);
                if (todo != null)
                {
                    titleEdit.Text = todo.Title;
                    descEdit.Text = todo.Description;
                    priorityCombo.SelectedIndex = todo.Priority - 1;
                }
            }
        }

        private void ListView_DoubleClick(object? sender, EventArgs e)
        {
            EditButton_Click(sender, e);
        }

        private void ListView_ColumnClick(object? sender, ColumnClickEventArgs e)
        {
            // Simple sorting implementation
            if (listView.Items.Count == 0) return;

            listView.ListViewItemSorter = new ListViewItemComparer(e.Column);
            listView.Sort();
        }

        private void SearchEdit_Enter(object? sender, EventArgs e)
        {
            if (searchEdit.Text == "Search todos...")
            {
                searchEdit.Text = "";
                searchEdit.ForeColor = Color.Black;
            }
        }

        private void SearchEdit_Leave(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchEdit.Text))
            {
                searchEdit.Text = "Search todos...";
                searchEdit.ForeColor = Color.Gray;
                RefreshListView();
            }
        }

        private void SearchEdit_TextChanged(object? sender, EventArgs e)
        {
            if (searchEdit.Text != "Search todos...")
            {
                RefreshListView();
            }
        }

        private void MainForm_Resize(object? sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void MainForm_ResizeEnd(object? sender, EventArgs e)
        {
            // Adjust controls on resize
            int width = this.ClientSize.Width;
            int height = this.ClientSize.Height;

            searchEdit.Width = width - 20;
            listView.Width = width - 20;
            listView.Height = Math.Max(350, height - 420);

            // Adjust input fields
            int y = 410;
            int x = 10;
            int labelW = 90;
            int gap = 10;
            int btnW = 80;
            int btnH = 28;
            int labelH = 18;

            int availableW = width - 20;
            int minEditW = 80;
            int minBtnW = 60;
            int editW = (availableW - labelW * 3 - 50 - minBtnW * 6 - gap * 6);
            if (editW < minEditW * 2) editW = minEditW * 2;
            int singleEditW = editW / 2;
            btnW = (availableW - (labelW * 3 + 50 + singleEditW * 2 + gap * 6)) / 6;
            if (btnW < minBtnW) btnW = minBtnW;

            titleLabel.Location = new Point(x, y);
            titleEdit.Location = new Point(x, y + labelH);
            titleEdit.Width = singleEditW;
            x += singleEditW + gap;

            descLabel.Location = new Point(x, y);
            descEdit.Location = new Point(x, y + labelH);
            descEdit.Width = singleEditW;
            x += singleEditW + gap;

            addButton.Location = new Point(x, y + labelH);
            addButton.Width = btnW;
            x += btnW + gap;

            priorityLabel.Location = new Point(x, y);
            priorityCombo.Location = new Point(x + labelW + gap, y + labelH);
            x += 50 + gap;

            int actionY = y + labelH + btnH + gap;
            int actionX = 10;

            editButton.Location = new Point(actionX, actionY);
            editButton.Width = btnW;
            actionX += btnW + gap;

            saveButton.Location = new Point(actionX, actionY);
            saveButton.Width = btnW;
            actionX += btnW + gap;

            cancelButton.Location = new Point(actionX, actionY);
            cancelButton.Width = btnW;
            actionX += btnW + gap;

            deleteButton.Location = new Point(actionX, actionY);
            deleteButton.Width = btnW;
            actionX += btnW + gap;

            completeButton.Location = new Point(actionX, actionY);
            completeButton.Width = btnW;
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();
            base.OnFormClosed(e);
        }

        private void SetAutoStart(bool enable)
        {
            try
            {
                RegistryKey? key = Registry.CurrentUser.OpenSubKey(
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (key != null)
                {
                    if (enable)
                    {
                        key.SetValue("TodoApp", Application.ExecutablePath);
                    }
                    else
                    {
                        key.DeleteValue("TodoApp", false);
                    }
                    key.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to set auto-start: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsAutoStartEnabled()
        {
            try
            {
                RegistryKey? key = Registry.CurrentUser.OpenSubKey(
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Run", false);
                if (key != null)
                {
                    object? value = key.GetValue("TodoApp");
                    key.Close();
                    return value != null;
                }
            }
            catch { }
            return false;
        }
    }

    // Helper class for ListView sorting
    public class ListViewItemComparer : System.Collections.IComparer
    {
        private int col;

        public ListViewItemComparer(int column)
        {
            col = column;
        }

        public int Compare(object? x, object? y)
        {
            if (x == null || y == null) return 0;
            ListViewItem? itemX = x as ListViewItem;
            ListViewItem? itemY = y as ListViewItem;
            if (itemX == null || itemY == null) return 0;

            string textX = col < itemX.SubItems.Count ? itemX.SubItems[col].Text : "";
            string textY = col < itemY.SubItems.Count ? itemY.SubItems[col].Text : "";

            // Try numeric comparison for ID and Priority columns
            if (col == 0 || col == 3)
            {
                if (int.TryParse(textX, out int numX) && int.TryParse(textY, out int numY))
                {
                    return numX.CompareTo(numY);
                }
            }

            return string.Compare(textX, textY);
        }
    }
}

