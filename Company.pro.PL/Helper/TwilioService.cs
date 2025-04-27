using Company.pro.PL.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Company.pro.PL.Helper
{
    public class TwilioService(IOptions<TwilioSettings> _options) : ITwilioService
    {
        public MessageResource SendSms(Sms sms)
        {
            // Initialize Connection 
            TwilioClient.Init(_options.Value.AccountSID, _options.Value.AuthToken);
            // Build Messege
            var messege =  MessageResource.Create(
                body: sms.Body,
                to: new PhoneNumber(sms.To),
                from: new Twilio.Types.PhoneNumber ( _options.Value.PhonNumber )
            );
            // Return
            return messege;
        }
        
    }
}
