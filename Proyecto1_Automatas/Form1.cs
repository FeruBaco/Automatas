using System;
using System.IO;
using System.Windows.Forms;

namespace Proyecto1_Automatas
{
    public partial class Form1 : Form
    {
        private string text;
        private int webCounter;
        private int ebayCounter;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                txtRoute.Text = openFile.FileName;
                text = File.ReadAllText(@txtRoute.Text);
                CountWords();
            }
        }

        private void CountWords()
        {
            webCounter = 0;
            ebayCounter = 0;
            StringReader reader = new StringReader(text.ToLower());
            //Estado 1
            while (reader.Peek() > -1)
            {
                if (reader.Peek() == -1) { break; }
                switch ((char)reader.Read())
                {
                    //Estado 12
                    case 'w':
                        //Estado 135
                        if ((char)reader.Peek() == 'e')
                        {
                            reader.Read();
                            if (reader.Peek() == -1) { break; }
                            //Estado 146
                            if ((char)reader.Peek() == 'b')
                            {
                                webCounter++;
                                reader.Read();
                                if (reader.Peek() == -1) { break; }
                                //Estado 17
                                if ((char)reader.Peek() == 'a')
                                {
                                    if (reader.Peek() == -1) { break; }
                                    reader.Read();
                                    //Estado 18
                                    if ((char)reader.Peek() == 'y')
                                    {
                                        ebayCounter++;
                                        break;
                                    }
                                    break;
                                }
                                break;
                            }
                            break;
                        }
                        break;
                    //Estado 15
                    case 'e':
                        //Estado 16
                        if ((char)reader.Peek() == 'b')
                        {
                            reader.Read();
                            if (reader.Peek() == -1) { break; }
                            if ((char)reader.Peek() == 'a')
                            {
                                reader.Read();
                                if (reader.Peek() == -1) { break; }
                                if ((char)reader.Peek() == 'y')
                                {
                                    ebayCounter++;
                                    break;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            lblWeb.Text = "Web aparece: " + webCounter;
            lblEbay.Text = "Ebay aparece: " + ebayCounter;
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            txtRoute.Text = openFile.FileName;
            text = File.ReadAllText(@txtRoute.Text);
            CountWords();
        }
    }
}
