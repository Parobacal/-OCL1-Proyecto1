using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using Proyecto1_201602503.Estructuras;
using Proyecto1_201602503.AFD_N;


namespace Proyecto1_201602503
{
    public partial class Form1 : Form
    {
        public int contador_img = 0;
        public int contador_img1 = 0;
        public string img,img1;
        Automata automata = new Automata();
        public Form1()
        {
            InitializeComponent();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            Stream myStream;
            ofd.Filter = "ER|*.er";
            if (ofd.ShowDialog() == DialogResult.OK) {
                if ((myStream = ofd.OpenFile()) != null)
                {
                    tabControl.SelectedTab.Controls.Clear();
                    tabControl.SelectedTab.Text = System.IO.Path.GetFileName(ofd.FileName);
                    RichTextBox richtextbox = new RichTextBox();
                    richtextbox.Size = tabControl.Size;
                    string texto = File.ReadAllText(ofd.FileName);
                    richtextbox.Text = texto;                  
                    tabControl.SelectedTab.Controls.Add(richtextbox);
                }
                else {
                    MessageBox.Show("No se pudo leer el archivo");
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnsiguiente_Click(object sender, EventArgs e)
        {
            contador_img++;
            if (contador_img < 2)
            {

                img = "A" + contador_img + ".png";
            }
            else {
                contador_img--;
            }
            pictureBox1.Image = Image.FromFile(@"C:\Users\Pablo Barillas\Desktop\Automatas\" + img);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void btnanterior_Click(object sender, EventArgs e)
        {
            contador_img--;
            if (contador_img >= 0)
            {
                
                img = "A" + contador_img + ".png";
            }
            else
            {
                contador_img++;
                img = "A" + contador_img + ".png";
            }
            pictureBox1.Image = Image.FromFile(@"C:\Users\Pablo Barillas\Desktop\Automatas\" + img);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void agregarPestanaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage tabpage = new TabPage("Nuevo");
            tabControl.TabPages.Add(tabpage);
        }

        private void cerrarPestanaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl.TabPages.Remove(tabControl.SelectedTab);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            contador_img1++;
            if (contador_img1 < 2)
            {

                img1 = "A" + contador_img1 + ".png";
            }
            else
            {
                contador_img1--;
            }
            pictureBox2.Image = Image.FromFile(@"C:\Users\Pablo Barillas\Desktop\Tablas_estados\" + img1);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK) {
                using (Stream s = File.Open(sfd.FileName,FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(s)) {
                    sw.Write(tabControl.SelectedTab.Controls.OfType<RichTextBox>().Reverse().FirstOrDefault().Text);
                }
            }
        }

        private void generarPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Metodo que genera el reporte HTML para los archivos de entrada
            automata.reporteHtml();
        }

        public string Output(Lista_Simple Conjuntos, Lista_Simple Expresiones, Lista_Simple Lexemas) {
            string texto = "";
            texto += "\n";
            texto += "Conjuntos: \n";
            for (int i = 0; i < Conjuntos.getSize(); i ++) {

                texto += "Nombre: " + Conjuntos.obtenerNodo(i).getNombre() + "\n";
                for (int j = 0; j < Conjuntos.obtenerNodo(i).getElementos().Count; j ++) {
                    texto += Conjuntos.obtenerNodo(i).getElementos()[j] + "\n";
                }
            }
            texto += "\n";
            texto += "Expresiones Regulares: \n";
            for (int i = 0; i < Expresiones.getSize(); i++)
            {
                texto += "Nombre: " + Expresiones.obtenerNodo(i).getNombre() + "\n";
                for (int j = 0; j < Expresiones.obtenerNodo(i).getElementos().Count; j++)
                {
                    texto += Expresiones.obtenerNodo(i).getElementos()[j] + "\n";
                }
            }
            texto += "\n";
            texto += "Lexemas: \n";
            for (int i = 0; i < Lexemas.getSize(); i++)
            {
                texto += "Nombre: " + Lexemas.obtenerNodo(i).getNombre() + "\n";
                for (int j = 0; j < Lexemas.obtenerNodo(i).getElementos().Count; j++)
                {
                    texto += Lexemas.obtenerNodo(i).getElementos()[j] + "\n";
                }
            }
            texto += "\n";
            /*texto += "Lexemas: \n";
            for (int i = 0; i < Lexemas.getSize(); i++)
            {
                texto += Lexemas.obtenerNodo(i) + "\n";
            }*/
            return texto;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            // SE INICIA EL ANALISIS LEXICO Y SINTACTICO HASTA CIERTO PUNTO
            automata.analisisLexico(tabControl.SelectedTab.Controls.OfType<RichTextBox>().Reverse().FirstOrDefault().Text); // Analisis Lexico
            automata.analisisSintacto(); // Analisis sintactico no recibe parametros porque se hace despues del analis lexico
            Consola.Text = Output(automata.getListaConjuntos(), automata.getListaExpresiones(), automata.getListaLexemas());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*ArrayList p1 = new ArrayList();
            p1.Add(".");
            p1.Add(".");
            p1.Add(".");
            p1.Add("@");

            ArrayList p2 = new ArrayList();
            p2.Add("p");
            p2.Add("a");
            p2.Add("b");
            p2.Add("l");
            p2.Add("o");

            ArrayList p3 = new ArrayList();
            p3.Add("m");
            p3.Add("a");
            p3.Add("r");
            p3.Add("i");
            p3.Add("a");

            Lista_Simple lista = new Lista_Simple();
            lista.Insertar("Nodo1", p1);
            lista.Insertar("Nodo2", p2);
            lista.Insertar("Nodo3", p3);

            for (int i = 0; i < lista.getSize(); i ++) {
                Console.WriteLine("El nodo " + i + ", Se llama: " + lista.obtenerNodo(i).getNombre() + " y tiene: ");
                foreach(Object o in lista.obtenerNodo(i).getElementos()){
                    Console.Write(o);
                }
                Console.WriteLine();
            } */

            Thompson t1 = new Thompson();
            t1.Raiz = t1.Insertar("a");
            t1.graficarAFND("prueba");
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            contador_img1--;
            if (contador_img1 >= 0)
            {
                img1 = "A" + contador_img1 + ".png";
            }
            else
            {
                contador_img1++;
                img1 = "A" + contador_img1 + ".png";
            }
            pictureBox2.Image = Image.FromFile(@"C:\Users\Pablo Barillas\Desktop\Tablas_estados\" + img1);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
