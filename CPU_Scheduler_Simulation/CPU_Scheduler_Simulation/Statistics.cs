using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CPU_Scheduler_Simulation
{
    public class Statistics
    {
        public List<PCB> finishedProcesses = new List<PCB>();   // finished processes from the algorithms
        Scheduler scheduler = new Scheduler();
        public Data data = new Data();

        // data collection
        double avgResponseTime;
        double avgWaitTime;
        double avgTurnaroundTime;
        double avgExecutionTime;
        double avgTRTS;

        // constructor
        public Statistics(List<PCB> processes, Scheduler scheduler)
        {
            this.finishedProcesses = processes;
            this.scheduler = scheduler;
        }

        // computes and outputs all of the statistics
        public void runStatistics()
        {
            // output statistics
            Console.WriteLine(data.getStatisticsOutput(
                determineAverageResponseTime(), determineMinResponseTime(), determineMaxResponseTime(),
                determineAverageWaitTime(), determineMinWaitTime(), determineMaxWaitTime(),
                determineAverageTurnaroundTime(), determineMinTurnaroundTime(), determineMaxTurnaroundTime(),
                determineAverageExecutionTime(), determineMinExecutionTime(), determineMaxExecutionTime(),
                determineAverageTRTS(), scheduler.averageThroughput, scheduler.speedup, scheduler.averageContextSwitchTime
                )
            );

            avgResponseTime = determineAverageResponseTime();
            avgTurnaroundTime = determineAverageTurnaroundTime();
            avgExecutionTime = determineAverageExecutionTime();
            avgTRTS = determineAverageTRTS();
            avgWaitTime = determineAverageWaitTime();
            
        }

        /* 
         * The functions below calculate the following variables:
         */

        public double determineMinResponseTime()
        {
            double min = finishedProcesses[0].responseTime;
            foreach (var p in finishedProcesses)
            {
                if (p.responseTime < min)
                {
                    min = p.responseTime;
                }
            }

            return min;
        }

        public double determineMaxResponseTime()
        {
            double max = 0;
            foreach (var p in finishedProcesses)
            {
                if (p.responseTime > max)
                {
                    max = p.responseTime;
                }
            }

            return max;
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
            int sum = 0;
            int average;

            foreach (var p in finishedProcesses)
            {
                sum += Convert.ToInt32(p.responseTime);
            }

            average = (sum > 0) ? (sum / finishedProcesses.Count) : 0;

            return average;

        }

        public double determineAverageWaitTime()
        {
            int sum = 0;
            int average;

            foreach (var p in finishedProcesses)
            {
                sum += Convert.ToInt32(p.waitTime);
            }

            average = (sum > 0) ? (sum / finishedProcesses.Count) : 0;

            return average;

        }

        public double determineAverageTurnaroundTime()
        {
            int sum = 0;
            int average;

            foreach (var p in finishedProcesses)
            {
                sum += Convert.ToInt32(p.turnaroundTime);
            }

            average = (sum > 0) ? (sum / finishedProcesses.Count) : 0;

            return average;
        }

        public double determineMinTurnaroundTime()
        {
            double min = finishedProcesses[0].turnaroundTime;
            foreach (var p in finishedProcesses)
            {
                if (p.turnaroundTime < min)
                {
                    min = p.turnaroundTime;
                }
            }

            return min;
        }

        public double determineMaxTurnaroundTime()
        {
            double max = 0;
            foreach (var p in finishedProcesses)
            {
                if (p.turnaroundTime > max)
                {
                    max = p.turnaroundTime;
                }
            }

            return max;
        }

        public double determineAverageExecutionTime()
        {
            int sum = 0;
            int average;

            foreach (var p in finishedProcesses)
            {
                sum += Convert.ToInt32(p.executionTime);
            }

            average = (sum > 0) ? (sum / finishedProcesses.Count) : 0;

            return average;
        }

        public double determineMinExecutionTime()
        {
            double min = finishedProcesses[0].executionTime;
            foreach (var p in finishedProcesses)
            {
                if (p.executionTime < min)
                {
                    min = p.executionTime;
                }
            }

            return min;
        }

        public double determineMaxExecutionTime()
        {
            double max = 0;
            foreach (var p in finishedProcesses)
            {
                if (p.executionTime > max)
                {
                    max = p.executionTime;
                }
            }

            return max;
        }

        // determine average turnaround time / service time
        public double determineAverageTRTS() {

            int sum = 0;
            int average;
            int countTRTS = 0;

            foreach (var p in finishedProcesses)
            {
                foreach (var t in p.tr_ts)
                {
                    sum += Convert.ToInt32(t);
                    countTRTS++;
                }
            }

            average = (sum > 0) ? sum / countTRTS : 0;

            return average;
        }
    }
}
