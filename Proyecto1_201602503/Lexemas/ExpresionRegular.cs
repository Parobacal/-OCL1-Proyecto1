using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto1_201602503.AFD_N;


namespace Proyecto1_201602503.Lexemas
{
    class ExpresionRegular
    {
        // Atributos de la clase 
        private string NombreEr;
        private AFND AFD;

        //Constructor de la clase
        public ExpresionRegular()
        {

            this.NombreEr = "";
            this.AFD = null;

        }

        // ------------------Metodos de la clase


        //Getters y Setters
        public string getNombreEr() {
            return NombreEr;
        }
        public AFND getAFD() {
            return AFD;
        }
        public void setNombreEr(string nombre_) {
            this.NombreEr = nombre_;
        }
        public void setAFD(AFND automata) {
            this.AFD = automata;
        }
    }
}
