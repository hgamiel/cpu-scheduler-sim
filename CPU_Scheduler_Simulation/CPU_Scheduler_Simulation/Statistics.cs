using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class Statistics
    {
        public List<PCB> finishedProcesses = new List<PCB>();

        public Statistics() { }

        public double determineAverageTRTS() {

            double sum = 0;
            double average;
            double countTRTS = 0;

            foreach (var p in finishedProcesses)
            {
                foreach (var t in p.tr_ts)
                {
                    sum += t;
                    countTRTS++;
                }
            }

            average = (sum > 0) ? sum / countTRTS : 0;

            return average;
        }

    }
}
