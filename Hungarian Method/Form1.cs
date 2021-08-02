using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hungarian_Method
{
    public partial class Form1 : Form
    {
        List<TextBox> performersTextBoxes = new List<TextBox>();
        List<TextBox> jobsTextBoxes = new List<TextBox>();
        List<List<TextBox>> efficiencyTextBoxes = new List<List<TextBox>>();
        TextBox currencyTextBox = new TextBox();
        public Form1()
        {
            InitializeComponent();
        }
        // Фунція для додавання виконавця та нового рядку.
        // У поля, що додаються функцією, користувач вписує дані задачі,
        // та ПІБ або назву виконавця.
        private void AddPerformer()
        {
            int performers = performersTextBoxes.Where(x => x.Visible).Count();
            int jobs = jobsTextBoxes.Where(x => x.Visible).Count();
            if (performers >= 10)
            {
                return;
            }

            performersTextBoxes[performers].Visible = true;
            for (int i = 0; i < jobs; i++)
            {
                efficiencyTextBoxes[performers][i].Visible = true;
            }

        }
        // Фунція для видалення рядку та виконавця
        // з програмного інтерфейса
        private void RemovePerformer()
        {
            int performers = performersTextBoxes.Where(x => x.Visible).Count();
            int jobs = jobsTextBoxes.Where(x => x.Visible).Count();
            if (performers <= 3)
            {
                return;
            }

            performersTextBoxes[performers - 1].Visible = false;
            performersTextBoxes[performers - 1].Text = $"Виконавець {performers}";
            for (int i = 0; i < jobs; i++)
            {
                efficiencyTextBoxes[performers - 1][i].Visible = false;
                efficiencyTextBoxes[performers - 1][i].BackColor = Color.White;
                efficiencyTextBoxes[performers - 1][i].Text = string.Empty;
            }

        }
        // Фунція для додавання вида роботи та нового стовпця.
        // У поля, що додаються функцією, користувач вписує дані задачі,
        // та назву роботи.
        private void AddJob()
        {
            int jobs = jobsTextBoxes.Where(x => x.Visible).Count();
            int performers = performersTextBoxes.Where(x => x.Visible).Count();
            if (jobs >= 10)
            {
                return;
            }

            jobsTextBoxes[jobs].Visible = true;
            for (int i = 0; i < performers; i++)
            {
                efficiencyTextBoxes[i][jobs].Visible = true;
            }

        }
        // Фунція для видалення стовпця та роботи
        // з програмного інтерфейса.
        private void RemoveJob()
        {
            int jobs = jobsTextBoxes.Where(x => x.Visible).Count();
            int performers = performersTextBoxes.Where(x => x.Visible).Count();
            if (jobs <= 3)
            {
                return;
            }

            jobsTextBoxes[jobs - 1].Visible = false;
            jobsTextBoxes[jobs - 1].Text = $"Робота { jobs}";
            for (int i = 0; i < performers; i++)
            {
                efficiencyTextBoxes[i][jobs - 1].Visible = false;
                efficiencyTextBoxes[i][jobs - 1].BackColor = Color.White;
                efficiencyTextBoxes[i][jobs - 1].Text = string.Empty;
            }

        }
        // Функція що додає функціональні кнопки, використовуючі які,
        // користувач може додавати виконавців, види роботи.
        private Button CreateGridControls(int top, int left, string text, EventHandler AddButton_Click, EventHandler RemoveButton_Click)
        {
            Label label = new Label() { Top = top, Left = left, Text = text };
            label.AutoSize = true;
            Controls.Add(label);

            Button addButton = new Button() { Top = label.Top + label.Height + 5, Left = label.Left, Text = "+", Width = 25 };
            addButton.Click += AddButton_Click;
            Controls.Add(addButton);

            Button removeButton = new Button() { Top = addButton.Top, Left = addButton.Left + addButton.Width + 5, Text = "-", Width = addButton.Width };
            removeButton.Click += RemoveButton_Click;
            Controls.Add(removeButton);

            return removeButton;
        }
        // Функція, що перевіряє правильність, введених у поля, даних.
        // Рядки для чисел приймають лише цифри(0-9) та кому й крапку.
        // Рядки не можуть бути порожніми.
        // Якщо поле не відповідає критеріям воно виділяється червоним
        // кольором, та подальша робота програми не відбувається
        private bool ValidateFields()
        {
            bool valid = true;

            int performers = performersTextBoxes.Where(x => x.Visible).Count();
            int jobs = jobsTextBoxes.Where(x => x.Visible).Count();

            // Перевірити поля для введеня виконавців
            for (int i = 0; i < performers; i++)
            {
                if (performersTextBoxes[i].Text == "")
                {
                    performersTextBoxes[i].BackColor = Color.Red;
                    valid = false;
                }
                else
                {
                    performersTextBoxes[i].BackColor = Color.White;
                }
            }

            // Перевірити поля для введеня робіт
            for (int i = 0; i < jobs; i++)
            {
                if (jobsTextBoxes[i].Text == "")
                {
                    jobsTextBoxes[i].BackColor = Color.Red;
                    valid = false;
                }
                else
                {
                    jobsTextBoxes[i].BackColor = Color.White;
                }
            }

            // Перевірити поля з розцінками
            for (int i = 0; i < performers; i++)
            {
                for (int j = 0; j < jobs; j++)
                {
                    if (efficiencyTextBoxes[i][j].Text == "")
                    {
                        efficiencyTextBoxes[i][j].BackColor = Color.Red;
                        valid = false;
                    }
                    else
                    {
                        efficiencyTextBoxes[i][j].BackColor = Color.White;
                        efficiencyTextBoxes[i][j].Text = efficiencyTextBoxes[i][j].Text.Replace('.', ',');
                    }
                }
            }

            return valid;
        }
        // Функція, що за наданими користувачем даними, вирішує задачу
        // угорським методом.
        private void HungarianAlgorithm()
        {
            int performers = performersTextBoxes.Where(x => x.Visible).Count();
            int jobs = jobsTextBoxes.Where(x => x.Visible).Count();

            // Формування матриці ефективності (цін)
            List<List<float>> startMatrix = new List<List<float>>();
            List<List<float>> efficiency = new List<List<float>>();
            for (int i = 0; i < performers; i++)
            {
                efficiency.Add(new List<float>());
                startMatrix.Add(new List<float>());
                for (int j = 0; j < jobs; j++)
                {
                    efficiency[i].Add(float.Parse(efficiencyTextBoxes[i][j].Text));
                    startMatrix[i].Add(float.Parse(efficiencyTextBoxes[i][j].Text));
                }
            }

            // Віднімання мінімального елементу стовпця від
            // кожного елемента стовпця
            for (int i = 0; i < efficiency[0].Count; i++)
            {
                float minimalOnTheColumn = efficiency.Select(x => x[i]).Min();
                for (int j = 0; j < efficiency.Count; j++)
                {
                    efficiency[j][i] -= minimalOnTheColumn;
                }
            }

            // Віднімання мінімального елементу рядку від
            // кожного елемента рядку
            for (int i = 0; i < efficiency.Count; i++)
            {
                float minimalOnTheRow = efficiency[i].Min();
                for (int j = 0; j < efficiency[0].Count; j++)
                {
                    efficiency[i][j] -= minimalOnTheRow;
                }
            }

            List<List<char>> marked = new List<List<char>>();
            List<bool> haveAsteriks = new List<bool>();
            List<bool> markedRows = new List<bool>();

            bool first = true;
            while (true)
            {
                // Зробити помітки елементів матриці ефективності
                // '*' якщо це оптимальний нуль, '°' якщо неоптимальний нуль,
                // виділити червоним якщо в стовпці знаходиться оптимальний нуль
                if (first)
                {
                    first = false;
                    for (int i = 0; i < efficiency.Count; i++)
                    {
                        marked.Add(new List<char>());
                        for (int j = 0; j < efficiency[0].Count; j++)
                        {
                            marked[i].Add('n');
                        }
                    }
                    // Позначенння рядків, в яких є оптимальний нуль
                    for (int i = 0; i < efficiency.Count; i++)
                    {
                        haveAsteriks.Add(false);
                    }
                    // Виділення червоним елементів, якщо в стовпці знаходиться
                    // оптимальний нуль
                    for (int j = 0; j < efficiency[0].Count; j++)
                    {
                        for (int i = 0; i < efficiency.Count; i++)
                        {
                            if (efficiency[i][j] == 0.0f && !haveAsteriks[i])
                            {
                                marked[i][j] = 'a';
                                for (int k = 0; k < efficiency.Count; k++)
                                {
                                    if (k != i)
                                    {
                                        marked[k][j] = 'r';
                                    }
                                }
                                haveAsteriks[i] = true;
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < efficiency.Count; i++)
                    {
                        markedRows.Add(false);
                    }
                }
                else
                {
                    for (int k = 0; k < markedRows.Count; k++)
                    {
                        markedRows[k] = false;
                    }

                    for (int k = 0; k < haveAsteriks.Count; k++)
                    {
                        haveAsteriks[k] = false;
                    }

                    for (int j = 0; j < marked[0].Count; j++)
                    {
                        for (int i = 0; i < marked.Count; i++)
                        {
                            if (marked[i][j] == 'a')
                            {
                                for (int k = 0; k < marked.Count; k++)
                                {
                                    if (k != i)
                                    {
                                        marked[k][j] = 'r';
                                    }
                                }
                                haveAsteriks[i] = true;
                                break;
                            }
                        }
                    }
                }
                // Перевірка поточного рішення на оптимальність
                // якщо є оптимальним, інформація виводиться
                // використовуючи спливаюче вікно
                if (haveAsteriks.Where(x => x).Count() == efficiency.Count)
                {
                    float sum = 0;
                    for (int i = 0; i < efficiency.Count; i++)
                    {
                        for (int j = 0; j < efficiency[0].Count; j++)
                        {
                            if (marked[i][j] == 'a')
                            {
                                sum += startMatrix[i][j];
                            }
                        }
                    }

                    string caption = "Знайдено оптимальний план призначення.";
                    string message = $"Загальні витрати за цим планом становлять: {sum} {currencyTextBox.Text}\n";
                    message += "\nПлан:\n";
                    for (int j = 0; j < marked[0].Count; j++)
                    {
                        for (int i = 0; i < marked.Count; i++)
                        {
                            if (marked[i][j] == 'a')
                            {
                                message += $"На {jobsTextBoxes[j].Text} призначити {performersTextBoxes[i].Text}.\n";
                                break;
                            }
                        }
                    }

                    MessageBox.Show(message, caption);
                    return;
                }
                // Модифікація матриці ефективності
                while (true)
                {
                    bool restart = false;
                    bool newIteration = false;
                    bool hadZero = false;
                    for (int j = 0; j < efficiency[0].Count; j++)
                    {
                        for (int i = 0; i < efficiency.Count; i++)
                        {
                            // Якщо існує невиділений нуль, обрати один з варіантів
                            if (efficiency[i][j] == 0.0f && marked[i][j] == 'n')
                            {
                                hadZero = true;
                                // Якщо невиділений нуль знаходиться в рядку з
                                // оптимальним нулем, виділяємо рядок, що містить цей нуль, 
                                // знімаємо виділення з стовпця, на перетині якого з тільки
                                // що виділеної рядком знаходиться нуль із зіркою.
                                if (haveAsteriks[i])
                                {
                                    marked[i][j] = 'z';
                                    for (int k = 0; k < efficiency[0].Count; k++)
                                    {
                                        if (marked[i][k] != 'a' && k != j)
                                        {
                                            marked[i][k] = 'r';
                                        }
                                        else if (marked[i][k] == 'a')
                                        {
                                            for (int l = 0; l < efficiency.Count; l++)
                                            {
                                                if (l != i && !markedRows[l])
                                                {
                                                    marked[l][k] = 'n';
                                                }
                                            }
                                        }
                                    }
                                    markedRows[i] = true;
                                    restart = true;
                                    break;
                                }
                                // Рядок, що містить невиділений нуль 
                                // не містить оптимальний нуль 
                                else
                                {
                                    // Виходячи з нуля зі знаком °, в рядку якого немає нуля із зіркою
                                    // будуємо наступний ланцюжок елементів матриці C: 
                                    // Вихідний 0° - 0* (що лежить в одному стовпці (якщо існує)) 
                                    // - 0° (лежить в одному рядку з попереднім 0*) і т.д.
                                    marked[i][j] = 'z';
                                    List<List<int>> path = new List<List<int>>();
                                    path.Add(new List<int>() { i, j });
                                    char searchChar = 'a';
                                    // Побудова ланцюжка елементів матриці
                                    while (true)
                                    {
                                        int prevPointRow = path.Last()[0];
                                        int prevPointColumn = path.Last()[1];

                                        bool found = false;
                                        if (searchChar == 'a')
                                        {
                                            for (int k = 0; k < efficiency.Count; k++)
                                            {
                                                if (marked[k][prevPointColumn] == searchChar)
                                                {
                                                    path.Add(new List<int>() { k, prevPointColumn });
                                                    searchChar = 'z';
                                                    found = true;
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            for (int k = 0; k < efficiency[0].Count; k++)
                                            {
                                                if (marked[prevPointRow][k] == searchChar)
                                                {
                                                    path.Add(new List<int>() { prevPointRow, k });
                                                    searchChar = 'a';
                                                    found = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (!found)
                                        {
                                            break;
                                        }
                                    }
                                    // Там, де 0 °, замінюємо на 0 *, а на парних
                                    // позиціях знищуємо знак * над нулями. 
                                    for (int k = 0; k < path.Count; k++)
                                    {
                                        if (k % 2 != 0)
                                        {
                                            marked[path[k][0]][path[k][1]] = 'z';
                                        }
                                        else
                                        {
                                            marked[path[k][0]][path[k][1]] = 'a';
                                        }
                                    }
                                    // Далі знищуємо все ° над нулями і 
                                    // знімаємо виділення з стовпців і рядків.
                                    for (int k = 0; k < marked.Count; k++)
                                    {
                                        for (int l = 0; l < marked[0].Count; l++)
                                        {
                                            if (marked[k][l] == 'z' || marked[k][l] == 'r')
                                            {
                                                marked[k][l] = 'n';
                                            }
                                        }
                                    }

                                    restart = true;
                                    newIteration = true;
                                    break;
                                }
                            }
                        }
                        if (restart)
                        {
                            break;
                        }
                    }
                    if (newIteration)
                    {
                        break;
                    }
                    // Якщо невиділених нулів немає, переходимо до
                    // модифікації матриці ефективності
                    if (!hadZero)
                    {
                        // Серед невиділених елементів знаходимо мінімальний q> 0.
                        // Далі величину q віднімаємо з усіх невиділених елементів 
                        // матриці C і додаємо до всіх елементів, що знаходяться 
                        // на перетині виділених рядків і стовпців.
                        List<float> unmarked = new List<float>();
                        for (int i = 0; i < marked.Count; i++)
                        {
                            for (int j = 0; j < marked[0].Count; j++)
                            {
                                if (marked[i][j] == 'n')
                                {
                                    unmarked.Add(efficiency[i][j]);
                                }
                            }
                        }
                        // Знаходження мінімального q>0 та віднімання
                        // від невиділених, та додавання до елементів
                        // на перетині виділених рядків і стовпців
                        float minNumber = unmarked.Min();
                        for (int j = 0; j < marked[0].Count; j++)
                        {
                            bool isFullColumn = true;
                            for (int i = 0; i < marked.Count; i++)
                            {
                                if (marked[i][j] == 'n')
                                {
                                    efficiency[i][j] -= minNumber;
                                    isFullColumn = false;
                                }
                            }
                            if (isFullColumn)
                            {
                                for (int i = 0; i < marked.Count; i++)
                                {
                                    if (markedRows[i])
                                    {
                                        efficiency[i][j] += minNumber;
                                    }
                                }
                            }
                        }
                    }
                }

                int a = 0;
            }

        }
        // Запуск програмного додатку, та формування графічного інтерфейсу додатку
        // заповнення додатку початковими даними, полями, рядками, стовпцями, кнопками.
        private void Form1_Load(object sender, EventArgs e)
        {
            // Створення поля для введення даних
            // про валюту, для якої будуть виконуватись
            // розрахунки
            Label currencyLabel = new Label() { Left = 20, Top = 20, Text = "Валюта:", AutoSize = false, TextAlign = ContentAlignment.MiddleLeft, Width = 50 };
            Controls.Add(currencyLabel);

            TextBox currencyTextBox = new TextBox() { Left = currencyLabel.Left + currencyLabel.Width + 5, Top = 20, Width = 45 };
            this.currencyTextBox = currencyTextBox;
            Controls.Add(currencyTextBox);

            // Створення полей для введення видів робіт
            for (int i = 0; i < 10; i++)
            {
                TextBox jobTextBox = new TextBox() { Left = 150 + 120 * i, Top = 20, Text = $"Робота {i + 1}", Visible = false };
                jobsTextBoxes.Add(jobTextBox);
                Controls.Add(jobTextBox);
            }
            // Створення полей для введення виконавців
            for (int i = 0; i < 10; i++)
            {
                TextBox performerTextBox = new TextBox() { Left = 20, Top = 60 + 40 * i, Text = $"Виконавець {i + 1}", Visible = false };
                performersTextBoxes.Add(performerTextBox);
                Controls.Add(performerTextBox);

                efficiencyTextBoxes.Add(new List<TextBox>());
                for (int j = 0; j < 10; j++)
                {
                    TextBox efficiencyTextBox = new TextBox() { Left = 150 + 120 * j, Top = 60 + 40 * i, Visible = false };
                    efficiencyTextBoxes[i].Add(efficiencyTextBox);
                    Controls.Add(efficiencyTextBox);
                }
            }
            // Створення кнопок для керування кількістю
            // виконавців, робіт
            var lastButton = CreateGridControls(480, 20, "Виконавці\nВиди робіт", AddPerformerJob_Click, RemovePerformerJob_Click);
            // Створення кнопки для розрахунку плану
            // за введеними даними
            Button submitButton = new Button() { Top = 520, Text = "Розрахувати", AutoSize = true };
            submitButton.Left = this.Width / 2 - submitButton.Width / 2;
            submitButton.Click += SubmitButton_Click;
            Controls.Add(submitButton);


            for (int i = 0; i < 4; i++)
            {
                AddPerformer();
                AddJob();
            }
        }
        // Додавання виконавця та роботи
        // при натисканні кнопки додавання 
        // виконавця та роботи
        private void AddPerformerJob_Click(object sender, EventArgs e)
        {
            AddPerformer();
            AddJob();

        }
        // Видалення виконавця та роботи
        // при натисканні кнопки видалення 
        // виконавця та роботи
        private void RemovePerformerJob_Click(object sender, EventArgs e)
        {
            RemovePerformer();
            RemoveJob();
        }
        // Перевірка введених у поле користувачем даних
        private void EfficiencyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ','))
            {
                e.Handled = true;
            }
        }
        // Перевірка введених у поле користувачем даних
        private void EfficiencyTextBox_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch((sender as TextBox).Text, "[^0-9.,]"))
            {
                (sender as TextBox).Text = "";
            }
        }
        // Перевірка введених користувачем даних
        // якщо дані корректні, виконання розрахунку
        // за допомогою угорського методу
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }
            HungarianAlgorithm();
        }

    }
}
