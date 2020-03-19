using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Proyecto1_201602503.AFD_N
{
    class Estado
    {
        //-------------------------------Atributos de la clase
        private int Indice; // Identificador del estado
        private List<Transicion> Transiciones; // Listado de transiciones que contendra el estado

        //-------------------------------Consturctor de la clase 
        public Estado() {
            
            this.Indice = 0;
            this.Transiciones = new List<Transicion>();       
        }

        //-------------------------------Metodos de la clase
        public void addTransition(Estado ei, Estado ef, string simbolo)
        {

            Transicion t = new Transicion();
            t.setEstadoInicial(ei);
            t.setEstadoFinal(ef);
            t.setSimbolo(simbolo);
            this.Transiciones.Add(t);
        }













        //Getters y Setters

        public int getIndice() {
            return Indice;
        }
        public List<Transicion> getTransiciones()
        {
            return Transiciones ;
        }

        public void setIndice(int num)
        {
            this.Indice = num;
        }

        public void setTransiciones(Transicion transicion_)
        {
            this.Transiciones.Add(transicion_);
        }

    }

}
