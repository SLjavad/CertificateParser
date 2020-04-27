using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CertificateAnalysis
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        byte[] certFile = null;
        CertificateModel model;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                certFile = File.ReadAllBytes(openFileDialog.FileName);
                if (certFile == null)
                {
                    MessageBox.Show("File is empty");
                }
                else
                {
                    try
                    {
                        model = CertificateReader.Read(certFile);
                        ShowForm showForm = new ShowForm();
                        showForm.richTextBox1.Text = $"Name is {model.Name} \n Issuer is {model.Issuer} \n ValidTo is {model.ValidTo} \n Valid From is {model.ValidFrom}";
                        showForm.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.GetBaseException().ToString());
                    }

                }
            }

            
        }

        private void button2_clicked(object sender, EventArgs e)
        {
            if (model == null)
            {
                MessageBox.Show("There's no cert");
            }else
            {
                ShowForm showForm = new ShowForm();
                showForm.richTextBox1.Text = model.PubKey;
                showForm.Show();
            }
        }

        private void btnText_Clicked(object sender, EventArgs e)
        {
            if (richTextBox1.Text == string.Empty)
            {
                MessageBox.Show("textbox is empty");
            }
            else
            {
                try
                {
                    model = CertificateReader.Read(richTextBox1.Text.Trim());
                    ShowForm showForm = new ShowForm();
                    showForm.richTextBox1.Text = $"Name is {model.Name} \n Issuer is {model.Issuer} \n ValidTo is {model.ValidTo} \n Valid From is {model.ValidFrom}";
                    showForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.GetBaseException().ToString());
                }
            }
        }

        private void HPKP(object sender, EventArgs e)
        {
            if (model == null)
            {
                MessageBox.Show("There's no cert");
            }
            else
            {
                MessageBox.Show(CertificateReader.HPKP(model.PubKey));
            }
        }

        private async void btnUrl_Clicked(object sender, EventArgs e)
        {
            var url = textBox1.Text.Trim();
            if (url == string.Empty)
            {
                MessageBox.Show("No URL Found");
            }
            else
            {
                if (!url.StartsWith("https"))
                {
                    if (!url.StartsWith("http"))
                    {
                        url = "https://" + url;
                    } else
                    {
                        url = "https:" + url.Split(':')[1];
                    }
                }
                HttpClient httpClient = new HttpClient(new HttpClientHandler
                {
                    UseDefaultCredentials = true,
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, error) =>
                    {
                        model = CertificateReader.Read(cert.RawData);
                        return true;
                    }
                });
                await httpClient.GetAsync(url);
                if (model != null)
                {
                    ShowForm showForm = new ShowForm();
                    showForm.richTextBox1.Text = $"Name is {model.Name} \n Issuer is {model.Issuer} \n ValidTo is {model.ValidTo} \n Valid From is {model.ValidFrom}";
                    showForm.Show();
                }
            }
        }
    }
}
