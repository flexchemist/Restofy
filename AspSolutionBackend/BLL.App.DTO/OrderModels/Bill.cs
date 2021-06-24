﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;
using BLL.App.DTO;


namespace BLL.App.DTO.OrderModels

{
    public sealed class Bill : DomainEntityId

    {
        public Guid OrderId { get; set; }

        [Column("total_cost_without_VAT", TypeName = "decimal(10, 2)")]
        public decimal TotalCostWithoutVat { get; set; }

        [Column("total_cost_with_VAT", TypeName = "decimal(5, 2)")]
        public decimal TotalCostWithVat { get; set; }
        public Order? Order { get; set; }
        
        public ICollection<BillLine>? BillLines { get; set; }
    }
}