using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
namespace Quckies
{
    public partial class Quickies : Form
    {
        int btnCount = 0;
        List<string> dynamicButtons = new List<string>();
        public Quickies()
        {
            InitializeComponent();
            if (System.IO.File.Exists(Application.StartupPath + "\\execPaths.txt"))
            {
                foreach (var line in System.IO.File.ReadAllLines(Application.StartupPath+"\\execPaths.txt"))
                {
                    createQuickie(btnCount, line);
                    btnCount++;

                }
            }
        }

        private void SaveConfig()
        {
            string fname = "execPaths.txt";
            fname = Application.StartupPath + @"\" + fname;
//            MessageBox.Show(fname);
            System.IO.File.Delete(fname);
            System.IO.File.WriteAllLines(fname, dynamicButtons);
        }

        private void createQuickie(int index,string execPath)
        {
            Button dynamicButton = new Button();
            // Set Button properties 
            dynamicButton.Height = 40;
            dynamicButton.Width = 120;
            dynamicButton.Location = new Point((index/7)*(dynamicButton.Width+20),(index%7)*(dynamicButton.Height+20));
            // Set background and foreground  
            if (System.IO.File.Exists(execPath))
            {
             //   dynamicButton.Image = Image.FromFile(execPath);
            }
            dynamicButton.Text = execPath.Remove(0, execPath.LastIndexOf('\\') + 1); 
            dynamicButton.Name = execPath;
            dynamicButton.Font = new Font("Georgia", 8);
            dynamicButton.Click += new EventHandler(DynamicButton_Click);
            dynamicButtons.Add(execPath);
            tabPage1.Controls.Add(dynamicButton);
            listBox1.Items.Add(execPath);
        }

        private void DynamicButton_Click(object sender, EventArgs e)
        {
            string temp = (sender as Button).Name;
            if (System.IO.File.Exists(temp))
            {
                new Process { StartInfo = new ProcessStartInfo(temp) { UseShellExecute = true } }.Start();
            }
            else
            {
                MessageBox.Show("File : " + temp + " not found!");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {//opens dialogbox to create a quick action
            string temp = "Creating a quickie bound to : " + textBox1.Text;
            MessageBox.Show(temp);
            createQuickie(btnCount,textBox1.Text.ToString());
            btnCount++;
        }
        private void button2_Click(object sender, EventArgs e)
        {//remove quickie
            string temp = listBox1.Text;
            dynamicButtons.RemoveAt(dynamicButtons.IndexOf(temp));
            listBox1.Items.RemoveAt(listBox1.Items.IndexOf(temp));
            MessageBox.Show("Removed : |" + temp + "|");
            SaveConfig();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            textBox1.Text = dlg.FileName;

        }

        private void button4_Click(object sender, EventArgs e)
        {//save button config                        
            SaveConfig();
        }

        private void Quickies_ResizeEnd(object sender, EventArgs e)
        {
            tabControl1.Size = this.Size;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            foreach(var line in dynamicButtons)
            {
                listBox2.Items.Add(line);
            }
        }
    }
}
