using UniGate.ServiceBus;
using UniGateAPI.DTOs;
using UniGateAPI.Interfaces;

namespace UniGateAPI.Services;

public class ApplicantService(IMessageBus messageBus) : IApplicantService
{
    public Task CreateApplicant(ApplicantDto applicant)
    {
        // Save to DB (omitted for brevity)

        // Publish an event to RabbitMQ
        messageBus.Publish(applicant, "applicant_queue");

        return Task.CompletedTask;
    }
}