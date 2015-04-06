using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class PCB
    {
        public char name;     //testing purposes
        public int PID;                 //unique id to a PCB
        public int serviceTime;         //time required by a process -- same as CPU burst time (see list below)
        public int priorityNumber;      //lowest interger -> higher priority
        public int arrivalTime;      //when processes arrive
        public int startTime;           //actual start time of a process
        public double turnaroundTime;   //executionTime - arrivalTime
        public double waitTime;         //time process must wait during it execution - accumulated
        public double responseTime;     //time from submission utnil response begins to be received - accumulated
        public double executionTime;    //time of completion
        public double tr_ts;            //turnaround time / service time
        //public Boolean waiting = false;
        public Boolean finished = false;

        public List<int> CPU = new List<int>();     //array of CPU 
        public List<int> IO = new List<int>();      //array of IOs

        public PCB() { }

        public String serveTime(double q) {
            this.serviceTime -= (int)q;
            return Convert.ToString(this.serviceTime);
        }
    }
}
