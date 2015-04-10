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
        //public int throughput;
        public Queue<PCB> readyIO = new Queue<PCB>();       // ready queue for IO
        public Queue<PCB> waitingIO = new Queue<PCB>();     // waiting queue for IO
        public Queue<PCB> readyCPU = new Queue<PCB>();      // ready queue for CPU
        public Queue<PCB> waitingCPU = new Queue<PCB>();    // waiting queue for CPU
        public Algorithm algorithms = new Algorithm();      // each CPU runs a set of algorithms 
        
    }
}
