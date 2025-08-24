using Firebase.Database;
using FirebaseAdmin.Messaging;
using Google.Cloud.Firestore;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;
using System.Data;
using static FirebaseA;

namespace WebApplication3.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class MiddleMan : ControllerBase
    {
        FirebaseA Firebase = FirebaseA.FirebaseInstance();
        FirebaseClient firebase = new FirebaseClient("https://stwbdkw-default-rtdb.asia-southeast1.firebasedatabase.app/");


        [HttpPost]
        [Route("/Notification")]
        public async Task<IActionResult> ChangeState([FromBody]bool state)
        {
            //get all token if ykwim
            var tokens = await firebase
                .Child("fcmTokens")
                .OnceAsync<object>();
            var AllTokens = tokens.Select(t => t.Key).ToList();

            if (state == true)
            {
                DateTime Time = TimeZoneInfo.ConvertTimeFromUtc(
                    DateTime.UtcNow,
                    TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
                    );

                var message = new MulticastMessage()
                {
                    Tokens = AllTokens,
                    Notification = new Notification
                    {
                        Title = "Intrution Alert",
                        Body = "Please ignore this message if you intended to trigger the alarm"
                    },
                    Data = new Dictionary<string, string>()
                    {
                        { "time", $"{Time}" },
                    },
                };

                var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
                if (response.FailureCount > 0)
                {
                    var failedTokens = new List<string>();
                    for (var i = 0; i < response.Responses.Count; i++)
                    {
                        if (!response.Responses[i].IsSuccess)
                        {
                            // The order of responses corresponds to the order of the registration tokens.
                            failedTokens.Add(AllTokens[i]);
                        }
                    }

                    Console.WriteLine($"List of tokens that caused failures: {failedTokens}");
                }
                return Ok("MessageSent");
            }
            return BadRequest();
        }
    }
}
