using UniGate.ServiceBus;
using UniGateAPI.Interfaces;

namespace UniGateAPI.Services;

public class ApplicantService(IMessageBus messageBus) : IApplicantService
{
}