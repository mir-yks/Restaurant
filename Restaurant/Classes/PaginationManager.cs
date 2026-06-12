using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Restaurant
{
    public class PaginationManager
    {
        private readonly DataGridView dataGridView;
        private readonly FlowLayoutPanel paginationPanel;
        private readonly Label labelPageInfo;
        private readonly Label labelTotal;

        private DataView currentView;

        public int CurrentPage { get; private set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; private set; } = 1;

        public PaginationManager(
            DataGridView dgv,
            FlowLayoutPanel panel,
            Label pageInfo,
            Label total)
        {
            dataGridView = dgv;
            paginationPanel = panel;
            labelPageInfo = pageInfo;
            labelTotal = total;
        }

        public void RecalculateLayoutOnly()
        {
            StretchRows();
        }

        public void SetData(DataView view)
        {
            currentView = view;
            CurrentPage = 1;
            ShowPage();
        }

        public void Refresh()
        {
            ShowPage();
        }

        public void GoToFirst()
        {
            CurrentPage = 1;
            ShowPage();
        }

        private void ShowPage()
        {
            if (currentView == null)
                return;

            int totalRows = currentView.Count;

            TotalPages = (int)Math.Ceiling((double)totalRows / PageSize);

            if (TotalPages == 0)
                TotalPages = 1;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            DataTable sourceTable = currentView.ToTable();

            DataTable pageTable = sourceTable.Clone();

            int start = (CurrentPage - 1) * PageSize;
            int end = Math.Min(start + PageSize, totalRows);

            for (int i = start; i < end; i++)
            {
                pageTable.ImportRow(sourceTable.Rows[i]);
            }

            dataGridView.DataSource = pageTable;

            dataGridView.DataBindingComplete -= DataGridView_DataBindingComplete;
            dataGridView.DataBindingComplete += DataGridView_DataBindingComplete;

            StretchRows();

            labelTotal.Text = $"Всего: {totalRows}";
            labelPageInfo.Text = $"Страница {CurrentPage} из {TotalPages}";

            CreateButtons();
        }

        private void DataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            StretchRows();
        }

        private void StretchRows()
        {
            if (dataGridView.Rows.Count == 0)
                return;

            int rowHeight =
                (dataGridView.ClientSize.Height
                - dataGridView.ColumnHeadersHeight)
                / PageSize;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                row.Height = rowHeight;
            }
        }

        private void CreateButtons()
        {
            paginationPanel.Controls.Clear();

            AddButton(">>", TotalPages, CurrentPage < TotalPages);
            AddButton(">", CurrentPage + 1, CurrentPage < TotalPages);

            int start = Math.Max(1, CurrentPage - 2);
            int end = Math.Min(TotalPages, CurrentPage + 2);

            for (int i = end; i >= start; i--)
            {
                Button btn = new Button();

                btn.Text = i.ToString();

                btn.Width = 40;
                btn.Height = 30;

                btn.Font = Fonts.MontserratAlternatesBold(12f);

                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;

                btn.BackColor = Color.FromArgb(57, 60, 70);
                btn.ForeColor = Color.White;

                if (i == CurrentPage)
                {
                    btn.BackColor = Color.FromArgb(90, 95, 110);
                }

                int page = i;

                btn.Click += (s, e) =>
                {
                    CurrentPage = page;
                    ShowPage();
                };

                paginationPanel.Controls.Add(btn);
            }

            AddButton("<", CurrentPage - 1, CurrentPage > 1);
            AddButton("<<", 1, CurrentPage > 1);
        }

        private void AddButton(string text, int page, bool enabled)
        {
            Button btn = new Button();

            btn.Text = text;

            btn.Width = 45;
            btn.Height = 30;

            btn.Font = Fonts.MontserratAlternatesBold(12f);

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            btn.BackColor = Color.FromArgb(57, 60, 70);
            btn.ForeColor = Color.White;

            btn.Enabled = enabled;

            if (!enabled)
            {
                btn.BackColor = Color.FromArgb(40, 40, 40);
                btn.ForeColor = Color.Gray;
            }

            btn.Click += (s, e) =>
            {
                CurrentPage = page;
                ShowPage();
            };

            paginationPanel.Controls.Add(btn);
        }
    }
}