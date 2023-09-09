﻿using OwnerGPT.DB.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Models.Entities.Agents;
using OwnerGPT.Services.Abstract;

namespace OwnerGPT.Services
{
    public class AgentsService : RDBMSServiceBase<Agent>
    {
        public AgentsService(IRDBMSUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
