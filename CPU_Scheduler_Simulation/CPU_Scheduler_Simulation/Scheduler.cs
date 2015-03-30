using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class Scheduler
    {
        public List<CPU> CPUs = new List<CPU>();
        public Queue<PCB> readyThreads;     //global queue of ready threads
        public Scheduler() {}
        public void loadBalance(List<PCB> processes) {
            readyThreads = new Queue<PCB>(processes);   //take list of processes from file and put them into a queue
            //run until there are no more processes
            //we will be using dual-core processors
            CPU cpu1 = new CPU();
            CPU cpu2 = new CPU();
            while (readyThreads.Count != 0)
            {
                //for each idle processor, select thread from ready queue -> readyIO or readyCPU queues
                if (cpu1.idle)
                {
                    var thread = readyThreads.Dequeue();
                    //IO or CPU first?
                }
                
            }
        }
    }
}
