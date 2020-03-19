using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Proyecto1_201602503.AFD_N
{
    class AFND
    {
        //---------------------Atributos de la clase 
        private Estado Estado_Inicial; //Estado inicial del automata
        private List<Estado> Estados;
        private List<Estado> Estados_Aceptacion;
        public AFND C1,C2; // Tipo de automatas a determinar en la recursividad
        

        //---------------------Constructor de la clase
        public AFND() {
         
            this.Estado_Inicial = null;
            this.Estados = new List<Estado>();
            this.Estados_Aceptacion = new List<Estado>();
            this.C1 = new AFND();
            this.C2 = new AFND();

        }

        //---------------------Metodos de la clase 

        

 
        // Getters y Setters
        public Estado getEstadoInicial() {
            return Estado_Inicial;
        }
        public List<Estado> getEstados()
        {
            return Estados;
        }
        public List<Estado> getEstadosAceptacion()
        {
            return Estados_Aceptacion;
        }

        public void setEstadoInicial(Estado estado_) 
        {
            this.Estado_Inicial = estado_;
        }
        public void setEstados(Estado estado_)
        {
            this.Estados.Add(estado_);
        }
        public void setEstadosAceptacion(Estado estado_)
        {
            this.Estados_Aceptacion.Add(estado_);
        }
    }
}
