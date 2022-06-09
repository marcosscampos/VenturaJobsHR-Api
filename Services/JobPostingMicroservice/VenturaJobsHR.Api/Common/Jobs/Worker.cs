using System.Timers;
using Coravel.Invocable;
using Microsoft.Extensions.Options;
using VenturaJobsHR.Common.Extensions;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Repositories;
using VenturaJobsHR.Domain.Aggregates.UserAgg.Repositories;

namespace VenturaJobsHR.Api.Common.Jobs;

public class Worker : IInvocable
{
    private readonly IJobRepository _jobRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOptions<Email> _configurations;
    private readonly ILogger _logger;

    public Worker(
        IJobRepository jobRepository,
        IUserRepository userRepository,
        IOptions<Email> configurations,
        ILoggerFactory logger)
    {
        _jobRepository = jobRepository;
        _userRepository = userRepository;
        _configurations = configurations;
        _logger = logger.CreateLogger<Worker>();
    }

    public async Task Invoke()
    {
        _logger.LogInformation(
            $"Início da verificação de vagas perto de expirar... Data de início: {DateTime.Now:dd/MM/YYYY hh:mm:ss}");
        var users = await _userRepository.GetAllAsync();
        var userList = users.ToList().Where(x => x.Active);

        foreach (var user in userList)
        {
            var jobs = await _jobRepository.GetJobsByFirebaseToken(user.FirebaseId);

            foreach (var job in jobs)
            {
                if (job.DeadLine < new DateTimeWithZone(DateTime.Now).LocalTime.AddDays(-1))
                {
                    var body = await _userRepository.ReturnTemplateEmailAsync();

                    body = body.Replace("{{JOB_NAME}}", job.Name);

                    EmailExtensions.SendEmail(_configurations.Value, new List<string> { user.Email }, body,
                        $"A vaga {job.Name} irá expirar!");
                }
                else if (job.DeadLine < new DateTimeWithZone(DateTime.Now).LocalTime && job.Status == JobStatusEnum.Published)
                {
                    _logger.LogInformation($"Vaga ID #{job.Id} expirada. Atualizando o status...");
                    job.Status = JobStatusEnum.Expired;
                    await _jobRepository.UpdateAsync(job);
                }
            }
        }

        _logger.LogInformation($"Verificação finalizada! Data fim: {DateTime.Now:dd/MM/YYYY hh:mm:ss}");

        await Task.CompletedTask;
    }
}