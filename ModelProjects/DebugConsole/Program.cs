using Common;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugConsole
{
    class Program
    {
        private static IModel model;
        static void Main(string[] args)
        {
             model = new Model(3, 3, 3);
        }
    }
}
