using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class CPU
    {
        //local queues
        public Queue<PCB> readyIO = new Queue<PCB>();       // ready queue for IO
        public Queue<PCB> readyCPU = new Queue<PCB>();      // ready queue for CPU
        public Algorithm algorithms = new Algorithm();      // each CPU runs a set of algorithms 
        public List<List<int>> throughput = new List<List<int>>
        {
            new List<int>(), new List<int>(), new List<int>(),
            new List<int>(), new List<int>(), new List<int>(),
            new List<int>(), new List<int>(), new List<int>()
        }; // number of processes that the algorithm finishes on each run
    }
}
