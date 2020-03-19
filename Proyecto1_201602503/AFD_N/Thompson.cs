using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Proyecto1_201602503.AFD_N
{
    class Thompson
    {
        //----------------------------Atributos de la clase 
        public AFND Raiz; // AFND Principal
        private int Contador;
        private StringBuilder grafo;
        private String ruta;
        private String cadena;
        public ArrayList ER; // Lista que contendra los caracteres

        //----------------------------Constructor de la clase
        public Thompson()
        {
            this.Raiz = null;
            this.Contador = 0;
            this.ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            this.cadena = "";
            this.grafo = null;
            this.ER = new ArrayList();

        }

        //----------------------------Metodos de la clase
        public AFND Insertar()
        {
            Console.WriteLine(ER[Contador]);
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
                Console.WriteLine("LLEGUE AL PUNTO");
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
                Console.WriteLine("LLEGUE Afuera");
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
        
        private void generarDot(String rdot, String rpng)
        {
            try
            {
                System.IO.File.WriteAllText(rdot, grafo.ToString());
                Console.WriteLine(grafo.ToString());
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

        public void graficarAFND(string nombre_archivo)
        {
            recorrerAFND(Raiz);
            //Console.WriteLine(ruta);
            grafo = new StringBuilder();
            String rdot = ruta + "\\Dots\\" + nombre_archivo + ".dot";
            String rpng = ruta + "\\Automatas\\" + nombre_archivo + ".png";
            grafo.Append("digraph G { \n");
            grafo.Append("rankdir=LR; \n");
            grafo.Append(cadena);
            grafo.Append("} \n");
            this.generarDot(rdot, rpng);
            cadena = "";
        }





    }
}
