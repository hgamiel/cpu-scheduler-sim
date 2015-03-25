using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class CPU
    {
        public Queue<PCB> readyIO;
        public Queue<PCB> waitingIO;
        public Queue<PCB> readyCPU;
        public Queue<PCB> waitingCPU;
        public Algorithm algorithms = new Algorithm();
    }
}
