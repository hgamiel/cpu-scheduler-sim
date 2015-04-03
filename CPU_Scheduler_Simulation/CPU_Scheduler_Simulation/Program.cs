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
            int quantum = 0;    //we can either have user input or hard-code
            Simulation simulation = new Simulation();   //create a simulation object
            //start the simulation
            simulation.startSim(quantum);  
        }
    }
}
