using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> quantum1 = new List<int> { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20 };
            List<int> quantum2 = new List<int> { 22, 24, 26, 28, 30, 32, 34, 36, 38, 40};
            //int quantum1 = 10;    // change the quantums for round robin
            //int quantum2 = 20;
            Simulation simulation = new Simulation();   //create a simulation object
            //start the simulation
            simulation.startSim(quantum1, quantum2);  
        }
    }
}
