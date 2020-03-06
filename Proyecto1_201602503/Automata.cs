using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace Proyecto1_201602503
{
    class Automata
    {
        //Atributos de la clase automata

        private string lexema; // String para guardar y concatenar cada lexema leido
        private int fila, columna; // Contadores para poder identificar la fila y la columna
        private ArrayList arrayLexemas, arrayToken, arrayFila, arrayColumna, arrayError, arrayFilaError, arrayColumnaError, arrayExp, arrayLex; // Arreglos para guardar toda la lista de simbolos

        // Constructor de la clase automata
        public Automata(){

            this.lexema = "";
            this.fila = 1;
            this.columna = 0;
            this.arrayLexemas = new ArrayList();
            this.arrayToken = new ArrayList();
            this.arrayFila = new ArrayList();
            this.arrayColumna = new ArrayList();
            this.arrayError = new ArrayList();
            this.arrayFilaError = new ArrayList();
            this.arrayColumnaError = new ArrayList();

        }

        // Metodos de la clase automata

        public void analisisLexico(string archivoTexto) {
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
                        else if (codigoASCII == 10) // Si es un salto de linea
                        {
                            Console.WriteLine("Econtre salto de linea");
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
                        }
                        else {
                            if (codigoASCII == 10) { // Si es igual a salto de linea
                                fila += 1;                               
                            }
                            arrayLexemas.Add(lexema);
                            arrayToken.Add("Tk_Simbolo");
                            arrayFila.Add(fila);
                            arrayColumna.Add(columna);
                            estado = 0;
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
                        else if (((codigoASCII >= 32) && (codigoASCII <= 33)) || ((codigoASCII >= 35) && (codigoASCII <= 46)) || ((codigoASCII >= 58) && (codigoASCII <= 59)) || ((codigoASCII >= 61) && (codigoASCII <= 64)) || ((codigoASCII >= 91) && (codigoASCII <= 94)) || (codigoASCII == 96) || ((codigoASCII >= 123) && (codigoASCII <= 126)))
                        {
                            columna = columna - restColumna; // Resto el valor de restColumna a columna para definir la columna en un solo token
                            arrayLexemas.Add(lexema);
                            arrayToken.Add("Tk_Id");
                            arrayColumna.Add(columna);
                            arrayFila.Add(fila);
                            restColumna = 0; // Inicializo la cuenta para otro token
                            estado = 0;
                            if ((codigoASCII == 44) || (codigoASCII == 59) || (codigoASCII == 58) || (codigoASCII == 126))
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
                        else {
                            estado = 4;
                            restColumna += 1;
                            lexema += simbolo;
                        }
                        break;
                    case 5:
                        if (codigoASCII == 33) // Si el simbolo es !
                        {
                            estado = 6;
                        }
                        else if ((simbolo >= 34) && (simbolo <= 126) || (simbolo == 32)) // Si el simbolo es cualquier otro del lenguaje
                        {
                            estado = 5;
                            lexema += simbolo;
                        }
                        else {
                            arrayError.Add("" + simbolo);
                            arrayColumnaError.Add(columna);
                            arrayFilaError.Add(fila);
                            estado = 0;
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
                        }
                        else if (simbolo == 33) // Si el simbolo es !
                        {
                            estado = 6;
                            restColumna += 1;
                            lexema += simbolo;
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
                        }
                        break;
                    case 7:
                        if ((simbolo >= 32) && (simbolo <= 126)) // Cualquier simbolo dentro del lenguaje
                        {
                            restColumna += 1;
                            estado = 7;
                            lexema += simbolo;
                        }
                        //FINAL DE COMENTARIO
                        else if (simbolo == 10)
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
                        else
                        {
                            arrayError.Add("" + simbolo);
                            arrayColumnaError.Add(columna);
                            arrayFilaError.Add(fila);
                        }
                        break;
                }
            }
            

        }

        public void analisisSintacto(string texto){



        }
        // Metodos getters
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

        // Metodos para los reportes
        public void reporteHtml() {
            StreamWriter sw = new StreamWriter(@"C:\Users\Pablo Barillas\Desktop\Rep_Token.html");
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
                cadena += " <TR ALIGN='CENTER'>\n";
                cadena += " <TD>" + arrayToken[i] + "</TD>";
                cadena += " <TD>" + arrayLexemas[i] + "</TD>";
                cadena += " <TD>" + arrayColumna[i] + "</TD>";
                cadena += " <TD>" + arrayFila[i] + "</TD>";
                cadena += " </TR>";
            }
            cadena += "</TABLE>";
            sw.WriteLine(cadena);
            cadena = "";          
        }
















    }
}
