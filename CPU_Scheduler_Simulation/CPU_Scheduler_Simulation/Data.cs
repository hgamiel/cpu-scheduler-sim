using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace CPU_Scheduler_Simulation
{
    public class Data
    {
        // graphs
        List<List<int>> quantums = new List<List<int>>();
        List<double> avgTurnaround = new List<double>();
        List<double> avgResponse = new List<double>();
        List<double> avgWait = new List<double>();
        List<List<double>> throughput = new List<List<double>>();
        List<int> numCPUs = new List<int>();
        List<double> speedup = new List<double>();

        // table
        List<String> orderings = new List<String>();
        List<double> contextSwitch = new List<double>();
        List<List<double>> cpuUtilization = new List<List<double>>();
        List<double> avgExecution = new List<double>();

        // default constructor
        public Data() { }

        public void addToLists(Statistics stats, IList<int> v)
        {
            avgTurnaround.Add(stats.avgTurnaroundTime);
            avgResponse.Add(stats.avgResponseTime);
            avgWait.Add(stats.avgWaitTime);
            speedup.Add(stats.speedup);
            contextSwitch.Add(stats.avgContext);
            avgExecution.Add(stats.avgExecutionTime);
            cpuUtilization.Add(stats.cpuUtilization);
            throughput.Add(stats.throughput);
            quantums.Add(stats.quantums);
            // orderings
            var str = "";
            for (int i = 0; i < v.Count; i++)
                str += algorithms[v[i]] + " ";
            orderings.Add(str);
        }

        // create some sample data
        public Queue<PCB> sample = new Queue<PCB>
        (
            new[]{ 
            new PCB(){name = 'A', arrivalTime = 0, serviceTime = 3, priorityNumber = 1},
            new PCB(){name = 'B', arrivalTime = 2, serviceTime = 6, priorityNumber = 4},
            new PCB(){name = 'C', arrivalTime = 4, serviceTime = 4, priorityNumber = 5},
            new PCB(){name = 'D', arrivalTime = 6, serviceTime = 5, priorityNumber = 2},
            new PCB(){name = 'E', arrivalTime = 8, serviceTime = 2, priorityNumber = 3}
            }
        );

        // more sample data
        public Queue<PCB> sample2 = new Queue<PCB>
        (
            new[] {
                new PCB(){PID = 1, arrivalTime = 0, serviceTime = 50},
                new PCB(){PID = 2, arrivalTime = 20, serviceTime = 20},
                new PCB(){PID = 3, arrivalTime = 40, serviceTime = 100},
                new PCB(){PID = 4, arrivalTime = 60, serviceTime = 60}
            }
        );

        // dictionary to hold algorithm names and IDs that identify their switch number
        public Dictionary<int, String> algorithms = new Dictionary<int, String>()
        {
            {8, "FIRST-COME-FIRST-SERVE"},
            {0, "SHORTEST-PROCESS-NEXT"},
            {1, "SHORTEST-REMAINING-TIME"},
            {2, "HIGHEST-RESPONSE-RATIO-NEXT"},
            {3, "ROUND ROBIN WITH LOW QUANTUM"},
            {4, "ROUND ROBIN WITH HIGH QUANTUM"},
            {5, "PRIORITY"},
            {6, "FEEDBACK WITH QUANTUM = 1"},
            {7, "FEEDBACK WITH QUANTUM = 2^i"}
        };

        // output data string
        public String getStatisticsOutput(double avgResp, double minResp, double maxResp,
            double avgWait, double minWait, double maxWait,
            double avgTurn, double minTurn, double maxTurn,
            double avgExec, double minExec, double maxExec,
            double avgTRTS, List<double> throughput, double speedup, 
            double contextSwitch)
        {
            var str = "";
            for (int i = 0; i < throughput.Count; i++)
                str += "\t" + algorithms[i] + ": " + throughput[i] + "\n";

            return "Average response time: " + avgResp + "\n"
                + "\tMinimum response time: " + minResp + "\n"
                + "\tMaximum response time: " + maxResp + "\n"
                + "Average wait time: " + avgWait + "\n"
                + "\tMinimum wait time: " + minWait + "\n"
                + "\tMaximum wait time: " + maxWait + "\n"
                + "Average turnaround time: " + avgTurn + "\n"
                + "\tMinimum turnaround time: " + minTurn + "\n"
                + "\tMaximum turnaround time: " + maxTurn + "\n"
                + "Average execution time: " + avgExec + "\n"
                + "\tMinimum execution time: " + minExec + "\n"
                + "\tMaximum execution time: " + maxExec + "\n"
                + "Ratio of turnaround time / service time: " + avgTRTS + "\n"
                + "Average throughput across all algorithms: \n"
                + str
                + "Average context switch time across all CPUs: " + contextSwitch + "\n"
                + "Speedup: " + speedup + "\n"
            ;
        }

        // output string that belongs to introduction of an algorithm
        public String introAlgString(String algName, int numProcesses, bool CPUburst)
        {
            return "--BEGIN " + algName +  "\n"
            + "\tNumber of processes to be serviced this round: " + numProcesses + "\n"
            + "\tCurrent burst processing: " + ((CPUburst) ? "CPU" : "I/O");
        }

        // output string that belogns to ending of an algorithm
        public String outroAlgString(String algName, int time, int throughput, int numProcessesLeft, int finishedProcesses, int totalTime)
        {
            return "\tTime spent in this round of " + algName + ": " + time + "\n"
                + "\tNumber of processes finished this round: " + throughput + "\n"
                + "\tAmount of processes left to finish/process: " + numProcessesLeft + "\n"
                + "\tAmount of processes \"done\" (no more bursts) so far: " + finishedProcesses + "\n"
                + "\tTotal time accumulated so far: " + totalTime + "\n"
                + "--END " + algName + "\n";
        }

        // output string specifically for round robin
        public String rrString(String algName, int quantum, int numProcesses)
        {
            return "--BEGIN " + algName + " " + quantum + "\n"
                + "\tNumber of processes to be serviced this round: " + numProcesses;
        }

        public String endSimOutput(Scheduler scheduler)
        {
            var str = "";
            for (int i = 0; i < scheduler.numCPUs; i++)
            {
                str += "Total time spent in CPU #" + (i + 1) + ": " + scheduler.cpus[i].algorithms.timeCounter + "\n";
                str += "\tProcessor Utilization: " + scheduler.cpus[i].utilization + "%\n";
            }
            str += "---------------------------------------------------\n";
            str += "Total time spent across all CPUs: " + scheduler.calcTotalTime();
            return str;
        }

        public String currentRun(IList<int> v)
        {
            //Console.WriteLine("Current run: " + String.Join(",",v));
            var str = "Current Run: \n";
            foreach (var i in v)
                str += "\t" + algorithms[i] + "\n";
            return str;
        }

        //source: http://stackoverflow.com/questions/23041021/how-to-write-some-data-to-excel-file-xlsx
        public void writeStatsToFile(Statistics stats)
        {
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Range oRng;
            object misvalue = System.Reflection.Missing.Value;
            try
            {
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();
                oXL.Visible = true;

                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(""));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;

                //Add table headers going cell by cell.
                oSheet.Cells[1, 1] = "Quantum";
                oSheet.Cells[1, 2] = "Average Turnaround Time";
                oSheet.Cells[1, 3] = "Average Response Time";
                oSheet.Cells[1, 4] = "Average Wait Time";

                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.get_Range("A1", "D1").Font.Bold = true;
                oSheet.get_Range("A1", "D1").VerticalAlignment =
                    Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                // Create an array to multiple values at once.
                int[,] saNames = new int[5, 2];

                //Fill A2:B6 with an array of values (First and Last Names).
                oSheet.get_Range("A2", "A6").Value2 = saNames;
                oRng = oSheet.get_Range("A2", "A6");
                oRng.NumberFormat = "0.00";

                //AutoFit columns A:D.
                oRng = oSheet.get_Range("A1", "D1");
                oRng.EntireColumn.AutoFit();

                oXL.Visible = false;
                oXL.UserControl = false;
                //change save as to save after creating
                oWB.SaveAs("C:\\Users\\tglasser15\\Documents\\Three_Averages.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
        public void writeRandomProcessesToFile()
        {
            Random rnd = new Random();

            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            object misvalue = System.Reflection.Missing.Value;
            try
            {
                Console.WriteLine("Creating processes...");
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();
                oXL.Visible = true;

                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(""));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;

                int idCount = 0;
                int priorityNum = 0;
                int arrivalTime = 0;
                int numBursts = 0;

                for(int p = 1; p <= 500; p++)
                {
                    idCount += 1;
                    priorityNum = rnd.Next(1, 1000);
                    arrivalTime = rnd.Next(2000);
                    numBursts = rnd.Next(1, 10);

                    oSheet.Cells[p,1] = idCount;
                    oSheet.Cells[p,2] = priorityNum;
                    oSheet.Cells[p,3] = arrivalTime;

                    for (int i = 0; i < numBursts; i++)
                    {
                        oSheet.Cells[p, i+4] = rnd.Next(1, 1000);
                    }

                }

                oXL.Visible = false;
                oXL.UserControl = false;
                //change save as to save after creating
                string path = "C:\\Users\\hgamiel15\\Documents\\";

                oWB.SaveAs((path + "processes.xls"), Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
                Console.WriteLine("Finished creating processes.\n\tSave to xls complete.");

                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Open(path+"processes.xls");
                wb.SaveAs((path+"processes.csv"), Microsoft.Office.Interop.Excel.XlFileFormat.xlCSVWindows);
                wb.Close(false);
                app.Quit();
                Console.WriteLine("\tSave to csv complete.");

                string filename = "processes.txt";
                Directory.SetCurrentDirectory(@"..\..\..\"); // default is \bin\Debug -> this sets the current directory up a few folders
                string filepath = Path.Combine(Environment.CurrentDirectory, filename);

                try
                {
                    System.IO.File.WriteAllText(filepath, System.IO.File.ReadAllText(path + "processes.csv").Replace(",", "\t"));
                    Console.WriteLine("\tSave to .txt complete.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error converting from csv to txt: " + ex.ToString());
                }

                var filepath1 = Path.Combine(Environment.CurrentDirectory, "processes.dat");
                var filepath2 = Path.Combine(Environment.CurrentDirectory, "processes.txt");

                try
                {
                    System.IO.File.WriteAllText(filepath1, System.IO.File.ReadAllText(filepath2));
                    Console.WriteLine("\tSave to .dat complete.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error converting from txt to .dat: " + ex.ToString());
                }

            }
            catch (Exception ex) { }
        }

    }
}
