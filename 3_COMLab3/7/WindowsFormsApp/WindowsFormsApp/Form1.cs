using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            axAcroPDF1.LoadFile(@"C:\Users\mipig\Desktop\KSR\COMLab3\7\Get_Started_With_Smallpdf.pdf");
            axWindowsMediaPlayer1.URL = @"C:\Users\mipig\Desktop\KSR\COMLab3\7\Ring01.wav";
            axWebBrowser1.Navigate(@"https://enauczanie.pg.edu.pl/");
        }
    }
}
