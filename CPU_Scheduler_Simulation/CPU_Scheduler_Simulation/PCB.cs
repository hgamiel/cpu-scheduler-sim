using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class PCB
    {
        public int PID;                 //unique id to a PCB
        //public int serviceTime;         //time required by a process -- same as CPU burst time (see list below)
        public int priorityNumber;      //lowest interger -> higher priority
        public double arrivalTime;      //when processes arrive
        public double turnaroundTime;   //executionTime - arrivalTime
        public double waitTime;         //time process must wait during it execution - accumulated
        public double responseTime;     //time from submission utnil response begins to be received - accumulated
        public double executionTime;    //time of completion
        public List<int> CPU = new List<int>();               //array of CPU 
        public List<int> IO = new List<int>();                //array of IOs
    }
}
