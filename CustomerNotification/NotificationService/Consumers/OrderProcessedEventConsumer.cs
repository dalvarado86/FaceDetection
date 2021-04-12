﻿using EmailService;
using MassTransit;
using Messaging.InterfacesConstants.Events;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace NotificationService.Consumers
{
    public class OrderProcessedEventConsumer : IConsumer<IOrderProcessedEvent>
    {
        private readonly IEmailSender _emailSender;

        public OrderProcessedEventConsumer(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task Consume(ConsumeContext<IOrderProcessedEvent> context)
        {
            var rootFolder = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            var result = context.Message;
            var facesData = result.Faces;

            if(facesData.Count <1)
            {
                await Console.Out.WriteLineAsync($"No faces Detected");
            }
            else
            {
                int j = 0;
                foreach(var face in facesData)
                {
                    var ms = new MemoryStream(face);
                    var image = Image.FromStream(ms);
                    image.Save(rootFolder + "/Images/face" + j + ".jpg", ImageFormat.Jpeg);
                    j++;
                }
            }

            string[] mailAddress = { result.UserEmail };
            await _emailSender.SendEmailAsync(new Message(mailAddress, "your order" + result.OrderId, "From FacesAndFaces", facesData));
            
            await context.Publish<IOrderDispatchedEvent>(new
            {
                context.Message.OrderId,
                DispatchDateTime = DateTime.UtcNow
            });
        }
    }
}
