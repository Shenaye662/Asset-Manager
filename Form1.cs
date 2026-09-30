using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace Asset_manager_1
{
    public partial class Form1 : Form
    {
        // =========================================================
        // THEME
        // =========================================================

        private string saveFile;
        private bool darkMode = true;

        private readonly Color backgroundLight =
            Color.FromArgb(245, 247, 251);

        private readonly Color panelLight =
            Color.White;

        private readonly Color textLight =
            Color.FromArgb(35, 42, 55);

        private readonly Color backgroundDark =
            Color.FromArgb(18, 20, 24);

        private readonly Color panelDark =
            Color.FromArgb(28, 31, 36);

        private readonly Color textDark =
            Color.White;

        private readonly Color mutedDark =
            Color.FromArgb(165, 172, 184);

        private readonly Color mutedLight =
            Color.FromArgb(105, 115, 130);

        private readonly Color accent =
            Color.FromArgb(80, 120, 235);


        // =========================================================
        // INPUT FIELDS
        // =========================================================

        TextBox assetTagBox;
        TextBox modelBox;
        TextBox serialBox;
        TextBox assignedUserBox;
        TextBox commentsBox;

        ComboBox categoryBox;
        ComboBox makeBox;
        ComboBox statusBox;

        DateTimePicker warrantyPicker;


        // =========================================================
        // BUTTONS
        // =========================================================

        Button addButton;
        Button updateButton;
        Button deleteButton;
        Button uploadButton;
        Button viewButton;
        Button clearButton;

        Button searchButton;
        Button refreshButton;
        Button themeButton;


        // =========================================================
        // DASHBOARD / SEARCH
        // =========================================================

        TextBox searchBox;

        DataGridView assetGrid;

        Label totalAssetsLabel;
        Label activeAssetsLabel;
        Label repairAssetsLabel;
        Label disposedAssetsLabel;

        // Separate total assets counter in header
        Label headerTotalAssetsLabel;

        Panel headerPanel;
        Panel leftPanel;
        Panel rightPanel;


        // =========================================================
        // ASSET DATA
        // =========================================================

        private class AssetData
        {
            public string AssetTag { get; set; } = "";
            public string Category { get; set; } = "";
            public string Make { get; set; } = "";
            public string Model { get; set; } = "";
            public string Serial { get; set; } = "";
            public string User { get; set; } = "";
            public string Status { get; set; } = "";
            public string Warranty { get; set; } = "";
            public string Comments { get; set; } = "";
        }


        // =========================================================
        // FORM
        // =========================================================

        public Form1()
        {
            saveFile = Path.Combine(
                Application.StartupPath,
                "assets.json");

            BuildInterface();

            LoadSavedAssets();

            ApplyTheme();

            AdjustLayout();
        }


        // =========================================================
        // BUILD INTERFACE
        // =========================================================

        private void BuildInterface()
        {
            Text = "Asset Manager";

            WindowState = FormWindowState.Maximized;

            StartPosition =
                FormStartPosition.CenterScreen;

            MinimumSize =
                new Size(1200, 750);

            Font =
                new Font("Segoe UI", 9F);

            FormBorderStyle =
                FormBorderStyle.Sizable;

            DoubleBuffered = true;

            BuildHeader();

            BuildLeftPanel();

            BuildRightPanel();

            Resize += Form1_Resize;
        }


        // =========================================================
        // HEADER
        // =========================================================

        private void BuildHeader()
        {
            headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(ClientSize.Width, 92),
                Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Left |
                    AnchorStyles.Right
            };

            Controls.Add(headerPanel);


            // Main title
            Label title = new Label
            {
                Text = "ASSET MANAGER",
                Font =
                    new Font(
                        "Segoe UI",
                        23F,
                        FontStyle.Bold),

                Location =
                    new Point(28, 16),

                AutoSize = true
            };

            headerPanel.Controls.Add(title);


            // Subtitle
            Label subtitle = new Label
            {
                Text =
                    "Manage and monitor your company's IT assets",

                Font =
                    new Font(
                        "Segoe UI",
                        9.5F),

                Location =
                    new Point(31, 55),

                AutoSize = true,

                Tag = "muted"
            };

            headerPanel.Controls.Add(subtitle);


            // =====================================================
            // HEADER TOTAL ASSETS
            // =====================================================

            headerTotalAssetsLabel = new Label
            {
                Text = "TOTAL ASSETS: 0",

                Font =
                    new Font(
                        "Segoe UI",
                        10F,
                        FontStyle.Bold),

                AutoSize = true,

                Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Right
            };

            headerPanel.Controls.Add(
                headerTotalAssetsLabel);


            // Theme button
            themeButton = CreateButton(
                "☾  Theme",
                120,
                40,
                Color.FromArgb(45, 48, 55));

            themeButton.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            themeButton.Location =
                new Point(
                    headerPanel.ClientSize.Width - 145,
                    25);

            themeButton.Click +=
                ThemeButton_Click;

            headerPanel.Controls.Add(themeButton);
        }


        // =========================================================
        // LEFT PANEL
        // =========================================================

        private void BuildLeftPanel()
        {
            leftPanel = new Panel
            {
                Location =
                    new Point(20, 112),

                Size =
                    new Size(
                        400,
                        ClientSize.Height - 132),

                Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Bottom |
                    AnchorStyles.Left,

                Padding =
                    new Padding(22),

                AutoScroll = true
            };

            Controls.Add(leftPanel);


            Label formTitle = new Label
            {
                Text = "Add / Edit Asset",

                Font =
                    new Font(
                        "Segoe UI",
                        17F,
                        FontStyle.Bold),

                Location =
                    new Point(22, 18),

                AutoSize = true
            };

            leftPanel.Controls.Add(formTitle);


            Label hint = new Label
            {
                Text =
                    "Enter asset information below",

                Font =
                    new Font(
                        "Segoe UI",
                        8.5F),

                Location =
                    new Point(24, 47),

                AutoSize = true,

                Tag = "muted"
            };

            leftPanel.Controls.Add(hint);


            // =====================================================
            // ASSET INFORMATION
            // =====================================================

            AddSectionLabel(
                "ASSET INFORMATION",
                78);


            CreateFieldLabel(
                "Asset Tag",
                105);

            assetTagBox =
                CreateTextBox(128);


            CreateFieldLabel(
                "Category",
                169);

            categoryBox =
                CreateComboBox(192);

            categoryBox.Items.AddRange(
                new object[]
                {
                    "Laptop",
                    "Desktop",
                    "Monitor",
                    "Printer",
                    "Keyboard",
                    "Mouse",
                    "Tablet",
                    "Phone",
                    "Server",
                    "Other"
                });


            CreateFieldLabel(
                "Make",
                233);

            makeBox =
                CreateComboBox(256);

            makeBox.Items.AddRange(
                new object[]
                {
                    "Dell",
                    "HP",
                    "Lenovo",
                    "Apple",
                    "Microsoft",
                    "Samsung",
                    "Acer",
                    "Asus",
                    "Other"
                });


            CreateFieldLabel(
                "Model",
                297);

            modelBox =
                CreateTextBox(320);


            // =====================================================
            // ASSIGNMENT
            // =====================================================

            AddSectionLabel(
                "ASSIGNMENT",
                361);


            CreateFieldLabel(
                "Serial Number",
                388);

            serialBox =
                CreateTextBox(411);


            CreateFieldLabel(
                "Assigned User",
                452);

            assignedUserBox =
                CreateTextBox(475);


            CreateFieldLabel(
                "Status",
                516);

            statusBox =
                CreateComboBox(539);

            statusBox.Items.AddRange(
                new object[]
                {
                    "Active",
                    "In Repair",
                    "Disposed"
                });


            // =====================================================
            // WARRANTY & NOTES
            // =====================================================

            AddSectionLabel(
                "WARRANTY & NOTES",
                580);


            CreateFieldLabel(
                "Warranty Date",
                607);

            warrantyPicker = new DateTimePicker
            {
                Location =
                    new Point(22, 630),

                Size =
                    new Size(352, 31),

                Format =
                    DateTimePickerFormat.Short,

                Font =
                    new Font(
                        "Segoe UI",
                        9.5F)
            };

            leftPanel.Controls.Add(
                warrantyPicker);


            // =====================================================
            // COMMENTS
            // =====================================================

            CreateFieldLabel(
                "Comments",
                671);

            commentsBox = new TextBox
            {
                Location =
                    new Point(22, 694),

                Size =
                    new Size(352, 58),

                Multiline = true,

                ScrollBars =
                    ScrollBars.Vertical,

                Font =
                    new Font(
                        "Segoe UI",
                        9.5F),

                BorderStyle =
                    BorderStyle.FixedSingle
            };

            leftPanel.Controls.Add(
                commentsBox);


            // =====================================================
            // BUTTONS
            // =====================================================

            FlowLayoutPanel buttonPanel =
                new FlowLayoutPanel
                {
                    Location =
                        new Point(22, 772),

                    Size =
                        new Size(352, 105),

                    FlowDirection =
                        FlowDirection.LeftToRight,

                    WrapContents = true,

                    Padding =
                        new Padding(0),

                    Margin =
                        new Padding(0),

                    BackColor =
                        Color.Transparent
                };

            leftPanel.Controls.Add(
                buttonPanel);


            addButton = CreateButton(
                "＋  Add Asset",
                108,
                40,
                Color.FromArgb(0, 170, 125));


            updateButton = CreateButton(
                "↻  Update",
                108,
                40,
                Color.FromArgb(70, 115, 220));


            deleteButton = CreateButton(
                "✕  Delete",
                108,
                40,
                Color.FromArgb(215, 70, 75));


            uploadButton = CreateButton(
                "↑  Upload Form",
                108,
                40,
                Color.FromArgb(120, 85, 195));


            viewButton = CreateButton(
                "◉  View Form",
                108,
                40,
                Color.FromArgb(85, 95, 115));


            clearButton = CreateButton(
                "⌫  Clear Fields",
                108,
                40,
                Color.FromArgb(215, 145, 45));


            addButton.Click +=
                AddButton_Click;

            updateButton.Click +=
                UpdateButton_Click;

            deleteButton.Click +=
                DeleteButton_Click;

            uploadButton.Click +=
                UploadButton_Click;

            viewButton.Click +=
                ViewButton_Click;

            clearButton.Click +=
                ClearButton_Click;


            buttonPanel.Controls.AddRange(
                new Control[]
                {
                    addButton,
                    updateButton,
                    deleteButton,
                    uploadButton,
                    viewButton,
                    clearButton
                });
        }


        // =========================================================
        // RIGHT PANEL
        // =========================================================

        private void BuildRightPanel()
        {
            rightPanel = new Panel
            {
                Location =
                    new Point(440, 112),

                Size =
                    new Size(
                        ClientSize.Width - 460,
                        ClientSize.Height - 132),

                Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Bottom |
                    AnchorStyles.Left |
                    AnchorStyles.Right,

                Padding =
                    new Padding(20)
            };

            Controls.Add(rightPanel);


            Label dashboardTitle = new Label
            {
                Text = "Dashboard",

                Font =
                    new Font(
                        "Segoe UI",
                        17F,
                        FontStyle.Bold),

                Location =
                    new Point(20, 18),

                AutoSize = true
            };

            rightPanel.Controls.Add(
                dashboardTitle);


            Label dashboardHint = new Label
            {
                Text =
                    "Overview of your current asset inventory",

                Font =
                    new Font(
                        "Segoe UI",
                        8.5F),

                Location =
                    new Point(22, 47),

                AutoSize = true,

                Tag = "muted"
            };

            rightPanel.Controls.Add(
                dashboardHint);


            // =====================================================
            // DASHBOARD CARDS
            // =====================================================

            totalAssetsLabel =
                CreateStatCard(
                    "TOTAL ASSETS",
                    20,
                    Color.FromArgb(70, 120, 225));


            activeAssetsLabel =
                CreateStatCard(
                    "ACTIVE",
                    250,
                    Color.FromArgb(0, 175, 130));


            repairAssetsLabel =
                CreateStatCard(
                    "IN REPAIR",
                    480,
                    Color.FromArgb(225, 150, 50));


            disposedAssetsLabel =
                CreateStatCard(
                    "DISPOSED",
                    710,
                    Color.FromArgb(215, 70, 75));


            // =====================================================
            // SEARCH
            // =====================================================

            searchBox = new TextBox
            {
                PlaceholderText =
                    "Search by tag, category, make, model, user or comments...",

                Location =
                    new Point(20, 178),

                Size =
                    new Size(600, 38),

                Font =
                    new Font(
                        "Segoe UI",
                        9.5F),

                BorderStyle =
                    BorderStyle.FixedSingle
            };

            rightPanel.Controls.Add(
                searchBox);


            searchButton = CreateButton(
                "⌕  Search",
                105,
                38,
                Color.FromArgb(65, 115, 220));

            searchButton.Location =
                new Point(630, 178);

            searchButton.Click +=
                SearchButton_Click;

            rightPanel.Controls.Add(
                searchButton);


            refreshButton = CreateButton(
                "↻  Refresh",
                105,
                38,
                Color.FromArgb(80, 88, 105));

            refreshButton.Location =
                new Point(745, 178);

            refreshButton.Click +=
                RefreshButton_Click;

            rightPanel.Controls.Add(
                refreshButton);


            // =====================================================
            // INVENTORY TITLE
            // =====================================================

            Label inventoryTitle = new Label
            {
                Text = "Asset Inventory",

                Font =
                    new Font(
                        "Segoe UI",
                        11F,
                        FontStyle.Bold),

                Location =
                    new Point(22, 229),

                AutoSize = true
            };

            rightPanel.Controls.Add(
                inventoryTitle);


            // =====================================================
            // ASSET TABLE
            // =====================================================

            assetGrid = new DataGridView
            {
                Location =
                    new Point(20, 260),

                Size =
                    new Size(
                        rightPanel.ClientSize.Width - 40,
                        rightPanel.ClientSize.Height - 280),

                Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Bottom |
                    AnchorStyles.Left |
                    AnchorStyles.Right,

                AllowUserToAddRows = false,

                AllowUserToDeleteRows = false,

                ReadOnly = true,

                MultiSelect = false,

                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,

                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,

                RowHeadersVisible = false,

                ColumnHeadersHeight = 42,

                BorderStyle =
                    BorderStyle.None,

                EnableHeadersVisualStyles = false
            };

            assetGrid.RowTemplate.Height = 38;


            assetGrid.Columns.Add(
                "AssetTag",
                "Asset Tag");

            assetGrid.Columns.Add(
                "Category",
                "Category");

            assetGrid.Columns.Add(
                "Make",
                "Make");

            assetGrid.Columns.Add(
                "Model",
                "Model");

            assetGrid.Columns.Add(
                "Serial",
                "Serial Number");

            assetGrid.Columns.Add(
                "User",
                "Assigned User");

            assetGrid.Columns.Add(
                "Status",
                "Status");

            assetGrid.Columns.Add(
                "Warranty",
                "Warranty");

            // NEW COMMENTS COLUMN
            assetGrid.Columns.Add(
                "Comments",
                "Comments");


            assetGrid.CellClick +=
                AssetGrid_CellClick;

            assetGrid.CellFormatting +=
                AssetGrid_CellFormatting;

            rightPanel.Controls.Add(
                assetGrid);
        }


        // =========================================================
        // SECTION LABEL
        // =========================================================

        private void AddSectionLabel(
            string text,
            int y)
        {
            Label label = new Label
            {
                Text = text,

                Location =
                    new Point(23, y),

                AutoSize = true,

                Font =
                    new Font(
                        "Segoe UI",
                        8F,
                        FontStyle.Bold),

                ForeColor =
                    accent,

                Tag = "section"
            };

            leftPanel.Controls.Add(
                label);
        }


        // =========================================================
        // FIELD LABEL
        // =========================================================

        private void CreateFieldLabel(
            string text,
            int y)
        {
            Label label = new Label
            {
                Text = text,

                Location =
                    new Point(22, y),

                AutoSize = true,

                Font =
                    new Font(
                        "Segoe UI",
                        8.5F,
                        FontStyle.Bold)
            };

            leftPanel.Controls.Add(
                label);
        }


        // =========================================================
        // TEXT BOX
        // =========================================================

        private TextBox CreateTextBox(
            int y)
        {
            TextBox box = new TextBox
            {
                Location =
                    new Point(22, y),

                Size =
                    new Size(352, 31),

                Font =
                    new Font(
                        "Segoe UI",
                        9.5F),

                BorderStyle =
                    BorderStyle.FixedSingle
            };

            leftPanel.Controls.Add(box);

            return box;
        }


        // =========================================================
        // COMBO BOX
        // =========================================================

        private ComboBox CreateComboBox(
            int y)
        {
            ComboBox box = new ComboBox
            {
                Location =
                    new Point(22, y),

                Size =
                    new Size(352, 31),

                DropDownStyle =
                    ComboBoxStyle.DropDownList,

                Font =
                    new Font(
                        "Segoe UI",
                        9.5F),

                // IMPORTANT:
                // Standard gives the dropdown a visible outline
                // in Light Mode.
                FlatStyle =
                    FlatStyle.Standard
            };

            leftPanel.Controls.Add(box);

            return box;
        }


        // =========================================================
        // BUTTON
        // =========================================================

        private Button CreateButton(
            string text,
            int width,
            int height,
            Color color)
        {
            Button button = new Button
            {
                Text = text,

                Size =
                    new Size(
                        width,
                        height),

                BackColor = color,

                ForeColor =
                    Color.White,

                FlatStyle =
                    FlatStyle.Flat,

                Font =
                    new Font(
                        "Segoe UI",
                        8.5F,
                        FontStyle.Bold),

                Cursor =
                    Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;


            Color normal = color;

            Color hover =
                ControlPaint.Light(
                    color,
                    0.12f);

            Color down =
                ControlPaint.Dark(
                    color,
                    0.12f);


            button.MouseEnter +=
                (s, e) =>
                button.BackColor = hover;

            button.MouseLeave +=
                (s, e) =>
                button.BackColor = normal;

            button.MouseDown +=
                (s, e) =>
                button.BackColor = down;

            button.MouseUp +=
                (s, e) =>
                button.BackColor = hover;


            SetRoundedCorners(
                button,
                7);

            button.Resize +=
                (s, e) =>
                SetRoundedCorners(
                    button,
                    7);

            return button;
        }


        // =========================================================
        // STAT CARD
        // =========================================================

        private Label CreateStatCard(
            string title,
            int x,
            Color accentColor)
        {
            Panel card = new Panel
            {
                Location =
                    new Point(x, 72),

                Size =
                    new Size(210, 86),

                BackColor =
                    darkMode
                        ? Color.FromArgb(34, 37, 43)
                        : Color.White,

                Tag =
                    accentColor
            };

            SetRoundedCorners(
                card,
                10);

            card.Resize +=
                (s, e) =>
                SetRoundedCorners(
                    card,
                    10);

            rightPanel.Controls.Add(
                card);


            Panel accentBar = new Panel
            {
                Location =
                    new Point(0, 0),

                Size =
                    new Size(5, card.Height),

                BackColor =
                    accentColor
            };

            card.Controls.Add(
                accentBar);


            Label titleLabel = new Label
            {
                Text = title,

                ForeColor =
                    darkMode
                        ? mutedDark
                        : mutedLight,

                Font =
                    new Font(
                        "Segoe UI",
                        8F,
                        FontStyle.Bold),

                Location =
                    new Point(20, 14),

                AutoSize = true,

                Tag = "statTitle"
            };

            card.Controls.Add(
                titleLabel);


            Label valueLabel = new Label
            {
                Text = "0",

                ForeColor =
                    darkMode
                        ? Color.White
                        : textLight,

                Font =
                    new Font(
                        "Segoe UI",
                        23F,
                        FontStyle.Bold),

                Location =
                    new Point(19, 36),

                AutoSize = true,

                Tag = "statValue"
            };

            card.Controls.Add(
                valueLabel);

            return valueLabel;
        }


        // =========================================================
        // ROUNDED CORNERS
        // =========================================================

        private void SetRoundedCorners(
            Control control,
            int radius)
        {
            if (control.Width <= 0 ||
                control.Height <= 0)
                return;

            using (GraphicsPath path =
                new GraphicsPath())
            {
                int d = radius * 2;

                Rectangle r =
                    new Rectangle(
                        0,
                        0,
                        control.Width,
                        control.Height);

                path.AddArc(
                    r.X,
                    r.Y,
                    d,
                    d,
                    180,
                    90);

                path.AddArc(
                    r.Right - d,
                    r.Y,
                    d,
                    d,
                    270,
                    90);

                path.AddArc(
                    r.Right - d,
                    r.Bottom - d,
                    d,
                    d,
                    0,
                    90);

                path.AddArc(
                    r.X,
                    r.Bottom - d,
                    d,
                    d,
                    90,
                    90);

                path.CloseFigure();

                control.Region =
                    new Region(path);
            }
        }


        // =========================================================
        // ADD ASSET
        // =========================================================

        private void AddButton_Click(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                assetTagBox.Text))
            {
                ShowMessage(
                    "Please enter an Asset Tag.",
                    "Missing Information");

                return;
            }


            if (string.IsNullOrWhiteSpace(
                categoryBox.Text))
            {
                ShowMessage(
                    "Please select a Category.",
                    "Missing Information");

                return;
            }


            assetGrid.Rows.Add(
                assetTagBox.Text,
                categoryBox.Text,
                makeBox.Text,
                modelBox.Text,
                serialBox.Text,
                assignedUserBox.Text,
                statusBox.Text,
                warrantyPicker.Value.ToShortDateString(),
                commentsBox.Text);


            SaveAssets();

            UpdateDashboard();

            ClearFields();

            ShowMessage(
                "Asset added successfully.",
                "Success");
        }


        // =========================================================
        // UPDATE ASSET
        // =========================================================

        private void UpdateButton_Click(
            object sender,
            EventArgs e)
        {
            if (assetGrid.SelectedRows.Count == 0)
            {
                ShowMessage(
                    "Click an asset in the table first.",
                    "No Asset Selected");

                return;
            }


            DataGridViewRow row =
                assetGrid.SelectedRows[0];


            row.Cells["AssetTag"].Value =
                assetTagBox.Text;

            row.Cells["Category"].Value =
                categoryBox.Text;

            row.Cells["Make"].Value =
                makeBox.Text;

            row.Cells["Model"].Value =
                modelBox.Text;

            row.Cells["Serial"].Value =
                serialBox.Text;

            row.Cells["User"].Value =
                assignedUserBox.Text;

            row.Cells["Status"].Value =
                statusBox.Text;

            row.Cells["Warranty"].Value =
                warrantyPicker.Value.ToShortDateString();

            row.Cells["Comments"].Value =
                commentsBox.Text;


            SaveAssets();

            UpdateDashboard();

            assetGrid.Refresh();


            ShowMessage(
                "Asset updated successfully.",
                "Updated");
        }


        // =========================================================
        // DELETE ASSET
        // =========================================================

        private void DeleteButton_Click(
            object sender,
            EventArgs e)
        {
            if (assetGrid.SelectedRows.Count == 0)
            {
                ShowMessage(
                    "Select an asset to delete.",
                    "No Asset Selected");

                return;
            }


            DialogResult result =
                MessageBox.Show(
                    "Are you sure you want to delete this asset?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);


            if (result != DialogResult.Yes)
                return;


            assetGrid.Rows.Remove(
                assetGrid.SelectedRows[0]);


            SaveAssets();

            UpdateDashboard();

            ClearFields();


            ShowMessage(
                "Asset deleted successfully.",
                "Deleted");
        }


        // =========================================================
        // TABLE CLICK
        // =========================================================

        private void AssetGrid_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            LoadRowIntoForm(
                assetGrid.Rows[e.RowIndex]);
        }


        // =========================================================
        // LOAD TABLE ROW INTO FORM
        // =========================================================

        private void LoadRowIntoForm(
            DataGridViewRow row)
        {
            assetTagBox.Text =
                row.Cells["AssetTag"]
                    .Value?.ToString() ?? "";

            categoryBox.Text =
                row.Cells["Category"]
                    .Value?.ToString() ?? "";

            makeBox.Text =
                row.Cells["Make"]
                    .Value?.ToString() ?? "";

            modelBox.Text =
                row.Cells["Model"]
                    .Value?.ToString() ?? "";

            serialBox.Text =
                row.Cells["Serial"]
                    .Value?.ToString() ?? "";

            assignedUserBox.Text =
                row.Cells["User"]
                    .Value?.ToString() ?? "";

            statusBox.Text =
                row.Cells["Status"]
                    .Value?.ToString() ?? "";

            commentsBox.Text =
                row.Cells["Comments"]
                    .Value?.ToString() ?? "";


            if (DateTime.TryParse(
                row.Cells["Warranty"]
                    .Value?.ToString(),
                out DateTime date))
            {
                warrantyPicker.Value = date;
            }
        }


        // =========================================================
        // VIEW FORM
        // =========================================================

        private void ViewButton_Click(
            object sender,
            EventArgs e)
        {
            if (assetGrid.SelectedRows.Count == 0)
            {
                ShowMessage(
                    "Select an asset from the table first.",
                    "No Asset Selected");

                return;
            }

            LoadRowIntoForm(
                assetGrid.SelectedRows[0]);
        }


        // =========================================================
        // UPLOAD FORM
        // =========================================================

        private void UploadButton_Click(
            object sender,
            EventArgs e)
        {
            using (OpenFileDialog dialog =
                new OpenFileDialog())
            {
                dialog.Title =
                    "Upload Asset Form";

                dialog.Filter =
                    "Documents|*.pdf;*.doc;*.docx;*.xlsx;*.xls|All Files|*.*";


                if (dialog.ShowDialog() ==
                    DialogResult.OK)
                {
                    ShowMessage(
                        "Form selected:\n\n" +
                        Path.GetFileName(
                            dialog.FileName),

                        "Upload Form");
                }
            }
        }


        // =========================================================
        // CLEAR BUTTON
        // =========================================================

        private void ClearButton_Click(
            object sender,
            EventArgs e)
        {
            ClearFields();
        }


        // =========================================================
        // CLEAR FIELDS
        // =========================================================

        private void ClearFields()
        {
            assetTagBox.Clear();

            categoryBox.SelectedIndex = -1;

            makeBox.SelectedIndex = -1;

            modelBox.Clear();

            serialBox.Clear();

            assignedUserBox.Clear();

            statusBox.SelectedIndex = -1;

            warrantyPicker.Value =
                DateTime.Today;

            commentsBox.Clear();

            assetGrid.ClearSelection();
        }


        // =========================================================
        // SEARCH
        // =========================================================

        private void SearchButton_Click(
            object sender,
            EventArgs e)
        {
            string search =
                searchBox.Text
                    .Trim()
                    .ToLower();


            foreach (DataGridViewRow row
                in assetGrid.Rows)
            {
                if (row.IsNewRow)
                    continue;


                bool found = false;


                foreach (DataGridViewCell cell
                    in row.Cells)
                {
                    if (cell.Value != null &&
                        cell.Value
                            .ToString()
                            .ToLower()
                            .Contains(search))
                    {
                        found = true;
                        break;
                    }
                }


                row.Visible = found;
            }
        }


        // =========================================================
        // REFRESH
        // =========================================================

        private void RefreshButton_Click(
            object sender,
            EventArgs e)
        {
            searchBox.Clear();


            foreach (DataGridViewRow row
                in assetGrid.Rows)
            {
                if (!row.IsNewRow)
                    row.Visible = true;
            }


            UpdateDashboard();

            assetGrid.ClearSelection();
        }


        // =========================================================
        // UPDATE DASHBOARD
        // =========================================================

        private void UpdateDashboard()
        {
            int total = 0;
            int active = 0;
            int repair = 0;
            int disposed = 0;


            foreach (DataGridViewRow row
                in assetGrid.Rows)
            {
                if (row.IsNewRow)
                    continue;


                total++;


                string status =
                    row.Cells["Status"]
                        .Value?.ToString();


                if (status == "Active")
                    active++;


                if (status == "In Repair")
                    repair++;


                if (status == "Disposed")
                    disposed++;
            }


            // Dashboard totals
            totalAssetsLabel.Text =
                total.ToString();

            activeAssetsLabel.Text =
                active.ToString();

            repairAssetsLabel.Text =
                repair.ToString();

            disposedAssetsLabel.Text =
                disposed.ToString();


            // Separate total shown in header
            headerTotalAssetsLabel.Text =
                "TOTAL ASSETS: " + total;


            // Keep header counter positioned correctly
            PositionHeaderTotal();
        }


        // =========================================================
        // SAVE ASSETS
        // =========================================================

        private void SaveAssets()
        {
            try
            {
                List<AssetData> assets =
                    new List<AssetData>();


                foreach (DataGridViewRow row
                    in assetGrid.Rows)
                {
                    if (row.IsNewRow)
                        continue;


                    assets.Add(
                        new AssetData
                        {
                            AssetTag =
                                row.Cells["AssetTag"]
                                    .Value?.ToString() ?? "",

                            Category =
                                row.Cells["Category"]
                                    .Value?.ToString() ?? "",

                            Make =
                                row.Cells["Make"]
                                    .Value?.ToString() ?? "",

                            Model =
                                row.Cells["Model"]
                                    .Value?.ToString() ?? "",

                            Serial =
                                row.Cells["Serial"]
                                    .Value?.ToString() ?? "",

                            User =
                                row.Cells["User"]
                                    .Value?.ToString() ?? "",

                            Status =
                                row.Cells["Status"]
                                    .Value?.ToString() ?? "",

                            Warranty =
                                row.Cells["Warranty"]
                                    .Value?.ToString() ?? "",

                            Comments =
                                row.Cells["Comments"]
                                    .Value?.ToString() ?? ""
                        });
                }


                string json =
                    JsonSerializer.Serialize(
                        assets,
                        new JsonSerializerOptions
                        {
                            WriteIndented = true
                        });


                File.WriteAllText(
                    saveFile,
                    json);
            }
            catch (Exception ex)
            {
                ShowMessage(
                    "Could not save assets.\n\n" +
                    ex.Message,

                    "Save Error");
            }
        }


        // =========================================================
        // LOAD SAVED ASSETS
        // =========================================================

        private void LoadSavedAssets()
        {
            try
            {
                if (!File.Exists(saveFile))
                {
                    AddSampleData();

                    SaveAssets();

                    UpdateDashboard();

                    return;
                }


                string json =
                    File.ReadAllText(
                        saveFile);


                List<AssetData> assets =
                    JsonSerializer.Deserialize<
                        List<AssetData>>(json);


                if (assets == null)
                    return;


                assetGrid.Rows.Clear();


                foreach (AssetData asset
                    in assets)
                {
                    assetGrid.Rows.Add(
                        asset.AssetTag,
                        asset.Category,
                        asset.Make,
                        asset.Model,
                        asset.Serial,
                        asset.User,
                        asset.Status,
                        asset.Warranty,
                        asset.Comments);
                }


                UpdateDashboard();
            }
            catch (Exception ex)
            {
                ShowMessage(
                    "Could not load saved assets.\n\n" +
                    ex.Message,

                    "Load Error");
            }
        }


        // =========================================================
        // SAMPLE DATA
        // =========================================================

        private void AddSampleData()
        {
            assetGrid.Rows.Clear();


            assetGrid.Rows.Add(
                "AST-001",
                "Laptop",
                "Dell",
                "Latitude 5420",
                "DL5420001",
                "John Smith",
                "Active",
                "15/06/2027",
                "Main office laptop");


            assetGrid.Rows.Add(
                "AST-002",
                "Laptop",
                "HP",
                "EliteBook 840",
                "HP840002",
                "Sarah Jones",
                "Active",
                "20/08/2027",
                "Assigned to finance");


            assetGrid.Rows.Add(
                "AST-003",
                "Monitor",
                "Samsung",
                "Business Monitor",
                "SM00321",
                "IT Department",
                "In Repair",
                "10/03/2026",
                "Screen needs repair");


            assetGrid.Rows.Add(
                "AST-004",
                "Desktop",
                "Lenovo",
                "ThinkCentre",
                "LN00451",
                "Finance",
                "Disposed",
                "05/01/2025",
                "Old desktop disposed");
        }


        // =========================================================
        // THEME BUTTON
        // =========================================================

        private void ThemeButton_Click(
            object sender,
            EventArgs e)
        {
            darkMode = !darkMode;

            ApplyTheme();
        }


        // =========================================================
        // APPLY THEME
        // =========================================================

        private void ApplyTheme()
        {
            Color background =
                darkMode
                    ? backgroundDark
                    : backgroundLight;

            Color panel =
                darkMode
                    ? panelDark
                    : panelLight;

            Color text =
                darkMode
                    ? textDark
                    : textLight;

            Color muted =
                darkMode
                    ? mutedDark
                    : mutedLight;


            BackColor = background;


            // Header
            headerPanel.BackColor =
                darkMode
                    ? Color.FromArgb(58, 42, 92)
                    : Color.FromArgb(88, 63, 140);


            // Panels
            leftPanel.BackColor = panel;

            rightPanel.BackColor = panel;


            ApplyControlTheme(
                leftPanel,
                text,
                muted);


            ApplyControlTheme(
                rightPanel,
                text,
                muted);


            // =====================================================
            // HEADER TEXT
            // =====================================================

            foreach (Control control
                in headerPanel.Controls)
            {
                if (control is Label label)
                {
                    if (label.Tag?.ToString() == "muted")
                    {
                        label.ForeColor =
                            Color.FromArgb(
                                210,
                                210,
                                225);
                    }
                    else
                    {
                        label.ForeColor =
                            Color.White;
                    }
                }
            }


            // =====================================================
            // SEARCH BOX
            // =====================================================

            searchBox.BackColor =
                darkMode
                    ? Color.FromArgb(35, 38, 44)
                    : Color.White;

            searchBox.ForeColor = text;


            // =====================================================
            // GRID
            // =====================================================

            assetGrid.BackgroundColor =
                darkMode
                    ? Color.FromArgb(20, 22, 26)
                    : Color.White;


            assetGrid.GridColor =
                darkMode
                    ? Color.FromArgb(52, 55, 62)
                    : Color.FromArgb(225, 229, 236);


            assetGrid.ColumnHeadersDefaultCellStyle
                .BackColor =
                    darkMode
                        ? Color.FromArgb(38, 42, 49)
                        : Color.FromArgb(239, 242, 247);


            assetGrid.ColumnHeadersDefaultCellStyle
                .ForeColor = text;


            assetGrid.ColumnHeadersDefaultCellStyle
                .Font =
                    new Font(
                        "Segoe UI",
                        8.5F,
                        FontStyle.Bold);


            assetGrid.DefaultCellStyle
                .BackColor =
                    darkMode
                        ? Color.FromArgb(27, 29, 34)
                        : Color.White;


            assetGrid.DefaultCellStyle
                .ForeColor = text;


            assetGrid.DefaultCellStyle
                .SelectionBackColor =
                    Color.FromArgb(
                        65,
                        105,
                        195);


            assetGrid.DefaultCellStyle
                .SelectionForeColor =
                    Color.White;


            assetGrid.DefaultCellStyle
                .Font =
                    new Font(
                        "Segoe UI",
                        8.5F);


            assetGrid.DefaultCellStyle
                .Padding =
                    new Padding(
                        6,
                        0,
                        6,
                        0);


            assetGrid.AlternatingRowsDefaultCellStyle
                .BackColor =
                    darkMode
                        ? Color.FromArgb(32, 35, 40)
                        : Color.FromArgb(248, 250, 253);


            assetGrid.AlternatingRowsDefaultCellStyle
                .ForeColor = text;


            // =====================================================
            // THEME BUTTON
            // =====================================================

            themeButton.Text =
                darkMode
                    ? "☼  Light Mode"
                    : "☾  Dark Mode";


            themeButton.BackColor =
                darkMode
                    ? Color.FromArgb(45, 48, 55)
                    : Color.FromArgb(60, 75, 100);


            // Update stat cards
            UpdateStatCardTheme(
                text,
                muted);


            // Keep header total positioned correctly
            PositionHeaderTotal();
        }


        // =========================================================
        // STAT CARD THEME
        // =========================================================

        private void UpdateStatCardTheme(
            Color text,
            Color muted)
        {
            foreach (Control control
                in rightPanel.Controls)
            {
                if (control is Panel card &&
                    card.Tag is Color)
                {
                    card.BackColor =
                        darkMode
                            ? Color.FromArgb(34, 37, 43)
                            : Color.White;


                    foreach (Control child
                        in card.Controls)
                    {
                        if (child is Label label)
                        {
                            if (label.Tag?.ToString()
                                == "statTitle")
                            {
                                label.ForeColor =
                                    muted;
                            }


                            if (label.Tag?.ToString()
                                == "statValue")
                            {
                                label.ForeColor =
                                    text;
                            }
                        }
                    }
                }
            }
        }


        // =========================================================
        // CONTROL THEME
        // =========================================================

        private void ApplyControlTheme(
            Control parent,
            Color text,
            Color muted)
        {
            foreach (Control control
                in parent.Controls)
            {
                if (control is Label label)
                {
                    if (label.Tag?.ToString()
                        == "muted")
                    {
                        label.ForeColor =
                            muted;
                    }
                    else if (
                        label.Tag?.ToString()
                        == "section")
                    {
                        label.ForeColor =
                            accent;
                    }
                    else
                    {
                        label.ForeColor =
                            text;
                    }
                }


                if (control is TextBox textBox)
                {
                    textBox.BackColor =
                        darkMode
                            ? Color.FromArgb(35, 38, 44)
                            : Color.White;

                    textBox.ForeColor = text;

                    textBox.BorderStyle =
                        BorderStyle.FixedSingle;
                }


                if (control is ComboBox comboBox)
                {
                    comboBox.BackColor =
                        darkMode
                            ? Color.FromArgb(35, 38, 44)
                            : Color.White;

                    comboBox.ForeColor = text;

                    // IMPORTANT:
                    // Keeps the outline visible.
                    comboBox.FlatStyle =
                        FlatStyle.Standard;
                }


                if (control is DateTimePicker picker)
                {
                    picker.BackColor =
                        darkMode
                            ? Color.FromArgb(35, 38, 44)
                            : Color.White;

                    picker.ForeColor = text;
                }


                if (control.HasChildren)
                {
                    ApplyControlTheme(
                        control,
                        text,
                        muted);
                }
            }
        }


        // =========================================================
        // STATUS COLOURS
        // =========================================================

        private void AssetGrid_CellFormatting(
            object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 ||
                e.ColumnIndex < 0 ||
                assetGrid.Columns[
                    e.ColumnIndex].Name != "Status")
                return;


            string status =
                e.Value?.ToString() ?? "";


            if (status == "Active")
            {
                e.CellStyle.ForeColor =
                    Color.FromArgb(
                        0,
                        170,
                        125);

                e.CellStyle.Font =
                    new Font(
                        "Segoe UI",
                        8.5F,
                        FontStyle.Bold);
            }
            else if (status == "In Repair")
            {
                e.CellStyle.ForeColor =
                    Color.FromArgb(
                        225,
                        150,
                        50);

                e.CellStyle.Font =
                    new Font(
                        "Segoe UI",
                        8.5F,
                        FontStyle.Bold);
            }
            else if (status == "Disposed")
            {
                e.CellStyle.ForeColor =
                    Color.FromArgb(
                        215,
                        70,
                        75);

                e.CellStyle.Font =
                    new Font(
                        "Segoe UI",
                        8.5F,
                        FontStyle.Bold);
            }
        }


        // =========================================================
        // RESIZE
        // =========================================================

        private void Form1_Resize(
            object sender,
            EventArgs e)
        {
            AdjustLayout();
        }


        // =========================================================
        // ADJUST LAYOUT
        // =========================================================

        private void AdjustLayout()
        {
            if (headerPanel == null ||
                leftPanel == null ||
                rightPanel == null)
                return;


            headerPanel.Width =
                ClientSize.Width;


            if (themeButton != null)
            {
                themeButton.Left =
                    headerPanel.ClientSize.Width -
                    themeButton.Width -
                    25;
            }


            PositionHeaderTotal();


            leftPanel.Left = 20;

            leftPanel.Top = 112;

            leftPanel.Height =
                ClientSize.Height - 132;


            rightPanel.Left = 440;

            rightPanel.Top = 112;

            rightPanel.Width =
                ClientSize.Width - 460;

            rightPanel.Height =
                ClientSize.Height - 132;


            if (searchBox != null &&
                searchButton != null &&
                refreshButton != null)
            {
                int rightPadding = 20;

                int gap = 10;


                refreshButton.Left =
                    rightPanel.ClientSize.Width -
                    rightPadding -
                    refreshButton.Width;


                refreshButton.Top = 178;


                searchButton.Left =
                    refreshButton.Left -
                    gap -
                    searchButton.Width;


                searchButton.Top = 178;


                searchBox.Left = 20;

                searchBox.Top = 178;


                searchBox.Width =
                    searchButton.Left -
                    searchBox.Left -
                    gap;
            }


            if (assetGrid != null)
            {
                assetGrid.Left = 20;

                assetGrid.Top = 260;

                assetGrid.Width =
                    rightPanel.ClientSize.Width - 40;


                assetGrid.Height =
                    rightPanel.ClientSize.Height -
                    assetGrid.Top -
                    20;
            }


            PositionStatCards();
        }


        // =========================================================
        // POSITION HEADER TOTAL
        // =========================================================

        private void PositionHeaderTotal()
        {
            if (headerPanel == null ||
                headerTotalAssetsLabel == null ||
                themeButton == null)
                return;


            headerTotalAssetsLabel.Left =
                headerPanel.ClientSize.Width -
                themeButton.Width -
                headerTotalAssetsLabel.Width -
                55;


            headerTotalAssetsLabel.Top = 35;
        }


        // =========================================================
        // POSITION STAT CARDS
        // =========================================================

        private void PositionStatCards()
        {
            if (rightPanel == null)
                return;


            List<Panel> cards =
                new List<Panel>();


            foreach (Control control
                in rightPanel.Controls)
            {
                if (control is Panel panel &&
                    panel.Tag is Color)
                {
                    cards.Add(panel);
                }
            }


            if (cards.Count < 4)
                return;


            int leftMargin = 20;

            int rightMargin = 20;

            int gap = 10;


            int availableWidth =
                rightPanel.ClientSize.Width -
                leftMargin -
                rightMargin;


            int cardWidth =
                (availableWidth -
                 gap * 3) / 4;


            if (cardWidth < 140)
                cardWidth = 140;


            for (int i = 0; i < 4; i++)
            {
                cards[i].Width =
                    cardWidth;

                cards[i].Height = 86;

                cards[i].Top = 72;

                cards[i].Left =
                    leftMargin +
                    i * (cardWidth + gap);
            }
        }


        // =========================================================
        // MESSAGE
        // =========================================================

        private void ShowMessage(
            string message,
            string title)
        {
            MessageBox.Show(
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}