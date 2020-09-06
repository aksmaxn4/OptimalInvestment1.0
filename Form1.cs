using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptimalInvestment2._1
{
    public partial class Form1 : Form
    {

        double[,] start_mas = null;
        double[,,] mas = null;

        int column_count = 3;
        int rows_count = 4;
        int x = 0;
        public Form1()
        {
            InitializeComponent();
            dataGridView1.Rows.Add(4);

            for (int i = 0; (i <= (dataGridView1.Rows.Count - 1)); i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = string.Format((i).ToString(), "0");
            }
            InitializeMas();
            button1.Enabled = false;


        }

        public bool GetStarted()
        {
            bool ok = true;
            for (int i = 0; i < start_mas.GetLength(0); i++)
            {
                for (int j = 0; j < start_mas.GetLength(1); j++)
                {
                    try
                    {
                        start_mas[i, j] = Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value);
                        if (start_mas[i, j] < 0) throw new Exception();
                    }
                    catch
                    {
                        MessageBox.Show("Проверьте введенные в таблице данные\nДолжны быть только положительные целые и вещественные числа");

                        ok = false;
                        break;

                    }
                }
                if (ok == false) break;
            }

            return ok;

        }

        public void FirstNSecondMaxValue(int A = 0)
        {
            if (A <= dataGridView1.Rows.Count - 1)
            {
                double sum = 0; double curr = 0;
                for (int x = 0; x <= A; x++)
                {
                    curr = start_mas[x, 0] + start_mas[A - x, 1];
                    if (curr >= sum)
                    {


                        sum = curr;
                        mas[A, 0, 0] = curr;
                        mas[A, 0, 1] = x;
                        mas[A, 0, 2] = A - x;

                    }
                }
                FirstNSecondMaxValue(++A);
            }
            else if (dataGridView1.ColumnCount > 2) NextGenMaxValue(2, 0);
            else return;



        }
        public void NextGenMaxValue(int columnStart, int columnMas, int A = 0)
        {
            if (A <= dataGridView1.Rows.Count - 1)
            {
                double sum = 0; double curr = 0;
                for (int x = 0; x <= A; x++)
                {
                    curr = mas[x, columnMas, 0] + start_mas[A - x, columnStart];
                    if (curr >= sum)
                    {
                        sum = curr;
                        mas[A, columnMas + 1, 0] = curr;
                        for (int k = 1; k < mas.GetLength(2); k++)
                        {
                            mas[A, columnMas + 1, k] = mas[x, columnMas, k];
                        }
                        mas[A, columnMas + 1, columnStart + 1] = A - x;

                    }
                }
                NextGenMaxValue(columnStart, columnMas, ++A);
            }
            else if (columnStart + 1 < start_mas.GetLength(1)) NextGenMaxValue(++columnStart, ++columnMas);
            else return;


        }

        private void Button1_Click(object sender, EventArgs e)
        {

            x = Convert.ToInt32(textBox1.Text);
            if (x > dataGridView1.Rows.Count - 1)
            {
                MessageBox.Show("Вложения не могут быть больше максимального значения левого столбца");
                return;
            }
            else
            {
                if (GetStarted() == true)
                {

                    ClearGrid();
                    Zero();
                    FirstNSecondMaxValue();
                    Finish();
                    pictureBox1.Visible = true;
                }
            }

        }

        void AddColumn()
        {
            column_count++;
            dataGridView1.Width += 40;
            dataGridView1.Columns.Add("Column " + column_count.ToString(), column_count.ToString());
            dataGridView1.Columns[column_count - 1].Width = 40;
            label2.Location = new Point(dataGridView1.Width / 2, 13);
            InitializeMas();


        }
        void DeleteColumn()
        {
            column_count--;
            dataGridView1.Width -= 40;
            dataGridView1.Columns.RemoveAt(column_count);
            label2.Location = new Point(dataGridView1.Width / 2, 13);
            InitializeMas();
        }
        void AddRows()
        {
            int x = label4.Location.X; int y = label4.Location.Y;
            dataGridView1.Rows.Add();
            dataGridView1.Rows[rows_count].HeaderCell.Value = rows_count.ToString();
            dataGridView1.Height += 22;
            label4.Location = new Point(x, dataGridView1.Height / 2);
            rows_count++;
            InitializeMas();
        }
        void DeleteRows()
        {
            int x = label4.Location.X; int y = label4.Location.Y;
            dataGridView1.Rows.RemoveAt(rows_count - 1);
            dataGridView1.Height -= 22;
            label4.Location = new Point(x, dataGridView1.Height / 2);
            rows_count--;
            InitializeMas();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;
            AddColumn();
            if (column_count == 10) button2.Enabled = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            DeleteColumn();
            if (column_count == 2) button3.Enabled = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            button5.Enabled = true;
            AddRows();
            if (rows_count == 10) button4.Enabled = false;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            button4.Enabled = true;
            DeleteRows();
            if (rows_count == 2) button5.Enabled = false;

        }

        private void InitializeMas()
        {
            start_mas = new double[dataGridView1.Rows.Count, dataGridView1.ColumnCount];
            mas = new double[dataGridView1.Rows.Count, dataGridView1.Columns.Count - 1, dataGridView1.Columns.Count + 1];
        }
        private void Finish()
        {
            int i = 0;
            label3.Text = "Максимальная прибыль:" + "\n" + mas[x, mas.GetLength(1) - 1, 0];
            label3.Visible = true;
            for (int k = 1; k < mas.GetLength(2); k++)
            {
                dataGridView1[i, Convert.ToInt32(mas[x, mas.GetLength(1) - 1, k])].Style.BackColor = System.Drawing.Color.Red;
                i++;
            }
        }
        private void ClearGrid()
        {
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    dataGridView1[i, j].Style.BackColor = System.Drawing.Color.White;

        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                e.Handled = true;
        }

        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox1.Text.Length > 0) button1.Enabled = true;
            else button1.Enabled = false;
        }
        private void Zero()
        {
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    if (dataGridView1[i, j].Value == null) dataGridView1[i, j].Value = "0";


        }
    }

}
