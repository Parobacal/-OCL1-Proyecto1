using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Proyecto1_201602503.Estructuras
{
    public class NodoLS
    {
        // Atributos de la clase Nodo LS
        private String Nombre;
        private int Id;
        private ArrayList Elementos;
        private NodoLS Siguiente;

        // Constructor de la clase nodo
        public NodoLS() {

            this.Nombre = "";
            this.Id = 0;
            this.Elementos = new ArrayList();
            this.Siguiente = null;
       
        }

        // ----------------Metodos de la clase-----------------

        // Metodos GETTER:

        public int getIdentificador()
        {
            return Id;
        }

        public String getNombre()
        {
            return Nombre;
        }
        public NodoLS getSiguiente()
        {
            return Siguiente;
        }

        public ArrayList getElementos()
        {
            return Elementos;
        }

        // Metodos SETTER:

        public void setIdentificador(int identificador_)
        {
            this.Id = identificador_;
        }

        public void setNombre(String nombre_)
        {
            this.Nombre = nombre_;

        }

        public void setSiguiente(NodoLS siguiente_)
        {
            this.Siguiente = siguiente_;
        }

        public void setElementos(ArrayList elementos_) {
            this.Elementos = elementos_;
        }





    }
}
