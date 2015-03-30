using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class CPU
    {
        public Queue<PCB> readyIO = new Queue<PCB>();
        public Queue<PCB> waitingIO = new Queue<PCB>();
        public Queue<PCB> readyCPU = new Queue<PCB>();
        public Queue<PCB> waitingCPU = new Queue<PCB>();
        public Algorithm algorithms = new Algorithm();
        public Boolean idle = true;     //set processors to initially not busy
    }
}
