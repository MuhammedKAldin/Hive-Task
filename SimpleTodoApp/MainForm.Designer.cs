using System.Drawing;
using System.Windows.Forms;

namespace SimpleTodoApp
{
    partial class MainForm
    {
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            searchEdit = new TextBox();
            listView = new ListView();
            titleLabel = new Label();
            titleEdit = new TextBox();
            descLabel = new Label();
            descEdit = new TextBox();
            addButton = new Button();
            priorityLabel = new Label();
            priorityCombo = new ComboBox();
            editButton = new Button();
            saveButton = new Button();
            cancelButton = new Button();
            deleteButton = new Button();
            completeButton = new Button();
            SuspendLayout();
            // 
            // searchEdit
            // 
            searchEdit.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            searchEdit.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            searchEdit.ForeColor = Color.Gray;
            searchEdit.Location = new Point(10, 10);
            searchEdit.Name = "searchEdit";
            searchEdit.Size = new Size(780, 23);
            searchEdit.TabIndex = 0;
            searchEdit.Text = "Search todos...";
            searchEdit.TextChanged += SearchEdit_TextChanged;
            searchEdit.Enter += SearchEdit_Enter;
            searchEdit.Leave += SearchEdit_Leave;
            // 
            // listView
            // 
            listView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listView.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Location = new Point(10, 48);
            listView.Name = "listView";
            listView.Size = new Size(780, 322);
            listView.TabIndex = 1;
            listView.UseCompatibleStateImageBehavior = false;
            listView.View = View.Details;
            listView.ColumnClick += ListView_ColumnClick;
            listView.SelectedIndexChanged += ListView_SelectedIndexChanged;
            listView.DoubleClick += ListView_DoubleClick;
            // 
            // titleLabel
            // 
            titleLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            titleLabel.Location = new Point(10, 380);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(90, 18);
            titleLabel.TabIndex = 2;
            titleLabel.Text = "Title:";
            // 
            // titleEdit
            // 
            titleEdit.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            titleEdit.Location = new Point(10, 398);
            titleEdit.Name = "titleEdit";
            titleEdit.Size = new Size(120, 23);
            titleEdit.TabIndex = 3;
            // 
            // descLabel
            // 
            descLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            descLabel.Location = new Point(130, 380);
            descLabel.Name = "descLabel";
            descLabel.Size = new Size(90, 18);
            descLabel.TabIndex = 4;
            descLabel.Text = "Description:";
            // 
            // descEdit
            // 
            descEdit.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            descEdit.Location = new Point(130, 398);
            descEdit.Name = "descEdit";
            descEdit.Size = new Size(120, 23);
            descEdit.TabIndex = 5;
            // 
            // addButton
            // 
            addButton.BackColor = Color.FromArgb(0, 120, 215);
            addButton.FlatAppearance.BorderSize = 0;
            addButton.FlatStyle = FlatStyle.Flat;
            addButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            addButton.ForeColor = Color.White;
            addButton.Location = new Point(260, 398);
            addButton.Name = "addButton";
            addButton.Size = new Size(80, 28);
            addButton.TabIndex = 6;
            addButton.Text = "Add";
            addButton.UseVisualStyleBackColor = false;
            addButton.Click += AddButton_Click;
            // 
            // priorityLabel
            // 
            priorityLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            priorityLabel.Location = new Point(350, 380);
            priorityLabel.Name = "priorityLabel";
            priorityLabel.Size = new Size(90, 18);
            priorityLabel.TabIndex = 7;
            priorityLabel.Text = "Priority:";
            // 
            // priorityCombo
            // 
            priorityCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            priorityCombo.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            priorityCombo.Items.AddRange(new object[] { "1", "2", "3", "4", "5" });
            priorityCombo.Location = new Point(450, 398);
            priorityCombo.Name = "priorityCombo";
            priorityCombo.Size = new Size(50, 23);
            priorityCombo.TabIndex = 8;
            // 
            // editButton
            // 
            editButton.BackColor = Color.FromArgb(0, 120, 215);
            editButton.FlatAppearance.BorderSize = 0;
            editButton.FlatStyle = FlatStyle.Flat;
            editButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            editButton.ForeColor = Color.White;
            editButton.Location = new Point(10, 436);
            editButton.Name = "editButton";
            editButton.Size = new Size(80, 28);
            editButton.TabIndex = 9;
            editButton.Text = "Edit";
            editButton.UseVisualStyleBackColor = false;
            editButton.Click += EditButton_Click;
            // 
            // saveButton
            // 
            saveButton.BackColor = Color.FromArgb(0, 120, 215);
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.FlatStyle = FlatStyle.Flat;
            saveButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            saveButton.ForeColor = Color.White;
            saveButton.Location = new Point(100, 436);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(80, 28);
            saveButton.TabIndex = 10;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = false;
            saveButton.Visible = false;
            saveButton.Click += SaveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.BackColor = Color.FromArgb(0, 120, 215);
            cancelButton.FlatAppearance.BorderSize = 0;
            cancelButton.FlatStyle = FlatStyle.Flat;
            cancelButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cancelButton.ForeColor = Color.White;
            cancelButton.Location = new Point(190, 436);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(80, 28);
            cancelButton.TabIndex = 11;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = false;
            cancelButton.Visible = false;
            cancelButton.Click += CancelButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.BackColor = Color.FromArgb(0, 120, 215);
            deleteButton.FlatAppearance.BorderSize = 0;
            deleteButton.FlatStyle = FlatStyle.Flat;
            deleteButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            deleteButton.ForeColor = Color.White;
            deleteButton.Location = new Point(280, 436);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(80, 28);
            deleteButton.TabIndex = 12;
            deleteButton.Text = "Delete";
            deleteButton.UseVisualStyleBackColor = false;
            deleteButton.Click += DeleteButton_Click;
            // 
            // completeButton
            // 
            completeButton.BackColor = Color.FromArgb(0, 120, 215);
            completeButton.FlatAppearance.BorderSize = 0;
            completeButton.FlatStyle = FlatStyle.Flat;
            completeButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            completeButton.ForeColor = Color.White;
            completeButton.Location = new Point(370, 436);
            completeButton.Name = "completeButton";
            completeButton.Size = new Size(80, 28);
            completeButton.TabIndex = 13;
            completeButton.Text = "Complete";
            completeButton.UseVisualStyleBackColor = false;
            completeButton.Click += CompleteButton_Click;
            // 
            // MainForm
            // 
            BackColor = Color.FromArgb(240, 240, 240);
            ClientSize = new Size(800, 650);
            Controls.Add(searchEdit);
            Controls.Add(listView);
            Controls.Add(titleLabel);
            Controls.Add(titleEdit);
            Controls.Add(descLabel);
            Controls.Add(descEdit);
            Controls.Add(addButton);
            Controls.Add(priorityLabel);
            Controls.Add(priorityCombo);
            Controls.Add(editButton);
            Controls.Add(saveButton);
            Controls.Add(cancelButton);
            Controls.Add(deleteButton);
            Controls.Add(completeButton);
            MinimumSize = new Size(600, 500);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Todo Application";
            FormClosing += MainForm_FormClosing;
            ResizeEnd += MainForm_ResizeEnd;
            Resize += MainForm_Resize;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
