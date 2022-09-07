using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ZT1
{
    public partial class Form1 : Form
    {
        string array;
        byte[] fileBytes;
        public Form1()
        {
            InitializeComponent();
        }
        private void textBoxInputNumbers_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxInput = (TextBox)sender;
            bool checkISNumber = int.TryParse(textBoxInput.Text, out int buffer);
            if (checkISNumber != true)
            {
                if (textBoxInput.Text.Length != 0)
                {
                    textBoxInput.Text = textBoxInput.Text.Remove((textBoxInput.Text.Length - 1));
                    textBoxInput.SelectionStart = textBoxInput.Text.Length;
                    textBoxInput.SelectionLength = 0;
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;


            OpenFileDialog opfd = new OpenFileDialog();
            opfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            opfd.FilterIndex = 2;


            if (opfd.ShowDialog() == DialogResult.OK)
            {
                filePath = opfd.FileName;
                var fileSize = new FileInfo(opfd.FileName).Length / 1024;
                var fileStream = opfd.OpenFile();

               
                txtFile.Text = filePath;
                txtSize.Text = fileSize.ToString() + " Kb";
                if (fileSize < 100000)
                {

                button2.Enabled= true;
                button3.Enabled = true;
               
                    fileBytes = File.ReadAllBytes(filePath);
                    StringBuilder sb = new StringBuilder();

                    foreach (byte b in fileBytes)
                    {
                        sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));

                    }

                    array = sb.ToString();
                }
                else { MessageBox.Show("Файл занадто великий, оберіть файл менший за 100мб"); }

            }
            else { MessageBox.Show("Оберіть файл у провіднику"); }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int k = 0;
            int x, y;

            if (txtY.Text != string.Empty || txtX.Text != string.Empty)
            {

                int height = Convert.ToInt32(txtY.Text);
                int width = Convert.ToInt32(txtX.Text);
                
                if (height > 0 && height < 513 && width > 0 && width < 1025)
                {
                    pictureBox1.Size= new Size(width, height);
                    try
                    {
                        // Retrieve the image.
                        Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                        // Loop through the images pixels to reset color.
                        for (y = 0; y < height; y++)
                        {

                            for (x = 0; x < width; x++)
                            {

                                if (k < array.Length)
                                {
                                    if (array[k] == '1') { bmp.SetPixel(x, y, Color.Black); }
                                    if (array[k] == '0') { bmp.SetPixel(x, y, Color.White); }
                                    k++;
                                }
                                else { bmp.SetPixel(x, y, Color.Gray); }



                            }
                        }

                        // Set the PictureBox to display the image.
                        pictureBox1.Image = bmp;

                        // Display the pixel format in Label1.

                    }
                    catch (ArgumentException)
                    {
                        MessageBox.Show("There was an error." +
                            "Try to write another resolution");
                    }

                }
                else
                {
                    MessageBox.Show("Введіть висоту від 0 до 512 та ширину від 0 до 1024");
                }
            }
            else { MessageBox.Show("Введіть висоту від 0 до 512 та ширину від 0 до 1024"); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string nameBin = txtBin.Text;
            string nameDec = txtDec.Text;
            
            

            // полная перезапись файла 
            using (StreamWriter writer = new StreamWriter(nameBin, false))
            {
                StringBuilder sb = new StringBuilder();
                foreach (byte b in fileBytes)
                {
                    
                    writer.Write(Convert.ToString(b, 2).PadLeft(8, '0')+" ");

                }
             
            }

            using (StreamWriter writer = new StreamWriter(nameDec, false))
            {
                StringBuilder sb = new StringBuilder();
                foreach (byte b in fileBytes)
                {

                    writer.Write(b.ToString() + " ");

                }


            }

            string directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            System.IO.FileInfo fileBin = new System.IO.FileInfo(directory + @"\FileBinary.txt");
            System.IO.FileInfo fileDec = new System.IO.FileInfo(directory + @"\FileDecimal.txt");
            long sizeBin = fileBin.Length;
            long sizeDec = fileDec.Length;
            txtBytesBin.Text = sizeBin.ToString();

            txtBytesDec.Text = sizeDec.ToString();


        }
    }
}