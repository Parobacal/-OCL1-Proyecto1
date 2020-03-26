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
using Proyecto1_201602503.Lexemas;
using System.Xml;


namespace Proyecto1_201602503
{
    public partial class Form1 : Form
    {
        private List<ExpresionRegular> listaAutomatas = new List<ExpresionRegular>();
        public int contador_img = 0;
        public int contador_img1 = 0;
        public int contador_img2 = 0;
        public int contador_lexema = 0;
        public int contador_html = 0;
        public int contador_html_errores = 0;
        public string ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        public int fila;
        public int columna;
        public string img,img1, img2;
        public int totalImagen = 0;
        private bool analisis = false;
        private ArrayList arrayLexemas, arrayToken, arrayFila, arrayColumna, arrayError, arrayFilaError, arrayColumnaError;
        Automata automata = new Automata();
        public Form1()
        {
            InitializeComponent();
        }

        public void Inicializar()
        { // Este metodo inicializara los arreglos y variables que serviran para cada archivo xml

            this.fila = 0;
            this.columna = 0;
            this.arrayLexemas = new ArrayList();
            this.arrayToken = new ArrayList();
            this.arrayFila = new ArrayList();
            this.arrayColumna = new ArrayList();
            this.arrayError = new ArrayList();
            this.arrayFilaError = new ArrayList();
            this.arrayColumnaError = new ArrayList();

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
            if (contador_img < totalImagen + 1)
            {

                img = "A" + contador_img + ".png";
            }
            else {
                contador_img--;
            }
            pictureBox1.Image = Image.FromFile(@"C:\Users\Pablo Barillas\Desktop\AFND\" + img);
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
            pictureBox1.Image = Image.FromFile(@"C:\Users\Pablo Barillas\Desktop\AFND\" + img);
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
            if (contador_img1 < totalImagen + 1)
            {

                img1 = "TE_" + contador_img1 + ".png";
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
            contador_html++;
            if (analisis.Equals(true))
            {
                automata.reporteHtml(contador_html);
            }
            else
            {
                MessageBox.Show("No se ha analizado el archivo");
            }            
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
            analisis = true;
            // SE INICIA EL ANALISIS LEXICO Y SINTACTICO HASTA CIERTO PUNTO
            automata.analisisLexico(tabControl.SelectedTab.Controls.OfType<RichTextBox>().Reverse().FirstOrDefault().Text); // Analisis Lexico
            automata.analisisSintacto(); // Analisis sintactico no recibe parametros porque se hace despues del analis lexico
            Consola.Text = Output(automata.getListaConjuntos(), automata.getListaExpresiones(), automata.getListaLexemas());
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (analisis.Equals(true))
            {
                generarThomphson();
            }
            else
            {
                MessageBox.Show("No se ha analizado el archivo");
            }          

        }

        private void button1_Click(object sender, EventArgs e)
        {
            contador_img1--;
            if (contador_img1 >= 0)
            {
                img1 = "TE_" + contador_img1 + ".png";
            }
            else
            {
                contador_img1++;
                img1 = "TE_" + contador_img1 + ".png";
            }
            pictureBox2.Image = Image.FromFile(@"C:\Users\Pablo Barillas\Desktop\Tablas_estados\" + img1);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void generarErroresHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contador_html_errores++;
            // Metodo que genera el reporte HTML de errores para los archivos de entrada
            if (analisis.Equals(true))
            {
                automata.reporteHtmlErrores(contador_html_errores);
            }
            else
            {
                MessageBox.Show("No se ha analizado el archivo");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            contador_img2++;
            if (contador_img2 < totalImagen + 1)
            {

                img2 = "AFD" + contador_img2 + ".png";
            }
            else
            {
                contador_img2--;
            }
            pictureBox3.Image = Image.FromFile(@"C:\Users\Pablo Barillas\Desktop\AFD\" + img2);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            analizarLexemas();
        }


        public void analizarLexemas() 
        {

            string texto = "";
            // Por cada lexema
            for (int i = 0; i < automata.getListaLexemas().getSize(); i ++) {
                Inicializar();               
                // Por cada ER
                for (int j = 0; j < listaAutomatas.Count; j ++) {
                    // Si el nombre del lexema coincide con el de la ER
                    if (automata.getListaLexemas().obtenerNodo(i).getNombre().Equals(listaAutomatas[j].getNombreEr())) {
                        bool valido = false;
                        bool valido1 = validarLexema(automata.getListaLexemas().obtenerNodo(i).getElementos(), listaAutomatas[j].getAFD().getEstados()[0], valido, listaAutomatas[j].getAFD());
                        Console.WriteLine(valido);
                        // Por cada caracter del lexema
                        // Si el lexema es valido
                        if (valido1.Equals(true))
                        {
                            texto += "El lexema: " + automata.getListaLexemas().obtenerNodo(i).getNombre() + " SI es valido para la expresion regular: " + listaAutomatas[j].getNombreEr() + "\n";
                            generarXmlTokens(i);
                            Consola.Text = texto;
                        }
                        // Si el lexema no es valido
                        else 
                        {
                            texto += "El lexema: " + automata.getListaLexemas().obtenerNodo(i).getNombre() + " NO es valido para la expresion regular: " + listaAutomatas[j].getNombreEr() + "\n";
                            generarXmlTokensError(i);
                            Consola.Text = texto;
                        }                       
                    }
                }
                //
            }
        }

        public void generarXmlTokens(int num) {
            XmlDocument doc = new XmlDocument();
            XmlElement raiz = doc.CreateElement("ListaTokens");
            doc.AppendChild(raiz);
            for (int i = 0; i < arrayLexemas.Count; i ++) {
                XmlElement token = doc.CreateElement("Token");
                raiz.AppendChild(token);
                XmlElement nombre = doc.CreateElement("Nombre");
                nombre.AppendChild(doc.CreateTextNode(arrayToken[i].ToString()));
                token.AppendChild(nombre);
                XmlElement valor = doc.CreateElement("Valor");
                valor.AppendChild(doc.CreateTextNode(arrayLexemas[i].ToString()));
                token.AppendChild(valor);
                XmlElement fila = doc.CreateElement("Fila");
                fila.AppendChild(doc.CreateTextNode(arrayFila[i].ToString()));
                token.AppendChild(fila);
                XmlElement columna = doc.CreateElement("Columna");
                columna.AppendChild(doc.CreateTextNode(arrayColumna[i].ToString()));
                token.AppendChild(columna);
            }
            doc.Save(ruta + "\\Lexema_" + num + ".xml");            
        }
        public void generarXmlTokensError(int num)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement raiz = doc.CreateElement("ListaErrores");
            doc.AppendChild(raiz);
            for (int i = 0; i < arrayError.Count; i++)
            {
                XmlElement token = doc.CreateElement("Error");
                raiz.AppendChild(token);
                XmlElement valor = doc.CreateElement("Valor");
                valor.AppendChild(doc.CreateTextNode(arrayError[i].ToString()));
                token.AppendChild(valor);
                XmlElement fila = doc.CreateElement("Fila");
                fila.AppendChild(doc.CreateTextNode(arrayFilaError[i].ToString()));
                token.AppendChild(fila);
                XmlElement columna = doc.CreateElement("Columna");
                columna.AppendChild(doc.CreateTextNode(arrayColumnaError[i].ToString()));
                token.AppendChild(columna);
            }
            doc.Save(ruta + "\\Lexema_" + num + "_Error.xml");
        }
        private bool validarLexema(ArrayList Simbolos, Estado State, bool valido, AFND AFD) {
            // Si hay una transicion vacia
            for (int i = 0; i < AFD.getEstadosAceptacion().Count; i ++) {
                if ((Simbolos.Count == 0) && (AFD.getEstadoInicial().getIndice().Equals(AFD.getEstadosAceptacion()[i].getIndice()))) 
                {
                    valido = true;
                    return valido;
                }
            }
            columna++;
            if (contador_lexema == Simbolos.Count - 1)
            {
                // Por cada transicion del estado
                for (int i = 0; i < State.getTransiciones().Count; i++)
                {
                    // SI HAY ALGUN CONJUNTO
                    if (State.getTransiciones()[i].getSimbolo().Contains('{'))
                    {
                        string Conjunto = "";
                        // Por cada conjunto
                        for (int j = 0; j < automata.getListaConjuntos().getSize(); j++)
                        {
                            // Se asigna el nombre con el que se comparara con el conjunto de la transicion
                            Conjunto = "{" + automata.getListaConjuntos().obtenerNodo(j).getNombre() + "}";
                            // Si los nombres coinciden
                            if (Conjunto.Equals(State.getTransiciones()[i].getSimbolo()))
                            {
                                Console.WriteLine("Entre");
                                // Se convierte el simbolo en codigo ascii
                                char Caracter = (char)Simbolos[contador_lexema];
                                int codigoASCII = Caracter;
                                // Por cada caracter valido dentro del conjunto
                                for (int k = 0; k < automata.getListaConjuntos().obtenerNodo(j).getElementos().Count; k++)
                                {
                                    Console.WriteLine(Simbolos[contador_lexema].ToString());
                                    // Si es un salto de linea
                                    if (automata.getListaConjuntos().obtenerNodo(j).getElementos()[k].Equals("\\" + "n"))
                                    {
                                        if (codigoASCII.Equals(10))
                                        {
                                            for (int x = 0; x < AFD.getEstadosAceptacion().Count; x++)
                                            {
                                                if (State.getTransiciones()[i].getEstadoFinal().getIndice().Equals(AFD.getEstadosAceptacion()[x].getIndice()))
                                                {
                                                    fila++;
                                                    arrayLexemas.Add(Simbolos[contador_lexema]);
                                                    arrayFila.Add(fila);
                                                    arrayColumna.Add(columna);
                                                    arrayToken.Add("Tk_Caracter_Especial");
                                                    valido = true;
                                                    contador_lexema = 0;
                                                    columna = 0;
                                                    return valido;
                                                }
                                            }
                                            arrayError.Add(Simbolos[contador_lexema]);
                                            arrayFilaError.Add(fila);
                                            arrayColumnaError.Add(columna);
                                            valido = false;
                                            contador_lexema = 0;
                                            return valido;
                                        }
                                        
                                    }
                                    // Si es una comilla simple
                                    else if (automata.getListaConjuntos().obtenerNodo(j).getElementos()[k].Equals("\\" + "'"))
                                    {
                                        if (codigoASCII.Equals(39))
                                        {
                                            for (int x = 0; x < AFD.getEstadosAceptacion().Count; x++)
                                            {
                                                if (State.getTransiciones()[i].getEstadoFinal().getIndice().Equals(AFD.getEstadosAceptacion()[x].getIndice()))
                                                {
                                                    arrayLexemas.Add(Simbolos[contador_lexema]);
                                                    arrayFila.Add(fila);
                                                    arrayColumna.Add(columna);
                                                    arrayToken.Add("Tk_Caracter_Especial");
                                                    valido = true;
                                                    contador_lexema = 0;
                                                    return valido;
                                                }
                                            }
                                            arrayError.Add(Simbolos[contador_lexema]);
                                            arrayFilaError.Add(fila);
                                            arrayColumnaError.Add(columna);
                                            valido = false;
                                            contador_lexema = 0;
                                            return valido;
                                        }
                                        
                                    }
                                    // Si es una comilla doble
                                    else if (automata.getListaConjuntos().obtenerNodo(j).getElementos()[k].Equals("\\" + "\""))
                                    {
                                        if (codigoASCII.Equals(34))
                                        {
                                            for (int x = 0; x < AFD.getEstadosAceptacion().Count; x++)
                                            {
                                                if (State.getTransiciones()[i].getEstadoFinal().getIndice().Equals(AFD.getEstadosAceptacion()[x].getIndice()))
                                                {
                                                    arrayLexemas.Add(Simbolos[contador_lexema]);
                                                    arrayFila.Add(fila);
                                                    arrayColumna.Add(columna);
                                                    arrayToken.Add("Tk_Caracter_Especial");
                                                    valido = true;
                                                    contador_lexema = 0;
                                                    return valido;
                                                }
                                            }
                                            arrayError.Add(Simbolos[contador_lexema]);
                                            arrayFilaError.Add(fila);
                                            arrayColumnaError.Add(columna);
                                            valido = false;
                                            contador_lexema = 0;
                                            return valido;
                                        }
                                        
                                    }
                                    // Si es una tabulacion
                                    else if (automata.getListaConjuntos().obtenerNodo(j).getElementos()[k].Equals("\\" + "t"))
                                    {
                                        if (codigoASCII.Equals(09))
                                        {
                                            for (int x = 0; x < AFD.getEstadosAceptacion().Count; x++)
                                            {
                                                if (State.getTransiciones()[i].getEstadoFinal().getIndice().Equals(AFD.getEstadosAceptacion()[x].getIndice()))
                                                {
                                                    arrayLexemas.Add(Simbolos[contador_lexema]);
                                                    arrayFila.Add(fila);
                                                    arrayColumna.Add(columna);
                                                    arrayToken.Add("Tk_Caracter_Especial");
                                                    valido = true;
                                                    contador_lexema = 0;
                                                    return valido;
                                                }
                                            }
                                            arrayError.Add(Simbolos[contador_lexema]);
                                            arrayFilaError.Add(fila);
                                            arrayColumnaError.Add(columna);
                                            valido = false;
                                            contador_lexema = 0;
                                            return valido;
                                        }
                                        
                                    }
                                    // Si hay alguna coincidencia
                                    else if (Simbolos[contador_lexema].ToString().Equals(automata.getListaConjuntos().obtenerNodo(j).getElementos()[k].ToString()))
                                    {
                                        for (int x = 0; x < AFD.getEstadosAceptacion().Count; x++)
                                        {
                                            if (State.getTransiciones()[i].getEstadoFinal().getIndice().Equals(AFD.getEstadosAceptacion()[x].getIndice()))
                                            {
                                                arrayLexemas.Add(Simbolos[contador_lexema]);
                                                arrayFila.Add(fila);
                                                arrayColumna.Add(columna);
                                                arrayToken.Add("Tk_Conjunto_" + automata.getListaConjuntos().obtenerNodo(j).getNombre());
                                                valido = true;
                                                contador_lexema = 0;
                                                return valido;
                                            }
                                        }
                                        arrayError.Add(Simbolos[contador_lexema]);
                                        arrayFilaError.Add(fila);
                                        arrayColumnaError.Add(columna);
                                        valido = false;
                                        contador_lexema = 0;
                                        return valido;
                                    }
                                }
                            }
                        }
                    }
                    // Si NO ES UN CONJUNTO
                    else
                    {
                        // Si el simbolo coincide
                        Console.WriteLine(Simbolos[contador_lexema] + " " + State.getTransiciones()[i].getSimbolo());
                        if (Simbolos[contador_lexema].ToString().Equals(State.getTransiciones()[i].getSimbolo()))
                        {
                            for (int x = 0; x < AFD.getEstadosAceptacion().Count; x ++)
                            {
                                if (State.getTransiciones()[i].getEstadoFinal().getIndice().Equals(AFD.getEstadosAceptacion()[x].getIndice()))
                                {
                                    arrayLexemas.Add(Simbolos[contador_lexema]);
                                    arrayFila.Add(fila);
                                    arrayColumna.Add(columna);
                                    arrayToken.Add("Tk_Simbolo");
                                    valido = true;
                                    contador_lexema = 0;
                                    return valido;
                                }
                            }
                            arrayError.Add(Simbolos[contador_lexema]);
                            arrayFilaError.Add(fila);
                            arrayColumnaError.Add(columna);
                            valido = false;
                            contador_lexema = 0;
                            return valido;
                        }
                    }
                }
                arrayError.Add(Simbolos[contador_lexema]);
                arrayFilaError.Add(fila);
                arrayColumnaError.Add(columna);
                valido = false;
                //return valido;
            }
            else 
            {
                if (State.getTransiciones().Any())
                {

                    // Por cada transicion del estado
                    for (int i = 0; i < State.getTransiciones().Count; i++)
                    {
                        // SI HAY ALGUN CONJUNTO
                        if (State.getTransiciones()[i].getSimbolo().Contains('{'))
                        {
                            string Conjunto = "";
                            // Por cada conjunto
                            for (int j = 0; j < automata.getListaConjuntos().getSize(); j++)
                            {
                                // Se asigna el nombre con el que se comparara con el conjunto de la transicion
                                Conjunto = "{" + automata.getListaConjuntos().obtenerNodo(j).getNombre() + "}";
                                // Si los nombres coinciden
                                if (Conjunto.Equals(State.getTransiciones()[i].getSimbolo()))
                                {
                                    Console.WriteLine("Entre");
                                    // Se convierte el simbolo en codigo ascii
                                    char Caracter = (char)Simbolos[contador_lexema];
                                    int codigoASCII = Caracter;
                                    // Por cada caracter valido dentro del conjunto
                                    for (int k = 0; k < automata.getListaConjuntos().obtenerNodo(j).getElementos().Count; k++)
                                    {
                                        Console.WriteLine(Simbolos[contador_lexema].ToString());
                                        // Si es un salto de linea
                                        if (automata.getListaConjuntos().obtenerNodo(j).getElementos()[k].Equals("\\" + "n"))
                                        {
                                            if (codigoASCII.Equals(10))
                                            {
                                                fila++;
                                                arrayLexemas.Add(Simbolos[contador_lexema]);
                                                arrayFila.Add(fila);
                                                arrayColumna.Add(columna);
                                                arrayToken.Add("Tk_Caracter_Especial");
                                                valido = true;
                                                contador_lexema++;
                                                columna = 0;
                                                Estado siguienteEstado = AFD.getEstados()[State.getTransiciones()[i].getEstadoFinal().getIndice()];
                                                return validarLexema(Simbolos, siguienteEstado, valido, AFD);
                                            }
                                            
                                        }
                                        // Si es una comilla simple
                                        else if (automata.getListaConjuntos().obtenerNodo(j).getElementos()[k].Equals("\\" + "'"))
                                        {
                                            if (codigoASCII.Equals(39))
                                            {
                                                arrayLexemas.Add(Simbolos[contador_lexema]);
                                                arrayFila.Add(fila);
                                                arrayColumna.Add(columna);
                                                arrayToken.Add("Tk_Caracter_Especial");
                                                valido = true;
                                                contador_lexema++;
                                                Estado siguienteEstado = AFD.getEstados()[State.getTransiciones()[i].getEstadoFinal().getIndice()];
                                                return validarLexema(Simbolos, siguienteEstado, valido, AFD);
                                            }
                                            
                                        }
                                        // Si es una comilla doble
                                        else if (automata.getListaConjuntos().obtenerNodo(j).getElementos()[k].Equals("\\" + "\""))
                                        {
                                            if (codigoASCII.Equals(34))
                                            {
                                                arrayLexemas.Add(Simbolos[contador_lexema]);
                                                arrayFila.Add(fila);
                                                arrayColumna.Add(columna);
                                                arrayToken.Add("Tk_Caracter_Especial");
                                                valido = true;
                                                contador_lexema++;
                                                Estado siguienteEstado = AFD.getEstados()[State.getTransiciones()[i].getEstadoFinal().getIndice()];
                                                return validarLexema(Simbolos, siguienteEstado, valido, AFD);
                                            }
                                            
                                        }
                                        // Si es una tabulacion
                                        else if (automata.getListaConjuntos().obtenerNodo(j).getElementos()[k].Equals("\\" + "t"))
                                        {
                                            if (codigoASCII.Equals(09))
                                            {
                                                arrayLexemas.Add(Simbolos[contador_lexema]);
                                                arrayFila.Add(fila);
                                                arrayColumna.Add(columna);
                                                arrayToken.Add("Tk_Caracter_Especial");
                                                valido = true;
                                                contador_lexema++;
                                                Estado siguienteEstado = AFD.getEstados()[State.getTransiciones()[i].getEstadoFinal().getIndice()];
                                                return validarLexema(Simbolos, siguienteEstado, valido, AFD);
                                            }
                                            
                                        }
                                        // Si hay alguna coincidencia
                                        else if (Simbolos[contador_lexema].ToString().Equals(automata.getListaConjuntos().obtenerNodo(j).getElementos()[k].ToString()))
                                        {
                                            arrayLexemas.Add(Simbolos[contador_lexema]);
                                            arrayFila.Add(fila);
                                            arrayColumna.Add(columna);
                                            arrayToken.Add("Tk_Conjunto_" + automata.getListaConjuntos().obtenerNodo(j).getNombre());
                                            valido = true;
                                            contador_lexema++;
                                            Estado siguienteEstado = AFD.getEstados()[State.getTransiciones()[i].getEstadoFinal().getIndice()];
                                            return validarLexema(Simbolos, siguienteEstado, valido, AFD);
                                        }
                                    }
                                }
                            }
                        }
                        // Si NO ES UN CONJUNTO
                        else
                        {
                            if (Simbolos[contador_lexema].ToString().Equals(State.getTransiciones()[i].getSimbolo()))
                            {
                                arrayLexemas.Add(Simbolos[contador_lexema]);
                                arrayFila.Add(fila);
                                arrayColumna.Add(columna);
                                arrayToken.Add("Tk_Simbolo");
                                valido = true;
                                contador_lexema++;
                                Estado siguienteEstado = AFD.getEstados()[State.getTransiciones()[i].getEstadoFinal().getIndice()];
                                return validarLexema(Simbolos, siguienteEstado, valido, AFD);
                            }
                        }
                    }
                    arrayError.Add(Simbolos[contador_lexema]);
                    arrayFilaError.Add(fila);
                    arrayColumnaError.Add(columna);
                    valido = false;
                }
                else 
                {
                    arrayError.Add(Simbolos[contador_lexema]);
                    arrayFilaError.Add(fila);
                    arrayColumnaError.Add(columna);
                    valido = false;
                }
            }                      
            contador_lexema = 0;
            return valido;                 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            contador_img2--;
            if (contador_img2 >= 0)
            {
                img2 = "AFD" + contador_img2 + ".png";
            }
            else
            {
                contador_img2++;
                img2 = "AFD" + contador_img2 + ".png";
            }
            pictureBox3.Image = Image.FromFile(@"C:\Users\Pablo Barillas\Desktop\AFD\" + img2);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;

        }

        private void generarThomphson() {

            // Se genera una nueva lista que contendra todos los thompson a generar
            List<Thompson> listaThompshon = new List<Thompson>();
            // Se genera una lista que contendra todos los AFD que se formen para valuar los lexemas posteriormente
            List<ExpresionRegular> listaAFD = new List<ExpresionRegular>();
            // Se recorre la lista que tiene las exp regulares
            for (int i = 0; i < automata.getListaExpresiones().getSize(); i ++) {
                // Se genera una instancia nueva del metodo por cada expresion o nodo de la lista de expresiones
                Thompson nuevoThompson = new Thompson();
                // Tambien genero una nueva instancia de AFD para definir el nombre de la ER al que correspondera
                ExpresionRegular nuevoAFD = new ExpresionRegular();
                // Se recorre cada elemento del atributo lista de cada nodo de la lista de exp reg
                for (int j = 0; j < automata.getListaExpresiones().obtenerNodo(i).getElementos().Count; j ++) {
                    // Se inserta en la lista de er propia del metodo cada elemento obtenido del recorrido
                    nuevoThompson.ER.Add(automata.getListaExpresiones().obtenerNodo(i).getElementos()[j]);
                    // Se agrega el nombre de la er al AFD creado
                    nuevoAFD.setNombreEr(automata.getListaExpresiones().obtenerNodo(i).getNombre());
                }
                // Se envia a generar y graficar el metodo
                nuevoThompson.Raiz = nuevoThompson.Insertar();
                // Se guarda en la lista el nuevo thompshon
                listaThompshon.Add(nuevoThompson);
                // Se guarda en la lista de AFD la nueva instancia de AFD
                listaAFD.Add(nuevoAFD);
                // Se guarda el total de imagenes creadas
                totalImagen = i;
            }

            for (int i = 0; i < listaThompshon.Count; i ++) {

                // Se genera cada metodo del thompson
                listaThompshon[i].graficarAFND("A" + i);
                listaThompshon[i].generarAFD("TE_" + i);
                listaThompshon[i].graficarAFD("AFD" + i);
                // Se agrega el AFD correspondiente al nombre que se le asigno
                listaAFD[i].setAFD(listaThompshon[i].AFD);
               
            }

            // Asigno la lista creada a la de la clase
            listaAutomatas = listaAFD;
        
        }





    }
}
