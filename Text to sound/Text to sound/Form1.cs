using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;

namespace Text_to_sound
{
    public partial class Form1 : Form
    {
        ChromeOptions options = new ChromeOptions();
        ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
        ChromeDriver session;

        string downloadPath = Application.StartupPath+"\\Downloads";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                listBox1.Items.Add(textBox1.Text + " > " + comboBox1.SelectedItem.ToString());
                session.FindElement(By.Name("text")).SendKeys(textBox1.Text);
                var dropDown = session.FindElement(By.Name("voice"));
                var selectElement = new SelectElement(dropDown);
                selectElement.SelectByIndex(comboBox1.SelectedIndex);
                session.FindElement(By.CssSelector("#app > div:nth-child(1) > div > form > div:nth-child(3) > input")).Click();
                textBox1.Text = null;
            }
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem != null)
            {
                Play(listBox1.Items.Count - listBox1.SelectedIndex);
            }
        }
        private void downloadButton_Click(object sender, EventArgs e)
        {
            Download(listBox1.Items.Count - listBox1.SelectedIndex);
        }
        private void downloadAllButton_Click(object sender, EventArgs e)
        {
            DownloadAll();
        }
        void Download(int index)
        {
            session.FindElement(By.CssSelector("#app > div.section.section--colored > div > div:nth-child(" + index.ToString() + ")" + " > div.card__actions > a:nth-child(2)")).Click();
        }
        void DownloadAll()
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                int index = listBox1.Items.Count - i;
                Download(index);
            }
        }
        void Play(int index) 
        {
            session.FindElement(By.CssSelector("#app > div.section.section--colored > div > div:nth-child(" + index.ToString() + ")" + " > div.card__actions > a:nth-child(1)")).Click();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(downloadPath))
            {
                System.IO.Directory.CreateDirectory(downloadPath);
            }
            driverService.HideCommandPromptWindow = true;
            options.AddArgument("headless");
            options.AddUserProfilePreference("download.default_directory", downloadPath);
            session = new ChromeDriver(driverService, options);
            session.Url = "https://soundoftext.com";
            SelectElement select = new SelectElement(session.FindElement(By.Name("voice")));
            foreach (WebElement item in select.Options)
            {
                comboBox1.Items.Add(item.Text);
            }
            comboBox1.SelectedIndex = comboBox1.FindStringExact("English (United States)");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            session.Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            session.Close();
        }
    }
}
