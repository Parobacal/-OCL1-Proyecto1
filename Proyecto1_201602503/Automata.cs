using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

using Proyecto1_201602503.Estructuras;

namespace Proyecto1_201602503
{
    class Automata
    {
        // ---------------Atributos de la clase automata---------------

        private string lexema; // String para guardar y concatenar cada lexema leido
        private int fila, columna; // Contadores para poder identificar la fila y la columna
        private ArrayList arrayLexemas, arrayToken, arrayFila, arrayColumna, arrayError, arrayFilaError, arrayColumnaError, arrayExp, arrayLex, arrayConj; // Arreglos para guardar toda la lista de simbolos
        private Lista_Simple listaConjuntos, listaLexemas, listaExpresiones; // Listas para guardar los valores que nos dan en el archivo de entrada
       
        // ---------------Constructor de la clase automata---------------
        public Automata(){  
            // Solamente se inicializan estos arreglos y listas una vez que se inicia la ejecucion del programa
            this.arrayConj = new ArrayList();
            this.arrayExp = new ArrayList();
            this.arrayLex = new ArrayList();
            this.listaConjuntos = new Lista_Simple();
            this.listaLexemas = new Lista_Simple();
            this.listaExpresiones = new Lista_Simple();
        }

        // ---------------Metodos de la clase automata---------------

        public void Inicializar() { // Este metodo inicializara los arreglos y variables que serviran por cada llamada del metodo analis lexico y sintactico

            this.lexema = "";
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
        public void analisisLexico(string archivoTexto) {
            Inicializar();
            int estado = 0; // Variable para indicar el numero de estado al que se dirigira cada caracter
            int restColumna = 0; // Variable para restar al numero de columna en caso de ser un token compuesto por varios simbolos
            for (int i = 0; i < archivoTexto.Length; i++) { // Ciclo para recorrer todo el archivo
                columna += 1; // Se inicia el conteo de columnas
                char simbolo = archivoTexto[i];
                int codigoASCII = simbolo; // Se asigna el caracter a una variable para comparar en cada estado con el codigo ASCII
                switch (estado) {
                    case 0:
                        if (codigoASCII == 60){ // Si el simbolo es <
                            estado = 1;
                            lexema = "" + simbolo;
                            //Console.WriteLine("Encontro <");
                        }
                        else if (codigoASCII == 47) // Si el simbolo es /
                        { 
                            estado = 2;
                            lexema = "" + simbolo;
                        }
                        else if (((codigoASCII >= 65) && (codigoASCII <= 90)) || ((codigoASCII >= 97) && (codigoASCII <= 122))){ // Si el simbolo es una letra
                            estado = 3;
                            lexema = "" + simbolo;
                        }
                        else if (codigoASCII == 34){ // Si el simbolo es "                      
                            estado = 4;
                            lexema = "" + simbolo;
                        }
                        else if ((codigoASCII >= 48) && (codigoASCII <= 57)) { // Si el simbolo es un numero
                            estado = 0;
                            arrayLexemas.Add(simbolo);
                            arrayToken.Add("Tk_Numero");
                            arrayColumna.Add(columna);
                            arrayFila.Add(fila);
                            lexema = "";
                        }
                        else if ((codigoASCII == 32)|| (codigoASCII == 33)||((codigoASCII >= 35)&&(codigoASCII <= 46))||((codigoASCII >= 58) && (codigoASCII <= 59)) || ((codigoASCII >= 61) && (codigoASCII <= 64)) || ((codigoASCII >= 91) && (codigoASCII <= 96)) || ((codigoASCII >= 123) && (codigoASCII <= 126))) // Si el simbolo es considerado un simbolo
                        {
                            if (codigoASCII == 32)
                            {
                                estado = 0;
                            }
                            else
                            {
                                estado = 0;
                                arrayLexemas.Add(simbolo);
                                arrayToken.Add("Tk_Simbolo");
                                arrayColumna.Add(columna);
                                arrayFila.Add(fila);
                                lexema = "";
                            }
                        }
                        else if (codigoASCII == 10) //Si es un salto de linea
                        {
                            fila += 1;
                            columna = 0;
                        }
                        else { // Si no es un simbolo permitido en el lenguaje
                            arrayError.Add("" + simbolo);
                            arrayColumnaError.Add(columna);
                            arrayFilaError.Add(fila);
                        }
                        break;
                    case 1:
                        if (codigoASCII == 33) { // Si es igual a !
                            estado = 5;
                            lexema += simbolo;
                            //Console.WriteLine("Encontro !");
                        }
                        else {
                            if (codigoASCII == 10) { // Si es igual a salto de linea
                                fila += 1;
                                columna = 0;
                            }
                            arrayLexemas.Add(lexema);
                            //falta agregar un simbolo
                            arrayToken.Add("Tk_Simbolo");
                            arrayFila.Add(fila);
                            arrayColumna.Add(columna);
                            lexema = "";

                            arrayLexemas.Add(simbolo);
                            //falta agregar un simbolo
                            arrayToken.Add("Tk_Simbolo");
                            arrayFila.Add(fila);
                            arrayColumna.Add(columna + 1);
                            estado = 0;
                            //Console.WriteLine("Encontro /n");
                        }
                        break;
                    case 2:
                        if (codigoASCII == 47) // Si el simbolo es /
                        { 
                            estado = 7;
                            lexema += simbolo;
                        }
                        else
                        {
                            if (codigoASCII == 10)
                            { // Si es igual a salto de linea
                                fila += 1;
                            }
                            
                            arrayLexemas.Add(lexema);
                            arrayToken.Add("Tk_Simbolo");
                            arrayFila.Add(fila);
                            arrayColumna.Add(columna);

                            arrayLexemas.Add(simbolo);
                            arrayToken.Add("Tk_Simbolo");
                            arrayFila.Add(fila);
                            arrayColumna.Add(columna + 1);

                            estado = 0;
                        }
                        break;
                    case 3:
                        if (((codigoASCII >= 65) && (codigoASCII <= 90)) || ((codigoASCII >= 97) && (codigoASCII <= 122)) || ((codigoASCII >= 48) && (codigoASCII <= 57)) || (codigoASCII == 95)) // Si el simbolo es una letra, numero o _
                        {
                            estado = 3;
                            restColumna += 1;
                            lexema += simbolo;
                        }
                        else if (((codigoASCII >= 32) && (codigoASCII <= 33)) || ((codigoASCII >= 35) && (codigoASCII <= 46)) || ((codigoASCII >= 58) && (codigoASCII <= 59)) || ((codigoASCII >= 61) && (codigoASCII <= 64)) || ((codigoASCII >= 91) && (codigoASCII <= 94)) || (codigoASCII == 96) || ((codigoASCII >= 123) && (codigoASCII <= 126))) // Si es un simbolo
                        {
                            columna = columna - restColumna; // Resto el valor de restColumna a columna para definir la columna en un solo token
                            arrayLexemas.Add(lexema);
                            arrayToken.Add("Tk_Id");
                            arrayColumna.Add(columna);
                            arrayFila.Add(fila);
                            restColumna = 0; // Inicializo la cuenta para otro token
                            estado = 0;
                            if (codigoASCII != 32)
                            {
                                columna++;
                                arrayLexemas.Add(simbolo);
                                arrayToken.Add("Tk_simbolo");
                                arrayColumna.Add(columna);
                                arrayFila.Add(fila);
                            }
                        }
                        break;
                    case 4:
                        if (codigoASCII == 34)
                        {
                            lexema += simbolo;
                            columna = columna - restColumna; // Resto el valor de restColumna a columna para definir la columna en un solo token
                            arrayLexemas.Add(lexema);
                            arrayToken.Add("Tk_Lexema");
                            arrayColumna.Add(columna);
                            arrayFila.Add(fila);
                            restColumna = 0; // Inicializo la cuenta para otro token
                            estado = 0;
                        }
                        else if (codigoASCII == 59) {
                            arrayLexemas.Add(lexema);
                            arrayToken.Add("Tk_Simbolo");
                            arrayColumna.Add(columna);
                            arrayFila.Add(fila);

                            arrayLexemas.Add(simbolo);
                            arrayToken.Add("Tk_Simbolo");
                            arrayColumna.Add(columna + 1);
                            arrayFila.Add(fila);
                            estado = 0;
                        }
                        else
                        {
                            estado = 4;
                            restColumna += 1;
                            lexema += simbolo;
                        }
                        break;
                    case 5:
                        if (codigoASCII == 33) // Si el simbolo es !
                        {
                            estado = 6;
                            lexema += simbolo;
                            //Console.WriteLine("Encontro !");
                        }
                        else // Si el simbolo es cualquier otro del lenguaje
                        {
                            estado = 5;
                            lexema += simbolo;
                            //Console.WriteLine("Encontro algun otro simbolo");
                        }
                        break;
                    case 6:
                        if (codigoASCII == 62) // Si el simbolo es >
                        {
                            estado = 0;
                            lexema += simbolo;
                            columna = columna - restColumna;
                            arrayLexemas.Add(lexema);
                            arrayToken.Add("Tk_CM");
                            arrayColumna.Add(columna);
                            arrayFila.Add(fila);
                            lexema = "";
                            restColumna = 0;
                            //Console.WriteLine("Encontro >");
                        }
                        else if (simbolo == 33) // Si el simbolo es !
                        {
                            estado = 6;
                            restColumna += 1;
                            lexema += simbolo;
                            //Console.WriteLine("Encontro !");
                        }
                        else
                        {
                            if (codigoASCII == 10) { // Si hay un salto de linea 
                                fila += 1;
                                columna = 0;
                            }
                            restColumna += 1;
                            estado = 5;
                            lexema += simbolo;
                            //Console.WriteLine("Encontro CM");
                        }
                        break;
                    case 7:
                        //FINAL DE COMENTARIO
                        if (codigoASCII == 10)
                        {
                            estado = 0;
                            columna = columna - restColumna;
                            arrayLexemas.Add(lexema);
                            arrayToken.Add("Tk_C");
                            arrayColumna.Add(columna);
                            arrayFila.Add(fila);
                            lexema = "";
                            restColumna = 0;
                        }
                        else {
                            restColumna += 1;
                            estado = 7;
                            lexema += simbolo;
                        }
                        break;
                }
            }
            

        }

        public void analisisSintacto(){
            int estado = 0;
            string lexema = "";
            for (int i = 0; i < arrayLexemas.Count; i++) {
                switch (estado) {
                    case 0:
                        if (arrayLexemas[i].Equals("CONJ"))
                        {
                            estado = 1;
                            lexema = "" + arrayLexemas[i];
                           // Console.WriteLine(lexema);
                        }
                        else if (arrayToken[i].Equals("Tk_Id")){
                            //Console.WriteLine("Encontro Id");
                            estado = 2;
                            lexema = "" + arrayLexemas[i];
                        }
                        break;
                    case 1:
                        if (arrayLexemas[i].Equals(';'))
                        {
                            estado = 0;
                            lexema += arrayLexemas[i];
                            

                            arrayConj.Add(lexema); // Se agregan los conjuntos
                            char c = 'c';
                            Asignar(lexema, c); // Asignamos los conjuntos correctamente
                            lexema = "";
                        }
                        else {
                            estado = 1;
                            //Console.WriteLine(lexema);
                            lexema += arrayLexemas[i];
                        }
                        break;
                    case 2:
                        if (arrayLexemas[i].Equals('-')) {
                            estado = 3;
                            lexema += arrayLexemas[i];
                            //Console.WriteLine(lexema);
                        }
                        else if (arrayLexemas[i].Equals(':')) { 
                            estado = 5;
                            lexema += arrayLexemas[i];
                            //Console.WriteLine(lexema);
                        }
                        break;
                    case 3:
                        if (arrayLexemas[i].Equals('>'))
                        {
                            estado = 4;
                            lexema += arrayLexemas[i];
                            //Console.WriteLine(lexema);
                        }
                        else {
                            estado = 0;
                            lexema = "";
                        }   
                        break;
                    case 4:
                        if (arrayLexemas[i].Equals(';'))
                        {
                            estado = 0;
                            lexema += arrayLexemas[i];
                            arrayExp.Add(lexema); // Se agrega la expresion regular
                            char c = 'e';
                            Asignar(lexema, c); // Asignamos las expresiones correctamente
                            lexema = "";
                            
                        }
                        else {
                            estado = 4;
                            lexema += arrayLexemas[i];
                            //Console.WriteLine(lexema);
                        }
                        break;
                    case 5:
                        if (arrayLexemas[i].Equals(';'))
                        {
                            estado = 0;
                            lexema += arrayLexemas[i];
                            //Console.WriteLine(lexema);
                            arrayLex.Add(lexema); // Se agrega el lexema
                            char c = 'l';
                            Asignar(lexema, c); // Asignamos los lexemas correctamente
                            lexema = "";
                            
                        }
                        else
                        {
                            estado = 5;
                            lexema += arrayLexemas[i];
                        }
                        break;
            }

            }
        }


        public void Asignar(string lexema, char Tipo) {
            string nombre;
            ArrayList elementos;
            if (Tipo.Equals('c'))
            {
                nombre = "";
                char primera_letra = ' ';
                char letra_extra = ' ';
                string lex = "";
                string especial = "";
                elementos = new ArrayList();
                // Asignamos los conjuntos:
                int estado = 0;
                for (int i = 0; i < lexema.Length; i++)
                {
                    char simbolo = lexema[i];
                    switch (estado)
                    {
                        case 0:
                            if (simbolo.Equals(':'))
                            {
                                estado = 1;
                            }
                            else if (simbolo.Equals('>'))
                            {
                                estado = 2;
                            }
                            else
                            {
                                estado = 0;
                            }
                            break;
                        case 1:
                            if (simbolo.Equals('-'))
                            {
                                estado = 0;
                            }
                            else
                            {
                                nombre += simbolo;
                                estado = 1;
                            }
                            break;
                        case 2:
                            int codigoASCII = simbolo;
                            if (simbolo.Equals(','))
                            {
                                estado = 3;
                                elementos.Add(primera_letra);
                            }
                            else if (simbolo.Equals('~'))
                            {
                                estado = 4;
                            }
                            else if (simbolo.Equals('['))
                            {
                                estado = 5;
                                primera_letra = simbolo;
                            }
                            else if (codigoASCII.Equals(92)) // \
                            {
                                estado = 8;
                                primera_letra = simbolo;
                            }
                            else
                            {
                                primera_letra = simbolo;
                                //elementos.Add(simbolo);
                                estado = 2;
                            }
                            break;
                        case 3:
                            if (simbolo.Equals(','))
                            {
                                //Console.WriteLine("Llegue aqui");
                                estado = 3;
                            }
                            else if (simbolo.Equals(';'))
                            {
                                listaConjuntos.Insertar(nombre, elementos); // Inserto el conjunto
                                nombre = "";
                                primera_letra = ' ';
                                lexema = "";
                            }
                            else
                            {
                                elementos.Add(simbolo);
                                estado = 3;
                            }
                            break;
                        case 4:
                            //Console.WriteLine(simbolo);
                            //Console.WriteLine("Estado 4");
                            int codigoASCII_U = simbolo;
                            int codigoASCII_P = primera_letra;
                            //Console.WriteLine(codigoASCII_P);
                            //Console.WriteLine(codigoASCII_U);
                            for (int j = codigoASCII_P; j <= codigoASCII_U; j++)
                            {
                                char c = (char)j;
                                elementos.Add(c);
                            }
                            listaConjuntos.Insertar(nombre, elementos); // Inserto el conjunto
                            estado = 0;
                            break;
                        case 5:
                            if (simbolo.Equals('~'))
                            {
                                estado = 4;
                            }
                            else if (simbolo.Equals(',')) 
                            {
                                estado = 3;
                                elementos.Add(primera_letra);
                            }
                            else
                            {
                                estado = 6;
                            }
                            break;
                        case 6:
                            if (simbolo.Equals(':'))
                            {
                                estado = 7;
                                letra_extra = simbolo;
                            }
                            else
                            {
                                estado = 6;
                                elementos.Add(simbolo);
                                lex += simbolo;
                                
                            }
                            break;
                        case 7:
                            if (simbolo.Equals(']'))
                            {
                                
                                estado = 0;
                                listaConjuntos.Insertar(nombre, elementos); // inserto el conjunto
                                letra_extra = ' ';
                                lex = "";
                                nombre = "";
                            }
                            else if (simbolo.Equals(':'))
                            {
                                estado = 7;
                                lex += letra_extra;
                            }
                            else
                            {
                                estado = 6;
                                lex += simbolo;
                                elementos.Add(simbolo);
                            }
                            break;
                        case 8:
                            if (simbolo.Equals('~'))
                            {
                                estado = 4;
                            }
                            else
                            {
                                if (i == lexema.Length - 2)
                                {
                                    string cadena = "";
                                    cadena += primera_letra;
                                    cadena += simbolo;
                                    elementos.Add(cadena);
                                    listaConjuntos.Insertar(nombre, elementos); // inserto el conjunto
                                    cadena = "";
                                    nombre = "";
                                    primera_letra = ' ';
                                    estado = 0;
                                }
                                else {
                                    estado = 9;
                                    especial += primera_letra;
                                    especial += simbolo;
                                    //elementos.Add(especial);
                                    //especial = "";
                                }
                                
                                /*string cadena = "";
                                cadena += primera_letra;
                                cadena += simbolo;
                                elementos.Add(cadena);
                                listaConjuntos.Insertar(nombre, elementos); // inserto el conjunto
                                cadena = "";
                                nombre = "";
                                primera_letra = ' ';
                                estado = 0;*/
                            }
                            break;
                        case 9:
                            if (simbolo.Equals(','))
                            {
                                estado = 9;
                                elementos.Add(especial);
                                especial = "";
                                primera_letra = ' ';
                            }
                            else 
                            {
                                especial += simbolo;
                                if (i == lexema.Length - 2)
                                {

                                    estado = 0;
                                    elementos.Add(especial);
                                    listaConjuntos.Insertar(nombre, elementos); // inserto el conjunto
                                    especial = "";
                                    nombre = "";
                                    primera_letra = ' ';
                                    
                                }
                                else 
                                {
                                    estado = 9;
                                    primera_letra = ' ';
                                }
                            }
                            break;
                    }
                }
            }
            else if (Tipo.Equals('e'))
            {
                // Asignamos las expresiones regulares:
                nombre = "";
                elementos = new ArrayList();
                string lex = "";
                int estado = 0;
                for (int i = 0; i < lexema.Length; i++)
                {
                    char simbolo = lexema[i];
                    switch (estado)
                    {
                        case 0:
                            if (simbolo.Equals('-'))
                            {
                                estado = 1;
                            }
                            else
                            {
                                nombre += simbolo;
                            }
                            break;
                        case 1:
                            estado = 2;
                            break;
                        case 2:
                            if (simbolo.Equals('{'))
                            {
                                estado = 3;
                                lex += simbolo;
                            }
                            else if (simbolo.Equals('"')) 
                            {
                                estado = 3;
                            }
                            else if (simbolo.Equals(' '))
                            {
                                estado = 2;
                            }
                            else if (simbolo.Equals(';'))
                            {
                                listaExpresiones.Insertar(nombre, elementos);
                                estado = 0;
                            }
                            else
                            {
                                estado = 2;
                                elementos.Add(simbolo);
                            }
                            break;
                        case 3:
                            if (simbolo.Equals('}'))
                            {
                                lex += simbolo;
                                elementos.Add(lex);
                                lex = "";
                                estado = 2;
                            }
                            else if (simbolo.Equals('"')) 
                            {
                                elementos.Add(lex);
                                lex = "";
                                estado = 2;
                            }
                            else
                            {
                                lex += simbolo;
                                estado = 3;
                            }
                            break;
                    }
                }
            }
            else
            {
                // Asignamos los lexemas:
                nombre = "";
                elementos = new ArrayList();
                //string lex = "";
                int estado = 0;
                for (int i = 0; i < lexema.Length; i++)
                {
                    char simbolo = lexema[i];
                    switch (estado)
                    {
                        case 0:
                            if (simbolo.Equals(':')) {
                                estado = 1;
                            } else {
                                nombre += simbolo;
                                estado = 0;
                            }
                            break;
                        case 1:
                            estado = 2;
                            break;
                        case 2:
                            if (simbolo.Equals('"'))
                            {
                                //Console.WriteLine("si llegue");
                                estado = 0;
                                listaLexemas.Insertar(nombre, elementos);
                                nombre = "";
                            }
                            else
                            {                
                                //Aqui podemos definir si guarda caracter por caracter o todo en un solo string
                                estado = 2;
                                elementos.Add(simbolo);

                            }
                            break;

                    }
                }
            }

        }


        // ---------------Metodos getters---------------
        public ArrayList getArrayToken() {
            return arrayToken;
        }
        public ArrayList getArrayLexemas()
        {
            return arrayLexemas;
        }
        public ArrayList getArrayFila()
        {
            return arrayFila;
        }
        public ArrayList getArrayColumna()
        {
            return arrayColumna;
        }

        public ArrayList getArrayConj()
        {
            return arrayConj;
        }
        public ArrayList getArrayExp()
        {
            return arrayExp;
        }
        public ArrayList getArrayLex()
        {
            return arrayLex;
        }

        public Lista_Simple getListaConjuntos()
        {
            return listaConjuntos;
        }
        public Lista_Simple getListaExpresiones()
        {
            return listaExpresiones;
        }
        public Lista_Simple getListaLexemas()
        {
            return listaLexemas;
        }


        // Metodos para los reportes
        public void reporteHtml(int num) {

            StreamWriter sw = new StreamWriter(@"C:\Users\Pablo Barillas\Desktop\Rep_Token" + num + ".html");
            string cadena = "";
            cadena += "<TABLE BORDER='5'    WIDTH='50 % '   CELLPADDING='4'CELLSPACING='3'>\n";
            cadena += "<TR>\n";
            cadena += "<TH COLSPAN='4'><BR><H3>Reporte de Tokens General</H3>\n";
            cadena += "</TH>\n";
            cadena += "</TR>\n";
            cadena += "<TR>\n";
            cadena += "<TH>Token</TH>\n";
            cadena += "<TH>Lexema</TH>\n";
            cadena += "<TH>Columna</TH>\n";
            cadena += "<TH>Fila</TH>\n";
            cadena += "</TR>\n";
            for (int i = 0; i < arrayLexemas.Count; i++)
            {
                Console.WriteLine(i);
                cadena += " <TR ALIGN='CENTER'>\n";
                cadena += " <TD>" + arrayToken[i] + "</TD>";
                cadena += " <TD>" + arrayLexemas[i] + "</TD>";                            
                cadena += " <TD>" + arrayColumna[i] + "</TD>";
                cadena += " <TD>" + arrayFila[i] + "</TD>";
                cadena += " </TR>";
                
            }
            cadena += "</TABLE>";
            Console.Write(cadena);
            sw.WriteLine(cadena);
            sw.Flush();
            cadena = "";

        }

        public void reporteHtmlErrores(int num)
        {

            StreamWriter sw = new StreamWriter(@"C:\Users\Pablo Barillas\Desktop\Rep_Errores" + num + ".html");
            string cadena = "";
            cadena += "<TABLE BORDER='5'    WIDTH='50 % '   CELLPADDING='4'CELLSPACING='3'>\n";
            cadena += "<TR>\n";
            cadena += "<TH COLSPAN='4'><BR><H3>Reporte de Errores General</H3>\n";
            cadena += "</TH>\n";
            cadena += "</TR>\n";
            cadena += "<TR>\n";
            cadena += "<TH>Error</TH>\n";
            cadena += "<TH>Columna</TH>\n";
            cadena += "<TH>Fila</TH>\n";
            cadena += "</TR>\n";

            for (int i = 0; i < arrayError.Count; i++)
            {

                Console.WriteLine(i);
                cadena += " <TR ALIGN='CENTER'>\n";
                cadena += " <TD>" + arrayError[i] + "</TD>";
                cadena += " <TD>" + arrayColumnaError[i] + "</TD>";
                cadena += " <TD>" + arrayFilaError[i] + "</TD>";
                cadena += " </TR>";

            }

            cadena += "</TABLE>";
            Console.Write(cadena);
            sw.WriteLine(cadena);
            sw.Flush();
            cadena = "";

        }


    }
}
