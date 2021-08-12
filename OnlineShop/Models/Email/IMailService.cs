﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Email
{
   public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
