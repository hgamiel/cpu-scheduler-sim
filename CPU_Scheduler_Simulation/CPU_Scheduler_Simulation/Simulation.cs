using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class Simulation
    {
        public int quantum;
        public List<PCB> processTable;
        public Scheduler SimScheduler = new Scheduler();

        public Simulation() { }
        public void readDataFiles() { }        //pushes info from lines on .dat files into processTable as a PCB
        public void startSim(int quantum) { }  //sets up the scheduler and runs the simulation
        public void endSim() { }               //ends the simulation
    }
}
