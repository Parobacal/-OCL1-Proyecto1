using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using Proyecto1_201602503.AFD;

namespace Proyecto1_201602503.AFD_N
{
    class Thompson
    {
        //----------------------------Atributos de la clase 
        public AFND Raiz, AFD; // AFND Principal y AFD que se formara
        private int Contador;
        private StringBuilder grafo, grafo1, grafo2;
        private String ruta, ruta1, ruta2;
        private String cadena, cadena1, cadena2;
        public ArrayList ER; // Lista que contendra los caracteres
        private ArrayList Terminales, Momentaneos; // Lista que guarda todos los simbolos terminales
        private List<Conjunto> ListaConjuntos; // Lista que guarda todos los conjuntos de las clausuras
        private List<Conjunto> ListaSubConjuntos; // Lista que guarda los subonjuntos formados
        private List<Trayecto> ListaTrayecto; // Lista que guarda todos los ir a 
        private bool Aceptacion;

       // public Conjunto nuevoConjunto;


        //----------------------------Constructor de la clase
        public Thompson()
        {
            this.Raiz = null;
            this.Aceptacion = false;
            this.AFD = null;
            this.Contador = 0;
            this.ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            this.ruta1 = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            this.ruta2 = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            this.cadena = "";
            this.cadena1 = "";
            this.cadena2 = "";
            this.grafo = null;
            this.grafo1 = null;
            this.grafo2 = null;
            this.ER = new ArrayList();
            this.Terminales = new ArrayList();
            this.Momentaneos = new ArrayList();
            this.ListaConjuntos = new List<Conjunto>();
            this.ListaSubConjuntos = new List<Conjunto>();
            this.ListaTrayecto = new List<Trayecto>();
           // this.nuevoConjunto = new Conjunto();

        }

        //----------------------------Metodos de la clase
        public AFND Insertar()
        {
            //Console.WriteLine(ER[Contador]);
            if (ER[Contador].Equals('*'))
            {
                AFND automataPadre = new AFND();
                Contador++;
                AFND automataHijo = Insertar();
                automataPadre = afnd_Kleene(automataHijo);
                return automataPadre;
            }
            else if (ER[Contador].Equals('.'))
            {
                //Console.WriteLine("LLEGUE AL PUNTO");
                AFND automataPadre = new AFND();
                Contador++;
                AFND automataHijo1 = Insertar();
                AFND automataHijo2 = Insertar();
                automataPadre = afnd_Punto(automataHijo1, automataHijo2);
                return automataPadre;
            }
            else if (ER[Contador].Equals('+'))
            {
                AFND automataPadre = new AFND();
                Contador++;
                AFND automataHijo = Insertar();
                automataPadre = afnd_Suma(automataHijo);
                return automataPadre;
            }
            else if (ER[Contador].Equals('|'))
            {
                AFND automataPadre = new AFND();
                Contador++;
                AFND automataHijo1 = Insertar();
                AFND automataHijo2 = Insertar();
                automataPadre = afnd_Or(automataHijo1, automataHijo2);
                return automataPadre;
            }
            else if (ER[Contador].Equals('?'))
            {
                AFND automataPadre = new AFND();
                Contador++;
                AFND automataHijo1 = Insertar();
                AFND automataHijo2 = new AFND();
                automataHijo2 = afnd_Simbolo("ε");
                automataPadre = afnd_Or(automataHijo1, automataHijo2);
                return automataPadre;
            }
            else
            {
                bool existe = false;
                if (Terminales.Count == 0) 
                {
                    Terminales.Add(ER[Contador]);
                }
                else 
                {
                    
                    for (int x = 0; x < Terminales.Count; x ++ ) 
                    {
                        if (Terminales[x].Equals(ER[Contador].ToString()))
                        {
                            Console.WriteLine("BOOL");
                            existe = true;
                            
                        }                     
                    }
                    if (existe == false) 
                    {
                        Terminales.Add(ER[Contador]);
                    }
                }
                              
                AFND Automata = new AFND();
                Automata = afnd_Simbolo(ER[Contador].ToString());
                Contador++;
                return Automata;
            }

        }

        // Metodos de incersion por cada operador
        public AFND afnd_Simbolo(string simbolo)
        {

            // Se crea un nuevo automata
            AFND Automata = new AFND();
            //Se generan los nuevos estados con sus indices respectivos
            Estado SO = new Estado();
            Estado SF = new Estado();
            SO.setIndice(0);
            SF.setIndice(1);
            // Se agrega la unica transicion que existe
            SO.addTransition(SO, SF, simbolo);
            // Se agregan los estados a los datos del automata
            Automata.setEstadoInicial(SO);
            Automata.setEstados(SO);
            Automata.setEstados(SF);
            Automata.setEstadosAceptacion(SF);
            //Se devuelve el automata
            return Automata;

        }

        public AFND afnd_Kleene(AFND automataHijo) {

            // Se crea un nuevo automata
            AFND automataNuevo = new AFND();
            //Se crea el nuevo estado inicial
            Estado estadoInicial = new Estado();
            estadoInicial.setIndice(0);
            //Se agrega el estado creado al automata nuevo
            automataNuevo.setEstadoInicial(estadoInicial);
            automataNuevo.setEstados(estadoInicial);
            // Agregamos cada uno de los estados que contiene el automata hijo al nuevo
            for (int i = 0; i < automataHijo.getEstados().Count; i++) {

                Estado nuevoEstado = new Estado();
                nuevoEstado = automataHijo.getEstados()[i];
                nuevoEstado.setIndice(i + 1);
                automataNuevo.setEstados(nuevoEstado);

            }
            // Se crea el estado final y se asigna como tal al autoamta
            Estado estadoFinal = new Estado();
            estadoFinal.setIndice(automataHijo.getEstados().Count + 1);
            automataNuevo.setEstados(estadoFinal);
            automataNuevo.setEstadosAceptacion(estadoFinal);
            // Se crea un nuevo estado para guardar el estado inicial del hijo
            Estado AI = new Estado();
            AI = automataHijo.getEstadoInicial();
            List<Estado> AF = automataHijo.getEstadosAceptacion();
            // Se asignan las transiciones con epsilon
            estadoInicial.addTransition(estadoInicial, automataHijo.getEstadoInicial(), "ε");
            estadoInicial.addTransition(estadoInicial, estadoFinal, "ε");
            // Se asignan las transiciones con epsilon para todos los estados de aceptacion del hijo
            for (int i = 0; i < AF.Count; i++) {
                AF[i].addTransition(AF[i], AI, "ε");
                AF[i].addTransition(AF[i], estadoFinal, "ε");
            }
            // Se devuelve el automata creado
            return automataNuevo;

        }

        public AFND afnd_Punto(AFND A1, AFND A2) {

            // Se crea un nuevo estado
            AFND automataNuevo = new AFND();
            int i = 0;
            // Se asignan los estados del hijo 1 al nuevo automata
            for (i = 0; i < A1.getEstados().Count; i++)
            {
                Estado tmp = new Estado();
                tmp = A1.getEstados()[i];
                tmp.setIndice(i);
                if (i == 0)
                {
                    automataNuevo.setEstadoInicial(tmp);
                }
                if (i == A1.getEstados().Count - 1)
                {
                //Se asignan las transiciones para los estados de aceptacion del hijo1
                    for (int k = 0; k < A1.getEstadosAceptacion().Count; k++)
                    {
                        tmp.addTransition(A1.getEstadosAceptacion()[k], A2.getEstadoInicial(), "ε");
                    }
                }
                automataNuevo.setEstados(tmp);

            }
            // Se asignan los estados del segundo hijo
            for (int j = 0; j < A2.getEstados().Count; j++)
            {
                Estado tmp = new Estado();
                tmp = A2.getEstados()[j];
                tmp.setIndice(i);

            // Se asigna el ultimo estado como el de aceptacion para el nuevo automata
                if (A2.getEstados().Count - 1 == j)
                    automataNuevo.setEstadosAceptacion(tmp);
                automataNuevo.setEstados(tmp);
                i++;
            }
            // Se devuelve el automata
            return automataNuevo;

        }

        public AFND afnd_Suma(AFND automataHijo) {

            // Se crea un nuevo automata
            AFND automataNuevo = new AFND();
            //Se crea el nuevo estado inicial
            Estado estadoInicial = new Estado();
            estadoInicial.setIndice(0);
            //Se agrega el estado creado al automata nuevo
            automataNuevo.setEstadoInicial(estadoInicial);
            automataNuevo.setEstados(estadoInicial);
            // Agregamos cada uno de los estados que contiene el automata hijo al nuevo
            for (int i = 0; i < automataHijo.getEstados().Count; i++)
            {

                Estado nuevoEstado = new Estado();
                nuevoEstado = automataHijo.getEstados()[i];
                nuevoEstado.setIndice(i + 1);
                automataNuevo.setEstados(nuevoEstado);

            }
            // Se crea el estado final y se asigna como estado de aceptacion
            Estado estadoFinal = new Estado();
            estadoFinal.setIndice(automataHijo.getEstados().Count + 1);
            automataNuevo.setEstados(estadoFinal);
            automataNuevo.setEstadosAceptacion(estadoFinal);
            // Se asigna un nuevo estado para obtener el estado inicial del hijo
            Estado AI = new Estado();
            AI = automataHijo.getEstadoInicial();
            List<Estado> AF = automataHijo.getEstadosAceptacion();
            // Se agrega la transicion del estado inicial al estado inicial del hijo
            estadoInicial.addTransition(estadoInicial, automataHijo.getEstadoInicial(), "ε");
            // Se agregan todas las transiciones con epsilon del hijo 
            for (int i = 0; i < AF.Count; i++)
            {
                AF[i].addTransition(AF[i], AI, "ε");
                AF[i].addTransition(AF[i], estadoFinal, "ε");
            }
            // Se devuelve el automata creado
            return automataNuevo;

        }

        public AFND afnd_Or(AFND A1, AFND A2){

            // Se crea un contador para llevar el orden de todos los estados a crear para el nuevo automata
            int cont = 0;
            // Se crea un nuevo automata
            AFND automataNuevo = new AFND();
            //Se crea el nuevo estado inicial
            Estado estadoInicial = new Estado();
            estadoInicial.setIndice(0);
            // Se crea la primera transicion del inicio hacia el inicio del hijo 1
            estadoInicial.addTransition(estadoInicial, A1.getEstadoInicial(), "ε");
            // Se agrega el primer estado al automata y se designa como inicial
            automataNuevo.setEstados(estadoInicial);
            automataNuevo.setEstadoInicial(estadoInicial);
            // Se agregan los estados del hijo 2
            for (cont = 0; cont < A2.getEstados().Count; cont ++) {
                Estado nuevoEstado = new Estado();
                nuevoEstado = A2.getEstados()[cont];
                nuevoEstado.setIndice(cont + 1);
                automataNuevo.setEstados(nuevoEstado);
            }
            // Se agregan los estados del hijo 1
            for (int i = 0; i < A1.getEstados().Count; i ++)
            {
                Estado nuevoEstado = new Estado();
                nuevoEstado = A1.getEstados()[i];
                nuevoEstado.setIndice(cont + 1);
                automataNuevo.setEstados(nuevoEstado);
                cont++;
            }
            // Se crea el estado final para el nuevo automata
            Estado estadoFinal = new Estado();
            estadoFinal.setIndice(A1.getEstados().Count + A2.getEstados().Count + 1); // Se le asigna el indice del total de estados de los dos hijos mas uno
            // Se agrega el estado a el nuevo automata y se designa como el de aceptacion
            automataNuevo.setEstados(estadoFinal);
            automataNuevo.setEstadosAceptacion(estadoFinal);
            // Se asigna un nuevo estado con el valor del estado inicial del hijo 2
            Estado AI = new Estado();
            AI = A2.getEstadoInicial();
            // Asignamos dos listas con los estados de aceptacion de cada hijo
            List<Estado> AF1 = new List<Estado>();
            AF1 = A2.getEstadosAceptacion();
            List<Estado> AF2 = new List<Estado>();
            AF2 = A1.getEstadosAceptacion();
            // Se hace la conexion del estado inicial al segundo hijo
            estadoInicial.addTransition(estadoInicial, AI, "ε");
            // Se agregan las transiciones del inicio del segundo hijo al estado final y tambien del primero
            for (int m = 0; m < AF1.Count; m ++) {
                AF1[m].addTransition(AF1[m], estadoFinal, "ε");
            }
            for (int n = 0; n < AF1.Count; n ++) {
                AF2[n].addTransition(AF2[n], estadoFinal, "ε");
            }
            // Se retorna el automata nuevo
            return automataNuevo;

        }

        public bool estadoExiste(Estado state, Conjunto conjunto_)
        {
            bool existe = false;
            for (int i = 0; i < conjunto_.getEstados().Count; i++)
            {
                if (conjunto_.getEstados()[i].getIndice().Equals(state.getIndice()))
                {
                    existe = true;
                    break;
                }
                //}
            }
            return existe;
        }


        public void obtenerConjunto(Estado State, Conjunto nuevoConjunto, string simbolo) 
        {
                if (State.getTransiciones().Any())
                {
                    if (simbolo.Equals("ε"))
                    {
                        if ((!nuevoConjunto.getEstados().Any())) // esta mal porque solo toma en cuenta si viene una vez y esta vacio en cambio pueden ser dos veces y no estar vacio
                        {

                            nuevoConjunto.setEstado(State.getTransiciones()[0].getEstadoInicial());

                        }
                    }
                    for (int i = 0; i < State.getTransiciones().Count; i++)
                    {
                        if (State.getTransiciones()[i].getSimbolo().Equals(simbolo))
                        {
                            Estado nuevoState = State.getTransiciones()[i].getEstadoFinal();
                            if (!estadoExiste(nuevoState, nuevoConjunto).Equals(true))
                            {                              
                                nuevoConjunto.setEstado(State.getTransiciones()[i].getEstadoFinal());
                                obtenerConjunto(nuevoState, nuevoConjunto, simbolo);
                            }

                        }

                    }
                }          
        }

        public void generarAFD(string nombre) {

            Conjunto primerConjunto = new Conjunto();
            obtenerConjunto(Raiz.getEstadoInicial(), primerConjunto, "ε");
            primerConjunto.setIndice(0);
            ListaConjuntos.Add(primerConjunto);
            obtenerTrayecto();
            Console.WriteLine(ListaTrayecto);
            ArrayList States = new ArrayList();
            obtenerEstados(States);
            graficarTablaEstados(nombre, States);
            
        }

        public void obtenerTrayecto() {
            int contador = 0;
            for (int m = 0; m < ListaConjuntos.Count; m ++) 
            {
               // Console.WriteLine(ListaTrayecto);
                Console.WriteLine("SOY LA M " + m);
                for (int i = 0; i < Terminales.Count; i++)
                {
                    Conjunto nuevoSubConjunto = new Conjunto();
                    Console.WriteLine("SOY LA I " + i);
                    for (int j = 0; j < ListaConjuntos[m].getEstados().Count; j++)
                    {
                        Console.WriteLine("SOY LA J " + j);
                        obtenerConjunto(ListaConjuntos[m].getEstados()[j], nuevoSubConjunto, Terminales[i].ToString());                       
                        
                    }
                    if (nuevoSubConjunto.getEstados().Any()) {
                        if (subConjuntoExiste(nuevoSubConjunto).Equals(false))
                        {
                            contador++;
                            Trayecto nuevoTrayecto = new Trayecto();
                            nuevoTrayecto.setSimbolo(Terminales[i].ToString());
                            nuevoTrayecto.setConjunto(nuevoSubConjunto);
                            nuevoTrayecto.setEstadoOrigen(m);
                            nuevoTrayecto.setEstadoFinal(contador);
                            ListaTrayecto.Add(nuevoTrayecto);
                            ListaSubConjuntos.Add(nuevoSubConjunto);

                            Conjunto nuevoConjunto = new Conjunto();
                            for (int y = 0; y < nuevoSubConjunto.getEstados().Count; y++) 
                            {
                                Conjunto Interno = new Conjunto();
                                if (nuevoSubConjunto.getEstados()[y].getIndice().Equals(Raiz.getEstadosAceptacion()[0].getIndice())) {
                                    nuevoConjunto.setEstado(nuevoSubConjunto.getEstados()[y]);
                                }
                                else {
                                    obtenerConjunto(nuevoSubConjunto.getEstados()[y], Interno, "ε");
                                }
                                for (int u = 0; u < Interno.getEstados().Count; u ++) {
                                    nuevoConjunto.setEstado(Interno.getEstados()[u]);
                                }
                            }
                            if (conjuntoExiste(nuevoConjunto).Equals(false)) {
                                nuevoConjunto.setIndice(m + 1);
                                ListaConjuntos.Add(nuevoConjunto);
                                Console.WriteLine(ListaConjuntos);
                            }
                          
                        }
                        else
                        {

                            //Console.WriteLine("soy VERDADERO");
                            Trayecto nuevoTrayecto = new Trayecto();
                            nuevoTrayecto.setSimbolo(Terminales[i].ToString());
                            nuevoTrayecto.setConjunto(nuevoSubConjunto);
                            nuevoTrayecto.setEstadoOrigen(m);
                            nuevoTrayecto.setEstadoFinal(obtenerSubConjuntoExiste(nuevoSubConjunto));
                            ListaTrayecto.Add(nuevoTrayecto);
                        }
                    }


                }
            }
        }

        public bool conjuntoExiste(Conjunto conjunto_) {
            bool existe = false;
            for (int i = 0; i < ListaConjuntos.Count; i ++) {
                //for (int j = 0; j < ListaConjuntos[i].getEstados().Count; j ++) {
                var a = ListaConjuntos[i].getEstados().All(conjunto_.getEstados().Contains) && ListaConjuntos[i].getEstados().Count == conjunto_.getEstados().Count;
                if (a.Equals(true))
                {
                    existe = true;
                    break;
                }
                //}
            }
            return existe;
        }

        public bool subConjuntoExiste(Conjunto conjunto_)
        {
            bool existe = false;
            for (int i = 0; i < ListaSubConjuntos.Count; i++)
            {
                //for (int j = 0; j < ListaConjuntos[i].getEstados().Count; j ++) {
                var a = ListaSubConjuntos[i].getEstados().All(conjunto_.getEstados().Contains) && ListaSubConjuntos[i].getEstados().Count == conjunto_.getEstados().Count;
                if (a.Equals(true))
                {
                    existe = true;
                    break;
                }
                //}
            }
            return existe;
        }

        public int obtenerSubConjuntoExiste(Conjunto conjunto_)
        {
           int  nuevoSub = 0;
            for (int i = 0; i < ListaTrayecto.Count; i++)
            {
                //for (int j = 0; j < ListaConjuntos[i].getEstados().Count; j ++) {
                var a = ListaTrayecto[i].getConjunto().getEstados().All(conjunto_.getEstados().Contains);
                if (a.Equals(true))
                {
                    nuevoSub = ListaTrayecto[i].getEstadoFinal();
                    return nuevoSub;
                }
                //}
            }
            return nuevoSub;
        }

        public void obtenerEstados(ArrayList states) {           
            for (int i = 0; i < ListaConjuntos.Count; i ++) {
                states.Add(i);
            }

        }

        //Metodos para graficar
        public void recorrerAFND(AFND raiz)
        {
            if (raiz != null)
            {
                for (int i = 0; i < raiz.getEstadosAceptacion().Count; i++)
                {
                    cadena += "node [shape = doublecircle]; " + "\"" +  raiz.getEstadosAceptacion()[i].getIndice().ToString() + "\""+ "; \n";

                }
                cadena += "node [shape = circle]; \n";
                for (int j = 0; j < raiz.getEstados().Count; j++)
                {
                    for (int i = 0; i < raiz.getEstados()[j].getTransiciones().Count; i++)
                    {
                        if (raiz.getEstados()[j].getTransiciones()[i] != null)
                        {
                            cadena += "\"" + raiz.getEstados()[j].getTransiciones()[i].getEstadoInicial().getIndice().ToString() + "\""  + " -> " + "\""  + raiz.getEstados()[j].getTransiciones()[i].getEstadoFinal().getIndice().ToString() + "\""  + "[ label = \"" + raiz.getEstados()[j].getTransiciones()[i].getSimbolo() + "\" ]; \n";
                        }
                    }
                }
            }
        }

        public void recorrerAFD(AFND raiz)
        {
            if (raiz != null)
            {
                for (int i = 0; i < raiz.getEstadosAceptacion().Count; i++)
                {
                    cadena2 += "node [shape = doublecircle]; " + "\"" + raiz.getEstadosAceptacion()[i].getIndice().ToString() + "\"" + "; \n";

                }
                cadena2 += "node [shape = circle]; \n";
                for (int j = 0; j < raiz.getEstados().Count; j++)
                {
                    for (int i = 0; i < raiz.getEstados()[j].getTransiciones().Count; i++)
                    {
                        if (raiz.getEstados()[j].getTransiciones()[i] != null)
                        {
                            cadena2 += "\"" + raiz.getEstados()[j].getTransiciones()[i].getEstadoInicial().getIndice().ToString() + "\"" + " -> " + "\"" + raiz.getEstados()[j].getTransiciones()[i].getEstadoFinal().getIndice().ToString() + "\"" + "[ label = \"" + raiz.getEstados()[j].getTransiciones()[i].getSimbolo() + "\" ]; \n";
                        }
                    }
                }
            }
        }

        private void generarDot(String rdot, String rpng)
        {
            try
            {
                System.IO.File.WriteAllText(rdot, grafo.ToString());
                //Console.WriteLine(grafo.ToString());
                String commandDot = "dot -Tpng " + "\"" + rdot + "\"" + " -o " + "\"" + rpng + "\"";
                var comando = string.Format(commandDot);
                var procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/C " + comando);               
                var proc = new System.Diagnostics.Process();
                proc.StartInfo = procStart;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex) {
                Console.WriteLine("Error" + ex);
            }
        }

        private void generarDot1(String rdot, String rpng)
        {
            try
            {
                System.IO.File.WriteAllText(rdot, grafo1.ToString());
                Console.WriteLine(grafo1.ToString());
                String commandDot = "dot -Tpng " + "\"" + rdot + "\"" + " -o " + "\"" + rpng + "\"";
                var comando = string.Format(commandDot);
                var procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/C " + comando);
                var proc = new System.Diagnostics.Process();
                proc.StartInfo = procStart;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex);
            }
        }

        private void generarDot2(String rdot, String rpng)
        {
            try
            {
                System.IO.File.WriteAllText(rdot, grafo2.ToString());
                //Console.WriteLine(grafo.ToString());
                String commandDot = "dot -Tpng " + "\"" + rdot + "\"" + " -o " + "\"" + rpng + "\"";
                var comando = string.Format(commandDot);
                var procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/C " + comando);
                var proc = new System.Diagnostics.Process();
                proc.StartInfo = procStart;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex);
            }
        }
        public void graficarAFND(string nombre_archivo)
        {
            recorrerAFND(Raiz);
            //Console.WriteLine(ruta);
            grafo = new StringBuilder();
            String rdot = ruta + "\\Dots\\" + nombre_archivo + ".dot";
            String rpng = ruta + "\\AFND\\" + nombre_archivo + ".png";
            grafo.Append("digraph G { \n");
            grafo.Append("rankdir=LR; \n");
            grafo.Append(cadena);
            grafo.Append("} \n");
            this.generarDot(rdot, rpng);
            cadena = "";
        }

        public void graficarTablaEstados(string nombre_archivo, ArrayList states)
        {

            recorrerTablaEstados(states);
            
            grafo1 = new StringBuilder();
            String rdot = ruta1 + "\\Dots\\" + nombre_archivo + ".dot";
            String rpng = ruta1 + "\\Tablas_estados\\" + nombre_archivo + ".png";
            grafo1.Append("digraph G { \n");
            grafo1.Append("tbl [ \n");
            grafo1.Append("shape=plaintext \n");
            grafo1.Append("label=< \n \n");
            grafo1.Append("<table border='0' cellborder='1' color='black' cellspacing='0'> \n");
            string texto = "";
            texto += "<tr><td>Estados</td>";
            for (int i = 0; i < Terminales.Count; i ++) {

                if (Terminales[i].Equals("<"))
                {
                    texto += "<td>Menor que</td>";
                }
                else if (Terminales[i].Equals(">")) 
                {
                    texto += "<td>Mayor que</td>";
                }
                else
                {
                    texto += "<td>" + Terminales[i].ToString() + "</td>";
                }
            }
            texto += "</tr> \n";
            grafo1.Append(texto);
            texto = "";
            grafo1.Append("<tr><td> \n");
            grafo1.Append("<table color='black' border='0' cellborder='1' cellpadding='10' cellspacing='0'> \n");
            for (int i = 0; i < states.Count; i ++) {
                texto += "<tr><td>" + states[i] + "</td></tr>\n";
            }
            grafo1.Append(texto);
            texto = "";
            grafo1.Append("</table> \n");
            grafo1.Append("</td> \n");
            grafo1.Append("<td colspan=\'" + Terminales.Count + "\' rowspan=\'" + Terminales.Count + "\'> \n");
            grafo1.Append("<table color='black' border='0' cellborder='1' cellpadding='10' cellspacing='0'> \n");
            grafo1.Append(cadena1);
            cadena1 = "";
            grafo1.Append("</table> \n");
            grafo1.Append("</td> \n");
            grafo1.Append("</tr> \n");
            grafo1.Append("</table> \n");
            grafo1.Append(">]; \n");
            grafo1.Append("} \n");
            this.generarDot1(rdot, rpng);
        }

        public void graficarAFD(string nombre_archivo)
        {
            recorrerAFD(AFD);
            //Console.WriteLine(ruta);
            grafo2 = new StringBuilder();
            String rdot = ruta2 + "\\Dots\\" + nombre_archivo + ".dot";
            String rpng = ruta2 + "\\AFD\\" + nombre_archivo + ".png";
            grafo2.Append("digraph G { \n");
            grafo2.Append("rankdir=LR; \n");
            grafo2.Append(cadena2);
            grafo2.Append("} \n");
            this.generarDot2(rdot, rpng);
            cadena2 = "";
        }

        public void recorrerTablaEstados(ArrayList states) 
        {

            AFND nuevoAFD = new AFND();
            // Bool para ver si existe o no transicion con determinado simbolo
            bool coincidencia = false;
            // Para cada estado
            for (int i = 0; i < states.Count; i ++) 
            {
                // Se crea nuevo estado para el nuevo AFD
                Estado nuevoEstado = new Estado();
                nuevoEstado.setIndice((int)states[i]);
                cadena1 += "<tr>";
                // Para cada simbolo
                for (int j = 0; j < Terminales.Count; j ++) 
                {
                    coincidencia = false;
                    // Para cada trayectoria
                    for (int k = 0; k < ListaTrayecto.Count; k ++) 
                    {
                        // Si hay una coincidencia
                        if ((ListaTrayecto[k].getEstadoOrigen().Equals(states[i])) && (ListaTrayecto[k].getSimbolo().Equals(Terminales[j])))
                        {
                            coincidencia = true;
                            // Se asigna indice al estado creado
                            
                            // Se crea estado final
                            Estado nuevoEstadoFinal = new Estado();
                            nuevoEstadoFinal.setIndice(ListaTrayecto[k].getEstadoFinal());
                            // Se crea nueva transicion del nuevo estado creado
                            nuevoEstado.addTransition(nuevoEstado, nuevoEstadoFinal, Terminales[j].ToString());

                            cadena1 += "<td>" + ListaTrayecto[k].getEstadoFinal() + "</td>";                       
                        }                                      
                    }
                    if (coincidencia.Equals(false)) {
                        cadena1 += "<td>-</td>";
                    }                                         
                }
                cadena1 += "</tr>\n";

                nuevoAFD.setEstados(nuevoEstado);
            }

            // Se asigna estado inicial
            for (int l = 0; l < nuevoAFD.getEstados().Count; l ++) 
            {
                if (nuevoAFD.getEstados()[l].getIndice().Equals(0)) 
                {
                    nuevoAFD.setEstadoInicial(nuevoAFD.getEstados()[l]);
                }
            }

            // Se asigna estado final
            for (int i = 0; i < ListaConjuntos.Count; i ++) 
            {
                Console.WriteLine("I = " + i);
                for (int j = 0; j < ListaConjuntos[i].getEstados().Count; j ++) 
                {
                    Console.WriteLine("J = " + j);
                    if (ListaConjuntos[i].getEstados()[j].getIndice().Equals(Raiz.getEstadosAceptacion()[0].getIndice())) 
                    {
                        for (int k = 0; k < nuevoAFD.getEstados().Count; k ++) 
                        {
                            Console.WriteLine("K = " + k);
                            if (nuevoAFD.getEstados()[k].getIndice().Equals(i)) 
                            {
                                nuevoAFD.setEstadosAceptacion(nuevoAFD.getEstados()[k]);
                            }
                        }
                    }
                }
            }

            AFD = nuevoAFD;
            
        }










    }
}
