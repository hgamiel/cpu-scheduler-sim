using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CPU_Scheduler_Simulation
{
    //public interface ICloneable<T> { T Clone(); }
    [Serializable]
    public class PCB : ICloneable
    {
        public char name;               //testing purposes
        public int PID;                 //unique id to a PCB
        public int serviceTime;         //time required by a process -- same as CPU burst time (see list below)
        public int beginServiceTime;    //temp to hold initial service time needed for current CPU burst
        public int priorityNumber;      //lowest interger -> higher priority
        public int arrivalTime;         //when processes arrive
        public int start;
        public int stop;
        public double turnaroundTime;   //executionTime - arrivalTime
        public double waitTime;         //time process must wait during its execution - accumulated
        public double responseTime;     //time from submission until first CPU allocation
        public double executionTime;    //time of completion
        public List<double> tr_ts = new List<double>();            //turnaround time / service time
        public double lastTimeProcessed; //for purposes of RR
        public Boolean finished = false;
        public Boolean alreadyProcessed = false;

        public Queue<int> CPU = new Queue<int>(); // queue of CPU bursts
        public Queue<int> IO = new Queue<int>(); // queue of IO bursts

        public PCB() {
            this.lastTimeProcessed = 0;
            this.waitTime = 0;
            this.responseTime = -1; // this is to avoid checks where startTime could actually start at 0
        }

        public object Clone()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                if (this.GetType().IsSerializable)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, this);
                    stream.Position = 0;
                    return formatter.Deserialize(stream);
                }
                return null;
            }
        }
        public void determineTurnaroundTime() {
            this.turnaroundTime = executionTime - arrivalTime;
        }

        public void determineTRTS(int st)
        {
            this.tr_ts.Add(turnaroundTime / st);
        }

        public void resetTempCounters () {
            this.lastTimeProcessed = 0;
            this.beginServiceTime = 0;
        }

        public String serveTime(double q) {
            this.serviceTime -= (int)q;
            return Convert.ToString(this.serviceTime);
        }
    }
}
