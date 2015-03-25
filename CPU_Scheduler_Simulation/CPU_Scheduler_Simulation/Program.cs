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
            int quantum = 0;
            Simulation simulation = new Simulation();
            simulation.startSim(quantum);
        }
    }
}
