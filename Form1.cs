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
        public string dgHS1 = null;
        public string dgHS2 = null;
        public bool modify = false;
        List<int> id = new List<int>();
        List<bool> before = new List<bool>();
        List<int> countWarriable = new List<int>();
        List<string> hasFirst = new List<string>();
        List<string[]> keeWordsFirst = new List<string[]>();
        List<string> hasSecond = new List<string>();
        List<string[]> keeWordsSecond = new List<string[]>();
        List<string> styleType = new List<string>();
        List<string> solution = new List<string>();
        List<string> description = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        public void parser_xml(string FileName)
        {
            // Чтение из файла и десериализация
            XmlSerializer serializer = new XmlSerializer(typeof(Rules));
            using (FileStream fileStream = new FileStream(FileName, FileMode.Open))
            {
                Rules rules = (Rules)serializer.Deserialize(fileStream);
                // Теперь данные находятся в rules.RuleList

                // Выводим содержимое каждого элемента
                foreach (Rule rule in rules.RuleList)
                {
                    // Собираем строку output из данных элемента Rule
                    string output = $"Ошибка: {rule.Description}; " +
                                    $"Решение: {rule.Solution}";
                    // Добавляем новую строку в dataGridView1 и записываем данные из output в столбец elem
                    dataGridView1.Rows.Add(output);
                    id.Add(rule.Id);
                    before.Add(rule.Before);
                    countWarriable.Add(rule.CountWarriable);
                    hasFirst.Add(rule.HasFirst);
                    keeWordsFirst.Add(rule.KeeWordsFirst);
                    hasSecond.Add(rule.HasSecond);
                    keeWordsSecond.Add(rule.KeeWordsSecond);
                    styleType.Add(rule.StyleType);
                    solution.Add(rule.Solution);
                    description.Add(rule.Description);
                }
            }
            dataGridView1_Update();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_Update();
        }
        public void dataGridView1_Update()
        {

            //int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView2.Rows.Clear();
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
                dataGridView2.Rows.Clear();
                if (hasFirst[index] != null)
                {
                    foreach (string values in keeWordsFirst[index])
                    {
                        string type = "";
                        if (hasFirst[index]=="true") { type = "Есть слово"; } else{ type = "Нет слова"; }
                        dataGridView2.Rows.Add("Первая", type, values.ToString());
                    }
                }
                if (hasSecond[index] != null)
                {
                    foreach (string values in keeWordsSecond[index])
                    {
                        string type = "";
                        if (hasSecond[index] == "true") { type = "Есть слово"; } else { type = "Нет слова"; }
                        dataGridView2.Rows.Add("Вторая", type, values.ToString());
                    }
                }

                int dgHS1Mod = 0;
                int dgHS2Mod = 0;
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    // Получаем значение в столбце "num" текущей строки
                    object numValue = row.Cells["num"].Value;
                    object typeValue = row.Cells["type"].Value;

                    // Проверяем, что значение не равно null и что можно привести к нужному типу данных (int)
                    if (numValue != null)
                    {
                        if (numValue.ToString() == "first")
                        {
                            dgHS1 = typeValue.ToString();
                            dgHS1Mod += 1;
                        }
                        if (numValue.ToString() == "second")
                        {
                            dgHS2 = typeValue.ToString();
                            dgHS2Mod += 1;
                        }
                    }
                }
                if (dgHS1Mod == 0) { dgHS1 = null; }
                if (dgHS2Mod == 0) { dgHS2 = null; }
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
            int rowCount = id.Last();
            id.Add(rowCount + 1);
            before.Add(true);
            countWarriable.Add(0);
            hasFirst.Add(null);
            keeWordsFirst.Add(null);
            hasSecond.Add(null);
            keeWordsSecond.Add(null);
            styleType.Add(null);
            solution.Add(null);
            description.Add(null);
            dataGridView1.Rows.Add($"ID: {rowCount + 1}\n");
            modify = true;
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            // Загружаем новое изображение
            pictureBox1.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\circleAddHover.png");
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            // Загружаем новое изображение
            pictureBox4.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\circleUpdateHover.png");
        }
        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {
            // Загружаем новое изображение
            pictureBox5.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\circleCutHover.png");
        }
        private void pictureBox6_MouseEnter(object sender, EventArgs e)
        {
            // Загружаем новое изображение
            pictureBox6.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\circleAddHover.png");
        }
        private void pictureBox7_MouseEnter(object sender, EventArgs e)
        {
            // Загружаем новое изображение
            pictureBox7.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\circleCutHover.png");
        }
        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            // Возвращаем изначальное изображение
            pictureBox4.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\circleUpdate.png");
        }
        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            // Возвращаем изначальное изображение
            pictureBox5.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\circleCut.png");
        }
        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {
            // Загружаем новое изображение
            pictureBox6.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\circleAdd.png");
        }
        private void pictureBox7_MouseLeave(object sender, EventArgs e)
        {
            // Загружаем новое изображение
            pictureBox7.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\circleCut.png");
        }
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            // Возвращаем изначальное изображение
            pictureBox1.Image = Image.FromFile(@"C:\Users\User\Desktop\icons\circleAdd.png");
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

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

                id.RemoveAt(selectedRowIndex);
                before.RemoveAt(selectedRowIndex);
                countWarriable.RemoveAt(selectedRowIndex);
                hasFirst.RemoveAt(selectedRowIndex);
                keeWordsFirst.RemoveAt(selectedRowIndex);
                hasSecond.RemoveAt(selectedRowIndex);
                keeWordsSecond.RemoveAt(selectedRowIndex);
                styleType.RemoveAt(selectedRowIndex);
                solution.RemoveAt(selectedRowIndex);
                description.RemoveAt(selectedRowIndex);
            }
            MessageBox.Show("Элемент удален из базы знаний.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

            modify = true;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            // получаем индекс текущей строки
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            description[selectedRowIndex] = textBox1.Text;
            solution[selectedRowIndex] = richTextBox1.Text;
            if (radioButton1.Checked) { before[selectedRowIndex] = true; }
            if (!radioButton1.Checked) { before[selectedRowIndex] = false; }
            if (radioButton3.Checked) { styleType[selectedRowIndex] = "error"; }
            if (!radioButton3.Checked) { styleType[selectedRowIndex] = "warning"; }

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                // Получаем значение в столбце "num" текущей строки
                object numValue = row.Cells["num"].Value;
                object typeValue = row.Cells["type"].Value;
                object keeWordValue = row.Cells["keeWord"].Value;

                // Проверяем, что значение не равно null и что можно привести к нужному типу данных (int)
                if (numValue != null)
                {
                    if (numValue.ToString() == "first")
                    {
                        if (keeWordsFirst[selectedRowIndex] != null)
                        {
                            string[] nowKeeWord = keeWordsFirst[selectedRowIndex];
                            Array.Resize(ref nowKeeWord, nowKeeWord.Length + 1);
                            nowKeeWord[nowKeeWord.Length - 1] = keeWordValue.ToString();
                            keeWordsFirst[selectedRowIndex] = nowKeeWord;
                        }
                        else
                        {
                            string[] nowKeeWord = new string[]{keeWordValue.ToString()};
                            keeWordsFirst[selectedRowIndex] = nowKeeWord;
                        }
                        hasFirst[selectedRowIndex] = typeValue.ToString();
                    }
                    if (numValue.ToString() == "second")
                    {
                        if (keeWordsFirst[selectedRowIndex] != null)
                        {
                            string[] nowKeeWord = keeWordsSecond[selectedRowIndex];
                            Array.Resize(ref nowKeeWord, nowKeeWord.Length + 1);
                            nowKeeWord[nowKeeWord.Length - 1] = keeWordValue.ToString();
                            keeWordsSecond[selectedRowIndex] = nowKeeWord;
                        }
                        else
                        {
                            string[] nowKeeWord = new string[] { keeWordValue.ToString() };
                            keeWordsSecond[selectedRowIndex] = nowKeeWord;
                        }
                        hasSecond[selectedRowIndex] = typeValue.ToString();
                    }
                }
            }

            if (hasFirst[selectedRowIndex] != null && hasSecond[selectedRowIndex] != null)
            {
                countWarriable[selectedRowIndex] = 2;
            }
            else if (hasFirst[selectedRowIndex] != null || hasSecond[selectedRowIndex] != null)
            {
                countWarriable[selectedRowIndex] = 1;

            }
            else
            {
                countWarriable[selectedRowIndex] = 0;
            }

                // Собираем строку output из данных элемента Rule
                string output = $"ID: {id[selectedRowIndex]}\n" +
                            $"Description: {description[selectedRowIndex]}\n" +
                            $"Before: {before[selectedRowIndex]}\n" +
                            $"StyleType: {styleType[selectedRowIndex]}\n" +
                            $"Solution: {solution[selectedRowIndex]}\n";
            dataGridView1.Rows[selectedRowIndex].Cells[0].Value = output;

            MessageBox.Show("Элемент базы знаний обновлен.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    // Инициализируем объект saveFileDialog1
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                    // Устанавливаем фильтры для отображения только файлов нужного расширения
                    saveFileDialog1.Filter = "Text Files (*.xml)|*.xml|All files (*.*)|*.*";

                    // Если пользователь выбрал место сохранения файла и ввел его имя, и нажал кнопку "OK"
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        // Возвращаем путь к выбранному файлу
                        saveFile(saveFileDialog1.FileName);
                    }
                }
                if (result == DialogResult.Cancel)
                {
                    // Остановка закрытия формы
                    e.Cancel = true;
                }
            }
        }
        public void saveFile(string FileName)
        {
            KnowledgeBaseXmlCreator creator = new KnowledgeBaseXmlCreator();
            creator.CreateXmlFile(FileName, id, before, countWarriable, hasFirst, keeWordsFirst, hasSecond, keeWordsSecond, styleType, solution, description);

            MessageBox.Show("Сохранено.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            int selectedRowIndex1 = dataGridView1.SelectedCells[0].RowIndex;
            // Получаем значение в столбце "keeWord" выбранной строки dataGridView2
            object keeWordValue = dataGridView2.SelectedCells[0].OwningRow.Cells["keeWord"].Value;
            object numValue = dataGridView2.SelectedCells[0].OwningRow.Cells["num"].Value;

            // Если значение не равно null и можно привести к типу string
            if (keeWordValue != null && keeWordValue is string keeWord)
            {
                if (numValue == "first")
                {
                    // Удаляем из массива keeWords все элементы, которые равны keeWord
                    keeWordsFirst[selectedRowIndex1] = Array.FindAll(keeWordsFirst[selectedRowIndex1], i => i != keeWord).ToArray();
                    //keeWordsFirst[selectedRowIndex1].RemoveAll(x => x[0] == keeWord);
                }
                if (numValue == "second")
                {
                    // Удаляем из массива keeWords все элементы, которые равны keeWord
                    keeWordsSecond[selectedRowIndex1] = Array.FindAll(keeWordsSecond[selectedRowIndex1], i => i != keeWord).ToArray();
                    //keeWordsSecond[selectedRowIndex1].RemoveAll(x => x[0] == keeWord);
                }
            }

            int selectedRowIndex2 = dataGridView2.SelectedCells[0].RowIndex;
            dataGridView2.Rows.RemoveAt(selectedRowIndex2);
            modify = true;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            string num = "";
            string type = "";
            string keeWord = "";
            bool approveType = false;
            if ((dgHS1 != null && radioButton7.Checked)||
                (dgHS2 != null && !radioButton7.Checked)||
                (dgHS1 == null && dgHS2 == null && radioButton5.Checked)||
                (dgHS1 == null && dgHS2 == null && !radioButton5.Checked)) { approveType = true; }
            if ((radioButton7.Checked|| radioButton8.Checked) && approveType && textBox2.Text.ToString()!="")
            {
                if (radioButton7.Checked) { num = "Первая"; } else { num = "Вторая"; }
                if (dgHS1 != null && radioButton7.Checked) { type = dgHS1; };
                if (dgHS2 != null && !radioButton7.Checked) { type = dgHS2; };
                if (dgHS1 == null && dgHS2 == null && radioButton5.Checked) { type = "Есть слово"; };
                if (dgHS1 == null && dgHS2 == null && !radioButton5.Checked) { type = "Нет слова"; };
                keeWord = textBox2.Text.ToString();
                dataGridView2.Rows.Add(num, type, keeWord);
                textBox2.Text = "";
                radioButton5.Checked = false;
                radioButton6.Checked = false;
                radioButton7.Checked = false;
                radioButton8.Checked = false;
                num = "";
                type = "";
                keeWord = "";
            }
            else
            {
                DialogResult result = MessageBox.Show(
                    "Необходимо заполнить все поля на форме.",
                    "Внимание",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            modify = true;
            label11.Visible = true;
            panel5.Visible = true;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (dgHS1 != null)
            {
                label11.Visible = false;
                panel5.Visible = false;
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (dgHS2 != null)
            {
                label11.Visible = false;
                panel5.Visible = false;
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Инициализируем объект openFileDialog1
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Устанавливаем фильтры для отображения только файлов нужного расширения
            openFileDialog1.Filter = "Text Files (*.xml)|*.xml|All files (*.*)|*.*";

            // Если пользователь выбрал файл и нажал кнопку "OK"
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Возвращаем путь к выбранному файлу
                parser_xml(openFileDialog1.FileName);
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Инициализируем объект saveFileDialog1
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            // Устанавливаем фильтры для отображения только файлов нужного расширения
            saveFileDialog1.Filter = "Text Files (*.xml)|*.xml|All files (*.*)|*.*";

            // Если пользователь выбрал место сохранения файла и ввел его имя, и нажал кнопку "OK"
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Возвращаем путь к выбранному файлу
                saveFile(saveFileDialog1.FileName);
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

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
        public string HasFirst { get; set; }

        [XmlArray("keeWordsFirst"), XmlArrayItem("keeWordFirst")]
        public string[] KeeWordsFirst { get; set; }

        [XmlElement("hasSecond")]
        public string HasSecond { get; set; }

        [XmlArray("keeWordsSecond"), XmlArrayItem("keeWordSecond")]
        public string[] KeeWordsSecond { get; set; }

        [XmlElement("styleType")]
        public string StyleType { get; set; }

        [XmlElement("solution")]
        public string Solution { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }
    }
    public class KnowledgeBaseXmlCreator
    {
        public void CreateXmlFile(string FileName, List<int> ids, List<bool> befores, List<int> countWarriables, List<string> hasFirsts, List<string[]> keeWordsFirsts, List<string> hasSeconds, List<string[]> keeWordsSeconds, List<string> styleTypes, List<string> solutions, List<string> descriptions)
        //public void CreateXmlFile(List<int> ids, List<bool> befores, List<int> countWarriables, List<string> styleTypes, List<string> solutions, List<string> descriptions)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement rules = xmlDoc.CreateElement("rules");
            for (int i = 0; i < ids.Count; i++)
            {
                XmlElement rule = CreateRuleElement(xmlDoc, ids[i], befores[i], countWarriables[i], hasFirsts[i], keeWordsFirsts[i], hasSeconds[i], keeWordsSeconds[i], styleTypes[i], solutions[i], descriptions[i]);

                //XmlElement rule = CreateRuleElement(xmlDoc, ids[i], befores[i], countWarriables[i], styleTypes[i], solutions[i], descriptions[i]);
                rules.AppendChild(rule);
            }
            xmlDoc.AppendChild(rules);
            xmlDoc.Save(FileName);
        }

        private XmlElement CreateRuleElement(XmlDocument xmlDoc, int id, bool before, int countWarriable, string hasFirst, string[] keeWordsFirst, string hasSecond, string[] keeWordsSecond, string styleType, string solution, string description)
        //private XmlElement CreateRuleElement(XmlDocument xmlDoc, int id, bool before, int countWarriable, string styleType, string solution, string description)
        {
            XmlElement rule = xmlDoc.CreateElement("rule");

            XmlElement idElem = xmlDoc.CreateElement("id");
            idElem.InnerText = id.ToString();
            rule.AppendChild(idElem);

            XmlElement beforeElem = xmlDoc.CreateElement("before");
            beforeElem.InnerText = before.ToString().ToLower();
            rule.AppendChild(beforeElem);

            XmlElement countWarriableElem = xmlDoc.CreateElement("countWarriable");
            countWarriableElem.InnerText = countWarriable.ToString();
            rule.AppendChild(countWarriableElem);

            if (hasFirst != null)
            {
                XmlElement hasFirstElem = xmlDoc.CreateElement("hasFirst");
                hasFirstElem.InnerText = hasFirst.ToString();
                rule.AppendChild(hasFirstElem);

                XmlElement keeWordsFirstElem = xmlDoc.CreateElement("keeWordsFirst");
                foreach (string word in keeWordsFirst)
                {
                    XmlElement keeWordFirstElem = xmlDoc.CreateElement("keeWordFirst");
                    keeWordFirstElem.InnerText = word;
                    keeWordsFirstElem.AppendChild(keeWordFirstElem);
                }
                rule.AppendChild(keeWordsFirstElem);
            }
            if (hasSecond != null)
            {
                XmlElement hasSecondElem = xmlDoc.CreateElement("hasSecond");
                hasSecondElem.InnerText = hasSecond.ToString();
                rule.AppendChild(hasSecondElem);

                XmlElement keeWordsSecondElem = xmlDoc.CreateElement("keeWordsSecond");
                foreach (string word in keeWordsSecond)
                {
                    XmlElement keeWordSecondElem = xmlDoc.CreateElement("keeWordSecond");
                    keeWordSecondElem.InnerText = word;
                    keeWordsSecondElem.AppendChild(keeWordSecondElem);
                }
                rule.AppendChild(keeWordsSecondElem);
            }
            XmlElement styleTypeElem = xmlDoc.CreateElement("styleType");
            styleTypeElem.InnerText = styleType;
            rule.AppendChild(styleTypeElem);

            XmlElement solutionElem = xmlDoc.CreateElement("solution");
            solutionElem.InnerText = solution;
            rule.AppendChild(solutionElem);

            XmlElement descriptionElem = xmlDoc.CreateElement("description");
            descriptionElem.InnerText = description;
            rule.AppendChild(descriptionElem);
            return rule;
        }
    }
}