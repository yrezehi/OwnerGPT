﻿using OwnerGPT.Models.Entities.Interfaces;

namespace OwnerGPT.Models.Entities
{
    public class Notification : IEntity
    {
        public int Id { get; set; }

        public List<string> SearchableProperties()
        {
            throw new NotImplementedException();
        }
    }
}
