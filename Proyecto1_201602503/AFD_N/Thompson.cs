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
        // Atributos de la clase 
        public AFND Raiz; // AFND Principal
        private int Contador;
        private StringBuilder grafo;
        private String ruta;
        private String cadena;
        public ArrayList ER; // Lista que contendra los caracteres

        // Constructor de la clase
        public Thompson()
        {
            this.Raiz = null;
            this.Contador = 0;
            this.ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            this.cadena = "";
            this.grafo = null;
            this.ER = new ArrayList();
        }

        public AFND Insertar()
        {
            if (ER[Contador].Equals("*"))
            {
                AFND automataPadre = new AFND();
                Contador++;
                AFND automataHijo = Insertar();
                automataPadre = afnd_Kleene(automataHijo);
                return automataPadre;
            }
            else if (ER[Contador].Equals("."))
            {
                Contador++;
                AFND Automata = new AFND();
                return Automata;
            }
            else if (ER[Contador].Equals("+"))
            {
                Contador++;
                AFND Automata = new AFND();
                return Automata;
            }
            else if (ER[Contador].Equals("|"))
            {
                Contador++;
                AFND Automata = new AFND();
                return Automata;
            }
            else if (ER[Contador].Equals("?"))
            {
                Contador++;
                AFND Automata = new AFND();
                return Automata;
            }
            else
            {                
                AFND Automata = new AFND();
                Automata = afnd_Simbolo(ER[Contador].ToString());
                Contador++;
                return Automata;
            }

        }

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

        public AFND afnd_Kleene(AFND automataHijo){
            // Se crea un nuevo automata
            AFND automataNuevo = new AFND();
            //Se crea el nuevo estado inicial
            Estado estadoInicial = new Estado();
            estadoInicial.setIndice(0);
            //Se agrega el estado creado al automata nuevo
            automataNuevo.setEstadoInicial(estadoInicial);
            automataNuevo.setEstados(estadoInicial);
            // Agregamos cada uno de los estados que contiene el automata hijo al nuevo
            for (int i = 0; i < automataHijo.getEstados().Count; i ++) {

                Estado nuevoEstado = new Estado();
                nuevoEstado = automataHijo.getEstados()[i];
                nuevoEstado.setIndice(i + 1);
                automataNuevo.setEstados(nuevoEstado);

            }

            Estado estadoFinal = new Estado();
            estadoFinal.setIndice(automataHijo.getEstados().Count + 1);
            automataNuevo.setEstados(estadoFinal);
            automataNuevo.setEstadosAceptacion(estadoFinal);

            Estado AI = new Estado();
            AI = automataHijo.getEstadoInicial();
            List<Estado> AF = automataHijo.getEstadosAceptacion();

            estadoInicial.addTransition(estadoInicial, automataHijo.getEstadoInicial(), "ε");
            estadoInicial.addTransition(estadoInicial, estadoFinal, "ε");

            for (int i = 0; i < AF.Count; i ++) {
                AF[i].addTransition(AF[i], AI, "ε");
                AF[i].addTransition(AF[i], estadoFinal, "ε");
            }

            // Se devuelve el automata creado
            return automataNuevo;
        }

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
            String rdot = ruta + "\\Automatas\\" + nombre_archivo + ".dot";
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
