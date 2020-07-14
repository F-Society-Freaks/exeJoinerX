using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace WindowsFormsApplication1
{
    public partial class form : Form
    {
        public form()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WindowsFormsApplication1.Resources.source.txt"))
            {
                if (stream != null)
                    using (var reader = new StreamReader(stream))
                    {
                        string result = reader.ReadToEnd();

                        byte[] res = File.ReadAllBytes(textBox1.Text);
                        result = result.Replace("$$$", Convert.ToBase64String(res));

                        byte[] res2 = File.ReadAllBytes(textBox2.Text);
                        result = result.Replace("###", Convert.ToBase64String(res2));

                        var providerOptions = new Dictionary<string, string>
                                                  {
                                                      {"CompilerVersion", "v2.0"}
                                                  };
                        CompilerResults results;
                        using (var provider = new CSharpCodeProvider(providerOptions))
                        {
                            var Params = new CompilerParameters { OutputAssembly = Directory.GetCurrentDirectory() + "/" + textBox3.Text +".exe", GenerateExecutable = true, CompilerOptions = "/t:winexe" };

                            Params.ReferencedAssemblies.Add("System.dll");
                            Params.ReferencedAssemblies.Add("System.Data.dll");

                            results = provider.CompileAssemblyFromSource(Params, result);
                        }

                        if (results.Errors.Count == 0)
                        {
                            MessageBox.Show("Готово!", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        foreach (CompilerError compilerError in results.Errors)
                        {
                            MessageBox.Show(string.Format("Ошибка: {0}", compilerError.ErrorText + " Строка: " + compilerError.Line));
                        }
                    }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if(textBox2.Text != "")
            {
                textBox3.Enabled = true;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if(textBox3.Text != "")
            {
                button1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
            }
        }
    }
}
