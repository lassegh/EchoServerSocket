using System;
using System.Collections.Generic;
using System.Text;

namespace Delegate
{
    class Worker
    {
        // Define type
        private delegate int MyMethodRefType(int x, int y);

        // Define reference
        private MyMethodRefType mref;

        // Predifinerede metoder

        // Action returnerer void
        // Func returnerer en værdi
        // Predicate returnerer altid en bool

        private Func<int, int, int> nymyref; // to første ints er input - sidste er output

        public Worker()
        {
            
        }

        public void Start()
        {
            mref = Add;
            Console.WriteLine("5 + 7 = "+mref(5,7));
            mref = Multiply;
            Console.WriteLine("5 * 7 = " + mref(5, 7));

            mref += Add; // En metode tilføjes til delegaten - den holder nu en liste af metoder og vil eksekvere begge, når den bliver kaldt
            Console.WriteLine("5 ? 7 = " + mref(5, 7));
        }

        private int Add(int i, int j)
        {
            return i + j;
        }

        private int Multiply(int y, int z)
        {
            return y * z;
        }
    }
}
