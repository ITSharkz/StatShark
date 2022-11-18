﻿using StatShark.Models;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace StatShark.Services
{
    public class DiagnosticService : IDiagnosticService
    {
        public string _HostName = "default";
        public DiagnosticService()
        {
            CheckCompatibility();
            //
        }

        public DiagnosticService(string HostName)
        {
            CheckCompatibility();
            this._HostName = HostName;
        }
        public StatsResponse Get()
        {
            StatsResponse response = new StatsResponse();

            response.HostId = this._HostName;
            response.DateTime = DateTime.UtcNow.ToUniversalTime();
            try
            {

                response.Ram = GetRamMetrics();
            }
            catch (Exception e)
            {
                Console.WriteLine("StatShark: " + e);
            }
            try
            {

                response.Cpu = GetCpuMetrics();
            }
            catch (Exception e)
            {
                Console.WriteLine("StatShark: " + e);
            }
            try
            {

                response.Disk = GetDiskMetrics();
            }
            catch (Exception e)
            {
                Console.WriteLine("StatShark: " + e);
            }

            return response;
        }
        private RawMetrics GetDiskMetrics()
        {
            var driveInfo = new DriveInfo("/");
            var metrics = new RawMetrics();
            metrics.Unit = "mb";
            metrics.Total = driveInfo.TotalSize / 1024 / 1024;
            metrics.Free = driveInfo.TotalFreeSpace / 1024 / 1024;
            metrics.Used = metrics.Total - metrics.Free;
            return metrics;
        }
        private RawMetrics GetCpuMetrics()
        {
            string output = "";
            var info = new ProcessStartInfo("uptime");
            info.RedirectStandardOutput = true;
            var process = Process.Start(info);
            process.WaitForExit();
            output = process.StandardOutput.ReadToEnd();
            var cpu = output.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var metrics = new RawMetrics();
            double percentage = double.Parse(cpu[cpu.Length - 3].Replace(",", "")) * 100;
            metrics.Total = 100;
            metrics.Free = 100 - percentage;
            metrics.Used = percentage;
            metrics.Unit = "%";
            return metrics;
        }

        private RawMetrics GetRamMetrics()
        {
            var output = "";
            var info = new ProcessStartInfo("free -m");
            info.FileName = "/bin/bash";
            info.Arguments = "-c \"free -m\"";
            info.RedirectStandardOutput = true;
            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();

            }
            var lines = output.Split("\n");
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var metrics = new RawMetrics();
            metrics.Total = double.Parse(memory[1]);
            metrics.Used = double.Parse(memory[2]);
            metrics.Free = double.Parse(memory[3]);
            metrics.Unit = "mb";
            return metrics;
        }

        private void CheckCompatibility()
        {
            if (!System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new InvalidOperationException("It works only on Linux.");
            }

        }
    }
}
