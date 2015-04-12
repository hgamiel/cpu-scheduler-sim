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
        public double utilization = 0;                      // processor utilization: total time - context switch time / total time
        public Queue<PCB> readyIO = new Queue<PCB>();       // ready queue for IO
        public Queue<PCB> readyCPU = new Queue<PCB>();      // ready queue for CPU
        public Algorithm algorithms = new Algorithm();      // each CPU runs a set of algorithms 
        public double throughput = 0;                       // number of processes that complete execution per time unit
    }
}
