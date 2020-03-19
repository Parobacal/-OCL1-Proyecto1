using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_201602503.AFD_N
{


    class Transicion
    {
        // Atributos de la clase
        private string Simbolo; // simbolo que correspondera a la transicion en cuestion
        private Estado Estado_Inicial; // Estado que define el inicio de la transicion
        private Estado Estado_Final; // Estado que define el final de la transicion

        // Constructor de la clase
        public Transicion() {

            this.Simbolo = "";
            this.Estado_Inicial = null;
            this.Estado_Final = null;       
        }

        //-------------------------------Metodos de la clase

        












        //Getters y Setters
        public string getSimbolo()
        {
            return Simbolo;
        }
        public Estado getEstadoInicial()
        {
            return Estado_Inicial;
        }
        public Estado getEstadoFinal()
        {
            return Estado_Final;
        }

        public void setSimbolo(string simbolo_)
        {
            this.Simbolo = simbolo_;
        }

        public void setEstadoInicial(Estado estado_inicial_)
        {
            this.Estado_Inicial = estado_inicial_;
        }
        public void setEstadoFinal(Estado estado_final_)
        {
            this.Estado_Final = estado_final_;
        }
    }
}
