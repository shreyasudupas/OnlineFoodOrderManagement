﻿using MediatR;
using MicroService.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.MicroService.Features.UserFeature.Queries
{
    public class GetUserRequestModel:IRequest<Users>
   {
        public string Username { get; set; }
    }
}