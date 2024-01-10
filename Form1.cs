using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms.Design;
using System.Runtime.InteropServices;
using System.Xml;


namespace OrganizationBoard
{

    public partial class Form1 : Form
    {
        public class csvValues
        {
            public string Todo { get; set; }
            public string Done { get; set; }
        }
        string folderPath;
        string[] filePaths;
        public List<string> lstToDo=  new List<String>();
        public List<string> lstDone= new List<String>();

        //Writes to First Column (Todo)
        public void csvWriterTodo(string toBeWritten, string path)
        {
                    string file = @path;
                    List<string> csv = new List<string>() { toBeWritten + "," };
                    File.AppendAllLines(file, csv);
        }
        
        //Writes to Second Column (Done)
        public void csvWriterDone(string toBeWritten, string path)
        {
            string file = @path;
            List<string> csv = new List<string>() { "," + toBeWritten };
            File.AppendAllLines(file, csv);
        }
        static public List<string> csvReader(string path, int position)
        {
            List<string> listToReturn = new List<string>();

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Assuming your CSV has two columns and you want to read the specified position
                while (csv.Read())
                {
                    if(string.IsNullOrWhiteSpace(csv.GetField<string>(position))== false)
                    listToReturn.Add(csv.GetField<string>(position));
                }
            }
            return listToReturn;
        }
            public Form1()
        {
            InitializeComponent();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            folderPath = @"D:\C#\OrganizationBoard\SavedCSV\";
            filePaths = Directory.GetFiles(folderPath);
            foreach (string filename in filePaths)
            {
                listBox2.Items.Add(Path.GetFileName(filename));
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            String file = @"D:\C#\OrganizationBoard\SavedCSV\" + textBox1.Text.ToString() + ".csv";

            using (var writer = new StreamWriter(file)) 
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<csvValues>();
                csv.NextRecord();
            }
            listBox2.Items.Clear();
            folderPath = @"D:\C#\OrganizationBoard\SavedCSV\";
            filePaths = Directory.GetFiles(folderPath);
            foreach (string filename in filePaths)
            {
                listBox2.Items.Add(Path.GetFileName(filename));
            }

        }
        private void listBox2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox3.Items.Clear();
            if (listBox2.SelectedItem != null)
            {
                String file = @"D:\C#\OrganizationBoard\SavedCSV\" + listBox2.SelectedItem.ToString();
                lstToDo = csvReader(file, 0);
                lstDone = csvReader(file, 1);
            }
            foreach (string item in lstToDo)
            {
                listBox3.Items.Add(item);
            }
            foreach (string item in lstDone)
            {
                listBox1.Items.Add(item);
            }


        }
        private void button2_Click(object sender, EventArgs e)

        {
            listBox1.Items.Clear();
            listBox3.Items.Clear();
            if (listBox2.SelectedItem != null) { 
            File.Delete(@"D:\C#\OrganizationBoard\SavedCSV\" + listBox2.SelectedItem.ToString());
            }
            listBox2.Items.Remove(listBox2.SelectedItem);
            
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string path = @"D:\C#\OrganizationBoard\SavedCSV\" + listBox2.SelectedItem.ToString();

            List<string> lines = File.ReadAllLines(path).ToList();
            if (!string.IsNullOrWhiteSpace(listBox3.SelectedItem?.ToString()))
            {
                string stringToRemove = listBox3.SelectedItem.ToString();

                // Assuming lines is a List<string>
                lines = lines
                    .Where(line =>
                    {
                        // Split the line into columns
                        string[] columns = line.Split(',');

                        // Check if the first column contains the string to remove
                        return columns.Length > 0 && !columns[0].Contains(stringToRemove);
                    })
                    .ToList();

                File.WriteAllLines(path, lines);
            }
            listBox1.Items.Add(listBox3.SelectedItem);
            csvWriterDone(listBox3.SelectedItem.ToString(), path);
            lstToDo.Remove(listBox3.SelectedItem.ToString());
            listBox3.Items.Remove(listBox3.SelectedItem);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string path = @"D:\C#\OrganizationBoard\SavedCSV\" + listBox2.SelectedItem.ToString();
            csvWriterTodo(textBox3.Text.ToString(), path);
            listBox3.Items.Add(textBox3.Text);
            lstToDo.Add(textBox3.Text);
            
                
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string path = @"D:\C#\OrganizationBoard\SavedCSV\" + listBox2.SelectedItem.ToString();
            List<string> lines = File.ReadAllLines(path).ToList();
            if (!string.IsNullOrWhiteSpace(listBox1.SelectedItem?.ToString()))
            {
                string stringToRemove = listBox1.SelectedItem.ToString();

                // Assuming lines is a List<string>
                lines = lines
                    .Where(line =>
                    {
                        // Split the line into columns
                        string[] columns = line.Split(',');

                        // Check if the first column contains the string to remove
                        return columns.Length > 0 && !columns[1].Contains(stringToRemove);
                    })
                    .ToList();

                File.WriteAllLines(path, lines);
            
            }
            lstDone.Remove(listBox1.SelectedItem.ToString());
            listBox1.Items.Remove(listBox1.SelectedItem);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                String file = @"D:\C#\OrganizationBoard\SavedCSV\" + listBox2.SelectedItem.ToString();
                lstToDo = csvReader(file, 0);
                lstDone = csvReader(file, 1);
            }
            foreach (string item in lstToDo)
            {
                listBox3.Items.Add(item);
            }
            foreach (string item in lstDone)
            {
                listBox1.Items.Add(item);
            }
        }
    }
}

