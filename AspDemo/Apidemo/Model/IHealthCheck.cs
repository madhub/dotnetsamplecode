using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apidemo.Model
{
    public enum CheckStatus
    {
        Unknown,
        Unhealthy,
        Healthy,
        Warning
    }
    public class HealthCheckResult : IHealthCheckResult
    {
        private static readonly IReadOnlyDictionary<string, object> _emptyData = new Dictionary<string, object>();

        public CheckStatus CheckStatus { get; }
        public IReadOnlyDictionary<string, object> Data { get; }
        public string Description { get; }

        private HealthCheckResult(CheckStatus checkStatus, string description, IReadOnlyDictionary<string, object> data)
        {
            CheckStatus = checkStatus;
            Description = description;
            Data = data ?? _emptyData;
        }

        public static HealthCheckResult Unhealthy(string description)
            => new HealthCheckResult(CheckStatus.Unhealthy, description, null);

        public static HealthCheckResult Unhealthy(string description, IReadOnlyDictionary<string, object> data)
            => new HealthCheckResult(CheckStatus.Unhealthy, description, data);

        public static HealthCheckResult Healthy(string description)
            => new HealthCheckResult(CheckStatus.Healthy, description, null);

        public static HealthCheckResult Healthy(string description, IReadOnlyDictionary<string, object> data)
            => new HealthCheckResult(CheckStatus.Healthy, description, data);

        public static HealthCheckResult Warning(string description)
            => new HealthCheckResult(CheckStatus.Warning, description, null);

        public static HealthCheckResult Warning(string description, IReadOnlyDictionary<string, object> data)
            => new HealthCheckResult(CheckStatus.Warning, description, data);

        public static HealthCheckResult Unknown(string description)
            => new HealthCheckResult(CheckStatus.Unknown, description, null);

        public static HealthCheckResult Unknown(string description, IReadOnlyDictionary<string, object> data)
            => new HealthCheckResult(CheckStatus.Unknown, description, data);

        public static HealthCheckResult FromStatus(CheckStatus status, string description)
            => new HealthCheckResult(status, description, null);

        public static HealthCheckResult FromStatus(CheckStatus status, string description, IReadOnlyDictionary<string, object> data)
            => new HealthCheckResult(status, description, data);
    }
    public interface IHealthCheckResult
    {
        CheckStatus CheckStatus { get; }
        string Description { get; }
        IReadOnlyDictionary<string, object> Data { get; }
    }

    public interface IHealthCheck
    {
        Task<HealthCheckResult> Check();
    }
    public class SampleHealthCheck : IHealthCheck
    {
        private readonly IConfiguration configuration;

        public SampleHealthCheck(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public Task<HealthCheckResult> Check()
        {
                return Task.FromResult(
                    HealthCheckResult.Healthy("The check indicates a healthy result."));
        }
    }

    public class SampleHealthCheck2 : IHealthCheck
    {
        private readonly IConfiguration configuration;

        public SampleHealthCheck2(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public Task<HealthCheckResult> Check()
        {
            return Task.FromResult(
                HealthCheckResult.Healthy("The check indicates a healthy result."));
        }
    }
}
