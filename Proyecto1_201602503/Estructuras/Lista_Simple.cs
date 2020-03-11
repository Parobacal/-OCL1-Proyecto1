using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Proyecto1_201602503.Estructuras
{
    public class Lista_Simple
    {
        // Atributos de la clase
        private NodoLS Inicio;
        private int Size;

        // Constructor de la clase lista

        public Lista_Simple() {
            this.Inicio = null;
            this.Size = 0;
        }

        // -------------Metodos de la clase lista-------------

        public bool esVacia() // Metodo para verificar si la lista se encuentra vacia
        {
            return Inicio == null;
        }

        public int getSize() // Metodo para obtener el tamanio de la lista
        {
            return Size;
        }

        public void Insertar(String nombre, ArrayList elementos_) // Este metodo creara un nuevo nodo e insertara el nombre y los elementos
        {
            NodoLS nuevo = new NodoLS();
            nuevo.setNombre(nombre);
            nuevo.setIdentificador(Size);
            nuevo.setElementos(elementos_);

            if (esVacia())
            {
                Inicio = nuevo;
            }
            else
            {
                NodoLS aux = Inicio;
                while (aux.getSiguiente() != null)
                {
                    aux = aux.getSiguiente();
                }
                aux.setSiguiente(nuevo);
            }
            Size++;
        }

        public bool Buscar(int referencia) // Este metodo buscara si existe el indice del nodo 
        {

            NodoLS aux = Inicio;
            bool encontrado = false;
            while (aux != null && encontrado != true)
            {
                if (referencia == aux.getIdentificador())
                {
                    encontrado = true;
                }
                else
                {
                    aux = aux.getSiguiente();
                }
            }
            return encontrado;
        }

        public NodoLS obtenerNodo(int referencia) // Este metodo devolvera el nodo buscado para obtener lo que se necesite de el
        {
            if (Buscar(referencia))
            {
                NodoLS aux = Inicio;
                while (aux.getIdentificador() != referencia)
                {
                    aux = aux.getSiguiente();
                }
                return aux;
            }
            return null;
        }













    }
}
