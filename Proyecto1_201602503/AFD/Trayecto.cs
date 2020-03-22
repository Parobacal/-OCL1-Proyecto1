using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_201602503.AFD
{
    class Trayecto
    {
        // Atributos de la clase
        private string Simbolo;
        private Conjunto Conjunto;
        private int EstadoOrigen;
        private int EstadoFinal;

        // Constructor de la clase

        public Trayecto()
        {
            this.Simbolo = "";
            this.Conjunto = null ;
            this.EstadoOrigen = 0;
            this.EstadoFinal = 0;
        }

        //------------------Metodos de la clase

        //Getters y Setters

        public string getSimbolo() {
            return Simbolo;
        }
        public Conjunto getConjunto()
        {
            return Conjunto;
        }
        public int getEstadoOrigen() {
            return EstadoOrigen;
        }
        public int getEstadoFinal()
        {
            return EstadoFinal;
        }

        public void setSimbolo(string nombre_) {
            this.Simbolo = nombre_;  
        }
        public void setConjunto(Conjunto conj_)
        {
            this.Conjunto = conj_;
        }
        public void setEstadoOrigen(int num) { 
            this.EstadoOrigen = num;
        }
        public void setEstadoFinal(int num)
        {
            this.EstadoFinal = num;
        }
    }
}
