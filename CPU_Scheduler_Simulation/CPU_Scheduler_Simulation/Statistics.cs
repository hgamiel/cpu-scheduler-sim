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

        public Statistics(List<PCB> processes)
        {
            finishedProcesses = processes;
        }

        public void runStatistics()
        {
            Console.WriteLine("Avg response time: " + determineAverageResponseTime());
            Console.WriteLine("Avg wait time: " + determineAverageWaitTime());
            Console.WriteLine("Avg turnaround: " + determineAverageTurnaround());
            Console.WriteLine("Avg trts: " + determineAverageTRTS());
            Console.WriteLine("Min wait time: " + determineMinWaitTime());
            Console.WriteLine("Max wait time: " + determineMaxWaitTime());
        }

        public double determineMinWaitTime()
        {
            double min = finishedProcesses[0].waitTime;
            foreach (var p in finishedProcesses)
            {
                if (p.waitTime < min)
                {
                    min = p.waitTime;
                }
            }

            return min;
        }

        public double determineMaxWaitTime()
        {
            double max = 0;
            foreach (var p in finishedProcesses)
            {
                if (p.waitTime > max)
                {
                    max = p.waitTime;
                }
            }

            return max;
        }

        public double determineAverageResponseTime()
        {
            double sum = 0;
            double average;

            foreach (var p in finishedProcesses)
            {
                sum += p.responseTime;
            }

            average = (sum > 0) ? (sum / finishedProcesses.Count) : 0;

            return average;

        }

        public double determineAverageWaitTime()
        {
            double sum = 0;
            double average;

            foreach (var p in finishedProcesses)
            {
                sum += p.waitTime;
            }

            average = (sum > 0) ? (sum / finishedProcesses.Count) : 0;

            return average;

        }

        public double determineAverageTurnaround()
        {
            double sum = 0;
            double average;

            foreach (var p in finishedProcesses)
            {
                sum += p.turnaroundTime;
            }

            average = (sum > 0) ? (sum / finishedProcesses.Count) : 0;

            return average;
        }

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
