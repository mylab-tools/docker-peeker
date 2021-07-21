﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MyLab.DockerPeeker.Services;

namespace MyLab.DockerPeeker.Tools
{
    public class MemStatCmProvider : IContainerMetricsProvider
    {
        private readonly IFileContentProvider _fileContentProvider;

        readonly ContainerMetricType _memSwapMetricType = new ContainerMetricType
        {
            Name = "container_mem_swap_bytes",
            Type = "gauge",
            Description = "The amount of swap currently used by the processes in this cgroup"

        };

        readonly ContainerMetricType _memCacheMetricType = new ContainerMetricType{
            Name = "container_mem_cache_bytes",
            Type = "gauge",
            Description = "The amount of memory used by the processes of this control group that can be associated precisely with a block on a block device"
        };

        readonly ContainerMetricType _memRssMetricType = new ContainerMetricType{
            Name = "container_mem_rss_bytes",
            Type = "gauge",
            Description = "The amount of memory that doesn’t correspond to anything on disk: stacks, heaps, and anonymous memory maps"
        };

        readonly ContainerMetricType _memLimitMetricType = new ContainerMetricType{
            Name = "container_mem_limit_bytes",
            Type = "gauge",
            Description = "Indicates the maximum amount of physical memory that can be used by the processes of this control group"
        };

        readonly ContainerMetricType _memSwLimitMetricType = new ContainerMetricType{
            Name = "container_memsw_limit_bytes",
            Type = "gauge",
            Description = "Indicates the maximum amount of RAM+swap that can be used by the processes of this control group"
        };

        public MemStatCmProvider(IFileContentProvider fileContentProvider)
        {
            _fileContentProvider = fileContentProvider;
        }

        public async Task<IEnumerable<ContainerMetric>> ProvideAsync(string containerLongId, string pid)
        {
            var statContent = await _fileContentProvider.ReadMemStat(containerLongId);
            var parser = StatParser.Create(statContent);

            return new[]
            {
                new ContainerMetric(parser.ExtractKey("swap", "Mem swap"), _memSwapMetricType),
                new ContainerMetric(parser.ExtractKey("cache", "Mem cache"), _memCacheMetricType),
                new ContainerMetric(parser.ExtractKey("rss", "Mem rss"), _memRssMetricType),
                new ContainerMetric(parser.ExtractKey("hierarchical_memory_limit", "Mem limit"), _memLimitMetricType),
                new ContainerMetric(parser.ExtractKey("hierarchical_memsw_limit", "Mem+Swap limit"), _memSwLimitMetricType),
            };

        }
    }
}
