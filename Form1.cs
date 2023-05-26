using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Редактор_базы_знаний
{
    public partial class Form1 : Form
    {

        public bool modify = false;
        List<int> id = new List<int>();
        List<bool> before = new List<bool>();
        List<int> countWarriable = new List<int>();
        List<bool> hasFirst = new List<bool>();
        List<string> keeWordsFirst = new List<string>();
        List<bool> hasSecond = new List<bool>();
        List<string> keeWordsSecond = new List<string>();
        List<string> styleType = new List<string>();
        List<string> solution = new List<string>();
        List<string> description = new List<string>();

        public Form1()
        {
            InitializeComponent();
            parser_xml();
        }

        public void parser_xml()
        {
            // Чтение из файла и десериализация
            XmlSerializer serializer = new XmlSerializer(typeof(Rules));
            using (FileStream fileStream = new FileStream(@"C:\Users\User\Desktop\plugin-for-requirements-analysis-main\knowledgebase.xml", FileMode.Open))
            {
                Rules rules = (Rules)serializer.Deserialize(fileStream);
                // Теперь данные находятся в rules.RuleList

                // Выводим содержимое каждого элемента
                foreach (Rule rule in rules.RuleList)
                {
                    // Собираем строку output из данных элемента Rule
                    string output = $"ID: {rule.Id}\n" +
                                    $"Description: {rule.Description}\n" +
                                    $"Before: {rule.Before}\n" +
                                    $"StyleType: {rule.StyleType}\n" +
                                    $"Solution: {rule.Solution}\n";
                    // Добавляем новую строку в dataGridView1 и записываем данные из output в столбец elem
                    dataGridView1.Rows.Add(output);
                    id.Add(rule.Id);
                    before.Add(rule.Before);
                    countWarriable.Add(rule.CountWarriable);
                    hasFirst.Add(rule.HasFirst);
                    styleType.Add(rule.StyleType);
                    solution.Add(rule.Solution);
                    description.Add(rule.Description);
                }
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //int index = dataGridView1.CurrentCell.RowIndex;
            int index = dataGridView1.CurrentRow.Index;
            if (index >= 0 && index < description.Count && description[index] != null)
            {
                textBox1.Text = description[index];
                richTextBox1.Text = solution[index];
                if (before[index])
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                }
                if (styleType[index] == "error")
                {
                    radioButton3.Checked = true;
                }
                else if (styleType[index] == "warning")
                {
                    radioButton4.Checked = true;
                }
                label8.Text = countWarriable[index].ToString();
            }
            else
            {
                textBox1.Text = "";
                richTextBox1.Text = "";
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = false;
                label8.Text = "";
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\newButtonClick.png");
            int rowCount = id.Last();
            id.Add(rowCount+1);
            before.Add(true);
            countWarriable.Add(0);
            hasFirst.Add(true);
            styleType.Add("");
            solution.Add("");
            description.Add("");
            dataGridView1.Rows.Add($"ID: {rowCount + 1}\n");
            pictureBox1.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\newButton.png");
            modify = true;
        }
        // Обработчик события MouseEnter
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            // Загружаем новое изображение
            pictureBox1.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\newButtonHover.png");
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            // Загружаем новое изображение
            pictureBox4.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\updateButtonHover.png");
        }
        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {
            // Загружаем новое изображение
            pictureBox5.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\deleteButtonHover.png");
        }
        // Обработчик события MouseLeave
        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            // Возвращаем изначальное изображение
            pictureBox4.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\updateButton.png");
        }
        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            // Возвращаем изначальное изображение
            pictureBox5.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\deleteButton.png");
        }
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            // Возвращаем изначальное изображение
            pictureBox1.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\newButton.png");
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

            pictureBox5.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\deleteButtonClick.png");
            // получаем индекс текущей строки
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;

            // выводим диалоговое окно подтверждения удаления
            DialogResult result = MessageBox.Show(
                "Вы точно хотите удалить элемент базы знаний?",
                "Удаление элемента",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            // если пользователь выбрал "Отмена", просто закрыть окно
            if (result == DialogResult.Cancel)
            {
                return;
            }

            // если пользователь выбрал "ОК", удалить текущую строку из DataGridView
            if (result == DialogResult.OK)
            {
                dataGridView1.Rows.RemoveAt(selectedRowIndex);
            }
            MessageBox.Show("Успешно", "Элемент удален из базы знаний.", MessageBoxButtons.OK,MessageBoxIcon.Information);

            pictureBox5.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\deleteButton.png");
            modify = true;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            pictureBox4.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\updateButtonClick.png");
            // получаем индекс текущей строки
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            description[selectedRowIndex] = textBox1.Text;
            solution[selectedRowIndex] = richTextBox1.Text;
            if (radioButton1.Checked) { before[selectedRowIndex] = true; }
            if (!radioButton1.Checked) { before[selectedRowIndex] = false; }
            if (radioButton3.Checked) { styleType[selectedRowIndex] = "error"; }
            if (!radioButton3.Checked) { styleType[selectedRowIndex] = "warning"; }

            //ПОКА ЗАГЛУШКА
            countWarriable[selectedRowIndex] = 0;

            // Собираем строку output из данных элемента Rule
            string output = $"ID: {id[selectedRowIndex]}\n" +
                            $"Description: {description[selectedRowIndex]}\n" +
                            $"Before: {before[selectedRowIndex]}\n" +
                            $"StyleType: {styleType[selectedRowIndex]}\n" +
                            $"Solution: {solution[selectedRowIndex]}\n";
            dataGridView1.Rows[selectedRowIndex].Cells[0].Value = output;

            MessageBox.Show("Успешно", "Элемент базы знаний обновлен.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            pictureBox4.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\updateButton.png");
            modify = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (modify)
            {
                DialogResult result = MessageBox.Show(
                    "Необходимо сохранить изменения в файле базы знаний. Продолжить?",
                    "Внимание",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Создаем XML-документ
                    XmlDocument doc = new XmlDocument();
                    // Создаем корневой элемент "rules"
                    XmlElement root = doc.CreateElement("rules");
                    // Добавляем корневой элемент в документ
                    doc.AppendChild(root);
                    // Сохраняем документ в файле "knowledgeBase.xml"
                    //doc.Save(@"C:\Users\User\Desktop\plugin-for-requirements-analysis-main\knowledgebase.xml");
                    doc.Save(@"C:\Users\User\Desktop\plugin-for-requirements-analysis-main\knowledgebaseTEST.xml");
                }
                if (result == DialogResult.Cancel)
                {
                    // Остановка закрытия формы
                    e.Cancel = true;
                }
            }
        }
    }
    // Определение классов для десериализации
    [XmlRoot("rules")]
    public class Rules
    {
        [XmlElement("rule")]
        public Rule[] RuleList { get; set; }
    }

    public class Rule
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("before")]
        public bool Before { get; set; }

        [XmlElement("countWarriable")]
        public int CountWarriable { get; set; }

        [XmlElement("hasFirst")]
        public bool HasFirst { get; set; }

        [XmlArray("keeWordsFirst"), XmlArrayItem("keeWordFirst")]
        public string[] KeeWordsFirst { get; set; }

        [XmlElement("hasSecond")]
        public bool HasSecond { get; set; }

        [XmlArray("keeWordsSecond"), XmlArrayItem("keeWordSecond")]
        public string[] KeeWordsSecond { get; set; }

        [XmlElement("styleType")]
        public string StyleType { get; set; }

        [XmlElement("solution")]
        public string Solution { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }
    }
}
