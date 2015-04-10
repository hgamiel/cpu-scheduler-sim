using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class PCB
    {
        public char name;               //testing purposes
        public int PID;                 //unique id to a PCB
        public int serviceTime;         //time required by a process -- same as CPU burst time (see list below)
        public int beginServiceTime;    //temp to hold initial service time needed for current CPU burst
        public int priorityNumber;      //lowest interger -> higher priority
        public int arrivalTime;         //when processes arrive
        public int startTime;           //actual start time of a process
        public double turnaroundTime;   //executionTime - arrivalTime
        public double waitTime;         //time process must wait during it execution - accumulated
        public double responseTime;     //time from submission until first CPU allocation
        public double executionTime;    //time of completion
        public List<double> tr_ts = new List<double>();            //turnaround time / service time
        public double lastTimeProcessed; //for purposes of RR
        public Boolean finished = false;

        public Queue<int> CPU = new Queue<int>(); // queue of CPU bursts
        public Queue<int> IO = new Queue<int>(); // queue of IO bursts

        public PCB() {
            lastTimeProcessed = 0;
            waitTime = 0;
            startTime = -1; // this is to avoid checks where startTime could actually start at 0
        }

        public void determineTurnaroundTime() {
            turnaroundTime = executionTime - arrivalTime;
        }

        public void determineTRTS(int st)
        {
            tr_ts.Add(turnaroundTime / st);
        }

        public void resetTempCounters () {
            lastTimeProcessed = 0;
            beginServiceTime = 0;
        }

        public String serveTime(double q) {
            this.serviceTime -= (int)q;
            return Convert.ToString(this.serviceTime);
        }
    }
}
