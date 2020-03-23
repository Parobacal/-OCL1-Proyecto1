using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto1_201602503.AFD_N;

namespace Proyecto1_201602503.AFD
{
    class Conjunto
    {

        // Atributos de la clase 
        private int Indice;
        List<Estado> Estados;

        // Constructor de la clas
        public Conjunto()
        {
            this.Indice = 0;
            this.Estados = new List<Estado>();
        }

        //----------------Metodos de la clase

        //Getters y Setters

        public int getIndice() {
            return Indice;
        }

        public List<Estado> getEstados() {
            return Estados;
        }

        public void setIndice(int simbolo_) {
            this.Indice = simbolo_;
        }
        public void setEstado(Estado estado_)
        {
            this.Estados.Add(estado_);
        }

    }
}
