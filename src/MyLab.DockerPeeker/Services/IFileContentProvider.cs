﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyLab.DockerPeeker.Services
{
    public interface IFileContentProvider
    {
        Task<string> ReadCpuAcctStat(string containerLongId);
        Task<string> ReadMemStat(string containerLongId);
        Task<string> ReadBlkIoServiceBytesStat(string containerLongId);
    }

    class RealFileContentProvider : IFileContentProvider
    {
        public Task<string> ReadCpuAcctStat(string containerLongId)
        {
            return File.ReadAllTextAsync($"/sys/fs/cgroup/cpuacct/docker/{containerLongId}/cpuacct.stat");
        }

        public Task<string> ReadMemStat(string containerLongId)
        {
            return File.ReadAllTextAsync($"/sys/fs/cgroup/memory/docker/{containerLongId}/memory.stat");
        }

        public Task<string> ReadBlkIoServiceBytesStat(string containerLongId)
        {
            return File.ReadAllTextAsync($"/sys/fs/cgroup/blkio/docker/{containerLongId}/blkio.throttle.io_service_bytes");
        }
    }
}
