﻿using System.ComponentModel.DataAnnotations.Schema;
using OwnerGPT.Models.Entities.Interfaces;
using Pgvector;

namespace OwnerGPT.Models.Entities
{
    public class VectorEmbedding : IEntity
    {
        public int Id { get; set; }

        public Vector Embedding { get; set; }

        public string Context { get; set; }
    }
}