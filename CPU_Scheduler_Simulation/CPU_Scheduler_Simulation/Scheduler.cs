using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class Scheduler
    {
        public CPU cpu1 = new CPU();
        public CPU cpu2 = new CPU();
        public Queue<PCB> readyThreads;     //global queue of ready threads
        public Scheduler() {}
        //for each idle processor, select thread from ready queue
        //version: first come first served to whichever processor is idle
        public void loadSharing(List<PCB> processes) {
            readyThreads = new Queue<PCB>(processes);
            while (readyThreads.Count != 0)
            {
                if (cpu1.idle)
                {
                    cpu1.idle = false;  //processor is now busy
                    var thread = readyThreads.Dequeue();
                    cpu1.readyCPU.Enqueue(thread);
                    cpu1.algorithms.rr(cpu1.readyCPU, 5);
                    cpu1.idle = true;   //process is open for another thread
                }
                else if (cpu2.idle)
                {
                    cpu2.idle = false;
                    
                    cpu2.idle = true;
                }
            }
        }
    }
}
