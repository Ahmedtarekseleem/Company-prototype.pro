using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Twilio.Rest.Api.V2010.Account;

namespace Company.pro.PL.Helper
{
    public interface ITwilioService
    {
        public MessageResource SendSms(Sms sms);
    }
}
